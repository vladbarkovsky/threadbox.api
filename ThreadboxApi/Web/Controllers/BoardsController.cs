using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Application.Boards.Commands;
using ThreadboxApi.Application.Boards.Models;
using ThreadboxApi.Application.Boards.Queries;

namespace ThreadboxApi.Web.Controllers
{
    public class BoardsController : MediatRController
    {
        [JwtAuthorize]
        [HttpGet("[action]")]
        public async Task<ActionResult<List<BoardListDto>>> GetBoardsList()
        {
            return await Mediator.Send(new GetBoardsList.Query());
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<BoardDto>> GetBoard([FromQuery] GetBoard.Query query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> CreateBoard([FromBody] CreateBoard.Command command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPut("[action]")]
        public async Task<ActionResult> UpdateBoard([FromBody] UpdateBoard.Command command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("[action]")]
        public async Task<ActionResult> DeleteBoard([FromQuery] DeleteBoard.Command command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
    }
}