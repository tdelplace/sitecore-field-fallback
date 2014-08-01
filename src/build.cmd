@echo off
cls

set PWD=%~dp0
cd "%PWD%"

"..\lib\NuGet.exe" "Install" "FAKE" "-OutputDirectory" "packages" "-ExcludeVersion"
"packages\FAKE\tools\Fake.exe" build.fsx
pause