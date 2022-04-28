using Cancun.Booking.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cancun.Booking.Application.Features.Bookings.Queries.GetAvailableDates
{
  public class GetAvailableDatesQueryHandler : IRequestHandler<GetAvailableDatesQuery, IEnumerable<DateTime>>
  {

    private readonly IBookingRepository _bookingRepository;

    public GetAvailableDatesQueryHandler(IBookingRepository bookingRepository)
    {

      _bookingRepository = bookingRepository;

    }

    public async Task<IEnumerable<DateTime>> Handle(GetAvailableDatesQuery request, CancellationToken cancellationToken)
    {
      var availableDates = await _bookingRepository.GetEmptyBookingsAsync(request.RoomId);
      return availableDates;
    }

  }
}
