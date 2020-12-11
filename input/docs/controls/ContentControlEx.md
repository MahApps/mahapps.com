Title: ContentControlEx
Description: ContentControl with some special properties
---

The `ContentControlEx` represents a control with a single piece of content of any type.
It has the same functionality as the original `ContentControl` with some additonal properties.

| Property                 | Type              | Description                              |
|--------------------------|-------------------|------------------------------------------|
| `ContentCharacterCasing` | `CharacterCasing` | Gets or sets the character casing of the Content. (default `CharacterCasing.Normal`) |
| `RecognizesAccessKey`    | `bool`            | Determine if the inner `ContentPresenter` should use `AccessText` in its style. (default `false`) |

:::{.alert .alert-info}
***Note***  
The `ContentCharacterCasing` property works only for `string` content type.
:::
