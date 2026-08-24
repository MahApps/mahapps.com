using Statiq.App;
using Statiq.Common;
using Statiq.Docs;

// Replaces config.wyam. Everything declarative lives in statiq.json;
// only the code-analysis inputs need the bootstrapper.

// Generating the API reference is what makes the build slow: ~2900 extra pages,
// three minutes instead of twenty seconds. The old Wyam script skipped it by
// not passing "SourceFiles" at all, and we do the same rather than just
// suppressing the output - otherwise the API index page is still generated and
// links to pages that were never written. build.cake sets this for every target
// except CIBuild; see its --api argument.
bool skipApi = string.Equals(
    Environment.GetEnvironmentVariable("MAHAPPS_DOCS_SKIP_API"),
    "true",
    StringComparison.OrdinalIgnoreCase);

Bootstrapper bootstrapper = Bootstrapper
    .Factory
    .CreateDocs(args)

    // The MahApps.Metro submodule is source input for the API docs, not
    // site content - keep Statiq from reading it as pages.
    .AddExcludedPath("mahapps")

    .AddSetting(DocsKeys.ApiPath, "api");

if (!skipApi)
{
    bootstrapper = bootstrapper
        // Same glob as the old build.cake "SourceFiles" setting. Statiq
        // resolves this relative to the input path, so "../mahapps" is
        // <root>/mahapps.
        .AddSourceFiles(
            "../mahapps/src/**/{!bin,!obj,!packages,!*.Tests,!*.Build,!*.Samples,!Microsoft.Windows.Shell,}/**/*.cs")
        .AddSetting(DocsKeys.OutputApiDocuments, true);
}

return await bootstrapper.RunAsync();
