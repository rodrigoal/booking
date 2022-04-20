using Cancun.Booking.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cancun.Booking.API.DbContexts
{
  public class BookingContext : DbContext
  {
    public DbSet<Country> Countries { get; set; }
    public DbSet<Cancun.Booking.API.Entities.Booking> Bookings { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<BookingLog> BookingLogs { get; set; }
    public DbSet<fn_getEmptyBookings> Fn_GetEmptyBookings { get; set; }
    public DbSet<Vw_Booking> Vw_Bookings { get; set; }


    public BookingContext(DbContextOptions<BookingContext> options) : base(options)
    {

    }
  }
}
