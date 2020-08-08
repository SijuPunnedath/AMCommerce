using System.Collections.Generic;

namespace Grand.Plugin.Widgets.AjaxFilters.Models
{
    public class FilterSpecificationsModel
    {
        public FilterSpecificationsModel()
        {
            SpecificationAttributes = new List<SpecificationAttributesModel>();
        }
        public AjaxFiltersUI CheckOrDropdowns { get; set; }

        public List<SpecificationAttributesModel> SpecificationAttributes { get; set; }
    }
    public class SpecificationAttributesModel
    {
        public SpecificationAttributesModel()
        {
            SpecificationAttributeOptions = new List<SpecificationAttributeOptionsModel>();
        }
        public string Id { get; set; }
        public string Name { get; set; }

        public IList<SpecificationAttributeOptionsModel> SpecificationAttributeOptions { get; set; }
    }
    public class SpecificationAttributeOptionsModel
    {
        public SpecificationAttributeOptionsModel()
        {
            CheckedState = CheckedState.UnChecked;
        }

        public int Count { get; set; }

        public CheckedState CheckedState { get; set; }

        public string Id { get; set; }
        public string Name { get; set; }
        public string ColorSquaresRgb { get; set; }
    }
}
