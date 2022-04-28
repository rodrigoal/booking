using Cancun.Booking.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cancun.Booking.Domain.Entities
{
  [Table("BookingDetail")]
  public class BookingDetail : Entity
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public override int? Id { get; set; }

    [ForeignKey("BookingID")]
    public Reservation Booking { get; set; }
    public int? BookingID { get; set; }
    public DateTime BookingDate { get; set; }

  }
}
