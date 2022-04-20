CREATE TABLE dbo.Room
(
  id INT IDENTITY(1,1) NOT NULL 
  ,roomNumber varchar(10) not null
  ,CONSTRAINT PK_Room PRIMARY KEY(id)
)
