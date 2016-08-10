using System.Collections.Generic;
using Nop.Web.Framework.Mvc;
using System.Web.Mvc;
using Nop.Web.Framework;
using System;
using System.ComponentModel.DataAnnotations;

namespace Nop.Plugin.Payments.Sisow.Models
{
    public class PaymentInfoModel : BaseNopModel
    {
        public string MerchantId { get; set; }

        public string ReturnUrl { get; set; }
              
        public string NotifyUrl { get; set; }

        [Display(Name = "Totaal bedrag")]
        [AllowHtml]
        public decimal Amount { get; set; }

        public string EntranceCode { get; set; }                

        public string MerchantKey { get; set; }

        public bool TestMode { get; set; }

        public string PurchaseId { get; set; }
        
        public string DescriptionText { get; set; }
        public string Description { get; set; }
        
        public string Payment { get; set; }

        public string Sha1 { get; set; }

    }
}
