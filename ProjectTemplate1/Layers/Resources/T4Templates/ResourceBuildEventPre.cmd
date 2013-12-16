set ttFilePath=%1
set targetName=%2
set ttExeFile="%CommonProgramFiles%\microsoft shared\TextTemplating\12.0\TextTransform.exe"

%ttExeFile% %ttFilePath% -a !!targetName!%targetName% 

IF ERRORLEVEL  1 GOTO Label1
GOTO End

:Label1
	echo ERROR OCCURRED WHILE GENERATING "Template.Resources.Helpers.ResourceMvcHelper"
	echo REMEMBER TO CHECKOUT FILE "Template.Resources.Helpers.ResourceMvcHelper.cs"
	rem Si descomentamos la siguiente linea veremos los errores al compilar en Visual Studio
	rem type logFileErr.txt
	rem type logFileOut.txt
	GOTO End

:End

	rem Eliminamos los ficheros que hemos creado
	rem del logFileErr.txt
	rem del logFileOut.txt

	rem Limpiamos la salida y aseguramos que el codigo de resultado es "0" --> mas info en "CMD.exe /?"
	exit /b 0
