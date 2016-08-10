using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Plugin.Payments.Sisow.Models;
using Nop.Plugin.Payments.Sisow.Validators;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
// ReSharper disable Mvc.ViewNotResolved

namespace Nop.Plugin.Payments.Sisow.Controllers
{
    public class PaymentSisowController : BasePaymentController
    {
        private readonly IWorkContext _workContext;
        private readonly ILocalizationService _localizationService;
        private readonly ISettingService _settingService;
        private readonly IPaymentService _paymentService;
        private readonly PaymentSettings _paymentSettings;
        private readonly IStoreService _storeService;
        private readonly SisowPaymentSettings _sisowPaymentSettings;

        private readonly IOrderProcessingService _orderProcessingService;
        private readonly IOrderService _orderService;
        private readonly ILogger _logger;


        public PaymentSisowController(IWorkContext workContext,
                                            IStoreService storeService,
                                            ILocalizationService localizationService,
                                            ISettingService settingService,
                                            IPaymentService paymentService,
                                            PaymentSettings paymentSettings,
                                            SisowPaymentSettings sisowPaymentSettings,
                                            IOrderProcessingService orderProcessingService,
                                            IOrderService orderService,
                                            ILogger logger)
        {
            _workContext = workContext;
            _storeService = storeService;
            _localizationService = localizationService;
            _settingService = settingService;
            _paymentService = paymentService;
            _paymentSettings = paymentSettings;
            _sisowPaymentSettings = sisowPaymentSettings;
            _orderProcessingService = orderProcessingService;
            _orderService = orderService;
            _logger = logger;
        }

        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            //load settings for a chosen store scope
            var storeScope = GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var sisowPaymentSettings = _settingService.LoadSetting<SisowPaymentSettings>(storeScope);

            var model = new ConfigurationModel
            {
                MerchantId = sisowPaymentSettings.MerchantId,
                MerchantKey = sisowPaymentSettings.MerchantKey,
                NormalReturnUrl = sisowPaymentSettings.NormalReturnUrl,
                NotifyUrl = sisowPaymentSettings.NotifyUrl,
                SisowUrl = sisowPaymentSettings.SisowUrl,
                MinimumAmount = sisowPaymentSettings.MinimumAmount,
                AdditionalFee = sisowPaymentSettings.AdditionalFee,
                PaymentMeanBrandList = sisowPaymentSettings.PaymentMeanBrandList,
                TestMode = sisowPaymentSettings.TestMode,
                DescriptionText = sisowPaymentSettings.DescriptionText
            };

            if (storeScope > 0)
            {
                model.MerchantId_OverrideForStore = _settingService.SettingExists(sisowPaymentSettings, x => x.MerchantId, storeScope);
                model.MerchantKey_OverrideForStore = _settingService.SettingExists(sisowPaymentSettings, x => x.MerchantKey, storeScope);
                model.NormalReturnUrl_OverrideForStore = _settingService.SettingExists(sisowPaymentSettings, x => x.NormalReturnUrl, storeScope);
                model.NotifyUrl_OverrideForStore = _settingService.SettingExists(sisowPaymentSettings, x => x.NotifyUrl, storeScope);
                model.AdditionalFee_OverrideForStore = _settingService.SettingExists(sisowPaymentSettings, x => x.AdditionalFee, storeScope);
                model.SisowUrl_OverrideForStore = _settingService.SettingExists(sisowPaymentSettings, x => x.SisowUrl, storeScope);
                model.MinimumAmount_OverrideForStore = _settingService.SettingExists(sisowPaymentSettings, x => x.MinimumAmount, storeScope);
                model.PaymentMeanBrandList_OverrideForStore = _settingService.SettingExists(sisowPaymentSettings, x => x.PaymentMeanBrandList, storeScope);
                model.TestMode_OverrideForStore = _settingService.SettingExists(sisowPaymentSettings, x => x.TestMode, storeScope);
                model.DescriptionText_OverrideForStore = _settingService.SettingExists(sisowPaymentSettings, x => x.DescriptionText, storeScope);
            }
            return View("~/Plugins/Payments.Sisow/Views/PaymentSisow/Configure.cshtml", model);
        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        [ValidateInput(false)]
        public ActionResult Configure(ConfigurationModel model)
        {
            if (!ModelState.IsValid)
                return Configure();

            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var sisowPaymentSettings = _settingService.LoadSetting<SisowPaymentSettings>(storeScope);

            sisowPaymentSettings.MerchantId = model.MerchantId;
            sisowPaymentSettings.MerchantKey = model.MerchantKey;
            sisowPaymentSettings.NormalReturnUrl = model.NormalReturnUrl;
            sisowPaymentSettings.NotifyUrl = model.NotifyUrl;
            sisowPaymentSettings.SisowUrl = model.SisowUrl;
            sisowPaymentSettings.MinimumAmount = model.MinimumAmount;
            sisowPaymentSettings.AdditionalFee = model.AdditionalFee;
            sisowPaymentSettings.PaymentMeanBrandList = model.PaymentMeanBrandList;
            sisowPaymentSettings.TestMode = model.TestMode;
            sisowPaymentSettings.DescriptionText = model.DescriptionText;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            _settingService.SaveSettingOverridablePerStore(sisowPaymentSettings, x => x.MerchantId, model.MerchantId_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(sisowPaymentSettings, x => x.MerchantKey, model.MerchantKey_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(sisowPaymentSettings, x => x.NormalReturnUrl, model.NormalReturnUrl_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(sisowPaymentSettings, x => x.NotifyUrl, model.NotifyUrl_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(sisowPaymentSettings, x => x.AdditionalFee, model.AdditionalFee_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(sisowPaymentSettings, x => x.SisowUrl, model.SisowUrl_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(sisowPaymentSettings, x => x.MinimumAmount, model.MinimumAmount_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(sisowPaymentSettings, x => x.PaymentMeanBrandList, model.PaymentMeanBrandList_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(sisowPaymentSettings, x => x.TestMode, model.TestMode_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(sisowPaymentSettings, x => x.DescriptionText, model.DescriptionText_OverrideForStore, storeScope, false);

            //now clear settings cache
            _settingService.ClearCache();

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
            //return View("~/Plugins/Payments.Sisow/Views/PaymentSisow/Configure.cshtml"; model);
        }

        [ChildActionOnly]
        public ActionResult PaymentInfo()
        {
            var totalAmount = _workContext.CurrentCustomer.ShoppingCartItems.Sum(i => i.Product.Price);

            var model = new PaymentInfoModel()
            {
                Amount = totalAmount,
                MerchantId = _sisowPaymentSettings.MerchantId,
                ReturnUrl = _sisowPaymentSettings.NormalReturnUrl,
                NotifyUrl = _sisowPaymentSettings.NotifyUrl,
                DescriptionText = _sisowPaymentSettings.DescriptionText
            };
            return View("~/Plugins/Payments.Sisow/Views/PaymentSisow/PaymentInfo.cshtml", model);
        }

        /// <summary>
        /// This method is called in the background by the Sisow service with the transaction id of the associated payment.
        /// The only task of this method is to validate the payment, no action or redirection is necessary.
        /// </summary>
        /// <returns>Returns always null</returns>
        [ValidateInput(false)]
        public ActionResult SisowReport(string trxid, string ec, string status, string sha1, bool? notify, bool? callback, string source = "Sisow Auto Response")
        {
            string errorStr;

            var processor = _paymentService.LoadPaymentMethodBySystemName("Payments.Sisow") as SisowPaymentProcessor;
            if (processor == null ||
                !processor.IsPaymentMethodActive(_paymentSettings) || !processor.PluginDescriptor.Installed)
                throw new NopException("Sisow module cannot be loaded");

            var order = _orderService.GetOrderByAuthorizationTransactionIdAndPaymentMethod(ec, "Payments.Sisow");

            if (order == null)
            {
                _logger.Error($"Order niet gevonden. TrxId: {trxid}, Ec: {ec}, Status: {status}");
                return null;
            }
            var calcSHA1 = SisowHelper.GetSHA1(trxid + ec + status + _sisowPaymentSettings.MerchantId + _sisowPaymentSettings.MerchantKey);
            order.AuthorizationTransactionCode = trxid;
            if (sha1 != calcSHA1)
            {

                _orderService.UpdateOrder(order);
                errorStr = $"{source} - Payment for order {order.Id} failed!, Sha1Status: TA9997, TrxId: {trxid}, Ec: {ec}, Status: {status}";
                _logger.Error(errorStr);
                //order note
                order.OrderNotes.Add(new OrderNote
                {
                    Note = errorStr,
                    DisplayToCustomer = false,
                    CreatedOnUtc = DateTime.UtcNow
                });
            }

            if (status == "Success")
            {
                // The order matches the XML message. 
                if (_orderProcessingService.CanMarkOrderAsPaid(order))
                {
                    order.AuthorizationTransactionResult = "Sisow OK";

                    //order.AmountPaid = order.OrderTotal;
                    _orderService.UpdateOrder(order);
                    _orderProcessingService.MarkOrderAsPaid(order);
                    //order note
                    order.OrderNotes.Add(new OrderNote
                    {
                        Note = "Order marked as paid",
                        DisplayToCustomer = false,
                        CreatedOnUtc = DateTime.UtcNow
                    });
                }
            }
            if (status == "Cancelled" || status == "Failure" || status == "Expired")
            {
                if (order.AuthorizationTransactionResult == null || order.AuthorizationTransactionResult.Trim() == "")
                {
                    order.AuthorizationTransactionResult = "Sisow NOT OK";
                    _orderService.UpdateOrder(order);

                    errorStr = $"{source} - Payment for order {order.Id} failed!, Status: {status}, TrxId: {trxid}, Ec: {ec}";
                    _logger.Error(errorStr);
                    //order note
                    order.OrderNotes.Add(new OrderNote
                    {
                        Note = errorStr,
                        DisplayToCustomer = false,
                        CreatedOnUtc = DateTime.UtcNow
                    });
                }
            }
            return null;
        }

        /// <summary>
        /// This method is called by the Sisow service to redirect the user to the next page. 
        /// </summary>
        /// <returns>Redirects to user to checkout completed or payment failed page.</returns>
        [ValidateInput(false)]
        public ActionResult SisowReturn(string trxid, string ec, string status, string sha1, bool? notify, bool? callback)
        {
            var order = _orderService.GetOrderByAuthorizationTransactionIdAndPaymentMethod(ec, "Payments.Sisow");

            if (order == null)
            {
                return null;
            }

            //TODO: This aint work if first time failed and Result is already filled and the automatic call to SisowReport from Sisow didt took place.
            // possible fix: empty this field just before call Sisow site from method PostProcessPayment
            if (order.AuthorizationTransactionResult == null || order.AuthorizationTransactionResult.Trim() == "")
            {
                SisowReport(trxid, ec, status, sha1, notify, callback, "Sisow Manual Response");
            }

            if (order.PaymentStatus == PaymentStatus.Paid)
            {
                return RedirectToRoute("CheckoutCompleted");
            }
            else
            {
                var returnCode = status;
                var errorText = "Er is een onbekende foutopgetreden";

                switch (returnCode)
                {
                    case "Failure":
                        errorText =
                            "Er is een fout (Failure) opgetreden bij het betalen met Ideal. Er heeft geen betaling plaatsgevonden.";
                        break;
                    case "Cancelled":
                        errorText =
                            "Er is een fout (Cancelled) opgetreden bij het betalen met Ideal. De gebruiker heeft de transactie geannuleerd. Er heeft geen betaling plaatsgevonden.";
                        break;
                    case "Expired":
                        errorText =
                            "Er is een fout (Expired) opgetreden bij het betalen met Ideal. De geldigheidsduur van de transactie is verlopen. Er heeft geen betaling plaatsgevonden.";
                        break;
                    default:
                        errorText += " bij het betalen met Ideal. (" + (returnCode) + "). Er heeft geen betaling plaatsgevonden.";
                        break;
                }

                var model = new PaymentFailedModel()
                {
                    OrderId = order.Id,
                    Amount = order.OrderTotal,
                    ErrorDescription = errorText

                };
                var errorStr = $"Sisow IDEAL: Er is een fout opgetreden. ErrorCode: {returnCode} OrderId: {order.Id} OrderBedrag: {order.OrderTotal} Foutmelding gebruiker: {errorText} TrxId: {trxid}";
                _logger.Error(errorStr);
                //order note
                order.OrderNotes.Add(new OrderNote
                {
                    Note = errorStr,
                    DisplayToCustomer = false,
                    CreatedOnUtc = DateTime.UtcNow
                });
                return View("~/Plugins/Payments.Sisow/Views/PaymentSisow/SisowReturn.cshtml", model);
            }
        }

        /// <summary>
        /// This method is called by the Sisow service to redirect the user to the error page. 
        /// </summary>
        /// <returns>Redirects to user to error / failed page.</returns>
        [ValidateInput(false)]
        public ActionResult SisowErrorReturn(string orderId, string errorCode)
        {
            var modelFailed = new PaymentFailedModel();
            var trxId = string.Empty;
            int idOrder = -1;
            if (orderId != "")
            {
                int.TryParse(orderId, out idOrder);
                var order = _orderService.GetOrderById(idOrder);
                trxId = order.AuthorizationTransactionCode;
                modelFailed.Amount = order.OrderTotal;
            }
            string errorMsg;

            switch (errorCode)
            {
                case "TA3210":
                case "TA3220":
                    //merchant
                    errorMsg = "Er is een fout opgetreden. Code: -1";
                    break;
                case "TA3230":
                case "TA3240":
                case "TA3250":
                    //purchase / Ordernummer
                    errorMsg = "Er is een fout opgetreden met Ordernummer. Code: -2";
                    break;
                case "TA3260":
                case "TA3270":
                case "TA3280":
                    //Amount
                    errorMsg = "Er is een fout opgetreden met orderbedrag. Code: -3";
                    break;
                case "TA3290":
                case "TA3300":
                    //IssuerId
                    errorMsg = "Er is een fout opgetreden met geselecteerde bank. Code: -4";
                    break;
                case "TA3310":
                case "TA3320":
                    //Entrancecode
                    errorMsg = "Er is een fout opgetreden. Code: -5";
                    break;
                case "TA3330":
                case "TA3340":
                    //SHA1
                    errorMsg = "Er is een fout opgetreden. Code: -6";
                    break;
                case "TA3350":
                case "TA3360":
                    //Description
                    errorMsg = "Er is een fout opgetreden. Code: -7";
                    break;
                case "TA3370":
                case "TA3380":
                case "TA3390":
                    //Return Url / Insert Error / IDealException
                    errorMsg = "Er is een fout opgetreden. Code: -8";
                    break;
                case "TA9997":
                    //SHA1 komt niet overeen
                    errorMsg = "Er is een fout opgetreden. Code: -23";
                    break;
                case "TA9998":
                    //Reponse Code van de Sisow URL is anders dan Success
                    errorMsg = "Er is een fout opgetreden. Code: -21";
                    break;
                case "TA9999":
                    // Onbekende reactie van de sisow server
                    errorMsg = "Er is een fout opgetreden. Code: -22";
                    break;
                default:
                    errorMsg = "Er is een onbekende fout opgetreden. Code: -99";
                    break;
            }

            modelFailed.ErrorCode = errorCode;
            modelFailed.OrderId = idOrder;
            modelFailed.ErrorCode = errorCode;
            modelFailed.ErrorDescription = errorMsg + ". Er heeft geen betaling plaatsgevonden.";

            _logger.Error($"Sisow IDEAL: Er is een fout opgetreden. ErrorCode: {modelFailed.ErrorCode}, OrderIdParameter: {modelFailed.OrderId}, OrderBedrag: {modelFailed.Amount}, Foutmelding gebruiker: {errorMsg}, Trxid: {trxId}, idOrder (Casted): {idOrder}");

            return View("~/Plugins/Payments.Sisow/Views/PaymentSisow/SisowReturn.cshtml", modelFailed);
        }

        /// <summary>
        /// This method is called by the Sisow service to redirect the user to the next page. 
        /// </summary>
        /// <returns>Redirects the user to checkout completed or payment failed page.</returns>
        [ValidateInput(false)]
        public ActionResult SisowPayment(string content)
        {
            ViewBag.content = content;
            return View("~/Plugins/Payments.Sisow/Views/PaymentSisow/SisowReturn.cshtml");
        }

        [NonAction]
        public override IList<string> ValidatePaymentForm(FormCollection form)
        {
            var warnings = new List<string>();

            //validate
            var validator = new PaymentInfoValidator(_localizationService);
            var totalAmount = _workContext.CurrentCustomer.ShoppingCartItems.Sum(i => i.Product.Price); //this._shoppingCartService.

            var model = new PaymentInfoModel()
            {
                //BankList = GetBankList(),
                Amount = totalAmount,
                MerchantId = _sisowPaymentSettings.MerchantId,
                ReturnUrl = _sisowPaymentSettings.NormalReturnUrl,
                NotifyUrl = _sisowPaymentSettings.NotifyUrl
                //EntranceCode = _sisowPaymentSettings.,                
            };

            var validationResult = validator.Validate(model);
            if (!validationResult.IsValid)
                foreach (var error in validationResult.Errors)
                    warnings.Add(error.ErrorMessage);

            return warnings;
        }

        [NonAction]
        public override ProcessPaymentRequest GetPaymentInfo(FormCollection form)
        {
            var paymentInfo = new ProcessPaymentRequest();
            return paymentInfo;
        }
    }
}