Title: WinUI
Description: A Fluent look, assembled from drop-in dictionaries this site provides
---

:::{.alert .alert-warning}
**MahApps.Metro does not ship a WinUI variant.** There is no `Styles/WinUI/` folder, no `WinUI` style key anywhere in the library, and nothing to merge — in 2.4.11 or on `develop`.
:::

What exists instead is a set of **drop-in dictionaries written for this documentation**. They give individual controls a Fluent 2 look — rounded corners, the accent used sparingly, chrome that recedes — and they are not part of the NuGet package. Download the ones you want and merge them into your application.

## The dictionaries

| | Covers | Page |
| --- | --- | --- |
| [`Controls.Calendar.WinUI.xaml`](../../assets/xaml/Controls.Calendar.WinUI.xaml) | `Calendar` | [Calendar](../styles/calendar) |
| [`Controls.DateTimePicker.WinUI.xaml`](../../assets/xaml/Controls.DateTimePicker.WinUI.xaml) | `DateTimePicker`, `TimePicker` | [DateTimePicker](../controls/DateTimePicker), [TimePicker](../controls/TimePicker) |
| [`Controls.ScrollBar.WinUI.xaml`](../../assets/xaml/Controls.ScrollBar.WinUI.xaml) | `ScrollBar`, `ScrollViewer` | [ScrollBars](../styles/scrollbars) |

Merge them after the library's own `Controls.xaml`:

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
- the [ScrollBar](../styles/scrollbars) dictionary *is* a template replacement, because a WinUI scrollbar's expand-on-hover behaviour cannot be reached from a style at all

## Related

[Win 10 (UWP)](win10) — the library does ship a few control styles in that older look, and this site adds more. [Clean](clean) and [Visual Studio](vs) are the two variants that really are in the package.
