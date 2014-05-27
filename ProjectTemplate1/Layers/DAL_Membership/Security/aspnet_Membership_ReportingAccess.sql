CREATE ROLE aspnet_Membership_ReportingAccess


GO
--ALTER ROLE [aspnet_Membership_ReportingAccess] ADD MEMBER [aspnet_Membership_FullAccess];


GO
EXEC sp_addrolemember N'aspnet_Membership_ReportingAccess', N'aspnet_Membership_FullAccess'