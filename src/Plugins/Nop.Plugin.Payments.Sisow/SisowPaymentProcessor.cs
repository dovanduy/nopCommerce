using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Core.Plugins;
using Nop.Plugin.Payments.Sisow.Controllers;
using Nop.Plugin.Payments.Sisow.Models;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Orders;
using Nop.Services.Payments;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Web;
using System.Web.Routing;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Nop.Plugin.Payments.Sisow
{
    /// <summary>
    /// sisow payment processor
    /// </summary>
    public class SisowPaymentProcessor : BasePlugin, IPaymentMethod
    {
        #region Fields

        private readonly SisowPaymentSettings _sisowPaymentSettings;
        private readonly ISettingService _settingService;
        private readonly IOrderService _orderService;
        private readonly IWebHelper _webHelper;
        private readonly HttpContextBase _httpContext;
        private readonly IGenericAttributeService _genericAttributeService;

        #endregion

        #region Ctor

        public SisowPaymentProcessor(SisowPaymentSettings sisowPaymentSettings,
                                            ISettingService settingService,
                                            IGenericAttributeService genericAttributeService,
                                            HttpContextBase httpContext,
                                            IOrderService orderService,
                                            IWebHelper webHelper)
        {
            _sisowPaymentSettings = sisowPaymentSettings;
            _settingService = settingService;
            _genericAttributeService = genericAttributeService;
            _orderService = orderService;
            _webHelper = webHelper;
            _httpContext = httpContext;
        }

        #endregion

        #region Utilities

        //private class ViewDataContainer : IViewDataContainer
        //{
        //    public ViewDataContainer(ViewDataDictionary viewData)
        //    {
        //        ViewData = viewData;
        //    }

        //    public ViewDataDictionary ViewData { get; set; }
        //}


        #endregion

        #region Methods

        /// <summary>
        /// Process a payment
        /// </summary>
        /// <param name="processPaymentRequest">Payment info required for an order processing</param>
        /// <returns>Process payment result</returns>
        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            return new ProcessPaymentResult() { NewPaymentStatus = PaymentStatus.Pending };
        }

        /// <summary>
        /// Post process payment (used by payment gateways that require redirecting to a third-party URL)
        /// </summary>
        /// <param name="postProcessPaymentRequest">Payment info required for an order processing</param>
        public void PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            var sisowUrl = _sisowPaymentSettings.SisowUrl;
            var entranceCode = Guid.NewGuid().ToString().Replace("-", "");
            var purchaseId = postProcessPaymentRequest.Order.Id.ToString();
            var description = postProcessPaymentRequest.Order.Id + " - " + postProcessPaymentRequest.Order.BillingAddress.LastName;

            if (description.Length > 32)
                description = description.Substring(0, 32);

            if (entranceCode.Length > 40)
                entranceCode = entranceCode.Substring(0, 40);

            if (purchaseId.Length > 16)
                purchaseId = purchaseId.Substring(0, 16);

            var info = new PaymentInfoModel()
            {
                MerchantId = _sisowPaymentSettings.MerchantId,
                MerchantKey = _sisowPaymentSettings.MerchantKey,
                Payment = "", // betalingsmethode - IDEAL is leeg
                PurchaseId = purchaseId, //Order Id
                Amount = Convert.ToInt32(postProcessPaymentRequest.Order.OrderTotal * 100),
                EntranceCode = entranceCode, // Guid
                Description = description, // OrderId en Achternaam
                ReturnUrl = _sisowPaymentSettings.NormalReturnUrl,
                NotifyUrl = _sisowPaymentSettings.NotifyUrl,
                TestMode = _sisowPaymentSettings.TestMode,
            };

            postProcessPaymentRequest.Order.AuthorizationTransactionId = entranceCode;
            //Guid // used to store the transactionID.
            //When is is a second attemp from My Account View. The resultcode has to be set to null.
            postProcessPaymentRequest.Order.AuthorizationTransactionResult = null;
            //Save the order transactionID so we can get the order back later in the sisowReturn method
            _orderService.UpdateOrder(postProcessPaymentRequest.Order);

            using (var client = new HttpClient())
            {
                var sha1 = SisowHelper.GetSHA1(info.PurchaseId + info.EntranceCode + info.Amount + info.MerchantId + info.MerchantKey);
                client.BaseAddress = new Uri(sisowUrl);
                var content = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>("merchantid", info.MerchantId)
                    ,   new KeyValuePair<string, string>("payment", info.Payment)
                    //,   new KeyValuePair<string, string>("issuerid", "99")
                    ,   new KeyValuePair<string, string>("purchaseid", info.PurchaseId)
                    ,   new KeyValuePair<string, string>("amount", info.Amount.ToString(CultureInfo.InvariantCulture))
                    ,   new KeyValuePair<string, string>("description", info.Description)
                    ,   new KeyValuePair<string, string>("entrancecode", info.EntranceCode)
                    ,   new KeyValuePair<string, string>("returnurl", info.ReturnUrl)
                    ,   new KeyValuePair<string, string>("notifyurl", info.NotifyUrl)
                    ,   new KeyValuePair<string, string>("sha1", sha1)
                    ,   new KeyValuePair<string, string>("testmode", (info.TestMode ? "true" : "false"))
                 });
                var response = client.PostAsync("TransactionRequest", content).Result;

                var resultContent = response.Content.ReadAsStringAsync().Result;
                //Console.WriteLine(resultContent);
                var storeUrl = _webHelper.GetStoreLocation(false);
                if (response.IsSuccessStatusCode)
                {
                    if (resultContent.Contains("<?xml version"))
                    {

                        if (resultContent.Contains("transactionrequest"))
                        {
                            var incomingXml = XElement.Parse(resultContent);
                            var reader = new StringReader(incomingXml.ToString());

                            var deserializer2 = new XmlSerializer(typeof(transactionrequest));
                            var xmlData2 = (transactionrequest)deserializer2.Deserialize(reader);

                            var calcSHA1 = SisowHelper.GetSHA1(xmlData2.transaction.trxid + xmlData2.transaction.issuerurl + _sisowPaymentSettings.MerchantId + _sisowPaymentSettings.MerchantKey);
                            if (xmlData2.signature.sha1 == calcSHA1)
                            {
                                _httpContext.Response.Redirect(HttpUtility.UrlDecode(xmlData2.transaction.issuerurl));
                            }
                            else
                            {
                                _httpContext.Response.Redirect(string.Concat(storeUrl, string.Format("PaymentSisow/SisowErrorReturn?orderId={0}&errorCode={1}", postProcessPaymentRequest.Order.Id, "TA9997")));
                            }
                        }
                        else
                        {
                            var paymentFailedInfo = new PaymentFailedModel
                            {
                                OrderId = postProcessPaymentRequest.Order.Id,
                                Amount = postProcessPaymentRequest.Order.OrderTotal
                            };

                            if (resultContent.Contains("errorresponse"))
                            {
                                var incomingErrorXml = XElement.Parse(resultContent);
                                var readerError = new StringReader(incomingErrorXml.ToString());

                                var deserializerError = new XmlSerializer(typeof(errorresponse));
                                var xmlDataError = (errorresponse)deserializerError.Deserialize(readerError);
                                paymentFailedInfo.ErrorCode = xmlDataError.error.errorcode;
                                paymentFailedInfo.ErrorDescription = xmlDataError.error.errormessage;
                            }
                            else
                            {
                                paymentFailedInfo.ErrorCode = "TA9999";
                                paymentFailedInfo.ErrorDescription = "Onbekende reactie van de Sisow server.";
                            }

                            _httpContext.Response.Redirect(string.Concat(storeUrl,
                                $"PaymentSisow/SisowErrorReturn?orderId={paymentFailedInfo.OrderId}&errorCode={paymentFailedInfo.ErrorCode}"));
                        }
                    }
                }

                else
                {
                    _httpContext.Response.Redirect(string.Concat(storeUrl,
                        $"PaymentSisow/SisowErrorReturn?orderId={postProcessPaymentRequest.Order.Id}&errorCode={"TA9998"}"));
                }

            }
        }

        /// <summary>
        /// Captures payment
        /// </summary>
        /// <param name="capturePaymentRequest">Capture payment request</param>
        /// <returns>Capture payment result</returns>
        public CapturePaymentResult Capture(CapturePaymentRequest capturePaymentRequest)
        {
            var result = new CapturePaymentResult();
            result.AddError("Capture method not supported");
            return result;

        }

        /// <summary>
        /// Refunds a payment
        /// </summary>
        /// <param name="refundPaymentRequest">Request</param>
        /// <returns>Result</returns>
        public RefundPaymentResult Refund(RefundPaymentRequest refundPaymentRequest)
        {
            var result = new RefundPaymentResult();

            var sisowUrl = _sisowPaymentSettings.SisowUrl;
            var trxId = refundPaymentRequest.Order.AuthorizationTransactionCode;

            var refundInfo = new RefundInfoModel
            {
                MerchantId = _sisowPaymentSettings.MerchantId,
                AuthorizationTransactionCode = trxId,
                RefundAmount = refundPaymentRequest.AmountToRefund,
                ReturnRefundUrl = _sisowPaymentSettings.RefundReturnUrl
            };

            using (var client = new HttpClient())
            {
                var sha1 =
                    SisowHelper.GetSHA1(refundInfo.AuthorizationTransactionCode + refundInfo.MerchantId +
                                        _sisowPaymentSettings.MerchantKey);
                client.BaseAddress = new Uri(sisowUrl);
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("merchantid", refundInfo.MerchantId)
                    , new KeyValuePair<string, string>("trxid", refundInfo.AuthorizationTransactionCode)
                    ,
                    new KeyValuePair<string, string>("amount",
                        refundPaymentRequest.AmountToRefund.ToString(CultureInfo.InvariantCulture))
                    , new KeyValuePair<string, string>("sha1", sha1)
                    , new KeyValuePair<string, string>("refundurl", refundInfo.ReturnRefundUrl)
                });
                var response = client.PostAsync("RefundRequest", content).Result;
                var resultContent = response.Content.ReadAsStringAsync().Result;
                var storeUrl = _webHelper.GetStoreLocation(false);
                if (response.IsSuccessStatusCode)
                {
                    if (resultContent.Contains("<?xml version"))
                    {

                        if (resultContent.Contains("refundresponse"))
                        {
                            var incomingXml = XElement.Parse(resultContent);
                            var reader = new StringReader(incomingXml.ToString());

                            var deserializer2 = new XmlSerializer(typeof(refundresponse));
                            var xmlData2 = (refundresponse)deserializer2.Deserialize(reader);

                            var calcSHA1 =
                                SisowHelper.GetSHA1(xmlData2.refund.refundid +
                                                    _sisowPaymentSettings.MerchantId + _sisowPaymentSettings.MerchantKey);
                            if (xmlData2.signature.sha1 == calcSHA1)
                            {
                                result.NewPaymentStatus = (refundPaymentRequest.IsPartialRefund && refundPaymentRequest.Order.RefundedAmount
                                    + refundPaymentRequest.AmountToRefund < refundPaymentRequest.Order.OrderTotal) ? PaymentStatus.PartiallyRefunded : PaymentStatus.Refunded;

                                //set refund transaction id for preventing refund twice
                                _genericAttributeService.SaveAttribute(refundPaymentRequest.Order, "RefundTransactionId", xmlData2.refund.refundid);
                            }
                            else
                            {
                                result.AddError("SHA1 value not valid.");
                            }
                        }
                        else
                        {
                            result.AddError("Invalid response.");
                        }
                    }
                    else
                    {
                        result.AddError("Response is not a XML.");
                    }
                }
                else
                {
                    result.AddError("Response not successfull. StatusCode: " + response.StatusCode);
                }
            }

            return result;
        }

        /// <summary>
        /// Voids a payment
        /// </summary>
        /// <param name="voidPaymentRequest">Request</param>
        /// <returns>Result</returns>
        public VoidPaymentResult Void(VoidPaymentRequest voidPaymentRequest)
        {
            var result = new VoidPaymentResult();
            result.AddError("Void method not supported");
            return result;
        }

        /// <summary>
        /// Process recurring payment
        /// </summary>
        /// <param name="processPaymentRequest">Payment info required for an order processing</param>
        /// <returns>Process payment result</returns>
        public ProcessPaymentResult ProcessRecurringPayment(ProcessPaymentRequest processPaymentRequest)
        {
            var result = new ProcessPaymentResult();
            result.AddError("Recurring payment not supported");
            return result;
        }

        /// <summary>
        /// Cancels a recurring payment
        /// </summary>
        /// <param name="cancelPaymentRequest">Request</param>
        /// <returns>Result</returns>
        public CancelRecurringPaymentResult CancelRecurringPayment(CancelRecurringPaymentRequest cancelPaymentRequest)
        {
            var result = new CancelRecurringPaymentResult();
            result.AddError("Recurring payment not supported");
            return result;
        }

        /// <summary>
        /// Gets a Amount indicating whether customers can complete a payment after order is placed but not completed (for redirection payment methods)
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>Result</returns>
        public bool CanRePostProcessPayment(Order order)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            //payment status should be Pending
            if (order.PaymentStatus != PaymentStatus.Pending)
                return false;

            //let's ensure that at least 1 minute passed after order is placed
            if ((DateTime.UtcNow - order.CreatedOnUtc).TotalMinutes < 1)
                return false;

            return true;
        }

        /// <summary>
        /// Gets a route for provider configuration
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "Paymentsisow";
            routeValues = new RouteValueDictionary() { { "Namespaces", "Nop.Plugin.Payments.sisow.Controllers" }, { "area", null } };
        }

        /// <summary>
        /// Gets a route for payment info
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetPaymentInfoRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "PaymentInfo";
            controllerName = "Paymentsisow";
            routeValues = new RouteValueDictionary() { { "Namespaces", "Nop.Plugin.Payments.sisow.Controllers" }, { "area", null } };
        }

        public Type GetControllerType()
        {
            return typeof(PaymentSisowController);
        }

        public override void Install()
        {
            var storeUrl = _webHelper.GetStoreLocation(false);

            var settings = new SisowPaymentSettings()
            {
                MerchantId = "",
                MerchantKey = "",
                NormalReturnUrl = storeUrl + "Paymentsisow/sisowReturn",
                NotifyUrl = storeUrl + "Paymentsisow/sisowReport",
                SisowUrl = "https://www.sisow.nl/Sisow/iDeal/RestHandler.ashx/",
                MinimumAmount = 0.45m,
                PaymentMeanBrandList = "",
                TestMode = true
            };
            _settingService.SaveSetting(settings);

            base.Install();
        }

        public bool HidePaymentMethod(IList<ShoppingCartItem> cart)
        {
            //you can put any logic here
            //for example, hide this payment method if all products in the cart are downloadable
            //or hide this payment method if current customer is from certain country
            return false;
        }

        public decimal GetAdditionalHandlingFee(IList<ShoppingCartItem> cart)
        {
            return _sisowPaymentSettings.AdditionalFee;
        }

        //public override void Uninstall()
        //{
        //    base.Uninstall();
        //}
        /* TODO
         *        public override void Install()
        {
            //locales
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.GoogleCheckout.Fields.UseSandbox", "Use Sandbox");
            
            base.Install();
        }
        
        
         */
        #endregion

        #region Properies

        /// <summary>
        /// Gets a Amount indicating whether capture is supported
        /// </summary>
        public bool SupportCapture
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a Amount indicating whether partial refund is supported
        /// </summary>
        public bool SupportPartiallyRefund
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets a Amount indicating whether refund is supported
        /// </summary>
        public bool SupportRefund
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets a Amount indicating whether void is supported
        /// </summary>
        public bool SupportVoid
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a recurring payment type of payment method
        /// </summary>
        public RecurringPaymentType RecurringPaymentType
        {
            get
            {
                return RecurringPaymentType.NotSupported;
            }
        }

        /// <summary>
        /// Gets a payment method type
        /// </summary>
        public PaymentMethodType PaymentMethodType
        {
            get
            {
                return PaymentMethodType.Redirection;
            }
        }

        /// <summary>
        /// Gets a value indicating whether we should display a payment information page for this plugin
        /// </summary>
        public bool SkipPaymentInfo
        {
            get
            {
                return false;
            }
        }

        #endregion

    }
}
