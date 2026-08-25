using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace MessageDialogShots
{
    // Renders message dialogs to PNG. Unlike the other generators these need a
    // live MetroWindow: a dialog is not a control you can lay out, it is shown
    // into the window's overlay. So each scenario opens a real window off
    // screen, starts ShowMessageAsync without awaiting the user's answer, lets
    // the dialog settle and then renders the window.
    public static class Program
    {
        private const double Scale = 2.0;
        private static string outputDirectory;

        [STAThread]
        public static void Main(string[] args)
        {
            outputDirectory = Array.Find(args, a => !a.StartsWith("--")) ?? "shots";
            System.IO.Directory.CreateDirectory(outputDirectory);

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
                        await CaptureAllAsync();
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

        private static async Task CaptureAllAsync()
        {
            await CaptureAsync("messagedialog-styles", 520, 340,
                ("MessageDialogStyle.Affirmative",
                    "Delete the file?", "This cannot be undone.",
                    MessageDialogStyle.Affirmative, null),
                ("MessageDialogStyle.AffirmativeAndNegative",
                    "Delete the file?", "This cannot be undone.",
                    MessageDialogStyle.AffirmativeAndNegative, null));

            await CaptureAsync("messagedialog-auxiliary", 700, 340,
                ("...AndSingleAuxiliary",
                    "Unsaved changes", "Save your changes before closing?",
                    MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary,
                    new MetroDialogSettings
                    {
                        AffirmativeButtonText = "Save",
                        NegativeButtonText = "Discard",
                        FirstAuxiliaryButtonText = "Cancel"
                    }),
                ("...AndDoubleAuxiliary",
                    "Unsaved changes", "Save your changes before closing?",
                    MessageDialogStyle.AffirmativeAndNegativeAndDoubleAuxiliary,
                    new MetroDialogSettings
                    {
                        AffirmativeButtonText = "Save",
                        NegativeButtonText = "Discard",
                        FirstAuxiliaryButtonText = "Save as...",
                        SecondAuxiliaryButtonText = "Cancel"
                    }));

            await CaptureAsync("messagedialog-colorscheme", 520, 340,
                ("ColorScheme = Theme (default)",
                    "Connection lost", "Retry the request?",
                    MessageDialogStyle.AffirmativeAndNegative,
                    new MetroDialogSettings { ColorScheme = MetroDialogColorScheme.Theme }),
                ("ColorScheme = Accented",
                    "Connection lost", "Retry the request?",
                    MessageDialogStyle.AffirmativeAndNegative,
                    new MetroDialogSettings { ColorScheme = MetroDialogColorScheme.Accented }));

            await CaptureAsync("messagedialog-custom", 560, 340,
                ("Custom button text",
                    "Leave the page?", "Your draft will be kept for seven days.",
                    MessageDialogStyle.AffirmativeAndNegative,
                    new MetroDialogSettings
                    {
                        AffirmativeButtonText = "Leave",
                        NegativeButtonText = "Stay here"
                    }),
                ("Custom font sizes",
                    "Leave the page?", "Your draft will be kept for seven days.",
                    MessageDialogStyle.AffirmativeAndNegative,
                    new MetroDialogSettings
                    {
                        AffirmativeButtonText = "Leave",
                        NegativeButtonText = "Stay here",
                        DialogTitleFontSize = 22,
                        DialogMessageFontSize = 14,
                        DialogButtonFontSize = 14
                    }));
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

        private static async Task<(MetroWindow Window, FrameworkElement Root)> OpenAsync(
            string title, string message, MessageDialogStyle style, MetroDialogSettings settings,
            double width, double height)
        {
            settings = settings ?? new MetroDialogSettings();
            // Animations would make the capture a race.
            settings.AnimateShow = false;
            settings.AnimateHide = false;

            var window = new MetroWindow
                         {
                             Title = "MainWindow",
                             Width = width,
                             Height = height,
                             ShowInTaskbar = false,
                             WindowStartupLocation = WindowStartupLocation.Manual,
                             Left = -20000,
                             Top = -20000,
                             Content = Backdrop()
                         };

            var rendered = new TaskCompletionSource<bool>();
            window.ContentRendered += (_, _) => rendered.TrySetResult(true);
            window.Show();
            await rendered.Task;

            // Deliberately not awaited: the task completes when a button is
            // clicked, which never happens here.
            _ = window.ShowMessageAsync(title, message, style, settings);

            await Task.Delay(700);
            await window.Dispatcher.InvokeAsync(() => { }, DispatcherPriority.ContextIdle);

            return (window, window);
        }

        private static async Task CaptureAsync(
            string name,
            double width,
            double height,
            params (string Caption, string Title, string Message, MessageDialogStyle Style, MetroDialogSettings Settings)[] scenarios)
        {
            var row = new StackPanel { Orientation = Orientation.Horizontal };
            var windows = new System.Collections.Generic.List<MetroWindow>();

            foreach (var scenario in scenarios)
            {
                var (window, _) = await OpenAsync(scenario.Title, scenario.Message, scenario.Style, scenario.Settings, width, height);
                windows.Add(window);

                // Render each window on its own, then compose the panels into
                // one image - a Window cannot be reparented into a StackPanel.
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

            var composed = new Border
                           {
                               Background = new SolidColorBrush(Color.FromRgb(0xF8, 0xF9, 0xFA)),
                               Padding = new Thickness(6),
                               Child = row
                           };

            await SaveComposedAsync(name, composed);

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

            var path = System.IO.Path.Combine(outputDirectory, name + ".png");
            using (var stream = System.IO.File.Create(path))
            {
                encoder.Save(stream);
            }

            Console.WriteLine($"{name}.png  {bitmap.PixelWidth}x{bitmap.PixelHeight}");
            host.Close();
        }
    }
}
