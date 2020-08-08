using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grand.Api.DTOs.Shipping;
using Grand.Api.Queries.Models.Common;
using Grand.Services.Security;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AmicusAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryDateController : ControllerBase
    {

        private readonly IMediator _mediator;
        private readonly IPermissionService _permissionService;

        public DeliveryDateController(IMediator mediator, IPermissionService permissionService)
        {
            _mediator = mediator;
            _permissionService = permissionService;
        }

        [HttpGet]
        [Route("GetDeliveryDateAsync")]
        public async Task<IActionResult> Get(string key)
        {
            //if (!await _permissionService.Authorize(PermissionSystemName.ShippingSettings))
            //    return Forbid();

            var deliverydate = await _mediator.Send(new GetQuery<DeliveryDateDto>() { Id = key });
            if (!deliverydate.Any())
                return NotFound();

            return Ok(deliverydate.FirstOrDefault());
        }

        [HttpGet]
        [Route("GetAllDeliveryDateAsync")]
        public async Task<IActionResult> Get()
        {
            //if (!await _permissionService.Authorize(PermissionSystemName.ShippingSettings))
            //    return Forbid();

            return Ok(await _mediator.Send(new GetQuery<DeliveryDateDto>()));
        }
    }
}
