using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grand.Api.Queries.Models.Common;
using Grand.Services.Security;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmicusAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryTemplateController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPermissionService _permissionService;
        public CategoryTemplateController(IMediator mediator,IPermissionService permissionService)
        {
            _mediator = mediator;
            _permissionService = permissionService;
        }

        [HttpGet]
        [Route("GetCategoryTemplateAsync")]
        public async Task<IActionResult> GetCategoryTemplate(string key)
        {
            //if (!await _permissionService.Authorize(PermissionSystemName.Maintenance))
            //    return Forbid();

            var template = await _mediator.Send(new GetMessageTemplateQuery() { Id = key, TemplateName = typeof(Grand.Core.Domain.Catalog.CategoryTemplate).Name });
            if (!template.Any())
                return NotFound();

            return Ok(template.FirstOrDefault());
        }


        [HttpGet]
        [Route("GetAllCategoryTemplateAsync")]
        public async Task<IActionResult> GetAllCategoryTemplate()
        {
            //if (!await _permissionService.Authorize(PermissionSystemName.Maintenance))
            //    return Forbid();

            return Ok(await _mediator.Send(new GetMessageTemplateQuery() { TemplateName = typeof(Grand.Core.Domain.Catalog.CategoryTemplate).Name }));

        }
    }
}
