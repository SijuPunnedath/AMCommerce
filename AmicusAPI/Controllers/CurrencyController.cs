using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grand.Api.DTOs.Common;
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
    public class CurrencyController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPermissionService _permissionService;
        public CurrencyController(IMediator mediator, IPermissionService permissionService)
        {
            _mediator = mediator;
            _permissionService = permissionService;
        }
        [HttpGet]
        [Route("GetCurrencyAsync")]
        public async Task<IActionResult> GetCurrency(string key)
        {
            //if (!await _permissionService.Authorize(PermissionSystemName.Currencies))
            //return Forbid();

            var currency = await _mediator.Send(new GetQuery<CurrencyDto>() { Id = key });
            if (!currency.Any())
                return NotFound();

            return Ok(currency.FirstOrDefault());
        }


        [HttpGet]
        [Route("GetAllCurrencyAsync")]
        public async Task<IActionResult> GetAllCurrency()
        {
            //if (!await _permissionService.Authorize(PermissionSystemName.Currencies))
            //    return Forbid();

            return Ok(await _mediator.Send(new GetQuery<CurrencyDto>()));
        }
    }
}
