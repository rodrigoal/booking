using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cancun.Booking.API.Entities
{
  [Table("User")]
  public class User : Entity
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public override int? Id { get; set; }
    [Required]
    public string Passport { get; set; }
    [Required]
    public int CountryID { get; set; }

  }
}
