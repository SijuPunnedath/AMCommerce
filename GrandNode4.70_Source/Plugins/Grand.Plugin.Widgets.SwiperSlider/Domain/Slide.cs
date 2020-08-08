using Grand.Plugin.Widgets.SwiperSlider.Models;
using Grand.Core;
using Grand.Core.Domain.Localization;
using Grand.Core.Domain.Stores;
using Grand.Services.Localization;
using System.Collections.Generic;

namespace Grand.Plugin.Widgets.SwiperSlider.Domain
{
    public class Slide : BaseEntity, ILocalizedEntity, IStoreMappingSupported
    {
        public Slide()
        {
            Locales = new List<LocalizedProperty>();
        }

        public string PictureId { get; set; }
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
        public bool LimitedToStores { get; set; }
        public bool ApplyForHomepage { get; set; }

        public string HeaderAnimation { get; set; }
        public string HeaderAnimationDelay { get; set; }
        public string BodyAnimation { get; set; }
        public string BodyAnimationDelay { get; set; }
        public string FooterAnimation { get; set; }
        public string FooterAnimationDelay { get; set; }
        public IList<LocalizedProperty> Locales { get; set; }
        public IList<string> Stores { get; set; }

        public SlideConfigurationModel ToConfModel()
        {
            SlideConfigurationModel model = new SlideConfigurationModel()
            {
                Id = Id,
                PictureId = PictureId,
                Publish = Publish,
                PictureTitle = PictureTitle,
                PictureAlt = PictureAlt,
                PictureWidth = PictureWidth,
                PictureHeight = PictureHeight,
                Caption = Caption,
                Header = Header,
                Body = Body,
                Footer = Footer,
                PictureUrl = "",
                SlideOrder = SlideOrder,
                CategoryId = CategoryId,
                ManufacturerId = ManufactureId,
                ApplyForHomepage = ApplyForHomepage,
                LimitedToStores = LimitedToStores,
                HeaderAnimation = HeaderAnimation,
                HeaderAnimationDelay = HeaderAnimationDelay,
                BodyAnimation = BodyAnimation,
                BodyAnimationDelay = BodyAnimationDelay,
                FooterAnimation = FooterAnimation,
                FooterAnimationDelay = FooterAnimationDelay
            };
            return model;
        }
        public PublicSlideModel ToModel(string pictureUrl = "", string languageId = "")
        {
            PublicSlideModel model = new PublicSlideModel()
            {
                PictureUrl = pictureUrl,
                Publish = Publish,
                PictureAlt = PictureAlt,
                PictureTitle = PictureTitle,
                PictureWidth = PictureWidth,
                PictureHeight = PictureHeight,
                Caption = Caption,
                Header = this.GetLocalized(x=>x.Header, languageId),
                Body = this.GetLocalized(x => x.Body, languageId),
                Footer= this.GetLocalized(x => x.Footer, languageId),
                SlideOrder = SlideOrder,
                CategoryId = CategoryId,
                ManufactureId = ManufactureId,
                ApplyForHomepage = ApplyForHomepage,
                HeaderAnimation = HeaderAnimation,
                HeaderAnimationDelay = HeaderAnimationDelay,
                BodyAnimation = BodyAnimation,
                BodyAnimationDelay = BodyAnimationDelay,
                FooterAnimation = FooterAnimation,
                FooterAnimationDelay = FooterAnimationDelay
            };
            if(GrandPlugin.TrialVersion)
            {
                model.CategoryId = null;
                model.ManufactureId = null;
                model.Header = "<p> Header Trial version Trial version</p>";
                model.Body = "<p> Body Trial version Trial version Body Trial version Trial version</p>";
                model.Footer = "<p> Footer Trial version Trial version</p>";
            }
            return model;
        }
    }
}
