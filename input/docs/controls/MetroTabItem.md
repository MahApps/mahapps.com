Title: MetroTabItem
Description: A TabItem with a close button
---

`MetroTabItem` is a `TabItem` with a close button and the four properties around it. It is what [MetroTabControl](MetroTabControl) creates for you when you bind `ItemsSource`, and its style is based on `MahApps.Styles.TabItem`, so everything on the [TabControl styles](../styles/tabcontrol) page applies here too.

![CloseButtonEnabled off and on](images/metrotabitem-closebutton.png)

```xml
<mah:MetroTabControl>
    <mah:MetroTabItem Header="General" CloseButtonEnabled="True">
        <TextBlock Margin="10" Text="First page" />
    </mah:MetroTabItem>
</mah:MetroTabControl>
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `CloseButtonEnabled` | `bool` | `False` | whether the tab has a close button at all |
| `CloseButtonMargin` | `Thickness` | `0` | where that button sits |
| `CloseTabCommand` | `ICommand` | `null` | run when the button is clicked |
| `CloseTabCommandParameter` | `object` | `null` | what the command is handed |

## The button appears on the selected tab

Look at the figure: only *Display* has a cross, and all three tabs have `CloseButtonEnabled="True"`.

`CloseButtonEnabled` alone puts the button in the layout but leaves it `Hidden` — it takes up its space so the header does not jump. Two triggers make it `Visible`: the tab being **selected**, and the pointer being **over** it. That is the usual pattern for closable tabs, and it is why the figure shows one cross rather than three.

The button's size follows `HeaderedControlHelper.HeaderFontSize` through a converter, so it stays in proportion when you shrink the tab headers.

:::{.alert .alert-info}
`CloseButtonEnabled` and `CloseButtonMargin` are declared with `FrameworkPropertyMetadataOptions.Inherits`, but they are properties of `MetroTabItem`, not attached properties — you cannot write them on the `MetroTabControl`. Put them on each item, or in an `ItemContainerStyle`:

```xml
<mah:MetroTabControl.ItemContainerStyle>
    <Style BasedOn="{StaticResource {x:Type mah:MetroTabItem}}" TargetType="{x:Type mah:MetroTabItem}">
        <Setter Property="CloseButtonEnabled" Value="True" />
    </Style>
</mah:MetroTabControl.ItemContainerStyle>
```
:::

## What the close button does

The button raises no `Click` you can handle — a `CloseTabItemAction` behaviour is attached to it, and that runs a fixed sequence:

1. **The item's `CloseTabCommand`** executes, if it is set and `CanExecute` allows it.
2. Then the control's `CloseThisTabItem` runs, which either executes the **control's** `CloseTabCommand` and stops, or raises the cancellable `TabItemClosingEvent` and removes the tab.

:::{.alert .alert-warning}
The two commands are not alternatives, and this is the part that catches people:

**The item's command cannot stop the tab from closing.** It runs first, the sequence continues regardless, and a `CanExecute` that returns `false` only means the command does not run — the tab still goes. Use it for the side effect: save the document, log the close, release a handle.

To *prevent* a close you need one of the other two: set `CloseTabCommand` on the [MetroTabControl](MetroTabControl), which replaces the removal entirely, or handle `TabItemClosingEvent` and set `e.Cancel`.
:::

```xml
<mah:MetroTabItem Header="Report"
                  CloseButtonEnabled="True"
                  CloseTabCommand="{Binding SaveBeforeCloseCommand}"
                  CloseTabCommandParameter="{Binding RelativeSource={RelativeSource Self}, Path=Header}" />
```

`CloseTabCommandParameter` does double duty: it is the parameter for the item's command, and — when the item's command is not set — the control's `CloseTabCommand` receives it too, falling back to the `MetroTabItem` itself.

## The plain TabItem alternative

`TabControlHelper` has `CloseButtonEnabled`, `CloseTabCommand` and `CloseTabCommandParameter` as attached properties that work on an ordinary `TabItem`.

:::{.alert .alert-warning}
Those three are read **only by the Visual Studio tab style** in `Styles/VS/TabControl.xaml`. Under the ordinary MahApps styles they do nothing, and the close button in the figure above comes from `MetroTabItem`'s own properties. If you want closable tabs without the Visual Studio look, use `MetroTabItem`.
:::

## Related

[MetroTabControl](MetroTabControl) for the closing pipeline seen from the other end, [TabControl](../styles/tabcontrol) for the look, and [TabControlHelper](../helper/tabcontrolhelper) for the underline and the transition.
