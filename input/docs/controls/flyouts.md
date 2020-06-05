Title: Flyouts
Description: An overlay control that can slide and render custom contents
---

A `Flyout` is an overlay for the [MetroWindow](MetroWindow) that can render custom contents. It has the ability to slide in and out from any side of the window.

It is strongly recommended to use the `Flyouts` within the `FlyoutsControl`. This control should be assigned to the `Flyouts` property of the [MetroWindow](MetroWindow).

```xml
<mah:MetroWindow.Flyouts>
    <mah:FlyoutsControl>
      <mah:Flyout x:Name="firstFlyout" Header="Flyout" Position="Right" Width="200">
          <!-- Your custom content here -->
      </mah:Flyout>

      <mah:Flyout Header="Second Flyout" IsOpen="{Binding IsSecondFlyoutOpen}" Position="Right" Width="300">
          <!-- Your custom content here -->
      </mah:Flyout>
    </mah:FlyoutsControl>
</mah:MetroWindow.Flyouts>
```

This creates 2 flyouts with a header, sliding out from the right side of the window.

To open a `Flyout` set the `IsOpen` property to `true` or use a property on your ModelView at the DataContext of the window.

The `Position` property can have these values

```csharp
public enum Position
{
  Left,
  Right,
  Top,
  Bottom
}
```

![](images/flyout-demo-dark.png)


# Themed Flyouts

Flyouts can have various themes, assignable through the `Theme` property, those are:

```csharp
public enum FlyoutTheme
{
  /// <summary>
  /// Adapts the Flyout's theme to the theme of its host window.
  /// </summary>
  Adapt,

  /// <summary>
  /// Adapts the Flyout's theme to the theme of its host window, but inverted.
  /// This theme can only be applied if the host window's theme abides the "Dark" and "Light" affix convention.
  /// (see <see cref="ThemeManager.GetInverseTheme"/> for more infos.
  /// </summary>
  Inverse,

  /// <summary>
  /// The dark theme. This is the default theme.
  /// </summary>
  Dark,
  Light,

  /// <summary>
  /// The flyouts theme will match the host window's accent color.
  /// </summary>
  Accent
}
```

![](images/flyout-demo-accent.png)

# Overlay Behavior

With the overlay behaviors you can set visibility of the `Icon`, `LeftWindowCommands`, `RightWindowCommands` and the `WindowButtonCommands` when a Flyout will be shown.

The `LeftWindowCommands`, `RightWindowCommands` and the `Icon` will never be shown over an opened Flyout.

```csharp
[Flags]
public enum WindowCommandsOverlayBehavior
{
  /// <summary>
  /// Doesn't overlay a hidden TitleBar.
  /// </summary>
  Never = 0,

  /// <summary>
  /// Overlays a hidden TitleBar.
  /// </summary>
  HiddenTitleBar = 1 << 0
}
```

The `WindowButtonCommands` (Min, Max/Restore and Close Buttons) can be handled by these values.

```csharp
[Flags]
public enum OverlayBehavior
{
  /// <summary>
  /// Doesn't overlay Flyouts nor a hidden TitleBar.
  /// </summary>
  Never = 0,

  /// <summary>
  /// Overlays opened <see cref="Flyout"/> controls.
  /// </summary>
  Flyouts = 1 << 0,

  /// <summary>
  /// Overlays a hidden TitleBar.
  /// </summary>
  HiddenTitleBar = 1 << 1,

  Always = ~(-1 << 2)
}
```
