@echo off
cd %~dp0

SETLOCAL
SET CACHED_NUGET=%LocalAppData%\NuGet\NuGet.exe

IF EXIST %CACHED_NUGET% goto copynuget
echo Downloading latest version of NuGet.exe...
IF NOT EXIST %LocalAppData%\NuGet md %LocalAppData%\NuGet
@powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest 'https://www.nuget.org/nuget.exe' -OutFile '%CACHED_NUGET%'"

:copynuget
IF EXIST packages\nuget.exe goto restore
md packages
copy %CACHED_NUGET% packages\nuget.exe > nul

:restore
IF EXIST packages\Cake goto run
echo Installing cake build
packages\NuGet.exe install Cake -version 0.2.2 -o packages -ExcludeVersion

:run
echo Starting cake build
packages\Cake\Cake.exe build.cake %*
