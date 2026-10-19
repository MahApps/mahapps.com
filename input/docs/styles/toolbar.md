Title: ToolBar
Description: The ToolBar and ToolBarTray styles
---

Two implicit styles — the bar and the tray it sits in — plus a set of styles for the controls that go inside one, which WPF applies for you through its own style keys.

![A toolbar in a tray](images/toolbar-default.png)

```xml
<ToolBarTray>
    <ToolBar>
        <Button Content="New" />
        <Button Content="Open" />
        <Separator />
        <ToggleButton Content="Bold" IsChecked="True" />
        <ToggleButton Content="Italic" />
    </ToolBar>
</ToolBarTray>
```

`Styles/Controls.xaml` applies `MahApps.Styles.ToolBar` and `MahApps.Styles.ToolBarTray`, so the [quick start](../guides/quick-start) is the whole setup.

## Controls inside a toolbar are restyled for you

A `ToolBar` re-styles its children through eight style keys that WPF looks up by name. MahApps fills in six of them:

| WPF key | MahApps style |
| --- | --- |
| `ToolBar.ButtonStyleKey` | `MahApps.Styles.Button.ToolBar` |
| `ToolBar.ToggleButtonStyleKey` | `MahApps.Styles.ToggleButton.ToolBar` |
| `ToolBar.CheckBoxStyleKey` | `MahApps.Styles.ToggleButton.ToolBar` |
| `ToolBar.RadioButtonStyleKey` | `MahApps.Styles.ToggleButton.ToolBar` |
| `ToolBar.TextBoxStyleKey` | `MahApps.Styles.TextBox` |
| `ToolBar.ComboBoxStyleKey` | `MahApps.Styles.ComboBox` |

Note rows three and four: a `CheckBox` or a `RadioButton` dropped into a toolbar is given the **toggle button** style. It loses its box or its dot and becomes a button that stays pressed:

![Button, ToggleButton, CheckBox and RadioButton in a toolbar](images/toolbar-items.png)

All four items in that figure are different control types, and three of them look identical because three keys point at the same style.

:::{.alert .alert-info}
That is WPF's behaviour, not a MahApps decision. The stock Aero theme maps `CheckBoxStyleKey` and `RadioButtonStyleKey` to the same toolbar button appearance, so a check box in a plain WPF toolbar loses its tick as well — MahApps only supplies its own look for it. A toolbar wants latching buttons, the way *Bold* and *Italic* work, so if you need a visible tick or dot, put the control next to the toolbar rather than in it.
:::

`ToolBar.MenuStyleKey` and `ToolBar.SeparatorStyleKey` are the two MahApps leaves alone, so a `Menu` or a `Separator` inside a toolbar keeps WPF's own look. The separator is a plain vertical line, which fits; a `Menu` is worth styling yourself if you put one there.

## Background belongs on the tray

:::{.alert .alert-warning}
**Setting `Background` on a `ToolBar` does nothing.** The template's root border hardcodes both its brushes:

```xml
<Border x:Name="Border"
        Background="{DynamicResource MahApps.Brushes.Transparent}"
        BorderBrush="{DynamicResource MahApps.Brushes.Transparent}"
        BorderThickness="1"
        CornerRadius="2">
```

Neither is a `TemplateBinding`, so `Background` and `BorderBrush` on the control are ignored. The colour behind a toolbar is the tray's:
:::

![Background set on the ToolBar and on the ToolBarTray](images/toolbar-background.png)

```xml
<ToolBarTray Background="{DynamicResource MahApps.Brushes.Gray10}">
    <ToolBar>
        <!-- items -->
    </ToolBar>
</ToolBarTray>
```

`MahApps.Styles.ToolBarTray` is one setter — a `Background` of `MahApps.Brushes.Window.Background` — so that is the only place the colour comes from unless you change it.

To colour a single bar rather than the whole tray, base a style on `MahApps.Styles.ToolBar` and replace the template, or put the bar in a tray of its own.

## The drag grip

![The tray unlocked and locked](images/toolbar-thumb.png)

The dotted grip on the left is a `Thumb` with a `SizeAll` cursor, and it is what lets the user drag a bar to another row of the tray. `ToolBarTray.IsLocked` collapses it:

```xml
<ToolBarTray IsLocked="True" />
```

The template also disables the grip while the overflow popup is open, so a bar cannot be dragged out from under its own menu.

## Overflow

When the bar is narrower than its items, the ones that do not fit move into a popup behind a chevron at the right-hand end. That button is `MahApps.Styles.ToggleButton.ToolBarOverflow`; it is disabled while `HasOverflowItems` is false, so it only lights up when there is something to show.

The popup itself is a bordered panel in `MahApps.Brushes.Control.Background` with a `ToolBarOverflowPanel` that wraps at 200 units. Mark an item as always overflowing, or never, with WPF's own attached property:

```xml
<Button Content="Rarely used" ToolBar.OverflowMode="Always" />
```

## The Windows 10 and the WinUI look

:::{.alert .alert-info}
**The Windows 10 and the WinUI toolbar are on `develop`.** They are not in 2.4.11.
:::

WPF has no `CommandBar`, and the `ToolBar` is the same thing: a row of commands with the rest behind the dots. `MahApps.Styles.ToolBar.Win10` and `MahApps.Styles.ToolBar.WinUI` give it the look of the CommandBar of UWP and of WinUI 3, and the [Win10](../stylevariants/win10) and the [WinUI](../stylevariants/winui) set apply them by type, together with `MahApps.Styles.ToolBarTray.Win10` and `.WinUI`.

```xml
<ToolBar ToolBarTray.IsLocked="True">
    <Button ToolTip="Add">
        <mah:FontIcon Glyph="&#xE710;" />
    </Button>
    <Button>
        <StackPanel Orientation="Horizontal">
            <mah:FontIcon Glyph="&#xE74E;" />
            <TextBlock Margin="8 0 0 0" VerticalAlignment="Center" Text="Save" />
        </StackPanel>
    </Button>
    <Separator />
    <ToggleButton IsChecked="True" ToolTip="Bold">
        <mah:FontIcon Glyph="&#xE8DD;" />
    </ToggleButton>
    <Button Content="Delete" ToolBar.OverflowMode="Always" />
</ToolBar>
```

The bar carries the style keys for its items in its own resources, so a Windows 10 bar has Windows 10 buttons even in a window of the default set. Seven of the eight keys are filled in. The separator gets a look of its own here, `MahApps.Styles.Separator.ToolBar.Win10` and `.WinUI`, and only `MenuStyleKey` stays empty.

| | Windows 10 | WinUI |
| --- | --- | --- |
| Bar | 40 high, in the chrome colour of the theme | 48 high and transparent, the buttons six units in from either edge |
| Button | the AppBarButton without its label, the pointer as a veil over the fill | the same with corners of 4 |
| Checked button | the accent, with the text in the colour it has under the pointer, as UWP does it | the accent button of the set, frame and bottom edge included |
| Dots | `&#xE10C;` at 16, 48 wide | `&#xE712;` at 20, 40 wide |
| Drop-down | a list in the colour of the menu flyout | rounded, in the acrylic colour of WinUI, with the shadow of the menus |
| Open | the bar stays as it is | the bar takes the colour of the drop-down and a frame, so the two read as one surface |

A few things differ from the Metro bar. The bar takes the `Background` it is given. The dots are only there while something has gone into the drop-down, as with the CommandBar, instead of sitting there disabled. And the drop-down is a list: the commands stand one under the other, each as wide as the widest, where the panel WPF has there would put short ones side by side. It opens below the dots and ends where they end.

An icon and a label are content like any other, so a label next to the icon is a `StackPanel`. There is no label below the icon the way AppBarButton draws one, and no `IsOpen` that folds the labels in and out.

The colours are in the theme as `MahApps.Brushes.ToolBar.Win10.*` and `MahApps.Brushes.ToolBar.WinUI.*`: `Background`, `Foreground`, `BackgroundOpen` and `BorderBrushOpen` (WinUI only), `OverflowBackground`, `OverflowBorderBrush`, `Separator`, `Gripper`, `ButtonBackground`, `ButtonForeground` with its `PointerOver`, `Pressed` and `Disabled` steps, and the `ToggleButton…Checked` steps of a checked button.

## Related

The toolbar button styles are variants of the ones on the [Buttons](buttons) page, and the text box and combo box inside a toolbar are the ordinary [TextBox](textbox) and [ComboBox](combobox) styles.
