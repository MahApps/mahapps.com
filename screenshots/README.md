# Screenshot generators

Small throwaway WPF apps that produce the control screenshots used in the
documentation. They are not part of the site build - Statiq never looks at this
folder - and CI does not run them. Regenerate the images by hand when a control
changes, then commit the PNGs together with the documentation.

Each app renders its scenarios straight to PNG with `RenderTargetBitmap` at 2x
scale rather than capturing the desktop. That keeps the images free of window
chrome, independent of the machine's DPI setting, cropped exactly to the
control, and identical on every run.

The apps reference MahApps.Metro from NuGet at the same version the `mahapps`
submodule is pinned to, so the screenshots match the API reference.

## SplitViewShots

Produces the figures on `input/docs/controls/SplitView.md`:

```
dotnet run --project screenshots/SplitViewShots -- input/docs/controls/images
```

Run it from the repository root: `dotnet run --project` keeps the caller's
working directory, so the output path is resolved against wherever you are, not
against the project folder. Without an argument the images land in a `shots`
folder next to you, which `.gitignore` covers.

Passing `--probe` additionally runs two diagnostics that verify the documented
`PaneClosing` behaviour: cancelling the event keeps the pane on screen but
leaves `IsPaneOpen` at `false`, and the veto only works once unless the handler
restores the property itself.

```
dotnet run --project screenshots/SplitViewShots -- --probe
```
