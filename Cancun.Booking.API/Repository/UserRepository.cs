using Cancun.Booking.API.DbContexts;
using Cancun.Booking.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cancun.Booking.API.Repository
{
  public class UserRepository : IUserRepository
  {

    private readonly BookingContext _context;
    public UserRepository(BookingContext context)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task AddUser(User user)
    {
      var u = await GetUserAsync(user.Passport, user.CountryID);
      if (u != null)
      {
        _context.Users.Add(user);
      }
     
    }

    public async Task<User?> GetUserAsync(string passport, int countryId)
    {
      return await _context.Users.Where(a => a.Passport == passport && a.CountryID == countryId).FirstOrDefaultAsync();
    }

    public async Task<bool> SaveChangesAsync()
    {
      return await _context.SaveChangesAsync() >= 0;
      
    }

  
  }
}
