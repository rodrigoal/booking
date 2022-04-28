using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cancun.Booking.Application.Features.Bookings.Commands.DeleteBooking
{
  public class DeleteBookingCommand : IRequest
  {
    public int ID { get; set; }
    public string UserPassport { get; set; }
    public int CountryID { get; set; }
  }
}
