declare @applicationName varchar(256)
declare @roleGuestName varchar(256)
declare @roleAdminName varchar(256)
declare @applicationId uniqueIdentifier
declare @adminUserName varchar(256)
declare @adminUserId uniqueidentifier 
declare @adminUserRoles varchar(512)
declare @adminUserPassword sql_variant
declare @adminUserPasswordQuestion varchar(256)
declare @adminUserPasswordQuestionAnswer varchar(256)
declare @adminUserSalt nvarchar(128)
declare @adminUserPasswordBinary varbinary(256)
declare @adminUserPasswordHashBytes varbinary(512)
declare @adminUserPasswordHash varchar(64)
declare @dateTimeNow datetime

set @applicationName = N'$(ApplicationName)'
set @adminUserName = N'$(ApplicationAdminEmail)'
set @adminUserPassword = N'$(ApplicationAdminPassword)'
set @adminUserPasswordQuestion = N'Password security question does not exists for this user'
set @adminUserPasswordQuestionAnswer = N'1238947wnjr897shn48nh12to7813bg87strb837btr6x846z'
set @roleGuestName = N'Guest'
set @roleAdminName = N'Administrator'
set @adminUserRoles = @roleGuestName + ',' + @roleAdminName
set @dateTimeNow = GETDATE()

begin try
	begin transaction

		-- BEGIN INIT APPLICATION
			if not exists(select * from dbo.aspnet_Applications where ApplicationName = @applicationName)
			begin
				set @applicationId = NEWID()
				exec aspnet_Applications_CreateApplication @applicationName, @applicationId
			end
			else begin
				select @applicationId = applicationId from aspnet_Applications where ApplicationName = @applicationName
			end
		-- END INIT APPLICATION

		-- BEGIN INIT ROLES
			if not exists(select * from dbo.aspnet_Roles where ApplicationId = @applicationId and RoleName = @roleGuestName)
			begin
				exec aspnet_Roles_CreateRole @applicationName, @roleGuestName
			end

			if not exists(select * from dbo.aspnet_Roles where ApplicationId = @applicationId and RoleName = @roleAdminName)
			begin
				exec aspnet_Roles_CreateRole @applicationName, @roleAdminName
			end
		-- END INIT ROLES

		--BEGIN INIT USER 
			IF not exists(select * from aspnet_Users where ApplicationId = @applicationId and UserName = @adminUserName )
			begin
			
				set quoted_identifier on
			
				set @adminUserSalt = N'EpmAM9gkaHGZxDdrxwrEeQ=='
				set @adminUserPasswordBinary = convert(varbinary(256),@adminUserPassword)
				set @adminUserPasswordHashBytes = hashbytes('sha1',cast('' as  xml).value('xs:base64Binary(sql:variable(''@adminUserSalt''))','varbinary(256)') + @adminUserPasswordBinary)
				set @adminUserPasswordHash = cast('' as xml).value('xs:base64Binary(xs:hexBinary(sql:variable(''@adminUserPasswordHashBytes'')))','varchar(64)')

				

				exec [dbo].[aspnet_Membership_CreateUser] @ApplicationName = @applicationName, 
														  @UserName = @adminUserName, 
														  @Password = @adminUserPasswordHash, 
														  @PasswordSalt = @adminUserSalt, 
														  @Email = @adminUserName, 
														  @PasswordQuestion = @adminUserPasswordQuestion, 
														  @PasswordAnswer = @adminUserPasswordQuestionAnswer, 
														  @IsApproved = 1, 
														  @CurrentTimeUtc = @dateTimeNow, 
														  @CreateDate = @dateTimeNow, 
														  @UniqueEmail = 1, 
														  @PasswordFormat = 1, 
														  @UserId = @adminUserId OUTPUT 


				 exec dbo.[aspnet_UsersInRoles_AddUsersToRoles] @applicationName, @adminUserName, @adminUserRoles, @dateTimeNow

			end 

		commit transaction;

		--select * from aspnet_Users			where UserName = @adminUserName
		--select * from aspnet_Membership		where Email = @adminUserName
		--select * from aspnet_Roles
		--select * from aspnet_UsersInRoles	where UserId = (select UserId from aspnet_Users where ApplicationId = @applicationId and UserName = @adminUserName)

end try
begin catch

    declare @errormessage nvarchar(max);
    declare @errorseverity int;
    declare @errorstate int;

    select 
        @errormessage = error_message(),
        @errorseverity = error_severity(),
        @errorstate = error_state();

    raiserror (@errormessage, -- message text.
               @errorseverity, -- severity.
               @errorstate -- state.
               );

	rollback transaction;

end catch