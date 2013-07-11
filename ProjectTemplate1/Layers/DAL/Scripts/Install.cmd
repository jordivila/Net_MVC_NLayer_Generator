set DatabaseServerInstance=IO_JV
set DatabaseNameAspNetMembership=CurlyDevelopmentMembership
set DatabaseNameEntLibLogging=CurlyDevelopmentLogging
set ApplicationAdminEmail=admin@admin.com
set ApplicationAdminPassword=123456
set ApplicationName=/

sqlcmd	-S "%DatabaseServerInstance%" -E -v DatabaseName="%DatabaseNameAspNetMembership%" -i ./AspNetServices/AspNetMembershipDatabase.sql
sqlcmd	-S "%DatabaseServerInstance%" -E -d %DatabaseNameAspNetMembership% -i ./AspNetServices/AspNetMembershipInitAdminUser.sql
sqlcmd	-S "%DatabaseServerInstance%" -E -d %DatabaseNameAspNetMembership% -i ./AspNetServices/AspNetMembershipUserSearch.sql
sqlcmd	-S "%DatabaseServerInstance%" -E -v DatabaseName="%DatabaseNameEntLibLogging%" -i ./EnterpriseLibrary/LoggingDatabase.sql
sqlcmd	-S "%DatabaseServerInstance%" -E -d "%DatabaseNameEntLibLogging%" -i ./EnterpriseLibrary/LoggingExceptionGetAll.sql
sqlcmd	-S "%DatabaseServerInstance%" -E -d "%DatabaseNameEntLibLogging%" -i ./EnterpriseLibrary/LoggingExceptionGetById.sql