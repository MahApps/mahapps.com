Title: WinUI
Description: The look Windows 11 draws, as a set that is only getting started
---

WinUI is what Windows 11 draws: rounded corners, the accent used sparingly, chrome that recedes until you point at it.

:::{.alert .alert-info}
**`Styles/WinUI/Controls.xaml` is on `develop` and ships with the next release.** 2.4.11 has no WinUI style at all; there, the drop-in dictionaries further down this page are the whole of it.
:::

## The set

The set has the text box, the password box and the scroll bar of its own, along with the calendar, the date picker and the two MahApps pickers that moved in on `develop`, and hands down the [Win10](win10) one for every control it has nothing for yet. That reads thinner than it is. The two looks belong to the same family, so a Windows 10 control sits next to a WinUI one far better than a Metro one would, and every WinUI style that gets written takes one of those places.

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

`MahApps.Styles.PasswordBox.WinUI` is that box with the eye of a UWP password box in it, held down to read what has been typed. The eye is there while the caret is in the box and something is written in it, which is what UWP does, and the clear button of the library answers to the same rule. [PasswordBox](../styles/passwordbox) covers it.

`MahApps.Styles.RichTextBox.WinUI` is that box again with a document in it, the scrollbars of the set along its edges and the text clipped to the rounded corners rather than running over them. UWP gives its rich box no button, so the one there is the clear button of the library, which waits for the caret the way the delete button does. [TextBox](../styles/textbox) covers it.

`MahApps.Styles.DatePicker.WinUI` is that box once more with a calendar behind the button. The border is a touch stronger along its bottom edge and turns into the accent there while the caret is in the field, and the drop-down is the WinUI calendar, round day cells and all. [DatePicker](../styles/datepicker) covers it.

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
