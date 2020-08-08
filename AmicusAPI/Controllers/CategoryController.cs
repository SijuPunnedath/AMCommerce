using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Grand.Api.Commands.Models.Catalog;
using Grand.Api.DTOs.Catalog;
using Grand.Api.Queries.Models.Common;
using Grand.Services.Security;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace AmicusAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPermissionService _permissionService;
        public CategoryController(IMediator mediator,IPermissionService permissionService)
        {
            _mediator = mediator;
            _permissionService = permissionService;
          
        }

       [HttpGet]
       [Route("GetCategoryAsync")]
        public async Task<IActionResult> GetCategory(string key)
        {
            //if (!await _permissionService.Authorize(PermissionSystemName.Categories))
            //    return Forbid();

            var category = await _mediator.Send(new GetQuery<CategoryDto>() { Id = key });
            if (!category.Any())
                return NotFound();
            return Ok(category.FirstOrDefault());
        }

        [HttpGet]
        [Route("GetAllCategoriesAsync")]
        public async Task<IActionResult> GetAllCategories()
        {
        //    if (!await _permissionService.Authorize(PermissionSystemName.Categories))
        //        return Forbid();

            return Ok(await _mediator.Send(new GetQuery<CategoryDto>()));
        }

        [HttpPost]
        [Route("CreateCategoryAsync")]
        public async Task<IActionResult> PostNewCategory([FromBody] CategoryDto model)
        {
            //if (!await _permissionService.Authorize(PermissionSystemName.Categories))
            //    return Forbid();

            if (ModelState.IsValid)
            {
                model = await _mediator.Send(new AddCategoryCommand() { Model = model });
                // return Created(model);
                return Ok(model);
                
            }
            return BadRequest(ModelState);
        }


        [HttpPut]
        [Route("UpdateCategoryAsync")]
        public async Task<IActionResult> PutCategory([FromBody] CategoryDto model)
        {
            //if (!await _permissionService.Authorize(PermissionSystemName.Categories))
            //    return Forbid();

            var category = await _mediator.Send(new GetQuery<CategoryDto>() { Id = model.Id });
            if (!category.Any())
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                model = await _mediator.Send(new UpdateCategoryCommand() { Model = model });
                return Ok(model);
            }
            return BadRequest(ModelState);
        }


        //[HttpPatch]
        //public async Task<IActionResult> Patch([FromBody] string key, Delta<CategoryDto> model)
        //{
        //    //if (!await _permissionService.Authorize(PermissionSystemName.Categories))
        //    //    return Forbid();

        //    var category = await _mediator.Send(new GetQuery<CategoryDto>() { Id = key });
        //    if (!category.Any())
        //    {
        //        return NotFound();
        //    }
        //    var cat = category.FirstOrDefault();
        //    model.Patch(cat);

        //    if (ModelState.IsValid)
        //    {
        //        await _mediator.Send(new UpdateCategoryCommand() { Model = cat });
        //        return Ok();
        //    }
        //    return BadRequest(ModelState);
        //}

        [HttpDelete]
        public async Task<IActionResult> Delete(string key)
        {
            //if (!await _permissionService.Authorize(PermissionSystemName.Categories))
            //    return Forbid();

            var category = await _mediator.Send(new GetQuery<CategoryDto>() { Id = key });
            if (!category.Any())
            {
                return NotFound();
            }

            await _mediator.Send(new DeleteCategoryCommand() { Model = category.FirstOrDefault() });

            return Ok();
        }

    }
}
