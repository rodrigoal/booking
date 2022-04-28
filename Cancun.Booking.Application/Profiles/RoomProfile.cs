using AutoMapper;
using Cancun.Booking.Application.Features.Rooms.Queries;
using Cancun.Booking.Domain.Entities;

namespace Cancun.Booking.Application.Profiles
{
  public class RoomProfile : Profile
  {
    public RoomProfile()
    {
      CreateMap<Room, RoomListVm>().ReverseMap();
    }
  }
}
