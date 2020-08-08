using Grand.Core;
using Grand.Core.Domain.Messages;
using Grand.Core.Plugins;
using Grand.Framework.Menu;
using Grand.Services.Cms;
using Grand.Services.Configuration;
using Grand.Services.Localization;
using Grand.Services.Messages;
using Grand.Services.Security;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grand.Plugin.Widgets.SwiperSlider
{
    /// <summary>
    /// PLugin
    /// </summary>
    public class SwiperSliderPlugin : BasePlugin, IWidgetPlugin, IAdminMenuPlugin
    {
        public static string pn = "Swiper Slider";

        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly ILocalizationService _localizationService;
        private readonly WidgetSwiperSliderSettings _widgetSwiperSliderSettings;
        private readonly IPermissionService _permissionService;
        private readonly IEmailAccountService _emailAccountService;
        private readonly EmailAccountSettings _emailAccountSettings;
        private readonly IStoreContext _storeContext;
        private readonly ILanguageService _languageService;
        private readonly IQueuedEmailService _queuedEmailService;

        public SwiperSliderPlugin(
            ISettingService settingService, IWebHelper webHelper,
            ILocalizationService localizationService,
            WidgetSwiperSliderSettings widgetSwiperSliderSettings,
            IPermissionService permissionService,
            IEmailAccountService emailAccountService,
            EmailAccountSettings emailAccountSettings,
            IStoreContext storeContext,
            ILanguageService languageService,
            IQueuedEmailService queuedEmailService)
        {
            _settingService = settingService;
            _localizationService = localizationService;
            _webHelper = webHelper;
            _widgetSwiperSliderSettings = widgetSwiperSliderSettings;
            _permissionService = permissionService;
            _emailAccountService = emailAccountService;
            _emailAccountSettings = emailAccountSettings;
            _storeContext = storeContext;
            _languageService = languageService;
            _queuedEmailService = queuedEmailService;
        }

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return _webHelper.GetStoreLocation() + "Admin/WidgetsSwiperSlider/Configure";
        }

        /// <summary>
        /// Gets widget zones where this widget should be rendered
        /// </summary>
        /// <returns>Widget zones</returns>
        public IList<string> GetWidgetZones()
        {
            List<string> _widgetZone = new List<string>();
            if (!string.IsNullOrEmpty(_widgetSwiperSliderSettings.widgetZoneOwn))
                _widgetZone.Add(_widgetSwiperSliderSettings.widgetZoneOwn);
            else
            {
                if (string.IsNullOrEmpty(_widgetSwiperSliderSettings.WidgetZone))
                    _widgetZone.Add("home_page_top");
                else
                    _widgetZone.Add(_widgetSwiperSliderSettings.WidgetZone);
            }
            if(!string.IsNullOrEmpty(_widgetSwiperSliderSettings.categoryWidgetZoneOwn))
                _widgetZone.Add(_widgetSwiperSliderSettings.categoryWidgetZoneOwn);
            else
            {
                if (!string.IsNullOrEmpty(_widgetSwiperSliderSettings.CateroryWidgetZone))
                    _widgetZone.Add(_widgetSwiperSliderSettings.CateroryWidgetZone);
            }

            if (!string.IsNullOrEmpty(_widgetSwiperSliderSettings.ManufacturerWidgetZoneOwn))
                _widgetZone.Add(_widgetSwiperSliderSettings.ManufacturerWidgetZoneOwn);
            else
            {
                if (!String.IsNullOrEmpty(_widgetSwiperSliderSettings.ManufacturerWidgetZone))
                    _widgetZone.Add(_widgetSwiperSliderSettings.ManufacturerWidgetZone);
            }

            return _widgetZone;
        }

        /// <summary>
        /// Gets a view component for displaying plugin in public store
        /// </summary>
        /// <param name="widgetZone">Name of the widget zone</param>
        /// <param name="viewComponentName">View component name</param>
        public void GetPublicViewComponent(string widgetZone, out string viewComponentName)
        {
            viewComponentName = "WidgetSwiperSliderPublicInfo";
            if (!String.IsNullOrEmpty(_widgetSwiperSliderSettings.CateroryWidgetZone))
                if (widgetZone.Contains(_widgetSwiperSliderSettings.CateroryWidgetZone))
                {
                    viewComponentName = "WidgetSwiperSliderPublicInfoCategoryManufacturer";
                }
            if (!String.IsNullOrEmpty(_widgetSwiperSliderSettings.ManufacturerWidgetZone))
                if (widgetZone.Contains(_widgetSwiperSliderSettings.ManufacturerWidgetZone))
                {
                    viewComponentName = "WidgetSwiperSliderPublicInfoCategoryManufacturer";
                }
        }


        /// <summary>
        /// Install plugin
        /// </summary>
        public override async Task Install()
        {

            //settings
            var settings = new WidgetSwiperSliderSettings()
            {
                showNavigation = false,
                showPagination = false,
                autoPlay = true,
                speed = 5000,
                loop = true,
                lazyLoading = false,
                preLoadImages = false,
                slidesPerView = 1,
                spaceBetween = 0,
                setMinHeight = 350

            };
            await _settingService.SaveSetting(settings);

             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.shownavigation", "Navigation");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.showpagination", "Pagination");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.autoplay", "Auto play");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.speed", "Speed");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.loop", "Loop");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.lazyloading", "Lazy loading");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.preloadimages", "Pre load images");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.widgetzone", "Widgetzone");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.caterorywidgetzone", "Widgetzone for category");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.manufacturerwidgetzones", "Widgetzone for manufacturer");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.header", "Header");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.body", "Body");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.footer", "Footer");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widgets.swiperslider.id", "ID");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widgets.swiperslider.picturetitle", "Picture title");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widgets.swiperslider.picturealt", "Picture alt");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widgets.swiperslider.header", "Header");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widgets.swiperslider.body", "Body");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widgets.swiperslider.footer", "Footer");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widgets.swiperslider.slideorder", "Order");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.shownavigation.hint", "Check to show navigation");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.showpagination.hint", "Check to show pagination");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.autoplay.hint", "Check to use autoplay");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.speed.hint", "Define slide speed");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.loop.hint", "Check to use loop");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.lazyloading.hint", "Check to use lazy loading for pictures");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.preloadimages.hint", "Check to pre-load images");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.widgetzone.hint", "Define widget zones");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.caterorywidgetzone.hint", "Define widget zone for category");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.manufacturerwidgetzones.hint", "Define widget zone for manufacturers");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.header.hint", "Header for slide");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.body.hint", "Context test for slide");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.footer.hint", "Footer for slide");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widgets.swiperslider.id.hint", "Id of picture");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widgets.swiperslider.picturetitle.hint", "Picture title tag text");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widgets.swiperslider.picturealt.hint", "Picture alt tag text");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widgets.swiperslider.header.hint", "Picture header");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widgets.swiperslider.body.hint", "Picture body");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widgets.swiperslider.footer.hint", "Picture footer");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widgets.swiperslider.slideorder.hint", "Define slider order");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.options", "Options");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.slideslist", "Slide list");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.advancedsettings", "Advanced");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.picture", "Picture");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.picturetitle", "Picture title");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.picturealt", "Picture alt");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.slideorder", "Slider order");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.header", "Header");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.addnew", "Add new");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.general", "General");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.stores", "Stores");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.picture", "Picture");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.slideorder", "Slider order");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.categoryid", "Category");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.manufacturerid", "Manufacturer");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.editslidedetails", "Edit slider details");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.limitedtostores", "Limit to stores");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.availablestores", "Available stores");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.usecountdown", "Countdown");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.usecountdown.hint", "Check to enabled countdown");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.slidesperview", "Slides per view");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.slidesperview.hint", "Define how many images should be vissible at once");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.spacebetween", "Space between");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.spacebetween.hint", "Defind space between images");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.widgetzoneown", "Own widget zone");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.categorywidgetzoneown", "Own category widget zone");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.manufacturerwidgetzoneown", "Own manufacturer widget zone");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.widgetzoneown.hint", "Type to use own widget zone");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.categorywidgetzoneown.hint", "Type to use own widget zone for category page");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.manufacturerwidgetzoneown.hint", "Type to use own widget zone for manufacturer page");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.headeranimation", "Header animation");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.headeranimationdelay", "Header animation delay");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.bodyanimation", "Body animation");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.bodyanimationdelay", "Body animation delay");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.footeranimation", "Footer animation");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.footeranimationdelay", "Footer animation delay");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.headeranimation.hint", "Choose header animation type");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.headeranimationdelay.hint", "Set header animation delay time");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.bodyanimation.hint", "Choose body animation type");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.bodyanimationdelay.hint", "Set body animation delay time");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.footeranimation.hint", "Choose footer animation type");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.footeranimationdelay.hint", "Set footer animation delay time");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.breakpoints", "Breakpoints");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.slidesperviewextrasmall", "Slide per view");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.spacebetweenextrasmall", "Space between");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.slidesperviewsmall", "Slide per view");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.spacebetweensmall", "Space between");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.slidesperviewmedium", "Slide per view");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.spacebetweenmedium", "Space between");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.slidesperviewlarge", "Slide per view");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.spacebetweenlarge", "Space between");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.slidesperviewextralarge", "Slide per view");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.spacebetweenextralarge", "Space between");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.breakpoints.hint", "Check to use breakpoints");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.slidesperviewextrasmall.hint", "Set slide per view for extra small view");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.spacebetweenextrasmall.hint", "Set space between for extra small view");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.slidesperviewsmall.hint", "Set slide per view for small view");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.spacebetweensmall.hint", "Set space between for small view");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.slidesperviewmedium.hint", "Set slide per view for medium view");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.spacebetweenmedium.hint", "Set space between for medium view");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.slidesperviewlarge.hint", "Set slide per view for large view");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.spacebetweenlarge.hint", "Set space between for large view");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.slidesperviewextralarge.hint", "Set slide per view for extra large view");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.spacebetweenextralarge.hint", "Set space between for extra large view");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.breakpoints.extrasmall", "Extra small");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.breakpoints.small", "Small");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.breakpoints.medium", "Medium");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.breakpoints.large", "Large");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.breakpoints.extralarge", "Extra large");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.SwiperSlider.widgetzonetab", "Widget zones");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.paginationtype", "Pagination type");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.paginationtype.hint", "Choose pagination type");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.SwiperSlider.picture", "Picture settings");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.caption", "Set caption");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.caption.hint", "Choose caption type");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.animations", "Animations");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.slides", "Configuration");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.animations.header", "Header section");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.animations.body", "Body section");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.animations.footer", "Footer section");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.turnoffextrasmall", "Disable");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.turnoffsmall", "Disable");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.turnoffmedium", "Disable");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.turnofflarge", "Disable");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.turnoffextralarge", "Disable");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.turnoffextrasmall.hint", "Check to disable for this device size");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.turnoffsmall.hint", "Check to disable for this device size");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.turnoffmedium.hint", "Check to disable for this device size");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.turnofflarge.hint", "Check to disable for this device size");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.turnoffextralarge.hint", "Check to disable for this device size");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.setminheight", "Set min height");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.setminheight.hint", "Set min height for container if not image added");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.publish", "Published");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.publish.hint", "Check to publish slide");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "admin.configuration.plugins.fields.publish", "Published");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.countdown", "Countdown type");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.countdown.hint", "Choose countdown type");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.SwiperSlider.Mappings", "Mappings");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.applyforhomepage", "Apply for home page");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.applyforhomepage.hint", "Check to apply this slide for homepage");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Admin.Configuration.Plugins.Fields.Homepage", "Homepage");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Admin.Configuration.Plugins.Fields.Category", "Category");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Admin.Configuration.Plugins.Fields.Manufacturer", "Manufacturer");
             await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.configuration", "Configuration");

            await CreateMessage();

            await base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override async Task Uninstall()
        {
            //settings
            await _settingService.DeleteSetting<WidgetSwiperSliderSettings>();

            //locales
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.shownavigation");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.showpagination");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.autoplay");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.speed");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.loop");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.lazyloading");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.preloadimages");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.widgetzone");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.caterorywidgetzone");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.manufacturerwidgetzones");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.header");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.body");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.footer");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widgets.swiperslider.id");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widgets.swiperslider.picturetitle");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widgets.swiperslider.picturealt");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widgets.swiperslider.header");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widgets.swiperslider.body");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widgets.swiperslider.footer");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widgets.swiperslider.slideorder");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.shownavigation.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.showpagination.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.autoplay.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.speed.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.loop.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.lazyloading.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.preloadimages.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.widgetzone.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.caterorywidgetzone.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.manufacturerwidgetzones.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.header.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.body.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.footer.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widgets.swiperslider.id.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widgets.swiperslider.picturetitle.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widgets.swiperslider.picturealt.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widgets.swiperslider.header.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widgets.swiperslider.body.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widgets.swiperslider.footer.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widgets.swiperslider.slideorder.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.options");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.slideslist");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.advancedsettings");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.picture");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.picturetitle");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.picturealt");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.slideorder");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.header");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.addnew");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.general");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.stores");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.picture");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.slideorder");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.categoryid");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.manufacturerid");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.editslidedetails");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.limitedtostores");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.availablestores");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.usecountdown");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.usecountdown.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.slidesperview");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.slidesperview.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.spacebetween");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.spacebetween.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.widgetzoneown");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.categorywidgetzoneown");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.manufacturerwidgetzoneown");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.widgetzoneown.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.categorywidgetzoneown.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.manufacturerwidgetzoneown.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.headeranimation");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.headeranimationdelay");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.bodyanimation");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.bodyanimationdelay");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.footeranimation");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.footeranimationdelay");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.headeranimation.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.headeranimationdelay.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.bodyanimation.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.bodyanimationdelay.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.footeranimation.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.footeranimationdelay.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.breakpoints");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.slidesperviewextrasmall");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.spacebetweenextrasmall");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.slidesperviewsmall");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.spacebetweensmall");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.slidesperviewmedium");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.spacebetweenmedium");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.slidesperviewlarge");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.spacebetweenlarge");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.slidesperviewextralarge");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.spacebetweenextralarge");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.breakpoints.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.slidesperviewextrasmall.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.spacebetweenextrasmall.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.slidesperviewsmall.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.spacebetweensmall.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.slidesperviewmedium.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.spacebetweenmedium.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.slidesperviewlarge.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.spacebetweenlarge.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.slidesperviewextralarge.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.spacebetweenextralarge.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.breakpoints.extrasmall");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.breakpoints.small");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.breakpoints.medium");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.breakpoints.large");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.breakpoints.extralarge");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.SwiperSlider.widgetzonetab");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.paginationtype");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.paginationtype.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.SwiperSlider.picture");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.caption");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.caption.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.animations");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.slides");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.animations.header");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.animations.body");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.animations.footer");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.turnoffextrasmall");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.turnoffsmall");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.turnoffmedium");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.turnofflarge");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.turnoffextralarge");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.turnoffextrasmall.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.turnoffsmall.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.turnoffmedium.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.turnofflarge.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.turnoffextralarge.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.setminheight");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.setminheight.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.publish");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.publish.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "admin.configuration.plugins.fields.publish");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.countdown");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.countdown.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.SwiperSlider.Mappings");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.applyforhomepage");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.fields.applyforhomepage.hint");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "Admin.Configuration.Plugins.Fields.Homepage");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "Admin.Configuration.Plugins.Fields.Category");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "Admin.Configuration.Plugins.Fields.Manufacturer");
            await this.DeletePluginLocaleResource(_localizationService, _languageService, "grand.widget.swiperslider.configuration");

            await CreateMessage(false);

            await base.Uninstall();
        }

        public string GetWidgetViewComponentName(string widgetZone)
        {
            string viewComponentName = "WidgetSwiperSliderPublicInfo";
            if (!string.IsNullOrEmpty(_widgetSwiperSliderSettings.CateroryWidgetZone))
                if (widgetZone.Contains(_widgetSwiperSliderSettings.CateroryWidgetZone))
                {
                    viewComponentName = "WidgetSwiperSliderPublicInfoCategoryManufacturer";
                }
            if (!string.IsNullOrEmpty(_widgetSwiperSliderSettings.ManufacturerWidgetZone))
                if (widgetZone.Contains(_widgetSwiperSliderSettings.ManufacturerWidgetZone))
                {
                    viewComponentName = "WidgetSwiperSliderPublicInfoCategoryManufacturer";
                }

            return viewComponentName;
            
        }

        #region Sitemap && notifications

        public async Task ManageSiteMap(SiteMapNode rootNode)
        {
            if (await _permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
            {
                var pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "extensions");
                if (pluginNode == null)
                {
                    rootNode.ChildNodes.Add(new SiteMapNode() {
                        SystemName = "extensions",
                        Title = "Extensions",
                        Visible = true,
                        IconClass = "icon-paper-clip"
                    });
                    pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "extensions");
                }

                SiteMapNode Menu = new SiteMapNode();
                Menu.Title = "Swiper Slider";
                Menu.Visible = true;
                Menu.SystemName = "SwiperSlider";

                Menu.ChildNodes.Add(new SiteMapNode() {
                    Title = _localizationService.GetResource("grand.widget.swiperslider.configuration"),
                    Visible = true,
                    SystemName = "SwiperSlider.Settings",
                    IconClass = "",
                    ControllerName = "WidgetsSwiperSlider",
                    ActionName = "Configure",
                    RouteValues = new RouteValueDictionary() { { "area", "Admin" } },
                });
                Menu.ChildNodes.Add(new SiteMapNode() {
                    Title = "Support",
                    Visible = true,
                    SystemName = "SwiperSlider.Support",
                    IconClass = "",
                    Url = "https://grandnode.com/boards/forum/59f6f028ba1646279c61a5e4/community-plugins-themes"
                });

                if (pluginNode != null)
                    pluginNode.ChildNodes.Add(Menu);
                else
                    rootNode.ChildNodes.Add(Menu);
            }
        }

        private async Task CreateMessage(bool install = true)
        {
            var emailAccount = await _emailAccountService.GetEmailAccountById(_emailAccountSettings.DefaultEmailAccountId);
            await _queuedEmailService.InsertQueuedEmail(new QueuedEmail() {
                CreatedOnUtc = DateTime.UtcNow,
                EmailAccountId = _emailAccountSettings.DefaultEmailAccountId,
                Priority = QueuedEmailPriority.Low,
                Subject = $"Plugin {(install ? "installation" : "uninstall")} notification {pn}",
                Body = $"<p><a href=\"{_storeContext.CurrentStore.Url}\">{_storeContext.CurrentStore.Name}</a>&nbsp;</p><p>Plugin {pn} has been installed</p>",
                To = "support@nop4you.com",
                ToName = "Support nop4you",
                From = emailAccount.Email,
                FromName = emailAccount.DisplayName
            });
        }


        #endregion
    }
}