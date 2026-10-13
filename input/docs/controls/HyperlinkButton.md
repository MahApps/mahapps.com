Title: HyperlinkButton
Description: A button that looks like a link, in the Metro, the Windows 10 and the WinUI look
---

`HyperlinkButton` is the HyperlinkButton of WinUI for WPF. It is a `ButtonBase` that looks like a link, for a link that stands on its own: under a form, in a dialog, below a list. The WPF [Hyperlink](../styles/hyperlink) is an inline that only lives inside a `TextBlock`; the button stands anywhere a button can, takes any content and has `Click`, `Command` and the focus of a button.

:::{.alert .alert-info}
**`HyperlinkButton` is new on `develop`.** It is not in 2.4.11.
:::

```xml
<mah:HyperlinkButton Content="mahapps.com" NavigateUri="https://mahapps.com/" />
```

## Navigating

A click raises `Click` and runs the `Command` first. With a `NavigateUri` it then raises `RequestNavigate`, which is the very event of the WPF `Hyperlink`, so a `Frame` or a `NavigationWindow` around the button navigates on its own, and any handler can take the URI by setting `Handled`.

When nobody takes it, an `http`, `https` or `mailto` URI opens in the program Windows has for it. Every other URI, a relative one, a `file:` one or one of a custom scheme, goes to the handlers alone. WinUI opens every scheme but `file:`; it does that from a sandbox, and a desktop app has none, so a URI from data the app does not control could start whatever program registered its scheme. An app that wants another scheme opened handles `RequestNavigate` and decides there. `LaunchUri` is the method that opens the web and mail URIs and can be overridden, for example to open them in a `WebView2` of the app instead.

```csharp
private void OnRequestNavigate(object sender, RequestNavigateEventArgs e)
{
    if (e.Uri.Scheme == "gallery")
    {
        this.ShowPage(e.Uri.AbsolutePath);
        e.Handled = true;
    }
}
```

Without a `NavigateUri` the button is a plain button: `Click` and `Command`, and nothing opens.

## The underline

Only text content is underlined, as in WinUI: a string, `_Help` with its access key included. An element given as content keeps its own look, so an image or a `TextBlock` of your own gets no line under it.

`IsUnderlineVisible` switches the underline. The Metro and the Windows 10 look have it on, WinUI has it off, because WinUI sets `HyperlinkUnderlineVisible` to False and only draws the line in high contrast.

## The three looks

Each style set has its own button, and the button follows the set it is in.

| Set | Style | Look |
|---|---|---|
| Metro, `Styles/Controls.xaml` | `MahApps.Styles.HyperlinkButton` | the text colour, underlined, `MahApps.Brushes.Highlight` under the mouse, no padding |
| Windows 10, `Styles/Win10/Controls.xaml` | `MahApps.Styles.HyperlinkButton.Win10` | the accent, underlined, the medium base colour under the mouse, the padding of the Windows SDK |
| WinUI, `Styles/WinUI/Controls.xaml` | `MahApps.Styles.HyperlinkButton.WinUI` | the accent, no underline, the padding and the rounded corners of a button, a subtle fill under the mouse and while pressed |

The text colours are the ones of the [link](../styles/hyperlink) in the same set: `MahApps.Brushes.Hyperlink.Win10.Foreground`, `.ForegroundPointerOver`, `.ForegroundPressed` and `.ForegroundDisabled`, and the same four for `WinUI`. Overriding them colours the inline link and the button alike.

## Differences to WinUI

The URI goes through `RequestNavigate` first, so the app can take it, and only web and mail URIs open by themselves. `IsUnderlineVisible` is a property rather than a theme resource. WinUI writes the link in a darker step of the accent in the light theme and a lighter one in the dark theme; the themes here do not have those steps yet, so it is the accent itself.

## Related

[Hyperlink](../styles/hyperlink) for the link in running text, [Win10](../stylevariants/win10) and [WinUI](../stylevariants/winui) for the sets.
