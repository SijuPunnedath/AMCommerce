using Grand.Core;
using Grand.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grand.Plugin.Widgets.AjaxFilters.Models
{
    public class SearchModel
    {
        public SearchModel()
        {
            OrderBy = ProductSortingEnum.Position;
            CategoryIds = new List<string>();
            ManufacturerIds = new List<string>();
            StoreId = "";
            VendorIds = new List<string>();
            WarehouseIds = new List<string>();
            LoadFilterableSpecificationAttributeOptionIds = false;
            PageIndex = 0;
            PageSize = 2147483647;
            ProductTagIds = new List<string>();
            SearchDescriptions = false;
            SearchSku = true;
            SearchProductTags = false;
            LanguageId = 0;
            FilteredSpecs = new List<string>();
            ShowHidden = false;
            ParentGroupedProductId = 0;
            AllowedCustomerRolesIds = new List<string>();
            AtributeIds = new List<Attribute>();
        }
        public decimal? PriceMin { get; set; }
        public decimal? PriceMax { get; set; }

        public bool VisibleIndividuallyOnly { get; set; }
        public int ParentGroupedProductId { get; set; }
        public ProductSortingEnum OrderBy { get; set; }

        public ProductType? ProductType { get; set; }
        public List<string> CategoryIds { get; set; }
        public IList<string> ManufacturerIds { get; set; }
        public string StoreId { get; set; }
        public IList<string> VendorIds { get; set; }
        public IList<string> WarehouseIds { get; set; }

        public bool LoadFilterableSpecificationAttributeOptionIds;
        public IList<Attribute> AtributeIds { get; set; }

        public int PageIndex;

        public int PageSize;

        public bool FeaturedProducts;

        public IList<string> ProductTagIds;

        public string Keywords;
        public bool SearchDescriptions;
        public bool SearchSku;
        public bool SearchProductTags;
        public int LanguageId;
        public IList<string> FilteredSpecs;
        public bool ShowHidden;

        public IList<string> AllowedCustomerRolesIds;
    }

    public class Attribute
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }


    public class SearchFilterResult
    {
        public SearchFilterResult()
        {
            resultPrice = new ResultPrice();
            manufacturers = new List<Manufacturer>();
            vendors = new List<Vendor>();
            specyfications = new List<Specyfication>();
            attributes = new List<Attribute>();
            Products = new PagedList<Product>();
        }

        public string Url { get; set; }
        public ResultPrice resultPrice { get; set; }
        public IList<Manufacturer> manufacturers { get; set; }
        public IList<Vendor> vendors { get; set; }
        public IList<Specyfication> specyfications { get; set; }
        public IList<Attribute> attributes { get; set; }

        public IPagedList<Product> Products { get; set; }

        public class ResultPrice
        {
            public int PriceMin { get; set; }
            public int PriceMax { get; set; }
        }
        public class Manufacturer
        {
            public string Id { get; set; }
            public int Count { get; set; }
        }
        public class Vendor
        {
            public string Id { get; set; }
            public int Count { get; set; }

        }
        public class Specyfication
        {
            public string Id { get; set; }
            public int Count { get; set; }
        }
        public class Attribute
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public int Count { get; set; }
            public string Color { get; set; }
        }
    }
}
