using Grand.Framework.Mvc.Models;

namespace Grand.Plugin.Widgets.AjaxFilters.Models
{
    public class PublicInfoModel : BaseGrandModel
    {
        public PublicInfoModel()
        {
            filterPriceModel = new FilterPriceRangeModel();
            manufacturerModel = new FilterManufacturersModel();
            specyficationModel = new FilterSpecificationsModel();
            vendorsModel = new FilterVendorsModel();
            attributesModel = new FilterProductAttributesModel();
        }
        public ProductsModel AjaxProductsModel { get; set; }
        public string CategoryId { get; set; }
        public string ManufacturerId { get; set; }
        public string VendorId { get; set; }
        public bool EnableManufacturersFilter { get; set; }
        public bool EnableVendorsFilter { get; set; }
        public bool EnablePriceRangeFilter { get; set; }
        public bool EnableSpecificationsFilter { get; set; }
        public bool EnableAttributesFilter { get; set; }

        public FilterPriceRangeModel filterPriceModel { get; set; }
        public FilterManufacturersModel manufacturerModel { get; set; }
        public FilterSpecificationsModel specyficationModel { get; set; }
        public FilterVendorsModel vendorsModel { get; set; }
        public FilterProductAttributesModel attributesModel { get; set; }

        public string ViewMode { get; set; }
        public int PageSize { get; set; }
        public int SortOption { get; set; }
        public int PageNumber { get; set; }
        public int Count { get; set; }

        public string WidgetZone { get; set; }
    }
}
