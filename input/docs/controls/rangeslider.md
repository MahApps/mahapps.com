Title: RangeSlider
---

The `RangeSlider` control was taken from the [Avalon Controls Library](https://avaloncontrolslib.codeplex.com/) (MS-PL) [GitHub fork](https://github.com/jogibear9988/avaloncontrolslib). It lets you select a range of values with a slider and 2 range bars, rather than a single value.

![](images/rangeslider.png)  

# Properties

- Orientation: RangeSlider support vertical orientation
- `IsMoveToPointEnabled` feature work like for Slider
- SmallChange / LargeChange: when `IsMoveToPointEnabled = False` thumbs will move on the value you set in Small / LargeChange
- `Interval`: This property will set interval between changing values when using Small / Larnge change. 
- `IsSnapToTickEnabled`:  If set to true, thumbs will snap to ticks like in standard Slider.
- TickBars and Tickplacement: RangeSlider receive support for displaying / hiding ticks and change its ticks width according to minimum and maximum values changed
- `ExtendedMode`: If it set `ExtendedMode = False` you **cannot** do any manipulations **inside** range except moving thumbs closer/farther to each other with mouse, but if it enabled you **can** use MoveToPoint or Small/Large change **inside** range by clicking **Left mouse button + left or right control button** to move left thumb and **Right mouse button + left or right control button to move right thumb inside range**. If Extended mode = true you also can without problems move whole range by clicking leftmouse button
- `MoveWholeRange`: This property will let you move whole range when using MoveToPoint or Small/Large change (working also inside range)
- `MinRangeWidth`: Sets minimum width of **central** Thumb. It can be in range **from 0 to range_slider_width/2**.
- `AutoToolTipPlacement` will display tooltip, which will move with Thumb and display current value. Implemented for left/central/right thumbs.
- `AutotoolTipPrecision` set the number of digits, which will be shown after dot in autotooltip.


```xml
<mah:RangeSlider Width="200"
                 Margin="4"
                 mah:SliderHelper.ChangeValueBy="LargeChange"
                 mah:SliderHelper.EnableMouseWheel="MouseHover"
                 AutoToolTipPlacement="TopLeft"
                 LargeChange="10"
                 LowerValue="40"
                 Maximum="100"
                 Minimum="0"
                 Orientation="Horizontal"
                 SmallChange="1"
                 Style="{DynamicResource MahApps.Styles.RangeSlider.Win10}"
                 UpperValue="60">
  <mah:RangeSlider.AutoToolTipRangeValuesTemplate>
    <DataTemplate DataType="mah:RangeSliderAutoTooltipValues">
      <UniformGrid Columns="2" Rows="2">
        <TextBlock HorizontalAlignment="Right" Text="From:" />
        <TextBlock HorizontalAlignment="Right" Text="{Binding LowerValue, StringFormat='{}{0:N2}'}" />
        <TextBlock HorizontalAlignment="Right" Text="To:" />
        <TextBlock HorizontalAlignment="Right" Text="{Binding UpperValue, StringFormat='{}{0:N2}'}" />
      </UniformGrid>
    </DataTemplate>
  </mah:RangeSlider.AutoToolTipRangeValuesTemplate>
</mah:RangeSlider>
```

