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
