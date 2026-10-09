Order: 165
Title: ToolTipHelper
Description: Puts a tool tip where Windows puts it and closes it when the page scrolls
---

Applies to `ToolTip`. On `develop` only; it ships with the next release.

WPF puts a tool tip below and to the right of the pointer. Windows puts it above: `ToolTip_Partial.cpp` of WinUI has `PlacementMode_Top` as its default, centres the tip on the pointer with 20 between the two, and centres it on the control with 12 between them when the keyboard opened it. Where there is no room above, the tip goes below.

| Property | Type | Default |
| --- | --- | --- |
| `PlaceLikeWindows` | `bool` | `False` |
| `CloseOnScroll` | `bool` | `False` |

`PlaceLikeWindows="True"` does the same in WPF. The tool tips of the [Win10](../stylevariants/win10) and the [WinUI](../stylevariants/winui) look turn it on, the Metro one leaves it off.

```xml
<ToolTip Content="Writes the document to disk"
         mah:ToolTipHelper.PlaceLikeWindows="True" />
```

What it does is set `Placement` to `Custom` and hand the tool tip a `CustomPopupPlacementCallback`, so the two are taken while it is on. A margin around the first element of the template counts as room for a shadow and is left out of the gap, which is how the WinUI tool tip stands where Windows puts it in spite of its shadow. `HorizontalOffset` and `VerticalOffset` move the tip on from there.

A `ToolTipService.Placement` set on the control still wins, as it does over any placement a style sets, so a single control can have its tool tip elsewhere without a style of its own.

## Closing when the page scrolls

The wheel moves a control away from under a pointer that stands still. WPF leaves the tool tip where it was then, so it ends up next to something it does not explain. Windows closes it. `CloseOnScroll="True"` does the same, and every tool tip of the library has it on, the Metro one included: while the tip is open, it closes it as soon as a scroll viewer the control sits in changes its offset. A scroll viewer somewhere else in the window leaves it alone.
