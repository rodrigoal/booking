using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cancun.Booking.Application.Features.Bookings.Queries.GetAvailableDates
{
  public class GetAvailableDatesQuery : IRequest<IEnumerable<DateTime>>
  {
    public int RoomId { get; set; }
  }
}
