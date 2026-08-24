Title: About
Description: What MahApps.Metro is, what you can build with it, and the people behind it
---

MahApps.Metro is a UI toolkit for WPF. It takes the standard Windows Presentation Foundation controls — the ones you already know and already use — and gives them a clean, modern look, then adds the pieces WPF never shipped: a proper window chrome, a theming engine, dialogs that do not block, and a set of navigation controls.

The project started on **29 January 2011** and has been going ever since. It is MIT licensed, has been part of the [.NET Foundation](https://dotnetfoundation.org) since 2020, and is used by thousands of desktop applications.

<div class="row text-center my-4">
    <div class="col-6 col-md-3"><div class="h3 mb-0 text-primary">2011</div><small class="text-muted">first commit</small></div>
    <div class="col-6 col-md-3"><div class="h3 mb-0 text-primary">9,800+</div><small class="text-muted">GitHub stars</small></div>
    <div class="col-6 col-md-3"><div class="h3 mb-0 text-primary">2,400+</div><small class="text-muted">forks</small></div>
    <div class="col-6 col-md-3"><div class="h3 mb-0 text-primary">MIT</div><small class="text-muted">licence</small></div>
</div>

## What makes it MahApps

**It restyles what you already have.** Most UI libraries ask you to swap your controls for theirs. MahApps.Metro does the opposite: reference it, merge two resource dictionaries, and your existing `Button`, `TextBox`, `DataGrid` and `TreeView` are restyled in place. No rewrite, no new control names, no vendor lock-in on your XAML.

**A window that actually looks like an app.** `MetroWindow` replaces the Windows title bar with one you control — your own commands to the left and right of the title, a coloured glow border, an overlay for dialogs, and flyouts that slide in from any edge.

**Theming is a first-class feature, not a stylesheet.** Themes are built from a base theme (Light or Dark) and an accent colour, switchable at runtime, and the `ThemeManager` can follow the Windows accent colour and the system's light/dark setting. You can generate your own themes from the same scheme the built-in ones use.

**Helpers instead of subclasses.** Watermarks, clear buttons, per-control corner radii, spell-check styling, selected-item brushes — these arrive as attached properties like `TextBoxHelper.Watermark`, so you attach behaviour to a stock control instead of inheriting from a special one.

**It is old in the good way.** Fifteen years of use in real applications, a stable v2 API, and an issue tracker with a long memory. The edge cases have been found.

## What you can build with it

<div class="row">
<div class="col-md-6">

**Windows and chrome.** `MetroWindow` with custom title-bar commands, glow borders, and `MetroNavigationWindow` for page-based navigation.

**Navigation.** A [HamburgerMenu](docs/controls/hamburgermenu) for the side-bar pattern, a [SplitView](docs/controls/splitview) underneath it, [Flyouts](docs/controls/flyouts) from any edge, and animated tab controls.

**Dialogs.** [Message, input, login and progress dialogs](docs/dialogs) shown as an overlay inside your window instead of a blocking modal — awaitable, and usable [from a view model](docs/dialogs/mvvm-dialog).

</div>
<div class="col-md-6">

**Input and data.** A [ColorPicker](docs/controls/colorpicker), [DateTimePicker](docs/controls/datetimepicker), [NumericUpDown](docs/controls/numericupdown), [MultiSelectionComboBox](docs/controls/multiselectioncombobox), [HotKeyBox](docs/controls/hotkeybox) and a restyled [DataGrid](docs/styles/datagrid).

**Feedback.** [Badges](docs/controls/badged), [progress bars and rings](docs/controls/metroprogressbar), [toggle switches](docs/controls/toggleswitch) and validation popups.

**Look and feel.** [Themes](docs/themes) you can switch at runtime and [style variants](docs/stylevariants) that follow Visual Studio, Windows 10 or WinUI conventions.

</div>
</div>

The [Quick Start](docs/guides/quick-start) gets you to a running `MetroWindow` in a few minutes.

MahApps.Metro 2.4 runs on .NET Framework 4.5.2 and newer as well as .NET Core 3.x, which is the asset current .NET applications resolve to. Version 3.0, available as a release candidate on NuGet, brings the window chrome up to date with Windows 11: the caption buttons are registered as non-client controls, so the maximise button takes part in snap layouts, and the window can use the newer backdrop materials.

## The people behind it

Presented in the order they first committed to the repository, with the dates and commit counts taken from its history.

<table class="table">
<thead><tr><th></th><th>Maintainer</th><th>Active</th><th>Commits</th></tr></thead>
<tbody>
<tr>
  <td><img src="https://github.com/vikingcode.png?size=48" width="48" height="48" alt="" class="rounded"></td>
  <td><strong>Paul Jenkins</strong><br><a href="https://github.com/vikingcode">@vikingcode</a></td>
  <td>Jan 2011 – Jan 2013</td><td>254</td>
</tr>
<tr>
  <td><img src="https://github.com/shiftkey.png?size=48" width="48" height="48" alt="" class="rounded"></td>
  <td><strong>Brendan Forster</strong><br><a href="https://github.com/shiftkey">@shiftkey</a></td>
  <td>Mar 2011 – Jul 2013</td><td>181</td>
</tr>
<tr>
  <td><img src="https://github.com/thoemmi.png?size=48" width="48" height="48" alt="" class="rounded"></td>
  <td><strong>Thomas Freudenberg</strong><br><a href="https://github.com/thoemmi">@thoemmi</a></td>
  <td>Jan 2012 – Feb 2017</td><td>106</td>
</tr>
<tr>
  <td><img src="https://github.com/flagbug.png?size=48" width="48" height="48" alt="" class="rounded"></td>
  <td><strong>Dennis Daume</strong><br><a href="https://github.com/flagbug">@flagbug</a></td>
  <td>Feb 2012 – Feb 2015</td><td>451</td>
</tr>
<tr>
  <td><img src="https://github.com/punker76.png?size=48" width="48" height="48" alt="" class="rounded"></td>
  <td><strong>Jan Karger</strong><br><a href="https://github.com/punker76">@punker76</a></td>
  <td>Dec 2012 – today</td><td>3,923</td>
</tr>
<tr>
  <td><img src="https://github.com/AzureKitsune.png?size=48" width="48" height="48" alt="" class="rounded"></td>
  <td><strong><a href="https://github.com/AzureKitsune">@AzureKitsune</a></strong></td>
  <td>Feb 2013 – Jun 2014</td><td>385</td>
</tr>
<tr>
  <td><img src="https://github.com/michaelmairegger.png?size=48" width="48" height="48" alt="" class="rounded"></td>
  <td><strong>Michael Mairegger</strong><br><a href="https://github.com/michaelmairegger">@michaelmairegger</a></td>
  <td>May 2013 – Jun 2017</td><td>287</td>
</tr>
<tr>
  <td><img src="https://github.com/batzen.png?size=48" width="48" height="48" alt="" class="rounded"></td>
  <td><strong>Bastian Schmidt</strong><br><a href="https://github.com/batzen">@batzen</a></td>
  <td>Feb 2017 – May 2024</td><td>111</td>
</tr>
<tr>
  <td><img src="https://github.com/timunie.png?size=48" width="48" height="48" alt="" class="rounded"></td>
  <td><strong>Tim</strong><br><a href="https://github.com/timunie">@timunie</a></td>
  <td>Jun 2019 – May 2023</td><td>53</td>
</tr>
</tbody>
</table>

**Paul Jenkins** started the project and wrote the first two years of it. **Brendan Forster** joined weeks later and shaped the early releases. **Thomas Freudenberg** and **Dennis Daume** carried it through the 2012–2015 period, Dennis with the second-largest body of work in the project after the current lead.

**Jan Karger** made his first commit in December 2012 and has maintained MahApps.Metro ever since — by a wide margin the most prolific contributor, and the person behind the v2 rewrite. **@AzureKitsune** and **Michael Mairegger** were the other main hands of the middle years, and **Tim** joined after v2.

**Bastian Schmidt** deserves a particular mention. He is the lead contributor to [ControlzEx](https://github.com/ControlzEx/ControlzEx), the library MahApps.Metro's window handling is built on, and the two projects have advanced together ever since — a good deal of what looks like MahApps behaviour is really ControlzEx underneath. He is also the driving force behind getting MahApps.Metro to behave correctly on Windows 11, from registering the caption buttons as non-client controls so that snap layouts work, to the backdrop materials.

Brendan Forster, @AzureKitsune, Bastian Schmidt and Jan Karger remain members of the [MahApps organisation](https://github.com/orgs/MahApps/people); Thomas Freudenberg and Tim are outside collaborators.

Beyond them, more than 160 people have contributed code, and many more have reported issues and answered questions. The full list is on the [contributors page](https://github.com/MahApps/MahApps.Metro/graphs/contributors).

## Getting involved

MahApps.Metro is developed in the open at [github.com/MahApps/MahApps.Metro](https://github.com/MahApps/MahApps.Metro). Issues and pull requests are welcome — including for this documentation, which lives in [its own repository](https://github.com/MahApps/mahapps.com) and has an edit link on every page.

The library is released under the MIT licence, copyright the .NET Foundation and contributors.
