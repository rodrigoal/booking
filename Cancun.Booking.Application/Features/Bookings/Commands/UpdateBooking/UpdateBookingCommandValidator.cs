using Cancun.Booking.Application.Contracts.Persistence;
using Cancun.Booking.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cancun.Booking.Application.Features.Bookings.Commands.UpdateBooking
{
  internal class UpdateBookingCommandValidator : AbstractValidator<UpdateBookingCommand>
  {
    private readonly IBookingRepository _bookingRepository;
    private readonly IUserRepository _userRepository;

    public UpdateBookingCommandValidator(IBookingRepository bookingRepository, IUserRepository userRepository)
    {

      _bookingRepository = bookingRepository;
      _userRepository = userRepository;


      RuleFor(p => p)
        .MustAsync(ExistsBooking)
        .WithMessage("The booking ID does not exist.");

      RuleFor(p => p)
        .MustAsync(IsMyBoooking)
        .WithMessage("You can not update/delete a booking that not yours.");

    }

    private async Task<bool> IsMyBoooking(UpdateBookingCommand arg1, CancellationToken arg2)
    {
      var userId = await _userRepository.GetUserID(arg1.UserPassport, arg1.CountryID);
      var booking = await _bookingRepository.GetBookingAsync(arg1.ID);
      return (userId == booking?.UserID);

    }

    private async Task<bool> ExistsBooking(UpdateBookingCommand e, CancellationToken token)
    {

      return ((await _bookingRepository.GetBookingAsync(e.ID)) == null);
    }


  }
}
