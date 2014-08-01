// include Fake lib
#r @"packages/FAKE/tools/FakeLib.dll"
open Fake
open Fake.Git

// Properties
let buildDir = "./build/"

// Targets
Target "Clean" (fun _ ->
    CleanDir buildDir
)

Target "BuildApp" (fun _ ->
    !! "FieldFallback.sln"
      |> MSBuild buildDir "Build"  [("Configuration","Release"); ("PackageVersion", "9.9.9")]
      |> Log "AppBuild-Output: "
    //trace "p"
)

Target "SetVersion" (fun _ ->
    
    // Run GitVersion so it updates the assembly info
    let result = 
        ExecProcess (fun info ->
                    info.FileName <- "../lib/GitVersion.exe" 
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

    trace semVer

//    !! "src/app/**/*.scproj"
//        |>
//        let tdsProj = new System.Xml.XmlDocument() in
//            tdsProj.LoadXml xml;
//        
//
//    XPathReplace("") (System.Xml.XmlDocument)
    // TODO: Modify the .scproj file
)

Target "Default" (fun _ ->
    trace "Completed building the Field Fallback module"
   
)

// Dependencies
"Clean"
  ==> "SetVersion"
  ==> "BuildApp"
  ==> "Default"

// start build
RunTargetOrDefault "Default"