using Cancun.Booking.Domain.Entities;

namespace Cancun.Booking.Application.Contracts.Persistence
{
  public interface IUserRepository : IAsyncRepository<User>
  {

    Task<User?> GetUserAsync(string passport, int countryId);

    Task<int> GetUserID(string userPassport, int countryID);
  }
}
