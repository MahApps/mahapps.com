Order: 60
Title: MVVM
Description: Use Dialogs together with MVVM
---

You can open dialogs from your ViewModel by using the `DialogCoordinator`.

There’s a couple of simple things you have to do:

1. Use the `DialogParticipation.Register` attached property in your Window to register your ViewModel with the dialog sub-system.

Assuming your View’s DataContext is set to the ViewModel from where you want to launch the dialog, add these attributes:

```xml
<mah:MetroWindow x:Class="SimpleApp.MainWindow"
                 xmlns:mah="clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro"
                 xmlns:Dialog="clr-namespace:MahApps.Metro.Controls.Dialogs;assembly=MahApps.Metro"
                 Dialog:DialogParticipation.Register="{Binding}">

</mah:MetroWindow>
```

2. Grab & use the `DialogCoordinator` to open dialogs.

You can instantiate `DialogCoordinator` directly, or, good citizens will probably want to inject in the interface `IDialogCoordinator` into their ViewModel.  This will play nicely with TDD, and is agnostic to whatever DI framework you may be using.

Without a DI framework you could just do something like this:

```csharp
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace SimpleApp
{
    public class MainWindow : MetroWindow
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

Opening up a dialog from your ViewModel is now easy, using the IDialogCoordinator instance.  To most methods the first parameter named “context” will typically be your ViewModel.  This is how the coordinator will match the ViewModel to the window (with what was registered in step 1) and display the dialog.  If you have multiple windows open, the dialog will display on the correct window.  Show your dialog from inside your ViewModel just like this:

```csharp
using MahApps.Metro.Controls.Dialogs;

namespace SimpleApp
{
    public class MainWindowViewModel
    {
        // The DialogCoordinator
        private IDialogCoordinator dialogCoordinator;

        public MainWindowViewModel(IDialogCoordinator instance)
        {                    
            this.dialogCoordinator = instance;
        }

        // Simple method which can be used on a Button
        public async void FooMessage()
        {
            await this.dialogCoordinator.ShowMessageAsync(this, "Message Title", "Bar");
        }
        
        public async void FooProgress()
        {
            // Show...
            ProgressDialogController controller = await this.dialogCoordinator.ShowProgressAsync(this, "Wait", "Waiting for the Answer to the Ultimate Question of Life, The Universe, and Everything...");

            controller.SetIndeterminate();
            
            // Do your work... 
            var result = await Task.Run(...);
             
            // Close...
            await controller.CloseAsync();
        }
    }
}
```
