using MediatR;
using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Application.Boards.Commands;
using ThreadboxApi.Application.Boards.Models;
using ThreadboxApi.Application.Boards.Queries;
using ThreadboxApi.Application.Common.Pagination;
using ThreadboxApi.Application.Threads.Models;
using ThreadboxApi.Application.Threads.Queries;

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

        [HttpPut("{id:guid}")]
        public async Task<BoardDto> Update([FromBody] UpdateBoard.Command command)
        {
            return await _sender.Send(command);
        }

        [HttpDelete("{id:guid}")]
        public async Task<NoContentResult> Delete(DeleteBoard.Command command)
        {
            return await _sender.Send(command);
        }

        [HttpPost("{boardId:guid}/threads")]
        public async Task<PaginatedResult<ThreadDto>> GetThreadList(GetThreadList.Query query)
        {
            return await _sender.Send(query);
        }
    }
}
