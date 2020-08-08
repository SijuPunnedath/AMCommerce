using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Grand.Services.Stores;
using System.Collections.Generic;
using System.Linq;
using Grand.Framework.Mvc.Filters;
using Grand.Framework.Controllers;
using Grand.Core;
using Grand.Services.Localization;
using Grand.Services.Media;
using Grand.Services.Configuration;
using Grand.Core.Caching;
using Grand.Plugin.Widgets.SwiperSlider.Models;
using Grand.Plugin.Widgets.SwiperSlider.Infrastructure.Cache;
using System.Threading.Tasks;

namespace Grand.Plugin.Widgets.SwiperSlider.Controllers
{
    [AuthorizeAdmin]
    [Area("Admin")]
    public class WidgetsSwiperSliderController : BasePluginController
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreService _storeService;
        private readonly ILocalizationService _localizationService;
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly ICacheManager _cacheManager;

        public WidgetsSwiperSliderController(IWorkContext workContext,
            IStoreService storeService,
            IPictureService pictureService,
            ILocalizationService localizationService,
            ISettingService settingService,
            ICacheManager cacheManager)
        {
            _workContext = workContext;
            _storeService = storeService;
            _pictureService = pictureService;
            _localizationService = localizationService;
            _settingService = settingService;
            _cacheManager = cacheManager;
        }

        protected async Task<string> GetPictureUrl(string pictureId)
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

        [AuthorizeAdmin]
        public async Task<IActionResult> Configure()
        {
            //var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var storeScope = await this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var sliderSettings = _settingService.LoadSetting<WidgetSwiperSliderSettings>(storeScope);
            var model = new ConfigurationModel();
            model.autoPlay = sliderSettings.autoPlay;
            model.setMinHeight = sliderSettings.setMinHeight;
            model.CateroryWidgetZone = sliderSettings.CateroryWidgetZone;
            model.WidgetZone = sliderSettings.WidgetZone;
            model.ManufacturerWidgetZone = sliderSettings.ManufacturerWidgetZone;
            model.showNavigation = sliderSettings.showNavigation;
            model.showPagination = sliderSettings.showPagination;
            model.paginationType = sliderSettings.paginationType;
            model.speed = sliderSettings.speed;
            model.loop = sliderSettings.loop;
            model.lazyLoading = sliderSettings.lazyLoading;
            model.preLoadImages = sliderSettings.preLoadImages;
            model.usecountdown = sliderSettings.usecountdown;
            model.countdown = sliderSettings.countdown;
            model.slidesPerView = sliderSettings.slidesPerView;
            model.spaceBetween = sliderSettings.spaceBetween;
            model.widgetZoneOwn = sliderSettings.widgetZoneOwn;
            model.categoryWidgetZoneOwn = sliderSettings.categoryWidgetZoneOwn;
            model.ManufacturerWidgetZoneOwn = sliderSettings.ManufacturerWidgetZoneOwn;

            model.breakpoints = sliderSettings.breakpoints;
            model.slidesPerViewExtraSmall = sliderSettings.slidesPerViewExtraSmall;
            model.spaceBetweenExtraSmall = sliderSettings.spaceBetweenExtraSmall;
            model.slidesPerViewSmall = sliderSettings.slidesPerViewSmall;
            model.spaceBetweenSmall = sliderSettings.spaceBetweenSmall;
            model.slidesPerViewMedium = sliderSettings.slidesPerViewMedium;
            model.spaceBetweenMedium = sliderSettings.spaceBetweenMedium;
            model.slidesPerViewLarge = sliderSettings.slidesPerViewLarge;
            model.spaceBetweenLarge = sliderSettings.spaceBetweenLarge;
            model.slidesPerViewExtraLarge = sliderSettings.slidesPerViewExtraLarge;
            model.spaceBetweenExtraLarge = sliderSettings.spaceBetweenExtraLarge;

            model.turnOffExtraSmall = sliderSettings.turnOffExtraSmall;
            model.turnOffSmall = sliderSettings.turnOffSmall;
            model.turnOffMedium = sliderSettings.turnOffMedium;
            model.turnOffLarge = sliderSettings.turnOffLarge;
            model.turnOffExtraLarge = sliderSettings.turnOffExtraLarge;

            if (!string.IsNullOrEmpty(storeScope))
            {
                model.autoPlay_OverrideForStore = _settingService.SettingExists(sliderSettings, x => x.autoPlay, storeScope);
                model.CateroryWidgetZone_OverrideForStore = _settingService.SettingExists(sliderSettings, x => x.CateroryWidgetZone, storeScope);
                model.WidgetZone_OverrideForStore = _settingService.SettingExists(sliderSettings, x => x.WidgetZone, storeScope);
                model.ManufacturerWidgetZone_OverrideForStore = _settingService.SettingExists(sliderSettings, x => x.ManufacturerWidgetZone, storeScope);
                model.showNavigation_OverrideForStore = _settingService.SettingExists(sliderSettings, x => x.showNavigation, storeScope);
                model.showPagination_OverrideForStore = _settingService.SettingExists(sliderSettings, x => x.showPagination, storeScope);
                model.paginationType_OverrideForStore = _settingService.SettingExists(sliderSettings, x => x.paginationType, storeScope);
                model.speed_OverrideForStore = _settingService.SettingExists(sliderSettings, x => x.speed, storeScope);
                model.loop_OverrideForStore = _settingService.SettingExists(sliderSettings, x => x.loop, storeScope);
                model.lazyLoading_OverrideForStore = _settingService.SettingExists(sliderSettings, x => x.lazyLoading, storeScope);
                model.preLoadImages_OverrideForStore = _settingService.SettingExists(sliderSettings, x => x.preLoadImages, storeScope);
                model.usecountdown_OverrideForStore = _settingService.SettingExists(sliderSettings, x => x.usecountdown, storeScope);
                model.countdown_OverrideForStore = _settingService.SettingExists(sliderSettings, x => x.countdown, storeScope);
                model.slidesPerView_OverrideForStore= _settingService.SettingExists(sliderSettings, x => x.slidesPerView, storeScope);
                model.spaceBetween_OverrideForStore = _settingService.SettingExists(sliderSettings, x => x.spaceBetween, storeScope);
                model.widgetZoneOwn_OverrideForStore = _settingService.SettingExists(sliderSettings, x => x.widgetZoneOwn, storeScope);
                model.categoryWidgetZoneOwn_OverrideForStore = _settingService.SettingExists(sliderSettings, x => x.categoryWidgetZoneOwn, storeScope);
                model.ManufacturerWidgetZoneOwn_OverrideForStore = _settingService.SettingExists(sliderSettings, x => x.ManufacturerWidgetZoneOwn, storeScope);
                model.setMinHeight_OverrideForStore = _settingService.SettingExists(sliderSettings, x => x.setMinHeight, storeScope);

                model.breakpoints_OverrideForStore = _settingService.SettingExists(sliderSettings, x => x.breakpoints, storeScope);
                model.slidesPerViewExtraSmall_OverrideForStore = _settingService.SettingExists(sliderSettings, x => x.slidesPerViewExtraSmall, storeScope);
                model.spaceBetweenExtraSmall_OverrideForStore = _settingService.SettingExists(sliderSettings, x => x.spaceBetweenExtraSmall, storeScope);
                model.slidesPerViewSmall_OverrideForStore = _settingService.SettingExists(sliderSettings, x => x.slidesPerViewSmall, storeScope);
                model.spaceBetweenSmall_OverrideForStore = _settingService.SettingExists(sliderSettings, x => x.spaceBetweenSmall, storeScope);
                model.slidesPerViewMedium_OverrideForStore = _settingService.SettingExists(sliderSettings, x => x.slidesPerViewMedium, storeScope);
                model.spaceBetweenMedium_OverrideForStore = _settingService.SettingExists(sliderSettings, x => x.spaceBetweenMedium, storeScope);
                model.slidesPerViewLarge_OverrideForStore = _settingService.SettingExists(sliderSettings, x => x.slidesPerViewLarge, storeScope);
                model.spaceBetweenLarge_OverrideForStore = _settingService.SettingExists(sliderSettings, x => x.spaceBetweenLarge, storeScope);
                model.slidesPerViewExtraLarge_OverrideForStore = _settingService.SettingExists(sliderSettings, x => x.slidesPerViewExtraLarge, storeScope);
                model.spaceBetweenExtraLarge_OverrideForStore = _settingService.SettingExists(sliderSettings, x => x.spaceBetweenExtraLarge, storeScope);

                model.turnOffExtraSmall_OverrideForStore = _settingService.SettingExists(sliderSettings, x => x.turnOffExtraSmall, storeScope);
                model.turnOffSmall_OverrideForStore = _settingService.SettingExists(sliderSettings, x => x.turnOffSmall, storeScope);
                model.turnOffMedium_OverrideForStore = _settingService.SettingExists(sliderSettings, x => x.turnOffMedium, storeScope);
                model.turnOffLarge_OverrideForStore = _settingService.SettingExists(sliderSettings, x => x.turnOffLarge, storeScope);
                model.turnOffExtraLarge_OverrideForStore = _settingService.SettingExists(sliderSettings, x => x.turnOffExtraLarge, storeScope);
            }

            var widgetZones = new Dictionary<string, string>(){
                {"home_page_top","home_page_top"},
                {"home_page_bottom", "home_page_bottom"},
                {"content_before","content_before"},
                {"content_after","content_after"},
                {"main_column_before", "main_column_before"},
                {"main_column_after", "main_column_after"}
            };

            model.WidgetZones = widgetZones.Select(x => new SelectListItem() { Value = x.Value, Text = x.Key, Selected = x.Key == model.WidgetZone }).ToList();

            var cwidgetZones = new Dictionary<string, string>(){
                {"none",""},
                {"categorydetails_top","categorydetails_top"},
                {"categorydetails_before_subcategories", "categorydetails_before_subcategories"},
                {"categorydetails_before_featured_products", "categorydetails_before_featured_products"},
                {"categorydetails_after_featured_products", "categorydetails_after_featured_products"},
                {"categorydetails_before_filters", "categorydetails_before_filters"},
                {"categorydetails_before_product_list","categorydetails_before_product_list"},
                {"categorydetails_bottom", "categorydetails_bottom"}
            };

            var mwidgetZones = new Dictionary<string, string>(){
                {"none",""},
                {"manufacturerdetails_top","manufacturerdetails_top"},
                {"manufacturerdetails_before_featured_products", "manufacturerdetails_before_featured_products"},
                {"manufacturerdetails_before_filters", "manufacturerdetails_before_filters"},
                {"manufacturerdetails_before_product_list","manufacturerdetails_before_product_list"},
                {"manufacturerdetails_bottom", "manufacturerdetails_bottom"}
            };

            model.CateroryWidgetZones = cwidgetZones.Select(x => new SelectListItem() { Value = x.Value, Text = x.Key, Selected = x.Key == model.CateroryWidgetZone }).ToList();
            model.ManufacturerWidgetZones = mwidgetZones.Select(x => new SelectListItem() { Value = x.Value, Text = x.Key, Selected = x.Key == model.ManufacturerWidgetZone }).ToList();


            return View("~/Plugins/Widgets.SwiperSlider/Views/WidgetsSwiperSlider/Configure.cshtml", model);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            var storeScope = await this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var sliderSettings = _settingService.LoadSetting<WidgetSwiperSliderSettings>(storeScope);
            sliderSettings.showNavigation = model.showNavigation;
            sliderSettings.showPagination = model.showPagination;
            sliderSettings.paginationType = model.paginationType;
            sliderSettings.autoPlay = model.autoPlay;
            sliderSettings.speed = model.speed;
            sliderSettings.loop = model.loop;
            sliderSettings.lazyLoading = model.lazyLoading;
            sliderSettings.preLoadImages = model.preLoadImages;
            sliderSettings.WidgetZone = model.WidgetZone;
            sliderSettings.CateroryWidgetZone = model.CateroryWidgetZone;
            sliderSettings.ManufacturerWidgetZone = model.ManufacturerWidgetZone;
            sliderSettings.usecountdown = model.usecountdown;
            sliderSettings.countdown = model.countdown;
            sliderSettings.slidesPerView = model.slidesPerView;
            sliderSettings.spaceBetween = model.spaceBetween;
            sliderSettings.widgetZoneOwn = model.widgetZoneOwn;
            sliderSettings.categoryWidgetZoneOwn = model.categoryWidgetZoneOwn;
            sliderSettings.ManufacturerWidgetZoneOwn = model.ManufacturerWidgetZoneOwn;
            sliderSettings.setMinHeight = model.setMinHeight;

            sliderSettings.breakpoints = model.breakpoints;
            sliderSettings.slidesPerViewExtraSmall = model.slidesPerViewExtraSmall;
            sliderSettings.spaceBetweenExtraSmall = model.spaceBetweenExtraSmall;
            sliderSettings.slidesPerViewSmall = model.slidesPerViewSmall;
            sliderSettings.spaceBetweenSmall = model.spaceBetweenSmall;
            sliderSettings.slidesPerViewMedium = model.slidesPerViewMedium;
            sliderSettings.spaceBetweenMedium = model.spaceBetweenMedium;
            sliderSettings.slidesPerViewLarge = model.slidesPerViewLarge;
            sliderSettings.spaceBetweenLarge = model.spaceBetweenLarge;
            sliderSettings.slidesPerViewExtraLarge = model.slidesPerViewExtraLarge;
            sliderSettings.spaceBetweenExtraLarge = model.spaceBetweenExtraLarge;

            sliderSettings.turnOffExtraSmall = model.turnOffExtraSmall;
            sliderSettings.turnOffSmall = model.turnOffSmall;
            sliderSettings.turnOffMedium = model.turnOffMedium;
            sliderSettings.turnOffLarge = model.turnOffLarge;
            sliderSettings.turnOffExtraLarge = model.turnOffExtraLarge;

            if (model.showNavigation_OverrideForStore || string.IsNullOrEmpty(storeScope))
                await _settingService.SaveSetting(sliderSettings, x => x.showNavigation, storeScope, false);
            else if (!string.IsNullOrEmpty(storeScope))
                await _settingService.DeleteSetting(sliderSettings, x => x.showNavigation, storeScope);
            if (model.showPagination_OverrideForStore || string.IsNullOrEmpty(storeScope))
                await _settingService.SaveSetting(sliderSettings, x => x.showPagination, storeScope, false);
            else if (!string.IsNullOrEmpty(storeScope))
                await _settingService.DeleteSetting(sliderSettings, x => x.showPagination, storeScope);
            if (model.paginationType_OverrideForStore || string.IsNullOrEmpty(storeScope))
                await _settingService.SaveSetting(sliderSettings, x => x.paginationType, storeScope, false);
            else if (!string.IsNullOrEmpty(storeScope))
                await _settingService.DeleteSetting(sliderSettings, x => x.paginationType, storeScope);
            if (model.autoPlay_OverrideForStore || string.IsNullOrEmpty(storeScope))
                await _settingService.SaveSetting(sliderSettings, x => x.autoPlay, storeScope, false);
            else if (!string.IsNullOrEmpty(storeScope))
                await _settingService.DeleteSetting(sliderSettings, x => x.autoPlay, storeScope);
            if (model.speed_OverrideForStore || string.IsNullOrEmpty(storeScope))
                await _settingService.SaveSetting(sliderSettings, x => x.speed, storeScope, false);
            else if (!string.IsNullOrEmpty(storeScope))
                await _settingService.DeleteSetting(sliderSettings, x => x.speed, storeScope);
            if (model.loop_OverrideForStore || string.IsNullOrEmpty(storeScope))
                await _settingService.SaveSetting(sliderSettings, x => x.loop, storeScope, false);
            else if (!string.IsNullOrEmpty(storeScope))
                await _settingService.DeleteSetting(sliderSettings, x => x.loop, storeScope);
            if (model.lazyLoading_OverrideForStore || string.IsNullOrEmpty(storeScope))
                await _settingService.SaveSetting(sliderSettings, x => x.lazyLoading, storeScope, false);
            else if (!string.IsNullOrEmpty(storeScope))
                await _settingService.DeleteSetting(sliderSettings, x => x.lazyLoading, storeScope);
            if (model.preLoadImages_OverrideForStore || string.IsNullOrEmpty(storeScope))
                await _settingService.SaveSetting(sliderSettings, x => x.preLoadImages, storeScope, false);
            else if (!string.IsNullOrEmpty(storeScope))
                await _settingService.DeleteSetting(sliderSettings, x => x.preLoadImages, storeScope);
            if (model.WidgetZone_OverrideForStore || string.IsNullOrEmpty(storeScope))
                await _settingService.SaveSetting(sliderSettings, x => x.WidgetZone, storeScope, false);
            else if (!string.IsNullOrEmpty(storeScope))
                await _settingService.DeleteSetting(sliderSettings, x => x.WidgetZone, storeScope);
            if (model.CateroryWidgetZone_OverrideForStore || string.IsNullOrEmpty(storeScope))
                await _settingService.SaveSetting(sliderSettings, x => x.CateroryWidgetZone, storeScope, false);
            else if (!string.IsNullOrEmpty(storeScope))
                await _settingService.DeleteSetting(sliderSettings, x => x.CateroryWidgetZone, storeScope);
            if (model.ManufacturerWidgetZone_OverrideForStore || string.IsNullOrEmpty(storeScope))
                await _settingService.SaveSetting(sliderSettings, x => x.ManufacturerWidgetZone, storeScope, false);
            else if (!string.IsNullOrEmpty(storeScope))
                await _settingService.DeleteSetting(sliderSettings, x => x.ManufacturerWidgetZone, storeScope);
            if (model.setMinHeight_OverrideForStore || string.IsNullOrEmpty(storeScope))
                await _settingService.SaveSetting(sliderSettings, x => x.setMinHeight, storeScope, false);
            else if (!string.IsNullOrEmpty(storeScope))
                await _settingService.DeleteSetting(sliderSettings, x => x.setMinHeight, storeScope);

            if (model.usecountdown_OverrideForStore || string.IsNullOrEmpty(storeScope))
                await _settingService.SaveSetting(sliderSettings, x => x.usecountdown, storeScope, false);
            else if (!string.IsNullOrEmpty(storeScope))
                await _settingService.DeleteSetting(sliderSettings, x => x.usecountdown, storeScope);
            if (model.countdown_OverrideForStore || string.IsNullOrEmpty(storeScope))
                await _settingService.SaveSetting(sliderSettings, x => x.countdown, storeScope, false);
            else if (!string.IsNullOrEmpty(storeScope))
                await _settingService.DeleteSetting(sliderSettings, x => x.countdown, storeScope);

            if (model.slidesPerView_OverrideForStore || string.IsNullOrEmpty(storeScope))
                await _settingService.SaveSetting(sliderSettings, x => x.slidesPerView, storeScope, false);
            else if (!string.IsNullOrEmpty(storeScope))
                await _settingService.DeleteSetting(sliderSettings, x => x.slidesPerView, storeScope);

            if (model.spaceBetween_OverrideForStore || string.IsNullOrEmpty(storeScope))
                await _settingService.SaveSetting(sliderSettings, x => x.spaceBetween, storeScope, false);
            else if (!string.IsNullOrEmpty(storeScope))
                await _settingService.DeleteSetting(sliderSettings, x => x.spaceBetween, storeScope);

            if (model.widgetZoneOwn_OverrideForStore || string.IsNullOrEmpty(storeScope))
                await _settingService.SaveSetting(sliderSettings, x => x.widgetZoneOwn, storeScope, false);
            else if (!string.IsNullOrEmpty(storeScope))
                await _settingService.DeleteSetting(sliderSettings, x => x.widgetZoneOwn, storeScope);
            if (model.categoryWidgetZoneOwn_OverrideForStore || string.IsNullOrEmpty(storeScope))
                await _settingService.SaveSetting(sliderSettings, x => x.categoryWidgetZoneOwn, storeScope, false);
            else if (!string.IsNullOrEmpty(storeScope))
                await _settingService.DeleteSetting(sliderSettings, x => x.categoryWidgetZoneOwn, storeScope);
            if (model.ManufacturerWidgetZoneOwn_OverrideForStore || string.IsNullOrEmpty(storeScope))
                await _settingService.SaveSetting(sliderSettings, x => x.ManufacturerWidgetZoneOwn, storeScope, false);
            else if (!string.IsNullOrEmpty(storeScope))
                await _settingService.DeleteSetting(sliderSettings, x => x.ManufacturerWidgetZoneOwn, storeScope);


            if (model.breakpoints_OverrideForStore || string.IsNullOrEmpty(storeScope))
                await _settingService.SaveSetting(sliderSettings, x => x.breakpoints, storeScope, false);
            else if (!string.IsNullOrEmpty(storeScope))
                await _settingService.DeleteSetting(sliderSettings, x => x.breakpoints, storeScope);

            if (model.slidesPerViewExtraSmall_OverrideForStore || string.IsNullOrEmpty(storeScope))
                await _settingService.SaveSetting(sliderSettings, x => x.slidesPerViewExtraSmall, storeScope, false);
            else if (!string.IsNullOrEmpty(storeScope))
                await _settingService.DeleteSetting(sliderSettings, x => x.slidesPerViewExtraSmall, storeScope);

            if (model.spaceBetweenExtraSmall_OverrideForStore || string.IsNullOrEmpty(storeScope))
                await _settingService.SaveSetting(sliderSettings, x => x.spaceBetweenExtraSmall, storeScope, false);
            else if (!string.IsNullOrEmpty(storeScope))
                await _settingService.DeleteSetting(sliderSettings, x => x.spaceBetweenExtraSmall, storeScope);

            if (model.slidesPerViewSmall_OverrideForStore || string.IsNullOrEmpty(storeScope))
                await _settingService.SaveSetting(sliderSettings, x => x.slidesPerViewSmall, storeScope, false);
            else if (!string.IsNullOrEmpty(storeScope))
                await _settingService.DeleteSetting(sliderSettings, x => x.slidesPerViewSmall, storeScope);

            if (model.spaceBetweenSmall_OverrideForStore || string.IsNullOrEmpty(storeScope))
                await _settingService.SaveSetting(sliderSettings, x => x.spaceBetweenSmall, storeScope, false);
            else if (!string.IsNullOrEmpty(storeScope))
                await _settingService.DeleteSetting(sliderSettings, x => x.spaceBetweenSmall, storeScope);

            if (model.slidesPerViewMedium_OverrideForStore || string.IsNullOrEmpty(storeScope))
                await _settingService.SaveSetting(sliderSettings, x => x.slidesPerViewMedium, storeScope, false);
            else if (!string.IsNullOrEmpty(storeScope))
                await _settingService.DeleteSetting(sliderSettings, x => x.slidesPerViewMedium, storeScope);

            if (model.spaceBetweenMedium_OverrideForStore || string.IsNullOrEmpty(storeScope))
                await _settingService.SaveSetting(sliderSettings, x => x.spaceBetweenMedium, storeScope, false);
            else if (!string.IsNullOrEmpty(storeScope))
                await _settingService.DeleteSetting(sliderSettings, x => x.spaceBetweenMedium, storeScope);

            if (model.slidesPerViewLarge_OverrideForStore || string.IsNullOrEmpty(storeScope))
                await _settingService.SaveSetting(sliderSettings, x => x.slidesPerViewLarge, storeScope, false);
            else if (!string.IsNullOrEmpty(storeScope))
                await _settingService.DeleteSetting(sliderSettings, x => x.slidesPerViewLarge, storeScope);

            if (model.spaceBetweenLarge_OverrideForStore || string.IsNullOrEmpty(storeScope))
                await _settingService.SaveSetting(sliderSettings, x => x.spaceBetweenLarge, storeScope, false);
            else if (!string.IsNullOrEmpty(storeScope))
                await _settingService.DeleteSetting(sliderSettings, x => x.spaceBetweenLarge, storeScope);

            if (model.slidesPerViewExtraLarge_OverrideForStore || string.IsNullOrEmpty(storeScope))
                await _settingService.SaveSetting(sliderSettings, x => x.slidesPerViewExtraLarge, storeScope, false);
            else if (!string.IsNullOrEmpty(storeScope))
                await _settingService.DeleteSetting(sliderSettings, x => x.slidesPerViewExtraLarge, storeScope);

            if (model.spaceBetweenExtraLarge_OverrideForStore || string.IsNullOrEmpty(storeScope))
                await _settingService.SaveSetting(sliderSettings, x => x.spaceBetweenExtraLarge, storeScope, false);
            else if (!string.IsNullOrEmpty(storeScope))
                await _settingService.DeleteSetting(sliderSettings, x => x.spaceBetweenExtraLarge, storeScope);


            if (model.turnOffExtraSmall_OverrideForStore || string.IsNullOrEmpty(storeScope))
                await _settingService.SaveSetting(sliderSettings, x => x.turnOffExtraSmall, storeScope, false);
            else if (!string.IsNullOrEmpty(storeScope))
                await _settingService.DeleteSetting(sliderSettings, x => x.turnOffExtraSmall, storeScope);
            if (model.turnOffSmall_OverrideForStore || string.IsNullOrEmpty(storeScope))
                await _settingService.SaveSetting(sliderSettings, x => x.turnOffSmall, storeScope, false);
            else if (!string.IsNullOrEmpty(storeScope))
                await _settingService.DeleteSetting(sliderSettings, x => x.turnOffSmall, storeScope);
            if (model.turnOffMedium_OverrideForStore || string.IsNullOrEmpty(storeScope))
                await _settingService.SaveSetting(sliderSettings, x => x.turnOffMedium, storeScope, false);
            else if (!string.IsNullOrEmpty(storeScope))
                await _settingService.DeleteSetting(sliderSettings, x => x.turnOffMedium, storeScope);
            if (model.turnOffLarge_OverrideForStore || string.IsNullOrEmpty(storeScope))
                await _settingService.SaveSetting(sliderSettings, x => x.turnOffLarge, storeScope, false);
            else if (!string.IsNullOrEmpty(storeScope))
                await _settingService.DeleteSetting(sliderSettings, x => x.turnOffLarge, storeScope);
            if (model.turnOffExtraLarge_OverrideForStore || string.IsNullOrEmpty(storeScope))
                await _settingService.SaveSetting(sliderSettings, x => x.turnOffExtraLarge, storeScope, false);
            else if (!string.IsNullOrEmpty(storeScope))
                await _settingService.DeleteSetting(sliderSettings, x => x.turnOffExtraLarge, storeScope);

            await _settingService.ClearCache();
            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return await Configure();
            
        }
    }
}