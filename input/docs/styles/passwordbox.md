Title: PasswordBox
Description: The PasswordBox styles
---

Every `PasswordBox` in a MahApps application is styled without any markup on your side. On top of that the library adds a caps lock warning, a watermark, a clear button, a reveal button and a bindable password — all through attached properties, so nothing has to be subclassed.

![The PasswordBox styles](images/passwordbox-styles.png)

## The implicit style

`Styles/Controls.xaml` contains a keyless style for `PasswordBox` that is based on `MahApps.Styles.PasswordBox`. Merging that dictionary — which the [quick start](../guides/quick-start) does — is all it takes:

```xml
<ResourceDictionary Source="pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml" />
```

From then on a plain `<PasswordBox />` is the MahApps one. Because the implicit style is keyless it also applies inside MahApps' own controls, which is why the [login dialog](../dialogs/login-dialogs) gets the caps lock warning for free.

To extend it rather than replace it, base your own style on the keyed one:

```xml
<Style BasedOn="{StaticResource MahApps.Styles.PasswordBox}" TargetType="{x:Type PasswordBox}">
    <Setter Property="mah:TextBoxHelper.ClearTextButton" Value="True" />
</Style>
```

Leaving out the `BasedOn` throws the template away with it, and with the template go the caps lock indicator, the watermark and the buttons.

## The explicit styles

Four styles are meant to be set on a `PasswordBox`. Each inherits from the one above it, so everything the base style offers keeps working.

| Style | |
| --- | --- |
| `MahApps.Styles.PasswordBox` | the base. What the implicit style applies |
| `MahApps.Styles.PasswordBox.Button` | adds a command button, driven by `TextBoxHelper.ButtonCommand` |
| `MahApps.Styles.PasswordBox.Button.Revealed` | adds a button that shows the password while it is held down |
| `MahApps.Styles.PasswordBox.Win8` | `Button.Revealed` with a larger font, no focus visual and `AllowDrop` |

```xml
<PasswordBox Style="{StaticResource MahApps.Styles.PasswordBox.Button.Revealed}" />
```

:::{.alert .alert-info}
`MahApps.Styles.PasswordBox.Win8` is a leftover name from the Windows 8 era. It differs from `MahApps.Styles.PasswordBox.Button.Revealed` only in `FontSize="15"`, `FocusVisualStyle="{x:Null}"` and `AllowDrop="True"` — reach for the revealed style unless you specifically want those three.
:::

Two more styles live in the same dictionary. They are building blocks of the templates above rather than something to set yourself: `MahApps.Styles.Button.Reveal` styles the reveal button, and `MahApps.Styles.TextBox.PasswordBox.Revealed` styles the borderless `TextBox` the revealed password is drawn into.

### The reveal button

The revealed styles show the password only while the button is pressed — releasing it hides the password again, so it cannot be left switched on by accident. The button appears only once something has been typed.

Its content is an attached property, so the eye icon can be swapped out:

```xml
<PasswordBox Style="{StaticResource MahApps.Styles.PasswordBox.Button.Revealed}"
             mah:PasswordBoxHelper.RevealButtonContent="Show" />
```

## PasswordBoxHelper

The helper that exists only for this control. All four properties are attached to the `PasswordBox`.

| Property | Type | Default | |
| --- | --- | --- | --- |
| `CapsLockIcon` | `object` | a warning triangle | what the caps lock indicator shows |
| `CapsLockWarningToolTip` | `object` | `Caps lock is on` | its tooltip |
| `RevealButtonContent` | `object` | an eye icon | content of the reveal button |
| `RevealButtonContentTemplate` | `DataTemplate` | `null` | template for that content |

`CapsLockIcon` and `RevealButtonContent` are typed as `object`, so anything goes in: a string, a `Path`, a whole `Grid`.

### The caps lock warning

While the box has keyboard focus, an indicator appears inside it whenever <kbd>Caps Lock</kbd> is on. Nothing has to be switched on — it is part of the base style, and therefore of the implicit one.

![The caps lock indicator](images/passwordbox-capslock.png)

```xml
<PasswordBox mah:PasswordBoxHelper.CapsLockIcon="CAPS"
             mah:PasswordBoxHelper.CapsLockWarningToolTip="Caps Lock is turned on" />
```

## TextBoxHelper

The `PasswordBox` templates read a good part of `TextBoxHelper`, which is why the watermark and the clear button work the same way here as on a `TextBox`.

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Watermark` | `string` | empty | placeholder shown while the box is empty |
| `WatermarkAlignment` | `TextAlignment` | `Left` | how it is aligned |
| `WatermarkTrimming` | `TextTrimming` | `None` | how it is trimmed when the box is too narrow |
| `UseFloatingWatermark` | `bool` | `false` | keep the watermark above the content once something is typed |
| `ClearTextButton` | `bool` | `false` | show a button that clears the password |
| `TextButton` | `bool` | `false` | show the button of `MahApps.Styles.PasswordBox.Button`, which that style sets for you |
| `ButtonCommand` | `ICommand` | `null` | invoked when the button is clicked |
| `ButtonCommandParameter` | `object` | the `PasswordBox` | passed to that command |
| `ButtonContent` | `object` | an ✕ glyph | content of the button |
| `ButtonContentTemplate` | `DataTemplate` | | template for that content |
| `ButtonTemplate` | `ControlTemplate` | `null` | template of the button itself |
| `ButtonWidth` | `double` | `22` | its width |
| `ButtonFontFamily`, `ButtonFontSize` | | | the font its content is drawn in |
| `ButtonsAlignment` | `ButtonsAlignment` | `Right` | which side the button sits on |
| `SelectAllOnFocus` | `bool` | `false` | select the password when the box gains focus |
| `IsWaitingForData` | `bool` | `false` | pulse a glow around the box while a value is being fetched |
| `IsMonitoring` | `bool` | `true` here | watch the password for changes; the base style turns it on |
| `HasText` | `bool` | | **read-only** — set by the monitoring above, useful in your own triggers |

`IsMonitoring` is what keeps `HasText` current and what raises the events the caps lock indicator and the floating watermark hang off. The base style sets it, so leave it alone unless you have replaced the style entirely.

### Watermark

![Watermark and floating watermark](images/passwordbox-watermark.png)

```xml
<PasswordBox mah:TextBoxHelper.Watermark="Password" />

<PasswordBox mah:TextBoxHelper.Watermark="Password"
             mah:TextBoxHelper.UseFloatingWatermark="True" />
```

A plain watermark disappears as soon as the first character is typed. A floating one moves above the content instead and stays there, which keeps the label visible while the password is being entered.

### Clear button

![Clear button variants](images/passwordbox-clearbutton.png)

```xml
<PasswordBox mah:TextBoxHelper.ClearTextButton="True" />

<PasswordBox mah:TextBoxHelper.ClearTextButton="True"
             mah:TextBoxHelper.ButtonsAlignment="Left" />

<PasswordBox mah:TextBoxHelper.ClearTextButton="True"
             mah:TextBoxHelper.ButtonContent="Clear"
             mah:TextBoxHelper.ButtonFontFamily="{DynamicResource MahApps.Fonts.Family.Control}"
             mah:TextBoxHelper.ButtonFontSize="12"
             mah:TextBoxHelper.ButtonWidth="48" />
```

Clearing calls `PasswordBox.Clear()` and pushes the empty value back through the password binding described below, so a bound view model sees the change.

### Command button

`MahApps.Styles.PasswordBox.Button` reuses the same button for a command of your own:

```xml
<PasswordBox Style="{StaticResource MahApps.Styles.PasswordBox.Button}"
             mah:TextBoxHelper.ButtonCommand="{Binding CheckStrengthCommand}"
             mah:TextBoxHelper.ButtonContent="&#xE1CF;"
             mah:TextBoxHelper.ButtonFontFamily="Segoe MDL2 Assets" />
```

Without a `ButtonCommand` the button is still shown — the style sets `TextButton="True"` — but clicking it does nothing. Setting `ClearTextButton="True"` as well makes it run the command *and* clear the box.

## ControlsHelper

The shared helper contributes the parts of the look that are not specific to text input.

| Property | Type | Default in this style | |
| --- | --- | --- | --- |
| `FocusBorderBrush` | `Brush` | `MahApps.Brushes.TextBox.Border.Focus` | border while the box has focus |
| `MouseOverBorderBrush` | `Brush` | `MahApps.Brushes.TextBox.Border.MouseOver` | border under the pointer |
| `CornerRadius` | `CornerRadius` | `0` | rounds the border |
| `DisabledVisualElementVisibility` | `Visibility` | `Visible` | the overlay drawn over a disabled box |

```xml
<PasswordBox mah:ControlsHelper.CornerRadius="4"
             mah:ControlsHelper.FocusBorderBrush="{DynamicResource MahApps.Brushes.Accent}" />
```

## Binding the password

`PasswordBox.Password` is not a dependency property, so it cannot be bound. MahApps works around that with `PasswordBoxBindingBehavior`, and the base style attaches the behaviour for you — the attached property is all you need:

```xml
<PasswordBox mah:PasswordBoxBindingBehavior.Password="{Binding Password, UpdateSourceTrigger=PropertyChanged}" />
```

It binds two-way by default, so setting the property from the view model fills the box as well.

:::{.alert .alert-warning}
This hands the password around as a plain `string`, which lives in memory until the garbage collector happens to reclaim it and cannot be wiped. Where that matters, read `PasswordBox.SecurePassword` from the control instead of binding.
:::

## Validation and busy state

`Validation.ErrorTemplate` is set to `MahApps.Templates.ValidationError`, so a failing validation rule on the password binding is drawn the same way as on any other MahApps input control. `TextBoxHelper.IsWaitingForData` runs a pulsing glow around the box, which suits a password that is being checked against a server:

```xml
<PasswordBox mah:TextBoxHelper.IsWaitingForData="{Binding IsLoading}" />
```
