/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

declare @featureName varchar(128)

set @featureName = 'common'

if not exists(select * from [dbo].[aspnet_SchemaVersions] where Feature = @featureName)
begin
	insert into [dbo].[aspnet_SchemaVersions] VALUES (@featureName, 1, 1)
end

set @featureName = 'membership'

if not exists(select * from [dbo].[aspnet_SchemaVersions] where Feature = @featureName)
begin
	insert into [dbo].[aspnet_SchemaVersions] VALUES (@featureName, 1, 1)
end

set @featureName = 'profile'

if not exists(select * from [dbo].[aspnet_SchemaVersions] where Feature = @featureName)
begin
	insert into [dbo].[aspnet_SchemaVersions] VALUES (@featureName, 1, 1)
end

set @featureName = 'role manager'

if not exists(select * from [dbo].[aspnet_SchemaVersions] where Feature = @featureName)
begin
	insert into [dbo].[aspnet_SchemaVersions] VALUES (@featureName, 1, 1)
end
