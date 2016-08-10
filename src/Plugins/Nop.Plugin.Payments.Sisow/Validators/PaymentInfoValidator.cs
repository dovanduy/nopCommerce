using FluentValidation;
using Nop.Plugin.Payments.Sisow.Models;
using Nop.Services.Localization;

namespace Nop.Plugin.Payments.Sisow.Validators
{
    public class PaymentInfoValidator : AbstractValidator<PaymentInfoModel>
    {
        public PaymentInfoValidator(ILocalizationService localizationService)
        {
            //useful links:
            //http://fluentvalidation.codeplex.com/wikipage?title=Custom&referringTitle=Documentation&ANCHOR#CustomValidator
            //http://benjii.me/2010/11/credit-card-validator-attribute-for-asp-net-mvc-3/

            // RuleFor(x => x.Amount).GreaterThan(x => x.MinimumAmount).WithMessage(localizationService.GetResource("Payment.Omnikassa.OrderTotalBelowMinimum"));
        }
    }
}