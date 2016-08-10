using Nop.Web.Framework.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Nop.Plugin.Payments.Sisow.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }

        [DisplayName("Sisow URL"), Required(ErrorMessage = "Sisow URL is required.")]
        [StringLength(512)]
        public string SisowUrl { get; set; }
        public bool SisowUrl_OverrideForStore { get; set; }


        [DisplayName("Merchant ID"), Required(ErrorMessage = "Merchant ID is required.")]
        public string MerchantId { get; set; }
        public bool MerchantId_OverrideForStore { get; set; }

        [DisplayName("Normal Return Url"), Required(ErrorMessage = "Normal Return url is required.")]
        [StringLength(512)]
        public string NormalReturnUrl { get; set; }
        public bool NormalReturnUrl_OverrideForStore { get; set; }

        [DisplayName("Notify Url"), Required(ErrorMessage = "Notify url is required.")]
        [StringLength(512)]
        public string NotifyUrl { get; set; }
        public bool NotifyUrl_OverrideForStore { get; set; }

        [DisplayName("Additional fee [EUR]")]
        public decimal AdditionalFee { get; set; }
        public bool AdditionalFee_OverrideForStore { get; set; }

        [DisplayName("Minimum order amount")]
        public decimal MinimumAmount { get; set; }
        public bool MinimumAmount_OverrideForStore { get; set; }

        [DisplayName("Merchant Key")]
        public string MerchantKey { get; set; }
        public bool MerchantKey_OverrideForStore { get; set; }

        [DisplayName("Payment Mean Brand List")]
        public string PaymentMeanBrandList { get; set; }
        public bool PaymentMeanBrandList_OverrideForStore { get; set; }

        [DisplayName("Test Mode (check to enable)")]
        public bool TestMode { get; set; }
        public bool TestMode_OverrideForStore { get; set; }

        [DisplayName("CustomerDescription (Information on website)")]
        public string DescriptionText { get; set; }
        public bool DescriptionText_OverrideForStore { get; set; }
    }
}


