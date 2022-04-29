using AutoMapper;

using Cancun.Booking.Application.Contracts.Persistence;
using Cancun.Booking.Application.Features.Rooms.Queries;
using Cancun.Booking.Application.Profiles;
using Cancun.Booking.Domain.Entities;
using Cancun.Booking.Persistence;
using Cancun.Booking.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Cancun.Booking.API.Tests.Rooms.Queries
{
  public class GetRoomListQueryHandlerTests
  {
    private readonly IMapper _mapper;
    private readonly BookingContext _bookingContext;
    private DbContextOptions<BookingContext> _dbContextOptions;
    private readonly IRoomRepository _roomRepository;

    public GetRoomListQueryHandlerTests()
    {
      //_mockRoomRepository = RepositoryMocks.GetRoomRepository();
      var configurationProvider = new MapperConfiguration(cfg =>
      {
        cfg.AddProfile<RoomProfile>();
      });
      _mapper = configurationProvider.CreateMapper();


      var dbName = $"BookingDB_{DateTime.Now.ToFileTimeUtc()}";

      _dbContextOptions = new DbContextOptionsBuilder<BookingContext>()
          .UseInMemoryDatabase(dbName)
          .Options;

      _bookingContext = new BookingContext(_dbContextOptions);
      _roomRepository = new RoomRepository(_bookingContext);

      ConfigureDbContext();

    }

    private void ConfigureDbContext()
    {

      _bookingContext.Rooms.Add(new Room() { Id = 1, RoomNumber = "001" });
      _bookingContext.SaveChanges();

    }

    [Fact]
    public async Task GetRoomListTest()
    {

      var handler = new GetRoomListQueryHandler(_mapper, _roomRepository);
      var result = await handler.Handle(new GetRoomListQuery(), System.Threading.CancellationToken.None);

      Assert.IsType<List<RoomListVm>>(result);
      Assert.Single(result);

    }

  }
}
