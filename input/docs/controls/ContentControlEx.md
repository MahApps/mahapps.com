Title: ContentControlEx
Description: ContentControl with some special properties
---

The `ContentControlEx` represents a control with a single piece of content of any type.
It has the same functionality as the original `ContentControl` with some additonal properties.

## Additional Properties

| Property                 | Type              | Description                              |
|--------------------------|-------------------|------------------------------------------|
| `ContentCharacterCasing` | `CharacterCasing` | Gets or sets the character casing of the Content. (default `CharacterCasing.Normal`) |
| `RecognizesAccessKey`    | `bool`            | Determine if the inner `ContentPresenter` should use `AccessText` in its style. (default `false`) |

:::{.alert .alert-info}
***Note***  
The `ContentCharacterCasing` property works only for `string` content type.
:::

## CharacterCasing Enum

Specifies the case of characters in a `ContentControlEx` or `TextBox` control.

[Official documentation](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.charactercasing) 

### Fields

| Value  |   | Description                              |
|--------|---|------------------------------------------|
| Lower  | 2 | Converts all characters to lowercase.    |
| Normal | 0 | The case of characters is left unchanged. |
| Upper  | 1 | Converts all characters to uppercase.    |
