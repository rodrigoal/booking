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

namespace Cancun.Booking.Application.Features.Bookings.Commands.DeleteBooking
{
  public class DeleteBookingCommandHandler : IRequestHandler<DeleteBookingCommand>
  {
    private readonly IMapper _mapper;
    private readonly IBookingRepository _bookingRepository;
    private readonly IUserRepository _userRepository;

    public DeleteBookingCommandHandler(IMapper mapper, IBookingRepository bookingRepository, IUserRepository userRepository)
    {
      _mapper = mapper;
      _bookingRepository = bookingRepository;
      _userRepository = userRepository;
    }

    public async Task<Unit> Handle(DeleteBookingCommand request, CancellationToken cancellationToken)
    {

      var bookingToDelete = await _bookingRepository.GetByIdAsync(request.ID);
      if (bookingToDelete == null)
      {
        throw new NotFoundException(nameof(Reservation), request.ID);
      }

      var validator = new DeleteBookingCommandValidator(_bookingRepository, _userRepository);
      var validationResult = await validator.ValidateAsync(request);

      
      if (validationResult.Errors.Count > 0)
      {
        throw new ValidationException(validationResult);
      }
      else
      {
        await _bookingRepository.DeleteAsync(bookingToDelete);
      }

      return Unit.Value;
    }
  }
}
