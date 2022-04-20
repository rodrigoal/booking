CREATE TABLE [dbo].[BookingLog]
(
  id INT IDENTITY(1,1) NOT NULL 
  ,bookingId int not null
  ,roomId int not null
  ,bookingStartDate date not null
  ,bookingEndDate date not null
  ,logDate smalldatetime not null
  ,CONSTRAINT PK_BookingLog PRIMARY KEY(id)
  ,constraint FK_BookingLog_bookingId FOREIGN Key (bookingId) references dbo.Booking(id)
  ,constraint FK_BookingLog_roomId FOREIGN Key (roomId) references dbo.Room(id)
)