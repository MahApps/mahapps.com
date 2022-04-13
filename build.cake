///////////////////////////////////////////////////////////////////////////////
// TOOLS / ADDINS
///////////////////////////////////////////////////////////////////////////////

#tool nuget:?package=Wyam&version=2.2.9

#addin nuget:?package=Cake.Figlet&version=2.0.1
#addin nuget:?package=Cake.Wyam&version=2.2.13

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var genApi = Argument("api", false);

///////////////////////////////////////////////////////////////////////////////
// PREPARATION
///////////////////////////////////////////////////////////////////////////////

var isLocal = BuildSystem.IsLocalBuild;
var settings = new Dictionary<string, object>
  {
      { "SourceFiles",  "../mahapps/src/**/{!bin,!obj,!packages,!*.Tests,!*.Build,!*.Samples,!Microsoft.Windows.Shell,}/**/*.cs" }
  };

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
  Information(Figlet("MahApps.Metro Docs"));
});

Teardown(ctx =>
{
});

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Build")
    .Does(() =>
    {
      Wyam(new WyamSettings
        {
          Recipe = "Docs",
          Theme = "Samson",
          UpdatePackages = false,
          UseGlobalSources = false,
          UseLocalPackages = true,
          Settings = settings
        });
    });
    
Task("Preview")
    .Does(() =>
    {
      var wyamSettings = new WyamSettings
        {
          Recipe = "Docs",
          Theme = "Samson",
          UpdatePackages = true,
          UseGlobalSources = false,
          UseLocalPackages = true,
          Preview = true,
          Watch = true
        };

      if (genApi)
      {
        wyamSettings.Settings = settings;
      }

      Wyam(wyamSettings);
    });

///////////////////////////////////////////////////////////////////////////////
// TASK TARGETS
///////////////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Preview")
    ;

Task("CIBuild")
    .IsDependentOn("Build")
    ;

///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

RunTarget(target);
