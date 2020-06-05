Title: Badged Control
Description: If you need to show a badge over a control then this is the right control
---

The `Badged` control renders a badge overlay on a given control.

```xml
<mah:Badged Badge="42">
  <!-- Control to wrap goes here -->
  <Button Content="Notifications" />
</mah:Badged>
```

![](images/badged-control-button.png)

The badge location can be set by the `BadgePlacementMode` property. The values are:

```csharp
public enum BadgePlacementMode
{
    TopLeft,
    Top,
    TopRight,
    Right,
    BottomRight,
    Bottom,
    BottomLeft,
    Left
}
```

```xml
<mah:Badged Badge="{Binding Path=BadgeValue}" BadgePlacementMode="BottomRight">
  <!-- Control to wrap goes here -->
  <Button>
    <iconPacks:PackIconFontAwesome Kind="CommentOutline"/>
  </Button>
</mah:Badged>
```

![](images/badged-control-button-icon.png)

# Properties

| Property           | Type               |                                          |
|--------------------|--------------------|------------------------------------------|
| Badge              | object             | Gets or sets the Badge content to display. |
| BadgeBackground    | Brush              | Gets or sets the background brush for the Badge. |
| BadgeForeground    | Brush              | Gets or sets the foreground brush for the Badge. |
| BadgePlacementMode | BadgePlacementMode | Gets or sets the placement of the Badge relative to its content. |
| BadgeMargin        | Thickness          | Gets or sets a margin which can be used to make minor adjustments to the placement of the Badge. |
| IsBadgeSet         | bool (readonly)    | Indicates whether the Badge has content to display. |
| BadgeChanged       | RoutedEvent        | Occurs if the Badge property was changed. |
