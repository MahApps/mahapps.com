Title: GroupBox
Description: The GroupBox style
---

## Character Casing of `Header`

The default character casing for the `Header` property is `Upper`.

You can change this by setting the property `ContentCharacterCasing` of `ControlsHelper` to a different value. This is similar to the documentation of `ContentControlEx`.

### Example

```xml
<!-- xmlns:mah="http://metro.mahapps.com/winfx/xaml/controls" -->

<StackPanel>
    <GroupBox Header="My upper Header"
              mah:ControlsHelper.ContentCharacterCasing="Upper"
              Margin="5" />
    <GroupBox Header="My normal Header"
              mah:ControlsHelper.ContentCharacterCasing="Normal"
              Margin="5" />
    <GroupBox Header="My lower Header"
              mah:ControlsHelper.ContentCharacterCasing="Lower"
              Margin="5" />
</StackPanel>
```

![image of GroupBoxes with different character casing](images/groupbox_charactercasing.png)
