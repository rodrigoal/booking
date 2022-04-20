CREATE VIEW [dbo].[vw_booking]
AS 
SELECT b.id
  ,b.userId
  ,b.roomId
  ,b.bookingStartDate
  ,b.bookingEndDate
  ,b.createdDate
  ,userPassport = u.passport
  ,userCountryName = c.[name]
  ,r.roomNumber
FROM dbo.Booking b
Join dbo.[User] u on u.id = b.userId
Join dbo.Room r on r.id = b.roomId
join dbo.Country c on c.id = u.countryId