Order: 65
Title: GroupItemHelper
Description: A group header that stays at the top, and one that picks its whole group
---

Applies to a grouped `ListView` or `ListBox`. It gives a group of items two things beyond standing there: a header that holds at the top while its own items pass under it, and a box in that header picking the whole group at once.

:::{.alert .alert-info}
`GroupItemHelper` is new in `develop` and is not in 2.4.11.
:::

| Property | Type | Default | |
| --- | --- | --- | --- |
| `IsHeaderSticky` | `bool` | `false` | hold the header at the top while the group scrolls under it |
| `CanSelectAllItems` | `bool` | `false` | let the box in the header stand for every item of the group |
| `GroupContainerStyle` | `Style` | `null` | the style for a group |
| `GroupContainerStyleWithCheckBox` | `Style` | `null` | the same with a box beside the name of the group |
| `IsGroupSelected` | `bool?` | `null` | whether the group is picked; `null` for some of it |

Both of the first two are set on the **list**, not on the group, because a group is built and thrown away again as the list scrolls. They inherit, so the value on the list reaches every group in it.

```xml
<ListView ItemsSource="{Binding AlbumsView}"
          SelectionMode="Extended"
          Style="{StaticResource MahApps.Styles.ListView.Win10}"
          mah:GroupItemHelper.CanSelectAllItems="True"
          mah:GroupItemHelper.IsHeaderSticky="True"
          mah:ItemHelper.IsMultiSelectCheckBoxEnabled="True" />
```

`MahApps.Styles.ListView.Win10` asks for both already, and `MahApps.Styles.ListView.WinUI` brings the two group styles along without the sticky header.

## The header that stays

WPF lays a group out as one `GroupItem` holding a header with the items under it, and scrolls the two together, so the header of the group you are reading leaves the top of the view before its own items do.

Holding it means moving it down by as much as the group has already passed the top edge, and no further than the bottom of that group, so the next group pushes it out of the way rather than drawing over it. The move is a render transform, so nothing is laid out again while the list scrolls.

:::{.alert .alert-warning}
**The list has to scroll by the pixel.** A list scrolling by the row, which is what WPF does unless told otherwise, never lets a group past the top edge at all: it puts a whole row at the top and shifts the contents of the group instead, so there is nothing to hold. Set `VirtualizingPanel.ScrollUnit` to `Pixel`, which is what `MahApps.Styles.ListView.Win10` does.

A style of its own has to name the element to hold `PART_Header`, and that element has to be free to draw over the items, which means the two sharing one cell of a grid with the header declared after them.
:::

## The box that picks a group

`CanSelectAllItems` lets the box in a group header stand for all of the items under it: ticking it picks them, clearing it lets them go, and a group only some of whose items are picked shows the third state. It needs a list that allows more than one item to be picked, since there is nothing for it to do otherwise.

`IsGroupSelected` is that answer as a property, `true`, `false` or `null` for a group that is partly picked, should you want to read or write it yourself.

The box only appears where [ItemHelper](itemhelper)'s `IsMultiSelectCheckBoxEnabled` asks for boxes on the rows as well. That is what `GroupContainerStyleWithCheckBox` is for: the helper hands that style to a group instead of the plain `GroupContainerStyle` while both flags are asking for it.

Two styles rather than one with a trigger inside it, because a trigger naming an attached property of this library costs a thrown exception every time WPF builds the style from inside a page, where the `mah:` prefix means nothing.

## Related

[ItemHelper](itemhelper) carries the per-state brushes of the rows and the box saying what is picked. [ListView](../styles/listview) and [ListBox](../styles/listbox) are the lists themselves.
