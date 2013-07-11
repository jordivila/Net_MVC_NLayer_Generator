
@echo off
cls
set tmpServiceName=%2
set servicePath=%1

echo %tmpServiceName%
echo %servicePath%

echo Cheking existing windows service exists...
sc query | find /I "%tmpServiceName%" > nul

if not "%ERRORLEVEL%"=="1" (
	echo Service Found. Stopping it...
	net stop %tmpServiceName%
	echo Stopped. Uninstall begin...
	C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\installutil /u %servicePath%
) else (
    echo Program is not running
)


