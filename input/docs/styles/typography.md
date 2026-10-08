Title: Typography
Description: The fonts of the three looks and their type ramps as TextBlock styles
---

:::{.alert .alert-info}
**The keys and styles on this page are on `develop` and ship with the next release.** 2.4.11 has only the Metro keys of `Styles/Fonts.xaml`, and its Windows 10 styles write most controls at 12.
:::

Each of the three looks writes in its own font. Metro has the keys of `Styles/Fonts.xaml` it has always had. The Windows 10 and the WinUI look take Microsoft's values: Segoe UI for Windows 10, as the UWP `generic.xaml` has it, and Segoe UI Variable for WinUI, as `TextBlock_themeresources.xaml` of WinUI has it. Controls of both are written at 14, the `ControlContentThemeFontSize` of both.

## The keys

| Key | Value |
| --- | --- |
| `MahApps.Fonts.Family.Control.Win10` | `Segoe UI` |
| `MahApps.Fonts.Family.Control.WinUI` | `Segoe UI Variable Text, Segoe UI` |
| `MahApps.Fonts.Family.Caption.WinUI` | `Segoe UI Variable Small, Segoe UI` |
| `MahApps.Fonts.Family.Display.WinUI` | `Segoe UI Variable Display, Segoe UI` |

`MahApps.Font.Size.Control.Win10` and `MahApps.Font.Size.Control.WinUI` are both 14, and every step of a ramp below has a size key of its own, such as `MahApps.Font.Size.Title.WinUI`. Every style of a set reads them as dynamic resources, so overriding a key in a window or in `App.xaml` changes what reads it and nothing else:

```xml
<system:Double x:Key="MahApps.Font.Size.Title.WinUI">32</system:Double>
```

Segoe UI Variable is a variable font, and WPF does not drive its optical size axis. What it does know are the three named cuts of it, Small, Text and Display, each with all its weights, so the WinUI keys pick the cut Microsoft assigns to the size: Small for the caption, Text up to the large body text, Display from the subtitle up. Windows 10 does not have Segoe UI Variable, and there every one of these keys falls back to Segoe UI.

## The ramps

A ramp is a set of `TextBlock` styles, one per step. They are keyed, never implicit, which is what Microsoft does too; the [Text](text) page has the reason a `TextBlock` style must never be implicit. A text picks its step by key:

```xml
<TextBlock Style="{DynamicResource MahApps.Styles.TextBlock.Title.WinUI}" Text="Deep Thought" />
```

WinUI:

| Style | Size | Weight |
| --- | --- | --- |
| `MahApps.Styles.TextBlock.Caption.WinUI` | 12 | Normal, Small cut |
| `MahApps.Styles.TextBlock.Body.WinUI` | 14 | Normal |
| `MahApps.Styles.TextBlock.BodyStrong.WinUI` | 14 | SemiBold |
| `MahApps.Styles.TextBlock.BodyLarge.WinUI` | 18 | Normal |
| `MahApps.Styles.TextBlock.BodyLargeStrong.WinUI` | 18 | SemiBold |
| `MahApps.Styles.TextBlock.Subtitle.WinUI` | 20 | SemiBold, Display cut |
| `MahApps.Styles.TextBlock.Title.WinUI` | 28 | SemiBold, Display cut |
| `MahApps.Styles.TextBlock.TitleLarge.WinUI` | 40 | SemiBold, Display cut |
| `MahApps.Styles.TextBlock.Display.WinUI` | 68 | SemiBold, Display cut |

Windows 10:

| Style | Size | Weight |
| --- | --- | --- |
| `MahApps.Styles.TextBlock.Caption.Win10` | 12 | Normal |
| `MahApps.Styles.TextBlock.Body.Win10` | 14 | Normal |
| `MahApps.Styles.TextBlock.Base.Win10` | 14 | SemiBold |
| `MahApps.Styles.TextBlock.Subtitle.Win10` | 20 | Normal |
| `MahApps.Styles.TextBlock.Title.Win10` | 24 | SemiLight |
| `MahApps.Styles.TextBlock.Subheader.Win10` | 34 | Light |
| `MahApps.Styles.TextBlock.Header.Win10` | 46 | Light |

WPF has no name for SemiLight, so the Windows 10 title sets the weight as the number 350, which is what SemiLight is.

Metro, made of the keys `Styles/Fonts.xaml` already had:

| Style | Size | Font |
| --- | --- | --- |
| `MahApps.Styles.TextBlock.Caption` | 12, `MahApps.Font.Size.Content` | `MahApps.Fonts.Family.Control` |
| `MahApps.Styles.TextBlock.Body` | 14, `MahApps.Font.Size.Default` | `MahApps.Fonts.Family.Control` |
| `MahApps.Styles.TextBlock.Subtitle` | 20, `MahApps.Font.Size.Flyout.Header` | `MahApps.Fonts.Family.Control` |
| `MahApps.Styles.TextBlock.Subheader` | 29.333, `MahApps.Font.Size.SubHeader` | `MahApps.Fonts.Family.Header` (Segoe UI Light) |
| `MahApps.Styles.TextBlock.Header` | 40, `MahApps.Font.Size.Header` | `MahApps.Fonts.Family.Header` (Segoe UI Light) |

None of the ramps sets a line height. WinUI does not either; the line heights in Microsoft's type ramp come from the font.

## The window

A plain `TextBlock` takes its font from the window it is in. The [Windows 10](../stylevariants/win10) and the [WinUI](../stylevariants/winui) set therefore bring a window style each, `MahApps.Styles.MetroWindow.Win10` and `MahApps.Styles.MetroWindow.WinUI`. The content of the window gets the font of the set at 14, and the title the font Windows writes its title bars in, at 12. The title has keys of its own, `MahApps.Font.Size.Window.Title.Win10` and `.WinUI` and the families `MahApps.Fonts.Family.Window.Title.Win10` and `.WinUI`, which the title bar of the [ContentDialog](../controls/contentdialog) reads as well.

A set hands these out by type, and WPF gives a style handed out that way only to that exact type. A window of your own that derives from `MetroWindow`, which is nearly every window, does not get it and has to name the style itself:

```xml
<mah:MetroWindow x:Class="MyApp.MainWindow"
                 Style="{DynamicResource MahApps.Styles.MetroWindow.WinUI}">
```

## Related

[Text](text) covers the `TextBlock` and `Label` styles and why there is no implicit `TextBlock` style. The dialogs of both looks, the [ContentDialog](../controls/contentdialog) and the [old dialogs](../dialogs/dialogsettings), take their title and their message from these ramps. Since every key is read as a dynamic resource, a single dialog can be given another title font by putting the key into its `CustomResourceDictionary`.
