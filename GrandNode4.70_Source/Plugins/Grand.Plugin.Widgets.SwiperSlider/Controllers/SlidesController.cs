using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Grand.Core;
using Grand.Core.Caching;
using Grand.Services.Catalog;
using Grand.Services.Configuration;
using Grand.Services.Localization;
using Grand.Services.Media;
using Grand.Services.Stores;
using Grand.Framework.Controllers;
using Grand.Framework.Kendoui;
using Grand.Framework.Mvc;
using Grand.Framework.Mvc.Filters;
using Grand.Plugin.Widgets.SwiperSlider.Domain;
using Grand.Plugin.Widgets.SwiperSlider.Infrastructure.Cache;
using Grand.Plugin.Widgets.SwiperSlider.Models;
using Grand.Plugin.Widgets.SwiperSlider.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grand.Plugin.Widgets.SwiperSlider.Controllers
{
    [AuthorizeAdmin]
    [Area("Admin")]
    public class SlidesController : BasePluginController
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly ILanguageService _languageService;
        private readonly IPictureService _pictureService;
        private readonly ISlideService _slideService;
        private readonly ILocalizationService _localizationService;
        private readonly ISettingService _settingService;
        private readonly ICacheManager _cacheManager;
        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IStoreMappingService _storeMappingService;

        public SlidesController(IWorkContext workContext,
            IStoreContext storeContext,
            IStoreService storeService,
            ISlideService slideService,
            IPictureService pictureService,
            ILocalizationService localizationService,
            ILanguageService languageService,
            ISettingService settingService,
            ICacheManager cacheManager,
            ICategoryService categoryService,
            IManufacturerService manufacturerService,
            IStoreMappingService storeMappingService)
        {
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._storeService = storeService;
            this._pictureService = pictureService;
            this._slideService = slideService;
            this._localizationService = localizationService;
            this._languageService = languageService;
            this._settingService = settingService;
            this._cacheManager = cacheManager;
            this._categoryService = categoryService;
            this._manufacturerService = manufacturerService;
            this._storeMappingService = storeMappingService;
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

        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command)
        {
            List<Slide> slides = _slideService.GetAllSlides();

            var sliders = new List<SlideConfigurationModel>();
            foreach (var x in slides)
            {
                var sldModel = x.ToConfModel();
                sldModel.PictureUrl = await GetPictureUrl(x.PictureId);
                sliders.Add(sldModel);
            }
            var gridModel = new DataSourceResult
            {
                Data = sliders,
                Total = slides.Count()
            };
            return Json(gridModel);
        }

        public async Task<IActionResult> Create()
        {
            var model = new SlideConfigurationModel();
            model.Publish = true;
            model.ApplyForHomepage = true;
            model.CategoryIds.Add(new SelectListItem
            {
                Text = "[None]",
                Value = ""
            });
            model.CategoryIds.Add(new SelectListItem
            {
                Text = "All categories",
                Value = "0"
            });

            var categories = await _categoryService.GetAllCategories(showHidden: true);
            foreach (var c in categories)
            {
                model.CategoryIds.Add(new SelectListItem
                {
                    Text = _categoryService.GetFormattedBreadCrumb(c, categories),
                    Value = c.Id.ToString()
                });
            }

            model.ManufacturerIds.Add(new SelectListItem
            {
                Text = "[None]",
                Value = ""
            });
            model.ManufacturerIds.Add(new SelectListItem
            {
                Text = "All manufacturers",
                Value = "0"
            });

            var manufacturers = await _manufacturerService.GetAllManufacturers(showHidden: true);
            foreach (var m in manufacturers)
            {
                model.ManufacturerIds.Add(new SelectListItem
                {
                    Text = m.Name,
                    Value = m.Id.ToString()
                });
            }


            //locales
            await AddLocales(_languageService, model.Locales);
            //PrepareStoresMappingModel(model, null, false);
            ViewBag.RefreshPage = false;
            return View("~/Plugins/Widgets.SwiperSlider/Views/WidgetsSwiperSlider/Create.cshtml", model);
        }


        [HttpPost]
        [FormValueRequired("save")]
        public async Task<IActionResult> Create(string sldId, string formId, SlideConfigurationModel model)
        {
            if (ModelState.IsValid)
            {
                var slide = model.ToEntity();
                var picture = await _pictureService.GetPictureById(model.PictureId);
                if (picture != null)
                {
                    var pictureBinary = await _pictureService.LoadPictureBinary(picture);
                    using (var image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(pictureBinary)))
                    {
                        slide.PictureWidth = image.Width;
                        slide.PictureHeight = image.Height;
                    }
                }

                _slideService.Add(slide);
                /*
                UpdateAttributeLocales(slide, model);
                SaveStoreMappings(slide, model);
                */
            }

            ViewBag.RefreshPage = true;
            ViewBag.sldId = sldId;
            ViewBag.formId = formId;
            //PrepareStoresMappingModel(model, null, true);
            return View("~/Plugins/Widgets.SwiperSlider/Views/WidgetsSwiperSlider/Create.cshtml", model);
        }

        //edit
        public async Task<IActionResult> Edit(string id)
        {

            var slide = _slideService.GetSlideById(id);
            if (slide == null)
                //No checkout attribute found with the specified id
                return RedirectToAction("List");

            var model = slide.ToConfModel();

            model.CategoryIds.Add(new SelectListItem
            {
                Text = "[None]",
                Value = ""
            });
            model.CategoryIds.Add(new SelectListItem
            {
                Text = "All categories",
                Value = "0"
            });

            var categories = await _categoryService.GetAllCategories(showHidden: true);
            foreach (var c in categories)
            {
                model.CategoryIds.Add(new SelectListItem
                {
                    Text = _categoryService.GetFormattedBreadCrumb(c, categories),
                    Value = c.Id.ToString()
                });
            }

            model.ManufacturerIds.Add(new SelectListItem
            {
                Text = "[None]",
                Value = ""
            });
            model.ManufacturerIds.Add(new SelectListItem
            {
                Text = "All manufacturers",
                Value = "0"
            });
            var manufacturers = await _manufacturerService.GetAllManufacturers(showHidden: true);
            foreach (var m in manufacturers)
            {
                model.ManufacturerIds.Add(new SelectListItem
                {
                    Text = m.Name,
                    Value = m.Id.ToString()
                });
            }


            //locales
            await AddLocales(_languageService, model.Locales, (locale, languageId) =>
            {
                locale.Header = slide.GetLocalized(x => x.Header, languageId, false, false);
                locale.Body = slide.GetLocalized(x => x.Body, languageId, false, false);
                locale.Footer = slide.GetLocalized(x => x.Footer, languageId, false, false);
            });
            
            //PrepareStoresMappingModel(model, slide, false);

            ViewBag.RefreshPage = false;
            return View("~/Plugins/Widgets.SwiperSlider/Views/WidgetsSwiperSlider/Edit.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string sldId, string formId, SlideConfigurationModel model)
        {

            var slide = _slideService.GetSlideById(model.Id);
            if (slide == null)
                return RedirectToAction("List");
            if (ModelState.IsValid)
            {

                var picture = await _pictureService.GetPictureById(model.PictureId);
                if (picture != null)
                {
                    var pictureBinary = await _pictureService.LoadPictureBinary(picture);
                    using (var image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(pictureBinary)))
                    {
                        model.PictureWidth = image.Width;
                        model.PictureHeight = image.Height;
                    }
                }
                slide.Publish = model.Publish;
                slide.Header = model.Header ?? string.Empty;
                slide.Body = model.Body;
                slide.Footer = model.Footer;
                slide.PictureId = model.PictureId;
                slide.PictureAlt = model.PictureAlt;
                slide.PictureTitle = model.PictureTitle;
                slide.SlideOrder = model.SlideOrder;
                slide.Caption = model.Caption;
                slide.PictureWidth = model.PictureWidth;
                slide.PictureHeight = model.PictureHeight;
                slide.CategoryId = model.CategoryId;
                slide.ManufactureId = model.ManufacturerId;
                slide.ApplyForHomepage = model.ApplyForHomepage;
                slide.LimitedToStores = model.LimitedToStores;
                slide.HeaderAnimation = model.HeaderAnimation;
                slide.HeaderAnimationDelay = model.HeaderAnimationDelay;
                slide.BodyAnimation = model.BodyAnimation;
                slide.BodyAnimationDelay = model.BodyAnimationDelay;
                slide.FooterAnimation = model.FooterAnimation;
                slide.FooterAnimationDelay = model.FooterAnimationDelay;

                _slideService.Update(slide);
                /*
                UpdateAttributeLocales(slide, model);
                SaveStoreMappings(slide, model);
                */
                SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));
            }
            ViewBag.RefreshPage = true;
            ViewBag.sldId = sldId;
            ViewBag.formId = formId;
            //PrepareStoresMappingModel(model, slide, true);
            return View("~/Plugins/Widgets.SwiperSlider/Views/WidgetsSwiperSlider/Edit.cshtml", model);
        }

        [HttpPost]
        public IActionResult SlideUpdate(SlideModel model)
        {
            var slide = _slideService.GetSlideById(model.Id);
            slide.Publish = model.Publish;
            slide.ApplyForHomepage = model.ApplyForHomepage;
            slide.SlideOrder = model.SlideOrder;
            _slideService.Update(slide);

            return new NullJsonResult();
        }

        //delete
        [HttpPost]
        public IActionResult Delete(DataSourceRequest command, string id)
        {
            var slide = _slideService.GetSlideById(id);
            _slideService.Delete(slide);

            SuccessNotification(_localizationService.GetResource("Admin.Catalog.Attributes.Slides.Deleted"));

            var slides = _slideService.GetAllSlides();
            var slideShippingGrid = slides.Skip((command.Page - 1) * (command.PageSize)).Take(command.PageSize);
            var model = new DataSourceResult
            {
                Data = slides.Select(x =>
                {
                    var sldModel = x.ToConfModel();

                    return sldModel;
                }),
                Total = slides.Count()
            };
            return Json(model);
        }

        
        
    }
}