Order: 170
Title: TreeViewItemHelper
Description: The expander button of a TreeViewItem
---

Applies to `TreeViewItem`. One property, for the little triangle that opens and closes a node.

| Property | Type | Default | |
| --- | --- | --- | --- |
| `ToggleButtonStyle` | `Style` | `null`, set by the item style | style of the expand and collapse button |

```xml
<TreeView>
    <TreeView.ItemContainerStyle>
        <Style BasedOn="{StaticResource MahApps.Styles.TreeViewItem}" TargetType="TreeViewItem">
            <Setter Property="mah:TreeViewItemHelper.ToggleButtonStyle" Value="{StaticResource BigExpander}" />
        </Style>
    </TreeView.ItemContainerStyle>
</TreeView>
```

Set it through `ItemContainerStyle` rather than on the `TreeView`. The helper's own default is `null`; the MahApps `TreeViewItem` style is what puts `MahApps.Styles.ToggleButton.TreeViewItem.ExpandCollapse` there, and a style setter on the item beats a value inherited from the tree.

The style targets `ToggleButton`. Base your own on the built-in one unless you intend to draw the whole button yourself:

```xml
<Style x:Key="BigExpander"
       BasedOn="{StaticResource MahApps.Styles.ToggleButton.TreeViewItem.ExpandCollapse}"
       TargetType="{x:Type ToggleButton}">
    <Setter Property="Width" Value="24" />
    <Setter Property="Height" Value="24" />
</Style>
```

## Related

The colours of the row itself — selected, hovered, disabled — come from [ItemHelper](itemhelper), which applies to `TreeViewItem` as well as to `ListBoxItem`.
