using AutoMapper;

namespace Cancun.Booking.API.Profiles
{
  public class CountryProfile : Profile
  {
    public CountryProfile()
    {
      CreateMap<Entities.Country, Models.CountryDto>();
    }
  }
}
