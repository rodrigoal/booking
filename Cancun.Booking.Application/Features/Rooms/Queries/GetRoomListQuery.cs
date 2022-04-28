using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cancun.Booking.Application.Features.Rooms.Queries
{
  public class GetRoomListQuery : IRequest<List<RoomListVm>>
  {

  }
}
