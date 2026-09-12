Order: 70
Title: HeaderedControlHelper
Description: The look of a header on a GroupBox, Expander, TabControl or Flyout
---

Applies to `HeaderedContentControl` and `TabControl`, which covers `GroupBox`, `Expander`, `Flyout`, `MetroHeader` and the tab strip. A `HeaderedContentControl` gives you a `Header` but no way to style it — that is what this helper is for.

![A GroupBox with the default header and a recoloured one](images/headeredcontrolhelper.png)

| Property | Type | Default | |
| --- | --- | --- | --- |
| `HeaderBackground` | `Brush` | the panel default | behind the header |
| `HeaderForeground` | `Brush` | white | the header text |
| `HeaderFontFamily` | `FontFamily` | the system message font | |
| `HeaderFontSize` | `double` | the system message font size | |
| `HeaderFontWeight` | `FontWeight` | the system message font weight | |
| `HeaderFontStretch` | `FontStretch` | `Normal` | |
| `HeaderMargin` | `Thickness` | `0` | padding around the header content |
| `HeaderHorizontalContentAlignment` | `HorizontalAlignment` | `Stretch` | |
| `HeaderVerticalContentAlignment` | `VerticalAlignment` | `Stretch` | |
| `HeaderBackgroundMouseOver` | `Brush` | not set | behind the header while the mouse is over it |
| `HeaderBackgroundPressed` | `Brush` | not set | behind the header while it is held down |
| `HeaderForegroundMouseOver` | `Brush` | not set | the header text while the mouse is over it |
| `HeaderForegroundPressed` | `Brush` | not set | the header text while it is held down |

```xml
<GroupBox Header="Details"
          mah:HeaderedControlHelper.HeaderBackground="{DynamicResource MahApps.Brushes.Accent}"
          mah:HeaderedControlHelper.HeaderForeground="{DynamicResource MahApps.Brushes.IdealForeground}"
          mah:HeaderedControlHelper.HeaderFontSize="16"
          mah:HeaderedControlHelper.HeaderMargin="10 6">
    <TextBlock Margin="4" Text="Group box content" />
</GroupBox>
```

As with the other helpers, the defaults in the table are the helper's own; a styled control shows what its style put there. The MahApps `GroupBox` style sets `HeaderBackground` to `MahApps.Brushes.Accent` and `HeaderFontSize` to the content size, and deliberately sets `HeaderForeground` to `{x:Null}` so the template picks a foreground that reads against that band. Use the theme brushes rather than fixed colours and the header keeps following the theme.

`HeaderFontWeight` is the one to reach for when a header should be quieter — the built-in look is deliberately loud, and dropping the weight tones a nested group box down without giving up the coloured band.

## While the header is touched

**The last four are on `develop` and ship with the next release.** A released version has one background and one foreground, and a header looks the same whether somebody is on it or not.

Left unset, they hand nothing over and the header keeps the colours it has at rest, so adding them changes nothing until you fill one in. They also only reach as far as the control has a state to show:

| Control | while the mouse is over it | while it is held down |
| --- | --- | --- |
| `Expander` | yes | yes, its header is a toggle button |
| `GroupBox` | yes | no such state |
| `MetroHeader` | yes | no such state |
| `Flyout` | yes, on the header | no such state |
| `TabItem` | yes | no such state |

```xml
<Expander Header="Details"
          mah:HeaderedControlHelper.HeaderBackground="{DynamicResource MahApps.Brushes.Accent}"
          mah:HeaderedControlHelper.HeaderBackgroundMouseOver="{DynamicResource MahApps.Brushes.Highlight}"
          mah:HeaderedControlHelper.HeaderBackgroundPressed="{DynamicResource MahApps.Brushes.Accent3}">
    <TextBlock Margin="8" Text="Expander content" />
</Expander>
```

Held down wins over the mouse being over it, because both hold while a header is pressed.

A tab item takes these off the tab control it sits in, the way it takes the rest of the header properties, so set them there and every tab follows. What answers the mouse is the strip the tab is drawn in and not the item as a whole, which counts the content below it as its own.
