using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cancun.Booking.Application.Features.Bookings.Queries.GetBookingList
{
  public class GetBookingListQuery : IRequest<List<BookingListDto>>
  {
    public string Passport { get; set; }
    public int CountryId { get; set; }
  }
}
