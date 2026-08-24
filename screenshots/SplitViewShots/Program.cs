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

namespace SplitViewShots
{
    // Renders a set of SplitView configurations straight to PNG with
    // RenderTargetBitmap. Capturing the desktop instead would drag in window
    // chrome, DPI guesswork and timing races; this way every image is
    // deterministic and cropped exactly to the control.
    public static class Program
    {
        private const double Scale = 2.0; // render at 2x for crisp docs images
        private static string outputDirectory;
        private static bool runProbes;

        [STAThread]
        public static void Main(string[] args)
        {
            runProbes = Array.Exists(args, a => a == "--probe");
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

        // What actually happens when PaneClosing is cancelled?
        private static async Task ProbeCancelAsync()
        {
            var splitView = NewSplitView(SplitViewDisplayMode.Inline, true);
            var closedFired = 0;
            splitView.PaneClosing += (_, e) => e.Cancel = true;
            splitView.PaneClosed += (_, _) => closedFired++;

            var window = new Window
                         {
                             Content = splitView,
                             SizeToContent = SizeToContent.WidthAndHeight,
                             WindowStyle = WindowStyle.None,
                             ShowInTaskbar = false,
                             Left = -20000,
                             Top = -20000
                         };
            var rendered = new TaskCompletionSource<bool>();
            window.ContentRendered += (_, _) => rendered.TrySetResult(true);
            window.Show();
            await rendered.Task;
            await Task.Delay(600);

            splitView.IsPaneOpen = false;
            await Task.Delay(600);

            Console.WriteLine($"PROBE cancel: IsPaneOpen={splitView.IsPaneOpen}, PaneClosed fired={closedFired}");

            // Is the pane still drawn open even though IsPaneOpen says false?
            splitView.UpdateLayout();
            var probeBitmap = new RenderTargetBitmap((int)splitView.ActualWidth, (int)splitView.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            probeBitmap.Render(splitView);
            var probeEncoder = new PngBitmapEncoder();
            probeEncoder.Frames.Add(BitmapFrame.Create(probeBitmap));
            using (var probeStream = File.Create(Path.Combine(outputDirectory, "probe-after-cancel.png")))
            {
                probeEncoder.Save(probeStream);
            }

            // Ask again - does a second close attempt still raise PaneClosing?
            var closingFired = 0;
            splitView.PaneClosing += (_, _) => closingFired++;
            splitView.IsPaneOpen = false;
            await Task.Delay(300);
            Console.WriteLine($"PROBE second close attempt: PaneClosing fired={closingFired}, IsPaneOpen={splitView.IsPaneOpen}");

            window.Close();

            await ProbeCancelWorkaroundAsync();
        }

        // Does restoring IsPaneOpen from the handler put things back in sync?
        private static async Task ProbeCancelWorkaroundAsync()
        {
            var splitView = NewSplitView(SplitViewDisplayMode.Inline, true);
            var closingFired = 0;
            splitView.PaneClosing += (s, e) =>
                {
                    closingFired++;
                    e.Cancel = true;
                    var sv = (SplitView)s;
                    sv.Dispatcher.BeginInvoke(new Action(() => sv.SetCurrentValue(SplitView.IsPaneOpenProperty, true)));
                };

            var window = new Window
                         {
                             Content = splitView,
                             SizeToContent = SizeToContent.WidthAndHeight,
                             WindowStyle = WindowStyle.None,
                             ShowInTaskbar = false,
                             Left = -20000,
                             Top = -20000
                         };
            var rendered = new TaskCompletionSource<bool>();
            window.ContentRendered += (_, _) => rendered.TrySetResult(true);
            window.Show();
            await rendered.Task;
            await Task.Delay(600);

            splitView.IsPaneOpen = false;
            await Task.Delay(600);
            Console.WriteLine($"PROBE workaround: after 1st cancel IsPaneOpen={splitView.IsPaneOpen} (closing fired={closingFired})");

            splitView.IsPaneOpen = false;
            await Task.Delay(600);
            Console.WriteLine($"PROBE workaround: after 2nd attempt IsPaneOpen={splitView.IsPaneOpen} (closing fired={closingFired})");

            window.Close();
        }

        private static async Task CaptureAllAsync()
        {
            if (runProbes)
            {
                await ProbeCancelAsync();
            }

            await CaptureAsync("splitview-anatomy",
                Showcase(("Pane + Content, DisplayMode=Inline, IsPaneOpen=True",
                    NewSplitView(SplitViewDisplayMode.Inline, true, annotate: true))));

            foreach (var mode in new[]
                     {
                         SplitViewDisplayMode.Inline,
                         SplitViewDisplayMode.Overlay,
                         SplitViewDisplayMode.CompactInline,
                         SplitViewDisplayMode.CompactOverlay
                     })
            {
                await CaptureAsync("splitview-displaymode-" + mode.ToString().ToLowerInvariant(),
                    Showcase(
                        ("IsPaneOpen=False", NewSplitView(mode, false)),
                        ("IsPaneOpen=True", NewSplitView(mode, true))));
            }

            await CaptureAsync("splitview-paneplacement",
                Showcase(
                    ("PanePlacement=Left", NewSplitView(SplitViewDisplayMode.CompactInline, true)),
                    ("PanePlacement=Right", NewSplitView(SplitViewDisplayMode.CompactInline, true,
                        placement: SplitViewPanePlacement.Right))));

            // The stock resize thumb is 3px wide and has a Transparent
            // background, so only the cursor gives it away - which a still
            // image cannot show. Contrast it with a styled one.
            await CaptureAsync("splitview-canresizeopenpane",
                Showcase(
                    ("Default ResizeThumbStyle (invisible, 3px)",
                        NewSplitView(SplitViewDisplayMode.Inline, true, canResize: true)),
                    ("Custom ResizeThumbStyle",
                        NewSplitView(SplitViewDisplayMode.Inline, true, canResize: true,
                            resizeThumbStyle: VisibleResizeThumbStyle()))));

            await CaptureAsync("splitview-overlaybrush",
                Showcase(
                    ("OverlayBrush not set (default Transparent)",
                        NewSplitView(SplitViewDisplayMode.Overlay, true)),
                    ("OverlayBrush=#60000000",
                        NewSplitView(SplitViewDisplayMode.Overlay, true,
                            overlay: new SolidColorBrush(Color.FromArgb(0x60, 0, 0, 0))))));
        }

        private static SplitView NewSplitView(
            SplitViewDisplayMode mode,
            bool isOpen,
            SplitViewPanePlacement placement = SplitViewPanePlacement.Left,
            bool canResize = false,
            Brush overlay = null,
            Style resizeThumbStyle = null,
            bool annotate = false)
        {
            var splitView = new SplitView
                            {
                                Width = 360,
                                // The annotated variant carries an extra caption
                                // inside the pane, so it needs the headroom.
                                Height = annotate ? 250 : 210,
                                DisplayMode = mode,
                                IsPaneOpen = isOpen,
                                PanePlacement = placement,
                                CanResizeOpenPane = canResize,
                                OpenPaneLength = 170,
                                CompactPaneLength = 48,
                                Pane = BuildPane(annotate),
                                Content = BuildContent(mode, annotate)
                            };

            if (overlay != null)
            {
                splitView.OverlayBrush = overlay;
            }

            if (resizeThumbStyle != null)
            {
                splitView.ResizeThumbStyle = resizeThumbStyle;
            }

            return splitView;
        }

        private static Style VisibleResizeThumbStyle()
        {
            var baseStyle = (Style)Application.Current.Resources["MahApps.Styles.MetroThumb.SplitView.Resize"];
            var style = new Style(typeof(MetroThumb), baseStyle);
            style.Setters.Add(new Setter(Control.BackgroundProperty, new SolidColorBrush(Color.FromRgb(0x00, 0x7A, 0xCC))));
            style.Setters.Add(new Setter(FrameworkElement.WidthProperty, 6d));
            return style;
        }

        private static UIElement BuildPane(bool annotate)
        {
            var items = new (string Glyph, string Label)[]
                        {
                            ("", "Menu"),
                            ("", "Home"),
                            ("", "Favorites"),
                            ("", "Mail"),
                            ("", "Settings")
                        };

            var stack = new StackPanel();

            if (annotate)
            {
                stack.Children.Add(Caption("Pane", HorizontalAlignment.Left));
            }

            foreach (var (glyph, label) in items)
            {
                var row = new Grid { Height = 40 };
                row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(48) });
                row.ColumnDefinitions.Add(new ColumnDefinition());

                var icon = new TextBlock
                           {
                               Text = glyph,
                               FontFamily = new FontFamily("Segoe MDL2 Assets"),
                               FontSize = 16,
                               HorizontalAlignment = HorizontalAlignment.Center,
                               VerticalAlignment = VerticalAlignment.Center
                           };
                Grid.SetColumn(icon, 0);
                row.Children.Add(icon);

                var text = new TextBlock
                           {
                               Text = label,
                               FontSize = 14,
                               VerticalAlignment = VerticalAlignment.Center
                           };
                Grid.SetColumn(text, 1);
                row.Children.Add(text);

                stack.Children.Add(row);
            }

            return stack;
        }

        private static UIElement BuildContent(SplitViewDisplayMode mode, bool annotate)
        {
            // Centred so that it stays visible next to an overlaying pane -
            // top-left text would simply disappear underneath it.
            var stack = new StackPanel
                        {
                            Margin = new Thickness(16),
                            HorizontalAlignment = annotate ? HorizontalAlignment.Left : HorizontalAlignment.Center,
                            VerticalAlignment = annotate ? VerticalAlignment.Top : VerticalAlignment.Center
                        };

            if (annotate)
            {
                stack.Children.Add(Caption("Content", HorizontalAlignment.Left));
            }

            stack.Children.Add(new TextBlock
                               {
                                   Text = "Main content area",
                                   FontSize = 16,
                                   FontWeight = FontWeights.SemiBold,
                                   Margin = new Thickness(0, 0, 0, 6)
                               });
            stack.Children.Add(new TextBlock
                               {
                                   Text = "DisplayMode = " + mode,
                                   FontSize = 13,
                                   Opacity = 0.75,
                                   TextWrapping = TextWrapping.Wrap
                               });

            return new Border
                   {
                       Background = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0xFF)),
                       Child = stack
                   };
        }

        private static TextBlock Caption(string text, HorizontalAlignment alignment)
        {
            return new TextBlock
                   {
                       Text = text,
                       FontSize = 11,
                       FontWeight = FontWeights.Bold,
                       Opacity = 0.55,
                       Margin = new Thickness(8, 6, 8, 2),
                       HorizontalAlignment = alignment
                   };
        }

        // Lays the given SplitViews out side by side, each under its caption.
        private static FrameworkElement Showcase(params (string Caption, SplitView View)[] items)
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

                column.Children.Add(new Border
                                    {
                                        BorderBrush = new SolidColorBrush(Color.FromRgb(0xCE, 0xD4, 0xDA)),
                                        BorderThickness = new Thickness(1),
                                        Child = view
                                    });

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
                             // Keep the capture windows off-screen so they do not
                             // flash over whatever the user is doing.
                             WindowStartupLocation = WindowStartupLocation.Manual,
                             Left = -20000,
                             Top = -20000,
                             Background = Brushes.White
                         };

            var rendered = new TaskCompletionSource<bool>();
            window.ContentRendered += (_, _) => rendered.TrySetResult(true);
            window.Show();
            await rendered.Task;

            // SplitView drives its visual states through storyboards; give them
            // time to settle before the pixels are read.
            await Task.Delay(900);
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
