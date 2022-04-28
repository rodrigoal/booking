using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cancun.Booking.Application.Features.Bookings.Commands.UpdateBooking
{
  public class UpdateBookingCommand : IRequest
  {
    public int ID { get; set; }
    public int RoomID { get; set; }
    public string UserPassport { get; set; }
    public int CountryID { get; set; }
    public DateTime BookingStartDate { get; set; }
    public DateTime BookingEndDate { get; set; }
  }
}
