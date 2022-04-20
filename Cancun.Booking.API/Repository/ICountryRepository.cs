using Cancun.Booking.API.Entities;

namespace Cancun.Booking.API.Repository
{
  public interface ICountryRepository
  {
    Task<IEnumerable<Country>> GetCountriesAsync();
    int? GetCountryIDAsync(string name);
  }
}
