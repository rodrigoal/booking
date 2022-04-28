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
  public class RoomRepository : BaseRepository<Room>, IRoomRepository
  {
    public RoomRepository(BookingContext dbContext) : base(dbContext)
    {

    }
    public async Task<bool> ExistsAsync(int roomId)
    {
      return await _dbContext.Rooms.AnyAsync(a => a.Id == roomId);
    }

    public async Task<IEnumerable<Room>> GetRoomsAsync()
    {
      return await _dbContext.Rooms.OrderBy(a => a.RoomNumber).ToListAsync();
    }
  }
}
