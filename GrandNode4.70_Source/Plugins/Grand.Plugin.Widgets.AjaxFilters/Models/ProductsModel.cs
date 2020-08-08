using Grand.Framework.Mvc.Models;
using Grand.Web.Models.Catalog;
using System.Collections.Generic;

namespace Grand.Plugin.Widgets.AjaxFilters.Models
{
    public class ProductsModel : BaseGrandModel
    {
        public ProductsModel()
        {
            this.Products = new List<ProductOverviewModel>();
            this.PagingFilteringContext = new CatalogPagingFilteringModel();
        }

        public int Count { get; set; }

        public CatalogPagingFilteringModel PagingFilteringContext { get; set; }

        public IEnumerable<ProductOverviewModel> Products { get; set; }

        public string ViewMode { get; set; }
    }
}
