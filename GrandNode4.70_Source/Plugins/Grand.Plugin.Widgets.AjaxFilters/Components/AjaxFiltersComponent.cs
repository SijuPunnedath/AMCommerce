using Grand.Plugin.Widgets.AjaxFilters.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace Grand.Plugin.Widgets.AjaxFilters.ViewComponents
{
    [ViewComponent(Name = "Grand.Plugin.Widgets.AjaxFilters")]
    public class AjaxFiltersComponent : ViewComponent
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAjaxFiltersService _ajaxFiltersService;

        public AjaxFiltersComponent(IHttpContextAccessor httpContextAccessor, IAjaxFiltersService ajaxFiltersService)
        {
            _httpContextAccessor = httpContextAccessor;
            _ajaxFiltersService = ajaxFiltersService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData = null)
        {
            var categoryId = Convert.ToString(_httpContextAccessor.HttpContext.GetRouteValue("CategoryId"));
            if (!string.IsNullOrEmpty(categoryId))
            {
                return View("~/Plugins/Widgets.AjaxFilters/Views/PublicInfo.cshtml", await _ajaxFiltersService.PreparePublicInfoModel(PublicInfoEnum.Category, categoryId));
            }
            var manufacturerId = Convert.ToString(_httpContextAccessor.HttpContext.GetRouteValue("ManufacturerId"));
            if (!string.IsNullOrEmpty(manufacturerId))
            {
                return View("~/Plugins/Widgets.AjaxFilters/Views/PublicInfo.cshtml", await _ajaxFiltersService.PreparePublicInfoModel(PublicInfoEnum.Manufacturer, manufacturerId));
            }

            return Content("");
        }
    }
}