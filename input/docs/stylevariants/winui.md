Title: WinUI
Description: The look Windows 11 draws, as a set that is only getting started
---

WinUI is what Windows 11 draws: rounded corners, the accent used sparingly, chrome that recedes until you point at it.

:::{.alert .alert-info}
**`Styles/WinUI/Controls.xaml` is on `develop` and ships with the next release.** 2.4.11 has no WinUI style at all; there, the drop-in dictionaries further down this page are the whole of it.
:::

## The set

The set has the text box, the password box, the check box, the radio button, the combo box, the [box that holds several picks](../controls/MultiSelectionComboBox), the [suggestion box](../controls/AutoSuggestBox), the [shortcut box](../controls/HotKeyBox), the [colour picker](../controls/ColorPicker), the four [up-downs](../controls/numericupdown) and the scroll bar of its own, along with the calendar, the date picker and the two MahApps pickers that moved in on `develop`, and hands down the [Win10](win10) one for every control it has nothing for yet. That reads thinner than it is. The two looks belong to the same family, so a Windows 10 control sits next to a WinUI one far better than a Metro one would, and every WinUI style that gets written takes one of those places.

It goes *in place of* `Styles/Controls.xaml`, because it merges `Styles/Win10/Controls.xaml`, which in turn merges `Styles/Controls.xaml`:

```xml
<ResourceDictionary.MergedDictionaries>
    <ResourceDictionary Source="pack://application:,,,/MahApps.Metro;component/Styles/WinUI/Controls.xaml" />
    <ResourceDictionary Source="pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml" />
    <ResourceDictionary Source="pack://application:,,,/MahApps.Metro;component/Styles/Themes/Light.Blue.xaml" />
</ResourceDictionary.MergedDictionaries>
```

Merge it into a window or a panel rather than into `App.xaml` and it reaches that part of the tree alone.

## What the library has of its own

`MahApps.Styles.TextBox.WinUI` is a light translucent fill, a border you have to look for that is a touch stronger along its bottom edge, and with the caret a solid fill, a line of accent underneath and rounded corners. The delete button comes and goes with the caret the way the UWP box does, while a button carrying a command of your own stays where it is.

What it is made of sits in the theme as `MahApps.Colors.WinUI.*` and `MahApps.Brushes.TextControl.WinUI.*`, taken from the WinUI values themselves, and the border thicknesses and the padding are resources a style of your own can answer differently without replacing the template. [TextBox](../styles/textbox) covers it.

How round any of it is comes from two keys rather than from the styles: `MahApps.CornerRadius.WinUI.Control` and `MahApps.CornerRadius.WinUI.Overlay`, four and eight, which are WinUI's own `ControlCornerRadius` and `OverlayCornerRadius`. Every style of the set reads one of the two, so overriding one key rounds the whole set differently. Three keys keep a radius of their own because it is neither of those: the day cell of the calendar is a pill, and the scroll bar is drawn to its own width.

`MahApps.Styles.PasswordBox.WinUI` is that box with the eye of a UWP password box in it, held down to read what has been typed. The eye is there while the caret is in the box and something is written in it, which is what UWP does, and the clear button of the library answers to the same rule. [PasswordBox](../styles/passwordbox) covers it.

`MahApps.Styles.RichTextBox.WinUI` is that box again with a document in it, the scrollbars of the set along its edges and the text clipped to the rounded corners rather than running over them. UWP gives its rich box no button, so the one there is the clear button of the library, which waits for the caret the way the delete button does. [TextBox](../styles/textbox) covers it.

`MahApps.Styles.DatePicker.WinUI` is that box once more with a calendar behind the button. The border is a touch stronger along its bottom edge and turns into the accent there while the caret is in the field, and the drop-down is the WinUI calendar, round day cells and all. [DatePicker](../styles/datepicker) covers it.

The three pickers of the set follow the two habits the text box has: the clear button waits for the caret and something written, and a picker that is switched off takes the disabled colours rather than a veil. [DatePicker](../styles/datepicker) has the detail.

`MahApps.Styles.ComboBox.WinUI` is that box with a list behind a chevron. The border is a touch stronger along its bottom edge and turns into the accent there while the caret is in it, and the list is a flyout of rounded tiles where the row that is picked carries the accent as a short bar along its left edge rather than being filled with it. [ComboBox](../styles/combobox) covers it.

`MahApps.Styles.MultiSelectionComboBox.WinUI` is that combo box holding more than one answer, with a row of what has been picked where the one pick would stand. The fill, the rounded corners, the bottom edge that turns into the accent and the rounded flyout the list comes up in are all that box; what is different is the rows in that list, which say what is picked by the fill rather than by a bar along the left edge, since several of them can be. [MultiSelectionComboBox](../controls/MultiSelectionComboBox) covers it.

`MahApps.Styles.AutoSuggestBox.WinUI` is that combo box with the chevron taken off it, since there is nothing to drop down by hand in a suggestion box. Everything else it keeps: the fill, the bottom edge that turns into the accent while the caret is in it, the delete button that waits for something to delete, and the list of rounded tiles the suggestions come up in. [AutoSuggestBox](../controls/AutoSuggestBox) covers it.

`MahApps.Styles.HotKeyBox.WinUI` is the text box itself, since a shortcut box is one box and nothing else: the same fill, the same padding, the same delete button, and the border a touch stronger along its bottom edge, turning into the accent there while the caret is in it. What is typed into it is a key combination rather than letters, which is the control and not the look. [HotKeyBox](../controls/HotKeyBox) covers it.

`MahApps.Styles.ColorPicker.WinUI` is that combo box with a swatch and the name of a colour where the one pick would stand. The fill, the rounded corners, the bottom edge that turns into the accent and the rounded flyout are all that box. What comes down inside the flyout follows it: the groups of swatches are headed by plain text rather than by a filled bar, the swatches are rounded by the same number the box is, and the boxes on the canvas beside them are the WinUI text box and the WinUI up-down. `MahApps.Styles.ColorCanvas.WinUI` and `MahApps.Styles.ColorPalette.WinUI` are those two on their own, for a window that shows them without the drop-down. [ColorPicker](../controls/ColorPicker) covers it.

`MahApps.Styles.NumericUpDown.WinUI` is that box with a pair of buttons standing in it. It is the text box down to the fill, the padding and the delete button; what it adds is the two chevrons at the right end, drawn in the quieter of the two text colours until the pointer reaches one of them, and the cell behind the one it reaches. The border is a touch stronger along its bottom edge and turns into the accent there while the caret is in the field. The same style covers all four up-downs, since they share a base class. [NumericUpDown](../controls/numericupdown) covers it.

`MahApps.Styles.CheckBox.WinUI` and `MahApps.Styles.RadioButton.WinUI` are the Fluent 2 pair: a rounded box and a ring of twenty, each with a hairline round a fill a shade off the page, each turning accent once it is ticked or picked. What sits on that accent is the ideal foreground the theme works out for it rather than a fixed black or white. [CheckBox](../styles/checkbox) and [RadioButton](../styles/radiobutton) cover them.

`MahApps.Styles.ScrollBar.WinUI` is the Fluent bar in both its visualisations: a two-unit line at the edge of the content that grows inwards into a thumb with a chevron at either end once the pointer is on it. The set applies `MahApps.Styles.ScrollViewer.WinUI` with it, which lays the bars over the content rather than beside it, so the sixteen units the expanded bar needs cost no layout while nobody is pointing at it. Leave that much padding at the edge of anything interactive. The [Win10](win10) bar works the same way in square shape, and both take their timings from the platform; [ScrollBars](../styles/scrollbars) covers all of it.

## Drop-in dictionaries from this site

For the controls the set does not reach yet, this documentation ships **drop-in dictionaries** in the same look. They are written for these pages and are not part of the NuGet package, and their keys already read the way the library's do, so nothing has to be renamed when one of them moves in:

| | Covers | Page |
| --- | --- | --- |
| [`Controls.Calendar.WinUI.xaml`](../../assets/xaml/Controls.Calendar.WinUI.xaml) | `Calendar` | [Calendar](../styles/calendar) |
| [`Controls.DateTimePicker.WinUI.xaml`](../../assets/xaml/Controls.DateTimePicker.WinUI.xaml) | `DateTimePicker`, `TimePicker` | [DateTimePicker](../controls/DateTimePicker), [TimePicker](../controls/TimePicker) |
| [`Controls.ScrollBar.WinUI.xaml`](../../assets/xaml/Controls.ScrollBar.WinUI.xaml) | `ScrollBar`, `ScrollViewer` | [ScrollBars](../styles/scrollbars) |

On `develop` all three moved into the set itself, under these same keys, and the set applies them without anything being merged. What follows is for 2.4.11 and the release candidate.

Merge them after whichever of the library's dictionaries you are on:

```xml
<ResourceDictionary Source="pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml" />
<ResourceDictionary Source="Styles/Controls.Calendar.WinUI.xaml" />
<ResourceDictionary Source="Styles/Controls.DateTimePicker.WinUI.xaml" />
<ResourceDictionary Source="Styles/Controls.ScrollBar.WinUI.xaml" />
```

Each defines keyed styles rather than implicit ones, so nothing changes until you apply them — either per control, or through an implicit style of your own:

```xml
<Style BasedOn="{StaticResource MahApps.Styles.Calendar.WinUI}" TargetType="{x:Type Calendar}" />
```

The `DateTimePicker` dictionary needs the `Calendar` one merged first, since its picker styles point at the WinUI calendar.

## Styles, not replacement templates

Where it was possible, these dictionaries change only what the library's templates already expose — a corner radius through `ControlsHelper.CornerRadius`, a `CalendarStyle`, an `IsClockVisible`. That keeps them working when the library's templates change underneath.

Where it was not possible, the pages say so plainly rather than pretending. Two examples worth knowing before you start:

- the [DateTimePicker](../controls/DateTimePicker)'s drop-down frame stays square whatever you do, because `PART_PopupBorder` has no `CornerRadius` and none of its brushes is a `TemplateBinding` — tracked as [#4582](https://github.com/MahApps/MahApps.Metro/issues/4582)
- the [ScrollBar](../styles/scrollbars) style *is* a template replacement, because a WinUI scrollbar's expand-on-hover behaviour cannot be reached from the library's template at all

## Related

[Win 10 (UWP)](win10) is the older of the two looks and the one this set falls back on. [Clean](clean) and [Visual Studio](vs) are variants of a different kind: Clean restyles the window chrome, Visual Studio turns the whole application into a tool window.
