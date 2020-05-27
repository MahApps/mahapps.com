Order: 20
Title: Message Dialog
Description: Dialog to show simple messages to the user.
---

Simple message dialogs can be displayed with the `ShowMessageAsync` method. It is an extension method for `MetroWindow`, so call it from your window class.

```csharp
private async void OnButtonClick(object sender, RoutedEventArgs e)
{
  await this.ShowMessageAsync("This is the title", "Some message");
}
```
