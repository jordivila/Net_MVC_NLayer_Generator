
set ttExePath=%1
rem %~2 --> remove quotes from ProjectDir Path
set projectDir=%~2
set ttFolderName=%3
set ttFileName=%4

rem Replace the ".tt" file extension by ".result" file extension & store it in a variable 
rem This "result" extension file is defined in Template_T4_BuildEvent_Base.tt as the output value
set tempTTResultFileName=%ttFileName:.tt=.result%	

rem		/*******************************************************************************************************************************************/ 
rem		If, for whatever reason, you want to execute a command from a Visual Studio build event, and you don't care if that command returns an error, then do the following: 
rem		[command to execute] 2>nul 1>nul 
rem		Mas info sobre redireccion de ficherios en http://technet.microsoft.com/en-us/library/bb490982.aspx 
rem		/*******************************************************************************************************************************************/ 


%ttExePath% "%projectDir%%ttFolderName%\%ttFileName%" 

IF ERRORLEVEL  1 GOTO Label1 

GOTO End 

:Label1 
	echo ERROR OCCURRED WHILE GENERATING "%ttFileName%" 
	GOTO End 
 
:End 

	rem Remove temporary files
	rem del /Q %projectDir%\%ttFolderName%\%tempTTResultFileName%
