using Cancun.Booking.Domain.Entities;

namespace Cancun.Booking.Application.Contracts.Persistence
{
  public interface IRoomRepository : IAsyncRepository<Room>
  {
    Task<IEnumerable<Room>> GetRoomsAsync();
    Task<bool> ExistsAsync(int roomId);
  }
}
