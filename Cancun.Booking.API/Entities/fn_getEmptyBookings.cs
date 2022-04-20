using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cancun.Booking.API.Entities
{
  [Table("fn_getEmptyBookings")]
  public class fn_getEmptyBookings
  {
    [Key]
    public DateTime EmptyDate { get; set; }
  }
}
