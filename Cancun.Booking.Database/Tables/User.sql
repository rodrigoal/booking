CREATE TABLE [dbo].[User]
(
  id INT IDENTITY(1,1) NOT NULL 
  ,passport varchar(50) not null
  ,countryId int not null
  ,CONSTRAINT PK_User PRIMARY KEY(id)
  ,constraint FK_User_countryId FOREIGN Key (countryId) references dbo.Country(id)
)
