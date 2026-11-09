using MediatR;
using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Application.Boards.Commands;
using ThreadboxApi.Application.Boards.Models;
using ThreadboxApi.Application.Boards.Queries;

namespace ThreadboxApi.Web.Controllers
{
    [Route("api/boards")]
    public sealed class BoardController : ApplicationController
    {
        public BoardController(ISender sender) : base(sender) { }

        [HttpGet]
        public async Task<SectionBoardDto[]> GetList()
        {
            return await _sender.Send(new GetBoardList.Query());
        }

        [HttpGet("{id:guid}")]
        public async Task<BoardDto> Get([FromRoute] GetBoard.Query query)
        {
            return await _sender.Send(query);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BoardDto), StatusCodes.Status201Created)]
        public async Task<CreatedAtActionResult> Create([FromBody] CreateBoard.Command command)
        {
            return await _sender.Send(command);
        }

        [HttpPut]
        public async Task<BoardDto> Update([FromBody] UpdateBoard.Command command)
        {
            return await _sender.Send(command);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<NoContentResult> Delete([FromBody] DeleteBoard.Command command)
        {
            return await _sender.Send(command);
        }
    }
}
