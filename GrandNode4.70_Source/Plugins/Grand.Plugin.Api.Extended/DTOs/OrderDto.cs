using Grand.Api.DTOs.Customers;
using Grand.Framework.Mvc.Models;
using System;
using System.Collections.Generic;

namespace Grand.Plugin.Api.Extended.DTOs
{
    public partial class OrderDto : BaseApiEntityModel
    {
        public OrderDto()
        {
            OrderItems = new List<OrderItemDto>();
        }

        public Guid OrderGuid { get; set; }
        public int OrderNumber { get; set; }
        public string StoreId { get; set; }
        public string CustomerId { get; set; }
        public bool PickUpInStore { get; set; }
        public int OrderStatusId { get; set; }
        public int ShippingStatusId { get; set; }
        public int PaymentStatusId { get; set; }
        public string PaymentMethodSystemName { get; set; }
        public string CustomerCurrencyCode { get; set; }
        public decimal CurrencyRate { get; set; }
        public int CustomerTaxDisplayTypeId { get; set; }
        public string VatNumber { get; set; }
        public int VatNumberStatusId { get; set; }
        public string CompanyName { get; set; }
        public string CustomerEmail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal OrderSubtotalInclTax { get; set; }
        public decimal OrderSubtotalExclTax { get; set; }
        public decimal OrderSubTotalDiscountInclTax { get; set; }
        public decimal OrderSubTotalDiscountExclTax { get; set; }
        public decimal OrderShippingInclTax { get; set; }
        public decimal OrderShippingExclTax { get; set; }
        public decimal PaymentMethodAdditionalFeeInclTax { get; set; }
        public decimal PaymentMethodAdditionalFeeExclTax { get; set; }
        public string TaxRates { get; set; }
        public decimal OrderTax { get; set; }
        public decimal OrderDiscount { get; set; }
        public decimal OrderTotal { get; set; }
        public decimal RefundedAmount { get; set; }
        public bool RewardPointsWereAdded { get; set; }
        public string CheckoutAttributeDescription { get; set; }
        public string CheckoutAttributesXml { get; set; }
        public string CustomerLanguageId { get; set; }
        public string AffiliateId { get; set; }
        public string CustomerIp { get; set; }
        public bool AllowStoringCreditCardNumber { get; set; }
        public string CardType { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string MaskedCreditCardNumber { get; set; }
        public string CardCvv2 { get; set; }
        public string CardExpirationMonth { get; set; }
        public string CardExpirationYear { get; set; }
        public string AuthorizationTransactionId { get; set; }
        public string AuthorizationTransactionCode { get; set; }
        public string AuthorizationTransactionResult { get; set; }
        public string CaptureTransactionId { get; set; }
        public string CaptureTransactionResult { get; set; }
        public string SubscriptionTransactionId { get; set; }
        public DateTime? PaidDateUtc { get; set; }
        public string ShippingMethod { get; set; }
        public string ShippingRateComputationMethodSystemName { get; set; }
        public string CustomValuesXml { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public bool Imported { get; set; }
        public string UrlReferrer { get; set; }
        public string ShippingOptionAttributeDescription { get; set; }
        public string ShippingOptionAttributeXml { get; set; }

        public virtual AddressDto BillingAddress { get; set; }
        public virtual AddressDto ShippingAddress { get; set; }

        public IList<OrderItemDto> OrderItems { get; set; }

        public partial class OrderItemDto : BaseApiEntityModel
        {
            public Guid OrderItemGuid { get; set; }
            public string ProductId { get; set; }
            public string VendorId { get; set; }
            public string WarehouseId { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPriceWithoutDiscInclTax { get; set; }
            public decimal UnitPriceWithoutDiscExclTax { get; set; }
            public decimal UnitPriceInclTax { get; set; }
            public decimal UnitPriceExclTax { get; set; }
            public decimal PriceInclTax { get; set; }
            public decimal PriceExclTax { get; set; }
            public decimal DiscountAmountInclTax { get; set; }
            public decimal DiscountAmountExclTax { get; set; }
            public decimal OriginalProductCost { get; set; }
            public string AttributeDescription { get; set; }
            public string AttributesXml { get; set; }
            public int DownloadCount { get; set; }
            public bool IsDownloadActivated { get; set; }
            public string LicenseDownloadId { get; set; }
            public decimal? ItemWeight { get; set; }
        }

    }
}
