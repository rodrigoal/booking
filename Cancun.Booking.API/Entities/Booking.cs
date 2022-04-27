using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cancun.Booking.API.Entities
{
  [Table("Booking")]
  public class Booking : Entity
  {

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public override int? Id { get; set; }

    public int RoomID { get; set; }
    public int UserID { get; set; }
    public DateTime BookingStartDate { get; set; }
    public DateTime BookingEndDate { get; set; }
    public DateTime CreatedDate { get; set; }


    public ICollection<BookingDetail> BookingDetails { get; set; }
      = new List<BookingDetail>();

    public ICollection<BookingLog> BookingLogs { get; set; } 
      = new List<BookingLog>();
  }
}
