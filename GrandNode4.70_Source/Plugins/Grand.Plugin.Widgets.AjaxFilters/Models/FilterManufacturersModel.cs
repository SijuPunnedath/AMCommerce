using System.Collections.Generic;

namespace Grand.Plugin.Widgets.AjaxFilters.Models
{
    public class FilterManufacturersModel
    {
        public FilterManufacturersModel()
        {
            Manufacturers = new List<ManufacturersModel>();
        }
        public AjaxFiltersUI CheckOrDropdown { get; set; }
        public IList<ManufacturersModel> Manufacturers { get; set; }
    }

    public class ManufacturersModel
    {
        public ManufacturersModel()
        {
            CheckedState = CheckedState.UnChecked;
        }

        public int Count { get; set; }

        public CheckedState CheckedState { get; set; }

        public string Id { get; set; }
        public string Name { get; set; }
    }
}
