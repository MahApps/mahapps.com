///////////////////////////////////////////////////////////////////////////////
// TOOLS / ADDINS
///////////////////////////////////////////////////////////////////////////////

// None. Unlike Wyam, Statiq is not a CLI tool and has no Cake addin - the
// generator *is* the MahApps.Docs project in this repository, so the targets
// below just drive it with Cake's built-in DotNetRun alias.

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var genApi = Argument("api", false);
var configuration = Argument("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// PREPARATION
///////////////////////////////////////////////////////////////////////////////

var project = "./MahApps.Docs.csproj";

// The site is deployed to <host>/<LinkRoot>/, so LinkRoot makes every generated
// link absolute under that subdirectory. The preview server, however, serves
// ./output at "/" - without matching the two up, every asset and page 404s
// locally. Read it from statiq.json so there is only one source of truth.
string GetLinkRoot()
{
    var match = System.Text.RegularExpressions.Regex.Match(
        System.IO.File.ReadAllText("./statiq.json"),
        "\"LinkRoot\"\\s*:\\s*\"([^\"]*)\"");
    return match.Success ? match.Groups[1].Value.Trim('/') : string.Empty;
}

// Generating the API reference from the mahapps submodule dominates the build:
// ~2900 pages, ~3min, versus ~20s without. Program.cs reads the environment
// variable below and then skips the code analysis inputs entirely, the same way
// the old Wyam script only passed "SourceFiles" when --api was set.
void RunStatiq(string command, bool api, params string[] extraArgs)
{
    Information("Generating the site ({0} configuration, API reference {1})", configuration, api ? "included" : "skipped");

    var args = new ProcessArgumentBuilder();

    if (!string.IsNullOrEmpty(command))
    {
        args.Append(command);
    }

    foreach (var extraArg in extraArgs)
    {
        args.Append(extraArg);
    }

    DotNetRun(
        project,
        args,
        new DotNetRunSettings
        {
            Configuration = configuration,
            EnvironmentVariables = new Dictionary<string, string>
            {
                { "MAHAPPS_DOCS_SKIP_API", api ? "false" : "true" }
            }
        });
}

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
    Information("MahApps.Metro Docs");
});

Teardown(ctx =>
{
});

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

// Both honour --api; it is off by default because the API reference is what
// makes the build slow, and content work rarely needs it.
Task("Build")
    .Does(() =>
    {
        RunStatiq(null, genApi);
    });

Task("Preview")
    .Does(() =>
    {
        var linkRoot = GetLinkRoot();

        if (string.IsNullOrEmpty(linkRoot))
        {
            RunStatiq("preview", genApi);
        }
        else
        {
            Information("Serving under the LinkRoot virtual directory: http://localhost:5080/{0}", linkRoot);
            RunStatiq("preview", genApi, "--virtual-dir", linkRoot);
        }
    });

///////////////////////////////////////////////////////////////////////////////
// TASK TARGETS
///////////////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Preview")
    ;

// CI always publishes the full site, so it ignores --api entirely.
Task("CIBuild")
    .Does(() =>
    {
        RunStatiq(null, true);
    })
    ;

///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

RunTarget(target);
