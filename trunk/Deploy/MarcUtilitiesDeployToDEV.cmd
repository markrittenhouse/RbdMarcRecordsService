rem echo off
rem set attributes=/K /R /D /Y /S /L
set attributes=/K /R /D /Y /S

set sourceDir=..\MarcRecordServiceApp\MarcRecordServiceApp\bin\Debug
set localDir=.\MarcUtilities

rmdir "%localDir%" /q /s
mkdir "%localDir%"
xcopy "%sourceDir%\*" "%localDir%\" %attributes% /exclude:MarcUtilitiesCopyExcludes.txt

echo.
echo.
echo MarcUtilities copy has been copied locally, please press any key to deploy this code to DEV
pause

set destDir=\\technoserv02\d$\Clients\Rittenhouse\MarcRecordService\App

xcopy "%localDir%\*" "%destDir%\" %attributes% /exclude:MarcUtilitiesDeployExcludes.txt

rem --------------------------------------------------
rem create 7zip file - start
@echo off
For /f "tokens=2-4 delims=/ " %%a in ('date /t') do (set currentDate=%%c-%%a-%%b)
echo Current Date: %currentDate%

echo.
set /p deleteAspx= Create deployment 7zip (Y/N)?
if "%deleteAspx%" == "Y" set /p zipSuffix= Enter unique 7zip file name suffix (a thru z)?
if "%deleteAspx%" == "Y" "C:\Program Files\7-Zip\7z.exe" a -t7z MarcProject.Utilities-%currentDate%%zipSuffix%.7z %localDir%
rem create 7zip file - end
rem --------------------------------------------------

pause




