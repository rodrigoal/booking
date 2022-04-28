using AutoMapper;
using Cancun.Booking.Application.Features.Countries.Queries;

namespace Cancun.Booking.Application.Profiles
{
  public class CountryProfile : Profile
  {
    public CountryProfile()
    {
      CreateMap<Domain.Entities.Country, CountryListVm>().ReverseMap();
    }
  }
}
