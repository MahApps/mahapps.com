Order: 50
Title: Progress Dialog
Description: Dialog with a progress bar
---

There is a built-in dialog that displays a progress bar at the bottom of the dialog. Call it like this:

```csharp
var controller = await this.ShowProgressAsync("Please wait...", "Progress message");
```
    
This method returns a `ProgressDialogController` object that exposes the `SetProgress` method, use it to set the current progress.

A picture of the progress dialog in the demo:

![](images/progressdialog.png)
