using Grand.Framework.Mvc.Models;
using System.Collections.Generic;

namespace Grand.Plugin.Widgets.SwiperSlider.Models
{
    public class PublicInfoModel : BaseGrandModel
    {
        public PublicInfoModel()
        {
            Slides = new List<PublicSlideModel>();
        }
        
        public List<PublicSlideModel> Slides { get; set; }
    }
    public class PublicSlideModel : BaseGrandModel
    {
        public string PictureUrl { get; set; }
        public bool Publish { get; set; }
        public string PictureTitle { get; set; }
        public string PictureAlt { get; set; }
        public int PictureWidth { get; set; }
        public int PictureHeight { get; set; }
        public string Caption { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
        public string Footer { get; set; }
        public int SlideOrder { get; set; }
        public string CategoryId { get; set; }
        public string ManufactureId { get; set; }
        public bool ApplyForHomepage { get; set; }
        public string HeaderAnimation { get; set; }
        public string HeaderAnimationDelay { get; set; }
        public string BodyAnimation { get; set; }
        public string BodyAnimationDelay { get; set; }
        public string FooterAnimation { get; set; }
        public string FooterAnimationDelay { get; set; }
    }
    
}