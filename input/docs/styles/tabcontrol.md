Title: TabControl
Description: The TabControl styles
---

There are three included tab styles

- Default `TabControl` style 
- `AnimatedTabControl` style
- `SingleRowAnimatedTabControl` style

All styles are included by default and can be set by using the desired style.

## Default

![](images/default_tab_control.png)  

This shows the three states - selected/active tab, hover and inactive.

## AnimatedTabControl

```xml
<TabControl Style="{DynamicResource MahApps.Styles.TabControl.Animated}">
```

Functioning just like the regular `TabControl`, except it animates every tab change by wrapping everything in a `MetroContentControl`.

## AnimatedSingleRowTabControl

```xml
<TabControl Style="{DynamicResource MahApps.Styles.TabControl.AnimatedSingleRow}">
```

`AnimatedSingleRowTabControl` functions exactly the same as the `AnimatedTabControl` except the tabs will only appear on a single line rather than wrapping. 
Instead of wrapping, arrows (left/right) are presented.

![](images/singlerow_tab_control.png)
