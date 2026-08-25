Order: 130
Title: SliderHelper
Description: Thumb and track brushes, and mouse wheel behaviour
---

Applies to `Slider` and `RangeSlider`. Twelve brushes for the three parts a slider is made of, plus two properties for the mouse wheel.

![A slider with the default brushes and with its own](images/sliderhelper.png)

## The brushes

Named `{Part}Fill{Interaction}Brush`, where interaction is nothing at all for the resting state, or `Hover`, `Pressed` or `Disabled`.

| Part | |
| --- | --- |
| `Thumb` | the handle you drag |
| `Track` | the groove, on the side the value has not reached |
| `TrackValue` | the groove, on the side it has |

So twelve properties: `ThumbFillBrush`, `ThumbFillHoverBrush`, `ThumbFillPressedBrush`, `ThumbFillDisabledBrush`, and the same four for `Track` and `TrackValue`.

```xml
<Slider Maximum="100" Value="40"
        mah:SliderHelper.ThumbFillBrush="#2E7D32"
        mah:SliderHelper.TrackValueFillBrush="#2E7D32"
        mah:SliderHelper.TrackFillBrush="#C8E6C9" />
```

All twelve default to `null`, and the MahApps slider style fills them in from the theme. Setting only the resting brush leaves hover, pressed and disabled on the theme colour, so a slider that should be green throughout wants the hover and pressed variants too.

## Mouse wheel

| Property | Type | Default | |
| --- | --- | --- | --- |
| `EnableMouseWheel` | `MouseWheelState` | `None` | when the wheel changes the value |
| `ChangeValueBy` | `MouseWheelChange` | `SmallChange` | how much one notch moves it |

`MouseWheelState` is `None`, `ControlFocused` or `MouseHover`. The wheel does nothing by default, which is deliberate — a slider inside a scrolling page would otherwise swallow the scroll.

```xml
<Slider mah:SliderHelper.EnableMouseWheel="MouseHover"
        mah:SliderHelper.ChangeValueBy="LargeChange"
        SmallChange="1"
        LargeChange="10" />
```

`ChangeValueBy` picks which of the slider's own `SmallChange` and `LargeChange` a notch applies, so the step size itself is still set on the slider.
