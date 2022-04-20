using Cancun.Booking.API.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Cancun.Booking.API.Repository
{
  public class BookingRepository : IBookingRepository
  {
    private readonly BookingContext _context;
    public BookingRepository(BookingContext context)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task AddBooking(Entities.Booking booking)
    {

      //_context.Bookings.Add(booking);
      await _context.Database.ExecuteSqlRawAsync("exec pr_genBooking @roomId = {0}, @userId ={1}, @bookingStartDate = {2}, @bookingEndDate = {3}",
                                                               booking.RoomID, booking.UserID, booking.BookingStartDate, booking.BookingEndDate);
      

    }

    public async Task UpdateBooking(int bookingId, Entities.Booking booking)
    {
      await _context.Database.ExecuteSqlRawAsync("exec pr_genBooking @id={0}, @roomId = {1}, @bookingStartDate = {2}, @bookingEndDate = {3}, @operation = 'u'",
                                                               bookingId, booking.RoomID, booking.BookingStartDate, booking.BookingEndDate);
    }

    public async Task<bool> BookingExistsAsync(DateTime startDate, DateTime endDate, int? bookingId)
    {

      bool exists = false;
      if (bookingId != null)
        exists = await _context.Bookings.AnyAsync(a => (a.Id != bookingId) && ((startDate >= a.BookingStartDate && startDate <= a.BookingEndDate) || (endDate >= a.BookingStartDate && endDate <= a.BookingEndDate)));
      else
        exists = await _context.Bookings.AnyAsync(a => ((startDate >= a.BookingStartDate && startDate <= a.BookingEndDate) || (endDate >= a.BookingStartDate && endDate <= a.BookingEndDate)));

      return exists;

    }

    public async Task DeleteBooking(Entities.Booking booking)
    {
      await _context.Database.ExecuteSqlRawAsync("exec pr_genBooking @id = {0}, @operation = 'd'", booking.Id!);
                                                               
      //_context.Bookings.Remove(booking);

    }

    public async Task<Entities.Booking?> GetBookingAsync(int bookingId)
    {
      return await _context.Bookings.FirstOrDefaultAsync(a => a.Id == bookingId);
    }
    public async Task<Entities.Booking?> GetBookingByStartDateAsync(DateTime startDate, int roomId)
    {
      return await _context.Bookings.FirstOrDefaultAsync(a => a.BookingStartDate == startDate && a.RoomID == roomId);
    }

    public async Task<IEnumerable<Entities.Booking>> GetBookingListAsync(int userId)
    {
      return await _context.Bookings.Where(a => a.UserID == userId).ToListAsync();
    }


    public IEnumerable<Entities.fn_getEmptyBookings> GetEmptyBookingsAsync()
    {
      DateTime startDate = DateTime.Now.AddDays(1);
     
      var bookings =  _context.Fn_GetEmptyBookings.FromSqlRaw("select emptyDate from dbo.fn_getEmptyBookings({0})", startDate);

      return bookings;

    }

    public int GetUserID(string userPassport, int countryID)
    {

      var user = _context.Users.Where(a => a.Passport == userPassport && a.CountryID == countryID).FirstOrDefault();
      if (user == null)
      {
        _context.Users.Add(new Entities.User() { Passport = userPassport, CountryID = countryID });
        _context.SaveChanges();
      }
      user = _context.Users.Where(a => a.Passport == userPassport && a.CountryID == countryID).FirstOrDefault();
      
      return user.Id.Value;
    }

    public async Task<bool> SaveChangesAsync()
    {
      return await _context.SaveChangesAsync() >= 0;
    }

   
  }
}
