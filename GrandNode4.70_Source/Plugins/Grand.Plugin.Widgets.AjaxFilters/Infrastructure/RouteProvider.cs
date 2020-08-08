using Grand.Framework.Mvc.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Grand.Plugin.Widgets.AjaxFilters.Infrastructure
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(IEndpointRouteBuilder routeBuilder)
        {          
            routeBuilder.MapControllerRoute("Plugins.Widgets.AjaxFilters.ReloadFilters",
                "AjaxFilters/ReloadFilters",
                new { controller = "AjaxCatalog", action = "ReloadFilters" });

        }
        public int Priority {
            get {
                return 0;
            }
        }
    }
}
