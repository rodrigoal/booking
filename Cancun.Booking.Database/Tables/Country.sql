CREATE TABLE [dbo].[Country]
(
  id INT IDENTITY(1,1) NOT NULL 
  ,[name] varchar(100) not null
  ,CONSTRAINT PK_Country PRIMARY KEY(id)
)
