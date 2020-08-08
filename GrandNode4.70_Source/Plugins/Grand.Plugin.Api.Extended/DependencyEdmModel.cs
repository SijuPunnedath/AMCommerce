using Grand.Api.Infrastructure.DependencyManagement;
using Grand.Core.Configuration;
using Grand.Plugin.Api.Extended.DTOs;
using Microsoft.AspNet.OData.Builder;

namespace Grand.Plugin.Api.Extended
{
    public class DependencyEdmModel : IDependencyEdmModel
    {
        public void Register(ODataConventionModelBuilder builder, ApiConfig apiConfig)
        {
            #region Category model

            builder.EntitySet<OrderDto>("Order");
            builder.EntityType<OrderDto>().Count().Filter().OrderBy().Page();
            builder.ComplexType<OrderDto.OrderItemDto>();

            #endregion
        }
        public int Order => 10;

    }
}
