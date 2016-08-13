namespace Nop.Web.Models.Checkout
{
    public partial class CheckoutShippingMethodModel
    {

        #region Nested classes

        public partial class ShippingMethodModel
        {
            public string ReasonDisabled { get; set; }
            public bool Disabled { get; set; }
        }
        #endregion
    }
}