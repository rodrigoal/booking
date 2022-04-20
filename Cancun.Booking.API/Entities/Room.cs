using System.ComponentModel.DataAnnotations.Schema;

namespace Cancun.Booking.API.Entities
{
  [Table("Room")]
  public class Room : Entity
  {
    public string RoomNumber { get; set; }

  }
}
