using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Grand.Plugin.Widgets.SwiperSlider.Domain;
using Grand.Framework.Localization;
using Grand.Framework.Mvc.Models;
using Grand.Framework.Mvc.ModelBinding;
using Grand.Core.Domain.Stores;
using Grand.Framework.Mapping;

namespace Grand.Plugin.Widgets.SwiperSlider.Models
{

    public class SlideConfigurationModel : BaseGrandModel, ILocalizedModel<SlideLocalizedModel>, IStoreMappingModel
    {

        public SlideConfigurationModel()
        {
            Locales = new List<SlideLocalizedModel>();
            CategoryIds = new List <SelectListItem>();
            ManufacturerIds = new List<SelectListItem>();
            AnimationTypes = new List<SelectListItem>();
            CaptionTypes = new List<SelectListItem>();
            AnimationDelays = new List<SelectListItem>();


            AnimationTypes.Add(new SelectListItem() { Text = "bounce", Value = "bounce" });
            AnimationTypes.Add(new SelectListItem() { Text = "flash", Value = "flash" });
            AnimationTypes.Add(new SelectListItem() { Text = "pulse", Value = "pulse" });
            AnimationTypes.Add(new SelectListItem() { Text = "rubberBand", Value = "rubberBand" });
            AnimationTypes.Add(new SelectListItem() { Text = "shake", Value = "shake" });
            AnimationTypes.Add(new SelectListItem() { Text = "headShake", Value = "headShake" });
            AnimationTypes.Add(new SelectListItem() { Text = "swing", Value = "swing" });
            AnimationTypes.Add(new SelectListItem() { Text = "tada", Value = "tada" });
            AnimationTypes.Add(new SelectListItem() { Text = "wobble", Value = "wobble" });
            AnimationTypes.Add(new SelectListItem() { Text = "jello", Value = "jello" });
            AnimationTypes.Add(new SelectListItem() { Text = "bounceIn", Value = "bounceIn" });
            AnimationTypes.Add(new SelectListItem() { Text = "bounceInDown", Value = "bounceInDown" });
            AnimationTypes.Add(new SelectListItem() { Text = "bounceInLeft", Value = "bounceInLeft" });
            AnimationTypes.Add(new SelectListItem() { Text = "bounceInRight", Value = "bounceInRight" });
            AnimationTypes.Add(new SelectListItem() { Text = "bounceInUp", Value = "bounceInUp" });
            AnimationTypes.Add(new SelectListItem() { Text = "bounceOut", Value = "bounceOut" });
            AnimationTypes.Add(new SelectListItem() { Text = "bounceOutDown", Value = "bounceOutDown" });
            AnimationTypes.Add(new SelectListItem() { Text = "bounceOutLeft", Value = "bounceOutLeft" });
            AnimationTypes.Add(new SelectListItem() { Text = "bounceOutRight", Value = "bounceOutRight" });
            AnimationTypes.Add(new SelectListItem() { Text = "bounceOutUp", Value = "bounceOutUp" });
            AnimationTypes.Add(new SelectListItem() { Text = "fadeIn", Value = "fadeIn" });
            AnimationTypes.Add(new SelectListItem() { Text = "fadeInDown", Value = "fadeInDown" });
            AnimationTypes.Add(new SelectListItem() { Text = "fadeInDownBig", Value = "fadeInDownBig" });
            AnimationTypes.Add(new SelectListItem() { Text = "fadeInLeft", Value = "fadeInLeft" });
            AnimationTypes.Add(new SelectListItem() { Text = "fadeInLeftBig", Value = "fadeInLeftBig" });
            AnimationTypes.Add(new SelectListItem() { Text = "fadeInRight", Value = "fadeInRight" });
            AnimationTypes.Add(new SelectListItem() { Text = "fadeInRightBig", Value = "fadeInRightBig" });
            AnimationTypes.Add(new SelectListItem() { Text = "fadeInUp", Value = "fadeInUp" });
            AnimationTypes.Add(new SelectListItem() { Text = "fadeInUpBig", Value = "fadeInUpBig" });
            AnimationTypes.Add(new SelectListItem() { Text = "fadeOut", Value = "fadeOut" });
            AnimationTypes.Add(new SelectListItem() { Text = "fadeOutDown", Value = "fadeOutDown" });
            AnimationTypes.Add(new SelectListItem() { Text = "fadeOutDownBig", Value = "fadeOutDownBig" });
            AnimationTypes.Add(new SelectListItem() { Text = "fadeOutLeft", Value = "fadeOutLeft" });
            AnimationTypes.Add(new SelectListItem() { Text = "fadeOutLeftBig", Value = "fadeOutLeftBig" });
            AnimationTypes.Add(new SelectListItem() { Text = "fadeOutRight", Value = "fadeOutRight" });
            AnimationTypes.Add(new SelectListItem() { Text = "fadeOutRightBig", Value = "fadeOutRightBig" });
            AnimationTypes.Add(new SelectListItem() { Text = "fadeOutUp", Value = "fadeOutUp" });
            AnimationTypes.Add(new SelectListItem() { Text = "fadeOutUpBig", Value = "fadeOutUpBig" });
            AnimationTypes.Add(new SelectListItem() { Text = "flipInX", Value = "flipInX" });
            AnimationTypes.Add(new SelectListItem() { Text = "flipInY", Value = "flipInY" });
            AnimationTypes.Add(new SelectListItem() { Text = "flipOutX", Value = "flipOutX" });
            AnimationTypes.Add(new SelectListItem() { Text = "flipOutY", Value = "flipOutY" });
            AnimationTypes.Add(new SelectListItem() { Text = "lightSpeedIn", Value = "lightSpeedIn" });
            AnimationTypes.Add(new SelectListItem() { Text = "lightSpeedOut", Value = "lightSpeedOut" });
            AnimationTypes.Add(new SelectListItem() { Text = "rotateIn", Value = "rotateIn" });
            AnimationTypes.Add(new SelectListItem() { Text = "rotateInDownLeft", Value = "rotateInDownLeft" });
            AnimationTypes.Add(new SelectListItem() { Text = "rotateInDownRight", Value = "rotateInDownRight" });
            AnimationTypes.Add(new SelectListItem() { Text = "rotateInUpLeft", Value = "rotateInUpLeft" });
            AnimationTypes.Add(new SelectListItem() { Text = "rotateInUpRight", Value = "rotateInUpRight" });
            AnimationTypes.Add(new SelectListItem() { Text = "rotateOutDownLeft", Value = "rotateOutDownLeft" });
            AnimationTypes.Add(new SelectListItem() { Text = "rotateOut", Value = "rotateOut" });
            AnimationTypes.Add(new SelectListItem() { Text = "rotateOutDownRight", Value = "rotateOutDownRight" });
            AnimationTypes.Add(new SelectListItem() { Text = "rotateOutUpLeft", Value = "rotateOutUpLeft" });
            AnimationTypes.Add(new SelectListItem() { Text = "rotateOutUpRight", Value = "rotateOutUpRight" });
            AnimationTypes.Add(new SelectListItem() { Text = "hinge", Value = "hinge" });
            AnimationTypes.Add(new SelectListItem() { Text = "jackInTheBox", Value = "jackInTheBox" });
            AnimationTypes.Add(new SelectListItem() { Text = "rollIn", Value = "rollIn" });
            AnimationTypes.Add(new SelectListItem() { Text = "rollOut", Value = "rollOut" });
            AnimationTypes.Add(new SelectListItem() { Text = "zoomIn", Value = "zoomIn" });
            AnimationTypes.Add(new SelectListItem() { Text = "zoomInDown", Value = "zoomInDown" });
            AnimationTypes.Add(new SelectListItem() { Text = "zoomInLeft", Value = "zoomInLeft" });
            AnimationTypes.Add(new SelectListItem() { Text = "zoomInRight", Value = "zoomInRight" });
            AnimationTypes.Add(new SelectListItem() { Text = "zoomInUp", Value = "zoomInUp" });
            AnimationTypes.Add(new SelectListItem() { Text = "zoomOut", Value = "zoomOut" });
            AnimationTypes.Add(new SelectListItem() { Text = "zoomOutDown", Value = "zoomOutDown" });
            AnimationTypes.Add(new SelectListItem() { Text = "zoomOutLeft", Value = "zoomOutLeft" });
            AnimationTypes.Add(new SelectListItem() { Text = "zoomOutRight", Value = "zoomOutRight" });
            AnimationTypes.Add(new SelectListItem() { Text = "zoomOutUp", Value = "zoomOutUp" });
            AnimationTypes.Add(new SelectListItem() { Text = "slideInDown", Value = "slideInDown" });
            AnimationTypes.Add(new SelectListItem() { Text = "slideInLeft", Value = "slideInLeft" });
            AnimationTypes.Add(new SelectListItem() { Text = "slideInRight", Value = "slideInRight" });
            AnimationTypes.Add(new SelectListItem() { Text = "slideInUp", Value = "slideInUp" });
            AnimationTypes.Add(new SelectListItem() { Text = "slideOutDown", Value = "slideOutDown" });
            AnimationTypes.Add(new SelectListItem() { Text = "slideOutLeft", Value = "slideOutLeft" });
            AnimationTypes.Add(new SelectListItem() { Text = "slideOutRight", Value = "slideOutRight" });
            AnimationTypes.Add(new SelectListItem() { Text = "slideOutUp", Value = "slideOutUp" });
            AnimationTypes.Add(new SelectListItem() { Text = "heartBeat", Value = "heartBeat" });

            CaptionTypes.Add(new SelectListItem() { Text = "center", Value = "center" });
            CaptionTypes.Add(new SelectListItem() { Text = "left", Value = "left" });
            CaptionTypes.Add(new SelectListItem() { Text = "right", Value = "right" });

            AnimationDelays.Add(new SelectListItem() { Text = "none", Value = "0" });
            AnimationDelays.Add(new SelectListItem() { Text = "0.5", Value = "0-5" });
            AnimationDelays.Add(new SelectListItem() { Text = "1", Value = "1" });
            AnimationDelays.Add(new SelectListItem() { Text = "1.5", Value = "1-5" });
            AnimationDelays.Add(new SelectListItem() { Text = "2", Value = "2" });
            AnimationDelays.Add(new SelectListItem() { Text = "2.5", Value = "2-5" });
            AnimationDelays.Add(new SelectListItem() { Text = "3", Value = "3" });
            AnimationDelays.Add(new SelectListItem() { Text = "3.5", Value = "3-5" });
            AnimationDelays.Add(new SelectListItem() { Text = "4", Value = "4" });
            AnimationDelays.Add(new SelectListItem() { Text = "4.5", Value = "4-5" });
            AnimationDelays.Add(new SelectListItem() { Text = "5", Value = "5" });
            //AnimationDelays.Add(new SelectListItem() { Text = "5.5", Value = "5-5" });
            //AnimationDelays.Add(new SelectListItem() { Text = "6", Value = "6" });
            //AnimationDelays.Add(new SelectListItem() { Text = "6.5", Value = "6-5" });
            //AnimationDelays.Add(new SelectListItem() { Text = "7", Value = "7" });
            //AnimationDelays.Add(new SelectListItem() { Text = "7.5", Value = "7-5" });
            //AnimationDelays.Add(new SelectListItem() { Text = "8", Value = "8" });
            //AnimationDelays.Add(new SelectListItem() { Text = "8.5", Value = "8-5" });
            //AnimationDelays.Add(new SelectListItem() { Text = "9", Value = "9" });
            //AnimationDelays.Add(new SelectListItem() { Text = "9.5", Value = "9-5" });
            //AnimationDelays.Add(new SelectListItem() { Text = "10", Value = "10" });
            
        }

        [GrandResourceDisplayName("grand.widget.swiperslider.fields.id")]
        [UIHint("Id")]
        public string Id { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.fields.picture")]
        [UIHint("Picture")]
        public string PictureId { get; set; }
        public bool PictureId_OverrideForStore { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.fields.publish")]
        public bool Publish { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.fields.picturetitle")]
        [UIHint("NewWindow")]
        public string PictureTitle { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.fields.picturealt")]
        public string PictureAlt { get; set; }

        public int PictureWidth { get; set; }
        public int PictureHeight { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.fields.caption")]
        public string Caption { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.fields.header")]
        public string Header { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.fields.headeranimation")]
        public string HeaderAnimation { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.fields.headeranimationdelay")]
        public string HeaderAnimationDelay { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.fields.body")]
        public string Body { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.fields.bodyanimation")]
        public string BodyAnimation { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.fields.bodyanimationdelay")]
        public string BodyAnimationDelay { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.fields.footer")]
        public string Footer { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.fields.footeranimation")]
        public string FooterAnimation { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.fields.footeranimationdelay")]
        public string FooterAnimationDelay { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.fields.pictureurl")]
        [UIHint("PictureUrl")]
        public string PictureUrl { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.fields.slideorder")]
        [UIHint("SlideOrder")]
        public int SlideOrder { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.fields.categoryid")]
        public string CategoryId { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.fields.manufacturerid")]
        public string ManufacturerId { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.fields.applyforhomepage")]
        public bool ApplyForHomepage { get; set; }


        //Store mapping
        [GrandResourceDisplayName("grand.widget.swiperslider.fields.limitedtostores")]
        public bool LimitedToStores { get; set; }
        [GrandResourceDisplayName("grand.widget.swiperslider.fields.availablestores")]
        public List<Grand.Core.Domain.Stores.Store> AvailableStores { get; set; }
        public int[] SelectedStoreIds { get; set; }

        public List<SelectListItem> ManufacturerIds { get; set; }
        public List<SelectListItem> CategoryIds { get; set; }
        public List<SelectListItem> AnimationTypes { get; set; }
        public List<SelectListItem> CaptionTypes { get; set; }
        public List<SelectListItem> AnimationDelays { get; set; }


        public Slide ToEntity()
        {
            return new Slide()
            {
                Id = this.Id,
                PictureId = this.PictureId,
                Publish = this.Publish,
                ApplyForHomepage = this.ApplyForHomepage,
                PictureTitle = this.PictureTitle,
                PictureAlt = this.PictureAlt,
                PictureWidth = this.PictureWidth,
                PictureHeight = this.PictureHeight,
                Caption = this.Caption,
                Header = this.Header,
                Body = this.Body ?? string.Empty,
                Footer = this.Footer ?? string.Empty,
                SlideOrder = this.SlideOrder,
                CategoryId = this.CategoryId,
                ManufactureId = this.ManufacturerId,
                LimitedToStores = this.LimitedToStores,
                HeaderAnimation = this.HeaderAnimation,
                HeaderAnimationDelay = this.HeaderAnimationDelay,
                BodyAnimation = this.BodyAnimation,
                BodyAnimationDelay = this.BodyAnimationDelay,
                FooterAnimation = this.FooterAnimation,
                FooterAnimationDelay = this.FooterAnimationDelay
            };
        }

        public IList<SlideLocalizedModel> Locales { get; set; }

        List<StoreModel> IStoreMappingModel.AvailableStores { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        string[] IStoreMappingModel.SelectedStoreIds { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }
    public partial class SlideLocalizedModel : ILocalizedModelLocal
    {
        public string LanguageId { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.fields.header")]
        public string Header { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.fields.body")]
        public string Body { get; set; }

        [GrandResourceDisplayName("grand.widget.swiperslider.fields.footer")]
        public string Footer { get; set; }

    }
}
