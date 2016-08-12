using Nop.Core.Configuration;

namespace Nop.Plugin.Payments.Sisow
{
    public class SisowPaymentSettings : ISettings
    {
        public string MerchantId { get; set; }
        public string MerchantKey { get; set; }

        [System.ComponentModel.DataAnnotations.StringLength(512)]
        public string NormalReturnUrl { get; set; }

        public string RefundReturnUrl { get; set; }
        [System.ComponentModel.DataAnnotations.StringLength(512)]
        public string NotifyUrl { get; set; }

        public string SisowUrl { get; set; }


        public string PaymentMeanBrandList { get; set; }

        public decimal AdditionalFee { get; set; }

        public decimal MinimumAmount { get; set; }

        public bool TestMode { get; set; }

        public string DescriptionText { get; set; }

    }
}
