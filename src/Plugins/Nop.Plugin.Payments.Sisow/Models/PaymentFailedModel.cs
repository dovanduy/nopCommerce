using System.ComponentModel;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Payments.Sisow.Models
{
    public class PaymentFailedModel : BaseNopModel
    {
        [DisplayName("Bestelnummer")]
        public int OrderId { get; set; }

        [DisplayName("Bedrag [EUR]")]
        public decimal Amount { get; set; }

        [DisplayName("Foutmelding")]
        public string ErrorDescription { get; set; }
        
        public string ErrorCode { get; set; }
    }
}
