using Cancun.Booking.API.Entities;

namespace Cancun.Booking.API.Repository
{
  public interface IBookingRepository
  {
    Task<IEnumerable<Entities.Booking>> GetBookingListAsync(int userId);
    Task<Entities.Booking?> GetBookingAsync(int bookingId);
    IEnumerable<Entities.fn_getEmptyBookings> GetEmptyBookingsAsync();
    Task<bool> BookingExistsAsync(DateTime startDate, DateTime endDate, int? bookingId);

    Task<Entities.Booking?> GetBookingByStartDateAsync(DateTime startDate, int roomId);

    Task AddBooking(Entities.Booking booking);
    Task DeleteBooking(Entities.Booking booking);

    Task UpdateBooking(int bookingId, Entities.Booking booking);

    Task<bool> SaveChangesAsync();
    int GetUserID(string userPassport, int countryID);
  }
}
