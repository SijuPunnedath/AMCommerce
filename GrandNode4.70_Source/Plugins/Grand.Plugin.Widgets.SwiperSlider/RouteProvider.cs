using Grand.Framework.Mvc.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Grand.Plugin.Widgets.SwiperSlider
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(IEndpointRouteBuilder routeBuilder)
        {
            routeBuilder.MapControllerRoute("Grand.Plugins.Widgets.SwiperSlider.List",
                 "Admin/Slides/List",
                 new { controller = "Slides", action = "List" }           
            );

            routeBuilder.MapControllerRoute("Grand.Plugins.Widgets.SwiperSlider.Add",
                 "Admin/Slides/Create",
                 new { controller = "Slides", action = "Create" }                  
            );

            routeBuilder.MapControllerRoute("Grand.Plugins.Widgets.SwiperSlider.Edit",
                 "Admin/Slides/Edit",
                 new { controller = "Slides", action = "Edit" }                  
            );
            routeBuilder.MapControllerRoute("Grand.Plugins.Widgets.SwiperSlider.Delete",
                 "Admin/Slides/Delete",
                 new { controller = "Slides", action = "Delete" }                  
            );

            routeBuilder.MapControllerRoute("Grand.Plugins.Widgets.SwiperSlider.SlideUpdate",
                 "Admin/Slides/SlideUpdate",
                 new { controller = "Slides", action = "SlideUpdate" }                  
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
