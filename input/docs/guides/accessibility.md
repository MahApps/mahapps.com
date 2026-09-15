Order: 40
Title: What a screen reader hears
Description: The names and values this library hands to assistive technology, and what it leaves out
---

A screen reader, an inspection tool and a UI test all ask the same thing: what is this element, what is it called, and what does it hold. WPF answers most of that from the control itself, and a control library has to fill in the rest.

## Names for buttons drawn from a picture

The name of a button comes from its content, and the content of a button drawn from a glyph is the path that glyph is cut from. Point a screen reader at one and it reads the coordinates out.

Every button of this kind carries a name of its own: the clear button on a text box, the one that opens a picker, the arrows of a scroll bar, the ones that scroll a tab strip, the button that shuts a flyout, the window buttons, and the overflow button beside the window commands.

:::{.alert .alert-warning}
**In a released version several of them read out their path data.** The button that opens a picker announces a page and a half of coordinates, and the one that empties a text box announces the letter its cross is cut from. Fixed on `develop`, which is [#4454](https://github.com/MahApps/MahApps.Metro/issues/4454).
:::

### They are translated

Microsoft asks for an accessible name the same way it asks for anything on screen: accurate, concise, unambiguous and localized. The names live in `Lang/Automation.resx`, with a German set beside it, and a language nobody has translated falls back to English, the way the colour names do. A translation is a welcome pull request: copy `Automation.resx` to `Automation.<culture>.resx` and fill in the values.

The button that opens a picker says *Show Calendar*, which is the wording WPF gives its own `DatePicker`, and *Show Clock* where the drop-down has no calendar in it.

## What each control tells a client

| | |
| --- | --- |
| `Tile` | named after its `Title` when the content leaves the name empty |
| `Badged` | hands the badge over as the status of what it wraps |
| `RangeSlider` | a slider reading out both ends, with each outer thumb carrying the value it stands for |
| `FlyoutsControl` | answers a point only while a flyout is open, so the content underneath stays reachable |
| `ToggleSwitch`, `NumericUpDown`, `MetroWindow`, `Flyout`, `MetroHeader`, `ProgressRing`, `WindowCommands` | carry a peer of their own |

Everything else takes what its base type gives it. A `MetroTabControl` is a tab control, a `MetroProgressBar` a progress bar, a `SplitButton` a combo box, and each of them reports the class name of that base type rather than its own.

:::{.alert .alert-info}
**Still missing on `develop`.** A `HotKeyBox` does not hand over the combination it holds. A `DropDownButton` arrives as a list with no name rather than as a button. A `FlipView` is a list with neither its content nor its banner. A `RevealImage` leaves its caption out. The `ColorCanvas` brings all seven channels as sliders, but the sliders have no names of their own.
:::

## Reaching a window at all

An inspection tool asks Windows what sits under the pointer, and that question goes by the box an element occupies rather than by what a click would hit. Anything lying over the whole window answers it, whether or not it draws something.

That is what the flyouts control did from .NET 8 on, which left nothing in a `MetroWindow` reachable for Accessibility Insights, inspect.exe, FlaUInspect or WinAppDriver. It is fixed on `develop`, and the [Flyouts](../controls/flyouts) page has the detail.

## Related

[RangeSlider](../controls/rangeslider) and [Slider](../styles/slider) describe how a test drives them. [Tile](../controls/tile) and [Badged](../controls/badged) say what each hands to a reader.
