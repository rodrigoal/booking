using Cancun.Booking.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cancun.Booking.Domain.Entities
{
  [Table("Room")]
  public class Room : Entity
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public override int? Id { get; set; }
    [Required]
    public string RoomNumber { get; set; }

  }
}
