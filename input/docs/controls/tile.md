Title: Tile
---

The `Tile` is exactly as it sounds. It is a rectangular control whose sole purpose is to mimick a tile from Window 8 / WinRT's / Windows 10 Start Screen.

```xml
<Controls:Tile Title="Mail"
               Margin="3"
               Controls:ControlsHelper.MouseOverBorderBrush="{DynamicResource MahApps.Brushes.ThemeForeground}"
               Background="Teal"
               HorizontalTitleAlignment="Right">
    <iconPacks:PackIconModern Width="40"
                              Height="40"
                              Kind="Email" />
</Controls:Tile>
```
![](images/tiles.png)
