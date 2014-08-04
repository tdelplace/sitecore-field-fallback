// include Fake lib
#r @"packages/FAKE/tools/FakeLib.dll"
open Fake
open Fake.Git

// Properties
let buildDir = "./build/"
//let gitVersionToolPath = "./Tools/GitVersion.exe" 
//let nugetPath = "../lib/nuget.exe"
let mutable packageVersion = "0.0.0" // The version we will use for the TDS .update packages. This will be assigned a value in the SetVersion target

//let RestorePackageParamF = 
//  fun _ ->{ ToolPath = nugetPath
//            Sources = []
//            TimeOut = System.TimeSpan.FromMinutes 5.
//            OutputPath = "./packages" 
//            Retries = 1
//           } :Fake.RestorePackageHelper.RestorePackageParams
//
//
//// override default
//let RestorePackages2() = 
//  !! "./**/packages.config"
//  |> Seq.iter (RestorePackage RestorePackageParamF)



// Targets
Target "Clean" (fun _ ->
    CleanDir buildDir
)

Target "RestorePackages" (fun _ ->
    RestorePackages()

    // Assume Chocolatey is installed so that GitVersion can be installed
    // http://chocolatey.org/
    // https://github.com/Particular/GitVersion/wiki/Command-Line-Tool
    cinst GitVersion
)

Target "BuildApp" (fun _ ->
 
    // restore NuGet packages
    //RestorePackages()
    //RestorePackages2()

    !! "FieldFallback.sln"
      |> MSBuild buildDir "Build"  [("Configuration","Release"); ("PackageVersion", packageVersion)]
      |> Log "AppBuild-Output: "
    //trace "p"
)

Target "SetVersion" (fun _ ->
    
    // Run GitVersion so it updates the assembly info
    let result = 
        ExecProcess (fun info ->
                    info.FileName <- "GitVersion.exe" // -- assumed in path via Chocolatey **//gitVersionToolPath
                    info.WorkingDirectory <- "." 
                    info.Arguments <- "/output buildserver /updateassemblyinfo"
                    ) (System.TimeSpan.FromMinutes 5.0) 
    
    if result <> 0 then failwith "Failed setting assembly version via GitVersion"
    
    // Run it simply to get version info out to the screen
    // This allows us to get the SemVer so we can modify the .scproj file
    let result = 
        ExecProcessAndReturnMessages (fun info ->
                    info.FileName <- "../lib/GitVersion.exe" 
                    info.WorkingDirectory <- "." 
                    info.Arguments <- ""
                    ) (System.TimeSpan.FromMinutes 5.0) 
    let fullString = 
        String.concat "," result.Messages

    let semVer =
        (result.Messages.Item(9))
    let semVer =
        semVer.Split [|':'|] 

    let semVer =
        semVer.[1].Replace("\"", "").Replace(",", "")

    // set the global variable
    packageVersion <- semVer

    trace semVer
)

Target "Default" (fun _ ->
    trace "Completed building the Field Fallback module"
)

// Dependencies
"Clean"
  ==> "RestorePackages"
  ==> "SetVersion"
  ==> "BuildApp"
  ==> "Default"

// start build
RunTargetOrDefault "Default"