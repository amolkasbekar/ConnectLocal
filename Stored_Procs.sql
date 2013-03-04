if exists (select *  from sysobjects  where id = object_id(N'dbo.CONNECT_SaveUser') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  drop procedure dbo.CONNECT_SaveUser;
go

CREATE PROCEDURE dbo.CONNECT_SaveUser
(
	@userName nvarchar(50),
	-- Password is not required to update user records
	@userPassword nvarchar(50) = null, 
	@token nvarchar(50) = null,
	@expiration_date datetime = null,
	@instanceUrl nvarchar(150) = null,
	@refreshToken nvarchar(50) = null
)
AS
BEGIN
	declare @userId int;
	
	select @userId = USER_ID from Login where USER_NAME = @userName;

	If (@userId is null)
	BEGIN
		INSERT INTO Login (User_Name, User_Password, OAuth_Token, Oauth_Expiration_date, Instance_Url, OAuth_Refresh_Token)
		VALUES (@userName, @userPassword, @token, @expiration_date, @instanceUrl, @refreshToken);
	END
	else
	BEGIN
		update Login
		set User_Name = @userName,
			OAuth_Token = @token,
			Oauth_Expiration_date = @expiration_date,
			Instance_Url = @instanceUrl,
			OAuth_Refresh_Token = @refreshToken
		WHERE
			USER_ID = @userId
	END
	
	return @userId;
END
go


if exists (select *  from sysobjects  where id = object_id(N'dbo.CONNECT_GetUser') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  drop procedure dbo.CONNECT_GetUser;
go

CREATE PROCEDURE dbo.CONNECT_GetUser(@userName nvarchar(50))
AS
BEGIN
	select user_id, user_name, user_password, OAuth_Token, Oauth_Expiration_date, Instance_Url, OAuth_Refresh_Token
	from Login
	where user_name = @userName;
END
	