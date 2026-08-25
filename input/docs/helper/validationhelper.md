Order: 180
Title: ValidationHelper
Description: How the validation error popup behaves
---

Applies to any `UIElement`. Two properties, both about the popup MahApps shows next to a control whose binding failed validation.

| Property | Type | Default | |
| --- | --- | --- | --- |
| `CloseOnMouseLeftButtonDown` | `bool` | `false` | clicking anywhere dismisses the popup |
| `ShowValidationErrorOnMouseOver` | `bool` | `false` | show the error when the pointer is over the control rather than only while it has focus |

```xml
<TextBox Text="{Binding Age, ValidatesOnDataErrors=True}"
         mah:ValidationHelper.ShowValidationErrorOnMouseOver="True"
         mah:ValidationHelper.CloseOnMouseLeftButtonDown="True" />
```

By default the error popup appears while the control has keyboard focus. That works while the user is filling a form in, but it means an error on a field they have moved away from is invisible — `ShowValidationErrorOnMouseOver` gives them a way to read it again without tabbing back.

`CloseOnMouseLeftButtonDown` suits a long message that would otherwise cover the controls beneath it.

Both can be set once for the whole window rather than per control:

```xml
<Style TargetType="{x:Type TextBox}" BasedOn="{StaticResource {x:Type TextBox}}">
    <Setter Property="mah:ValidationHelper.ShowValidationErrorOnMouseOver" Value="True" />
</Style>
```

## Related

The template the popup uses is `MahApps.Templates.ValidationError`, which the input control styles set as `Validation.ErrorTemplate`. Replace that resource to change what an error looks like rather than how it behaves.
