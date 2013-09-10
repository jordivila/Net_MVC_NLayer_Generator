
set DatabaseServerInstance=%~1
set DatabaseNameAspNetMembership=%~2
set DatabaseNameEntLibLogging=%~3
set ApplicationAdminEmail=%~4
set ApplicationAdminPassword=%~5
set ApplicationName=%~6
set CurrentScriptFullPath=%~7

sqlcmd	-S "%DatabaseServerInstance%" -E -v DatabaseName="%DatabaseNameAspNetMembership%" -i "%CurrentScriptFullPath%\AspNetServices\AspNetMembershipDatabase.sql"
sqlcmd	-S "%DatabaseServerInstance%" -E -d %DatabaseNameAspNetMembership% -i "%CurrentScriptFullPath%\AspNetServices\AspNetMembershipInitAdminUser.sql"
sqlcmd	-S "%DatabaseServerInstance%" -E -d %DatabaseNameAspNetMembership% -i "%CurrentScriptFullPath%\AspNetServices\AspNetMembershipUserSearch.sql"
sqlcmd	-S "%DatabaseServerInstance%" -E -v DatabaseName="%DatabaseNameEntLibLogging%" -i "%CurrentScriptFullPath%\EnterpriseLibrary\LoggingDatabase.sql"
sqlcmd	-S "%DatabaseServerInstance%" -E -d "%DatabaseNameEntLibLogging%" -i "%CurrentScriptFullPath%\EnterpriseLibrary\LoggingExceptionGetAll.sql"
sqlcmd	-S "%DatabaseServerInstance%" -E -d "%DatabaseNameEntLibLogging%" -i "%CurrentScriptFullPath%\EnterpriseLibrary\LoggingExceptionGetById.sql"

