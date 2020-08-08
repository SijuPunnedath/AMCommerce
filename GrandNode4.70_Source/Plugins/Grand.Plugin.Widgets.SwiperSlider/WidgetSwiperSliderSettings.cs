using Grand.Core.Configuration;

namespace Grand.Plugin.Widgets.SwiperSlider
{
    public class WidgetSwiperSliderSettings : ISettings
    {
        public bool showNavigation { get; set; }
        public bool showPagination { get; set; }
        public string paginationType { get; set; }
        public int setMinHeight { get; set; }
        public bool autoPlay { get; set; }
        public int speed { get; set; }
        public bool loop { get; set; }
        public bool lazyLoading { get; set; }
        public bool preLoadImages { get; set; }
        public string WidgetZone { get; set; }
        public string CateroryWidgetZone { get; set; }
        public string ManufacturerWidgetZone { get; set; }
        public bool usecountdown { get; set; }
        public string countdown { get; set; }
        public int slidesPerView { get; set; }
        public int spaceBetween { get; set; }
        public string widgetZoneOwn { get; set; }
        public string categoryWidgetZoneOwn { get; set; }
        public string ManufacturerWidgetZoneOwn { get; set; }

        public bool breakpoints { get; set; }
        public int slidesPerViewExtraSmall { get; set; }
        public int spaceBetweenExtraSmall { get; set; }
        public bool turnOffExtraSmall { get; set; }
        public int slidesPerViewSmall { get; set; }
        public int spaceBetweenSmall { get; set; }
        public bool turnOffSmall { get; set; }
        public int slidesPerViewMedium { get; set; }
        public int spaceBetweenMedium { get; set; }
        public bool turnOffMedium { get; set; }
        public int slidesPerViewLarge { get; set; }
        public int spaceBetweenLarge { get; set; }
        public bool turnOffLarge { get; set; }
        public int slidesPerViewExtraLarge { get; set; }
        public int spaceBetweenExtraLarge { get; set; }
        public bool turnOffExtraLarge { get; set; }
    }
}