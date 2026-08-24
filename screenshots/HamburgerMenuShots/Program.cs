using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace HamburgerMenuShots
{
    // Renders HamburgerMenu configurations straight to PNG with
    // RenderTargetBitmap. The scenarios are written as XAML and loaded through
    // XamlReader rather than assembled in C#: the control is driven almost
    // entirely by templates, and XAML keeps those readable - and liftable
    // straight into the documentation.
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

        // MahApps ships no DataTemplate for the menu item types, so every menu
        // needs one. Keyed by DataType, which lets a single menu mix item types.
        private const string ItemTemplates = @"
    <mah:HamburgerMenu.Resources>
      <DataTemplate DataType=""{x:Type mah:HamburgerMenuGlyphItem}"">
        <Grid Height=""48"">
          <Grid.ColumnDefinitions>
            <ColumnDefinition Width=""{Binding RelativeSource={RelativeSource AncestorType={x:Type mah:HamburgerMenu}}, Path=CompactPaneLength}"" />
            <ColumnDefinition />
          </Grid.ColumnDefinitions>
          <TextBlock Grid.Column=""0"" HorizontalAlignment=""Center"" VerticalAlignment=""Center""
                     FontFamily=""Segoe MDL2 Assets"" FontSize=""16"" Text=""{Binding Glyph}"" />
          <TextBlock Grid.Column=""1"" VerticalAlignment=""Center"" FontSize=""14"" Text=""{Binding Label}"" />
        </Grid>
      </DataTemplate>
      <DataTemplate DataType=""{x:Type mah:HamburgerMenuIconItem}"">
        <Grid Height=""48"">
          <Grid.ColumnDefinitions>
            <ColumnDefinition Width=""{Binding RelativeSource={RelativeSource AncestorType={x:Type mah:HamburgerMenu}}, Path=CompactPaneLength}"" />
            <ColumnDefinition />
          </Grid.ColumnDefinitions>
          <ContentControl Grid.Column=""0"" HorizontalAlignment=""Center"" VerticalAlignment=""Center""
                          Content=""{Binding Icon}"" Focusable=""False"" IsTabStop=""False"" />
          <TextBlock Grid.Column=""1"" VerticalAlignment=""Center"" FontSize=""14"" Text=""{Binding Label}"" />
        </Grid>
      </DataTemplate>
      <DataTemplate DataType=""{x:Type mah:HamburgerMenuImageItem}"">
        <Grid Height=""48"">
          <Grid.ColumnDefinitions>
            <ColumnDefinition Width=""{Binding RelativeSource={RelativeSource AncestorType={x:Type mah:HamburgerMenu}}, Path=CompactPaneLength}"" />
            <ColumnDefinition />
          </Grid.ColumnDefinitions>
          <Ellipse Grid.Column=""0"" Width=""26"" Height=""26"" HorizontalAlignment=""Center"" VerticalAlignment=""Center"">
            <Ellipse.Fill>
              <ImageBrush ImageSource=""{Binding Thumbnail}"" Stretch=""UniformToFill"" />
            </Ellipse.Fill>
          </Ellipse>
          <TextBlock Grid.Column=""1"" VerticalAlignment=""Center"" FontSize=""14"" Text=""{Binding Label}"" />
        </Grid>
      </DataTemplate>
      <DataTemplate DataType=""{x:Type mah:HamburgerMenuHeaderItem}"">
        <TextBlock Margin=""12 4"" FontSize=""11"" FontWeight=""Bold"" Opacity=""0.6"" Text=""{Binding Label}"" />
      </DataTemplate>
    </mah:HamburgerMenu.Resources>";

        private const string ContentTemplate = @"
    <mah:HamburgerMenu.ContentTemplate>
      <DataTemplate>
        <StackPanel Margin=""18"" VerticalAlignment=""Center"">
          <TextBlock FontSize=""16"" FontWeight=""SemiBold"" Text=""{Binding Label}"" />
          <TextBlock FontSize=""12"" Opacity=""0.7"" Text=""selected item content"" />
        </StackPanel>
      </DataTemplate>
    </mah:HamburgerMenu.ContentTemplate>";

        private static string Items(bool withHeaderAndSeparator = false, string logoPath = null)
        {
            var sb = new StringBuilder();
            sb.Append(@"<mah:HamburgerMenu.ItemsSource><mah:HamburgerMenuItemCollection>");

            if (withHeaderAndSeparator)
            {
                sb.Append(@"<mah:HamburgerMenuHeaderItem Label=""LIBRARY"" />");
            }

            sb.Append(@"<mah:HamburgerMenuGlyphItem Glyph=""&#xE80F;"" Label=""Home"" />");
            sb.Append(@"<mah:HamburgerMenuGlyphItem Glyph=""&#xE734;"" Label=""Favorites"" />");

            if (withHeaderAndSeparator)
            {
                sb.Append(@"<mah:HamburgerMenuSeparatorItem />");
                sb.Append(@"<mah:HamburgerMenuHeaderItem Label=""ACCOUNT"" />");
                sb.Append(@"<mah:HamburgerMenuIconItem Label=""Mail"">
                              <mah:HamburgerMenuIconItem.Icon>
                                <TextBlock FontFamily=""Segoe MDL2 Assets"" FontSize=""16"" Text=""&#xE715;"" />
                              </mah:HamburgerMenuIconItem.Icon>
                            </mah:HamburgerMenuIconItem>");
                if (logoPath != null)
                {
                    sb.Append($@"<mah:HamburgerMenuImageItem Label=""Profile"" Thumbnail=""{logoPath}"" />");
                }
            }
            else
            {
                sb.Append(@"<mah:HamburgerMenuGlyphItem Glyph=""&#xE715;"" Label=""Mail"" />");
            }

            sb.Append(@"</mah:HamburgerMenuItemCollection></mah:HamburgerMenu.ItemsSource>");
            return sb.ToString();
        }

        private static string Options()
        {
            return @"<mah:HamburgerMenu.OptionsItemsSource><mah:HamburgerMenuItemCollection>
                       <mah:HamburgerMenuGlyphItem Glyph=""&#xE713;"" Label=""Settings"" />
                     </mah:HamburgerMenuItemCollection></mah:HamburgerMenu.OptionsItemsSource>";
        }

        private static FrameworkElement Menu(
            string attributes,
            string inner,
            double width = 400,
            double height = 300,
            int selectedIndex = 0)
        {
            // HamburgerMenu only assigns Content when an item is actually
            // invoked, so a declarative SelectedIndex leaves the content area
            // empty. Bind it for the screenshots.
            const string contentBinding =
                @"Content=""{Binding RelativeSource={RelativeSource Self}, Path=SelectedItem}""";

            var xaml = $@"<mah:HamburgerMenu {Xmlns} Width=""{width}"" Height=""{height}"" SelectedIndex=""{selectedIndex}"" {contentBinding} {attributes}>
                            {ItemTemplates}
                            {ContentTemplate}
                            {inner}
                          </mah:HamburgerMenu>";
            return (FrameworkElement)XamlReader.Parse(xaml);
        }

        private static async Task CaptureAllAsync()
        {
            var logo = new Uri(Path.GetFullPath(Path.Combine("input", "assets", "img", "mahapps.metro.logo.png"))).AbsoluteUri;
            var logoExists = File.Exists(Path.GetFullPath(Path.Combine("input", "assets", "img", "mahapps.metro.logo.png")));
            if (!logoExists)
            {
                Console.WriteLine("NOTE: logo not found, the image item will be skipped. Run from the repository root.");
                logo = null;
            }

            await CaptureAsync("hamburgermenu-anatomy",
                Showcase(("Default style, DisplayMode=CompactInline, IsPaneOpen=True",
                    Menu(@"IsPaneOpen=""True""", Items() + Options()))));

            await CaptureAsync("hamburgermenu-ispaneopen",
                Showcase(
                    ("IsPaneOpen=False", Menu(@"IsPaneOpen=""False""", Items() + Options())),
                    ("IsPaneOpen=True", Menu(@"IsPaneOpen=""True""", Items() + Options()))));

            await CaptureAsync("hamburgermenu-itemtypes",
                Showcase(("Header, Glyph, Separator, Icon and Image items",
                    // Index 0 is the "LIBRARY" header, so start the selection on
                    // the first real item instead.
                    Menu(@"IsPaneOpen=""True""", Items(true, logo) + Options(), height: 420, selectedIndex: 1))));

            await CaptureAsync("hamburgermenu-paneplacement",
                Showcase(
                    ("PanePlacement=Left", Menu(@"IsPaneOpen=""True""", Items() + Options())),
                    ("PanePlacement=Right", Menu(@"IsPaneOpen=""True"" PanePlacement=""Right""", Items() + Options()))));

            await CaptureAsync("hamburgermenu-selectionindicator",
                Showcase(
                    ("Default style", Menu(@"IsPaneOpen=""True""", Items() + Options())),
                    ("MahApps.Styles.HamburgerMenu.CreatorsUpdate",
                        Menu(@"IsPaneOpen=""True"" Style=""{DynamicResource MahApps.Styles.HamburgerMenu.CreatorsUpdate}""",
                            Items() + Options()))));

            // OptionsVisibility is a no-op in the library - nothing consumes it -
            // so the way to drop the options block is to not fill it.
            await CaptureAsync("hamburgermenu-options",
                Showcase(
                    ("With OptionsItemsSource", Menu(@"IsPaneOpen=""True""", Items() + Options())),
                    ("Without OptionsItemsSource", Menu(@"IsPaneOpen=""True""", Items()))));

            await CaptureAsync("hamburgermenu-header",
                Showcase(("HamburgerMenuHeaderTemplate + HamburgerVisibility=Collapsed",
                    Menu(@"IsPaneOpen=""True"" HamburgerVisibility=""Collapsed""",
                        @"<mah:HamburgerMenu.HamburgerMenuHeaderTemplate>
                            <DataTemplate>
                              <TextBlock Margin=""12 0"" VerticalAlignment=""Center"" FontSize=""16""
                                         Foreground=""{DynamicResource MahApps.Brushes.IdealForeground}"" Text=""My App"" />
                            </DataTemplate>
                          </mah:HamburgerMenu.HamburgerMenuHeaderTemplate>" + Items() + Options()))));
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
                             WindowStartupLocation = WindowStartupLocation.Manual,
                             Left = -20000,
                             Top = -20000,
                             Background = Brushes.White
                         };

            var rendered = new TaskCompletionSource<bool>();
            window.ContentRendered += (_, _) => rendered.TrySetResult(true);
            window.Show();
            await rendered.Task;

            // The pane animates between its states; let it settle.
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
