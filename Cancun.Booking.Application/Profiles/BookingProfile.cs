using AutoMapper;
using Cancun.Booking.Application.Features.Bookings.Commands.AddBooking;
using Cancun.Booking.Application.Features.Bookings.Commands.DeleteBooking;
using Cancun.Booking.Application.Features.Bookings.Commands.UpdateBooking;
using Cancun.Booking.Application.Features.Bookings.Queries.GetBookingList;

namespace Cancun.Booking.Application.Profiles
{
  public class BookingProfile : Profile
  {
    public BookingProfile()
    {
      //CreateMap<Domain.Entities.Booking, Models.BookingDto>().ReverseMap();
      //CreateMap<Domain.Entities.Booking, Models.BookingListDto>();
      //CreateMap<Models.BookingDto, Entities.Booking>();
      CreateMap<AddBookingCommand, Domain.Entities.Reservation>().ReverseMap();
      CreateMap<UpdateBookingCommand, Domain.Entities.Reservation>().ReverseMap();
      CreateMap<DeleteBookingCommand, Domain.Entities.Reservation>().ReverseMap();
      CreateMap<BookingListDto, Domain.Entities.Reservation>().ReverseMap();
      CreateMap<AddBookingDto, Domain.Entities.Reservation>().ReverseMap();
      //CreateMap<Models.BookingForDeleteDto, Models.BookingDto>();
      //CreateMap<Models.BookingForUpdateDto, Models.BookingDto>();
    }
  }
}
