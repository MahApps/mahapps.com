Order: 190
Title: VisibilityHelper
Description: Bind visibility to a bool without a converter
---

Applies to any `UIElement`. Three properties, all of them shortcuts for a `BooleanToVisibilityConverter` you would otherwise have to declare and reference.

| Property | Type | Default | `true` means | `false` means |
| --- | --- | --- | --- | --- |
| `IsVisible` | `bool?` | `null` | `Visible` | `Collapsed` |
| `IsCollapsed` | `bool?` | `null` | `Collapsed` | `Visible` |
| `IsHidden` | `bool?` | `null` | `Hidden` | `Visible` |

```xml
<TextBlock mah:VisibilityHelper.IsVisible="{Binding HasResults}" Text="..." />

<Button mah:VisibilityHelper.IsCollapsed="{Binding IsReadOnly}" Content="Edit" />
```

`IsVisible` is the common one. `IsCollapsed` is there so a view model property that reads naturally in the negative — `IsReadOnly`, `IsBusy` — does not need inverting. `IsHidden` keeps the element's space reserved, which stops a layout jumping as things appear and disappear.

:::{.alert .alert-info}
All three are `bool?`, but `null` is not a third state. The callback runs `value == true ? … : …`, so `null` is treated exactly like `false`. Never setting the property leaves `Visibility` alone, because the callback never runs — but binding one to a nullable that goes to `null` collapses the element rather than leaving it as it was.
:::

They write to the element's `Visibility`, so setting two of them on the same element is a contradiction — the last one to change wins. Pick one.

## Related

For hiding a whole region rather than one element, WPF's own `BooleanToVisibilityConverter` on a container is still the simpler answer; these properties earn their keep on the individual controls, where the converter reference is the bulk of the markup.
