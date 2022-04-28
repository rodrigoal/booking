using Cancun.Booking.Application.Contracts.Persistence;
using Cancun.Booking.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cancun.Booking.Application.Features.Bookings.Commands.AddBooking
{
  public class AddBookingCommandValidator : AbstractValidator<AddBookingCommand>
  {
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;

    public AddBookingCommandValidator(IBookingRepository bookingRepository, IRoomRepository roomRepository)
    {

      _bookingRepository = bookingRepository;
      _roomRepository = roomRepository;

      RuleFor(p => p.BookingStartDate)
        .NotEmpty()
        .LessThanOrEqualTo(p => p.BookingEndDate)
        .WithMessage("The end date must be greater than start date.");

      RuleFor(p => p)
        .MustAsync(ExistsBooking)
        .WithMessage("Unfortunately some dates of reservation are not available for now.");

      RuleFor(p => p)
        .MustAsync(RoomNotExists)
        .WithMessage("This room does not exists. Choose another one.");

      RuleFor(p => p)
        .MustAsync(ReservationMoreThan3Days)
        .WithMessage("Your reservation can not have more than 3 days. Sorry.");

      RuleFor(p => p)
        .MustAsync(StartAtLeastNextDay)
        .WithMessage("The bookings must start at least on the next day.");

      RuleFor(p => p)
        .MustAsync(ReservationMustEndNext30Days)
        .WithMessage("The bookings must end at least next 30 days.");
    }

    private Task<bool> ReservationMustEndNext30Days(AddBookingCommand arg1, CancellationToken arg2)
    {
      return Task.Run(() => !(arg1.BookingEndDate.Date > DateTime.Now.Date.AddDays(30)));
    }

    private Task<bool> StartAtLeastNextDay(AddBookingCommand arg1, CancellationToken arg2)
    {
      return Task.Run(() => !(arg1.BookingStartDate.Date < DateTime.Now.Date.AddDays(1)));
    }

    private Task<bool> ReservationMoreThan3Days(AddBookingCommand arg1, CancellationToken arg2)
    {
      return Task.Run(() => !((arg1.BookingEndDate.Date - arg1.BookingStartDate.Date).TotalDays > 2));
    }

    private async Task<bool> RoomNotExists(AddBookingCommand arg1, CancellationToken arg2)
    {
      return (await _roomRepository.ExistsAsync(arg1.RoomID));

    }

    private async Task<bool> ExistsBooking(AddBookingCommand e, CancellationToken token)
    {
      return !(await _bookingRepository.BookingExistsAsync(e.BookingStartDate, e.BookingEndDate));
    }




 
  }
}
