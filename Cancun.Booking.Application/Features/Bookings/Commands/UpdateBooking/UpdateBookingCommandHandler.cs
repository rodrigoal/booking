using AutoMapper;
using Cancun.Booking.Application.Contracts.Persistence;
using Cancun.Booking.Application.Exceptions;
using Cancun.Booking.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cancun.Booking.Application.Features.Bookings.Commands.UpdateBooking
{
  public class UpdateBookingCommandHandler : IRequestHandler<UpdateBookingCommand>
  {
    private readonly IMapper _mapper;
    private readonly IBookingRepository _bookingRepository;
    private readonly IUserRepository _userRepository;

    public UpdateBookingCommandHandler(IMapper mapper, IBookingRepository bookingRepository, IUserRepository userRepository)
    {
      _mapper = mapper;
      _bookingRepository = bookingRepository;
      _userRepository = userRepository;
    }

    public async Task<Unit> Handle(UpdateBookingCommand request, CancellationToken cancellationToken)
    {

      var bookingToUpdate = await _bookingRepository.GetByIdAsync(request.ID);
      if (bookingToUpdate == null)
      {
        throw new NotFoundException(nameof(Reservation), request.ID);
      }

      var validator = new UpdateBookingCommandValidator(_bookingRepository, _userRepository);
      var validationResult = await validator.ValidateAsync(request);

      if (validationResult.Errors.Count > 0)
      {
        throw new ValidationException(validationResult);

      }
      else
      {
        _mapper.Map(request, bookingToUpdate, typeof(UpdateBookingCommand), typeof(Reservation));
        await _bookingRepository.UpdateAsync(bookingToUpdate);
      }

      return Unit.Value;
    }
  }
}
