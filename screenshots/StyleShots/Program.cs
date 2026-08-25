using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace StyleShots
{
    // Renders the control styles straight to PNG with RenderTargetBitmap. Like
    // HamburgerMenuShots the scenarios are written as XAML and loaded through
    // XamlReader, so what the figure shows and what the documentation prints
    // are the same markup.
    //
    // This app covers the pages under input/docs/styles/. Adding a figure for
    // another control means adding a scenario to CaptureAllAsync.
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

        private const string Xmlns =
            "xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" " +
            "xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" " +
            "xmlns:mah=\"http://metro.mahapps.com/winfx/xaml/controls\"";

        // PasswordBox.Password is not a dependency property, so it cannot be
        // set from the XAML the scenarios are written in - and setting it
        // before the style is applied would happen before
        // PasswordBoxBindingBehavior is attached, which is what feeds the
        // revealed-password text box the reveal button's visibility hangs off.
        // Both problems go away by filling the boxes once the window is up.
        private static readonly List<(PasswordBox Box, string Password)> pendingPasswords = new();

        // Same for the caps lock indicator: the library shows it by looking the
        // template part up and setting its visibility, from a key handler that
        // an off-screen render never triggers. Driving the same part directly
        // is the only way to photograph the state.
        private static readonly List<PasswordBox> pendingCapsLock = new();

        private static FrameworkElement Box(string attributes, string password = null, bool capsLock = false, double width = 190)
        {
            var box = (PasswordBox)XamlReader.Parse($@"<PasswordBox {Xmlns} Width=""{width}"" {attributes} />");

            if (password != null)
            {
                pendingPasswords.Add((box, password));
            }

            if (capsLock)
            {
                pendingCapsLock.Add(box);
            }

            return box;
        }

        private static async Task CaptureAllAsync()
        {
            const string secret = "sw0rdf1sh";

            await CaptureAsync("passwordbox-styles",
                Showcase(
                    ("implicit style", Box(string.Empty, secret)),
                    ("MahApps.Styles.PasswordBox.Button",
                        Box(@"Style=""{StaticResource MahApps.Styles.PasswordBox.Button}""", secret)),
                    ("MahApps.Styles.PasswordBox.Button.Revealed",
                        Box(@"Style=""{StaticResource MahApps.Styles.PasswordBox.Button.Revealed}""", secret)),
                    ("MahApps.Styles.PasswordBox.Win8",
                        Box(@"Style=""{StaticResource MahApps.Styles.PasswordBox.Win8}""", secret))));

            await CaptureAsync("passwordbox-watermark",
                Showcase(
                    ("Watermark, empty",
                        Box(@"mah:TextBoxHelper.Watermark=""Password""")),
                    ("Watermark, filled",
                        Box(@"mah:TextBoxHelper.Watermark=""Password""", secret)),
                    ("UseFloatingWatermark",
                        Box(@"mah:TextBoxHelper.Watermark=""Password"" mah:TextBoxHelper.UseFloatingWatermark=""True""", secret))));

            await CaptureAsync("passwordbox-clearbutton",
                Showcase(
                    ("ClearTextButton",
                        Box(@"mah:TextBoxHelper.ClearTextButton=""True""", secret)),
                    ("ButtonsAlignment=Left",
                        Box(@"mah:TextBoxHelper.ClearTextButton=""True"" mah:TextBoxHelper.ButtonsAlignment=""Left""", secret)),
                    ("ButtonContent + ButtonWidth",
                        Box(@"mah:TextBoxHelper.ClearTextButton=""True"" mah:TextBoxHelper.ButtonContent=""Clear"" mah:TextBoxHelper.ButtonFontFamily=""{DynamicResource MahApps.Fonts.Family.Control}"" mah:TextBoxHelper.ButtonFontSize=""12"" mah:TextBoxHelper.ButtonWidth=""48""", secret))));

            await CaptureAsync("passwordbox-capslock",
                Showcase(
                    ("default CapsLockIcon", Box(string.Empty, secret, capsLock: true)),
                    ("custom CapsLockIcon",
                        Box(@"mah:PasswordBoxHelper.CapsLockIcon=""CAPS""", secret, capsLock: true))));
        }

        private static FrameworkElement Showcase(params (string Caption, FrameworkElement View)[] items)
        {
            var row = new StackPanel { Orientation = Orientation.Horizontal };

            foreach (var (caption, view) in items)
            {
                var column = new StackPanel { Margin = new Thickness(10) };
                column.Children.Add(new TextBlock
                                    {
                                        Text = caption,
                                        FontSize = 12,
                                        FontWeight = FontWeights.SemiBold,
                                        Margin = new Thickness(2, 0, 2, 6),
                                        Foreground = new SolidColorBrush(Color.FromRgb(0x49, 0x50, 0x57))
                                    });
                view.VerticalAlignment = VerticalAlignment.Center;
                column.Children.Add(new Grid { Height = 60, Children = { view } });
                row.Children.Add(column);
            }

            return new Border
                   {
                       Background = new SolidColorBrush(Color.FromRgb(0xF8, 0xF9, 0xFA)),
                       Padding = new Thickness(6),
                       Child = row
                   };
        }

        private static async Task CaptureAsync(string name, FrameworkElement root)
        {
            var window = new Window
                         {
                             Content = root,
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
            window.ContentRendered += (_, _) => rendered.TrySetResult(true);
            window.Show();
            await rendered.Task;

            foreach (var (box, password) in pendingPasswords)
            {
                box.Password = password;
            }

            foreach (var box in pendingCapsLock)
            {
                box.ApplyTemplate();
                if (box.Template?.FindName("PART_CapsLockIndicator", box) is FrameworkElement indicator)
                {
                    indicator.Visibility = Visibility.Visible;
                }
            }

            pendingPasswords.Clear();
            pendingCapsLock.Clear();

            // Nothing here animates, but the floating watermark and the reveal
            // button appear through the bindings above; let them settle.
            await Task.Delay(400);

            // Whether a box wears the focus border depends on where keyboard
            // focus landed, which is not something the scenario says. Clear it.
            Keyboard.ClearFocus();

            await window.Dispatcher.InvokeAsync(() => { }, DispatcherPriority.ContextIdle);

            root.UpdateLayout();

            var width = (int)Math.Ceiling(root.ActualWidth * Scale);
            var height = (int)Math.Ceiling(root.ActualHeight * Scale);
            var bitmap = new RenderTargetBitmap(width, height, 96 * Scale, 96 * Scale, PixelFormats.Pbgra32);
            bitmap.Render(root);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            var path = Path.Combine(outputDirectory, name + ".png");
            using (var stream = File.Create(path))
            {
                encoder.Save(stream);
            }

            Console.WriteLine($"{name}.png  {width}x{height}");
            window.Close();
        }
    }
}
