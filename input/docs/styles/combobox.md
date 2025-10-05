Title: ComboBox
Description: The ComboBox styles
---

# Default Style

The standard `ComboBox` is automatically styled when you include the MahApps.Metro library. You can enhance it with helper properties from `TextBoxHelper`.

-----

## Watermark

You can add placeholder text, known as a watermark, which appears when the `ComboBox` has no selected item.

Add the following attribute to your `ComboBox`: `mah:TextBoxHelper.Watermark="Your placeholder text..."`

```xml
<ComboBox mah:TextBoxHelper.Watermark="Please select an item..."
          Width="200">
    <ComboBoxItem Content="Item 1" />
    <ComboBoxItem Content="Item 2" />
</ComboBox>
```

-----

## ClearText Button

A button to clear the selected item can be added to the `ComboBox`.

Set `mah:TextBoxHelper.ClearTextButton="True"` to make the button always visible.

```xml
<ComboBox mah:TextBoxHelper.ClearTextButton="True"
          mah:TextBoxHelper.Watermark="Please select an item..."
          Width="200">
    <ComboBoxItem Content="Item 1" />
    <ComboBoxItem Content="Item 2" />
</ComboBox>
```

Alternatively, you can bind its visibility to the `SelectedItem` property, so it only appears when an item is selected.

```xml
<ComboBox mah:TextBoxHelper.ClearTextButton="{Binding Path=SelectedItem, RelativeSource={RelativeSource Mode=Self}, Converter={x:Static mah:IsNotNullConverter.Instance}}"
          mah:TextBoxHelper.Watermark="Please select an item..."
          Width="200">
    <ComboBoxItem Content="Item 1" />
    <ComboBoxItem Content="Item 2" />
</ComboBox>
```

-----

## Floating Watermark

The watermark can be configured to float above the `ComboBox` like a label when an item is selected. This also works for editable `ComboBoxes`.

Add the following attribute to apply this style: `mah:TextBoxHelper.UseFloatingWatermark="True"`

```xml
<ComboBox mah:TextBoxHelper.UseFloatingWatermark="True"
          mah:TextBoxHelper.Watermark="Please select an item..."
          Width="200">
    <ComboBoxItem Content="Item 1" />
    <ComboBoxItem Content="Item 2" />
</ComboBox>
```

-----

## Custom Button Content

You can replace the default dropdown arrow with custom content, such as an icon. This is useful for creating search or filter-like inputs.

Use the `ButtonCommand`, `ButtonContent`, and `ButtonContentTemplate` properties from `TextBoxHelper` to customize the button.

```xml
<ComboBox Width="200"
          mah:TextBoxHelper.ButtonCommand="{Binding ControlButtonCommand, Mode=OneWay}"
          mah:TextBoxHelper.ButtonContent="M17.545,15.467l-3.779-3.779..."
          mah:TextBoxHelper.Watermark="Search here...">
    <mah:TextBoxHelper.ButtonContentTemplate>
        <DataTemplate>
            <mah:PathIcon Width="16"
                          Height="16"
                          Padding="3"
                          Data="{Binding Mode=OneWay}" />
        </DataTemplate>
    </mah:TextBoxHelper.ButtonContentTemplate>
    <ComboBoxItem Content="Item 1" />
    <ComboBoxItem Content="Item 2" />
</ComboBox>
```

-----

## Editable / Auto-Completion

Set `IsEditable="True"` to allow users to type directly into the `ComboBox`. When combined with an `ItemsSource`, this enables auto-completion.

For `ComboBoxes` with a large number of items, use the virtualized style for better performance.

Add the following to a `ComboBox` to apply this style: `Style="{DynamicResource MahApps.Styles.ComboBox.Virtualized}"`

```xaml
<ComboBox Width="200"
          mah:TextBoxHelper.Watermark="Auto completion"
          DisplayMemberPath="Title"
          IsEditable="True"
          ItemsSource="{Binding Albums}"
          Style="{DynamicResource MahApps.Styles.ComboBox.Virtualized}" />
```

-----

## Grouping

Items within the dropdown can be grouped under custom headers. This is achieved by setting the `<ComboBox.GroupStyle>`.

```xml
<ComboBox Width="200"
          mah:TextBoxHelper.Watermark="Auto completion with grouping"
          DisplayMemberPath="Title"
          IsEditable="True"
          ItemsSource="{Binding Albums}"
          Style="{DynamicResource MahApps.Styles.ComboBox.Virtualized}">
    <ComboBox.GroupStyle>
        <GroupStyle>
            <GroupStyle.HeaderTemplate>
                <DataTemplate>
                    <TextBlock Text="{Binding Path=Name.Name}" />
                </DataTemplate>
            </GroupStyle.HeaderTemplate>
        </GroupStyle>
    </ComboBox.GroupStyle>
</ComboBox>
```
