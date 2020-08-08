using System.Collections.Generic;
namespace Grand.Plugin.Widgets.SwiperSlider.Services
{
    public interface ISlideService
    {
        int Add(Grand.Plugin.Widgets.SwiperSlider.Domain.Slide sld);
        int Delete(Grand.Plugin.Widgets.SwiperSlider.Domain.Slide sld);
        List<Grand.Plugin.Widgets.SwiperSlider.Domain.Slide> GetAllSlides();
        Grand.Plugin.Widgets.SwiperSlider.Domain.Slide GetSlideById(string sldId);
        int Update(Grand.Plugin.Widgets.SwiperSlider.Domain.Slide sld);
    }
}
