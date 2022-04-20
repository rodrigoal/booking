CREATE PROCEDURE [dbo].[pr_genBooking]
  @id INT = null
  ,@roomId int = null
  ,@userId int = null
  ,@bookingStartDate date = null
  ,@bookingEndDate date = null
  ,@createdDate smalldatetime = null
  ,@operation char(1) = 'i'
AS
BEGIN

  begin try
    

    if @operation = 'i' begin
      begin tran
      
      set @createdDate = getdate()
      insert into dbo.Booking (roomId, userId, bookingStartDate, bookingEndDate, createdDate)
        values (@roomId, @userId, @bookingStartDate, @bookingEndDate, @createdDate)
      
      select @id = SCOPE_IDENTITY()

      while @bookingStartDate <= @bookingEndDate begin
        insert into dbo.BookingDetail(bookingId, bookingDate)
          values (@id, @bookingStartDate)
        set @bookingStartDate = dateadd(dd, 1, @bookingStartDate)
      end

      commit
      select @id as id
    end
    else begin
      if @operation = 'u' begin
        begin tran
          
          --Logging previous values
          insert into dbo.BookingLog (bookingId, roomId, bookingStartDate, bookingEndDate, logDate)
            select @id, roomId, bookingStartDate, bookingEndDate, getdate() from Booking
              where id = @id
          
          -- updating values
          update dbo.Booking set
            roomId = @roomId
            ,bookingStartDate = @bookingStartDate
            ,bookingEndDate = @bookingEndDate
            where id = @id

          -- recalculating date's table values
          delete from BookingDetail where bookingId = @id

          while @bookingStartDate <= @bookingEndDate begin
            insert into dbo.BookingDetail(bookingId, bookingDate)
              values (@id, @bookingStartDate)
            set @bookingStartDate = dateadd(dd, 1, @bookingStartDate)
          end

        commit
        
      end
      else begin
        if @operation = 'd' begin

          begin tran
          delete from BookingLog where bookingId = @id
          delete from BookingDetail where bookingId = @id
          delete from Booking where id = @id

          commit

        end

      end

    end  

  end try
  begin catch
    
    SELECT   
         ERROR_NUMBER() AS ErrorNumber  
        ,ERROR_SEVERITY() AS ErrorSeverity  
        ,ERROR_STATE() AS ErrorState  
        ,ERROR_LINE () AS ErrorLine  
        ,ERROR_PROCEDURE() AS ErrorProcedure  
        ,ERROR_MESSAGE() AS ErrorMessage;  
    
    if @@TRANCOUNT > 0 begin
      rollback
    end

  end catch

  
END



