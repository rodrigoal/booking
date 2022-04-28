using Cancun.Booking.Domain.Entities;

namespace Cancun.Booking.Application.Contracts.Persistence
{
  public interface ICountryRepository : IAsyncRepository<Country>
  {
    Task<IEnumerable<Country>> GetCountriesAsync();
    int? GetCountryIDAsync(string name);
  }
}
