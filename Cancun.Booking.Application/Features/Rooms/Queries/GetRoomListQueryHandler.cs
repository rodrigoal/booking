using AutoMapper;
using Cancun.Booking.Application.Contracts.Persistence;
using Cancun.Booking.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cancun.Booking.Application.Features.Rooms.Queries
{
  public class GetRoomListQueryHandler : IRequestHandler<GetRoomListQuery, List<RoomListVm>>
  {
    private readonly IMapper _mapper;
    private readonly IAsyncRepository<Room> _roomRepository;

    public GetRoomListQueryHandler(IMapper mapper, IAsyncRepository<Room> roomRepository)
    {
      _mapper = mapper;
      _roomRepository = roomRepository;
    }

    public async Task<List<RoomListVm>> Handle(GetRoomListQuery request, CancellationToken cancellationToken)
    {
      var room = (await _roomRepository.GetAllAsync()).ToList();
      return _mapper.Map<List<RoomListVm>>(room);
    }
  }
}
