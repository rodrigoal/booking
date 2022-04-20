using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cancun.Booking.API.Entities
{
  [Table("vw_booking")]
  public class Vw_Booking : Entity
  {

    public int UserID { get; set; }
    public int RoomID { get; set; }
    public DateTime BookingStartDate { get; set; }
    public DateTime BookingEndDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public string UserPassport { get; set; }
    public string UserCountryName { get; set; }

    public string RoomNumber { get; set; }



  }
}
