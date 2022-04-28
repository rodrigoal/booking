using Cancun.Booking.Application.Contracts.Persistence;
using Cancun.Booking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cancun.Booking.Persistence.Repositories
{
  public class CountryRepository : BaseRepository<Country>, ICountryRepository
  {
    public CountryRepository(BookingContext dbContext) : base(dbContext)
    {

    }
    public async Task<IEnumerable<Country>> GetCountriesAsync()
    {
      return await _dbContext.Countries.OrderBy(a => a.Name).ToListAsync();
    }

    public int? GetCountryIDAsync(string name)
    {
      return _dbContext.Countries.Where(a => a.Name == name).FirstOrDefault()?.Id;
    }
  }
}
