using Cancun.Booking.Application.Features.Rooms.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cancun.Booking.API.Controllers
{

  [ApiController]
  [Route("api/room")]
  public class RoomController : ControllerBase
  {

    private readonly IMediator _mediator;

    public RoomController(IMediator mediator)
    {
      _mediator = mediator;

    }

    /// <summary>
    /// Returns the list of rooms availables to do an reservation.
    /// </summary>
    /// <returns>List of rooms</returns>
    [HttpGet("all", Name = "GetRooms")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RoomListVm>>> GetRooms()
    {

      var rooms = await _mediator.Send(new GetRoomListQuery());
      return Ok(rooms);
    }
  }
}
