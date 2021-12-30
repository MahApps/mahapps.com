Title: MetroHeader
Description: A HeaderedContentControl
---

The `MetroHeader` is a UI control that allows content to be displayed with a specified header (like the HeaderedContentControl). The `Header` property can be any object and you can use the `HeaderTemplate` to specify a custom look to the header. Content for the `MetroHeader` will streched horizontally and vertically.

:::{.alert .alert-info}
***Note***  
Setting the `BorderBrush` and `BorderThickness` properties will not have any effect on the `MetroHeader`. This is to maintain the same functionality as the ContentControl.
:::

## Syntax

```xml
<Page ...
     xmlns:mah="http://metro.mahapps.com/winfx/xaml/controls" />

<mah:MetroHeader>
    <!-- Header content or HeaderTemplate content -->
</mah:MetroHeader>
```

## Properties

| Property       | Type         | Description                                                                                                                                                                                 |
|----------------|--------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Header         | object       | Gets or sets the data used for the header of each control.                                                                                                                                  |
| HeaderTemplate | DataTemplate | Gets or sets the template used to display the content of the control's header.                                                                                                              |
| Orientation    | Orientation  | Gets or sets the Orientation to use for layout of the header. If set to Vertical the Header will be above the content. If set to Horizontal the Header will be to the left of the content.. |

## Example

The `Header` property can be set to a string or any other Xaml elements.

_Sample Code_

```xml
<mah:MetroHeader Header="This is the header!" />

<mah:MetroHeader>
    <mah:MetroHeader.Header>
        <Border Background="Gray">
            <TextBlock Text="This is the header!" FontSize="16" />
        </Border>
    </mah:MetroHeader.Header>
</mah:MetroHeader>

<mah:MetroHeader mah:HeaderedControlHelper.HeaderFontWeight="Bold"
                 mah:HeaderedControlHelper.HeaderForeground="{DynamicResource MahApps.Brushes.Accent}"
                 Header="TextBox sample">
    <TextBox Text="Proident ut reprehenderit excepteur esse non." />
</mah:MetroHeader>

```

![](images/metroheader.png)

For binding the `Header` to an object that is not a string, use the `HeaderTemplate` to control how the content is rendered. The default value for the `HeaderTemplate` will display the string representation of the `Header`.

_Sample Code_

```xml
<mah:MetroHeader Header="{Binding CustomObject}">
    <mah:MetroHeader.HeaderTemplate>
        <DataTemplate>
            <TextBlock Text="{Binding Title}" />
        </DataTemplate>
    </mah:MetroHeader.HeaderTemplate>
</mah:MetroHeader>
```
