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
  public class UserRepository : BaseRepository<User>, IUserRepository
  {
    public UserRepository(BookingContext dbContext) : base(dbContext)
    {

    }

    public async Task<User?> GetUserAsync(string passport, int countryId)
    {
      return await _dbContext.Users.Where(a => a.Passport == passport && a.CountryID == countryId).FirstOrDefaultAsync();
    }

    public async Task<int> GetUserID(string passport, int countryID)
    {

      if (await GetUserAsync(passport, countryID) == null)
        await _dbContext.Users.AddAsync(new User() { Passport = passport, CountryID = countryID });

      return (int)(await _dbContext.Users.Where(a => a.Passport == passport && a.CountryID == countryID).FirstOrDefaultAsync()).Id;
    }
  }
}
