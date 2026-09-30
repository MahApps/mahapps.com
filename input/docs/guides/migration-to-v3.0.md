Order: 15
Title: Migration to v3.0
Description: What changed between 2.4.11 and v3, and what to do about it
---

This guide covers the step from 2.4.11 to 3.0. It is a much smaller step than [the one to v2.0](migration-to-v2.0) was: a handful of names went, a few more changed shape, and most of what is new is new rather than different.

The part worth reading twice is [what changed without telling you](#what-changed-without-telling-you). Everything above it the compiler or the XAML parser will point at; everything in it compiles and then behaves differently.

:::{.alert .alert-info}
v3 is not released yet. What is described here is `develop` and the 3.0.0 prereleases, and it can still move before it ships.
:::

## Start with the target framework

This is the first thing to settle, because nothing else matters until the package restores.

| | 2.4.11 | v3 |
| --- | --- | --- |
| .NET Framework | 4.5.2, 4.6, 4.7 | **4.6.2** |
| .NET | Core 3.0, Core 3.1 | **6.0 and 8.0**, Windows |
| ControlzEx | 4.x | **7.0** |

An application on .NET Framework 4.5.2 or 4.6 has to move to 4.6.2 or later. One on .NET Core 3.1 has to move to .NET 6 or later.

ControlzEx going from 4 to 7 is what carries most of the window changes below, and it is worth knowing that `ThemeManager`, `Theme` and `LibraryTheme` are its types rather than MahApps ones. That was already true in 2.4.11.

## The window

`MetroWindow` derives from `ControlzEx.WindowChromeWindow` now rather than straight from `Window`. Most of what moved moved quietly: `IgnoreTaskbarOnMaximize`, `KeepBorderOnMaximize`, `ResizeBorderThickness`, `ShowMinButton` and `ShowMaxRestoreButton` are properties of that base now and are set on a `MetroWindow` exactly as before.

Three do not.

| 2.4.11 | v3 |
| --- | --- |
| `GlowBrush` | `GlowColor`, a `Color?` |
| `NonActiveGlowBrush` | `NonActiveGlowColor`, a `Color?` |
| `UseNoneWindowStyle="True"` | `ShowTitleBar="False"`, which is all it ever did |
| `TryToBeFlickerFree` | gone, with nothing in its place |

The glow taking a colour rather than a brush means the resource key changes with it:

```xml
<!-- 2.4.11 -->
<mah:MetroWindow GlowBrush="{DynamicResource MahApps.Brushes.Accent}" />

<!-- v3 -->
<mah:MetroWindow GlowColor="{DynamicResource MahApps.Colors.Accent}" />
```

In return the base brings `GlowDepth`, `IsGlowTransitionEnabled`, `PreferDWMBorderColor` and `CornerPreference`, the last of which is how Windows 11 is asked to round the corners or leave them square. [MetroWindow](../controls/metrowindow) has the table.

## Style keys that are gone

Eleven keys were removed. A `StaticResource` to one of them throws at load; a `DynamicResource` quietly resolves to nothing, which for a `Style` means a control with no template at all.

| Gone | What to do |
| --- | --- |
| `MahApps.Styles.Button.MetroSquare` | `MahApps.Styles.Button.Square` |
| `MahApps.Styles.Button.MetroSquare.Accent` | `MahApps.Styles.Button.Square.Accent` |
| `MahApps.Styles.ContentControl.PathIcon` | the `PathIcon` control, see below |
| `MahApps.Styles.TextBox.Button` | the base style carries the button now |
| `MahApps.Styles.RichTextBox.Button` | the same |
| `MahApps.Styles.PasswordBox.Button` | the same |
| `MahApps.Styles.PasswordBox.Button.Revealed` | `MahApps.Styles.PasswordBox.Revealed` |
| `MahApps.DropShadowEffect.WaitingForData` | nothing; the feature went |
| `MahApps.Storyboard.WaitingForData` | the same |
| `MahApps.Templates.DateTimePicker.MinuteIndicator` | `MahApps.Templates.AnalogClock.MinuteIndicator` |
| `MahApps.Templates.DateTimePicker.FiveMinuteIndicator` | `MahApps.Templates.AnalogClock.FiveMinuteIndicator` |

The three `.Button` styles and `TextBoxHelper.TextButton` went together. In 2.4.11 a button of your own on a text box needed that style, and the style set the flag that made the button appear. On v3 the base style carries the button, so `TextBoxHelper.ButtonCommand` on an ordinary box is all it takes.

```xml
<!-- 2.4.11 -->
<TextBox Style="{StaticResource MahApps.Styles.TextBox.Button}"
         mah:TextBoxHelper.ButtonCommand="{Binding SearchCommand}" />

<!-- v3 -->
<TextBox mah:TextBoxHelper.ButtonCommand="{Binding SearchCommand}" />
```

## Attached properties that are gone

| Gone | What to do |
| --- | --- |
| `TextBoxHelper.TextButton` | `TextBoxHelper.ClearTextButton`, see above |
| `TextBoxHelper.IsWaitingForData` | a [ProgressRing](../controls/progressring) or an indeterminate bar beside the control |
| `TextBoxHelper.IsClearTextButtonBehaviorEnabled` | nothing; the templates set it, and the clearing is `MahAppsCommands.ClearControlCommand` now |
| `ControlsHelper.FocusBorderThickness` | marked obsolete rather than removed; `ControlsHelper.FocusBorderBrush` colours the border the control already has, without the jump a thicker one causes |

## Types that changed shape

**`PathIcon` is a control.** In 2.4.11 a vector icon was a `ContentControl` wearing a style with the geometry as its content. Now it is a control of its own with a `Data` property, which is what the library's own templates use.

```xml
<!-- 2.4.11 -->
<ContentControl Width="16" Height="16"
                Content="M7.41,8.58L12,13.17L16.59,8.58L18,10L12,16L6,10L7.41,8.58Z"
                Style="{DynamicResource MahApps.Styles.ContentControl.PathIcon}" />

<!-- v3 -->
<mah:PathIcon Width="16" Height="16" Data="M7.41,8.58L12,13.17L16.59,8.58L18,10L12,16L6,10L7.41,8.58Z" />
```

**`ColorHelper` is an instance class.** It was static. Calls go through `ColorHelper.DefaultInstance` or through an instance of your own, `ColorNamesDictionary` is read-only, the dictionary key is `Color` rather than `Color?`, and `GetColorName` takes a third argument saying whether the alpha channel counts. `ColorFromString` and `GetColorName` are `virtual`, so the whole lookup can be replaced, which is what the change was for. [ColorPicker](../controls/colorpicker) has the detail.

**`NumericUpDown` has a generic base.** It derives from `NumericUpDownBase<double>` now, with `DecimalUpDown`, `IntegerUpDown` and `LongUpDown` beside it. `Value` is still a `double?` on `NumericUpDown` itself and nothing in XAML changes. What does change is the implicit style: it targets `NumericUpDownBase`, and WPF matches an implicit style by the exact type, so a style of your own needs a one-line entry per control. [NumericUpDown](../controls/numericupdown) shows it.

**A HamburgerMenu item is a `FrameworkContentElement`.** It was a `Freezable`. `Tag`, `IsEnabled` and `ToolTip` are the framework's own properties now rather than MahApps ones, so nothing in XAML changes; what you gain is that the items sit in the logical tree, where a `DynamicResource` hears about a theme change and a binding reaches the `DataContext` of the window.

**Four public types went.** `BindableResourceBehavior`, `BorderlessWindowBehavior`, `WinApiHelper` and `MetroTabItemCloseButtonWidthConverter`. `Flyout.OverrideFlyoutResources` went with them, so a `Flyout` of your own that overrode it no longer compiles.

**The dialogs lost their external window API.** `BaseMetroDialog.RequestCloseAsync`, `OnRequestClose` and `ParentDialogWindow` are gone, and a login dialog now follows the window it belongs to. [Custom dialogs](../dialogs/custom-dialogs) covers what is left.

## What changed without telling you

Everything here compiles, loads and then does something else.

**A window glows now.** The default style sets `GlowColor` to the accent, which it never did in 2.4.11. An application that deliberately had no glow has one, and takes it away with `GlowColor="{x:Null}"`.

**A `ProgressBar` honours `Foreground`.** In 2.4.11 the template filled the indicator from `MahApps.Brushes.Progress` rather than from a template binding, so `Foreground` was dead and the way to recolour one bar was a resource of its own. Both work now, and an application that set `Foreground` in passing and saw nothing happen will suddenly see it. [#4579](https://github.com/MahApps/MahApps.Metro/issues/4579).

**A close button on a tab can be turned down.** A `CloseTabCommand` on a `MetroTabItem` whose `CanExecute` says no now keeps its tab; in 2.4.11 the command simply did not run and the tab went anyway. A handler of the closing event can also stop a `CloseTabCommand` on the control, which it could not before, because the event was not raised at all when that command was set. [MetroTabControl](../controls/metrotabcontrol).

**A numeric grid cell is a `TextBlock` at rest.** `DataGridNumericUpDownColumn` showed a stripped-down `NumericUpDown` in every cell that was not being edited and shows plain text now. A style of your own hung on `MahApps.Styles.NumericUpDown.DataGrid` no longer reaches the cell; the resting style is `MahApps.Styles.TextBlock.NumericUpDown.DataGrid`.

**An expander glyph keeps the header's colour under the pointer.** It used to turn grey, which on the accent coloured band it sits on left almost nothing to see. [#4386](https://github.com/MahApps/MahApps.Metro/issues/4386).

**An underline falls back rather than vanishing.** `TabControlHelper`'s four underline brushes each painted their own state and nothing stood in for one that was empty, so a brush bound to something that could be cleared left the tab with no line. A state nobody handed a brush to now falls back to the nearest one that was.

**A dialog knows its window earlier.** `OwningWindow` is set when the dialog is shown rather than later, which matters to code that reads it from a handler.

## What is new and worth picking up

Two whole style sets, [Windows 10](../stylevariants/win10) and [WinUI](../stylevariants/winui), which put the whole library in the look of one of those two. They are opt-in: merge one dictionary and every control follows.

New controls: [AutoSuggestBox](../controls/autosuggestbox), [AnalogClock](../controls/analogclock), [MultiSelectionComboBox](../controls/multiselectioncombobox), [PathIcon](../controls/fonticon#pathicon), and `DecimalUpDown`, `IntegerUpDown` and `LongUpDown` with a `DataGrid` column each.

New helpers: [GroupItemHelper](../helper/groupitemhelper) for a group header that holds at the top and picks its whole group, [ProgressBarHelper](../helper/progressbarhelper) for the paused and error colours of a bar, and [ContextMenuHelper](../helper/contextmenuhelper) for a menu that names its items the way Windows does.

Elsewhere: a closing tab can be [held while a handler asks](../controls/metrotabcontrol#taking-your-time-over-the-answer), an input dialog can [check what was typed](../dialogs/input-dialog), a window can keep its placement [in a file rather than in the application settings](../controls/metrowindow#where-the-placement-is-stored), and every custom control has an automation peer, so [what a screen reader hears](accessibility) is a good deal more than it was.

## Related

[Quick Start](quick-start) is the shortest path to a working window. [Migration to v2.0](migration-to-v2.0) is the older guide and still the place to look for the v1 resource key renames, since those names have not come back.
