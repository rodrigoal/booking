/*
  Returns free dates to booking

*/
CREATE FUNCTION [dbo].[fn_getEmptyBookings]
(
  @startDate date
  ,@roomId int
)
RETURNS @returntable TABLE
(
  emptyDate date
)
AS
BEGIN
  
  declare @temp table (emptyDate date)
  declare @actualDate date = @startDate
  declare @endDate date = dateadd(dd,29, @startDate) -- 30 days minus 1 day that start.

  while @actualDate <= @endDate begin
    insert into @temp values (@actualDate)
    set @actualDate = DATEADD(DD,1, @actualDate)
  end
  
  insert @returntable
    select emptyDate from @temp
      where emptyDate not in (select bookingDate from BookingDetail bd 
                              join Booking b on b.id = bd.bookingId 
                              where bookingDate >= @startDate 
                                and b.roomId = @roomId)

  RETURN

END
