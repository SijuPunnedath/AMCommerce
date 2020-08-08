using Grand.Framework.Mvc.ModelBinding;
using Grand.Framework.Mvc.Models;

namespace Grand.Plugin.Widgets.AjaxFilters.Models
{
    public class ConfigurationModel : BaseGrandModel
    {

        [GrandResourceDisplayName("Plugin.Widgets.AjaxFilters.FilterEnablePriceRange")]
        public bool FilterEnablePriceRange { get; set; }

        [GrandResourceDisplayName("Plugin.Widgets.AjaxFilters.FilterEnableManufacturers")]
        public bool FilterEnableManufacturers { get; set; }

        [GrandResourceDisplayName("Plugin.Widgets.AjaxFilters.FilterEnableVendors")]
        public bool FilterEnableVendors { get; set; }

        [GrandResourceDisplayName("Plugin.Widgets.AjaxFilters.FilterEnableSpecifications")]
        public bool FilterEnableSpecifications { get; set; }

        [GrandResourceDisplayName("Plugin.Widgets.AjaxFilters.FilterEnableAttributes")]
        public bool FilterEnableAttributes { get; set; }


        [GrandResourceDisplayName("Plugin.Widgets.AjaxFilters.FilterManufacturerCheckOrDropdown")]
        public AjaxFiltersUI FilterManufacturerCheckOrDropdown { get; set; }

        [GrandResourceDisplayName("Plugin.Widgets.AjaxFilters.FilterVendorCheckOrDropdown")]
        public AjaxFiltersUI FilterVendorCheckOrDropdown { get; set; }

        [GrandResourceDisplayName("Plugin.Widgets.AjaxFilters.FilterSpecyficationCheckOrDropdown")]
        public AjaxFiltersUI FilterSpecyficationCheckOrDropdown { get; set; }

        [GrandResourceDisplayName("Plugin.Widgets.AjaxFilters.FilterAttributesCheckOrDropdown")]
        public AjaxFiltersUI FilterAttributesCheckOrDropdown { get; set; }

        [GrandResourceDisplayName("Plugin.Widgets.AjaxFilters.FilterWidgetZone")]
        public string FilterWidgetZone { get; set; }

    }
}
