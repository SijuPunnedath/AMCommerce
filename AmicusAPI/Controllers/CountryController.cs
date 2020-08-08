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

namespace AmicusAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPermissionService _permissionService;
        public CountryController(IMediator mediator, IPermissionService permissionService)
        {
            _mediator = mediator;
            _permissionService = permissionService;
        }

        [HttpGet]
        [Route("GetCountryAsync")]
        public async Task<IActionResult> GetCountry(string key)
        {
            //if (!await _permissionService.Authorize(PermissionSystemName.Countries))
            //    return Forbid();

            var country = await _mediator.Send(new GetQuery<CountryDto>() { Id = key });
            if (!country.Any())
                return NotFound();

            return Ok(country.FirstOrDefault());
        }

        [HttpGet]
        [Route("GetAllCountriesAsync")]
        public async Task<IActionResult> GetAllCountries()
        {
            //if (!await _permissionService.Authorize(PermissionSystemName.Countries))
            //    return Forbid();

            return Ok(await _mediator.Send(new GetQuery<CountryDto>()));
        }
    }
}
