using MediatR;
using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Application.Boards.Commands;
using ThreadboxApi.Application.Boards.Models;
using ThreadboxApi.Application.Identity.Permissions;
using ThreadboxApi.Application.Sections.Commands;
using ThreadboxApi.Application.Sections.Models;
using ThreadboxApi.Application.Sections.Queries;
using ThreadboxApi.Web.PermissionHandling;

namespace ThreadboxApi.Web.Controllers
{
    [Route("api/sections")]
    public sealed class SectionController : ApplicationController
    {
        public SectionController(ISender sender) : base(sender) { }

        [HttpGet]
        public async Task<SectionListDto[]> GetList()
        {
            return await _sender.Send(new GetSectionList.Query());
        }

        [HttpGet("{id:guid}")]
        public async Task<SectionDto> Get([FromRoute] GetSection.Query query)
        {
            return await _sender.Send(query);
        }

        [HttpPost]
        [Permission(SectionsPermissions.Manage)]
        [ProducesResponseType(typeof(SectionDto), StatusCodes.Status201Created)]
        public async Task<CreatedAtActionResult> Create([FromBody] CreateSection.Command command)
        {
            return await _sender.Send(command);
        }

        [HttpPut("{id:guid}")]
        [Permission(SectionsPermissions.Manage)]
        public async Task<SectionDto> Update(UpdateSection.Command command)
        {
            return await _sender.Send(command);
        }

        [HttpPost("{sectionId:guid}")]
        [ProducesResponseType(typeof(BoardDto), StatusCodes.Status201Created)]
        public async Task<CreatedAtActionResult> Create(CreateBoard.Command command)
        {
            return await _sender.Send(command);
        }
    }
}