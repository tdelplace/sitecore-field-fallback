@echo off
cls

set PWD=%~dp0
cd "%PWD%"

REM Install the FAKE package so we can run our build script
"..\lib\NuGet.exe" "Install" "FAKE" "-OutputDirectory" "packages" "-ExcludeVersion"
"packages\FAKE\tools\Fake.exe" build.fsx