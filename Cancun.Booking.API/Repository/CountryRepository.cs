using Cancun.Booking.API.DbContexts;
using Cancun.Booking.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cancun.Booking.API.Repository
{
  public class CountryRepository : ICountryRepository
  {
    private readonly BookingContext _context;

    public CountryRepository(BookingContext context)
    { 
      _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task<IEnumerable<Country>> GetCountriesAsync()
    {

      return await _context.Countries.OrderBy(a => a.Name).ToListAsync();
      
    }
    public int? GetCountryIDAsync(string name)
    {

      return _context.Countries.Where(a => a.Name == name).FirstOrDefault()?.Id;

    }
    
  }
}
