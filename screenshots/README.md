# Screenshot generators

Small throwaway WPF apps that produce the control screenshots used in the
documentation. They are not part of the site build - Statiq never looks at this
folder - and CI does not run them. Regenerate the images by hand when a control
changes, then commit the PNGs together with the documentation.

Each app renders its scenarios straight to PNG with `RenderTargetBitmap` at 2x
scale rather than capturing the desktop. That keeps the images independent of
the machine's DPI setting, cropped exactly to what is being shown, and
reproducible - a rerun produces the same pixels, though the encoder does not
always write byte-identical files.

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

## HamburgerMenuShots

Produces the figures on `input/docs/controls/HamburgerMenu.md`:

```
dotnet run --project screenshots/HamburgerMenuShots -- input/docs/controls/images
```

One scenario embeds `input/assets/img/mahapps.metro.logo.png` as the thumbnail
of a `HamburgerMenuImageItem`, which is the other reason to run this from the
repository root; the app says so and carries on without it otherwise.

Unlike SplitViewShots this one writes its scenarios as XAML and loads them with
`XamlReader`. The control is driven almost entirely by templates, and spelling
those out in C# would obscure what the documentation is trying to show.

## DialogShots

Produces the figures for the pages under `input/docs/dialogs/`:

```
dotnet run --project screenshots/DialogShots -- input/docs/dialogs/images
```

It writes every dialog figure in one run, so expect the message dialog images
to be rewritten even when you only meant to change an input dialog one. They
come out the same unless the scenario changed.

A dialog is not a control that can be laid out on a canvas - it is shown into a
`MetroWindow`'s overlay. Each scenario therefore opens a real window off screen,
starts the dialog without awaiting the answer that never comes, renders the
window once it has settled, and composes those renders into one image.
`AnimateShow` is turned off so the capture is not a race.

Adding a figure for another dialog type means adding a scenario: the capture
takes a `Func<MetroWindow, Task>`, so anything you can start on a window fits.

Two things had to be pinned down to make these reproducible, both worth keeping
if you touch the capture code. Only one window can be the active one, so with
several panels in a figure the others would render with the inactive title bar
- the windows are opened with `ShowActivated = false` and the inactive brushes
are set to the active ones. And whether a button ends up wearing the dashed
focus adorner depends on where keyboard focus landed, so focus is cleared
before rendering.
