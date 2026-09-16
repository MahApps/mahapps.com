Title: AutoSuggestBox
Description: A text box that offers suggestions while the user types
---

`AutoSuggestBox` is a text box with a list that comes up while the user types, the way a search field offers what you might have meant. It derives from `ComboBox`, so everything a `ComboBox` can do it can do too, and it wears the same clothes as every other MahApps text control.

:::{.alert .alert-info}
**`AutoSuggestBox` is on `develop` and ships with the next release.** It is not in 2.4.11.
:::

```xml
<mah:AutoSuggestBox x:Name="ArtistBox"
                    Width="300"
                    DisplayMemberPath="Name"
                    QuerySubmitted="OnQuerySubmitted"
                    TextChanged="OnTextChanged"
                    mah:TextBoxHelper.ClearTextButton="True"
                    mah:TextBoxHelper.Watermark="Artist" />
```

```csharp
private void OnTextChanged(object sender, RoutedEventArgs e)
{
    if (((AutoSuggestBoxTextChangedEventArgs)e).Reason != AutoSuggestionBoxTextChangeReason.UserInput)
    {
        return;
    }

    this.ArtistBox.ItemsSource = this.artists
                                     .Where(artist => artist.Name.StartsWith(this.ArtistBox.Text, StringComparison.CurrentCultureIgnoreCase))
                                     .Take(10)
                                     .ToList();
}

private void OnQuerySubmitted(object sender, RoutedEventArgs e)
{
    var args = (AutoSuggestBoxQuerySubmittedEventArgs)e;

    this.Search(args.ChosenSuggestion as Artist, args.QueryText);
}
```

## How it works

The box does not filter anything. It says that the text changed and who changed it, and the application answers by filling `ItemsSource` with whatever it thinks fits. That is the whole contract, and it is deliberate: matching, sorting, casing, accents and the question of what counts as a hit all belong where the data is, not in a set of filter modes here. It is also how WinUI's `AutoSuggestBox` works.

So there is no `FilterMode`, no `CustomFilter` and no `SearchMemberPath`. There is a `TextChanged` event and your own code.

## Properties

| Property | Type | Default | |
| --- | --- | --- | --- |
| `UpdateTextOnSelect` | `bool` | `True` | write the picked suggestion into the text box |

Everything else is inherited and works as it does on a `ComboBox`:

| Property | |
| --- | --- |
| `ItemsSource`, `ItemTemplate`, `ItemTemplateSelector` | the suggestions and how they are drawn |
| `DisplayMemberPath` | which member of a suggestion is shown, and written into the text |
| `TextSearch.TextPath` | the same, when the shown text and the written text differ |
| `Text` | what stands in the box, two-way by default |
| `SelectedItem` | the suggestion the user is on, `null` while they are only typing |
| `IsDropDownOpen` | whether the list is up; set it yourself to open or close it |
| `MaxDropDownHeight` | how tall the list may get |

Three properties are set for you and are not meant to be changed: `IsEditable` is `True`, `IsTextSearchEnabled` is `False`, so WPF does not complete the text behind your back while the user is still typing, and `StaysOpenOnEdit` is `True`, so the list stays up between keystrokes.

## Events

| Event | Arguments | Raised |
| --- | --- | --- |
| `TextChanged` | `AutoSuggestBoxTextChangedEventArgs` | after the text changed, whoever changed it |
| `SuggestionChosen` | `AutoSuggestBoxSuggestionChosenEventArgs` | when the user lands on a suggestion, by arrow key or by click |
| `QuerySubmitted` | `AutoSuggestBoxQuerySubmittedEventArgs` | when the user is done, by <kbd>Enter</kbd> or by clicking a suggestion |

All three are bubbling routed events, so they can be handled further up or attached from a style.

`AutoSuggestBoxTextChangedEventArgs.Reason` says who wrote the text:

| Reason | |
| --- | --- |
| `UserInput` | the user typed, and this is the one worth new suggestions |
| `ProgrammaticChange` | code or a binding set `Text` |
| `SuggestionChosen` | the user picked a suggestion and the box wrote it in |

:::{.alert .alert-warning}
**Check the reason before you go looking for suggestions.** Picking a suggestion writes it into the text box, which changes the text again. Answering that with a fresh search finds the item the box has just offered, replaces the list, and the selection and the text go with it. The box keeps the three cases apart so that a single `if` at the top of the handler is all it takes.
:::

`AutoSuggestBoxQuerySubmittedEventArgs` carries both halves of the answer:

| Member | Type | |
| --- | --- | --- |
| `QueryText` | `string?` | what stands in the box |
| `ChosenSuggestion` | `object?` | the suggestion the user picked, or `null` if they only typed |

`AutoSuggestBoxSuggestionChosenEventArgs.SelectedItem` is the suggestion the user landed on.

## When the list shows itself

The list comes up once there is text in the box and there is something to show, and it goes away when either of those stops being true. An empty list is never shown, because an empty popup is a sliver of border under a text box and nothing else.

Suggestions do not have to be there by the time the text changes. An application that goes off to a database or a service hands them over later, and the list comes up when they arrive:

```csharp
private async void OnTextChanged(object sender, RoutedEventArgs e)
{
    if (((AutoSuggestBoxTextChangedEventArgs)e).Reason != AutoSuggestionBoxTextChangeReason.UserInput)
    {
        return;
    }

    var query = this.Box.Text;
    var hits = await this.service.SuggestAsync(query).ConfigureAwait(true);

    // the user has typed on in the meantime, so this answer is stale
    if (this.Box.Text != query)
    {
        return;
    }

    this.Box.ItemsSource = hits;
}
```

There is no delay property. Where a request per keystroke is too much, hold the keystrokes back in the handler:

```csharp
private readonly DispatcherTimer typingPause = new() { Interval = TimeSpan.FromMilliseconds(300) };
```

Restart that timer on every `UserInput`, and do the asking in its `Tick`.

## Picking a suggestion

Clicking a suggestion, or arrowing onto one and pressing <kbd>Enter</kbd>, writes it into the text box and submits. What is written is the same text a `ComboBox` would write, so `DisplayMemberPath` and `TextSearch.TextPath` decide it for objects.

Set `UpdateTextOnSelect` to `False` where the suggestions are not text the user is composing but shortcuts to somewhere, a hit list the application opens on its own for instance. The typed text then stays as it was, and `QuerySubmitted` still hands you the item that was picked.

<kbd>Esc</kbd> closes the list and submits nothing.

## Styling

The control is styled as `MahApps.Styles.AutoSuggestBox`, which stands on `MahApps.Styles.ComboBox`, so the brushes, the fonts, the focus and mouse-over borders and the validation template are the ones every other MahApps input control uses. The template is its own: there is nothing to drop down by hand here, so there is no arrow, and the spot it would sit in takes one button instead.

The usual [TextBoxHelper](../helper/textboxhelper) properties are passed through to the inner text box:

| Property | |
| --- | --- |
| `Watermark`, `WatermarkAlignment`, `WatermarkTrimming` | the hint in an empty box |
| `UseFloatingWatermark` | let the hint rise above the text once there is text |
| `ClearTextButton` | put a clear button in the button spot |
| `ButtonCommand`, `ButtonContent`, `ButtonTemplate`, `ButtonWidth` | or put your own button there, a magnifier for instance |

`ComboBoxHelper.MaxLength` and `ComboBoxHelper.CharacterCasing` reach the text box as well.

With neither `ClearTextButton` nor a `ButtonCommand` set, the button spot collapses and the text takes the whole width.

## What a client is told

The box reports itself as an `AutoSuggestBox` and as a combo box control type, and hands over its text as a value, so a test or a screen reader gets the box rather than the text field inside it.

## Related

[MultiSelectionComboBox](MultiSelectionComboBox) where the user picks several items rather than typing free text, [TextBoxHelper](../helper/textboxhelper) for the watermark and the buttons, [validation](../styles/validation) for the error treatment.
