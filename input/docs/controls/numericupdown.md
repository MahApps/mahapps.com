Title: NumericUpDown
Description: A numeric text field with increment and decrement buttons
---

`NumericUpDown` is a text field for a number, with a button to step it up and one to step it down.

![The buttons on the right, on the left, opposite, and hidden](images/numericupdown-buttons.png)

```xml
<mah:NumericUpDown Minimum="0"
                   Maximum="10000"
                   Interval="100"
                   StringFormat="C2"
                   Value="{Binding Amount}" />
```

## Four controls, one template

**On `develop` there are four of these, one per type.** They share a base class, a template and every property on this page; all that differs is what `Value`, `Minimum`, `Maximum` and `Interval` are. A released version has only `NumericUpDown`.

| Control | Value | Use it for |
| --- | --- | --- |
| `NumericUpDown` | `double?` | a measurement: a length, a weight, a rate |
| `DecimalUpDown` | `decimal?` | money, and anything else where the digits typed in are the digits that come back out |
| `IntegerUpDown` | `int?` | something counted: copies, a page number |
| `LongUpDown` | `long?` | a count past two billion: a file size in bytes, a database identifier |

`NumericUpDown` stays a `double` and keeps every property it had, so nothing written against it needs touching.

The reason to reach for `DecimalUpDown` is that a `double` cannot hold `0.1`. It holds something very close, and the error shows once such values are added up: stepping a `NumericUpDown` from `0.1` twice by `0.1` leaves `Value` at `0.30000000000000004`, where a `DecimalUpDown` leaves it at `0.3`. The field reads `0.3` either way, so the difference only turns up in whatever the value is bound to. Binding a `decimal` property to a `NumericUpDown` has the same problem, since the value is carried through a `double` on the way in and out.

`IntegerUpDown` and `LongUpDown` start with `NumericInputMode` at `Numbers`, so the decimal separator is refused as a keystroke. A `LongUpDown` also holds a count exactly all the way to `long.MaxValue`, which a `double` stops doing at about nine quadrillion.

## The value and its range

| Property | Type | Default |
| --- | --- | --- |
| `Value` | `T?` | `null` |
| `Minimum` | `T` | the type's `MinValue` |
| `Maximum` | `T` | the type's `MaxValue` |
| `Interval` | `T` | `1` |

`T` is `double` for `NumericUpDown` and the matching type for each of the other three. In a released version everything here is a `double`.

`Value` is **nullable**, so an empty field is a real state rather than zero. Bind to a nullable if the user is allowed to clear it.

`SnapToMultipleOfInterval` (default `False`) rounds every new value to the nearest multiple of `Interval`:

```csharp
value = Math.Round(newValue / this.Interval) * this.Interval;
```

It is applied to values that arrive by any route, not just to the buttons, and turning the property on rounds the current value immediately.

## How the user changes it

| Property | Default | |
| --- | --- | --- |
| `InterceptArrowKeys` | `True` | <kbd>↑</kbd> and <kbd>↓</kbd> step by `Interval` |
| `InterceptMouseWheel` | `True` | the wheel steps by `Interval` while the control has focus |
| `TrackMouseWheelWhenMouseOver` | `False` | when `True`, the wheel works on hover, without focus |
| `InterceptManualEnter` | `True` | the number can be typed |
| `ChangeValueOnTextChanged` | `True` | when `False`, typing updates `Value` only on <kbd>Enter</kbd> or lost focus |

`ChangeValueOnTextChanged` is worth knowing about for bound scenarios: left at its default, every keystroke pushes a value, so typing `15` in an empty field momentarily sets `1`.

Holding a button repeats the step. `Delay` (default **500** ms) is the pause before the repeat begins, and `Speedup` (default `True`) makes the repeat accelerate the longer the button is held. Set `Speedup="False"` for a constant rate.

## The buttons

| Property | Type | Default | |
| --- | --- | --- | --- |
| `ButtonsAlignment` | `ButtonsAlignment` | `Right` | `Left`, `Right` or `Opposite` |
| `HideUpDownButtons` | `bool` | `False` | |
| `SwitchUpDownButtons` | `bool` | `False` | puts the down button first |
| `UpDownButtonsWidth` | `double` | `20` | each button |
| `UpDownButtonsFocusable` | `bool` | `True` | |

`Opposite` puts one button at each end of the field, as in the third panel at the top of this page.

![NumericInputMode and switched buttons](images/numericupdown-inputmode.png)

Hiding the buttons does not make the control read-only — the arrow keys, the wheel and typing all still work. To make it look inert when it is, drive the property from `IsReadOnly`:

```xml
<Style BasedOn="{StaticResource {x:Type mah:NumericUpDown}}" TargetType="{x:Type mah:NumericUpDown}">
    <Style.Triggers>
        <Trigger Property="IsReadOnly" Value="True">
            <Setter Property="HideUpDownButtons" Value="True" />
        </Trigger>
    </Style.Triggers>
</Style>
```

## Formatting

![No format, N2, C2 and a custom format](images/numericupdown-format.png)

`StringFormat` takes a standard or custom .NET numeric format string:

| | |
| --- | --- |
| `N2` | `1,234.50` |
| `C2` | `$1,234.50` |
| `P0` | a percentage |
| `{}{0:N2} psc` | a composite format; the leading `{}` escapes the brace for XAML |

`Culture` (default `null`, meaning the thread's culture) decides the separators and the currency symbol. The examples above are `en-US`.

`ParsingNumberStyle` (default `NumberStyles.Any`) is what typed text is parsed with, if you need to be stricter than that.

### A hexadecimal StringFormat

A format such as `X`, `X8` or `{}0x{0:X}` writes letters, and only a hexadecimal parse reads them back. The control therefore follows the format: it sets `ParsingNumberStyle` to `NumberStyles.HexNumber` and adds `NumericInput.Decimal` to the `NumericInputMode` for as long as the format stays hexadecimal.

**This changed on `develop`** with [#4499](https://github.com/MahApps/MahApps.Metro/issues/4499). In a released version there was no way out again: once a hexadecimal format had been set the parsing stayed hexadecimal even after the format changed back, and a typed `10` came out as `16`. On `develop` the control gives back what the format took over. A parsing style or input mode you changed in the meantime is yours and is left alone.

### Without a StringFormat

**This changed on `develop`** with [#3673](https://github.com/MahApps/MahApps.Metro/issues/3673). A value with no format of its own used to go straight to `double.ToString()`, which reaches for an exponent on a small number and, since .NET Core 3.0, writes out every digit a calculation left behind:

| Value | A released version | `develop` |
| --- | --- | --- |
| `0.00005` | `5E-05` | `0.00005` |
| `0.1 + 0.2` | `0.30000000000000004` | `0.3` |
| `1d / 3d` | `0.3333333333333333` | `0.333333333333333` |

Plain decimal notation is used while the magnitude stays between `1e-15` and `1e15`, and whole numbers that fit in a `long` are written out in full. Outside that the framework still decides, because a decimal format would either drop a significant digit or bury the number in zeroes. A `StringFormat` of your own decides everything, exactly as before.

The sixteenth and seventeenth significant digit of a `double` are no longer shown, though they are still held in the value. If they matter to you, they are the reason `DecimalUpDown` exists.

### Decimals

`NumericInputMode` is a `[Flags]` enum with `Numbers`, `Decimal` and `All`, and defaults to **`All`**. Set it to `Numbers` and the decimal separator is refused, so `3.5` becomes `3` — the second panel above.

:::{.alert .alert-info}
The old documentation called this a replacement for a `HasDecimals` property. That property has not existed since v1.x; `NumericInputMode` is simply the current API.
:::

### The numeric keypad's decimal key

`DecimalPointCorrection` solves a small, real annoyance: the <kbd>.</kbd> key on the numeric keypad produces a period regardless of the keyboard layout, which is wrong in every culture whose decimal separator is a comma.

| Value | |
| --- | --- |
| `Inherits` | **the default** — no correction; the key inserts whatever it produces |
| `Number` | insert `NumberFormat.NumberDecimalSeparator` instead |
| `Currency` | insert `CurrencyDecimalSeparator` |
| `Percent` | insert `PercentDecimalSeparator` |

The control intercepts `Key.Decimal` only — the period key on the main keyboard is left alone — marks the event handled, and inserts the separator its `Culture` calls for. The three non-default modes exist because those three separators can differ within one culture.

## Related

`DataGridNumericUpDownColumn` puts one of these in a `DataGrid` cell and forwards most of these properties to it, `DecimalPointCorrection` included. It holds a `double`.

The default style targets the shared base class, `NumericUpDownBase`, so one style covers all four. WPF still matches an implicit style by the exact type, so a style of your own needs a one-line entry per control:

```xml
<Style x:Key="MyUpDown"
       BasedOn="{StaticResource MahApps.Styles.NumericUpDown}"
       TargetType="{x:Type mah:NumericUpDownBase}">
    <Setter Property="ButtonsAlignment" Value="Opposite" />
</Style>

<Style BasedOn="{StaticResource MyUpDown}" TargetType="{x:Type mah:NumericUpDown}" />
<Style BasedOn="{StaticResource MyUpDown}" TargetType="{x:Type mah:DecimalUpDown}" />
```
