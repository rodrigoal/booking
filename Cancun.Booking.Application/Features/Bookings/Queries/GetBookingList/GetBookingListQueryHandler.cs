using AutoMapper;
using Cancun.Booking.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cancun.Booking.Application.Features.Bookings.Queries.GetBookingList
{
  public class GetBookingListQueryHandler : IRequestHandler<GetBookingListQuery, List<BookingListDto>>
  {

    private readonly IMapper _mapper;
    private readonly IBookingRepository _bookingRepository;
    private readonly IUserRepository _userRepository;

    public GetBookingListQueryHandler(IMapper mapper, IBookingRepository bookingRepository, IUserRepository userRepository)
    {
      _mapper = mapper;
      _bookingRepository = bookingRepository;
      _userRepository = userRepository;
    }

    public async Task<List<BookingListDto>> Handle(GetBookingListQuery request, CancellationToken cancellationToken)
    {
      var userId = await _userRepository.GetUserID(request.Passport, request.CountryId);
      var bookings = (await _bookingRepository.GetBookingListAsync(userId)).ToList();
      return _mapper.Map<List<BookingListDto>>(bookings);
    }

  }
}
