Title: PipsPager
Description: A row of dots for paging, in the Metro, the Windows 10 and the WinUI look
---

`PipsPager` is the PipsPager of WinUI for WPF: a row of dots, one per page, that shows which page is up and lets the user pick another. It knows nothing about the pages themselves, only how many there are and which one is selected, so it goes with anything that pages: a [FlipView](flipview), a wizard, a list shown a page at a time.

:::{.alert .alert-info}
**`PipsPager` is new on `develop`.** It is not in 2.4.11.
:::

```xml
<mah:PipsPager NumberOfPages="10" SelectedPageIndex="{Binding Page}" />
```

## Pages

`NumberOfPages` is the number of dots, and -1, the default, means the number is not known: the pager then shows `MaxVisiblePips` dots and adds one whenever the last of them is selected. `SelectedPageIndex` starts at 0 and binds two ways by default. A value past the last page is set back to the last one, a value below 0 to the first. `SelectedIndexChanged` is raised once for the value that ends up selected.

A click on a dot selects it. With the keyboard, Tab lands on the selected dot, the arrow keys move from dot to dot, Left and Up back, Right and Down on, whatever the orientation, and Enter or Space selects the dot that has the focus.

## Many pages

`MaxVisiblePips`, 5 by default, is how many dots are shown at once. With more pages the row becomes a window that slides along and keeps the selected dot in the middle, so a pager of a hundred pages takes no more room than one of five.

```xml
<mah:PipsPager MaxVisiblePips="7" NumberOfPages="100" />
```

## The arrows

`PreviousButtonVisibility` and `NextButtonVisibility` are `Collapsed` by default. `Visible` shows the arrow, `VisibleOnPointerOver` shows it only while the pointer is over the pager or a dot has the keyboard focus. At the first and the last page the arrow for that way is hidden and disabled, unless `WrapMode` is `Wrap`, which takes the last page on to the first and back.

```xml
<mah:PipsPager NextButtonVisibility="VisibleOnPointerOver"
               NumberOfPages="8"
               PreviousButtonVisibility="VisibleOnPointerOver"
               WrapMode="Wrap" />
```

`Orientation` lays the row out across or down. `PreviousButtonStyle`, `NextButtonStyle`, `NormalPipStyle` and `SelectedPipStyle` take the styles of the two arrows and the two kinds of dot. A dot style that wants to know the orientation, for a cell that is wider than high, triggers on the attached `PipsPager.PipOrientation`, which the pager hands down to its dots.

## With a FlipView

A pager next to a flip view needs the number of items and the selected index:

```xml
<mah:FlipView x:Name="Pictures" ItemsSource="{Binding Pictures}" />
<mah:PipsPager NumberOfPages="{Binding Items.Count, ElementName=Pictures}"
               SelectedPageIndex="{Binding SelectedIndex, ElementName=Pictures, Mode=TwoWay}" />
```

The WinUI look of the [FlipView](flipview) does this on its own and shows a pager as its index.

## The three looks

| Set | Style | Look |
|---|---|---|
| Metro, `Themes/Generic.xaml` | `MahApps.Styles.PipsPager` | the dots of the flip view index, the selected one in the accent |
| Windows 10, `Styles/Win10/Controls.xaml` | `MahApps.Styles.PipsPager.Win10` | the small dots of WinUI 2 in a cell of 12 by 20, and square arrows of 20 |
| WinUI, `Styles/WinUI/Controls.xaml` | `MahApps.Styles.PipsPager.WinUI` | the same dots in a cell of 12 by 24, and arrows of 24 rounded like the other controls of the set |

In both of the newer looks a dot grows under the pointer and the selected one is the bigger, and the dots are in the strong fill colour of WinUI rather than the accent, as in the original.

## What a client is told

The pager offers its pages as a selection, with the selected dot as the one selected. It is called "Pager" unless it has a name of its own, each dot "Page 3" with its place in the set, and the two arrows "Previous Page" and "Next Page", in German "Seitenanzeige", "Seite", "Vorherige Seite" and "Nächste Seite". On .NET Framework 4.6.2 the dots carry no place in the set, since WPF has that only from 4.7.2 on.

## Related

The pager is a port of the PipsPager of [microsoft-ui-xaml](https://github.com/microsoft/microsoft-ui-xaml). The [FlipView](flipview) uses it as the index of its WinUI look.
