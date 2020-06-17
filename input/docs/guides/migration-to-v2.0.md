Order: 20
Title: Migration to v2.0
Description: Migration to MahApps.Metro v2.0
---

This guide assumes the migration from version 1.6.5 to 2.0.

## Main Breaking Changes

- All marked obsolete code were removed
- Drop support for .Net 4.0
- Drop support for .Net 4.5 and move up to 4.5.2
- Support .Net Core 3.0 and 3.1
- Assemblies are strong named again, but AssemblyVersion is now fixed for every major release. This means that, for example, for version 2.1 the AssemblyVersion will still be 2.0. Other versions like AssemblyFileVersion etc. won't be fixed.
- The theming got rewritten, this means that there is no longer a separation between AppTheme and Accent. These got replaced by Theme everywhere.
- Use new updated `GlowWindowBehavior` and `BorderlessWindowBehavior` from [ControlzEx](https://github.com/ControlzEx/ControlzEx)
- Instead of depending on `System.Windows.Interactivity` we now depend on the open source version [Microsoft.Xaml.Behaviors.Wpf](https://github.com/microsoft/XamlBehaviorsWpf)
- Use a ToolBar overflow mechanism for WindowCommands
- Use `TabControlEx` from [ControlzEx](https://github.com/ControlzEx/ControlzEx) for `BaseMetroTabControl`

## Theming

The theming got rewritten, this means that there is no longer a separation between `AppTheme` and `Accent`. These got replaced by Theme everywhere.
Instead of something like `pack://application:,,,/MahApps.Metro;component/Styles/Accents/Blue.xaml` and `pack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseLight.xaml` you now have to use `pack://application:,,,/MahApps.Metro;component/Styles/Themes/Light.Blue.xaml`.

### Old

```xml
<Application x:Class="WpfApplication.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             StartupUri="MainWindow.xaml">
  <Application.Resources>
    <ResourceDictionary>
      <ResourceDictionary.MergedDictionaries>
        <ResourceDictionary Source="pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml" />
        <ResourceDictionary Source="pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml" />
        <ResourceDictionary Source="pack://application:,,,/MahApps.Metro;component/Styles/Colors.xaml" />
        <ResourceDictionary Source="pack://application:,,,/MahApps.Metro;component/Styles/Accents/Blue.xaml" />
        <ResourceDictionary Source="pack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseLight.xaml" />
      </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
  </Application.Resources>
</Application>
```

### New

```xml
<Application x:Class="WpfApplication.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             StartupUri="MainWindow.xaml">
  <Application.Resources>
    <ResourceDictionary>
      <ResourceDictionary.MergedDictionaries>
        <ResourceDictionary Source="pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml" />
        <ResourceDictionary Source="pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml" />
        <ResourceDictionary Source="pack://application:,,,/MahApps.Metro;component/Styles/Themes/Light.Blue.xaml" />
      </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
  </Application.Resources>
</Application>
```

## Resource Keys

The keys of various resources got rewritten. This makes it easier to find and use these resources in your applications.

Final naming convention is:

`MahApps.`

+ and the resource type which is one of `Colors.`, `Brushes.`, `Sizes.`, `Font.`, `Fonts.`, `Storyboard.`, `Styles.`, `Templates.`
+ and the target control for Templates and Styles (e.g. `Button.`, `Slider.`, `Window.`, ...)
+ an optional specifier which gives more context (e.g. `Slider` for `MahApps.Styles.Thumb` which helps understanding that this thumb style is for the context of a slider)
+ an optional category (e.g. `Metro`, `Flat`, `Square`, `Win10`, ...)

### Renamed and new Colors

| Old Key                     | New Key                                  |
|-----------------------------|------------------------------------------|
| HighlightColor              | MahApps.Colors.Highlight                 |
| AccentBaseColor             | MahApps.Colors.AccentBase                |
| AccentColor                 | MahApps.Colors.Accent                    |
| AccentColor2                | MahApps.Colors.Accent2                   |
| AccentColor3                | MahApps.Colors.Accent3                   |
| AccentColor4                | MahApps.Colors.Accent4                   |
| BlackColor                  | MahApps.Colors.ThemeForeground           |
| WhiteColor                  | MahApps.Colors.ThemeBackground           |
| IdealForegroundColor        | MahApps.Colors.IdealForeground           |
| Gray1                       | MahApps.Colors.Gray1                     |
| Gray2                       | MahApps.Colors.Gray2                     |
| Gray3                       | MahApps.Colors.Gray3                     |
| Gray4                       | MahApps.Colors.Gray4                     |
| Gray5                       | MahApps.Colors.Gray5                     |
| Gray6                       | MahApps.Colors.Gray6                     |
| Gray7                       | MahApps.Colors.Gray7                     |
| Gray8                       | MahApps.Colors.Gray8                     |
| Gray9                       | MahApps.Colors.Gray9                     |
| Gray10                      | MahApps.Colors.Gray10                    |
| GrayNormal                  | MahApps.Colors.Gray                      |
| GrayHover                   | MahApps.Colors.Gray.MouseOver            |
|                             | MahApps.Colors.Gray.SemiTransparent      |
| FlyoutColor                 | MahApps.Colors.Flyout                    |
| ProgressIndeterminateColor1 | MahApps.Colors.ProgressIndeterminate1    |
| ProgressIndeterminateColor2 | MahApps.Colors.ProgressIndeterminate2    |
| ProgressIndeterminateColor3 | MahApps.Colors.ProgressIndeterminate3    |
| ProgressIndeterminateColor4 | MahApps.Colors.ProgressIndeterminate4    |
| MenuShadowColor             | MahApps.Colors.MenuShadow                |
|                             | MahApps.Colors.SystemAccent              |
|                             | MahApps.Colors.SystemAltHigh             |
|                             | MahApps.Colors.SystemAltLow              |
|                             | MahApps.Colors.SystemAltMedium           |
|                             | MahApps.Colors.SystemAltMediumHigh       |
|                             | MahApps.Colors.SystemAltMediumLow        |
|                             | MahApps.Colors.SystemBaseHigh            |
|                             | MahApps.Colors.SystemBaseLow             |
|                             | MahApps.Colors.SystemBaseMedium          |
|                             | MahApps.Colors.SystemBaseMediumHigh      |
|                             | MahApps.Colors.SystemBaseMediumLow       |
|                             | MahApps.Colors.SystemChromeAltLow        |
|                             | MahApps.Colors.SystemChromeBlackHigh     |
|                             | MahApps.Colors.SystemChromeBlackLow      |
|                             | MahApps.Colors.SystemChromeBlackMediumLow |
|                             | MahApps.Colors.SystemChromeBlackMedium   |
|                             | MahApps.Colors.SystemChromeDisabledHigh  |
|                             | MahApps.Colors.SystemChromeDisabledLow   |
|                             | MahApps.Colors.SystemChromeHigh          |
|                             | MahApps.Colors.SystemChromeLow           |
|                             | MahApps.Colors.SystemChromeMedium        |
|                             | MahApps.Colors.SystemChromeMediumLow     |
|                             | MahApps.Colors.SystemChromeWhite         |
|                             | MahApps.Colors.SystemChromeGray          |
|                             | MahApps.Colors.SystemListLow             |
|                             | MahApps.Colors.SystemListMedium          |
|                             | MahApps.Colors.SystemErrorText           |

### Renamed and new Brushes

| Old Key                                  | New Key                                  |
|------------------------------------------|------------------------------------------|
| WhiteBrush                               | MahApps.Brushes.ThemeBackground          |
| BlackBrush                               | MahApps.Brushes.ThemeForeground          |
| TextBrush                                | MahApps.Brushes.Text                     |
| IdealForegroundColorBrush                | MahApps.Brushes.IdealForeground          |
| IdealForegroundDisabledBrush             | MahApps.Brushes.IdealForegroundDisabled  |
| AccentSelectedColorBrush                 | MahApps.Brushes.Selected.Foreground      |
| WindowTitleColorBrush                    | MahApps.Brushes.WindowTitle              |
| NonActiveWindowTitleColorBrush           | MahApps.Brushes.WindowTitle.NonActive    |
| NonActiveBorderColorBrush                | MahApps.Brushes.Border.NonActive         |
| HighlightBrush                           | MahApps.Brushes.Highlight                |
| TransparentWhiteBrush                    | MahApps.Brushes.Transparent              |
| SemiTransparentWhiteBrush                | MahApps.Brushes.SemiTransparent          |
| AccentBaseColorBrush                     | MahApps.Brushes.AccentBase               |
| AccentColorBrush                         | MahApps.Brushes.Accent                   |
| AccentColorBrush2                        | MahApps.Brushes.Accent2                  |
| AccentColorBrush3                        | MahApps.Brushes.Accent3                  |
| AccentColorBrush4                        | MahApps.Brushes.Accent4                  |
| GrayBrush1                               | MahApps.Brushes.Gray1                    |
| GrayBrush2                               | MahApps.Brushes.Gray2                    |
| GrayBrush3                               | MahApps.Brushes.Gray3                    |
| GrayBrush4                               | MahApps.Brushes.Gray4                    |
| GrayBrush5                               | MahApps.Brushes.Gray5                    |
| GrayBrush6                               | MahApps.Brushes.Gray6                    |
| GrayBrush7                               | MahApps.Brushes.Gray7                    |
| GrayBrush8                               | MahApps.Brushes.Gray8                    |
| GrayBrush9                               | MahApps.Brushes.Gray9                    |
| GrayBrush10                              | MahApps.Brushes.Gray10                   |
| GrayNormalBrush                          | MahApps.Brushes.Gray                     |
| GrayHoverBrush                           | MahApps.Brushes.Gray.MouseOver           |
| SemiTransparentGreyBrush                 | MahApps.Brushes.Gray.SemiTransparent     |
| TextBoxBorderBrush                       | MahApps.Brushes.TextBox.Border           |
| TextBoxFocusBorderBrush                  | MahApps.Brushes.TextBox.Border.Focus     |
| TextBoxMouseOverBorderBrush              | MahApps.Brushes.TextBox.Border.MouseOver |
| ControlBackgroundBrush                   | MahApps.Brushes.Control.Background       |
| ControlBorderBrush                       | MahApps.Brushes.Control.Border           |
| ControlsDisabledBrush                    | MahApps.Brushes.Control.Disabled         |
| ControlsValidationBrush                  | MahApps.Brushes.Control.Validation       |
|                                          | MahApps.Brushes.Button.Border            |
|                                          | MahApps.Brushes.Button.Border.Focus      |
| ButtonMouseOverBorderBrush               | MahApps.Brushes.Button.Border.MouseOver  |
| ComboBoxMouseOverBorderBrush             | MahApps.Brushes.ComboBox.Border.MouseOver |
|                                          | MahApps.Brushes.ComboBox.Border.Focus    |
| ComboBoxPopupBorderBrush                 | MahApps.Brushes.ComboBox.PopupBorder     |
| CheckBoxBrush                            | MahApps.Brushes.CheckBox                 |
| CheckBoxMouseOverBrush                   | MahApps.Brushes.CheckBox.MouseOver       |
| ThumbBrush                               | MahApps.Brushes.Thumb                    |
| ProgressBrush                            | MahApps.Brushes.Progress                 |
| SliderValueDisabled                      | MahApps.Brushes.SliderValue.Disabled     |
| SliderTrackDisabled                      | MahApps.Brushes.SliderTrack.Disabled     |
| SliderThumbDisabled                      | MahApps.Brushes.SliderThumb.Disabled     |
| SliderTrackHover                         | MahApps.Brushes.SliderTrack.Hover        |
| SliderTrackNormal                        | MahApps.Brushes.SliderTrack.Normal       |
| FlyoutBackgroundBrush                    | MahApps.Brushes.Flyout.Background        |
| FlyoutForegroundBrush                    | MahApps.Brushes.Flyout.Foreground        |
|                                          | MahApps.Brushes.Window.FlyoutOverlay     |
| WindowBackgroundBrush                    | MahApps.Brushes.Window.Background        |
| SeperatorBrush                           | MahApps.Brushes.Separator                |
| FlatButtonBackgroundBrush                | MahApps.Brushes.Button.Flat.Background   |
| FlatButtonForegroundBrush                | MahApps.Brushes.Button.Flat.Foreground   |
|                                          | MahApps.Brushes.Button.Flat.Background.MouseOver |
| FlatButtonPressedBackgroundBrush         | MahApps.Brushes.Button.Flat.Background.Pressed |
| FlatButtonPressedForegroundBrush         | MahApps.Brushes.Button.Flat.Foreground.Pressed |
| CleanWindowCloseButtonBackgroundBrush    | MahApps.Brushes.Button.CleanWindow.Close.Background.MouseOver |
|                                          | MahApps.Brushes.Button.CleanWindow.Close.Foreground.MouseOver |
| CleanWindowCloseButtonPressedBackgroundBrush | MahApps.Brushes.Button.CleanWindow.Close.Background.Pressed |
|                                          | MahApps.Brushes.Button.Square.Background.MouseOver |
|                                          | MahApps.Brushes.Button.Square.Foreground.MouseOver |
|                                          | MahApps.Brushes.Button.AccentedSquare.Background.MouseOver |
|                                          | MahApps.Brushes.Button.AccentedSquare.Foreground.MouseOver |
| ValidationBrush1                         | MahApps.Brushes.Validation1              |
| ValidationBrush2                         | MahApps.Brushes.Validation2              |
| ValidationBrush3                         | MahApps.Brushes.Validation3              |
| ValidationBrush4                         | MahApps.Brushes.Validation4              |
| ValidationBrush5                         | MahApps.Brushes.Validation5              |
| ValidationSummaryColor1                  | MahApps.Brushes.ValidationSummary1       |
| ValidationSummaryColor2                  | MahApps.Brushes.ValidationSummary2       |
| ValidationSummaryColor3                  | MahApps.Brushes.ValidationSummary3       |
| ValidationSummaryColor4                  | MahApps.Brushes.ValidationSummary4       |
| ValidationSummaryColor5                  | MahApps.Brushes.ValidationSummary5       |
| ValidationSummaryFillColor1              | MahApps.Brushes.ValidationSummaryFill1   |
| ValidationSummaryFillColor2              | MahApps.Brushes.ValidationSummaryFill2   |
| ValidationTextBrush                      | MahApps.Brushes.Text.Validation          |
| MenuBackgroundBrush                      | MahApps.Brushes.Menu.Background          |
| ContextMenuBackgroundBrush               | MahApps.Brushes.ContextMenu.Background   |
| SubMenuBackgroundBrush                   | MahApps.Brushes.SubMenu.Background       |
| MenuItemBackgroundBrush                  | MahApps.Brushes.MenuItem.Background      |
| ContextMenuBorderBrush                   | MahApps.Brushes.ContextMenu.Border       |
| SubMenuBorderBrush                       | MahApps.Brushes.SubMenu.Border           |
| MenuItemSelectionFill                    | MahApps.Brushes.MenuItem.SelectionFill   |
| MenuItemSelectionStroke                  | MahApps.Brushes.MenuItem.SelectionStroke |
| TopMenuItemPressedFill                   | MahApps.Brushes.TopMenuItem.PressedFill  |
| TopMenuItemPressedStroke                 | MahApps.Brushes.TopMenuItem.PressedStroke |
| TopMenuItemSelectionStroke               | MahApps.Brushes.TopMenuItem.SelectionStroke |
| DisabledMenuItemForeground               | MahApps.Brushes.MenuItem.Foreground.Disabled |
| DisabledMenuItemGlyphPanel               | MahApps.Brushes.MenuItem.GlyphPanel.Disabled |
| CheckmarkFill                            | MahApps.Brushes.CheckmarkFill            |
| RightArrowFill                           | MahApps.Brushes.RightArrowFill           |
|                                          | MahApps.Brushes.WindowButtonCommands.Background.MouseOver |
| MetroDataGrid.HighlightBrush             | MahApps.Brushes.DataGrid.Selection.Background |
|                                          | MahApps.Brushes.DataGrid.Selection.BorderBrush |
| MetroDataGrid.DisabledHighlightBrush     | MahApps.Brushes.DataGrid.Selection.Background.Disabled |
|                                          | MahApps.Brushes.DataGrid.Selection.BorderBrush.Disabled |
| MetroDataGrid.HighlightTextBrush         | MahApps.Brushes.DataGrid.Selection.Text  |
|                                          | MahApps.Brushes.DataGrid.Selection.Text.Disabled |
| MetroDataGrid.MouseOverHighlightBrush    | MahApps.Brushes.DataGrid.Selection.Background.MouseOver |
|                                          | MahApps.Brushes.DataGrid.Selection.BorderBrush.MouseOver |
| MetroDataGrid.FocusBorderBrush           | MahApps.Brushes.DataGrid.Selection.BorderBrush.Focus |
| MetroDataGrid.InactiveSelectionHighlightBrush | MahApps.Brushes.DataGrid.Selection.Background.Inactive |
|                                          | MahApps.Brushes.DataGrid.Selection.BorderBrush.Inactive |
| MetroDataGrid.InactiveSelectionHighlightTextBrush | MahApps.Brushes.DataGrid.Selection.Text.Inactive |
|                                          | MahApps.Brushes.DataGrid.Selection.Text.MouseOver |
|                                          | MahApps.Brushes.Badged.Background        |
| MahApps.Metro.Brushes.Badged.DisabledBackgroundBrush | MahApps.Brushes.Badged.Background.Disabled |
|                                          | MahApps.Brushes.Badged.Foreground        |
|                                          | MahApps.Brushes.Badged.Foreground.Disabled |
| MahApps.Metro.HamburgerMenu.PaneBackgroundBrush | MahApps.HamburgerMenu.Pane.Background    |
| MahApps.Metro.HamburgerMenu.PaneForegroundBrush | MahApps.HamburgerMenu.Pane.Foreground    |
|                                          | MahApps.Brushes.SystemControlBackgroundAccent |
|                                          | MahApps.Brushes.SystemControlBackgroundAltHigh |
|                                          | MahApps.Brushes.SystemControlBackgroundAltMedium |
|                                          | MahApps.Brushes.SystemControlBackgroundAltMediumHigh |
|                                          | MahApps.Brushes.SystemControlBackgroundAltMediumLow |
|                                          | MahApps.Brushes.SystemControlBackgroundBaseHigh |
|                                          | MahApps.Brushes.SystemControlBackgroundBaseLow |
|                                          | MahApps.Brushes.SystemControlBackgroundBaseMedium |
|                                          | MahApps.Brushes.SystemControlBackgroundBaseMediumHigh |
|                                          | MahApps.Brushes.SystemControlBackgroundBaseMediumLow |
|                                          | MahApps.Brushes.SystemControlBackgroundChromeBlackHigh |
|                                          | MahApps.Brushes.SystemControlBackgroundChromeBlackLow |
|                                          | MahApps.Brushes.SystemControlBackgroundChromeBlackMedium |
|                                          | MahApps.Brushes.SystemControlBackgroundChromeBlackMediumLow |
|                                          | MahApps.Brushes.SystemControlBackgroundChromeMedium |
|                                          | MahApps.Brushes.SystemControlBackgroundChromeMediumLow |
|                                          | MahApps.Brushes.SystemControlBackgroundChromeWhite |
|                                          | MahApps.Brushes.SystemControlBackgroundListLow |
|                                          | MahApps.Brushes.SystemControlBackgroundListMedium |
|                                          | MahApps.Brushes.SystemControlDisabledAccent |
|                                          | MahApps.Brushes.SystemControlDisabledBaseHigh |
|                                          | MahApps.Brushes.SystemControlDisabledBaseLow |
|                                          | MahApps.Brushes.SystemControlDisabledBaseMediumLow |
|                                          | MahApps.Brushes.SystemControlDisabledChromeDisabledHigh |
|                                          | MahApps.Brushes.SystemControlDisabledChromeDisabledLow |
|                                          | MahApps.Brushes.SystemControlDisabledChromeHigh |
|                                          | MahApps.Brushes.SystemControlDisabledChromeMediumLow |
|                                          | MahApps.Brushes.SystemControlDisabledListMedium |
|                                          | MahApps.Brushes.SystemControlDisabledTransparent |
|                                          | MahApps.Brushes.SystemControlErrorTextForeground |
|                                          | MahApps.Brushes.SystemControlFocusVisualPrimary |
|                                          | MahApps.Brushes.SystemControlFocusVisualSecondary |
|                                          | MahApps.Brushes.SystemControlForegroundAccent |
|                                          | MahApps.Brushes.SystemControlForegroundAltHigh |
|                                          | MahApps.Brushes.SystemControlForegroundAltMediumHigh |
|                                          | MahApps.Brushes.SystemControlForegroundBaseHigh |
|                                          | MahApps.Brushes.SystemControlForegroundBaseLow |
|                                          | MahApps.Brushes.SystemControlForegroundBaseMedium |
|                                          | MahApps.Brushes.SystemControlForegroundBaseMediumHigh |
|                                          | MahApps.Brushes.SystemControlForegroundBaseMediumLow |
|                                          | MahApps.Brushes.SystemControlForegroundChromeBlackHigh |
|                                          | MahApps.Brushes.SystemControlForegroundChromeBlackMedium |
|                                          | MahApps.Brushes.SystemControlForegroundChromeBlackMediumLow |
|                                          | MahApps.Brushes.SystemControlForegroundChromeDisabledLow |
|                                          | MahApps.Brushes.SystemControlForegroundChromeGray |
|                                          | MahApps.Brushes.SystemControlForegroundChromeHigh |
|                                          | MahApps.Brushes.SystemControlForegroundChromeMedium |
|                                          | MahApps.Brushes.SystemControlForegroundChromeWhite |
|                                          | MahApps.Brushes.SystemControlForegroundListLow |
|                                          | MahApps.Brushes.SystemControlForegroundListMedium |
|                                          | MahApps.Brushes.SystemControlForegroundTransparent |
|                                          | MahApps.Brushes.SystemControlHighlightAccent |
|                                          | MahApps.Brushes.SystemControlHighlightAltAccent |
|                                          | MahApps.Brushes.SystemControlHighlightAltAltHigh |
|                                          | MahApps.Brushes.SystemControlHighlightAltAltMediumHigh |
|                                          | MahApps.Brushes.SystemControlHighlightAltBaseHigh |
|                                          | MahApps.Brushes.SystemControlHighlightAltBaseLow |
|                                          | MahApps.Brushes.SystemControlHighlightAltBaseMedium |
|                                          | MahApps.Brushes.SystemControlHighlightAltBaseMediumHigh |
|                                          | MahApps.Brushes.SystemControlHighlightAltBaseMediumLow |
|                                          | MahApps.Brushes.SystemControlHighlightAltChromeWhite |
|                                          | MahApps.Brushes.SystemControlHighlightAltListAccentHigh |
|                                          | MahApps.Brushes.SystemControlHighlightAltListAccentLow |
|                                          | MahApps.Brushes.SystemControlHighlightAltListAccentMedium |
|                                          | MahApps.Brushes.SystemControlHighlightAltTransparent |
|                                          | MahApps.Brushes.SystemControlHighlightBaseHigh |
|                                          | MahApps.Brushes.SystemControlHighlightBaseLow |
|                                          | MahApps.Brushes.SystemControlHighlightBaseMedium |
|                                          | MahApps.Brushes.SystemControlHighlightBaseMediumHigh |
|                                          | MahApps.Brushes.SystemControlHighlightBaseMediumLow |
|                                          | MahApps.Brushes.SystemControlHighlightChromeAltLow |
|                                          | MahApps.Brushes.SystemControlHighlightChromeHigh |
|                                          | MahApps.Brushes.SystemControlHighlightChromeWhite |
|                                          | MahApps.Brushes.SystemControlHighlightListAccentHigh |
|                                          | MahApps.Brushes.SystemControlHighlightListAccentLow |
|                                          | MahApps.Brushes.SystemControlHighlightListAccentMedium |
|                                          | MahApps.Brushes.SystemControlHighlightListLow |
|                                          | MahApps.Brushes.SystemControlHighlightListMedium |
|                                          | MahApps.Brushes.SystemControlHighlightTransparent |
|                                          | MahApps.Brushes.SystemControlHyperlinkBaseHigh |
|                                          | MahApps.Brushes.SystemControlHyperlinkBaseMedium |
|                                          | MahApps.Brushes.SystemControlHyperlinkBaseMediumHigh |
|                                          | MahApps.Brushes.SystemControlHyperlinkText |
|                                          | MahApps.Brushes.SystemControlPageBackgroundAltHigh |
|                                          | MahApps.Brushes.SystemControlPageBackgroundAltMedium |
|                                          | MahApps.Brushes.SystemControlPageBackgroundBaseLow |
|                                          | MahApps.Brushes.SystemControlPageBackgroundBaseMedium |
|                                          | MahApps.Brushes.SystemControlPageBackgroundChromeLow |
|                                          | MahApps.Brushes.SystemControlPageBackgroundChromeMediumLow |
|                                          | MahApps.Brushes.SystemControlPageBackgroundListLow |
|                                          | MahApps.Brushes.SystemControlPageBackgroundMediumAltMedium |
|                                          | MahApps.Brushes.SystemControlPageBackgroundTransparent |
|                                          | MahApps.Brushes.SystemControlPageTextBaseHigh |
|                                          | MahApps.Brushes.SystemControlPageTextBaseMedium |
|                                          | MahApps.Brushes.SystemControlPageTextChromeBlackMediumLow |
|                                          | MahApps.Brushes.SystemControlRevealFocusVisual |
|                                          | MahApps.Brushes.SystemControlTransientBorder |
|                                          | MahApps.Brushes.SystemControlTransparent |
|                                          | MahApps.Brushes.SystemControlDescriptionTextForeground |
|                                          | MahApps.Brushes.CheckBox.ForegroundUnchecked |
|                                          | MahApps.Brushes.CheckBox.ForegroundUncheckedMouseOver |
|                                          | MahApps.Brushes.CheckBox.ForegroundUncheckedPressed |
|                                          | MahApps.Brushes.CheckBox.ForegroundUncheckedDisabled |
|                                          | MahApps.Brushes.CheckBox.ForegroundChecked |
|                                          | MahApps.Brushes.CheckBox.ForegroundCheckedMouseOver |
|                                          | MahApps.Brushes.CheckBox.ForegroundCheckedPressed |
|                                          | MahApps.Brushes.CheckBox.ForegroundCheckedDisabled |
|                                          | MahApps.Brushes.CheckBox.ForegroundIndeterminate |
|                                          | MahApps.Brushes.CheckBox.ForegroundIndeterminateMouseOver |
|                                          | MahApps.Brushes.CheckBox.ForegroundIndeterminatePressed |
|                                          | MahApps.Brushes.CheckBox.ForegroundIndeterminateDisabled |
|                                          | MahApps.Brushes.CheckBox.BackgroundUnchecked |
|                                          | MahApps.Brushes.CheckBox.BackgroundUncheckedMouseOver |
|                                          | MahApps.Brushes.CheckBox.BackgroundUncheckedPressed |
|                                          | MahApps.Brushes.CheckBox.BackgroundUncheckedDisabled |
|                                          | MahApps.Brushes.CheckBox.BackgroundChecked |
|                                          | MahApps.Brushes.CheckBox.BackgroundCheckedMouseOver |
|                                          | MahApps.Brushes.CheckBox.BackgroundCheckedPressed |
|                                          | MahApps.Brushes.CheckBox.BackgroundCheckedDisabled |
|                                          | MahApps.Brushes.CheckBox.BackgroundIndeterminate |
|                                          | MahApps.Brushes.CheckBox.BackgroundIndeterminateMouseOver |
|                                          | MahApps.Brushes.CheckBox.BackgroundIndeterminatePressed |
|                                          | MahApps.Brushes.CheckBox.BackgroundIndeterminateDisabled |
|                                          | MahApps.Brushes.CheckBox.BorderBrushUnchecked |
|                                          | MahApps.Brushes.CheckBox.BorderBrushUncheckedMouseOver |
|                                          | MahApps.Brushes.CheckBox.BorderBrushUncheckedPressed |
|                                          | MahApps.Brushes.CheckBox.BorderBrushUncheckedDisabled |
|                                          | MahApps.Brushes.CheckBox.BorderBrushChecked |
|                                          | MahApps.Brushes.CheckBox.BorderBrushCheckedMouseOver |
|                                          | MahApps.Brushes.CheckBox.BorderBrushCheckedPressed |
|                                          | MahApps.Brushes.CheckBox.BorderBrushCheckedDisabled |
|                                          | MahApps.Brushes.CheckBox.BorderBrushIndeterminate |
|                                          | MahApps.Brushes.CheckBox.BorderBrushIndeterminateMouseOver |
|                                          | MahApps.Brushes.CheckBox.BorderBrushIndeterminatePressed |
|                                          | MahApps.Brushes.CheckBox.BorderBrushIndeterminateDisabled |
|                                          | MahApps.Brushes.CheckBox.CheckBackgroundStrokeUnchecked |
|                                          | MahApps.Brushes.CheckBox.CheckBackgroundStrokeUncheckedMouseOver |
|                                          | MahApps.Brushes.CheckBox.CheckBackgroundStrokeUncheckedPressed |
|                                          | MahApps.Brushes.CheckBox.CheckBackgroundStrokeUncheckedDisabled |
|                                          | MahApps.Brushes.CheckBox.CheckBackgroundStrokeChecked |
|                                          | MahApps.Brushes.CheckBox.CheckBackgroundStrokeCheckedMouseOver |
|                                          | MahApps.Brushes.CheckBox.CheckBackgroundStrokeCheckedPressed |
|                                          | MahApps.Brushes.CheckBox.CheckBackgroundStrokeCheckedDisabled |
|                                          | MahApps.Brushes.CheckBox.CheckBackgroundStrokeIndeterminate |
|                                          | MahApps.Brushes.CheckBox.CheckBackgroundStrokeIndeterminateMouseOver |
|                                          | MahApps.Brushes.CheckBox.CheckBackgroundStrokeIndeterminatePressed |
|                                          | MahApps.Brushes.CheckBox.CheckBackgroundStrokeIndeterminateDisabled |
|                                          | MahApps.Brushes.CheckBox.CheckBackgroundFillUnchecked |
|                                          | MahApps.Brushes.CheckBox.CheckBackgroundFillUncheckedMouseOver |
|                                          | MahApps.Brushes.CheckBox.CheckBackgroundFillUncheckedPressed |
|                                          | MahApps.Brushes.CheckBox.CheckBackgroundFillUncheckedDisabled |
|                                          | MahApps.Brushes.CheckBox.CheckBackgroundFillChecked |
|                                          | MahApps.Brushes.CheckBox.CheckBackgroundFillCheckedMouseOver |
|                                          | MahApps.Brushes.CheckBox.CheckBackgroundFillCheckedPressed |
|                                          | MahApps.Brushes.CheckBox.CheckBackgroundFillCheckedDisabled |
|                                          | MahApps.Brushes.CheckBox.CheckBackgroundFillIndeterminate |
|                                          | MahApps.Brushes.CheckBox.CheckBackgroundFillIndeterminateMouseOver |
|                                          | MahApps.Brushes.CheckBox.CheckBackgroundFillIndeterminatePressed |
|                                          | MahApps.Brushes.CheckBox.CheckBackgroundFillIndeterminateDisabled |
|                                          | MahApps.Brushes.CheckBox.CheckGlyphForegroundUnchecked |
|                                          | MahApps.Brushes.CheckBox.CheckGlyphForegroundUncheckedMouseOver |
|                                          | MahApps.Brushes.CheckBox.CheckGlyphForegroundUncheckedPressed |
|                                          | MahApps.Brushes.CheckBox.CheckGlyphForegroundUncheckedDisabled |
|                                          | MahApps.Brushes.CheckBox.CheckGlyphForegroundChecked |
|                                          | MahApps.Brushes.CheckBox.CheckGlyphForegroundCheckedMouseOver |
|                                          | MahApps.Brushes.CheckBox.CheckGlyphForegroundCheckedPressed |
|                                          | MahApps.Brushes.CheckBox.CheckGlyphForegroundCheckedDisabled |
|                                          | MahApps.Brushes.CheckBox.CheckGlyphForegroundIndeterminate |
|                                          | MahApps.Brushes.CheckBox.CheckGlyphForegroundIndeterminateMouseOver |
|                                          | MahApps.Brushes.CheckBox.CheckGlyphForegroundIndeterminatePressed |
|                                          | MahApps.Brushes.CheckBox.CheckGlyphForegroundIndeterminateDisabled |
|                                          | MahApps.Brushes.RadioButton.Foreground   |
|                                          | MahApps.Brushes.RadioButton.ForegroundPointerOver |
|                                          | MahApps.Brushes.RadioButton.ForegroundPressed |
|                                          | MahApps.Brushes.RadioButton.ForegroundDisabled |
|                                          | MahApps.Brushes.RadioButton.Background   |
|                                          | MahApps.Brushes.RadioButton.BackgroundPointerOver |
|                                          | MahApps.Brushes.RadioButton.BackgroundPressed |
|                                          | MahApps.Brushes.RadioButton.BackgroundDisabled |
|                                          | MahApps.Brushes.RadioButton.BorderBrush  |
|                                          | MahApps.Brushes.RadioButton.BorderBrushPointerOver |
|                                          | MahApps.Brushes.RadioButton.BorderBrushPressed |
|                                          | MahApps.Brushes.RadioButton.BorderBrushDisabled |
|                                          | MahApps.Brushes.RadioButton.OuterEllipseStroke |
|                                          | MahApps.Brushes.RadioButton.OuterEllipseStrokePointerOver |
|                                          | MahApps.Brushes.RadioButton.OuterEllipseStrokePressed |
|                                          | MahApps.Brushes.RadioButton.OuterEllipseStrokeDisabled |
|                                          | MahApps.Brushes.RadioButton.OuterEllipseFill |
|                                          | MahApps.Brushes.RadioButton.OuterEllipseFillPointerOver |
|                                          | MahApps.Brushes.RadioButton.OuterEllipseFillPressed |
|                                          | MahApps.Brushes.RadioButton.OuterEllipseFillDisabled |
|                                          | MahApps.Brushes.RadioButton.OuterEllipseCheckedStroke |
|                                          | MahApps.Brushes.RadioButton.OuterEllipseCheckedStrokePointerOver |
|                                          | MahApps.Brushes.RadioButton.OuterEllipseCheckedStrokePressed |
|                                          | MahApps.Brushes.RadioButton.OuterEllipseCheckedStrokeDisabled |
|                                          | MahApps.Brushes.RadioButton.OuterEllipseCheckedFill |
|                                          | MahApps.Brushes.RadioButton.OuterEllipseCheckedFillPointerOver |
|                                          | MahApps.Brushes.RadioButton.OuterEllipseCheckedFillPressed |
|                                          | MahApps.Brushes.RadioButton.OuterEllipseCheckedFillDisabled |
|                                          | MahApps.Brushes.RadioButton.CheckGlyphFill |
|                                          | MahApps.Brushes.RadioButton.CheckGlyphFillPointerOver |
|                                          | MahApps.Brushes.RadioButton.CheckGlyphFillPressed |
|                                          | MahApps.Brushes.RadioButton.CheckGlyphFillDisabled |
|                                          | MahApps.Brushes.RadioButton.CheckGlyphStroke |
|                                          | MahApps.Brushes.RadioButton.CheckGlyphStrokePointerOver |
|                                          | MahApps.Brushes.RadioButton.CheckGlyphStrokePressed |
|                                          | MahApps.Brushes.RadioButton.CheckGlyphStrokeDisabled |
|                                          | MahApps.Brushes.ToggleSwitch.ContentForeground |
|                                          | MahApps.Brushes.ToggleSwitch.ContentForegroundDisabled |
|                                          | MahApps.Brushes.ToggleSwitch.HeaderForeground |
|                                          | MahApps.Brushes.ToggleSwitch.HeaderForegroundDisabled |
|                                          | MahApps.Brushes.ToggleSwitch.ContainerBackground |
|                                          | MahApps.Brushes.ToggleSwitch.ContainerBackgroundPointerOver |
|                                          | MahApps.Brushes.ToggleSwitch.ContainerBackgroundPressed |
|                                          | MahApps.Brushes.ToggleSwitch.ContainerBackgroundDisabled |
|                                          | MahApps.Brushes.ToggleSwitch.FillOff     |
|                                          | MahApps.Brushes.ToggleSwitch.FillOffPointerOver |
|                                          | MahApps.Brushes.ToggleSwitch.FillOffPressed |
|                                          | MahApps.Brushes.ToggleSwitch.FillOffDisabled |
|                                          | MahApps.Brushes.ToggleSwitch.StrokeOff   |
|                                          | MahApps.Brushes.ToggleSwitch.StrokeOffPointerOver |
|                                          | MahApps.Brushes.ToggleSwitch.StrokeOffPressed |
|                                          | MahApps.Brushes.ToggleSwitch.StrokeOffDisabled |
|                                          | MahApps.Brushes.ToggleSwitch.FillOn      |
|                                          | MahApps.Brushes.ToggleSwitch.FillOnPointerOver |
|                                          | MahApps.Brushes.ToggleSwitch.FillOnPressed |
|                                          | MahApps.Brushes.ToggleSwitch.FillOnDisabled |
|                                          | MahApps.Brushes.ToggleSwitch.StrokeOn    |
|                                          | MahApps.Brushes.ToggleSwitch.StrokeOnPointerOver |
|                                          | MahApps.Brushes.ToggleSwitch.StrokeOnPressed |
|                                          | MahApps.Brushes.ToggleSwitch.StrokeOnDisabled |
|                                          | MahApps.Brushes.ToggleSwitch.KnobFillOff |
|                                          | MahApps.Brushes.ToggleSwitch.KnobFillOffPointerOver |
|                                          | MahApps.Brushes.ToggleSwitch.KnobFillOffPressed |
|                                          | MahApps.Brushes.ToggleSwitch.KnobFillOffDisabled |
|                                          | MahApps.Brushes.ToggleSwitch.KnobFillOn  |
|                                          | MahApps.Brushes.ToggleSwitch.KnobFillOnPointerOver |
|                                          | MahApps.Brushes.ToggleSwitch.KnobFillOnPressed |
|                                          | MahApps.Brushes.ToggleSwitch.KnobFillOnDisabled |

### Deleted Brushes

| Key                                      |
|------------------------------------------|
| ComboBoxPopupBrush                       |
| MenuItemDisabledBrush                    |
| CheckBoxBackgroundBrush                  |
| WhiteColorBrush                          |
| BlackColorBrush                          |
| LabelTextBrush                           |
| DisabledWhiteBrush                       |
| TextBoxMouseOverInnerBorderBrush         |
| ButtonMouseOverInnerBorderBrush          |
| ComboBoxMouseOverInnerBorderBrush        |
| DarkIdealForegroundDisabledBrush         |
| MahApps.Metro.Brushes.ToggleSwitchButton.PressedBrush.Win10 |
| MahApps.Metro.Brushes.ToggleSwitchButton.OffBorderBrush.Win10 |
| MahApps.Metro.Brushes.ToggleSwitchButton.OffMouseOverBorderBrush.Win10 |
| MahApps.Metro.Brushes.ToggleSwitchButton.OffDisabledBorderBrush.Win10 |
| MahApps.Metro.Brushes.ToggleSwitchButton.OffSwitchBrush.Win10 |
| MahApps.Metro.Brushes.ToggleSwitchButton.OnSwitchBrush.Win10 |
| MahApps.Metro.Brushes.ToggleSwitchButton.OnSwitchDisabledBrush.Win10 |
| MahApps.Metro.Brushes.ToggleSwitchButton.OnSwitchMouseOverBrush.Win10 |
| MahApps.Metro.Brushes.ToggleSwitchButton.ThumbIndicatorBrush.Win10 |
| MahApps.Metro.Brushes.ToggleSwitchButton.ThumbIndicatorMouseOverBrush.Win10 |
| MahApps.Metro.Brushes.ToggleSwitchButton.ThumbIndicatorCheckedBrush.Win10 |
| MahApps.Metro.Brushes.ToggleSwitchButton.ThumbIndicatorPressedBrush.Win10 |
| MahApps.Metro.Brushes.ToggleSwitchButton.ThumbIndicatorDisabledBrush.Win10 |

## List of deleted, renamed and new fonts

| Old Key                            | New Key                                  |
|------------------------------------|------------------------------------------|
|                                    | MahApps.Fonts.Family.SymbolTheme         |
| DefaultFont                        | MahApps.Fonts.Family.Button              |
| HeaderFontFamily                   | MahApps.Fonts.Family.Header              |
|                                    | MahApps.Fonts.Family.Window.Title        |
| ContentFontFamily                  | MahApps.Fonts.Family.Control             |
| ToggleSwitchFontFamily             | MahApps.Fonts.Family.ToggleSwitch        |
| ToggleSwitchHeaderFontFamily       | MahApps.Fonts.Family.ToggleSwitch.Header |
| ToggleSwitchFontFamily.Win10       | -- deleted --                            |
| ToggleSwitchHeaderFontFamily.Win10 | -- deleted --                            |

## List of deleted, renamed and new font sizes

| Old Key                          | New Key                               |
|----------------------------------|---------------------------------------|
| HeaderFontSize                   | MahApps.Font.Size.Header              |
| SubHeaderFontSize                | MahApps.Font.Size.SubHeader           |
| WindowTitleFontSize              | MahApps.Font.Size.Window.Title        |
| NormalFontSize                   | MahApps.Font.Size.Default             |
| ContentFontSize                  | MahApps.Font.Size.Content             |
| FlatButtonFontSize               | MahApps.Font.Size.Button.Flat         |
| TabItemFontSize                  | MahApps.Font.Size.TabItem             |
| UpperCaseContentFontSize         | MahApps.Font.Size.Button              |
| FloatingWatermarkFontSize        | MahApps.Font.Size.FloatingWatermark   |
| ClearTextButtonFontSize          | MahApps.Font.Size.Button.ClearText    |
| TooltipFontSize                  | MahApps.Font.Size.Tooltip             |
| MenuFontSize                     | MahApps.Font.Size.Menu                |
| ContextMenuFontSize              | MahApps.Font.Size.ContextMenu         |
| StatusBarFontSize                | MahApps.Font.Size.StatusBar           |
| DialogTitleFontSize              | MahApps.Font.Size.Dialog.Title        |
| DialogMessageFontSize            | MahApps.Font.Size.Dialog.Message      |
|                                  | MahApps.Font.Size.Dialog.Button       |
| FlyoutHeaderFontSize             | MahApps.Font.Size.Flyout.Header       |
| ToggleSwitchFontSize             | MahApps.Font.Size.ToggleSwitch        |
| ToggleSwitchHeaderFontSize       | MahApps.Font.Size.ToggleSwitch.Header |
| ToggleSwitchFontSize.Win10       | -- deleted --                         |
| ToggleSwitchHeaderFontSize.Win10 | -- deleted --                         |

## List of renamed and new sizes

| Old Key                              | New Key                                  | Resource Dictionary      |
|--------------------------------------|------------------------------------------|--------------------------|
| MetroScrollBarHeight                 | MahApps.Sizes.ScrollBar.Height           | Controls.Scrollbars.xaml |
| MetroScrollBarWidth                  | MahApps.Sizes.ScrollBar.Width            | Controls.Scrollbars.xaml |
| HorizontalScrollBarRepeatButtonWidth | MahApps.Sizes.ScrollBar.HorizontalRepeatButton.Width | Controls.Scrollbars.xaml |
| VerticalScrollBarRepeatButtonHeight  | MahApps.Sizes.ScrollBar.VerticalRepeatButton.Height | Controls.Scrollbars.xaml |
| HorizontalThumbHeight                | MahApps.Sizes.Slider.HorizontalThumb.Height | Controls.Slider.xaml     |
| HorizontalThumbWidth                 | MahApps.Sizes.Slider.HorizontalThumb.Width | Controls.Slider.xaml     |
| VerticalThumbWidth                   | MahApps.Sizes.Slider.VerticalThumb.Width | Controls.Slider.xaml     |
| VerticalThumbHeight                  | MahApps.Sizes.Slider.VerticalThumb.Height | Controls.Slider.xaml     |
| GroupBoxHeaderThemeFontSize          | MahApps.Sizes.GroupBox.HeaderThemeFont   | VS/GroupBox.xaml         |
| ProgressBarMinHeight                 | MahApps.Sizes.ProgressBar.MinHeight      | MetroProgressBar.xaml    |
|                                      | MahApps.Sizes.Slider.Flat.Horizontal.MinHeight | Controls.FlatSlider.xaml |
|                                      | MahApps.Sizes.Slider.Flat.Vertical.MinWidth | Controls.FlatSlider.xaml |

## Styles and Templates

### List of deleted, renamed and new styles

| Old Key / Name                           | New Key / Name                           | Resource Dictionary                      |
|------------------------------------------|------------------------------------------|------------------------------------------|
| FloatingMessageContainerStyle            | MahApps.Styles.ContentControl.FloatingMessageContainer | Controls.ContentControl.xaml             |
| PathIconContentControlStyle              | MahApps.Styles.ContentControl.PathIcon   | Controls.ContentControl.xaml             |
|                                          | MahApps.Styles.GridSplitter.Preview      | Controls.GridSplitter.xaml               |
|                                          | MahApps.Styles.GridSplitter              | Controls.GridSplitter.xaml               |
| Controls.AnimatedSingleRowTabControl.xaml | MahApps.Styles.TabControl.AnimatedSingleRow | Controls.TabControl.xaml                 |
| ScrollBarRepeatButtonStyle               | MahApps.Styles.RepeatButton.ScrollBar.AnimatedSingleRow | Controls.TabControl.xaml                 |
| HorizontalScrollBarTemplate              | MahApps.Templates.ScrollBar.AnimatedSingleRow.Horizontal | Controls.TabControl.xaml                 |
| VerticalScrollBarTemplate                | MahApps.Templates.ScrollBar.AnimatedSingleRow.Vertical | Controls.TabControl.xaml                 |
| ScrollViewerTemplate                     | MahApps.Templates.ScrollViewer.AnimatedSingleRow | Controls.TabControl.xaml                 |
| VerticalScrollViewerTemplate             | -- deleted --                            |                                          |
| HorizontalAnimatedSingleRowTabControl    | MahApps.Templates.TabControl.AnimatedSingleRow.Horizontal | Controls.TabControl.xaml                 |
| VerticalAnimatedSingleRowTabControl      | MahApps.Templates.TabControl.AnimatedSingleRow.Vertical | Controls.TabControl.xaml                 |
| Controls.AnimatedTabControl.xaml         | MahApps.Styles.TabControl.Animated       | Controls.TabControl.xaml                 |
| MahApps.Metro.Styles.FlatButtonFocusVisualStyle | MahApps.Styles.Button.FocusVisualStyle.Flat | Controls.Buttons.xaml                    |
| MetroFlatButton                          | MahApps.Styles.Button.Flat               | Controls.Buttons.xaml                    |
| MetroFlatToggleButton                    | MahApps.Styles.ToggleButton.Flat         | Controls.Buttons.xaml                    |
| MahApps.Metro.Styles.CircleButtonFocusVisualStyle | MahApps.Styles.Button.FocusVisualStyle.Circle | Controls.Buttons.xaml                    |
| MahApps.Metro.Styles.MetroCircleFocusVisual | -- deleted --                            |                                          |
| MetroCircleButtonFocusVisual             | -- deleted --                            |                                          |
| MahApps.Metro.Styles.MetroCircleButtonStyle | MahApps.Styles.Button.Circle             | Controls.Buttons.xaml                    |
| MetroCircleButtonStyle                   | -- deleted --                            |                                          |
| ChromelessButtonTemplate                 | MahApps.Templates.Button.Chromeless      | Controls.Buttons.xaml                    |
| ChromelessButtonStyle                    | MahApps.Styles.Button.Chromeless         | Controls.Buttons.xaml                    |
| BaseMetroWindowButtonStyle               | MahApps.Styles.Button.MetroWindow.Base   | Controls.Buttons.xaml                    |
| MetroBaseWindowButtonStyle               | -- deleted --                            |                                          |
| MetroWindowButtonStyle                   | -- deleted --                            |                                          |
| LightMetroWindowButtonStyle              | MahApps.Styles.Button.MetroWindow.Light  | Controls.Buttons.xaml                    |
| DarkMetroWindowButtonStyle               | MahApps.Styles.Button.MetroWindow.Dark   | Controls.Buttons.xaml                    |
| MahApps.Metro.Styles.WindowButton.Close.Light.Win10 | MahApps.Styles.Button.MetroWindow.Close.Light.Win10 | Controls.Buttons.xaml                    |
| MahApps.Metro.Styles.WindowButton.Close.Dark.Win10 | MahApps.Styles.Button.MetroWindow.Close.Dark.Win10 | Controls.Buttons.xaml                    |
| MahApps.Metro.Styles.MetroButton         | MahApps.Styles.Button                    | Controls.Buttons.xaml                    |
| MetroButton                              | -- deleted --                            |                                          |
| SquareButtonStyle                        | MahApps.Styles.Button.Square             | Controls.Buttons.xaml                    |
| AccentedSquareButtonStyle                | MahApps.Styles.Button.Square.Accent      | Controls.Buttons.xaml                    |
| HighlightedSquareButtonStyle             | MahApps.Styles.Button.Square.Highlight   | Controls.Buttons.xaml                    |
| MahApps.Metro.Styles.MetroCircleToggleButtonStyle | MahApps.Styles.ToggleButton.Circle       | Controls.Buttons.xaml                    |
| MetroCircleToggleButtonStyle             | -- deleted --                            |                                          |
| MahApps.Metro.Styles.MetroToggleButton   | MahApps.Styles.ToggleButton              | Controls.Buttons.xaml                    |
| MetroToggleButton                        | -- deleted --                            |                                          |
| SquareMetroButton                        | MahApps.Styles.Button.MetroSquare        | Controls.Buttons.xaml                    |
| MetroAccentButton                        | MahApps.Styles.Button.MetroSquare.Accent | Controls.Buttons.xaml                    |
| ButtonDropDownStyle                      | MahApps.Styles.Button.DropDown           | Controls.Buttons.xaml                    |
| ButtonSplitStyle                         | MahApps.Styles.Button.Split              | Controls.Buttons.xaml                    |
| ButtonSplitArrowStyle                    | MahApps.Styles.Button.Split.Arrow        | Controls.Buttons.xaml                    |
| MahApps.Metro.Styles.MetroCalendarDayButtonStyle | MahApps.Styles.CalendarDayButton         | Controls.Calendar.xaml                   |
| MetroCalendarDayButtonStyle              | -- deleted --                            |                                          |
| MahApps.Metro.Styles.MetroCalendarItemStyle | MahApps.Styles.CalendarItem              | Controls.Calendar.xaml                   |
| PreviousButtonTemplate                   | MahApps.Templates.Button.Calendar.Previous | Controls.Calendar.xaml                   |
| NextButtonTemplate                       | MahApps.Templates.Button.Calendar.Next   | Controls.Calendar.xaml                   |
| HeaderButtonTemplate                     | MahApps.Templates.Button.Calendar.Header | Controls.Calendar.xaml                   |
| PreviousCalendarButtonStyle              | MahApps.Styles.Button.Calendar.Previous  | Controls.Calendar.xaml                   |
| NextCalendarButtonStyle                  | MahApps.Styles.Button.Calendar.Next      | Controls.Calendar.xaml                   |
| HeaderCalendarButtonStyle                | MahApps.Styles.Button.Calendar.Header    | Controls.Calendar.xaml                   |
| MetroCalendarItemStyle                   | -- deleted --                            |                                          |
| MahApps.Metro.Styles.MetroCalendarButtonStyle | MahApps.Styles.CalendarButton            | Controls.Calendar.xaml                   |
| MetroCalendarButtonStyle                 | -- deleted --                            |                                          |
| MahApps.Metro.Styles.BaseMetroCalendar   | MahApps.Styles.Calendar.Base             | Controls.Calendar.xaml                   |
| MahApps.Metro.Styles.MetroCalendar       | MahApps.Styles.Calendar                  | Controls.Calendar.xaml                   |
| MetroCalendar                            | -- deleted --                            |                                          |
| MetroCheckBox                            | MahApps.Styles.CheckBox                  | Controls.CheckBox.xaml                   |
|                                          | MahApps.Styles.CheckBox.Win10            | Controls.CheckBox.xaml                   |
| EditableTextBoxStyle                     | MahApps.Styles.TextBox.ComboBox.Editable | Controls.ComboBox.xaml                   |
| MetroComboBoxDropDownToggleButtonStyle   | MahApps.Styles.ToggleButton.ComboBoxDropDown | Controls.ComboBox.xaml                   |
| MetroComboBox                            | MahApps.Styles.ComboBox                  | Controls.ComboBox.xaml                   |
| VirtualisedMetroComboBox                 | MahApps.Styles.ComboBox.Virtualized      | Controls.ComboBox.xaml                   |
| MetroComboBoxItem                        | MahApps.Styles.ComboBoxItem              | Controls.ComboBox.xaml                   |
| MetroMenu                                | MahApps.Styles.Menu                      | Controls.Menu.xaml                       |
| MetroContextMenu                         | MahApps.Styles.ContextMenu               | Controls.ContextMenu.xaml                |
| MetroMenuItem                            | MahApps.Styles.MenuItem                  | Controls.MenuItem.xaml                   |
| TextBoxMetroContextMenu                  | MahApps.TextBox.ContextMenu              | Controls.ContextMenu.xaml                |
| MenuScrollButton                         | MahApps.Styles.RepeatButton.MenuScroll   | Controls.MenuItem.xaml                   |
| MetroDataGridCheckBox                    | MahApps.Styles.CheckBox.DataGrid         | Controls.DataGrid.xaml                   |
|                                          | MahApps.Styles.CheckBox.DataGrid.Win10   | Controls.DataGrid.xaml                   |
|                                          | MahApps.Styles.ComboBox.DataGrid         | Controls.DataGrid.xaml                   |
|                                          | MahApps.Styles.ComboBox.DataGrid.Editing | Controls.DataGrid.xaml                   |
|                                          | MahApps.Styles.TextBlock.DataGrid        | Controls.DataGrid.xaml                   |
|                                          | MahApps.Styles.Hyperlink.DataGrid        | Controls.DataGrid.xaml                   |
|                                          | MahApps.Styles.TextBox.DataGrid.Editing  | Controls.DataGrid.xaml                   |
|                                          | MahApps.Styles.NumericUpDown.DataGrid    | Controls.DataGrid.xaml                   |
|                                          | MahApps.Styles.NumericUpDown.DataGrid.Editing | Controls.DataGrid.xaml                   |
| MetroColumnHeaderGripperStyle            | MahApps.Styles.Thumb.ColumnHeaderGripper | Controls.DataGrid.xaml                   |
| MetroRowHeaderGripperStyle               | MahApps.Styles.Thumb.RowHeaderGripper    | Controls.DataGrid.xaml                   |
| MetroDataGridColumnHeader                | MahApps.Styles.DataGridColumnHeader      | Controls.DataGrid.xaml                   |
| MetroDataGridRowHeader                   | MahApps.Styles.DataGridRowHeader         | Controls.DataGrid.xaml                   |
| MetroDataGridCell                        | MahApps.Styles.DataGridCell              | Controls.DataGrid.xaml                   |
| DefaultRowValidationErrorTemplate        | MahApps.Templates.DataGridRow.ValidationError | Controls.DataGrid.xaml                   |
| MetroDataGridRow                         | MahApps.Styles.DataGridRow               | Controls.DataGrid.xaml                   |
| MetroDataGrid                            | MahApps.Styles.DataGrid                  | Controls.DataGrid.xaml                   |
|                                          | MahApps.Styles.ToggleButton.ExpanderHeader.Down.DataGrid.GroupItem | Controls.DataGrid.xaml                   |
|                                          | MahApps.Styles.GroupItem.DataGrid        | Controls.DataGrid.xaml                   |
| AzureDataGridColumnHeader                | MahApps.Styles.DataGridColumnHeader.Azure | Controls.DataGrid.xaml                   |
| AzureDataGridRowHeader                   | MahApps.Styles.DataGridRowHeader.Azure   | Controls.DataGrid.xaml                   |
| AzureDataGridCell                        | MahApps.Styles.DataGridCell.Azure        | Controls.DataGrid.xaml                   |
| AzureDataGridRow                         | MahApps.Styles.DataGridRow.Azure         | Controls.DataGrid.xaml                   |
| AzureDataGridRowWithMargin               | MahApps.Styles.DataGridRow.AzureWithMargin | Controls.DataGrid.xaml                   |
| AzureDataGrid                            | MahApps.Styles.DataGrid.Azure            | Controls.DataGrid.xaml                   |
| MetroDatePicker                          | MahApps.Styles.DatePicker                | Controls.DatePicker.xaml                 |
| MetroDatePickerTextBox                   | MahApps.Styles.DatePickerTextBox         | Controls.DatePicker.xaml                 |
|                                          | MahApps.Styles.DatePickerTextBox.TimePickerBase | Controls.DatePicker.xaml                 |
| ExpanderBaseHeaderStyle                  | MahApps.Styles.ToggleButton.ExpanderHeader.Base | Controls.Expander.xaml                   |
| ExpanderRightHeaderStyle                 | MahApps.Styles.ToggleButton.ExpanderHeader.Right | Controls.Expander.xaml                   |
| ExpanderUpHeaderStyle                    | MahApps.Styles.ToggleButton.ExpanderHeader.Up | Controls.Expander.xaml                   |
| ExpanderLeftHeaderStyle                  | MahApps.Styles.ToggleButton.ExpanderHeader.Left | Controls.Expander.xaml                   |
| ExpanderDownHeaderStyle                  | MahApps.Styles.ToggleButton.ExpanderHeader.Down | Controls.Expander.xaml                   |
| MetroExpanderExpandStoryboard            | MahApps.Storyboard.Expander.Expand       | Controls.Expander.xaml                   |
| MetroExpanderCollapseStoryboard          | MahApps.Storyboard.Expander.Collapse     | Controls.Expander.xaml                   |
| MetroExpander                            | MahApps.Styles.Expander                  | Controls.Expander.xaml                   |
| MetroGroupBox                            | MahApps.Styles.GroupBox                  | Controls.GroupBox.xaml                   |
|                                          | MahApps.Styles.Hyperlink                 | Controls.Hyperlink.xaml                  |
| MetroLabel                               | MahApps.Styles.Label                     | Controls.Label.xaml                      |
| MetroListBox                             | MahApps.Styles.ListBox                   | Controls.ListBox.xaml                    |
| VirtualisedMetroListBox                  | MahApps.Styles.ListBox.Virtualized       | Controls.ListBox.xaml                    |
| MetroListBoxItem                         | MahApps.Styles.ListBoxItem               | Controls.ListBox.xaml                    |
| MetroGridViewScrollViewerStyle           | MahApps.Styles.ScrollViewer.GridView     | Controls.ListView.xaml                   |
| MetroListView                            | MahApps.Styles.ListView                  | Controls.ListView.xaml                   |
| VirtualisedMetroListView                 | MahApps.Styles.ListView.Virtualized      | Controls.ListView.xaml                   |
| MetroListViewItem                        | MahApps.Styles.ListViewItem              | Controls.ListView.xaml                   |
| NonSelectableListViewContainerStyle      | MahApps.Styles.ListViewItem.NonSelectable | Controls.ListView.xaml                   |
| GridViewColumnHeaderGripper              | MahApps.Styles.Thumb.GridViewColumnHeaderGripper | Controls.ListView.xaml                   |
| MetroGridViewColumnHeader                | MahApps.Styles.GridViewColumnHeader      | Controls.ListView.xaml                   |
| MetroPage                                | MahApps.Styles.Page                      | Controls.Page.xaml                       |
| RevealButtonIcon                         | -- deleted --                            |                                          |
| RevealButtonStyle                        | MahApps.Styles.Button.Reveal             | Controls.PasswordBox.xaml                |
| DefaultRevealButtonIcon                  | MahApps.Controls.RevealIcon              | Controls.PasswordBox.xaml                |
| DefaultCapsLockIcon                      | MahApps.Controls.CapsLockIcon            | Controls.PasswordBox.xaml                |
| MetroPasswordBox                         | MahApps.Styles.PasswordBox               | Controls.PasswordBox.xaml                |
| MetroButtonPasswordBox                   | MahApps.Styles.PasswordBox.Button        | Controls.PasswordBox.xaml                |
| RevealedTextBoxStyle                     | MahApps.Styles.TextBox.PasswordBox.Revealed | Controls.PasswordBox.xaml                |
| MetroButtonRevealedPasswordBox           | MahApps.Styles.PasswordBox.Button.Revealed | Controls.PasswordBox.xaml                |
| Win8MetroPasswordBox                     | MahApps.Styles.PasswordBox.Win8          | Controls.PasswordBox.xaml                |
| MahApps.Metro.Styles.ProgressBar         | MahApps.Styles.ProgressBar               | Controls.ProgressBar.xaml                |
| MetroProgressBar                         | -- deleted --                            |                                          |
| MetroRadioButton                         | MahApps.Styles.RadioButton               | Controls.RadioButton.xaml                |
|                                          | MahApps.Styles.RadioButton.Win10         | Controls.RadioButton.xaml                |
| MetroScrollBarThumbMouseOverStoryboard   | MahApps.Storyboard.ScrollBarThumbMouseOver | Controls.Scrollbars.xaml                 |
| MetroScrollBarThumbPressedStoryboard     | MahApps.Storyboard.ScrollBarThumbPressed | Controls.Scrollbars.xaml                 |
| MetroScrollBarRepeatButtonSmallStyle     | MahApps.Styles.RepeatButton.ScrollBarSmall | Controls.Scrollbars.xaml                 |
| MetroScrollBarRepeatButtonLargeStyle     | MahApps.Styles.RepeatButton.ScrollBarLarge | Controls.Scrollbars.xaml                 |
| MetroScrollBarThumbStyle                 | MahApps.Styles.Thumb.ScrollBar           | Controls.Scrollbars.xaml                 |
| HorizontalScrollBar                      | MahApps.Templates.ScrollBar.Horizontal   | Controls.Scrollbars.xaml                 |
| VerticalScrollBar                        | MahApps.Templates.ScrollBar.Vertical     | Controls.Scrollbars.xaml                 |
| MetroScrollBar                           | MahApps.Styles.ScrollBar                 | Controls.Scrollbars.xaml                 |
| MetroScrollViewer                        | MahApps.Styles.ScrollViewer              | Controls.Scrollbars.xaml                 |
| DropShadowBrush                          | DropShadowBrush                          | Controls.Shadows.xaml                    |
| MahApps.Metro.Styles.Slider.Thumb        | MahApps.Styles.Thumb.Slider              | Controls.Slider.xaml                     |
| MahApps.Metro.Styles.Slider.HorizontalTrack | MahApps.Styles.RepeatButton.Slider.HorizontalTrack | Controls.Slider.xaml                     |
| MahApps.Metro.Styles.Slider.HorizontalTrackValue | MahApps.Styles.RepeatButton.Slider.HorizontalTrackValue | Controls.Slider.xaml                     |
| MahApps.Metro.Styles.Slider.VerticalTrack | MahApps.Styles.RepeatButton.Slider.VerticalTrack | Controls.Slider.xaml                     |
| MahApps.Metro.Styles.Slider.VerticalTrackValue | MahApps.Styles.RepeatButton.Slider.VerticalTrackValue | Controls.Slider.xaml                     |
| MahApps.Metro.Styles.Slider.HorizontalTemplate | MahApps.Templates.Slider.Horizontal      | Controls.Slider.xaml                     |
| MahApps.Metro.Styles.Slider.VerticalTemplate | MahApps.Templates.Slider.Vertical        | Controls.Slider.xaml                     |
| MahApps.Metro.Styles.Slider              | MahApps.Styles.Slider                    | Controls.Slider.xaml                     |
| HorizontalSliderThumb                    | -- deleted --                            |                                          |
| HorizontalTrackLargeDecrease             | -- deleted --                            |                                          |
| HorizontalTrackValue                     | -- deleted --                            |                                          |
| HorizontalSliderTemplate                 | -- deleted --                            |                                          |
| VerticalSliderThumb                      | -- deleted --                            |                                          |
| VerticalTrackLargeDecrease               | -- deleted --                            |                                          |
| VerticalTrackValue                       | -- deleted --                            |                                          |
| VerticalSliderTemplate                   | -- deleted --                            |                                          |
| MetroSlider                              | -- deleted --                            |                                          |
| MahApps.Metro.Styles.Slider.Thumb.Win10  | MahApps.Styles.Thumb.Slider.Win10        | Controls.Slider.xaml                     |
| MahApps.Metro.Styles.Slider.HorizontalTrack.Win10 | MahApps.Styles.RepeatButton.Slider.HorizontalTrack.Win10 | Controls.Slider.xaml                     |
| MahApps.Metro.Styles.Slider.VerticalTrack.Win10 | MahApps.Styles.RepeatButton.Slider.VerticalTrack.Win10 | Controls.Slider.xaml                     |
| MahApps.Metro.Styles.Slider.HorizontalTemplate.Win10 | MahApps.Templates.Slider.Horizontal.Win10 | Controls.Slider.xaml                     |
| MahApps.Metro.Styles.Slider.VerticalTemplate.Win10 | MahApps.Templates.Slider.Vertical.Win10  | Controls.Slider.xaml                     |
| MahApps.Metro.Styles.Slider.Win10        | MahApps.Styles.Slider.Win10              | Controls.Slider.xaml                     |
| MetroStatusBarItem                       | MahApps.Styles.StatusBarItem             | Controls.StatusBar.xaml                  |
| MetroStatusBarSeparator                  | MahApps.Styles.Separator.StatusBar       | Controls.StatusBar.xaml                  |
| MetroStatusBar                           | MahApps.Styles.StatusBar                 | Controls.StatusBar.xaml                  |
| MetroTabControl                          | MahApps.Styles.TabControl                | Controls.TabControl.xaml                 |
| MetroTabItem                             | MahApps.Styles.TabItem                   | Controls.TabControl.xaml                 |
| MetroTextBlock                           | MahApps.Styles.TextBlock                 | Controls.TextBlock.xaml                  |
| MahApps.Metro.Styles.MetroWatermarkTextBlock | MahApps.Styles.TextBlock.Watermark       | Controls.TextBlock.xaml                  |
| MetroAutoCollapsingTextBlock             | MahApps.Styles.TextBlock.AutoCollapsing  | Controls.TextBlock.xaml                  |
| MetroTextBox                             | MahApps.Styles.TextBox                   | Controls.TextBox.xaml                    |
| MetroButtonTextBox                       | MahApps.Styles.TextBox.Button            | Controls.TextBox.xaml                    |
| ButtonCommandMetroTextBox                | -- deleted --                            |                                          |
| SearchMetroTextBox                       | MahApps.Styles.TextBox.Search            | Controls.TextBox.xaml                    |
| MetroRichTextBox                         | MahApps.Styles.RichTextBox               | Controls.TextBox.xaml                    |
|                                          | MahApps.Styles.RichTextBox.Button        | Controls.TextBox.xaml                    |
| MetroToggleSwitchButton                  | -- deleted --                            |                                          |
| MetroToggleSwitch                        | MahApps.Styles.ToggleSwitch              | Themes/ToggleSwitch.xaml                 |
| MahApps.Metro.Styles.ToggleSwitchButton.Win10 | -- deleted --                            |                                          |
| MahApps.Metro.Styles.ToggleSwitch.Win10  | -- deleted --                            |                                          |
| ToolBarButtonBaseStyle                   | MahApps.Styles.Button.ToolBar            | Controls.Toolbar.xaml                    |
|                                          | MahApps.Styles.ToggleButton.ToolBar      | Controls.Toolbar.xaml                    |
| ToolBarThumbStyle                        | MahApps.Styles.Thumb.ToolBar             | Controls.Toolbar.xaml                    |
| ToolBarOverflowButtonStyle               | MahApps.Styles.ToggleButton.ToolBarOverflow | Controls.Toolbar.xaml                    |
| ToolBar                                  | MahApps.Styles.ToolBar                   | Controls.Toolbar.xaml                    |
| ToolBarTray                              | MahApps.Styles.ToolBarTray               | Controls.Toolbar.xaml                    |
| MetroToolTip                             | MahApps.Styles.ToolTip                   | Controls.Tooltip.xaml                    |
| ExpandCollapseToggleStyle                | MahApps.Styles.ToggleButton.TreeViewItem.ExpandCollapse | Controls.TreeView.xaml                   |
| TreeViewItemFocusVisual                  | -- deleted --                            |                                          |
| MetroTreeViewItem                        | MahApps.Styles.TreeViewItem              | Controls.TreeView.xaml                   |
| MetroTreeView                            | MahApps.Styles.TreeView                  | Controls.TreeView.xaml                   |
| VirtualisedMetroTreeView                 | MahApps.Styles.TreeView.Virtualized      | Controls.TreeView.xaml                   |
| MetroFlatButton                          | MahApps.Styles.Button.Flat               | Controls.Buttons.xaml, Controls.FlatButton.xaml |
| MahApps.Metro.Styles.FlatSlider.Track    | MahApps.Styles.RepeatButton.SliderTrack.Flat | Controls.FlatSlider.xaml                 |
| SliderButtonStyle                        | -- deleted --                            |                                          |
| MahApps.Metro.Styles.FlatSlider.Thumb    | MahApps.Styles.Thumb.Slider.Flat         | Controls.FlatSlider.xaml                 |
| SliderThumbStyle                         | -- deleted --                            |                                          |
| MahApps.Metro.Styles.FlatSlider.TickBar  | MahApps.Styles.TickBar.Slider.Flat       | Controls.FlatSlider.xaml                 |
| SliderTickBarStyle                       | -- deleted --                            |                                          |
| MahApps.Metro.Styles.FlatSlider.HorizontalTemplate | MahApps.Templates.Slider.Horizontal.Flat | Controls.FlatSlider.xaml                 |
| HorizontalSlider                         | -- deleted --                            |                                          |
| MahApps.Metro.Styles.FlatSlider.VerticalTemplate | MahApps.Templates.Slider.Vertical.Flat   | Controls.FlatSlider.xaml                 |
| VerticalSlider                           | -- deleted --                            |                                          |
| MahApps.Metro.Styles.FlatSlider          | MahApps.Styles.Slider.Flat               | Controls.FlatSlider.xaml                 |
| FlatSlider                               | -- deleted --                            |                                          |
| ExpoEaseIn                               | -- deleted --                            |                                          |
| ExpoEaseOut                              | -- deleted --                            |                                          |
| ExpoEaseInOut                            | MahApps.ExponentialEase.EaseInOut        | Controls.Shared.xaml                     |
| WaitingForDataEffect                     | MahApps.DropShadowEffect.WaitingForData  | Controls.Shared.xaml                     |
| WaitingForDataStoryboard                 | MahApps.Storyboard.WaitingForData        | Controls.Shared.xaml                     |
| HideFloatingMessageStoryboard            | MahApps.Storyboard.HideFloatingMessage   | Controls.Shared.xaml                     |
| ShowFloatingMessageStoryboard            | MahApps.Storyboard.ShowFloatingMessage   | Controls.Shared.xaml                     |
| enterGotFocus                            | MahApps.Storyboard.EnterGotFocus         | Controls.Shared.xaml                     |
| exitGotFocus                             | MahApps.Storyboard.ExitGotFocus          | Controls.Shared.xaml                     |
| enterHasText                             | MahApps.Storyboard.EnterHasText          | Controls.Shared.xaml                     |
| exitHasText                              | MahApps.Storyboard.ExitHasText           | Controls.Shared.xaml                     |
|                                          | MahApps.Storyboard.EnterGotFocus.DatePickerTextBox | Controls.Shared.xaml                     |
|                                          | MahApps.Storyboard.ExitGotFocus.DatePickerTextBox | Controls.Shared.xaml                     |
|                                          | MahApps.Storyboard.EnterHasText.DatePickerTextBox | Controls.Shared.xaml                     |
|                                          | MahApps.Storyboard.ExitHasText.DatePickerTextBox | Controls.Shared.xaml                     |
| MetroValidationPopup                     | MahApps.Styles.CustomValidationPopup     | Controls.ValidationError.xaml            |
| ValidationErrorTemplate                  | MahApps.Templates.ValidationError        | Controls.ValidationError.xaml            |
| FlatButton.xaml                          |                                          | Controls.FlatButton.xaml                 |
| FlatSlider.xaml                          |                                          | Controls.FlatSlider.xaml                 |
| Shared.xaml                              |                                          | Controls.Shared.xaml                     |
| Sizes.xaml                               | -- deleted --                            |                                          |
| ValidationErrorTemplate.xaml             |                                          | Controls.ValidationError.xaml            |
| Clean/Clean.xaml                         |                                          | Clean/Controls.xaml                      |
| Clean/CleanGroupBox.xaml                 |                                          | Clean/GroupBox.xaml                      |
| Clean/CleanStatusBar.xaml                |                                          | Clean/StatusBar.xaml                     |
| Clean/CleanWindow.xaml                   |                                          | Clean/MetroWindow.xaml                   |
| Clean/CleanWindowButtonCommands.xaml     |                                          | Clean/WindowButtonCommands.xaml          |
| Clean/CleanWindowButtons.xaml            |                                          | Clean/WindowButtons.xaml                 |
| Clean/CleanWindowCommands.xaml           |                                          | Clean/WindowCommands.xaml                |
| CleanGroupBoxStyleKey                    | MahApps.Styles.GroupBox.Clean            | Clean/GroupBox.xaml                      |
| CleanMetroStatusBarKey                   | MahApps.Styles.StatusBar.Clean           | Clean/StatusBar.xaml                     |
| CleanMetroStatusBarSeparator             | MahApps.Styles.Separator.Clean           | Clean/StatusBar.xaml                     |
| CleanWindowStyleKey                      | MahApps.Styles.MetroWindow.Clean         | Clean/MetroWindow.xaml                   |
| CleanWindowButtonCommandsStyleKey        | MahApps.Styles.WindowButtonCommands.Clean | Clean/WindowButtonCommands.xaml          |
|                                          | MahApps.Styles.WindowButtonCommands.Clean.Win10 | Clean/WindowButtonCommands.xaml          |
|                                          | MahApps.Styles.Button.MetroWindow.Light.Clean | Clean/WindowButtons.xaml                 |
|                                          | MahApps.Styles.Button.MetroWindow.Dark.Clean | Clean/WindowButtons.xaml                 |
| LightCleanWindowCloseButtonStyle         | MahApps.Styles.Button.MetroWindow.Close.Light.Clean | Clean/WindowButtons.xaml                 |
| DarkCleanWindowCloseButtonStyle          | MahApps.Styles.Button.MetroWindow.Close.Dark.Clean | Clean/WindowButtons.xaml                 |
|                                          | MahApps.Styles.Button.MetroWindow.Close.Light.Clean.Win10 | Clean/WindowButtons.xaml                 |
|                                          | MahApps.Styles.Button.MetroWindow.Close.Dark.Clean.Win10 | Clean/WindowButtons.xaml                 |
| CleanWindowButtonStyle                   | -- deleted --                            |                                          |
| CleanCloseWindowButtonStyle              | -- deleted --                            |                                          |
| CleanWindowCommandsStyleKey              | MahApps.Styles.WindowCommands.Clean      | Clean/WindowCommands.xaml                |
| VS/Styles.xaml                           |                                          | VS/Controls.xaml                         |
| VS/Button.xaml                           |                                          | VS/Button.xaml                           |
| VS/Expander.xaml                         |                                          | VS/Expander.xaml                         |
| VS/GroupBox.xaml                         |                                          | VS/GroupBox.xaml                         |
| VS/ListBox.xaml                          |                                          | VS/ListBox.xaml                          |
| VS/Menu.xaml                             |                                          | VS/Menu.xaml                             |
|                                          |                                          | VS/MenuItem.xaml                         |
|                                          |                                          | VS/ContextMenu.xaml                      |
| VS/ScrollBar.xaml                        |                                          | VS/ScrollBar.xaml                        |
| VS/TabControl.xaml                       |                                          | VS/TabControl.xaml                       |
| VS/TextBox.xaml                          |                                          | VS/TextBox.xaml                          |
| VS/Window.xaml                           |                                          | VS/MetroWindow.xaml                      |
| StandardButton                           | MahApps.Styles.Button.VisualStudio       | VS/Button.xaml                           |
| LinkButton                               | MahApps.Styles.Button.Link.VisualStudio  | VS/Button.xaml                           |
| ExpanderBaseHeaderStyle                  | MahApps.Styles.ToggleButton.ExpanderHeader.Base.VisualStudio | VS/Expander.xaml                         |
| ExpanderRightHeaderStyle                 | MahApps.Styles.ToggleButton.ExpanderHeader.Right.VisualStudio | VS/Expander.xaml                         |
| ExpanderUpHeaderStyle                    | MahApps.Styles.ToggleButton.ExpanderHeader.Up.VisualStudio | VS/Expander.xaml                         |
| ExpanderLeftHeaderStyle                  | MahApps.Styles.ToggleButton.ExpanderHeader.Left.VisualStudio | VS/Expander.xaml                         |
| ExpanderDownHeaderStyle                  | MahApps.Styles.ToggleButton.ExpanderHeader.Down.VisualStudio | VS/Expander.xaml                         |
| MetroExpanderExpandStoryboard            | MahApps.Storyboard.Expander.Expand.VS    | VS/Expander.xaml                         |
| MetroExpanderCollapseStoryboard          | MahApps.Storyboard.Expander.Collapse.VS  | VS/Expander.xaml                         |
| StandardExpander                         | MahApps.Styles.Expander.VisualStudio     | VS/Expander.xaml                         |
| StandardGroupBox                         | MahApps.Styles.GroupBox.VisualStudio     | VS/GroupBox.xaml                         |
| StandardListBox                          | MahApps.Styles.ListBox.VisualStudio      | VS/ListBox.xaml                          |
|                                          | MahApps.Styles.ListBoxItem.VisualStudio  | VS/ListBox.xaml                          |
| StandardMenu                             | MahApps.Styles.Menu.VisualStudio         | VS/Menu.xaml                             |
| StandardMenuItem                         | MahApps.Styles.MenuItem.VisualStudio     | VS/MenuItem.xaml                         |
|                                          | MahApps.Styles.ContextMenu.VisualStudio  | VS/ContextMenu.xaml                      |
| ScrollBarLineButton                      | MahApps.Styles.RepeatButton.ScrollBarLine.VisualStudio | VS/ScrollBar.xaml                        |
| ScrollBarPageButton                      | MahApps.Styles.RepeatButton.ScrollBarPage.VisualStudio | VS/ScrollBar.xaml                        |
| ScrollBarThumb                           | MahApps.Styles.Thumb.ScrollBar.VisualStudio | VS/ScrollBar.xaml                        |
| HorizontalScrollBar                      | MahApps.Templates.ScrollBar.Horizontal.VisualStudio | VS/ScrollBar.xaml                        |
| VerticalScrollBar                        | MahApps.Templates.ScrollBar.Vertical.VisualStudio | VS/ScrollBar.xaml                        |
| StandardScrollBar                        | MahApps.Styles.ScrollBar.VisualStudio    | VS/ScrollBar.xaml                        |
| TabItemFontSize                          | MahApps.Font.Size.TabItem                | VS/TabControl.xaml                       |
| StandardTabControl                       | MahApps.Styles.TabControl.VisualStudio   | VS/TabControl.xaml                       |
| StandardTabItemCloseButtonStyle          | MahApps.Styles.Button.TabItemClose.VisualStudio | VS/TabControl.xaml                       |
| StandardTabItem                          | MahApps.Styles.TabItem.VisualStudio      | VS/TabControl.xaml                       |
| ClosableTabItemTemplate                  | MahApps.Templates.TabItem.Closable.VisualStudio | VS/TabControl.xaml                       |
| WorkspacesTemplate                       | MahApps.Templates.TabControl.Workspaces.VisualStudio | VS/TabControl.xaml                       |
| StandardTextBox                          | MahApps.Styles.TextBox.VisualStudio      | VS/TextBox.xaml                          |
| SearchTextBox                            | MahApps.Styles.TextBox.Search.VisualStudio | VS/TextBox.xaml                          |
| VSWindowStyleKey                         | MahApps.Styles.MetroWindow.VisualStudio  | VS/MetroWindow.xaml                      |
| VSWindowButtonStyle                      | MahApps.Styles.Button.Window.VisualStudio | VS/MetroWindow.xaml                      |
|                                          | MahApps.Styles.Button.Window.Notification.VisualStudio | VS/MetroWindow.xaml                      |
|                                          | MahApps.Styles.TextBox.Window.QuickLaunch.VisualStudio | VS/MetroWindow.xaml                      |

### List of deleted, renamed and new control theme styles

| Old Key / Name                           | New Key / Name                           | Resource Dictionary                   |
|------------------------------------------|------------------------------------------|---------------------------------------|
| MahApps.Metro.Styles.ContentControlEx    | MahApps.Styles.ContentControlEx          | ContentControlEx.xaml                 |
| MahApps.Metro.Styles.MetroThumbContentControl | MahApps.Styles.MetroThumbContentControl  | ContentControlEx.xaml                 |
| FiveMinuteIndicator                      | MahApps.Templates.DateTimePicker.FiveMinuteIndicator | DateTimePicker.xaml                   |
| MinuteIndicator                          | MahApps.Templates.DateTimePicker.MinuteIndicator | DateTimePicker.xaml                   |
| TimePartPickerBase                       | MahApps.Styles.TimePickerBase            | DateTimePicker.xaml                   |
| SplitButtonHorizontal                    | -- deleted --                            |                                       |
| SplitButtonVertical                      | -- deleted --                            |                                       |
| ButtonDropDownFocusVisual                | MahApps.Styles.DropDownButton.FocusVisualStyle | DropDownButton.xaml                   |
|                                          | MahApps.Styles.FlipView                  | FlipView.xaml                         |
| FlipViewTemplate                         | MahApps.Templates.FlipView               | FlipView.xaml                         |
| ControlButton                            | MahApps.Styles.Button.FlipView.Navigation | FlipView.xaml                         |
|                                          | MahApps.Styles.Flyout                    | Flyout.xaml                           |
| HeaderTemplate                           | MahApps.Templates.Flyout.Header          | Flyout.xaml                           |
| FlyoutTemplate                           | MahApps.Templates.Flyout                 | Flyout.xaml                           |
| MahApps.Metro.Styles.HamburgerMenu       | MahApps.Styles.HamburgerMenu             | HamburgerMenu.xaml                    |
| HamburgerButtonStyle                     | MahApps.Styles.Button.Hamburger          | HamburgerMenuTemplate.xaml            |
| HamburgerScrollViewerStyle               | MahApps.Styles.ScrollViewer.Hamburger    | HamburgerMenuTemplate.xaml            |
| HamburgerMenuItemFocusVisualTemplate     | MahApps.Templates.HamburgerMenuItem.FocusVisual | HamburgerMenuTemplate.xaml            |
|                                          | MahApps.Styles.ListBoxItem.HamburgerBase | HamburgerMenuTemplate.xaml            |
| HamburgerMenuItemStyle                   | MahApps.Styles.ListBoxItem.HamburgerMenuItem | HamburgerMenuTemplate.xaml            |
|                                          | MahApps.Styles.TextBlock.HamburgerMenuHeader | HamburgerMenuTemplate.xaml            |
|                                          | MahApps.Styles.ListBoxItem.HamburgerMenuHeader | HamburgerMenuTemplate.xaml            |
|                                          | MahApps.Styles.ListBoxItem.HamburgerMenuSeparator | HamburgerMenuTemplate.xaml            |
| HamburgerListBoxItemStyle                | -- deleted --                            |                                       |
| HamburgerMenuListStyle                   | MahApps.Styles.ListBox.HamburgerMenu     | HamburgerMenuTemplate.xaml            |
| HamburgerListBoxStyle                    | -- deleted --                            |                                       |
| HamburgerMenuTemplate                    | MahApps.Templates.HamburgerMenu          | HamburgerMenuTemplate.xaml            |
|                                          | MahApps.Styles.ListBoxItem.HamburgerMenuItem.CreatorsUpdate | HamburgerMenuTemplate.xaml            |
|                                          | MahApps.Styles.HamburgerMenu.CreatorsUpdate | HamburgerMenu.xaml                    |
| HorizontalMetroAnimatedSingleRowTabControl | MahApps.Templates.MetroAnimatedSingleRowTabControl.Horizontal | MetroAnimatedSingleRowTabControl.xaml |
| VerticalMetroAnimatedSingleRowTabControl | MahApps.Templates.MetroAnimatedSingleRowTabControl.Vertical | MetroAnimatedSingleRowTabControl.xaml |
| MahApps.Metro.Styles.MetroHeader         | MahApps.Styles.MetroHeader               | MetroHeader.xaml                      |
| MahApps.Metro.Styles.MetroProgressBar    | MahApps.Styles.MetroProgressBar          | MetroProgressBar.xaml                 |
|                                          | MahApps.Templates.MetroTabControl.KeepVisualTreeInMemory | MetroTabControl.xaml                  |
|                                          | MahApps.Templates.MetroTabControl.DoNotKeepVisualTreeInMemory | MetroTabControl.xaml                  |
| WindowTemplateKey                        | MahApps.Templates.MetroWindow            | MetroWindow.xaml                      |
| CenterWindowTemplateKey                  | MahApps.Templates.MetroWindow.Center     | MetroWindow.xaml                      |
| PivotHeaderTemplate                      | MahApps.Templates.PivotHeader            | Pivot.xaml                            |
| PivotListViewItem                        | MahApps.Styles.ListViewItem.Pivot        | Pivot.xaml                            |
| MahApps.Metro.Styles.RangeSlider.HorizontalMiddleThumb | MahApps.Styles.MetroThumb.RangeSlider.Horizontal.Middle | RangeSlider.xaml                      |
| MahApps.Metro.Styles.RangeSlider.VerticalMiddleThumb | MahApps.Styles.MetroThumb.RangeSlider.Vertical.Middle | RangeSlider.xaml                      |
| MahApps.Metro.Styles.RangeSlider.HorizontalTemplate | MahApps.Templates.RangeSlider.Horizontal | RangeSlider.xaml                      |
| MahApps.Metro.Styles.RangeSlider.VerticalTemplate | MahApps.Templates.RangeSlider.Vertical   | RangeSlider.xaml                      |
| MahApps.Metro.Styles.RangeSlider         | MahApps.Styles.RangeSlider               | RangeSlider.xaml                      |
| MahApps.Metro.Styles.RangeSlider.HorizontalMiddleThumb.Win10 | MahApps.Styles.MetroThumb.RangeSlider.Horizontal.Middle.Win10 | RangeSlider.xaml                      |
| MahApps.Metro.Styles.RangeSlider.VerticalMiddleThumb.Win10 | MahApps.Styles.MetroThumb.RangeSlider.Vertical.Middle.Win10 | RangeSlider.xaml                      |
| MahApps.Metro.Styles.RangeSlider.HorizontalTemplate.Win10 | MahApps.Templates.RangeSlider.Horizontal.Win10 | RangeSlider.xaml                      |
| MahApps.Metro.Styles.RangeSlider.VerticalTemplate.Win10 | MahApps.Templates.RangeSlider.Vertical.Win10 | RangeSlider.xaml                      |
| MahApps.Metro.Styles.RangeSlider.Win10   | MahApps.Styles.RangeSlider.Win10         | RangeSlider.xaml                      |
| SplitButtonHorizontal                    | MahApps.Templates.SplitButton.Horizontal | SplitButton.xaml                      |
| SplitButtonVertical                      | MahApps.Templates.SplitButton.Vertical   | SplitButton.xaml                      |
| ButtonSplitFocusVisual                   | MahApps.Styles.SplitButton.FocusVisualStyle | SplitButton.xaml                      |
| WindowTitleThumbStyle                    | MahApps.Styles.Thumb.WindowTitle         | Thumb.xaml                            |
| MahApps.Metro.Styles.ToggleSwitchButton  | -- deleted --                            |                                       |
| MahApps.Metro.Styles.ToggleSwitch        | MahApps.Styles.ToggleSwitch              | ToggleSwitch.xaml                     |
| WindowButtonCommandsTemplate             | MahApps.Templates.WindowButtonCommands   | WindowButtonCommands.xaml             |
| MahApps.Metro.Templates.WindowButtonCommands.Win10 | MahApps.Templates.WindowButtonCommands.Win10 | WindowButtonCommands.xaml             |
| MahApps.Metro.Styles.WindowButtonCommands | MahApps.Styles.WindowButtonCommands.Base | WindowButtonCommands.xaml             |
|                                          | MahApps.Styles.WindowButtonCommands      | WindowButtonCommands.xaml             |
| MahApps.Metro.Styles.WindowButtonCommands.Win10 | MahApps.Styles.WindowButtonCommands.Win10 | WindowButtonCommands.xaml             |
| WindowCommandsButtonTemplate             | MahApps.Templates.Button.WindowCommands  | WindowCommands.xaml                   |
| WindowCommandsToggleButtonTemplate       | MahApps.Templates.ToggleButton.WindowCommands | WindowCommands.xaml                   |
|                                          | MahApps.Styles.Control.WindowCommands    | WindowCommands.xaml                   |
|                                          | MahApps.Styles.Button.WindowCommands     | WindowCommands.xaml                   |
|                                          | MahApps.Styles.ToggleButton.WindowCommands | WindowCommands.xaml                   |
|                                          | MahApps.Styles.SplitButton.Button.WindowCommands | WindowCommands.xaml                   |
|                                          | MahApps.Styles.SplitButton.ButtonArrow.WindowCommands | WindowCommands.xaml                   |
|                                          | MahApps.Styles.SplitButton.WindowCommands | WindowCommands.xaml                   |
|                                          | MahApps.Styles.DropDownButton.Button.WindowCommands | WindowCommands.xaml                   |
|                                          | MahApps.Styles.DropDownButton.WindowCommands | WindowCommands.xaml                   |
|                                          | MahApps.Styles.ToggleButton.ToolBarOverflow | WindowCommands.xaml                   |
|                                          | MahApps.Templates.WindowCommands         | WindowCommands.xaml                   |
|                                          | MahApps.Styles.WindowCommandsItem        | WindowCommands.xaml                   |
|                                          | MahApps.Styles.WindowCommands.Base       | WindowCommands.xaml                   |
| WindowCommandsControlStyle               | MahApps.Styles.WindowCommands            | WindowCommands.xaml                   |
| LightWindowCommandsTemplate              | -- deleted --                            |                                       |
| DarkWindowCommandsTemplate               | -- deleted --                            |                                       |
|                                          | MahApps.Styles.Button.Dialogs            | BaseMetroDialog.xaml                  |
| AccentedDialogSquareButton               | MahApps.Styles.Button.Dialogs.Accent     | BaseMetroDialog.xaml                  |
| AccentedDialogHighlightedSquareButton    | MahApps.Styles.Button.Dialogs.AccentHighlight | BaseMetroDialog.xaml                  |
| DialogShownStoryboard                    | MahApps.Storyboard.Dialogs.Show          | BaseMetroDialog.xaml                  |
| DialogCloseStoryboard                    | MahApps.Storyboard.Dialogs.Close         | BaseMetroDialog.xaml                  |
| MetroDialogTemplate                      | MahApps.Templates.BaseMetroDialog        | BaseMetroDialog.xaml                  |
| MahApps.Metro.Styles.MetroDialog         | MahApps.Styles.BaseMetroDialog           | BaseMetroDialog.xaml                  |

