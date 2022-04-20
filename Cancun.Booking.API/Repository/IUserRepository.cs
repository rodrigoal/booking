namespace Cancun.Booking.API.Repository
{
  public interface IUserRepository
  {
    Task<Entities.User?> GetUserAsync(string passport, int countryId);
    
    Task AddUser(Entities.User user);

    Task<bool> SaveChangesAsync();
  }
}
