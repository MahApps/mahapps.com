Title: InfoBar
Description: A bar that tells the user how things stand, inline or over the top of a window, in the Metro, the Windows 10 and the WinUI look
---

`InfoBar` is the InfoBar of WinUI for WPF. It is a bar in the layout with a severity, a title, a message, an optional action and a close button, for news the user should see but does not have to answer, such as an update that is ready or a connection that is gone. Unlike a [dialog](contentdialog) it blocks nothing, and unlike a [flyout](flyouts) it does not slide in from an edge.

:::{.alert .alert-info}
**`InfoBar` is new on `develop`.** It is not in 2.4.11.
:::

```xml
<mah:InfoBar Title="Offline"
             IsOpen="True"
             Message="42 files are waiting to be synchronised."
             Severity="Warning" />
```

A bar starts closed, as in WinUI, so `IsOpen="True"` is what shows it. A closed bar takes no room in the layout. `IsOpen` binds two-way by default, so a view model can open the bar and hears about it when the user closes it.

## Severity, title and message

`Severity` is `Informational`, `Success`, `Warning` or `Error` and sets the colour of the bar and its icon. `Title` is set in semibold, `Message` follows it, and both are optional. The title, the message and the `ActionButton` stand side by side while they fit on one line, and one under the other once they do not, or once the message wraps.

```xml
<mah:InfoBar Title="Update ready"
             IsOpen="True"
             Message="Version 3 has been downloaded and will be installed the next time the app starts."
             Severity="Success">
    <mah:InfoBar.ActionButton>
        <Button HorizontalAlignment="Right" Content="Restart now" />
    </mah:InfoBar.ActionButton>
</mah:InfoBar>
```

`ActionButton` takes any element, not just a button, and `Content` with `ContentTemplate` puts anything else under the text, a progress bar for example.

## The icon

The default icons are the glyphs WinUI uses, from the Segoe MDL2 Assets font that MahApps carries with it, so they look the same on every Windows version. `Icon` takes anything in their place and shows it at 16 by 16:

```xml
<mah:InfoBar Title="Podcast" IsOpen="True" Message="A new episode is out.">
    <mah:InfoBar.Icon>
        <iconPacks:PackIconMaterial Kind="Podcast" />
    </mah:InfoBar.Icon>
</mah:InfoBar>
```

An element can stand in one bar only. To give every bar an icon of its own, set `IconTemplate` in a style instead. Without an `Icon` the template is handed the `Severity`, so one template can draw a different icon for each of them. `IsIconVisible="False"` leaves the icon out.

## Closing

`IsClosable` shows the close button, and is on by default. A click raises `CloseButtonClick` and closes the bar, and the button runs `CloseButtonCommand` with its parameter. Closing raises `Closing` with the `Reason`, `CloseButton` or `Programmatic`, and setting `Cancel` there keeps the bar open. `Closed` follows once it is gone. Setting `IsOpen` to false in code goes through both events as well.

Both events bubble, so a handler on a window hears every bar in it. When the bar closes while its close button has the keyboard focus, the focus moves on to the next element instead of getting lost.

A screen reader hears the bar as a status bar. On .NET 6 and later it is told when a bar opens, with its severity, title and message, and when one is closed. The .NET Framework build has no such notifications, so there the reader only finds the bar when it gets to it.

## Over the top of a window

`MetroWindow.InfoBars` takes an `InfoBarsControl` whose bars lie over the top of the content, right under the title bar. They push nothing aside, and the flyouts, the overlay and the dialogs lie over them. Open bars stand one under the other, closed ones take no room.

```xml
<mah:MetroWindow.InfoBars>
    <mah:InfoBarsControl>
        <mah:InfoBar Title="Update ready"
                     IsOpen="True"
                     Message="Version 3 will be installed the next time the app starts."
                     Severity="Success" />
        <mah:InfoBar Title="Offline"
                     IsOpen="True"
                     Message="42 files are waiting to be synchronised." />
    </mah:InfoBarsControl>
</mah:MetroWindow.InfoBars>
```

The informational bar has a translucent background inline. Over a window it would let the content shine through, so there it gets the opaque `MahApps.Brushes.InfoBar.Window.Informational.Background`. With `ShowTitleBar="False"` the bars move down by the height of the window buttons, so the close button of a bar does not end up under the one of the window. The bars share the `DataContext` of the window.

`InfoBarsControl` is an `ItemsControl` and works outside a window too, wherever bars should lie over something. `InfoBarsControl.IsHosted` tells a style whether a bar stands in one.

## The three looks

Each style set has its own bar, and the bar follows the set it is in.

| Set | Style | Look |
|---|---|---|
| Metro, `Styles/Controls.xaml` | `MahApps.Styles.InfoBar` | square, no border, the font of the Metro controls |
| Windows 10, `Styles/Win10/Controls.xaml` | `MahApps.Styles.InfoBar.Win10` | square with a border, the close button of the Windows 10 SDK |
| WinUI, `Styles/WinUI/Controls.xaml` | `MahApps.Styles.InfoBar.WinUI` | rounded with a border, the close button of WinUI |

The colours come from the theme and are the ones of WinUI, in a light and a dark version: `MahApps.Brushes.InfoBar.Informational.Background`, `.Success.Background`, `.Warning.Background` and `.Error.Background`, with an `.IconBackground` and an `.IconForeground` for each severity, plus `MahApps.Brushes.InfoBar.Border` and `.Foreground`. The informational icon takes the accent of the theme. The close buttons are `MahApps.Styles.Button.InfoBar.Close`, `.Close.Win10` and `.Close.WinUI`, and `CloseButtonStyle` sets one per bar.

## Differences to WinUI

There is no `IconSource`; `Icon` takes any element and `IconTemplate` covers the rest. The events are routed events and bubble. `InfoBarsControl` and `MetroWindow.InfoBars` have no counterpart in WinUI, where a bar always stands in the layout.

## Related

[ContentDialog](contentdialog) is the dialog of WinUI for a question the user has to answer. [MetroWindow](metrowindow) hosts the bars over its content, under its [flyouts](flyouts).
