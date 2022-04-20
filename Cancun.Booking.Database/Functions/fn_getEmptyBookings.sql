/*
  Returns free dates to booking

*/
CREATE FUNCTION [dbo].[fn_getEmptyBookings]
(
  @startDate date
)
RETURNS @returntable TABLE
(
  emptyDate date
)
AS
BEGIN
  
  declare @temp table (emptyDate date)
  declare @actualDate date = @startDate
  declare @endDate date = dateadd(dd,29, @startDate)

  while @actualDate <= @endDate begin
    insert into @temp values (@actualDate)
    set @actualDate = DATEADD(DD,1, @actualDate)
  end
  
  insert @returntable
    select emptyDate from @temp
      where emptyDate not in (select bookingDate from BookingDetail where bookingDate >= @startDate)

  RETURN

END
