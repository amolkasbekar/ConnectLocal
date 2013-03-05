If not exists (select 1 from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Login')
Begin
	CREATE TABLE [dbo].[Login](
		[User_Id] [int] IDENTITY(1,1) NOT NULL,
		[User_Name] [nvarchar](50) NOT NULL,
		[User_Password] [nvarchar](50) NULL,
		[OAuth_Token] [nvarchar](50) NULL,
		[Oauth_Expiration_date] [DateTime] NULL,
		[Instance_Url] [nvarchar](150) NULL,
		[OAuth_Refresh_Token] [nvarchar](50) NULL,
	 CONSTRAINT [PK_Login] PRIMARY KEY CLUSTERED 
	(
		[User_Id] ASC
	)) ON [PRIMARY]
End
GO

IF not exists (select 1 from sys.indexes where name = 'IX_UserName')
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX IX_UserName ON dbo.Login
		(
		User_Name
		) ON [PRIMARY]
	END

GO


