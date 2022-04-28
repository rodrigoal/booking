using Cancun.Booking.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cancun.Booking.Domain.Entities
{
  [Table("BookingLog")]
  public class BookingLog : Entity
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public override int? Id { get; set; }

    [ForeignKey("BookingID")]
    public Reservation Booking { get; set; }

    public int BookingID { get; set; }
    public DateTime BookingStartDate { get; set; }
    public DateTime BookingEndDate { get; set; }
    public DateTime LogDate { get; set; }

  }
}
