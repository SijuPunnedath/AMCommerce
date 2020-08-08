using Grand.Core.Configuration;

namespace Grand.Plugin.Widgets.AjaxFilters
{
    public class AjaxFiltersSettings : ISettings
    {
        public string FilterWidgetZone { get; set; }
        public bool FilterEnablePriceRange { get; set; }
        public bool FilterEnableManufacturers { get; set; }
        public bool FilterEnableVendors { get; set; }
        public bool FilterEnableSpecifications { get; set; }
        public bool FilterEnableAttributes { get; set; }
        public int DefaultPageSize { get; set; }
        public AjaxFiltersUI FilterManufacturerCheckOrDropdown { get; set; }
        public AjaxFiltersUI FilterVendorCheckOrDropdown { get; set; }
        public AjaxFiltersUI FilterSpecyficationCheckOrDropdown { get; set; }
        public AjaxFiltersUI FilterAttributesCheckOrDropdown { get; set; }
    }
}
