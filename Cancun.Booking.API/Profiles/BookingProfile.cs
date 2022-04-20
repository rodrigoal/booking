using AutoMapper;

namespace Cancun.Booking.API.Profiles
{
  public class BookingProfile : Profile
  {
    public BookingProfile()
    {
      CreateMap<Entities.fn_getEmptyBookings, Models.EmptyDateDto>();
      CreateMap<Entities.Booking, Models.BookingDto>();
      CreateMap<Entities.Booking, Models.BookingListDto>();
      CreateMap<Models.BookingDto, Entities.Booking>();
      CreateMap<Models.BookingForCreationDto, Models.BookingDto>();
    }
  }
}
