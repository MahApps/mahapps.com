Order: 25
Title: ContextMenuHelper
Description: Items named the way WPF names its own text box menu
---

Applies to `ContextMenu`.

:::{.alert .alert-info}
`UseSystemCommandText` is new in `develop` and is not in 2.4.11.
:::

| Property | Type | Default | |
| --- | --- | --- | --- |
| `UseSystemCommandText` | `bool` | `false` | names the items after WPF's own text box menu instead of after their commands |

```xml
<ContextMenu mah:ContextMenuHelper.UseSystemCommandText="True">
    <MenuItem Command="ApplicationCommands.Cut" />
    <MenuItem Command="ApplicationCommands.Copy" />
    <MenuItem Command="ApplicationCommands.Paste" />
</ContextMenu>
```

## Why the property exists

A `MenuItem` with a command and no `Header` of its own is named after `RoutedUICommand.Text`. WPF reads that text once per process and then keeps it, so an application that sets a different `CultureInfo.CurrentUICulture` while it runs is left with a menu in the language it started in.

The menu WPF builds for a `TextBox` itself does not work that way. It has its own words for cut, copy and paste, and it writes them anew every time the user opens it. `UseSystemCommandText` asks the framework for those same words at the same moment, so a menu of your own says what the system menu beside it says — in every language WPF ships, and in the new one after the culture has changed.

Four commands are covered: `ApplicationCommands.Cut`, `Copy` and `Paste`, and `EditingCommands.IgnoreSpellingError` for the spell check menu. An item carrying any other command is left alone, as is one that was given a `Header`.

## What it does not change

The words come with their access keys, the same ones the system menu offers, so `t`, `C` and `P` work in the menu. WPF only underlines them once the user presses Alt, so nothing looks different until then.

The key combination beside the word stays as it is. WPF writes that into the `KeyGesture` when the command is created and never asks again, which means its own menu shows exactly the same one.

## You usually get this for free

`MahApps.TextBox.ContextMenu`, the menu the MahApps styles put on a `TextBox`, `PasswordBox`, `RichTextBox`, `DatePicker`, `NumericUpDown` and `HotKeyBox`, has the property set already, and so do the Visual Studio, Windows 10 and WinUI variants of it. You only need it on a menu you build yourself.

## Related

The menu itself is described under [TextBoxHelper](textboxhelper), whose `IsSpellCheckContextMenuEnabled` adds the spelling suggestions to it. [ControlsHelper](controlshelper) carries the corner radius the WinUI menu is drawn with.
