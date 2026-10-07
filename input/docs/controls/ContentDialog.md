Title: ContentDialog
Description: A dialog with a title, content and up to three buttons, in the Metro, the Windows 10 and the WinUI look
---

`ContentDialog` is the dialog of WinUI for a [MetroWindow](metrowindow). It is a [ChildWindow](childwindow), so it opens in the container the dialogs use, over an overlay, and it brings what the WinUI dialog has: a title, any content, a primary, a secondary and a close button, and a result that says which of them closed it.

:::{.alert .alert-info}
**`ContentDialog` is new on `develop`.** It is not in 2.4.11.
:::

```xml
<mah:ContentDialog x:Key="SaveDialog"
                   Title="Save the answer?"
                   PrimaryButtonText="Save"
                   SecondaryButtonText="Don't save"
                   CloseButtonText="Cancel"
                   DefaultButton="Primary">
    <TextBlock Text="The answer is 42. Do you want to keep it for the next question?" TextWrapping="Wrap" />
</mah:ContentDialog>
```

```csharp
var dialog = (ContentDialog)this.FindResource("SaveDialog");
var result = await dialog.ShowAsync();
```

`ShowAsync()` opens the dialog in the active `MetroWindow` of the application, or in its main window, and `ShowAsync(window)` in the one you name. `window.ShowContentDialogAsync(dialog)` does the same as an extension method. A second dialog opens over the first one, where WinUI would throw.

## The buttons and the result

A button shows only when it has a text. `IsPrimaryButtonEnabled` and `IsSecondaryButtonEnabled` switch the two of them off, and each button has a command with a parameter and a style of its own.

| Closed by | Result |
|---|---|
| the primary button | `ContentDialogResult.Primary` |
| the secondary button | `ContentDialogResult.Secondary` |
| the close button, Escape, the close button in the title bar, `Hide()`, anything else | `ContentDialogResult.None` |

A click goes through the click event first, `PrimaryButtonClick` and so on, where `Cancel` keeps the dialog open. Then the command runs, if its CanExecute allows it, and the dialog closes. `Closing` comes last and carries the `Result`; setting `Cancel` there keeps the dialog open as well. `ClosingFinished` from ChildWindow takes the place of the `Closed` event of WinUI.

## Keyboard and focus

Escape is the close button, also when that button has no text and does not show: it raises `CloseButtonClick` and runs `CloseButtonCommand`. Enter clicks the `DefaultButton`, as long as it shows and is enabled, unless the focus is on a button or in a text box that takes Return. The default button also wears the accent style of its set, and it gets the focus when the content has nothing that can take it.

## The three looks

Each style set has its own dialog, and the dialog follows the set it is in.

| Set | Style | Look |
|---|---|---|
| Metro, `Styles/Controls.xaml` | `MahApps.Styles.ContentDialog` | the child window with its title bar and the dialog buttons in a row under the content |
| Windows 10, `Styles/Win10/Controls.xaml` | `MahApps.Styles.ContentDialog.Win10` | square, one surface, three buttons in thirds, after the Windows 10 SDK |
| WinUI, `Styles/WinUI/Controls.xaml` | `MahApps.Styles.ContentDialog.WinUI` | rounded, the content on a lighter layer above the buttons, after WinUI |

The colours of the two Microsoft looks are aliases you can override, `MahApps.Brushes.ContentDialog.Win10.Background`, `.Foreground`, `.Border` and `.Overlay`, and for WinUI `MahApps.Brushes.ContentDialog.WinUI.Background`, `.TopOverlay`, `.Foreground`, `.Border`, `.Separator` and `.Overlay`.

## Title bar, moving and the close button

Windows 10 and WinUI draw the dialog without a title bar, so their styles set `ShowTitleBar`, `AllowMove` and `ShowTitleBarCloseButton` to false. Switched on, they do what they say. The title moves into a bar, the dialog can be dragged by its title, with or without the bar, and a close button sits in the bar or in the corner of the content.

`CloseButtonText`, `CloseButtonCommand` and `CloseButtonStyle` belong to the close button in the row, as in WinUI. The close button in the title bar is styled by `TitleBarCloseButtonStyle`, and clicking it is one more way to the close button of the row. `TitleBarCloseButtonCommand` from ChildWindow is not used by the content dialog.

## Differences to WinUI

Dialogs stack instead of throwing. There is no `Placement`, no `FullSizeDesired`, no deferral, and no `Opened` or `Closed` event. The title bar is an option WinUI does not have.

## Related

[ChildWindow](childwindow) is what it builds on. [IDialogCoordinator](../dialogs/mvvm-dialog) opens one from a view model with `ShowContentDialogAsync`, and the [dialogs](../dialogs/custom-dialogs) share its container.
