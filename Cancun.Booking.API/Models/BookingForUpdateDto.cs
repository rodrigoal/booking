namespace Cancun.Booking.API.Models
{
  public class BookingForUpdateDto
  {
    public int RoomID { get; set; }
    public string UserPassport { get; set; }
    public int CountryID { get; set; }
    public DateTime BookingStartDate { get; set; }
    public DateTime BookingEndDate { get; set; }
  }
}
