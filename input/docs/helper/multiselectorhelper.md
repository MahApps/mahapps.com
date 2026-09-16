Order: 90
Title: MultiSelectorHelper
Description: Bind the selected items of a list to a view model
---

Applies to `ListBox` and `MultiSelector`, which covers `ListView` and `DataGrid`. It exists for one reason: `Selector.SelectedItems` is read-only, so it cannot be bound.

| Property | Type | Default | |
| --- | --- | --- | --- |
| `SelectedItems` | `IList` | `null` | the collection the selection is kept in sync with |

```xml
<ListBox SelectionMode="Extended"
         ItemsSource="{Binding People}"
         mah:MultiSelectorHelper.SelectedItems="{Binding SelectedPeople}" />
```

The binding runs both ways: selecting items in the list adds them to your collection, and adding to your collection selects them in the list.

The view model side wants a collection that raises change notifications, so an `ObservableCollection<T>`:

```csharp
public ObservableCollection<Person> SelectedPeople { get; } = new();
```

Give the property a getter only and fill the existing collection rather than replacing it. Replacing the instance means the helper is still watching the old one.

## Listening for the change yourself

The helper works from the list's `SelectionChanged`, and so does anything you hook up to the same event. It keeps the collection in step before anybody else is told, so a handler of your own reads the collection as it is by then:

```csharp
private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
{
    // this.SelectedPeople already holds what was just picked
}
```

:::{.alert .alert-warning}
**In a released version it is the other way round whenever your handler was hooked up first**, which is what writing `SelectionChanged="..."` in XAML does: your handler runs before the helper, and the collection it reads is the one from before the click, one step behind. Fixed on `develop` by [#4458](https://github.com/MahApps/MahApps.Metro/issues/4458), where the helper listens on the class rather than on the list, which puts it ahead of anything an application hooks up.
:::

## A list inside a list

`SelectionChanged` is a routed event, so a list sitting in the item template of another one sends its selection up to the outer list as well.

:::{.alert .alert-warning}
**In a released version the outer helper takes that for its own**, and puts whatever the inner list holds into your collection. Where the two hold different things, that throws:

```
System.ArgumentException: The value "Dog" is not of type "Person"
and cannot be used in this generic collection.
```

Fixed on `develop` by [#4461](https://github.com/MahApps/MahApps.Metro/issues/4461): a change is only taken up by the list it came from. Until that ships, mark the event as handled in the inner list so it does not travel:

```csharp
private void OnInnerSelectionChanged(object sender, SelectionChangedEventArgs e)
{
    e.Handled = true;
}
```
:::

`SelectedItemBinding` is used internally to hold the binding while it is attached; it is not something to set.

## Related

`SelectionMode` is still WPF's own — the helper synchronises whatever the list allows, so a `ListBox` left at `Single` will only ever put one item in your collection.
