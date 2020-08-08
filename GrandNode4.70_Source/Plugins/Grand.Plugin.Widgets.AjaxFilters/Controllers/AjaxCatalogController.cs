using Grand.Core.Domain.Catalog;
using Grand.Framework.Controllers;
using Grand.Plugin.Widgets.AjaxFilters.Models;
using Grand.Plugin.Widgets.AjaxFilters.Services;
using Grand.Services.Catalog;
using Grand.Web.Features.Models.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Grand.Plugin.Widgets.AjaxFilters.Controllers
{
    public class AjaxCatalogController : BaseController
    {
        private readonly IAjaxFiltersService _ajaxFiltersService;
        private readonly IProductService _productService;
        private readonly IMediator _mediator;
        private readonly CatalogSettings _catalogSettings;

        public AjaxCatalogController(IAjaxFiltersService ajaxFiltersService, IProductService productService, IMediator mediator, CatalogSettings catalogSettings)
        {
            _ajaxFiltersService = ajaxFiltersService; 
            _productService = productService;
            _mediator = mediator;
            _catalogSettings = catalogSettings;
        }

        private PublicInfoModel ClearModel(PublicInfoModel changeModel)
        {

            if (changeModel.manufacturerModel != null)
                for (int i = changeModel.manufacturerModel.Manufacturers.Count - 1; i >= 0; i--)
                {
                    if (changeModel.manufacturerModel.Manufacturers[i].Id == "")
                        changeModel.manufacturerModel.Manufacturers.RemoveAt(i);
                }

            if (changeModel.vendorsModel != null)
                for (int i = changeModel.vendorsModel.Vendors.Count - 1; i >= 0; i--)
                {
                    if (changeModel.vendorsModel.Vendors[i].Id == "")
                        changeModel.vendorsModel.Vendors.RemoveAt(i);
                }

            if (changeModel.specyficationModel != null)
                for (int i = changeModel.specyficationModel.SpecificationAttributes.Count - 1; i >= 0; i--)
                {
                    for (int j = changeModel.specyficationModel.SpecificationAttributes[i].SpecificationAttributeOptions.Count - 1; j >= 0; j--)
                    {
                        if (changeModel.specyficationModel.SpecificationAttributes[i].SpecificationAttributeOptions[j].Id == "")
                            changeModel.specyficationModel.SpecificationAttributes[i].SpecificationAttributeOptions.RemoveAt(j);
                    }

                    if (changeModel.specyficationModel.SpecificationAttributes[i].SpecificationAttributeOptions.Count == 0)
                        changeModel.specyficationModel.SpecificationAttributes.RemoveAt(i);
                }

            if (changeModel.attributesModel != null)
                for (int i = changeModel.attributesModel.ProductVariantAttributes.Count - 1; i >= 0; i--)
                {
                    for (int j = changeModel.attributesModel.ProductVariantAttributes[i].ProductVariantAttributesOptions.Count - 1; j >= 0; j--)
                    {
                        if (String.IsNullOrEmpty(changeModel.attributesModel.ProductVariantAttributes[i].ProductVariantAttributesOptions[j].Name))
                            changeModel.attributesModel.ProductVariantAttributes[i].ProductVariantAttributesOptions.RemoveAt(j);
                    }
                    if (changeModel.attributesModel.ProductVariantAttributes[i].ProductVariantAttributesOptions.Count == 0)
                        changeModel.attributesModel.ProductVariantAttributes.RemoveAt(i);

                }

            return changeModel;
        }

        public async Task<IActionResult> ReloadFilters(PublicInfoModel model, string typ)
        {
            if (typ == null)
                typ = "";

            ClearModel(model);

            var searchModel = await _ajaxFiltersService.PrepareSearchModel(model, typ);
            var query = await _ajaxFiltersService.PrepareSearchFilterResult(searchModel, true, typ, model.ViewMode);

            await _ajaxFiltersService.PreparePublicInfoModel2(model, query);

            model.AjaxProductsModel = new ProductsModel();
            model.AjaxProductsModel.PagingFilteringContext.LoadPagedList(query.Products);
            model.AjaxProductsModel.PagingFilteringContext.ViewMode = model.ViewMode;
            model.AjaxProductsModel.ViewMode = model.ViewMode;
            model.AjaxProductsModel.Products = await _mediator.Send(new GetProductOverview() {
                PrepareSpecificationAttributes = _catalogSettings.ShowSpecAttributeOnCatalogPages,
                Products = query.Products,
            });               
            model.AjaxProductsModel.Count = query.Products.TotalCount;

            return Json(new
            {
                Type = typ,
                Data = model,
                Url = query.Url,
                Products = await RenderPartialViewToString("~/Plugins/Widgets.AjaxFilters/Views/ProductsInGridOrLines.cshtml", model.AjaxProductsModel),
            });
        }

    }
}
