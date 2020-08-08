using System.ComponentModel.DataAnnotations;
using Grand.Framework.Mvc.Models;
using Grand.Framework.Mvc.ModelBinding;

namespace Grand.Plugin.Widgets.SwiperSlider.Models
{
    public class SlideModel : BaseGrandModel
    {
        [GrandResourceDisplayName("grand.widgets.swiperslider.id")]
        [UIHint("Id")]
        public string Id { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.fields.publish")]
        public bool Publish { get; set; }

        [GrandResourceDisplayName("grand.widgets.swiperslider.picturetitle")]
        [UIHint("PictureTitle")]
        public string PictureTitle { get; set; }

        [GrandResourceDisplayName("grand.widgets.swiperslider.picturealt")]
        [UIHint("PictureAlt")]
        public string PictureAlt { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.fields.caption")]
        [UIHint("Caption")]
        public string Caption { get; set; }

        [GrandResourceDisplayName("grand.widgets.swiperslider.header")]
        [UIHint("Header")]
        public string Header { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.fields.headeranimation")]
        public string HeaderAnimation { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.fields.headeranimationdelay")]
        public int HeaderAnimationDelay { get; set; }

        [GrandResourceDisplayName("grand.widgets.swiperslider.body")]
        [UIHint("Body")]
        public string Body { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.fields.bodyanimation")]
        public string BodyAnimation { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.fields.bodyanimationdelay")]
        public int BodyAnimationDelay { get; set; }

        [GrandResourceDisplayName("grand.widgets.swiperslider.footer")]
        [UIHint("Footer")]
        public string Footer { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.fields.footeranimation")]
        public string FooterAnimation { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.fields.footeranimationdelay")]
        public int FooterAnimationDelay { get; set; }
        
        [GrandResourceDisplayName("grand.widgets.swiperslider.slideorder")]
        [UIHint("SlideOrder")]
        public int SlideOrder { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.fields.categoryid")]
        public string CategoryId { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.fields.manufacturerid")]
        public string ManufacturerId { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.fields.applyforhomepage")]
        public bool ApplyForHomepage { get; set; }

    }
}