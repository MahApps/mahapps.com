Title: AnalogClock
Description: The clock face from the picker drop-down, on its own
---

`AnalogClock` is a round face with an hour, a minute and a second hand. It is the clock the [TimePicker](TimePicker) and the [DateTimePicker](DateTimePicker) show in their drop-down, and since it is a control you can put one anywhere else as well.

```xml
<mah:AnalogClock Time="{Binding Departure}" />
```

:::{.alert .alert-info}
**`AnalogClock` is new on `develop`** and is in neither 2.4.11 nor the 3.0 release candidate. Before it, the face was markup in the middle of the shared picker template, so the only way at it was a copy of that template — which is what [#4514](https://github.com/MahApps/MahApps.Metro/issues/4514) was about.
:::

## What it shows

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Time` | `DateTime?` | `null` | where the hands point; only the time of day is read |
| `HandVisibility` | `TimePartVisibility` | `All` | which of the three hands are drawn |

The clock shows what it is given and nothing else — it does not tick. A wall clock is a binding to something that changes, a view model property a `DispatcherTimer` keeps on the current time:

```xml
<mah:AnalogClock Time="{Binding Now}" />
```

`TimePartVisibility` is the same flags enum the pickers use: `Hour`, `Minute`, `Second`, plus `HourMinute` and `All`. A face without the thin second hand is one setter:

```xml
<mah:AnalogClock HandVisibility="HourMinute" Time="{Binding Departure}" />
```

## Size and colour

The face is drawn at a natural 120 by 120 and scaled to whatever size the clock is given, so the ring, the dots around it and the three hands keep their proportions at any size and there is one number to set:

```xml
<mah:AnalogClock Width="240" Height="240" />
```

`BorderBrush` and `BorderThickness` are the ring, `Foreground` the hands and the dot in the middle, `Background` the face behind them. The ring is on the accent by default and the hands follow the theme foreground. Because the whole face is scaled, a border of two on a clock drawn at twice its natural size is four on the screen.

| Style | |
| --- | --- |
| `MahApps.Styles.AnalogClock` | the default one, accent ring and theme-coloured hands |
| `MahApps.Styles.AnalogClock.Win10` | the hands in the foreground of a Windows 10 text control |
| `MahApps.Styles.AnalogClock.WinUI` | the same for WinUI |

Neither the Windows 10 nor the WinUI set shows a clock in its picker drop-down, because neither Windows 10 nor Fluent has a clock face — the columns are the whole time selection there. The two styles are for a clock you place yourself, and for a picker in those sets that is told `IsClockVisible="True"`.

## In a picker

The picker owns the clock in its drop-down, so it is reached through the picker rather than directly: `ClockSize` for how large it is drawn, `ClockStyle` for its look, `IsClockVisible` for whether it is there at all, and `HandVisibility`, which the picker passes down. [The TimePicker page](TimePicker#reaching-into-the-drop-down) has them together.

```xml
<mah:TimePicker ClockSize="200" HandVisibility="All" />
```

## Related

[TimePicker](TimePicker) and [DateTimePicker](DateTimePicker) for the pickers this face came out of.
