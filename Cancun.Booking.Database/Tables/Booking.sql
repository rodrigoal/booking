CREATE TABLE [dbo].[Booking]
(
  id INT IDENTITY(1,1) NOT NULL 
  ,roomId int not null
  ,userId int not null
  ,bookingStartDate date not null
  ,bookingEndDate date not null
  ,createdDate smalldatetime not null
  ,CONSTRAINT PK_Booking PRIMARY KEY(id)
  ,constraint FK_Booking_roomId FOREIGN Key (roomId) references dbo.Room(id)
  ,constraint FK_Booking_userId FOREIGN Key (userId) references dbo.[User](id)
)
