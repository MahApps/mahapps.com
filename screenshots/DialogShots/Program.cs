using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace DialogShots
{
    // Renders the dialog figures for the docs/dialogs pages.
    //
    // A dialog is not a control that can be laid out on a canvas - it is shown
    // into a MetroWindow's overlay. So each scenario opens a real window off
    // screen, starts the dialog without awaiting the answer that never comes,
    // renders the window once it has settled, and the renders are composed into
    // one image per figure.
    public static class Program
    {
        private const double Scale = 2.0;
        private static string outputDirectory;

        [STAThread]
        public static void Main(string[] args)
        {
            outputDirectory = Array.Find(args, a => !a.StartsWith("--")) ?? "shots";
            Directory.CreateDirectory(outputDirectory);

            var app = new Application { ShutdownMode = ShutdownMode.OnExplicitShutdown };
            foreach (var source in new[]
                     {
                         "pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml",
                         "pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml",
                         "pack://application:,,,/MahApps.Metro;component/Styles/Themes/Light.Blue.xaml"
                     })
            {
                app.Resources.MergedDictionaries.Add(
                    new ResourceDictionary { Source = new Uri(source, UriKind.Absolute) });
            }

            app.Startup += async (_, _) =>
                {
                    try
                    {
                        await MessageDialogFiguresAsync();
                        await InputDialogFiguresAsync();
                        await LoginDialogFiguresAsync();
                        await ProgressDialogFiguresAsync();
                        await DialogSettingsFiguresAsync();
                        await CustomDialogFiguresAsync();
                        Console.WriteLine("done");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("FAILED: " + ex);
                    }
                    finally
                    {
                        app.Shutdown();
                    }
                };

            app.Run();
        }

        // Animating would turn the capture into a race.
        private static MetroDialogSettings Still(MetroDialogSettings settings = null)
        {
            settings = settings ?? new MetroDialogSettings();
            settings.AnimateShow = false;
            settings.AnimateHide = false;
            return settings;
        }

        private static async Task MessageDialogFiguresAsync()
        {
            await CaptureAsync("messagedialog-styles", 520, 340,
                ("MessageDialogStyle.Affirmative",
                    w => w.ShowMessageAsync("Delete the file?", "This cannot be undone.",
                        MessageDialogStyle.Affirmative, Still())),
                ("MessageDialogStyle.AffirmativeAndNegative",
                    w => w.ShowMessageAsync("Delete the file?", "This cannot be undone.",
                        MessageDialogStyle.AffirmativeAndNegative, Still())));

            await CaptureAsync("messagedialog-auxiliary", 700, 340,
                ("...AndSingleAuxiliary",
                    w => w.ShowMessageAsync("Unsaved changes", "Save your changes before closing?",
                        MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary,
                        Still(new MetroDialogSettings
                              {
                                  AffirmativeButtonText = "Save",
                                  NegativeButtonText = "Discard",
                                  FirstAuxiliaryButtonText = "Cancel"
                              }))),
                ("...AndDoubleAuxiliary",
                    w => w.ShowMessageAsync("Unsaved changes", "Save your changes before closing?",
                        MessageDialogStyle.AffirmativeAndNegativeAndDoubleAuxiliary,
                        Still(new MetroDialogSettings
                              {
                                  AffirmativeButtonText = "Save",
                                  NegativeButtonText = "Discard",
                                  FirstAuxiliaryButtonText = "Save as...",
                                  SecondAuxiliaryButtonText = "Cancel"
                              }))));

            await CaptureAsync("messagedialog-colorscheme", 520, 340,
                ("ColorScheme = Theme (default)",
                    w => w.ShowMessageAsync("Connection lost", "Retry the request?",
                        MessageDialogStyle.AffirmativeAndNegative,
                        Still(new MetroDialogSettings { ColorScheme = MetroDialogColorScheme.Theme }))),
                ("ColorScheme = Accented",
                    w => w.ShowMessageAsync("Connection lost", "Retry the request?",
                        MessageDialogStyle.AffirmativeAndNegative,
                        Still(new MetroDialogSettings { ColorScheme = MetroDialogColorScheme.Accented }))));

            await CaptureAsync("messagedialog-custom", 560, 340,
                ("Custom button text",
                    w => w.ShowMessageAsync("Leave the page?", "Your draft will be kept for seven days.",
                        MessageDialogStyle.AffirmativeAndNegative,
                        Still(new MetroDialogSettings
                              {
                                  AffirmativeButtonText = "Leave",
                                  NegativeButtonText = "Stay here"
                              }))),
                ("Custom font sizes",
                    w => w.ShowMessageAsync("Leave the page?", "Your draft will be kept for seven days.",
                        MessageDialogStyle.AffirmativeAndNegative,
                        Still(new MetroDialogSettings
                              {
                                  AffirmativeButtonText = "Leave",
                                  NegativeButtonText = "Stay here",
                                  DialogTitleFontSize = 22,
                                  DialogMessageFontSize = 14,
                                  DialogButtonFontSize = 14
                              }))));
        }

        private static async Task InputDialogFiguresAsync()
        {
            await CaptureAsync("inputdialog-basic", 560, 360,
                ("Empty, with the default buttons",
                    w => w.ShowInputAsync("What is your name?", "This will appear on your profile.",
                        Still())),
                ("DefaultText and custom buttons",
                    w => w.ShowInputAsync("What is your name?", "This will appear on your profile.",
                        Still(new MetroDialogSettings
                              {
                                  DefaultText = "Ada Lovelace",
                                  AffirmativeButtonText = "Save",
                                  NegativeButtonText = "Skip"
                              }))));

            await CaptureAsync("inputdialog-colorscheme", 560, 360,
                ("ColorScheme = Theme (default)",
                    w => w.ShowInputAsync("Rename the file", "Enter a new name.",
                        Still(new MetroDialogSettings { DefaultText = "report.pdf" }))),
                ("ColorScheme = Accented",
                    w => w.ShowInputAsync("Rename the file", "Enter a new name.",
                        Still(new MetroDialogSettings
                              {
                                  DefaultText = "report.pdf",
                                  ColorScheme = MetroDialogColorScheme.Accented
                              }))));
        }

        // LoginDialogSettings derives from MetroDialogSettings, so it needs the
        // same animation treatment.
        private static LoginDialogSettings Still(LoginDialogSettings settings)
        {
            settings.AnimateShow = false;
            settings.AnimateHide = false;
            return settings;
        }

        private static async Task LoginDialogFiguresAsync()
        {
            await CaptureAsync("logindialog-basic", 560, 420,
                ("Defaults",
                    w => w.ShowLoginAsync("Sign in", "Enter your credentials.",
                        Still(new LoginDialogSettings()))),
                ("Cancel button and remember box",
                    w => w.ShowLoginAsync("Sign in", "Enter your credentials.",
                        Still(new LoginDialogSettings
                              {
                                  NegativeButtonVisibility = Visibility.Visible,
                                  NegativeButtonText = "Cancel",
                                  RememberCheckBoxVisibility = Visibility.Visible,
                                  RememberCheckBoxText = "Keep me signed in"
                              }))));

            await CaptureAsync("logindialog-variants", 560, 420,
                ("ShouldHideUsername",
                    w => w.ShowLoginAsync("Unlock", "Enter your password to continue.",
                        Still(new LoginDialogSettings
                              {
                                  ShouldHideUsername = true,
                                  AffirmativeButtonText = "Unlock"
                              }))),
                ("EnablePasswordPreview",
                    w => w.ShowLoginAsync("Sign in", "Enter your credentials.",
                        Still(new LoginDialogSettings
                              {
                                  InitialUsername = "ada",
                                  InitialPassword = "analytical",
                                  EnablePasswordPreview = true
                              }))));
        }

        private static async Task ProgressDialogFiguresAsync()
        {
            await CaptureAsync("progressdialog-basic", 560, 360,
                ("Defaults",
                    async w =>
                        {
                            var controller = await w.ShowProgressAsync("Copying files", "3 of 12 files copied", settings: Still());
                            controller.SetProgress(0.25);
                        }),
                ("isCancelable: true",
                    async w =>
                        {
                            var controller = await w.ShowProgressAsync("Copying files", "3 of 12 files copied", true, Still());
                            controller.SetProgress(0.25);
                        }));

            // The indeterminate bar animates, so this is the one figure whose
            // pixels differ between runs.
            await CaptureAsync("progressdialog-indeterminate", 560, 360,
                ("SetIndeterminate()",
                    async w =>
                        {
                            var controller = await w.ShowProgressAsync("Connecting", "Waiting for the server...", settings: Still());
                            controller.SetIndeterminate();
                        }));

            await CaptureAsync("progressdialog-canceled", 560, 360,
                ("After Cancel was pressed",
                    async w =>
                        {
                            var controller = await w.ShowProgressAsync("Copying files", "3 of 12 files copied", true, Still());
                            controller.SetProgress(0.25);
                            await PressCancelAsync(w);
                        }));

            await CaptureAsync("progressdialog-colorscheme", 560, 360,
                ("ColorScheme = Theme",
                    async w =>
                        {
                            var controller = await w.ShowProgressAsync("Copying files", "3 of 12 files copied", true,
                                Still(new MetroDialogSettings { ColorScheme = MetroDialogColorScheme.Theme }));
                            controller.SetProgress(0.25);
                        }),
                ("ColorScheme = Accented",
                    async w =>
                        {
                            var controller = await w.ShowProgressAsync("Copying files", "3 of 12 files copied", true,
                                Still(new MetroDialogSettings { ColorScheme = MetroDialogColorScheme.Accented }));
                            controller.SetProgress(0.25);
                        }));
        }

        private const string DialogXmlns =
            "xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" " +
            "xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" " +
            "xmlns:mah=\"http://metro.mahapps.com/winfx/xaml/controls\"";

        // The dialogs on the custom dialogs page, written as the XAML the page
        // prints rather than assembled in C#, so the figure and the sample
        // cannot drift apart.
        private static CustomDialog Dialog(string inner)
        {
            return (CustomDialog)System.Windows.Markup.XamlReader.Parse(
                $"<mah:CustomDialog {DialogXmlns}>{inner}</mah:CustomDialog>");
        }

        private const string UpdateDialog = @"
  <mah:CustomDialog.DialogTop>
    <Border Padding=""20 14"" Background=""{DynamicResource MahApps.Brushes.Accent}"">
      <StackPanel Orientation=""Horizontal"">
        <TextBlock VerticalAlignment=""Center"" FontFamily=""Segoe MDL2 Assets"" FontSize=""18""
                   Foreground=""{DynamicResource MahApps.Brushes.IdealForeground}"" Text=""&#xE896;"" />
        <TextBlock Margin=""12 0 0 0"" VerticalAlignment=""Center"" FontSize=""14""
                   Foreground=""{DynamicResource MahApps.Brushes.IdealForeground}"" Text=""Version 2.5.0 is ready to install"" />
      </StackPanel>
    </Border>
  </mah:CustomDialog.DialogTop>

  <StackPanel Margin=""0 4 0 20"">
    <TextBlock Margin=""0 0 0 12"" FontSize=""15"" Opacity=""0.75"" TextWrapping=""Wrap""
               Text=""This release fixes the crash on startup and adds two things you asked for."" />
    <StackPanel Margin=""0 0 0 4"" Orientation=""Horizontal"">
      <TextBlock Width=""22"" FontFamily=""Segoe MDL2 Assets"" FontSize=""12""
                 Foreground=""{DynamicResource MahApps.Brushes.Accent}"" Text=""&#xE73E;"" />
      <TextBlock FontSize=""14"" Text=""Dark theme follows the system setting"" />
    </StackPanel>
    <StackPanel Margin=""0 0 0 4"" Orientation=""Horizontal"">
      <TextBlock Width=""22"" FontFamily=""Segoe MDL2 Assets"" FontSize=""12""
                 Foreground=""{DynamicResource MahApps.Brushes.Accent}"" Text=""&#xE73E;"" />
      <TextBlock FontSize=""14"" Text=""Export to CSV"" />
    </StackPanel>
  </StackPanel>

  <mah:CustomDialog.DialogBottom>
    <Border Padding=""20 14"" Background=""{DynamicResource MahApps.Brushes.Gray10}"">
      <StackPanel HorizontalAlignment=""Right"" Orientation=""Horizontal"">
        <Button Content=""Later"" Style=""{DynamicResource MahApps.Styles.Button.Dialogs}"" />
        <Button Margin=""8 0 0 0"" Content=""Install now"" Style=""{DynamicResource MahApps.Styles.Button.Dialogs.Accent}"" />
      </StackPanel>
    </Border>
  </mah:CustomDialog.DialogBottom>";

        private const string ExportDialogBody = @"
  <StackPanel Margin=""0 4 0 20"">
    <TextBlock Margin=""0 0 0 14"" FontSize=""15"" Opacity=""0.75"" Text=""Choose a format and what to include."" />

    <TextBox Margin=""0 0 0 14""
             mah:TextBoxHelper.UseFloatingWatermark=""True""
             mah:TextBoxHelper.Watermark=""File name""
             Text=""quarterly-report"" />

    <RadioButton Margin=""0 0 0 6"" Content=""PDF document"" IsChecked=""True"" />
    <RadioButton Margin=""0 0 0 6"" Content=""Excel workbook"" />
    <RadioButton Margin=""0 0 0 14"" Content=""Comma separated values"" />

    <CheckBox Content=""Include charts"" IsChecked=""True"" />
  </StackPanel>

  <mah:CustomDialog.DialogBottom>
    <Border Padding=""20 14"" Background=""{DynamicResource MahApps.Brushes.Gray10}"">
      <StackPanel HorizontalAlignment=""Right"" Orientation=""Horizontal"">
        <Button Content=""Cancel"" Style=""{DynamicResource MahApps.Styles.Button.Dialogs}"" />
        <Button Margin=""8 0 0 0"" Content=""Export"" Style=""{DynamicResource MahApps.Styles.Button.Dialogs.Accent}"" />
      </StackPanel>
    </Border>
  </mah:CustomDialog.DialogBottom>";

        private static string Account(string initials, string name, string mail, bool selected)
        {
            return $@"
      <ListBoxItem IsSelected=""{selected.ToString().ToLowerInvariant()}"" Padding=""8"">
        <StackPanel Orientation=""Horizontal"">
          <Border Width=""36"" Height=""36"" Background=""{{DynamicResource MahApps.Brushes.Accent}}"" CornerRadius=""18"">
            <TextBlock HorizontalAlignment=""Center"" VerticalAlignment=""Center"" FontSize=""13""
                       Foreground=""{{DynamicResource MahApps.Brushes.IdealForeground}}"" Text=""{initials}"" />
          </Border>
          <StackPanel Margin=""12 0 0 0"" VerticalAlignment=""Center"">
            <TextBlock FontSize=""14"" Text=""{name}"" />
            <TextBlock FontSize=""12"" Opacity=""0.7"" Text=""{mail}"" />
          </StackPanel>
        </StackPanel>
      </ListBoxItem>";
        }

        private static async Task CustomDialogFiguresAsync()
        {
            await CaptureAsync("customdialog-update", 620, 440,
                ("DialogTop, Title, content and DialogBottom",
                    w => w.ShowMetroDialogAsync(
                        Dialog($@"<mah:CustomDialog.Title>Update available</mah:CustomDialog.Title>{UpdateDialog}"),
                        Still())));

            await CaptureAsync("customdialog-form", 620, 520,
                ("A form in a dialog",
                    w => w.ShowMetroDialogAsync(
                        Dialog($@"<mah:CustomDialog.Title>Export report</mah:CustomDialog.Title>{ExportDialogBody}"),
                        Still())));

            await CaptureAsync("customdialog-list", 620, 480,
                ("A list in a dialog",
                    w => w.ShowMetroDialogAsync(
                        Dialog($@"<mah:CustomDialog.Title>Choose an account</mah:CustomDialog.Title>
  <StackPanel Margin=""0 4 0 20"">
    <TextBlock Margin=""0 0 0 12"" FontSize=""15"" Opacity=""0.75"" Text=""You are signed in with more than one account."" />
    <ListBox BorderThickness=""0"">
      {Account("AL", "Ada Lovelace", "ada@example.com", true)}
      {Account("GH", "Grace Hopper", "grace@example.com", false)}
      {Account("AT", "Alan Turing", "alan@example.com", false)}
    </ListBox>
  </StackPanel>
  <mah:CustomDialog.DialogBottom>
    <Border Padding=""20 14"" Background=""{{DynamicResource MahApps.Brushes.Gray10}}"">
      <Button HorizontalAlignment=""Right"" Content=""Cancel"" Style=""{{DynamicResource MahApps.Styles.Button.Dialogs}}"" />
    </Border>
  </mah:CustomDialog.DialogBottom>"),
                        Still())));

            await CaptureAsync("customdialog-contentwidth", 620, 520,
                ("Default, DialogContentWidth 50*",
                    w => w.ShowMetroDialogAsync(
                        Dialog($@"<mah:CustomDialog.Title>Export report</mah:CustomDialog.Title>{ExportDialogBody}"),
                        Still())),
                ("DialogContentWidth 80*",
                    w => w.ShowMetroDialogAsync(
                        Dialog($@"<mah:CustomDialog.Title>Export report</mah:CustomDialog.Title>
  <mah:CustomDialog.DialogContentWidth>80*</mah:CustomDialog.DialogContentWidth>{ExportDialogBody}"),
                        Still())));
        }

        // Figures for the shared MetroDialogSettings page. A message dialog is
        // the vehicle; the point of each figure is the setting, not the type.
        private static async Task DialogSettingsFiguresAsync()
        {
            await CaptureAsync("dialogsettings-colorscheme", 520, 340,
                ("Theme",
                    w => w.ShowMessageAsync("Delete the file?", "This cannot be undone.",
                        MessageDialogStyle.AffirmativeAndNegative,
                        Still(new MetroDialogSettings { ColorScheme = MetroDialogColorScheme.Theme }))),
                ("Inverted",
                    w => w.ShowMessageAsync("Delete the file?", "This cannot be undone.",
                        MessageDialogStyle.AffirmativeAndNegative,
                        Still(new MetroDialogSettings { ColorScheme = MetroDialogColorScheme.Inverted }))),
                ("Accented",
                    w => w.ShowMessageAsync("Delete the file?", "This cannot be undone.",
                        MessageDialogStyle.AffirmativeAndNegative,
                        Still(new MetroDialogSettings { ColorScheme = MetroDialogColorScheme.Accented }))));

            await CaptureAsync("dialogsettings-fontsizes", 520, 340,
                ("Defaults: 26 / 15 / 12",
                    w => w.ShowMessageAsync("Delete the file?", "This cannot be undone.",
                        MessageDialogStyle.AffirmativeAndNegative, Still())),
                ("18 / 13 / 14",
                    w => w.ShowMessageAsync("Delete the file?", "This cannot be undone.",
                        MessageDialogStyle.AffirmativeAndNegative,
                        Still(new MetroDialogSettings
                              {
                                  DialogTitleFontSize = 18,
                                  DialogMessageFontSize = 13,
                                  DialogButtonFontSize = 14
                              }))));
        }

        // Pressing Cancel is the only way into the state the page warns about -
        // the dialog stays up and only the button is disabled - and the
        // controller does not expose a way to trigger it. The button carries the
        // template part name, so drive the real one.
        private static async Task PressCancelAsync(DependencyObject window)
        {
            await window.Dispatcher.InvokeAsync(() => { }, DispatcherPriority.ContextIdle);

            var button = FindByName<Button>(window, "PART_NegativeButton");
            button?.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));

            await window.Dispatcher.InvokeAsync(() => { }, DispatcherPriority.ContextIdle);
        }

        private static T FindByName<T>(DependencyObject root, string name)
            where T : FrameworkElement
        {
            if (root is T match && match.Name == name)
            {
                return match;
            }

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
            {
                var found = FindByName<T>(VisualTreeHelper.GetChild(root, i), name);
                if (found != null)
                {
                    return found;
                }
            }

            return null;
        }

        // Content behind the dialog, so the overlay that dims it is visible.
        private static UIElement Backdrop()
        {
            return new StackPanel
                   {
                       Margin = new Thickness(24),
                       Children =
                       {
                           new TextBlock
                           {
                               Text = "Your application",
                               FontSize = 18,
                               FontWeight = FontWeights.SemiBold,
                               Margin = new Thickness(0, 0, 0, 8)
                           },
                           new TextBlock
                           {
                               Text = "The dialog is shown inside the window rather than as a separate one.",
                               FontSize = 13,
                               Opacity = 0.7,
                               TextWrapping = TextWrapping.Wrap
                           }
                       }
                   };
        }

        private static async Task<MetroWindow> OpenAsync(Func<MetroWindow, Task> show, double width, double height)
        {
            var window = new MetroWindow
                         {
                             Title = "MainWindow",
                             Width = width,
                             Height = height,
                             ShowInTaskbar = false,
                             // Only one window can be the active one, so with
                             // several panels in a figure the rest would render
                             // with the inactive title bar - and which is which
                             // is not deterministic. Take activation out of the
                             // picture and make both states look the same.
                             ShowActivated = false,
                             WindowStartupLocation = WindowStartupLocation.Manual,
                             Left = -20000,
                             Top = -20000,
                             Content = Backdrop()
                         };
            window.NonActiveWindowTitleBrush = window.WindowTitleBrush;
            window.NonActiveBorderBrush = window.BorderBrush;

            var rendered = new TaskCompletionSource<bool>();
            window.ContentRendered += (_, _) => rendered.TrySetResult(true);
            window.Show();
            await rendered.Task;

            // Deliberately not awaited: the task completes when a button is
            // clicked, which never happens here.
            _ = show(window);

            await Task.Delay(700);

            // Whether a button ends up carrying the dashed focus adorner
            // depends on where keyboard focus landed, which varies between
            // runs. Drop it so the figures are reproducible; the affirmative
            // button is still recognisable by its accent fill.
            System.Windows.Input.Keyboard.ClearFocus();
            System.Windows.Input.FocusManager.SetFocusedElement(window, null);

            await window.Dispatcher.InvokeAsync(() => { }, DispatcherPriority.ContextIdle);

            return window;
        }

        private static async Task CaptureAsync(
            string name,
            double width,
            double height,
            params (string Caption, Func<MetroWindow, Task> Show)[] scenarios)
        {
            var row = new StackPanel { Orientation = Orientation.Horizontal };
            var windows = new List<MetroWindow>();

            foreach (var scenario in scenarios)
            {
                var window = await OpenAsync(scenario.Show, width, height);
                windows.Add(window);

                // Render each window on its own; a Window cannot be reparented
                // into the composition panel.
                var shot = RenderToBitmap(window);

                var column = new StackPanel { Margin = new Thickness(10) };
                column.Children.Add(new TextBlock
                                    {
                                        Text = scenario.Caption,
                                        FontSize = 12,
                                        FontWeight = FontWeights.SemiBold,
                                        Margin = new Thickness(2, 0, 2, 6),
                                        Foreground = new SolidColorBrush(Color.FromRgb(0x49, 0x50, 0x57))
                                    });
                column.Children.Add(new Border
                                    {
                                        BorderBrush = new SolidColorBrush(Color.FromRgb(0xCE, 0xD4, 0xDA)),
                                        BorderThickness = new Thickness(1),
                                        Child = new Image
                                                {
                                                    Source = shot,
                                                    Width = window.ActualWidth,
                                                    Height = window.ActualHeight,
                                                    Stretch = Stretch.Fill
                                                }
                                    });
                row.Children.Add(column);
            }

            await SaveComposedAsync(name,
                new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(0xF8, 0xF9, 0xFA)),
                    Padding = new Thickness(6),
                    Child = row
                });

            foreach (var window in windows)
            {
                window.Close();
            }
        }

        private static RenderTargetBitmap RenderToBitmap(FrameworkElement element)
        {
            element.UpdateLayout();
            var bitmap = new RenderTargetBitmap(
                (int)Math.Ceiling(element.ActualWidth * Scale),
                (int)Math.Ceiling(element.ActualHeight * Scale),
                96 * Scale,
                96 * Scale,
                PixelFormats.Pbgra32);
            bitmap.Render(element);
            return bitmap;
        }

        private static async Task SaveComposedAsync(string name, FrameworkElement composed)
        {
            var host = new Window
                       {
                           Content = composed,
                           SizeToContent = SizeToContent.WidthAndHeight,
                           WindowStyle = WindowStyle.None,
                           ResizeMode = ResizeMode.NoResize,
                           ShowInTaskbar = false,
                           WindowStartupLocation = WindowStartupLocation.Manual,
                           Left = -20000,
                           Top = -20000,
                           Background = Brushes.White
                       };

            var rendered = new TaskCompletionSource<bool>();
            host.ContentRendered += (_, _) => rendered.TrySetResult(true);
            host.Show();
            await rendered.Task;
            await host.Dispatcher.InvokeAsync(() => { }, DispatcherPriority.ContextIdle);

            var bitmap = RenderToBitmap(composed);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            var path = Path.Combine(outputDirectory, name + ".png");
            using (var stream = File.Create(path))
            {
                encoder.Save(stream);
            }

            Console.WriteLine($"{name}.png  {bitmap.PixelWidth}x{bitmap.PixelHeight}");
            host.Close();
        }
    }
}
