rem echo off
rem set attributes=/K /R /D /Y /S /L
set attributes=/K /R /D /Y /S

set sourceDir=..\MarcRecordServiceApp\bin\Debug
set destDir=.\MarcRecordServiceApp

rmdir "%destDir%" /q /s
mkdir "%destDir%"
xcopy "%sourceDir%\*" "%destDir%\" %attributes% /exclude:GetLatestCodeForDeployExcludes.txt

rem copy code into the 'code' directory
mkdir "%destDir%\code"
mkdir "%destDir%\code\References"
mkdir "%destDir%\code\MarcRecordServiceApp"

xcopy "..\References\*" "%destDir%\code\References" %attributes% 
xcopy "..\MarcRecordServiceApp\*" "%destDir%\code\MarcRecordServiceApp" %attributes% /exclude:GetLatestCodeForDeployExcludes2.txt

"c:\program files\7-zip\7z.exe" a -t7z "MarcRecordServiceApp\Code.7z" "%destDir%\code"
rmdir "%destDir%\code" /q /s

"c:\program files\7-zip\7z.exe" a -t7z "MarcRecordServiceApp.7z" "%destDir%"

pause
