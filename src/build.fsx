// include Fake lib
#r @"packages/FAKE/tools/FakeLib.dll"
#r @"packages/GitVersion/Lib/Net45/GitVersionCore.dll"
open Fake
open Fake.Git

// Properties
let buildDir = "./build/"
let buildConfiguration = "Release"
let gitVersionToolPath = "packages/GitVersion.CommandLine/Tools/GitVersion.exe" 

let mutable packageVersion = "0.0.0" // The version we will use for the TDS .update packages. This will be assigned a value in the SetVersion target

// Targets
Target "Clean" (fun _ ->
    CleanDir buildDir
)

Target "RestorePackages" (fun _ ->
    RestorePackages()
)

Target "BuildApp" (fun _ ->
    !! "FieldFallback.sln"
      |> MSBuild buildDir "Build"  [("Configuration",buildConfiguration); ("PackageVersion", packageVersion)]
      |> Log "AppBuild-Output: "
)

Target "SetVersion" (fun _ ->
    
    // Run GitVersion so it updates the assembly info
    let result = 
        ExecProcess (fun info ->
                    info.FileName <- gitVersionToolPath
                    info.WorkingDirectory <- "." 
                    info.Arguments <- "/output buildserver /updateassemblyinfo"
                    ) (System.TimeSpan.FromMinutes 5.0) 
    
    if result <> 0 then failwith "Failed setting assembly version via GitVersion"
    
    // Use the GitVersion assembly to get the version so we can pass it to MSBUILD
    //let semanticVersion = GitVersion.VersionCache.GetVersion "../.git"
    //let variables = GitVersion.VariableProvider.GetVariablesFor(semanticVersion, GitVersion.AssemblyVersioningScheme.MajorMinorPatch, true)
    //packageVersion <- variables.["FullSemVer"]
    
    // ^^^ There appears to be a bug with the assembly version where it required LibGit2Sharp
    
    // so....
    // Run it simply to get version info out to the screen
    // This allows us to get the SemVer so we can modify the .scproj file
    let result = 
        ExecProcessAndReturnMessages (fun info ->
                    info.FileName <- gitVersionToolPath 
                    info.WorkingDirectory <- "." 
                    info.Arguments <- ""
                    ) (System.TimeSpan.FromMinutes 5.0) 
    let fullString = 
        String.concat "," result.Messages

    let semVer =
        (result.Messages.Item(14))
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