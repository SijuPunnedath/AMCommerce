using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Grand.Core;
using Grand.Core.Caching;
using Grand.Core.Domain.Catalog;
using Grand.Core.Domain.Media;
using Grand.Core.Infrastructure;
using Grand.Plugin.Widgets.SwiperSlider.Infrastructure.Cache;
using Grand.Plugin.Widgets.SwiperSlider.Models;
using Grand.Plugin.Widgets.SwiperSlider.Services;
using Grand.Services.Configuration;
using Grand.Services.Media;
using Grand.Services.Stores;
using System;
using System.Linq;
using Grand.Plugin.Widgets.SwiperSlider;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Grand.Plugin.Widgets.SwiperSlider.Components
{
    [ViewComponent(Name = "WidgetSwiperSliderPublicInfoCategoryManufacturer")]
    public class WidgetsSwiperSliderPublicInfoCMViewComponent : ViewComponent
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
        private readonly IServiceProvider _serviceProvider;

        public WidgetsSwiperSliderPublicInfoCMViewComponent(
            IStoreContext storeContext,
            IStoreService storeService,
            ISlideService slideService,
            IPictureService pictureService,
            ISettingService settingService,
            CatalogSettings catalogSettings,
            MediaSettings mediaSettings,
            ICacheManager cacheManager,
            IWorkContext workContext,
            IStoreMappingService storeMappingService,
            IServiceProvider serviceProvider)
        {
            _storeContext = storeContext;
            _storeService = storeService;
            _pictureService = pictureService;
            _slideService = slideService;
            _settingService = settingService;
            _catalogSettings = catalogSettings;
            _mediaSettings = mediaSettings;
            _cacheManager = cacheManager;
            _storeMappingService = storeMappingService;
            _workContext = workContext;
            _serviceProvider = serviceProvider;
        }

        private async Task<string> GetPictureUrl(string pictureId)
        {
            string cacheKey = string.Format(ModelCacheEventConsumer.PICTURE_URL_MODEL_KEY, pictureId);
            return await _cacheManager.Get(cacheKey, async () =>
            {
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


            PublicSlideModel mm = new PublicSlideModel();
            var query = _slideService.GetAllSlides();
            query = query.Where(x => x.Publish == true).ToList();

            var httpContextAccessor = _serviceProvider.GetRequiredService<IHttpContextAccessor>();
            string idCategory = Convert.ToString(httpContextAccessor.HttpContext.GetRouteValue("CategoryId"));
            if (!string.IsNullOrEmpty(idCategory))
            {
                query = query.Where(x => !string.IsNullOrEmpty(x.CategoryId) && (!string.IsNullOrEmpty(x.CategoryId) || x.CategoryId == idCategory)).ToList();
            }

            string idManufacturer = Convert.ToString(httpContextAccessor.HttpContext.GetRouteValue("ManufacturerId"));
            if (!string.IsNullOrEmpty(idManufacturer))
            {
                query = query.Where(x => !string.IsNullOrEmpty(x.ManufactureId) && (!string.IsNullOrEmpty(x.ManufactureId) || x.ManufactureId == idManufacturer)).ToList();
            }

            var slides = new List<PublicSlideModel>();
            foreach (var x in query.OrderBy(x => x.SlideOrder))
            {
                var y = x.ToModel(await GetPictureUrl(x.PictureId), _workContext.WorkingLanguage.Id);
                slides.Add(y);
            }

            var model = new PublicInfoModel()
            {
                Slides = slides
            };
            if (model.Slides.Count == 0)
                return Content("");


            return View("~/Plugins/Widgets.SwiperSlider/Views/WidgetsSwiperSlider/PublicInfo.cshtml", model);
        }
    }
}
