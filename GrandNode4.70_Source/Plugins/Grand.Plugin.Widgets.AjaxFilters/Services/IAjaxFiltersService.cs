using Grand.Plugin.Widgets.AjaxFilters.Models;
using System.Threading.Tasks;

namespace Grand.Plugin.Widgets.AjaxFilters.Services
{
    public interface IAjaxFiltersService
    {
        Task<PublicInfoModel> PreparePublicInfoModel(PublicInfoEnum publicInfo, string id);
        Task PreparePublicInfoModel2(PublicInfoModel model, SearchFilterResult searchFilterResult);
        Task<SearchModel> PrepareSearchModel(PublicInfoEnum publicInfo, string id);
        Task<SearchModel> PrepareSearchModel(PublicInfoModel publicInfoModel, string typ);
        Task<SearchFilterResult> PrepareSearchFilterResult(SearchModel model, bool prepareproducts, string typ, string viewmode);
    }
}
