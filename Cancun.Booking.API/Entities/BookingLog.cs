using System.ComponentModel.DataAnnotations.Schema;

namespace Cancun.Booking.API.Entities
{
  [Table("BookingLog")]
  public class BookingLog : Entity
  {
    public int BookingID { get; set; }
    public DateTime BookingStartDate { get; set; }
    public DateTime BookingEndDate { get; set; }
    public DateTime LogDate { get; set; }

  }
}
