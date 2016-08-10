using Nop.Web.Framework.Mvc.Routes;
using System.Web.Mvc;
using System.Web.Routing;

namespace Nop.Plugin.Payments.Sisow
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            var route = routes.MapRoute("Plugin.Payments.Sisow.Configure",
                 "Plugins/PaymentSisow/Configure",
                 new { controller = "PaymentSisow", action = "Configure" },
                 new[] { "Nop.Plugin.Payments.Sisow.Controllers" }
            );
            route.DataTokens.Add("area", "Admin");

            routes.MapRoute("Plugin.Payments.Sisow.PaymentInfo",
                 "Plugins/PaymentSisow/PaymentInfo",
                 new { controller = "PaymentSisow", action = "PaymentInfo" },
                 new[] { "Nop.Plugin.Payments.Sisow.Controllers" }
            );

            routes.MapRoute("Plugin.Payments.Sisow.SisowReport",
                 "Plugins/PaymentSisow/SisowReport",
                 new { controller = "PaymentSisow", action = "SisowReport" },
                 new[] { "Nop.Plugin.Payments.Sisow.Controllers" }
            );

            routes.MapRoute("Plugin.Payments.Sisow.SisowReturn",
                 "Plugins/PaymentSisow/SisowReturn",
                 new { controller = "PaymentSisow", action = "SisowReturn" },
                 new[] { "Nop.Plugin.Payments.Sisow.Controllers" }
            );

            routes.MapRoute("Plugin.Payments.Sisow.SisowPayment",
             "Plugins/PaymentSisow/SisowPayment",
             new { controller = "PaymentSisow", action = "SisowPayment" },
             new[] { "Nop.Plugin.Payments.Sisow.Controllers" }
            );

        }

        public int Priority
        {
            get
            {
                return 0;
            }
        }
    }
}
