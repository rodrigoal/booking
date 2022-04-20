using Cancun.Booking.API.DbContexts;
using Cancun.Booking.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cancun.Booking.API.Repository
{
  public class RoomRepository : IRoomRepository
  {
    private readonly BookingContext _context;

    public RoomRepository(BookingContext context)
    { 
      _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<bool> ExistsAsync(int roomId)
    {
      return await _context.Rooms.AnyAsync(a => a.Id == roomId);
    }

    public async Task<IEnumerable<Room>> GetRoomsAsync()
    {

      return await _context.Rooms.OrderBy(a => a.RoomNumber).ToListAsync();
      
    }

    
  }
}
