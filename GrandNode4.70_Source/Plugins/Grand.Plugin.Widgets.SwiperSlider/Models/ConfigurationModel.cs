using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Grand.Framework.Mvc.Models;
using Grand.Framework.Mvc.ModelBinding;

namespace Grand.Plugin.Widgets.SwiperSlider.Models
{
    public class ConfigurationModel : BaseGrandModel
    {
        public ConfigurationModel()
        {
            WidgetZones = new List<SelectListItem>();
            CateroryWidgetZones = new List<SelectListItem>();
            ManufacturerWidgetZones = new List<SelectListItem>();
            PaginationTypes = new List<SelectListItem>();
            CountDownTypes = new List<SelectListItem>();

            PaginationTypes.Add(new SelectListItem() { Text = "bullets", Value = "bullets" });
            PaginationTypes.Add(new SelectListItem() { Text = "fraction", Value = "fraction" });
            PaginationTypes.Add(new SelectListItem() { Text = "progressbar", Value = "progressbar" });
            //PaginationTypes.Add(new SelectListItem() { Text = "custom", Value = "custom" });

            CountDownTypes.Add(new SelectListItem() { Text = "bar", Value = "bar" });
            CountDownTypes.Add(new SelectListItem() { Text = "circle", Value = "circle" });
        }

        public int ActiveStoreScopeConfiguration { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.shownavigation")]
        public bool showNavigation { get; set; }
        public bool showNavigation_OverrideForStore { get; set; }
        [GrandResourceDisplayName("grand.widget.swiperslider.showpagination")]
        public bool showPagination { get; set; }
        public bool showPagination_OverrideForStore { get; set; }
        [GrandResourceDisplayName("grand.widget.swiperslider.fields.paginationtype")]
        public string paginationType { get; set; }
        public bool paginationType_OverrideForStore { get; set; }
        public List<SelectListItem> PaginationTypes { get; set; }
        [GrandResourceDisplayName("grand.widget.swiperslider.autoplay")]
        public bool autoPlay { get; set; }
        public bool autoPlay_OverrideForStore { get; set; }
        [GrandResourceDisplayName("grand.widget.swiperslider.setminheight")]
        public int setMinHeight { get; set; }
        public bool setMinHeight_OverrideForStore { get; set; }
        [GrandResourceDisplayName("grand.widget.swiperslider.speed")]
        public int speed { get; set; }
        public bool speed_OverrideForStore { get; set; }
        [GrandResourceDisplayName("grand.widget.swiperslider.loop")]
        public bool loop { get; set; }
        public bool loop_OverrideForStore { get; set; }
        [GrandResourceDisplayName("grand.widget.swiperslider.lazyloading")]
        public bool lazyLoading { get; set; }
        public bool lazyLoading_OverrideForStore { get; set; }
        [GrandResourceDisplayName("grand.widget.swiperslider.preloadimages")]
        public bool preLoadImages { get; set; }
        public bool preLoadImages_OverrideForStore { get; set; }
        [GrandResourceDisplayName("grand.widget.swiperslider.widgetzone")]
        public string WidgetZone { get; set; }
        public bool WidgetZone_OverrideForStore { get; set; }
        public List<SelectListItem> WidgetZones { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.widgetzoneown")]
        public string widgetZoneOwn { get; set; }
        public bool widgetZoneOwn_OverrideForStore { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.categorywidgetzoneown")]
        public string categoryWidgetZoneOwn { get; set; }
        public bool categoryWidgetZoneOwn_OverrideForStore { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.manufacturerwidgetzoneown")]
        public string ManufacturerWidgetZoneOwn { get; set; }
        public bool ManufacturerWidgetZoneOwn_OverrideForStore { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.caterorywidgetzone")]
        public string CateroryWidgetZone { get; set; }
        public bool CateroryWidgetZone_OverrideForStore { get; set; }
        public List<SelectListItem> CateroryWidgetZones { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.manufacturerwidgetzones")]
        public string ManufacturerWidgetZone { get; set; }
        public bool ManufacturerWidgetZone_OverrideForStore { get; set; }
        public List<SelectListItem> ManufacturerWidgetZones { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.usecountdown")]
        public bool usecountdown { get; set; }
        public bool usecountdown_OverrideForStore { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.countdown")]
        public string countdown { get; set; }
        public bool countdown_OverrideForStore { get; set; }
        public List<SelectListItem> CountDownTypes { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.slidesperview")]
        public int slidesPerView { get; set; }
        public bool slidesPerView_OverrideForStore { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.spacebetween")]
        public int spaceBetween { get; set; }
        public bool spaceBetween_OverrideForStore { get; set; }


        // braakpoints
        #region breakpoints
        [GrandResourceDisplayName("grand.widget.swiperslider.breakpoints")]
        public bool breakpoints { get; set; }
        public bool breakpoints_OverrideForStore { get; set; }

        #region extra small
        [GrandResourceDisplayName("grand.widget.swiperslider.slidesperviewextrasmall")]
        public int slidesPerViewExtraSmall { get; set; }
        public bool slidesPerViewExtraSmall_OverrideForStore { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.spacebetweenextrasmall")]
        public int spaceBetweenExtraSmall { get; set; }
        public bool spaceBetweenExtraSmall_OverrideForStore { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.turnoffextrasmall")]
        public bool turnOffExtraSmall { get; set; }
        public bool turnOffExtraSmall_OverrideForStore { get; set; }

        #endregion

        #region small
        [GrandResourceDisplayName("grand.widget.swiperslider.slidesperviewsmall")]
        public int slidesPerViewSmall { get; set; }
        public bool slidesPerViewSmall_OverrideForStore { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.spacebetweensmall")]
        public int spaceBetweenSmall { get; set; }
        public bool spaceBetweenSmall_OverrideForStore { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.turnoffsmall")]
        public bool turnOffSmall { get; set; }
        public bool turnOffSmall_OverrideForStore { get; set; }


        #endregion

        #region medium
        [GrandResourceDisplayName("grand.widget.swiperslider.slidesperviewmedium")]
        public int slidesPerViewMedium { get; set; }
        public bool slidesPerViewMedium_OverrideForStore { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.spacebetweenmedium")]
        public int spaceBetweenMedium { get; set; }
        public bool spaceBetweenMedium_OverrideForStore { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.turnoffmedium")]
        public bool turnOffMedium { get; set; }
        public bool turnOffMedium_OverrideForStore { get; set; }

        #endregion

        #region large
        [GrandResourceDisplayName("grand.widget.swiperslider.slidesperviewlarge")]
        public int slidesPerViewLarge { get; set; }
        public bool slidesPerViewLarge_OverrideForStore { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.spacebetweenlarge")]
        public int spaceBetweenLarge { get; set; }
        public bool spaceBetweenLarge_OverrideForStore { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.turnofflarge")]
        public bool turnOffLarge { get; set; }
        public bool turnOffLarge_OverrideForStore { get; set; }


        #endregion

        #region extra large
        [GrandResourceDisplayName("grand.widget.swiperslider.slidesperviewextralarge")]
        public int slidesPerViewExtraLarge { get; set; }
        public bool slidesPerViewExtraLarge_OverrideForStore { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.spacebetweenextralarge")]
        public int spaceBetweenExtraLarge { get; set; }
        public bool spaceBetweenExtraLarge_OverrideForStore { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.turnoffextralarge")]
        public bool turnOffExtraLarge { get; set; }
        public bool turnOffExtraLarge_OverrideForStore { get; set; }

        #endregion

        #endregion
    }
}