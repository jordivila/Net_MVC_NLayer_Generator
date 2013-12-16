rem %~1 --> remove quotes from ProjectDir Path
set projectDir=%~1
set ttExePath="%CommonProgramFiles%\microsoft shared\TextTemplating\12.0\TextTransform.exe"
set ttFolderName=Common\T4
set listItems=Template_T4_BuildEvent_JS_Ajax.tt;Template_T4_BuildEvent_JS_Intellisense.tt;Template_T4_BuildEvent_CDN_JS_Generator.tt;Template_T4_BuildEvent_CDN_CSS_Generator.tt;




FOR %%A IN (%listItems%) DO "%projectDir%%ttFolderName%\BuildEventBatchFiles\Template_T4_BuildEvent__TransformBatchItem" %ttExePath% "%projectDir%" %ttFolderName% %%A

rem Clean output and ensure result is "0" --> more info at "CMD.exe /?"
exit /b 0
