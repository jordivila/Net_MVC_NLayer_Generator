
set DatabaseServerInstance=%~1
set DatabaseNameAspNetMembership=%~2
set DatabaseNameEntLibLogging=%~3
set DatabaseNameTokenPersistence=%~4
set ApplicationAdminEmail=%~5
set ApplicationAdminPassword=%~6
set ApplicationName=%~7
set CurrentScriptFullPath=%~8

sqlcmd	-S "%DatabaseServerInstance%" -E -v DatabaseName="%DatabaseNameAspNetMembership%" -i "%CurrentScriptFullPath%\AspNetServices\AspNetMembershipDatabase.sql"
sqlcmd	-S "%DatabaseServerInstance%" -E -d %DatabaseNameAspNetMembership% -i "%CurrentScriptFullPath%\AspNetServices\AspNetMembershipInitAdminUser.sql"
sqlcmd	-S "%DatabaseServerInstance%" -E -d %DatabaseNameAspNetMembership% -i "%CurrentScriptFullPath%\AspNetServices\AspNetMembershipUserSearch.sql"

sqlcmd	-S "%DatabaseServerInstance%" -E -v DatabaseName="%DatabaseNameTokenPersistence%" -i "%CurrentScriptFullPath%\TokenPersistence\TokenPersistence.sql"
sqlcmd	-S "%DatabaseServerInstance%" -E -d %DatabaseNameTokenPersistence% -i "%CurrentScriptFullPath%\TokenPersistence\TokenPersistence.sql"

sqlcmd	-S "%DatabaseServerInstance%" -E -v DatabaseName="%DatabaseNameEntLibLogging%" -i "%CurrentScriptFullPath%\EnterpriseLibrary\LoggingDatabase.sql"
sqlcmd	-S "%DatabaseServerInstance%" -E -d "%DatabaseNameEntLibLogging%" -i "%CurrentScriptFullPath%\EnterpriseLibrary\LoggingExceptionGetAll.sql"
sqlcmd	-S "%DatabaseServerInstance%" -E -d "%DatabaseNameEntLibLogging%" -i "%CurrentScriptFullPath%\EnterpriseLibrary\LoggingExceptionGetById.sql"

