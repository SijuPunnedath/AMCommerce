using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Grand.Core;
using Grand.Core.Caching;
using Grand.Core.Domain.Catalog;
using Grand.Core.Domain.Media;
using Grand.Plugin.Widgets.SwiperSlider.Infrastructure.Cache;
using Grand.Plugin.Widgets.SwiperSlider.Models;
using Grand.Plugin.Widgets.SwiperSlider.Services;
using Grand.Services.Configuration;
using Grand.Services.Media;
using Grand.Services.Stores;
using System.Linq;
using System.Threading.Tasks;

namespace Grand.Plugin.Widgets.SwiperSlider.Components
{
    [ViewComponent(Name = "WidgetSwiperSliderPublicInfo")]
    public class WidgetsSwiperSliderPublicInfoViewComponent : ViewComponent
    {
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly IPictureService _pictureService;
        private readonly ISlideService _slideService;
        private readonly ISettingService _settingService;
        private readonly ICacheManager _cacheManager;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IWorkContext _workContext;
        private readonly MediaSettings _mediaSettings;
        private readonly CatalogSettings _catalogSettings;

        public WidgetsSwiperSliderPublicInfoViewComponent(
            IStoreContext storeContext,
            IStoreService storeService,
            ISlideService slideService,
            IPictureService pictureService,
            ISettingService settingService,
            CatalogSettings catalogSettings,
            MediaSettings mediaSettings,
            ICacheManager cacheManager,
            IStoreMappingService storeMappingService,
            IWorkContext workContext)
        {
            this._storeContext = storeContext;
            this._storeService = storeService;
            this._pictureService = pictureService;
            this._slideService = slideService;
            this._settingService = settingService;
            this._catalogSettings = catalogSettings;
            this._mediaSettings = mediaSettings;
            this._cacheManager = cacheManager;
            this._storeMappingService = storeMappingService;
            this._workContext = workContext;
        }

        private async Task<string> GetPictureUrl(string pictureId)
        {
            string cacheKey = string.Format(ModelCacheEventConsumer.PICTURE_URL_MODEL_KEY, pictureId);
            return await _cacheManager.Get(cacheKey, async () =>
            {
                var picture = await _pictureService.GetPictureById(pictureId);
                var url = await _pictureService.GetPictureUrl(pictureId, showDefaultPicture: false);
                if (url == null)
                    url = "";

                return url;
            });
        }
        
        public async Task<IViewComponentResult> InvokeAsync(string widgetZone)
        {
            var sliderSettings = _settingService.LoadSetting<WidgetSwiperSliderSettings>(_storeContext.CurrentStore.Id);
            
            var Data = new
            {
                autoPlay = sliderSettings.autoPlay,
                showNavigation = sliderSettings.showNavigation,
                showPagination = sliderSettings.showPagination,
                paginationType = sliderSettings.paginationType,
                speed = sliderSettings.speed,
                loop = sliderSettings.loop,
                lazyLoading = sliderSettings.lazyLoading,
                preLoadImages = sliderSettings.preLoadImages,
                WidgetZone = sliderSettings.WidgetZone,
                CateoryWidgetZone = sliderSettings.CateroryWidgetZone,
                ManufacturerWidgetZone = sliderSettings.ManufacturerWidgetZone,
                usecountdown = sliderSettings.usecountdown,
                countdown = sliderSettings.countdown,
                slidesPerView = sliderSettings.slidesPerView,
                spaceBetween = sliderSettings.spaceBetween,
                setMinHeight = sliderSettings.setMinHeight,

                breakpoints = sliderSettings.breakpoints,
                slidesPerViewExtraSmall = sliderSettings.slidesPerViewExtraSmall,
                spaceBetweenExtraSmall = sliderSettings.spaceBetweenExtraSmall,
                slidesPerViewSmall = sliderSettings.slidesPerViewSmall,
                spaceBetweenSmall = sliderSettings.spaceBetweenSmall,
                slidesPerViewMedium = sliderSettings.slidesPerViewMedium,
                spaceBetweenMedium = sliderSettings.spaceBetweenMedium,
                slidesPerViewLarge = sliderSettings.slidesPerViewLarge,
                spaceBetweenLarge = sliderSettings.spaceBetweenLarge,
                slidesPerViewExtraLarge = sliderSettings.slidesPerViewExtraLarge,
                spaceBetweenExtraLarge = sliderSettings.spaceBetweenExtraLarge,

                turnOffExtraSmall = sliderSettings.turnOffExtraSmall,
                turnOffSmall = sliderSettings.turnOffSmall,
                turnOffMedium = sliderSettings.turnOffMedium,
                turnOffLarge = sliderSettings.turnOffLarge,
                turnOffExtraLarge = sliderSettings.turnOffExtraLarge
            };
            //int currentStore = _storeContext.CurrentStore.Id;
            PublicSlideModel mm = new PublicSlideModel();

            var model = new PublicInfoModel();

            foreach (var x in _slideService.GetAllSlides().Where(x => x.Publish == true && (string.IsNullOrEmpty(x.CategoryId) && string.IsNullOrEmpty(x.ManufactureId) || x.ApplyForHomepage == true)
                && (x.LimitedToStores == false)
                ).OrderBy(x => x.SlideOrder))
            {
                var y = x.ToModel(await GetPictureUrl(x.PictureId), _workContext.WorkingLanguage.Id);
                model.Slides.Add(y);
            }

            if (model.Slides.Count == 0)
                return Content("");


            return View("~/Plugins/Widgets.SwiperSlider/Views/WidgetsSwiperSlider/PublicInfo.cshtml", model);
        }
    }
}