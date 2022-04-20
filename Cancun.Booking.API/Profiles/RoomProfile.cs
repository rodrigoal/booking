using AutoMapper;

namespace Cancun.Booking.API.Profiles
{
  public class RoomProfile : Profile
  {
    public RoomProfile()
    {
      CreateMap<Entities.Room, Models.RoomDto>();
    }
  }
}
