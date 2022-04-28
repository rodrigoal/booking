using Cancun.Booking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cancun.Booking.Persistence
{
  public class BookingContext : DbContext
  {
    public DbSet<Country> Countries { get; set; }
    public DbSet<Reservation> Bookings { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<BookingDetail> BookingDetails { get; set; }
    public DbSet<BookingLog> BookingLogs { get; set; }



    public BookingContext(DbContextOptions<BookingContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<Room>().HasData(
        new Room()
        {
          Id = 1,
          RoomNumber = "001"
        });

      modelBuilder.Entity<Country>().HasData(new Country()
      {
        Id = 27,
        Name = "Brasil"
      });

      modelBuilder.Entity<User>().HasData(new User()
      {
        Id = 1,
        CountryID = 27,
        Passport = "123"
      });

    }
  }
}
