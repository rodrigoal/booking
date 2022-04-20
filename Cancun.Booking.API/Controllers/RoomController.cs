using AutoMapper;
using Cancun.Booking.API.Models;
using Cancun.Booking.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Cancun.Booking.API.Controllers
{

  [ApiController]
  [Route("api/room")]
  public class RoomController : ControllerBase
  {
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;

    public RoomController(IRoomRepository roomRepository, IMapper mapper)
    {
      _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
      
    }

    [HttpGet]
    [Route("GetRooms")]
    public async Task<ActionResult<IEnumerable<RoomDto>>> GetRooms()
    {

      var rooms = await _roomRepository.GetRoomsAsync();

      return Ok(_mapper.Map<IEnumerable<RoomDto>>(rooms));
    }
  }
}
