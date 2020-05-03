Title: ToggleButton
---

There are two kinds of styles for ToggleButtons available in MahApps.Metro.

The default style, available just by placing a `ToggleButton` control in XAML looks like the default MahApps.Metro button. 

```xml
  <Grid>
      <ToggleButton/>
  </Grid>
```

![](images/toggle-button-normal.png)

Another style, `MetroCircleToggleButtonStyle` is available by setting the ToggleButton's style to `MetroCircleToggleButtonStyle`. This style changes the button's background to `AccentColorBrush` when it is checked. To modify this behaviour, you will have to edit a copy of the control template using Blend.

```xml
  <ToggleButton Width="50"
                Height="50"
                Margin="0, 10, 0, 0"
                Style="{DynamicResource MetroCircleToggleButtonStyle}">
  </ToggleButton>
```

![](images/toggle-button-circle.png)

## Using Icons within a Circle ToggleButton
