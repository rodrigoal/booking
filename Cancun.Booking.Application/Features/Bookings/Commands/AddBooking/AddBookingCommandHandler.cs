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

namespace Cancun.Booking.Application.Features.Bookings.Commands.AddBooking
{
  public class AddBookingCommandHandler : IRequestHandler<AddBookingCommand, AddBookingCommandResponse>
  {
    private readonly IMapper _mapper;
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;

    public AddBookingCommandHandler(IMapper mapper, IBookingRepository bookingRepository, IRoomRepository roomRepository)
    {
      _mapper = mapper;
      _bookingRepository = bookingRepository;
      _roomRepository = roomRepository;
    }

    public async Task<AddBookingCommandResponse> Handle(AddBookingCommand request, CancellationToken cancellationToken)
    {

      var addBookingCommandResponse = new AddBookingCommandResponse();

      var validator = new AddBookingCommandValidator(_bookingRepository, _roomRepository);
      var validationResult = await validator.ValidateAsync(request);

      if (validationResult.Errors.Count > 0)
      {
        addBookingCommandResponse.Success = false;
        addBookingCommandResponse.ValidationErrors = new List<string>();
        foreach (var error in validationResult.Errors)
        {
          addBookingCommandResponse.ValidationErrors.Add(error.ErrorMessage);
        }

      }
      else
      {
        var booking = _mapper.Map<Domain.Entities.Reservation>(request);
        booking = await _bookingRepository.AddAsync(booking);
        addBookingCommandResponse.Booking = _mapper.Map<AddBookingDto>(booking);
      }

      return addBookingCommandResponse;
    }
  }
}
