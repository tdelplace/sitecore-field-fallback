@echo off
cls

set PWD=%~dp0
cd "%PWD%"

REM Install the FAKE package so we can run our build script
".\.nuget\NuGet.exe" "Install" "FAKE" "-OutputDirectory" "packages" "-ExcludeVersion"
".\.nuget\NuGet.exe" "Install" "GitVersion" "-OutputDirectory" "packages" "-ExcludeVersion"
".\.nuget\NuGet.exe" "Install" "GitVersion.CommandLine" "-OutputDirectory" "packages" "-ExcludeVersion"

"packages\FAKE\tools\Fake.exe" build.fsx