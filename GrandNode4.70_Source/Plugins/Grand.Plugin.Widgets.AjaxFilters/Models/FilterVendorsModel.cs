using System.Collections.Generic;

namespace Grand.Plugin.Widgets.AjaxFilters.Models
{
    public class FilterVendorsModel
    {
        public FilterVendorsModel()
        {
            Vendors = new List<VendorsModel>();
        }
        public AjaxFiltersUI CheckOrDropdown { get; set; }
        public IList<VendorsModel> Vendors { get; set; }
    }

    public class VendorsModel
    {
        public VendorsModel()
        {
            CheckedState = CheckedState.UnChecked;
        }

        public string Id { get; set; }

        public int Count { get; set; }

        public CheckedState CheckedState { get; set; }
        public string Name { get; set; }
    }
}
