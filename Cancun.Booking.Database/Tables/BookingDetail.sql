CREATE TABLE [dbo].[BookingDetail]
(
  id INT IDENTITY(1,1) NOT NULL 
  ,bookingId int not null
  ,bookingDate date not null
  ,CONSTRAINT PK_BookingDetail PRIMARY KEY(id)
  ,constraint FK_BookingDetail_bookingId FOREIGN Key (bookingId) references dbo.Booking(id)
)