using System.Collections.Generic;

namespace Grand.Plugin.Widgets.AjaxFilters.Models
{
    public class FilterProductAttributesModel
    {
        public FilterProductAttributesModel()
        {
            this.ProductVariantAttributes = new List<FilterProductVariantAttributesModel>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public AjaxFiltersUI CheckOrDropdowns { get; set; }
        public IList<FilterProductVariantAttributesModel> ProductVariantAttributes { get; set; }
    }

    public class FilterProductVariantAttributesModel
    {
        public FilterProductVariantAttributesModel()
        {
            this.ProductVariantAttributesOptions = new List<ProductVariantAttributesOptionsModel>();
        }

        public IList<ProductVariantAttributesOptionsModel> ProductVariantAttributesOptions { get; set; }

        public string Id { get; set; }
        public string Name { get; set; }

    }
    public class ProductVariantAttributesOptionsModel
    {
        public ProductVariantAttributesOptionsModel()
        {
            CheckedState = CheckedState.UnChecked;
        }
        public CheckedState CheckedState { get; set; }

        public string Name { get; set; }
        public int Count { get; set; }
        public string ColorSquaresRgb { get; set; }

    }
}
