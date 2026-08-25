# Screenshot generators

Small throwaway WPF apps that produce the control screenshots used in the
documentation. They are not part of the site build - Statiq never looks at this
folder - and CI does not run them. Regenerate the images by hand when a control
changes, then commit the PNGs together with the documentation.

Each app renders its scenarios straight to PNG with `RenderTargetBitmap` at 2x
scale rather than capturing the desktop. That keeps the images independent of
the machine's DPI setting, cropped exactly to what is being shown, and
near enough reproducible that a rerun can be diffed. It is not exact, so diff a
rewritten PNG before committing it rather than assuming the rerun found
something.

Measured over a full rerun, every figure came back pixel-identical except
these three:

- `progressdialog-indeterminate` - the bar animates, so it is caught at
  whatever phase it happened to be in. Expect this one to differ every time.
- `logindialog-basic` and `logindialog-variants` - about a thousand pixels
  each, all of them inside the watermark text, which is drawn at `Opacity=0.6`
  by `MahApps.Styles.TextBlock.Watermark`. The largest channel difference
  measured was 9 of 255 and the whole run came out uniformly lighter, so it is
  a rounding difference in compositing that semi-transparent layer, not a
  change in what is drawn. Invisible side by side.

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

## StyleShots

Produces the figures for the pages under `input/docs/styles/` and
`input/docs/helper/`. Unlike the other apps its argument is the docs root, not
one images folder - each figure names the section it belongs to and is written
to `<docs-root>/<section>/images/`:

```
dotnet run --project screenshots/StyleShots -- input/docs
```

Like HamburgerMenuShots it writes its scenarios as XAML and loads them with
`XamlReader`, so the markup in the figure and the markup in the documentation
are the same. It writes every figure for both sections in one run.

Two things a `PasswordBox` scenario cannot express in XAML and that the app
therefore does after the window is up. `Password` is not a dependency property,
and setting it earlier would also be before the style has attached
`PasswordBoxBindingBehavior`, which is what feeds the hidden text box the reveal
button's visibility hangs off. And the caps lock indicator is shown by the
library looking the template part up and setting its visibility from a key
handler - an off-screen render never presses a key, so the same part is driven
directly.

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
The progress dialog scenarios use that to drive the returned controller, and
one of them presses the real cancel button - found in the visual tree by its
template part name - because the disabled-button state the documentation warns
about cannot be reached any other way.

The custom dialog figures are the exception to this app being written in C#:
their dialogs are parsed from XAML with `XamlReader`, because the page prints
that same XAML as its sample. Keep them that way so the figure and the sample
cannot drift apart.

Two things had to be pinned down to make these reproducible, both worth keeping
if you touch the capture code. Only one window can be the active one, so with
several panels in a figure the others would render with the inactive title bar
- the windows are opened with `ShowActivated = false` and the inactive brushes
are set to the active ones. And whether a button ends up wearing the dashed
focus adorner depends on where keyboard focus landed, so focus is cleared
before rendering.
