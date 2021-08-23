echo off
echo ------------------------------------------
echo -- DEPLOYING dev-marcrecords.r2library.com TO DEV       --
echo -- 1 - Uses msbuild.exe to publish site --
echo -- 2 - Copies site to technoserv05      --
echo -- 3 - Creates deployment package       --
echo ------------------------------------------

rem set attributes=/K /R /D /Y /S /L
set attributes=/K /R /D /Y /S

set sourceDir=.\Web
set destDir=\\technoserv02\d$\Clients\Rittenhouse\MarcRecordService\Site

echo Destination: %destDir%
pause

REM rmdir %sourceDir% /s /q

echo.
echo ----------------------
echo -- Building Web App --
echo ----------------------
"C:\Program Files (x86)\Microsoft Visual Studio\2017\BuildTools\MSBuild\15.0\Bin\msbuild.exe" "..\MarcRecordServiceSite\MarcRecordServiceSite.sln" /p:DeployOnBuild=true /p:PublishProfile=Profile1
pause

echo.
echo -----------------------
echo -- Stopping app pool --
echo -----------------------
.\psexec.exe \\technoserv02 C:\Windows\System32\inetsrv\appcmd.exe stop apppool /apppool.name:dev-marcrecords.r2library.com

echo.
set /p deleteAspx= Do you want to delete all .cshtml files from the destinations first (Y/N)?
if /I "%deleteAspx%" == "Y" del /S "%destDir%\Views\*.cshtml"

echo.
echo copying "%sourcedir%\*"
xcopy "%sourcedir%\*" "%destdir%\" %attributes% /exclude:DeployExcludes.txt

echo.
echo -----------------------
echo -- Starting app pool --
echo -----------------------
.\psexec.exe \\technoserv02 C:\Windows\System32\inetsrv\appcmd.exe start apppool /apppool.name:dev-marcrecords.r2library.com

rem --------------------------------------------------
rem create 7zip file - start
@echo off
For /f "tokens=2-4 delims=/ " %%a in ('date /t') do (set currentDate=%%c-%%a-%%b)
echo Current Date: %currentDate%

echo.
set /p deleteAspx= Create deployment 7zip (Y/N)?
if /I "%deleteAspx%" == "Y" set /p zipSuffix= Enter unique 7zip file name suffix (a thru z)?
if /I "%deleteAspx%" == "Y" "C:\Program Files\7-Zip\7z.exe" a -t7z MarcProject.Web-%currentDate%%zipSuffix%.7z %sourceDir%
rem create 7zip file - end
rem --------------------------------------------------

pause