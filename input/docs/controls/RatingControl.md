Title: RatingControl
Description: Stars to rate with, in the Metro, the Windows 10 and the WinUI look
---

`RatingControl` is the RatingControl of WinUI for WPF: a row of stars the user clicks to give a rating, with an optional caption after it.

:::{.alert .alert-info}
**`RatingControl` is new on `develop`.** It is not in 2.4.11.
:::

```xml
<mah:RatingControl Caption="42 ratings" Value="3" />
```

## Value

`Value` is a `double` and -1 means not rated, which is also the default. `MaxRating` gives the number of stars, 5 by default. A click sets the star under the pointer, and a click on the star of the current value clears the rating again, unless `IsClearEnabled` is off. Dragging off the left side of the row clears it as well. `ValueChanged` is raised when the user changes the rating, not when code sets `Value`.

The keyboard follows WinUI: Left and Right change the rating by one star (mirrored right to left), Up and Down do the same without the mirroring, Home clears it, or goes to the lowest rating while `IsClearEnabled` is off, and End gives the full rating. From no rating, the first step goes to `InitialSetValue`, 1 by default. `IsReadOnly` shows the value and takes no input.

## Fractions and half stars

Since `Value` is a `double`, code or a binding can set any fraction, and the star it ends in is drawn in part. The pointer and the keyboard give whole stars, as in WinUI.

`IsHalfStepEnabled` goes a step further than WinUI: the left half of a star gives a half, the right half the whole star, and the keys count in halves. The smallest rating is then 0.5. It is off by default.

```xml
<mah:RatingControl IsHalfStepEnabled="True" Value="3.5" />
```

## Placeholder

`PlaceholderValue` is shown while there is no rating, for example the average of all ratings, in the text colour instead of the accent. Unlike `Value` it may be any fraction, also below 1.

```xml
<mah:RatingControl Caption="3.5 on average, from 128 reviews"
                   IsClearEnabled="False"
                   PlaceholderValue="3.5" />
```

## Glyphs and images

`ItemInfo` decides what the items look like. A `RatingItemFontInfo` takes glyphs of a font, by default the filled and the outlined star of the symbol font, and a `RatingItemImageInfo` takes an `ImageSource` per state. Both have a property for the set, the unset, the placeholder, the pointer over a set rating, the pointer over the placeholder and the disabled state; one left empty falls back to the next, in the end to `Glyph` or `Image`.

```xml
<mah:RatingControl MaxRating="7" Value="5">
    <mah:RatingControl.ItemInfo>
        <mah:RatingItemFontInfo Glyph="&#xEB52;" UnsetGlyph="&#xEB51;" />
    </mah:RatingControl.ItemInfo>
</mah:RatingControl>
```

## The three looks

| Set | Style | Look |
|---|---|---|
| Metro, `Themes/Generic.xaml` | `MahApps.Styles.RatingControl` | the accent for set stars, outlined gray stars for the rest |
| Windows 10, `Styles/Win10/Controls.xaml` | `MahApps.Styles.RatingControl.Win10` | filled stars throughout, the unset ones in the light base colour, and the stars under the pointer grow, as in WinUI 2 |
| WinUI, `Styles/WinUI/Controls.xaml` | `MahApps.Styles.RatingControl.WinUI` | the colours of WinUI and its caption font |

The colours come from the theme: `MahApps.Brushes.RatingControl.Unselected`, `.Selected`, `.Placeholder`, `.PointerOverUnselected`, `.PointerOverPlaceholder`, `.PointerOverSelected`, `.DisabledSelected` and `.Caption` for Metro, and the same eight with `.Win10.` and `.WinUI.` in front of the state for the other two. Override these keys to change the colours; a copy of the template can't set them, since the properties that carry them into the template are internal.

## Differences to WinUI

There is no gamepad engagement and no sound. `IsHalfStepEnabled` is new. The pointer over the stars is measured the same way for the preview and for the click, where WinUI measures the two from slightly different points.

## Related

[Win10](../stylevariants/win10) and [WinUI](../stylevariants/winui) for the sets.
