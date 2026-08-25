Order: 60
Title: MVVM
Description: Use Dialogs together with MVVM
---

The dialogs are shown by a `MetroWindow`, but a view model has no business knowing about windows. `IDialogCoordinator` bridges that: the view model asks the coordinator for a dialog and passes itself along as the context, and the coordinator works out which window that context belongs to.

Two pieces are involved:

- **`IDialogCoordinator`** — the service your view model calls. Inject it, so the view model stays testable.
- **`DialogParticipation`** — an attached property that tells the coordinator which window a given context belongs to. Without it the coordinator cannot resolve a window and throws.

## 1. Register the view model

Set `DialogParticipation.Register` on the window and bind it to the view model. `{Binding}` binds the `DataContext` itself, which is usually what you want:

```xml
<mah:MetroWindow x:Class="SimpleApp.MainWindow"
                 xmlns:mah="http://metro.mahapps.com/winfx/xaml/controls"
                 mah:DialogParticipation.Register="{Binding}">

</mah:MetroWindow>
```

You do not have to register on the window itself. The coordinator resolves the window with `Window.GetWindow`, so registering on a `UserControl` inside a `MetroWindow` works too — useful when each view brings its own view model.

## 2. Give the view model a coordinator

`DialogCoordinator.Instance` is the ready-made implementation. Pass it in rather than reaching for it inside the view model, and tests can substitute their own:

```csharp
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace SimpleApp
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();

            // Set the DataContext for your View
            this.DataContext = new MainWindowViewModel(DialogCoordinator.Instance);
        }
    }
}
```

With a DI container you would register `IDialogCoordinator` against `DialogCoordinator.Instance` once and let the container do the rest.

## 3. Show dialogs

Every method takes the context as its first parameter — that is the object you registered in step 1, normally the view model itself. With several windows open this is what puts the dialog on the right one.

The methods below return `Task` so callers can await them, which suits the async command types most MVVM frameworks provide. Wiring one straight to a plain `ICommand`, which cannot await, would push you towards `async void` and its swallowed exceptions.

```csharp
using MahApps.Metro.Controls.Dialogs;

namespace SimpleApp
{
    public class MainWindowViewModel
    {
        private readonly IDialogCoordinator dialogCoordinator;

        public MainWindowViewModel(IDialogCoordinator dialogCoordinator)
        {
            this.dialogCoordinator = dialogCoordinator;
        }

        public async Task ShowMessage()
        {
            await this.dialogCoordinator.ShowMessageAsync(this, "Message Title", "Bar");
        }

        public async Task ShowProgress()
        {
            var controller = await this.dialogCoordinator.ShowProgressAsync(
                this,
                "Wait",
                "Waiting for the Answer to the Ultimate Question of Life, The Universe, and Everything...");

            controller.SetIndeterminate();

            // Do your work...
            var result = await Task.Run(...);

            await controller.CloseAsync();
        }
    }
}
```

## Unregister views that come and go

:::{.alert .alert-warning}
**A registration that is never removed keeps the view model and the window alive for the lifetime of the process.**
:::

`DialogParticipation` keeps its registrations in a static dictionary that maps the context to the element it was registered on. Both are held by a strong reference, and an entry is only removed when the attached property *changes*. Closing a window does not change it.

For a main window that lives as long as the application this makes no difference. For a dialog window or a view that is opened repeatedly it does: every instance leaves its view model behind, and memory grows with each round.

Clear the registration when the view goes away:

```csharp
public partial class MyDialogWindow : MetroWindow
{
    public MyDialogWindow()
    {
        this.InitializeComponent();
        this.Closed += (sender, e) => DialogParticipation.SetRegister(this, null);
    }
}
```

Setting the property to `null` removes the old value from the dictionary and puts nothing back, which is exactly what is wanted. For a `UserControl` that is swapped in and out, do the same from `Unloaded`.

## When it does not work

The coordinator throws instead of failing quietly, and the two messages are worth recognising:

| Message | Cause |
| --- | --- |
| `Context is not registered.` | Nothing called `DialogParticipation.Register` for this context — the attached property is missing, or the object passed as context is not the one that was registered. |
| `Context is not inside a MetroWindow.` | The element you registered sits in a plain `Window`. The dialogs are drawn by `MetroWindow`, so the containing window has to be one. |

The first one also appears when the `DataContext` is replaced after the binding was evaluated: the registration then still points at the previous object, while the view model calling the coordinator is the new one.
