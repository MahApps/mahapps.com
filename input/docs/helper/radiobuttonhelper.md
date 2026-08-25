Order: 110
Title: RadioButtonHelper
Description: Size and per-state brushes of a RadioButton
---

Applies to `RadioButton`. Like [CheckBoxHelper](checkboxhelper) it is three sizes plus one naming scheme for the brushes.

![RadioSize and recoloured brushes](images/radiobuttonhelper.png)

## Size

| Property | Type | Default | |
| --- | --- | --- | --- |
| `RadioSize` | `double` | `18` | diameter of the outer circle |
| `RadioCheckSize` | `double` | `10` | diameter of the dot inside it |
| `RadioStrokeThickness` | `double` | `1` | thickness of the outline |

```xml
<RadioButton mah:RadioButtonHelper.RadioSize="26"
             mah:RadioButtonHelper.RadioCheckSize="14"
             Content="Selected" />
```

## The brushes

Named `{Part}{Interaction}`, where interaction is nothing at all for the resting state, or `PointerOver`, `Pressed` or `Disabled`.

| Part | |
| --- | --- |
| `Foreground` | the label |
| `Background` | behind the whole control |
| `BorderBrush` | around the whole control |
| `OuterEllipseFill` | inside the circle, while unselected |
| `OuterEllipseStroke` | its outline, while unselected |
| `OuterEllipseCheckedFill` | inside the circle, while selected |
| `OuterEllipseCheckedStroke` | its outline, while selected |
| `CheckGlyphFill` | the dot |
| `CheckGlyphStroke` | the dot's outline |

`Foreground`, `Background` and `BorderBrush` have no resting variant — that is the control's own property. The other six times four combinations make up the rest of the 36.

```xml
<RadioButton mah:RadioButtonHelper.OuterEllipseCheckedFill="#2E7D32"
             mah:RadioButtonHelper.OuterEllipseCheckedStroke="#2E7D32"
             mah:RadioButtonHelper.CheckGlyphFill="White"
             mah:RadioButtonHelper.CheckGlyphStroke="White"
             Content="Selected" />
```

Note there is one set of ellipse brushes for unselected and another for selected, but only one set for the dot — the dot is not drawn when nothing is selected, so it needs no unselected variant.

:::{.alert .alert-info}
This helper says `PointerOver` where `CheckBoxHelper` says `MouseOver`. The two were written at different times and the naming was never reconciled, so a check box and a radio button that should match need different property names for the same state.
:::

## Related

`ToggleButtonHelper.ContentDirection` puts the circle on the right of the label. See [ToggleButtonHelper](togglebuttonhelper).
