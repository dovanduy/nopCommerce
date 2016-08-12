using Nop.Web.Framework.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Nop.Plugin.Payments.Sisow.Models
{
    public class RefundInfoModel : BaseNopModel
    {
        public string MerchantId { get; set; }
        public string ReturnRefundUrl { get; set; }
        [Display(Name = "Totaal bedrag")]
        public decimal RefundAmount { get; set; }

        public string Sha1 { get; set; }
        public string AuthorizationTransactionCode { get; set; }

    }
}
