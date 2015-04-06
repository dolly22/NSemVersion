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
IF EXIST packages\Sake goto run
packages\NuGet.exe install Sake -o packages -ExcludeVersion

:run
packages\sake\tools\sake.exe -I build/sake -f makefile.shade %*
