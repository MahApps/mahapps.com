Title: ChildWindow
Description: A window inside a MetroWindow, on the container the dialogs use
---

`ChildWindow` is a window that opens inside a [MetroWindow](metrowindow). It is a `ContentControl` with a title bar of its own, and it sits over an overlay in the container the [dialogs](../dialogs/custom-dialogs) use, where it can be moved and closed. It brings no buttons and no result type of its own, so the content and what the window hands back are up to you.

:::{.alert .alert-info}
**`ChildWindow` is new on `develop`.** In 2.4.11 it is the separate package MahApps.Metro.SimpleChildWindow. The last section says what changes when you move over.
:::

```xml
<mah:ChildWindow Title="A question" ChildWindowWidth="360">
    <StackPanel Margin="16">
        <TextBlock Text="What is the answer?" />
        <Button Content="42" Click="OnAnswer" />
    </StackPanel>
</mah:ChildWindow>
```

```csharp
var answer = await this.ShowChildWindowAsync<string>(new QuestionChildWindow());
```

`ShowChildWindowAsync` is an extension method on the window. It puts the child window into the dialog container, opens it and completes once it is closed and gone again.

## Closing it and the result

`Close(result)` closes the window, and `ShowChildWindowAsync<TResult>` returns that `result`. Closed without one, by Escape, by the close button or by a click on the overlay, the task returns the `CloseReason` instead if `TResult` can hold it, and the default otherwise. `ClosedBy` says which of these it was. The `Closing` event can cancel a close.

Escape closes the window unless `CloseByEscape` is off. A click on the overlay closes it only with `CloseOnOverlay`, and `IsAutoCloseEnabled` closes it on its own after `AutoCloseInterval` milliseconds.

## The overlay and the title bar

`ShowChildWindowAsync` takes an `OverlayFillBehavior`. `WindowContent`, the default, leaves the title bar of the window free, `FullWindow` covers it as well. `IsModal` set to false takes the veil away, while the overlay still catches the click that `CloseOnOverlay` asks for. Its brush is `MahApps.Brushes.ChildWindow.Overlay` and follows the theme.

`AllowMove` lets the title bar move the window around, as long as it is not stretched across. `ShowCloseButton` puts a close button into the title bar, and `ShowTitleBar` set to false leaves the bar out altogether.

## From a view model

`IDialogCoordinator.ShowChildWindowAsync` does the same for a view model that is registered with `DialogParticipation.Register`, the way the [MVVM dialogs](../dialogs/mvvm-dialog) do it.

## Coming from SimpleChildWindow

The control kept its names. Replace the `http://metro.mahapps.com/winfx/xaml/simplechildwindow` namespace with the controls one, `http://metro.mahapps.com/winfx/xaml/controls`, and drop the package. `TitleBarHeight` is a `double` now instead of an `int`, and `ShowChildWindowAsync<TResult>` returns `Task<TResult?>`. Setting `CornerRadius` from code reaches the window as well, where the package wrote the attached property of `Border` instead.

## Related

[MetroWindow](metrowindow) hosts it, and its `IsAnyDialogOpen` is true while a child window is open. The [dialogs](../dialogs/custom-dialogs) share the same container, and whichever opens last lies on top.
