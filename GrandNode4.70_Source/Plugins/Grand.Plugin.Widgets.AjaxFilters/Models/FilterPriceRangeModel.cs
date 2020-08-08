using Grand.Framework.Mvc.Models;

namespace Grand.Plugin.Widgets.AjaxFilters.Models
{
    public class FilterPriceRangeModel : BaseGrandModel
    {
        public string CurrencySymbol { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal CurrentMinPrice { get; set; }
        public decimal CurrentMaxPrice { get; set; }
    }
}
