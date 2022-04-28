using Cancun.Booking.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cancun.Booking.Application.Features.Bookings.Commands.AddBooking
{
  public class AddBookingCommandResponse : BaseResponse
  {
    public AddBookingCommandResponse() : base()
    {

    }

    public AddBookingDto Booking { get; set; }
  }
}
