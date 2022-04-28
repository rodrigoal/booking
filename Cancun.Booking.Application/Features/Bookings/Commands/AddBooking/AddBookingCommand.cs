using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cancun.Booking.Application.Features.Bookings.Commands.AddBooking
{
  public class AddBookingCommand : IRequest<AddBookingCommandResponse>
  {
    public int RoomID { get; set; }
    public string UserPassport { get; set; }
    public int CountryID { get; set; }
    public DateTime BookingStartDate { get; set; }
    public DateTime BookingEndDate { get; set; }
  }
}
