Order: 105
Title: ProgressBarHelper
Description: The paused and error colours of a bar, and the dots of its indeterminate run
---

Applies to `ProgressBar`. Every property here is read by the [Windows 10 and WinUI](../stylevariants/winui) bar styles; the default MahApps bar looks at none of them.

:::{.alert .alert-info}
`ProgressBarHelper` is new in `develop` and is not in 2.4.11.
:::

| Property | Type | Default | |
| --- | --- | --- | --- |
| `ShowPaused` | `bool` | `false` | draw the bar as work that is on hold |
| `ShowError` | `bool` | `false` | draw the bar as work that has gone wrong |
| `EllipseDiameter` | `double` | `0` | how big the dots of an indeterminate run are |
| `EllipseOffset` | `double` | `0` | how much room there is between two of them |
| `AdjustIndeterminateAnimation` | `bool` | `false` | fit that animation to the width the bar actually has |

## Paused and error

Windows gives a progress bar three colours rather than one: the ordinary accent, a yellow for work that is waiting on something, and a red for work that failed. `ShowPaused` and `ShowError` are how a bar is put into the second and the third.

```xml
<ProgressBar Style="{StaticResource MahApps.Styles.ProgressBar.WinUI}"
             Value="{Binding Progress}"
             mah:ProgressBarHelper.ShowError="{Binding HasFailed}"
             mah:ProgressBarHelper.ShowPaused="{Binding IsWaiting}" />
```

Error wins where both are set. The colours come from `MahApps.Brushes.ProgressBar.Win10.Paused` and `…Error`, and from `MahApps.Brushes.ProgressBar.WinUI.Paused` and `…Error`, so a set of your own is a resource away.

## The dots

An indeterminate Windows 10 bar is five dots crossing it rather than a block or a stripe. `EllipseDiameter` sizes them and `EllipseOffset` is the room between two, both left to the style, which is where the numbers that match Windows live.

`AdjustIndeterminateAnimation` is the reason the run looks right at any width. WPF writes the distances of a storyboard down when the template is built, so an animation authored for one width crosses too far or stops short at another. With the flag on, the helper takes the storyboard the template brought, writes the numbers for the width the bar has at that moment into a copy of it, and runs the copy, redoing it when the bar is resized.

A template asking for that has to name the element the dots live in `ContainingGrid` and hand its unadjusted storyboard over under the key `IndeterminateStoryboard`, the way `MahApps.Styles.ProgressBar.Win10` does. It costs nothing while the bar is determinate: the storyboard is only started once `IsIndeterminate` is `True` and is taken off again as soon as it is not.

## Related

[ProgressBar](../styles/progressbar) is the control itself and its own styles, [MetroProgressBar](../controls/metroprogressbar) the MahApps bar with the dots built in, and [ProgressRing](../controls/progressring) the round one.
