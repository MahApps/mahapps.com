Title: Win 10 (UWP)
Description: The look UWP drew on Windows 10, as a set to merge or as single styles
---

The Win10 styles draw a control the way UWP drew it on Windows 10: a filled accent square for a check box rather than an outline with a tick, a ring with a solid centre for a radio button, a rounded pill for the slider thumb, and a frame two pixels thick around a text box that turns accent-coloured once the caret is in it.

![The default styles and the Win10 ones](images/win10-controls.png)

Three of them are already what a control wears with nothing merged on your side: the [Slider](../styles/slider), the [RangeSlider](../controls/rangeslider) and the [WindowButtonCommands](../controls/WindowButtonCommands).

## The set

:::{.alert .alert-info}
**`Styles/Win10/Controls.xaml` is on `develop` and ships with the next release.** It is not in 2.4.11, where the styles below are individual keys you apply one control at a time.
:::

The set goes *in place of* `Styles/Controls.xaml` rather than next to it, because it merges that dictionary itself and puts its own styles in front of it:

```xml
<ResourceDictionary.MergedDictionaries>
    <ResourceDictionary Source="pack://application:,,,/MahApps.Metro;component/Styles/Win10/Controls.xaml" />
    <ResourceDictionary Source="pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml" />
    <ResourceDictionary Source="pack://application:,,,/MahApps.Metro;component/Styles/Themes/Light.Blue.xaml" />
</ResourceDictionary.MergedDictionaries>
```

Everything the library has is still there. What comes out in the Windows 10 look is the `Button`, the `RepeatButton`, the `CheckBox`, the `RadioButton`, the `TextBox` and the four [up-downs](../controls/numericupdown); every other control keeps the default look, because the set has nothing of its own for it.

Merge it into a window or a panel rather than into `App.xaml` and it reaches that part of the tree alone. The demo does exactly that, to stand the three sets next to each other.

## The individual styles

Nothing forces the whole set on you. Every style in it has a key, and always has had one:

| Style | Covered on |
| --- | --- |
| `MahApps.Styles.Button.Win10`, `…Button.Accent.Win10` | [Buttons](../styles/buttons) |
| `MahApps.Styles.CheckBox.Win10` | [CheckBox](../styles/checkbox) |
| `MahApps.Styles.CheckBox.DataGrid.Win10` | [DataGrid columns](../styles/datagridcolumns) |
| `MahApps.Styles.RadioButton.Win10` | [RadioButton](../styles/radiobutton) |
| `MahApps.Styles.TextBox.Win10` (on `develop`) | [TextBox](../styles/textbox) |
| `MahApps.Styles.NumericUpDown.Win10` (on `develop`) | [NumericUpDown](../controls/numericupdown) |
| `MahApps.Styles.Slider.Win10` | [Slider](../styles/slider) |
| `MahApps.Styles.RangeSlider.Win10` | [RangeSlider](../controls/rangeslider) |
| `MahApps.Styles.WindowButtonCommands.Win10` | [WindowButtonCommands](../controls/WindowButtonCommands) |

Each brings its own supporting pieces — `MahApps.Templates.Slider.Horizontal.Win10` and `.Vertical.Win10`, `MahApps.Styles.Thumb.Slider.Win10`, the two `RepeatButton.Slider.*Track.Win10` styles, the range slider's thumb and templates, and light and dark close-button styles for the window buttons.

To apply one everywhere without taking the set, declare an implicit style based on it:

```xml
<Style BasedOn="{StaticResource MahApps.Styles.CheckBox.Win10}" TargetType="{x:Type CheckBox}" />
```

`MahApps.Styles.CheckBox.DataGrid.Win10` is the one style the set leaves out, because a DataGrid column takes its style through `ElementStyle` and `EditingElementStyle` rather than by type.

:::{.alert .alert-info}
`MahApps.Styles.NumericUpDown.Win10` was called `MahApps.Styles.NumericUpDown.Fluent` up to and including 2.4.11. What it draws — a border that thickens when the control takes the caret, chevrons on the spin buttons, the text against the left edge — is this look and not the [WinUI](winui) one, so it is named for what it is.
:::

:::{.alert .alert-info}
The [Clean](clean) variant uses `MahApps.Styles.WindowButtonCommands.Clean.Win10` for its title-bar buttons, so if you are already on Clean you have the Win10 window buttons.
:::

## Additions from this site

The library stops at the controls above. For several others this documentation ships **drop-in dictionaries** in the same look, written for these pages and not part of the package:

| | |
| --- | --- |
| [`Controls.Calendar.Win10.xaml`](../../assets/xaml/Controls.Calendar.Win10.xaml) | a square calendar — see [Calendar](../styles/calendar) |
| [`Controls.DateTimePicker.Win10.xaml`](../../assets/xaml/Controls.DateTimePicker.Win10.xaml) | [DateTimePicker](../controls/DateTimePicker) and [TimePicker](../controls/TimePicker) |
| [`Controls.ScrollBar.Win10.xaml`](../../assets/xaml/Controls.ScrollBar.Win10.xaml) | [ScrollBars](../styles/scrollbars) |

On `develop` the calendar and the two pickers are no longer among them: the set carries those itself, under the same keys, so only the scroll bar is left to download. In a released version all three are files.

Download them, merge them after the set, and read the page each one belongs to — several note what the built-in templates do and do not let a style reach.

## Related

[WinUI](winui) is the newer of the two looks, and its set stands on this one. [Clean](clean) and [Visual Studio](vs) are variants of a different kind: Clean restyles the window chrome, Visual Studio turns the whole application into a tool window.
