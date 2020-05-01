Title: SplitButton
---

![]({{site.baseurl}}/images/splitButton_1.png)  

This controls is like button with dropdownlist, but content of the button changed when selected item changed.

### Events
SplitButton has SelectionChanged and Click events


### SelectedItem and SelectedIndex
This properties usage is just like in Listbox or ComboBox. When one of this properties changed, Content of the button will also changed.

### Binding to `ObservableCollection` or `Dictionary`
To correctly bind an `ObservableCollection` or a `Dictionary` to `SplitButton`, you need to use `ItemsSource` and `DisplayMemberPath`
For ex, `ItemsSource="{Binding Albums}" DisplayMemberPath="Title"`
In case you bind simple types like an enum or integer, you don`t need to use `DisplayMemberPath` property, only `ItemsSource`.

### Orientation
SplitButton supports orientation changing as you can see on the screenshot.

### Button commands
You can use button commands for SplitButton

### Icon property
You can add separate icon to SplitButton to display it independently from its content.
It could be bitmap image or vector icon.
`Icon="{DynamicResource appbar_alert}"`

### Example
```xml
<Controls:SplitButton 
    Icon="{DynamicResource appbar_alert}"
    HorizontalContentAlignment="Left"
    HorizontalAlignment="Center"
    VerticalContentAlignment="Center"
    Width="120"
    SelectedIndex="2"
    ItemsSource="{Binding Albums}"
    DisplayMemberPath="Title"
    VerticalAlignment="Center" />
```
								
result will be:  
![]({{site.baseurl}}/images/splitButton_2.png)
