using Cancun.Booking.API.Entities;

namespace Cancun.Booking.API.Repository
{
  public interface IRoomRepository
  {
    Task<IEnumerable<Room>> GetRoomsAsync();
  }
}
