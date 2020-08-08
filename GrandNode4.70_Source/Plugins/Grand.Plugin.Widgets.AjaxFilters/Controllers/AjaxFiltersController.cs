using Grand.Framework.Controllers;
using Grand.Framework.Mvc.Filters;
using Grand.Plugin.Widgets.AjaxFilters.Models;
using Grand.Services.Configuration;
using Grand.Services.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Grand.Plugin.Widgets.AjaxFilters.Controllers
{
    [AuthorizeAdmin]
    [Area("Admin")]
    public class AjaxFiltersController : BasePluginController
    {
        private readonly AjaxFiltersSettings _ajaxFiltersSettings;
        private readonly ILocalizationService _localizationService;
        private readonly ISettingService _settingService;

        public AjaxFiltersController(AjaxFiltersSettings ajaxFiltersSettings,
            ILocalizationService localizationService,
            ISettingService settingService)
        {
            _ajaxFiltersSettings = ajaxFiltersSettings;
            _localizationService = localizationService;
            _settingService = settingService;
        }

        public IActionResult Configure()
        {
            var model = new ConfigurationModel();
            model.FilterWidgetZone = _ajaxFiltersSettings.FilterWidgetZone;
            model.FilterEnableAttributes = _ajaxFiltersSettings.FilterEnableAttributes;
            model.FilterEnableManufacturers = _ajaxFiltersSettings.FilterEnableManufacturers;
            model.FilterEnablePriceRange = _ajaxFiltersSettings.FilterEnablePriceRange;
            model.FilterEnableSpecifications = _ajaxFiltersSettings.FilterEnableSpecifications;
            model.FilterEnableVendors = _ajaxFiltersSettings.FilterEnableVendors;
            model.FilterManufacturerCheckOrDropdown = _ajaxFiltersSettings.FilterManufacturerCheckOrDropdown;
            model.FilterSpecyficationCheckOrDropdown = _ajaxFiltersSettings.FilterSpecyficationCheckOrDropdown;
            model.FilterVendorCheckOrDropdown = _ajaxFiltersSettings.FilterVendorCheckOrDropdown;
            model.FilterAttributesCheckOrDropdown = _ajaxFiltersSettings.FilterAttributesCheckOrDropdown;

            return View("~/Plugins/Widgets.AjaxFilters/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            //save settings
            _ajaxFiltersSettings.FilterEnableAttributes = model.FilterEnableAttributes;
            _ajaxFiltersSettings.FilterEnableManufacturers = model.FilterEnableManufacturers;
            _ajaxFiltersSettings.FilterEnablePriceRange = model.FilterEnablePriceRange;
            _ajaxFiltersSettings.FilterEnableSpecifications = model.FilterEnableSpecifications;
            _ajaxFiltersSettings.FilterEnableVendors = model.FilterEnableVendors;
            _ajaxFiltersSettings.FilterWidgetZone = model.FilterWidgetZone;
            _ajaxFiltersSettings.FilterManufacturerCheckOrDropdown = model.FilterManufacturerCheckOrDropdown;
            _ajaxFiltersSettings.FilterSpecyficationCheckOrDropdown = model.FilterSpecyficationCheckOrDropdown;
            _ajaxFiltersSettings.FilterVendorCheckOrDropdown = model.FilterVendorCheckOrDropdown;
            _ajaxFiltersSettings.FilterAttributesCheckOrDropdown = model.FilterAttributesCheckOrDropdown;

            await _settingService.SaveSetting(_ajaxFiltersSettings);

            //now clear settings cache
            await _settingService.ClearCache();
            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));
            return Configure();
        }

    }
}