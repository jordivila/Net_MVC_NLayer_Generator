@echo off
cls
set tmpServiceName=%2
set servicePath=%1

echo Checking Windows Service is still running
sc query | find /I "%tmpServiceName%" > nul

if not "%ERRORLEVEL%"=="1" (
	echo Program is running. Something went wrong on prebuild
) else (
	echo Installing service...
	C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\installutil %servicePath%
	echo starting service...
	net start %tmpServiceName%
)
