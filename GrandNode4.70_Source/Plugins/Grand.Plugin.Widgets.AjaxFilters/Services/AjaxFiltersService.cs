using Grand.Core;
using Grand.Core.Caching;
using Grand.Core.Data;
using Grand.Core.Domain.Catalog;
using Grand.Plugin.Widgets.AjaxFilters.Models;
using Grand.Services.Catalog;
using Grand.Services.Customers;
using Grand.Services.Localization;
using Grand.Services.Seo;
using Grand.Services.Vendors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grand.Plugin.Widgets.AjaxFilters.Services
{
    public class AjaxFiltersService : IAjaxFiltersService
    {
        public const string CATEGORY_CHILD_IDENTIFIERS_MODEL_KEY = "Grand.pres.category.childidentifiers-{0}-{1}-{2}";

        private readonly AjaxFiltersSettings _ajaxFiltersSettings;
        private readonly CatalogSettings _catalogSettings;
        private readonly IRepository<Product> _productRepository;
        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IVendorService _vendorService;
        private readonly ISpecificationAttributeService _specificationAttributeService;
        private readonly IProductAttributeService _productAttributeService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly ICacheManager _cacheManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AjaxFiltersService(AjaxFiltersSettings ajaxFiltersSettings,
            CatalogSettings catalogSettings,
            IRepository<Product> productRepository,
            ICategoryService categoryService,
            IManufacturerService manufacturerService,
            IVendorService vendorService,
            ISpecificationAttributeService specificationAttributeService,
            IProductAttributeService productAttributeService,
            IWorkContext workContext,
            IStoreContext storeContext,
            ICacheManager cacheManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _ajaxFiltersSettings = ajaxFiltersSettings;
            _catalogSettings = catalogSettings;
            _productRepository = productRepository;
            _categoryService = categoryService;
            _manufacturerService = manufacturerService;
            _vendorService = vendorService;
            _specificationAttributeService = specificationAttributeService;
            _productAttributeService = productAttributeService;
            _workContext = workContext;
            _storeContext = storeContext;
            _cacheManager = cacheManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public virtual async Task<List<string>> GetChildCategoryIds(string parentCategoryId)
        {
            string cacheKey = string.Format(CATEGORY_CHILD_IDENTIFIERS_MODEL_KEY,
                parentCategoryId,
                string.Join(",", _workContext.CurrentCustomer.GetCustomerRoleIds()),
                _storeContext.CurrentStore.Id);
            return await _cacheManager.GetAsync(cacheKey, async () =>
            {
                var categoriesIds = new List<string>();
                var categories = await _categoryService.GetAllCategoriesByParentCategoryId(parentCategoryId);
                foreach (var category in categories)
                {
                    categoriesIds.Add(category.Id);
                    categoriesIds.AddRange(await GetChildCategoryIds(category.Id));
                }
                return categoriesIds;
            });
        }

        protected virtual void PreparePrices(PublicInfoModel model, SearchFilterResult searchFilterResult)
        {
            model.EnablePriceRangeFilter = true;
            model.filterPriceModel = new FilterPriceRangeModel();
            model.filterPriceModel.CurrencySymbol = _workContext.WorkingCurrency.CurrencyCode;
            model.filterPriceModel.MinPrice = searchFilterResult.resultPrice.PriceMin;
            model.filterPriceModel.MaxPrice = searchFilterResult.resultPrice.PriceMax;
            model.filterPriceModel.CurrentMinPrice = searchFilterResult.resultPrice.PriceMin;
            model.filterPriceModel.CurrentMaxPrice = searchFilterResult.resultPrice.PriceMax;

            var pricemin = _httpContextAccessor.HttpContext.Request.Query["pricemin"].ToString();
            if (!string.IsNullOrEmpty(pricemin))
            {
                if (Decimal.TryParse(pricemin, out var minpr))
                    model.filterPriceModel.CurrentMinPrice = minpr;
            }
            var pricemax = _httpContextAccessor.HttpContext.Request.Query["pricemax"].ToString();
            if (!string.IsNullOrEmpty(pricemax))
            {
                if (Decimal.TryParse(pricemax, out var maxpr))
                    model.filterPriceModel.CurrentMaxPrice = maxpr;
            }
        }

        protected virtual async Task PrepareManufacturers(PublicInfoModel model, SearchFilterResult searchFilterResult)
        {
            model.EnableManufacturersFilter = true;
            model.manufacturerModel = new FilterManufacturersModel();
            model.manufacturerModel.CheckOrDropdown = _ajaxFiltersSettings.FilterManufacturerCheckOrDropdown;

            var queryManufacturers = new List<string>();
            if (_httpContextAccessor.HttpContext.Request.Query.Where(x => x.Key == "manufacturer").Any())
            {
                var values = _httpContextAccessor.HttpContext.Request.Query.FirstOrDefault(x => x.Key == "manufacturer").Value;
                foreach (var item in values.ToString().Split(","))
                {
                    queryManufacturers.Add(item);
                }
            }

            foreach (var item in searchFilterResult.manufacturers)
            {
                var manuf = await _manufacturerService.GetManufacturerById(item.Id);
                model.manufacturerModel.Manufacturers.Add(new ManufacturersModel() {
                    Id = item.Id,
                    Name = manuf?.Name,
                    Count = item.Count,
                    CheckedState = queryManufacturers.Where(x => x == manuf.SeName).Any() ? CheckedState.Checked : CheckedState.UnChecked
                });
            }
        }

        protected virtual async Task PrepareVendors(PublicInfoModel model, SearchFilterResult searchFilterResult)
        {
            model.EnableVendorsFilter = true;
            model.vendorsModel = new FilterVendorsModel();
            model.vendorsModel.CheckOrDropdown = _ajaxFiltersSettings.FilterVendorCheckOrDropdown;
            foreach (var item in searchFilterResult.vendors)
            {
                model.vendorsModel.Vendors.Add(new VendorsModel() {
                    Id = item.Id,
                    Name = (await _vendorService.GetVendorById(item.Id))?.Name,
                    Count = item.Count
                });
            }
        }

        protected virtual async Task PrepareSpecifications(PublicInfoModel model, SearchFilterResult searchFilterResult)
        {
            model.EnableSpecificationsFilter = true;
            model.specyficationModel = new FilterSpecificationsModel();
            model.specyficationModel.CheckOrDropdowns = _ajaxFiltersSettings.FilterSpecyficationCheckOrDropdown;

            var _tmp_spec = new List<SpecificationAttributesModel>();
            foreach (var item in searchFilterResult.specyfications)
            {
                var spec = await _specificationAttributeService.GetSpecificationAttributeByOptionId(item.Id);
                if (spec != null)
                {
                    if (!_tmp_spec.Any(x => x.Id == spec.Id))
                    {
                        _tmp_spec.Add(new SpecificationAttributesModel() {
                            Id = spec.Id,
                            Name = spec.Name,
                        });
                    }
                    var _sp = _tmp_spec.FirstOrDefault(x => x.Id == spec.Id);
                    if (!_sp.SpecificationAttributeOptions.Any(x => x.Id == item.Id))
                    {
                        var state = CheckedState.UnChecked;
                        var queryspec = _httpContextAccessor.HttpContext.Request.Query[spec.SeName].ToString();
                        if (!string.IsNullOrEmpty(queryspec))
                        {
                            var opt = spec.SpecificationAttributeOptions.FirstOrDefault(x => x.Id == item.Id);
                            if (opt != null)
                            {
                                foreach (var _s in queryspec.Split(","))
                                {
                                    if (opt.SeName == _s)
                                        state = CheckedState.Checked;
                                }
                            }
                        }
                        _sp.SpecificationAttributeOptions.Add(new SpecificationAttributeOptionsModel() {
                            Id = item.Id,
                            Name = spec.SpecificationAttributeOptions.FirstOrDefault(x => x.Id == item.Id)?.Name,
                            ColorSquaresRgb = spec.SpecificationAttributeOptions.FirstOrDefault(x => x.Id == item.Id)?.ColorSquaresRgb,
                            Count = item.Count,
                            CheckedState = state
                        });
                    }
                }
            }
            model.specyficationModel.SpecificationAttributes.AddRange(_tmp_spec);
        }

        protected virtual async Task PrepareAttributes(PublicInfoModel model, SearchFilterResult searchFilterResult)
        {
            model.EnableAttributesFilter = true;

            model.attributesModel = new FilterProductAttributesModel();
            model.attributesModel.CheckOrDropdowns = _ajaxFiltersSettings.FilterAttributesCheckOrDropdown;
            foreach (var attr in searchFilterResult.attributes.GroupBy(x => x.Id).Select(x => x.Key))
            {
                var filterProdVariantAttr = new FilterProductVariantAttributesModel();
                var _pa = await _productAttributeService.GetProductAttributeById(attr);
                filterProdVariantAttr.Id = attr;
                filterProdVariantAttr.Name = _pa.GetLocalized(x => x.Name, _workContext.WorkingLanguage.Id);

                var queryattr = _httpContextAccessor.HttpContext.Request.Query[_pa.SeName].ToString();
                foreach (var att in searchFilterResult.attributes.Where(x => x.Id == attr))
                {
                    var state = CheckedState.UnChecked;
                    if (!string.IsNullOrEmpty(queryattr))
                    {
                        foreach (var _s in queryattr.Split(","))
                        {
                            if (att.Name == _s)
                                state = CheckedState.Checked;
                        }
                    }

                    filterProdVariantAttr.ProductVariantAttributesOptions.Add(new ProductVariantAttributesOptionsModel() {
                        Name = att.Name,
                        Count = att.Count,
                        ColorSquaresRgb = att.Color,
                        CheckedState = state,
                    });
                }
                model.attributesModel.ProductVariantAttributes.Add(filterProdVariantAttr);
            }
        }

        protected virtual async Task QueryManufacturers(FilterDefinition<Product> filter, SearchFilterResult searchFilterResult)
        {
            var queryManufacturers = await _productRepository.Collection
                        .Aggregate()
                        .Match(filter)
                        .Unwind(x => x.ProductManufacturers)
                        .Project(new BsonDocument {
                        {
                            "ManufacturerId", "$ProductManufacturers.ManufacturerId"
                        }
                        })
                        .Group(new BsonDocument {
                        {"_id",
                            new BsonDocument {
                                { "ManufacturerId", "$ManufacturerId" },
                            }
                        },
                        {"count",
                            new BsonDocument {
                                { "$sum" , 1}
                            }
                        }
                        }).ToListAsync();

            foreach (var item in queryManufacturers)
            {
                searchFilterResult.manufacturers.Add(new SearchFilterResult.Manufacturer() {
                    Id = item["_id"]["ManufacturerId"].ToString(),
                    Count = Convert.ToInt32(item["count"].ToString())
                });
            }
        }
        protected virtual async Task QueryVendors(FilterDefinition<Product> filter, FilterDefinitionBuilder<Product> builder, SearchFilterResult searchFilterResult)
        {
            var filterVendorExists = filter &
                   builder.Where(x => !string.IsNullOrEmpty(x.VendorId));

            var queryVendors = await _productRepository.Collection
                        .Aggregate()
                        .Match(filterVendorExists)
                        .Project(new BsonDocument {
                        {
                            "VendorId", "$VendorId"
                        }
                        })
                        .Group(new BsonDocument {
                        {"_id",
                            new BsonDocument {
                                { "VendorId", "$VendorId" },
                            }
                        },
                        {"count",
                            new BsonDocument {
                                { "$sum" , 1}
                            }
                        }
                        }).ToListAsync();

            foreach (var item in queryVendors)
            {
                searchFilterResult.vendors.Add(new SearchFilterResult.Vendor() {
                    Id = item["_id"]["VendorId"].ToString(),
                    Count = Convert.ToInt32(item["count"].ToString())
                });
            }
        }
        protected virtual async Task QueryMinMaxPrice(FilterDefinition<Product> filter, FilterDefinitionBuilder<Product> builder, SearchFilterResult searchFilterResult)
        {
            var queryPrices = await _productRepository.Collection
            .Aggregate()
            .Match(filter)
            .Group(
                new BsonDocument
                {
                    { "_id", BsonNull.Value },
                    { "min", new BsonDocument { { "$min", "$Price" } } },
                    { "max", new BsonDocument { { "$max", "$Price" } } }
                })
            .ToListAsync();

            foreach (var item in queryPrices)
            {
                var min = item["min"].AsDecimal128;
                var max = item["max"].AsDecimal128;
                searchFilterResult.resultPrice = new SearchFilterResult.ResultPrice() {
                    PriceMin = Convert.ToInt32(min),
                    PriceMax = Convert.ToInt32(max)
                };
            }
        }
        protected virtual async Task QuerySpecifications(FilterDefinition<Product> filter, FilterDefinitionBuilder<Product> builder, SearchFilterResult searchFilterResult)
        {
            var filterSpecExists = filter &
                   builder.Where(x => x.ProductSpecificationAttributes.Any(y => y.AllowFiltering));

            var querySpec = await _productRepository.Collection
                    .Aggregate()
                    .Match(filterSpecExists)
                    .Unwind(x => x.ProductSpecificationAttributes)
                    .Match(new BsonDocument("ProductSpecificationAttributes.AllowFiltering", true))
                    .Project(new BsonDocument {
                            {"SpecificationAttributeOptionId", "$ProductSpecificationAttributes.SpecificationAttributeOptionId"}
                    })
                    .Group(new BsonDocument {
                        {"_id",
                            new BsonDocument {
                                { "SpecificationAttributeOptionId", "$SpecificationAttributeOptionId" },
                            }
                        },
                        {"count",
                            new BsonDocument {
                                { "$sum" , 1}
                            }
                        }
                    }).ToListAsync();

            foreach (var item in querySpec)
            {
                searchFilterResult.specyfications.Add(new SearchFilterResult.Specyfication() {
                    Id = item["_id"]["SpecificationAttributeOptionId"].ToString(),
                    Count = Convert.ToInt32(item["count"].ToString())
                });
            }
        }
        protected virtual async Task QueryAttributes(FilterDefinition<Product> filter, FilterDefinitionBuilder<Product> builder, SearchFilterResult searchFilterResult)
        {
            var filterAttrExists = filter &
                    builder.Where(x => x.ProductAttributeMappings.Any());
            var filterDefAttr = new FilterDefinitionBuilder<BsonDocument>();
            var filterAttr = (filterDefAttr.Eq("ProductAttributeMappings.AttributeControlTypeId", 1) |
                filterDefAttr.Eq("ProductAttributeMappings.AttributeControlTypeId", 2) |
                filterDefAttr.Eq("ProductAttributeMappings.AttributeControlTypeId", 3) |
                filterDefAttr.Eq("ProductAttributeMappings.AttributeControlTypeId", 40));

            var querySpec = await _productRepository.Collection
                    .Aggregate()
                    .Match(filterAttrExists)
                    .Unwind(x => x.ProductAttributeMappings)
                    .Match(filterAttr)
                    .Project(new BsonDocument {
                            { "ProductAttributeId", "$ProductAttributeMappings.ProductAttributeId" },
                            { "ProductAttributeValues","$ProductAttributeMappings.ProductAttributeValues.Name" },
                    })
                    .Unwind("ProductAttributeValues")
                    .Group(new BsonDocument {
                        {"_id",
                            new BsonDocument {
                                { "ProductAttributeId", "$ProductAttributeId" },
                                { "ProductAttributeName", "$ProductAttributeValues" },
                            }
                        },
                        {"count",
                            new BsonDocument {
                                { "$sum" , 1}
                            }
                        }
                    }).ToListAsync();

            foreach (var item in querySpec)
            {
                searchFilterResult.attributes.Add(new SearchFilterResult.Attribute() {
                    Id = item["_id"]["ProductAttributeId"].ToString(),
                    Name = item["_id"]["ProductAttributeName"].ToString(),
                    Count = Convert.ToInt32(item["count"].ToString())
                });
            }
        }
        protected virtual async Task QueryProducts(FilterDefinition<Product> filter, FilterDefinitionBuilder<Product> builder, SearchFilterResult searchFilterResult, SearchModel model)
        {
            var builderSort = Builders<Product>.Sort.Descending(x => x.CreatedOnUtc);
            if (model.OrderBy == ProductSortingEnum.Position && model.CategoryIds.Any())
            {
                //category position
                builderSort = Builders<Product>.Sort.Ascending(x => x.DisplayOrderCategory);
            }
            else if (model.OrderBy == ProductSortingEnum.NameAsc)
            {
                //Name: A to Z
                builderSort = Builders<Product>.Sort.Ascending(x => x.Name);
            }
            else if (model.OrderBy == ProductSortingEnum.NameDesc)
            {
                //Name: Z to A
                builderSort = Builders<Product>.Sort.Descending(x => x.Name);
            }
            else if (model.OrderBy == ProductSortingEnum.PriceAsc)
            {
                //Price: Low to High
                builderSort = Builders<Product>.Sort.Ascending(x => x.Price);
            }
            else if (model.OrderBy == ProductSortingEnum.PriceDesc)
            {
                //Price: High to Low
                builderSort = Builders<Product>.Sort.Descending(x => x.Price);
            }
            else if (model.OrderBy == ProductSortingEnum.CreatedOn)
            {
                //creation date
                builderSort = Builders<Product>.Sort.Ascending(x => x.CreatedOnUtc);
            }
            else if (model.OrderBy == ProductSortingEnum.OnSale)
            {
                //on sale
                builderSort = Builders<Product>.Sort.Descending(x => x.OnSale);
            }
            else if (model.OrderBy == ProductSortingEnum.MostViewed)
            {
                //most viewed
                builderSort = Builders<Product>.Sort.Descending(x => x.Viewed);
            }
            else if (model.OrderBy == ProductSortingEnum.BestSellers)
            {
                //best seller
                builderSort = Builders<Product>.Sort.Descending(x => x.Sold);
            }
            searchFilterResult.Products = await PagedList<Product>.Create(_productRepository.Collection, filter, builderSort, model.PageIndex, model.PageSize);

        }

        public virtual async Task<PublicInfoModel> PreparePublicInfoModel(PublicInfoEnum publicInfo, string id)
        {
            var pm = new PublicInfoModel();
            pm.ViewMode = _catalogSettings.DefaultViewMode;
            var viemode = _httpContextAccessor.HttpContext.Request.Query["viewmode"].ToString();
            if (!string.IsNullOrEmpty(viemode))
                pm.ViewMode = viemode;

            var pagenumber = _httpContextAccessor.HttpContext.Request.Query["pagenumber"].ToString();
            if (!string.IsNullOrEmpty(pagenumber))
                pm.PageNumber = int.Parse(pagenumber) - 1;

            if (publicInfo == PublicInfoEnum.Category)
            {
                var category = await _categoryService.GetCategoryById(id);
                if (category != null)
                    pm.PageSize = category.PageSize;
                pm.CategoryId = id;
            }
            if (publicInfo == PublicInfoEnum.Manufacturer)
            {
                var manufacturer = await _manufacturerService.GetManufacturerById(id);
                if (manufacturer != null)
                    pm.PageSize = manufacturer.PageSize;
                pm.ManufacturerId = id;
            }
            var search = await PrepareSearchModel(publicInfo, id);
            var sr = await PrepareSearchFilterResult(search, false, "start", "");

            if (_ajaxFiltersSettings.FilterEnablePriceRange)
            {
                PreparePrices(pm, sr);
            }
            if (_ajaxFiltersSettings.FilterEnableManufacturers)
            {
                await PrepareManufacturers(pm, sr);
            }
            if (_ajaxFiltersSettings.FilterEnableVendors)
            {
                await PrepareVendors(pm, sr);
            }
            if (_ajaxFiltersSettings.FilterEnableSpecifications)
            {
                await PrepareSpecifications(pm, sr);
            }
            if (_ajaxFiltersSettings.FilterEnableAttributes)
            {
                await PrepareAttributes(pm, sr);
            }
            return pm;
        }

        public virtual async Task PreparePublicInfoModel2(PublicInfoModel model, SearchFilterResult searchFilterResult)
        {
            if (_ajaxFiltersSettings.FilterEnableManufacturers)
            {
                await PrepareManufacturers(model, searchFilterResult);
            }
            if (_ajaxFiltersSettings.FilterEnableVendors)
            {
                await PrepareVendors(model, searchFilterResult);
            }
            if (_ajaxFiltersSettings.FilterEnableSpecifications)
            {
                await PrepareSpecifications(model, searchFilterResult);
            }
            if (_ajaxFiltersSettings.FilterEnableAttributes)
            {
                await PrepareAttributes(model, searchFilterResult);
            }
        }
        public virtual async Task<SearchModel> PrepareSearchModel(PublicInfoEnum publicInfo, string id)
        {
            var model = new SearchModel();

            if (publicInfo == PublicInfoEnum.Category)
            {
                model.CategoryIds.Add(id);
                if (_catalogSettings.ShowProductsFromSubcategories)
                {
                    //include subcategories
                    model.CategoryIds.AddRange(await GetChildCategoryIds(id));
                }

            }
            if (publicInfo == PublicInfoEnum.Manufacturer)
                model.ManufacturerIds.Add(id);

            return model;
        }

        public virtual async Task<SearchModel> PrepareSearchModel(PublicInfoModel publicInfoModel, string typ)
        {
            var searchModel = new SearchModel();
            if (!_catalogSettings.IgnoreAcl)
            {
                var allowedCustomerRolesIds = _workContext.CurrentCustomer.CustomerRoles
                    .Where(cr => cr.Active).Select(cr => cr.Id).ToList();
                searchModel.AllowedCustomerRolesIds = allowedCustomerRolesIds;
            }

            searchModel.StoreId = _storeContext.CurrentStore.Id;

            if (!string.IsNullOrEmpty(publicInfoModel.CategoryId))
            {
                searchModel.CategoryIds.Add(publicInfoModel.CategoryId);
                if (_catalogSettings.ShowProductsFromSubcategories)
                {
                    //include subcategories
                    searchModel.CategoryIds.AddRange(await GetChildCategoryIds(publicInfoModel.CategoryId));
                }
            }

            searchModel.ShowHidden = false;
            searchModel.VisibleIndividuallyOnly = true;
            searchModel.PageSize = publicInfoModel.PageSize;
            searchModel.PageIndex = publicInfoModel.PageNumber;
            searchModel.OrderBy = (ProductSortingEnum)publicInfoModel.SortOption;
            if (!string.IsNullOrEmpty(publicInfoModel.ManufacturerId))
                searchModel.ManufacturerIds.Add(publicInfoModel.ManufacturerId);

            if (publicInfoModel.manufacturerModel != null)
            {
                foreach (var manuf in publicInfoModel.manufacturerModel.Manufacturers)
                {
                    if (!string.IsNullOrEmpty(manuf.Id))
                    {
                        searchModel.ManufacturerIds.Add(manuf.Id);
                    }
                }
            }

            if (publicInfoModel.filterPriceModel != null)
            {
                if (publicInfoModel.filterPriceModel.CurrentMinPrice != publicInfoModel.filterPriceModel.MinPrice)
                    searchModel.PriceMin = publicInfoModel.filterPriceModel.CurrentMinPrice;

                if (publicInfoModel.filterPriceModel.CurrentMaxPrice != publicInfoModel.filterPriceModel.MaxPrice)
                    searchModel.PriceMax = publicInfoModel.filterPriceModel.CurrentMaxPrice;
            }

            if (publicInfoModel.vendorsModel != null)
            {
                foreach (var vendor in publicInfoModel.vendorsModel.Vendors)
                {
                    if (!string.IsNullOrEmpty(vendor.Id))
                        searchModel.VendorIds.Add(vendor.Id);
                }
            }

            if (publicInfoModel.specyficationModel != null)
            {
                foreach (var spec in publicInfoModel.specyficationModel.SpecificationAttributes)
                {
                    foreach (var sa in spec.SpecificationAttributeOptions)
                    {
                        if (!string.IsNullOrEmpty(sa.Id))
                        {
                            searchModel.FilteredSpecs.Add(sa.Id);
                        }
                    }
                }
            }
            if (publicInfoModel.attributesModel != null)
            {
                foreach (var attr in publicInfoModel.attributesModel.ProductVariantAttributes)
                {
                    foreach (var aa in attr.ProductVariantAttributesOptions)
                    {
                        if (!string.IsNullOrEmpty(attr.Id) && !string.IsNullOrEmpty(aa.Name))
                        {
                            searchModel.AtributeIds.Add(new Models.Attribute() {
                                Id = attr.Id,
                                Name = aa.Name,
                            });
                        }
                    }
                }
            }
            return searchModel;
        }

        public virtual async Task<SearchFilterResult> PrepareSearchFilterResult(SearchModel model, bool prepareproducts, string typ, string viewmode)
        {
            var modelResult = new SearchFilterResult();

            var builder = Builders<Product>.Filter;
            var filter = FilterDefinition<Product>.Empty;

            var query = new QueryBuilder();
            if (typ == "start")
            {
                foreach (var item in _httpContextAccessor.HttpContext.Request.Query)
                {
                    query.Add(item.Key, item.Value.ToString());
                }
            }

            var limitmanuf = model.VendorIds.Any() && model.FilteredSpecs.Any() && model.AtributeIds.Any() && model.VendorIds.Any() && model.PriceMin.HasValue && model.PriceMax.HasValue;

            //only published products

            filter = filter & builder.Where(p => p.Published);
            filter = filter & builder.Where(p => p.VisibleIndividually);
            filter = filter & builder.Where(p => p.ProductTypeId == (int)ProductType.SimpleProduct);

            //category filtering
            if (model.CategoryIds.Any())
            {
                filter = filter & builder.Where(x => x.ProductCategories.Any(y => model.CategoryIds.Contains(y.CategoryId)));
            }

            //manufacturer filtering
            if (model.ManufacturerIds.Any())
            {
                if(limitmanuf)
                    filter = filter & builder.Where(x => x.ProductManufacturers.Any(y => model.ManufacturerIds.Contains(y.ManufacturerId)));
                var collection = new List<string>();
                foreach (var item in model.ManufacturerIds)
                {
                    var manufacturer = await _manufacturerService.GetManufacturerById(item);
                    if (manufacturer != null)
                        collection.Add(manufacturer.SeName);
                }
                query.Add("manufacturer", string.Join(",", collection));
            }
            //vendor filtering
            if (model.VendorIds.Any())
            {
                filter = filter & builder.Where(x => model.VendorIds.Contains(x.VendorId));
                foreach (var item in model.VendorIds)
                {
                    var vendor = await _vendorService.GetVendorById(item);
                    query.Add("vendor", vendor?.SeName);
                }
            }

            //acl
            if (!_catalogSettings.IgnoreAcl)
            {
                filter = filter & (builder.AnyIn(x => x.CustomerRoles, model.AllowedCustomerRolesIds) | builder.Where(x => !x.SubjectToAcl));
            }

            //store
            if (!string.IsNullOrEmpty(model.StoreId) && !_catalogSettings.IgnoreStoreLimitations)
            {
                filter = filter & builder.Where(x => x.Stores.Any(y => y == model.StoreId) || !x.LimitedToStores);
            }

            //search by specs
            if (model.FilteredSpecs.Any())
            {
                var spec = new HashSet<string>();
                Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
                foreach (string key in model.FilteredSpecs)
                {
                    var specification = await _specificationAttributeService.GetSpecificationAttributeByOptionId(key);
                    if (specification != null)
                    {
                        query.Add(specification.SeName, specification.SpecificationAttributeOptions.FirstOrDefault(x => x.Id == key).SeName);

                        spec.Add(specification.Id);
                        if (!dictionary.ContainsKey(specification.Id))
                        {
                            //add
                            dictionary.Add(specification.Id, new List<string>());
                        }
                        dictionary[specification.Id].Add(key);
                    }
                }
                foreach (var item in dictionary)
                {
                    filter = filter & builder.Where(x => x.ProductSpecificationAttributes.Any(y => y.SpecificationAttributeId == item.Key && y.AllowFiltering
                    && item.Value.Contains(y.SpecificationAttributeOptionId)));
                }
            }
            //search by attr
            if (model.AtributeIds.Any())
            {
                foreach (var item in model.AtributeIds)
                {
                    filter = filter & builder.Where(x => x.ProductAttributeMappings.Any(y => y.ProductAttributeId == item.Id
                    && y.ProductAttributeValues.Any(z => z.Name == item.Name)));

                    var attr = await _productAttributeService.GetProductAttributeById(item.Id);
                    query.Add(attr.SeName, item.Name);
                }
            }

            //min price
            if (model.PriceMin.HasValue)
            {
                filter = filter & builder.Where(p => p.Price >= model.PriceMin.Value);
                query.Add("pricemin", model.PriceMin.Value.ToString());
            }
            //max price
            if (model.PriceMax.HasValue)
            {
                filter = filter & builder.Where(p => p.Price <= model.PriceMax.Value);
                query.Add("pricemax", model.PriceMax.Value.ToString());
            }
            //start/end date
            var nowUtc = DateTime.UtcNow;
            if (!_catalogSettings.IgnoreFilterableAvailableStartEndDateTime)
            {
                filter = filter & builder.Where(p =>
                    (p.AvailableStartDateTimeUtc == null || p.AvailableStartDateTimeUtc < nowUtc) &&
                    (p.AvailableEndDateTimeUtc == null || p.AvailableEndDateTimeUtc > nowUtc));
            }

            //prepare filters
            //manuf
            if (_ajaxFiltersSettings.FilterEnableManufacturers)
            {
                await QueryManufacturers(filter, modelResult);
            }
            //manufacturer filtering
            if (model.ManufacturerIds.Any() && !limitmanuf)
            {
                filter = filter & builder.Where(x => x.ProductManufacturers.Any(y => model.ManufacturerIds.Contains(y.ManufacturerId)));
            }

            //prices
            if (_ajaxFiltersSettings.FilterEnablePriceRange)
            {
                await QueryMinMaxPrice(filter, builder, modelResult);
            }

            
            //vendors
            if (_ajaxFiltersSettings.FilterEnableVendors)
            {
                await QueryVendors(filter, builder, modelResult);
            }
            //specifications
            if (_ajaxFiltersSettings.FilterEnableSpecifications)
            {
                await QuerySpecifications(filter, builder, modelResult);
            }
            //attributes
            if (_ajaxFiltersSettings.FilterEnableAttributes)
            {
                await QueryAttributes(filter, builder, modelResult);
            }

            //Filter products
            if (prepareproducts)
                await QueryProducts(filter, builder, modelResult, model);

            if (typ != "start")
                PrepareType(model, query, typ, viewmode);

            modelResult.Url = query.ToQueryString().ToString();
            return modelResult;
        }

        private void PrepareType(SearchModel model, QueryBuilder query, string typ, string viewmode)
        {
            var list = new List<string>() { "orderby", "viewmode", "pagesize"/*, "pagenumber"*/ };
            if (typ == "update")
                list.Add("pagenumber");

            var requestQuery = _httpContextAccessor.HttpContext.Request.Query.Where(x => list.Contains(x.Key)).ToList();
            foreach (var item in requestQuery.Where(x => x.Key != typ))
            {
                if (!query.Where(x => x.Key == item.Key).Any())
                    query.Add(item.Key, item.Value.ToString());
            }
            switch (typ)
            {
                case "orderby":
                    if (!query.Where(x => x.Key == "orderby").Any())
                        query.Add("orderby", ((int)model.OrderBy).ToString());
                    break;
                case "viewmode":
                    if (!query.Where(x => x.Key == "viewmode").Any())
                        query.Add("viewmode", viewmode);
                    break;
                case "pagesize":
                    if (!query.Where(x => x.Key == "pagesize").Any())
                        query.Add("pagesize", model.PageSize.ToString());
                    break;
                case "pagenumber":
                    if (!query.Where(x => x.Key == "pagenumber").Any())
                        query.Add("pagenumber", (model.PageIndex + 1).ToString());
                    break;
                default:
                    break;
            }
        }
    }
}
