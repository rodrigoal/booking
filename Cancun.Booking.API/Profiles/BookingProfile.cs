﻿using AutoMapper;

namespace Cancun.Booking.API.Profiles
{
  public class BookingProfile : Profile
  {
    public BookingProfile()
    {
      CreateMap<Entities.Booking, Models.BookingDto>();
      CreateMap<Entities.Booking, Models.BookingListDto>();
      CreateMap<Models.BookingDto, Entities.Booking>();
      CreateMap<Models.BookingForCreationDto, Models.BookingDto>();
      CreateMap<Models.BookingForDeleteDto, Models.BookingDto>();
      CreateMap<Models.BookingForUpdateDto, Models.BookingDto>();
    }
  }
}
