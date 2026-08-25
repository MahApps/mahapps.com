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

`SelectedItemBinding` is used internally to hold the binding while it is attached; it is not something to set.

## Related

`SelectionMode` is still WPF's own — the helper synchronises whatever the list allows, so a `ListBox` left at `Single` will only ever put one item in your collection.
