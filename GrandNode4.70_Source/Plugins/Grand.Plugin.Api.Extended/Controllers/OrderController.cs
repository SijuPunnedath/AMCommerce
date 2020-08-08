using Grand.Core.Data;
using Grand.Plugin.Api.Extended.DTOs;
using Grand.Services.Security;
using Grand.Web.Areas.Api.Controllers;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace Grand.Plugin.Api.Extended.Controllers
{
    public partial class OrderController : BaseODataController
    {
        private readonly IMongoDBContext _mongoDBContext;
        private readonly IPermissionService _permissionService;

        private readonly IMongoCollection<OrderDto> _orderDto;

        public OrderController(IMongoDBContext mongoDBContext, IPermissionService permissionService)
        {
            _mongoDBContext = mongoDBContext;
            _permissionService = permissionService;
            _orderDto = _mongoDBContext.Database().GetCollection<OrderDto>(typeof(Core.Domain.Orders.Order).Name);
        }

        [HttpGet]
        public async Task<IActionResult> Get(string key)
        {
            if (!await _permissionService.Authorize(PermissionSystemName.Orders))
                return Forbid();

            var orders = _orderDto.AsQueryable().FirstOrDefault(x => x.Id == key);
            if (orders == null)
                return NotFound();

            return Ok(orders);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            if (!await _permissionService.Authorize(PermissionSystemName.Orders))
                return Forbid();

            return Ok(_orderDto.AsQueryable());
        }
    }
}
