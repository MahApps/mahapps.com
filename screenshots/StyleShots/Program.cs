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
    // Renders the control figures straight to PNG with RenderTargetBitmap. Like
    // HamburgerMenuShots the scenarios are written as XAML and loaded through
    // XamlReader, so what the figure shows and what the documentation prints
    // are the same markup.
    //
    // This app covers the pages under input/docs/styles/ and input/docs/helper/.
    // The argument is the docs root; each figure is written to
    // <docs-root>/<section>/images/. Adding a figure means adding a scenario.
    public static class Program
    {
        private const double Scale = 2.0;
        private static string outputRoot;

        [STAThread]
        public static void Main(string[] args)
        {
            outputRoot = Array.Find(args, a => !a.StartsWith("--")) ?? "shots";

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
                        await TextBoxFiguresAsync();
                        await DatePickerFiguresAsync();
                        await PasswordBoxFiguresAsync();
                        await HelperFiguresAsync();
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
            "xmlns:sys=\"clr-namespace:System;assembly=mscorlib\" " +
            "xmlns:mah=\"http://metro.mahapps.com/winfx/xaml/controls\"";

        // Every scenario is a XAML fragment wrapped in a Grid that carries the
        // namespaces, which keeps the fragments identical to what the pages print.
        private static FrameworkElement Xaml(string inner)
        {
            return (FrameworkElement)XamlReader.Parse($"<Grid {Xmlns}>{inner}</Grid>");
        }

        // ---------------------------------------------------------------- styles

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

        private static async Task TextBoxFiguresAsync()
        {
            await CaptureAsync("styles", "textbox-styles",
                Showcase(
                    ("implicit style", Xaml(@"<TextBox Width=""190"" Text=""metro"" />")),
                    ("MahApps.Styles.TextBox.Button",
                        Xaml(@"<TextBox Width=""190"" Style=""{StaticResource MahApps.Styles.TextBox.Button}"" Text=""metro"" />")),
                    ("MahApps.Styles.TextBox.Search",
                        Xaml(@"<TextBox Width=""190"" Style=""{StaticResource MahApps.Styles.TextBox.Search}"" Text=""metro"" />"))));

            await CaptureAsync("styles", "textbox-richtextbox",
                Showcase(
                    ("MahApps.Styles.RichTextBox",
                        Xaml(@"<RichTextBox Width=""210"" Height=""70"">
                                 <FlowDocument><Paragraph>Rich text</Paragraph></FlowDocument>
                               </RichTextBox>")),
                    ("MahApps.Styles.RichTextBox.Button",
                        Xaml(@"<RichTextBox Width=""210"" Height=""70"" Style=""{StaticResource MahApps.Styles.RichTextBox.Button}"">
                                 <FlowDocument><Paragraph>Rich text</Paragraph></FlowDocument>
                               </RichTextBox>"))));

            await CaptureAsync("styles", "textbox-watermark",
                Showcase(
                    ("Watermark, empty", Xaml(@"<TextBox Width=""190"" mah:TextBoxHelper.Watermark=""Search"" />")),
                    ("Watermark, filled", Xaml(@"<TextBox Width=""190"" mah:TextBoxHelper.Watermark=""Search"" Text=""metro"" />")),
                    ("UseFloatingWatermark", Xaml(@"<TextBox Width=""190"" mah:TextBoxHelper.Watermark=""Search"" mah:TextBoxHelper.UseFloatingWatermark=""True"" Text=""metro"" />"))));

            await CaptureAsync("styles", "textbox-buttons",
                Showcase(
                    ("ClearTextButton", Xaml(@"<TextBox Width=""190"" mah:TextBoxHelper.ClearTextButton=""True"" Text=""metro"" />")),
                    ("ButtonsAlignment=Left", Xaml(@"<TextBox Width=""190"" mah:TextBoxHelper.ClearTextButton=""True"" mah:TextBoxHelper.ButtonsAlignment=""Left"" Text=""metro"" />")),
                    ("ButtonContent + ButtonWidth", Xaml(@"<TextBox Width=""190"" mah:TextBoxHelper.ClearTextButton=""True""
                                                                   mah:TextBoxHelper.ButtonContent=""Clear""
                                                                   mah:TextBoxHelper.ButtonFontFamily=""{DynamicResource MahApps.Fonts.Family.Control}""
                                                                   mah:TextBoxHelper.ButtonFontSize=""12""
                                                                   mah:TextBoxHelper.ButtonWidth=""48""
                                                                   Text=""metro"" />"))));
        }

        private static async Task DatePickerFiguresAsync()
        {
            await CaptureAsync("styles", "datepicker-styles",
                Showcase(
                    ("empty", Xaml(@"<DatePicker Width=""190"" />")),
                    ("SelectedDate", Xaml(@"<DatePicker Width=""190"" SelectedDate=""2026-08-25"" />")),
                    ("ClearTextButton", Xaml(@"<DatePicker Width=""190"" SelectedDate=""2026-08-25"" mah:TextBoxHelper.ClearTextButton=""True"" />"))));

            await CaptureAsync("styles", "datepicker-watermark",
                Showcase(
                    ("Watermark", Xaml(@"<DatePicker Width=""190"" mah:TextBoxHelper.Watermark=""Start date"" />")),
                    ("UseFloatingWatermark", Xaml(@"<DatePicker Width=""190"" SelectedDate=""2026-08-25""
                                                                mah:TextBoxHelper.Watermark=""Start date""
                                                                mah:TextBoxHelper.UseFloatingWatermark=""True"" />"))));

            // The drop-down draws its calendar in a popup, which lives in its
            // own window and is out of reach of RenderTargetBitmap. A Calendar
            // carrying the same style shows exactly what the drop-down shows.
            await CaptureAsync("styles", "datepicker-calendar",
                Showcase(
                    ("MahApps.Styles.Calendar.Base",
                        Xaml(@"<Calendar Style=""{StaticResource MahApps.Styles.Calendar.Base}""
                                         DisplayDate=""2026-08-25"" SelectedDate=""2026-08-25"" />"))));
        }

        private static async Task PasswordBoxFiguresAsync()
        {
            const string secret = "sw0rdf1sh";

            await CaptureAsync("styles", "passwordbox-styles",
                Showcase(
                    ("implicit style", Box(string.Empty, secret)),
                    ("MahApps.Styles.PasswordBox.Button",
                        Box(@"Style=""{StaticResource MahApps.Styles.PasswordBox.Button}""", secret)),
                    ("MahApps.Styles.PasswordBox.Button.Revealed",
                        Box(@"Style=""{StaticResource MahApps.Styles.PasswordBox.Button.Revealed}""", secret)),
                    ("MahApps.Styles.PasswordBox.Win8",
                        Box(@"Style=""{StaticResource MahApps.Styles.PasswordBox.Win8}""", secret))));

            await CaptureAsync("styles", "passwordbox-watermark",
                Showcase(
                    ("Watermark, empty",
                        Box(@"mah:TextBoxHelper.Watermark=""Password""")),
                    ("Watermark, filled",
                        Box(@"mah:TextBoxHelper.Watermark=""Password""", secret)),
                    ("UseFloatingWatermark",
                        Box(@"mah:TextBoxHelper.Watermark=""Password"" mah:TextBoxHelper.UseFloatingWatermark=""True""", secret))));

            await CaptureAsync("styles", "passwordbox-clearbutton",
                Showcase(
                    ("ClearTextButton",
                        Box(@"mah:TextBoxHelper.ClearTextButton=""True""", secret)),
                    ("ButtonsAlignment=Left",
                        Box(@"mah:TextBoxHelper.ClearTextButton=""True"" mah:TextBoxHelper.ButtonsAlignment=""Left""", secret)),
                    ("ButtonContent + ButtonWidth",
                        Box(@"mah:TextBoxHelper.ClearTextButton=""True"" mah:TextBoxHelper.ButtonContent=""Clear"" mah:TextBoxHelper.ButtonFontFamily=""{DynamicResource MahApps.Fonts.Family.Control}"" mah:TextBoxHelper.ButtonFontSize=""12"" mah:TextBoxHelper.ButtonWidth=""48""", secret))));

            await CaptureAsync("styles", "passwordbox-capslock",
                Showcase(
                    ("default CapsLockIcon", Box(string.Empty, secret, capsLock: true)),
                    ("custom CapsLockIcon",
                        Box(@"mah:PasswordBoxHelper.CapsLockIcon=""CAPS""", secret, capsLock: true))));
        }

        // ---------------------------------------------------------------- helper

        private static async Task HelperFiguresAsync()
        {
            await CaptureAsync("helper", "checkboxhelper",
                Showcase(
                    ("default", Xaml(@"<CheckBox Content=""Checked"" IsChecked=""True"" />")),
                    ("CheckSize", Xaml(@"<CheckBox mah:CheckBoxHelper.CheckSize=""26"" Content=""Checked"" IsChecked=""True"" />")),
                    ("CheckCornerRadius", Xaml(@"<CheckBox mah:CheckBoxHelper.CheckSize=""26"" mah:CheckBoxHelper.CheckCornerRadius=""13"" Content=""Checked"" IsChecked=""True"" />")),
                    ("recoloured", Xaml(@"<CheckBox mah:CheckBoxHelper.CheckSize=""26""
                                                    mah:CheckBoxHelper.CheckBackgroundFillChecked=""#2E7D32""
                                                    mah:CheckBoxHelper.CheckBackgroundStrokeChecked=""#2E7D32""
                                                    mah:CheckBoxHelper.CheckGlyphForegroundChecked=""White""
                                                    Content=""Checked"" IsChecked=""True"" />"))));

            await CaptureAsync("helper", "checkboxhelper-states",
                Showcase(
                    ("Unchecked", Xaml(@"<CheckBox Content=""Unchecked"" IsChecked=""False"" />")),
                    ("Checked", Xaml(@"<CheckBox Content=""Checked"" IsChecked=""True"" />")),
                    ("Indeterminate", Xaml(@"<CheckBox Content=""Indeterminate"" IsChecked=""{x:Null}"" IsThreeState=""True"" />")),
                    ("Disabled", Xaml(@"<CheckBox Content=""Disabled"" IsChecked=""True"" IsEnabled=""False"" />"))));

            await CaptureAsync("helper", "radiobuttonhelper",
                Showcase(
                    ("default", Xaml(@"<RadioButton Content=""Selected"" IsChecked=""True"" />")),
                    ("RadioSize", Xaml(@"<RadioButton mah:RadioButtonHelper.RadioSize=""26"" Content=""Selected"" IsChecked=""True"" />")),
                    ("recoloured", Xaml(@"<RadioButton mah:RadioButtonHelper.RadioSize=""26""
                                                       mah:RadioButtonHelper.OuterEllipseCheckedFill=""#2E7D32""
                                                       mah:RadioButtonHelper.OuterEllipseCheckedStroke=""#2E7D32""
                                                       mah:RadioButtonHelper.CheckGlyphFill=""White""
                                                       mah:RadioButtonHelper.CheckGlyphStroke=""White""
                                                       Content=""Selected"" IsChecked=""True"" />"))));

            await CaptureAsync("helper", "togglebuttonhelper",
                Showcase(
                    ("LeftToRight (default)", Xaml(@"<StackPanel Width=""170"">
                                                       <CheckBox Content=""Check me"" IsChecked=""True"" />
                                                       <RadioButton Margin=""0 8 0 0"" Content=""Pick me"" IsChecked=""True"" />
                                                     </StackPanel>")),
                    ("RightToLeft", Xaml(@"<StackPanel Width=""170"">
                                             <CheckBox mah:ToggleButtonHelper.ContentDirection=""RightToLeft"" Content=""Check me"" IsChecked=""True"" />
                                             <RadioButton Margin=""0 8 0 0"" mah:ToggleButtonHelper.ContentDirection=""RightToLeft"" Content=""Pick me"" IsChecked=""True"" />
                                           </StackPanel>"))));

            await CaptureAsync("helper", "controlshelper",
                Showcase(
                    ("default", Xaml(@"<StackPanel Width=""170"">
                                         <TextBox Text=""Text"" />
                                         <Button Margin=""0 8 0 0"" Content=""Save"" />
                                       </StackPanel>")),
                    ("CornerRadius", Xaml(@"<StackPanel Width=""170"">
                                              <TextBox mah:ControlsHelper.CornerRadius=""8"" Text=""Text"" />
                                              <Button Margin=""0 8 0 0"" mah:ControlsHelper.CornerRadius=""8"" Content=""Save"" />
                                            </StackPanel>")),
                    // The button style sets ContentCharacterCasing to Upper, so
                    // the interesting direction is turning it off again.
                    ("ContentCharacterCasing=Normal", Xaml(@"<StackPanel Width=""170"">
                                                               <TextBox Text=""Text"" />
                                                               <Button Margin=""0 8 0 0"" mah:ControlsHelper.ContentCharacterCasing=""Normal"" Content=""Save"" />
                                                             </StackPanel>"))));

            await CaptureAsync("helper", "headeredcontrolhelper",
                Showcase(
                    ("default", Xaml(@"<GroupBox Width=""210"" Header=""Details"">
                                         <TextBlock Margin=""4"" Text=""Group box content"" />
                                       </GroupBox>")),
                    ("Header brushes and font", Xaml(@"<GroupBox Width=""210"" Header=""Details""
                                                                 mah:HeaderedControlHelper.HeaderBackground=""#2E7D32""
                                                                 mah:HeaderedControlHelper.HeaderForeground=""White""
                                                                 mah:HeaderedControlHelper.HeaderFontSize=""16""
                                                                 mah:HeaderedControlHelper.HeaderMargin=""10 6"">
                                                          <TextBlock Margin=""4"" Text=""Group box content"" />
                                                        </GroupBox>"))));

            await CaptureAsync("helper", "itemhelper",
                Showcase(
                    ("default", Xaml(@"<ListBox Width=""170"" SelectedIndex=""1"">
                                         <ListBoxItem Content=""Ada"" />
                                         <ListBoxItem Content=""Grace"" />
                                         <ListBoxItem Content=""Alan"" />
                                       </ListBox>")),
                    // Set through the container style: the MahApps ListBoxItem
                    // style already sets these, and a style setter on the item
                    // beats the inherited value from the ListBox.
                    ("recoloured selection", Xaml(@"<ListBox Width=""170"" SelectedIndex=""1"">
                                                      <ListBox.ItemContainerStyle>
                                                        <Style BasedOn=""{StaticResource MahApps.Styles.ListBoxItem}"" TargetType=""ListBoxItem"">
                                                          <Setter Property=""mah:ItemHelper.ActiveSelectionBackgroundBrush"" Value=""#2E7D32"" />
                                                          <Setter Property=""mah:ItemHelper.ActiveSelectionForegroundBrush"" Value=""White"" />
                                                          <Setter Property=""mah:ItemHelper.SelectedBackgroundBrush"" Value=""#2E7D32"" />
                                                          <Setter Property=""mah:ItemHelper.SelectedForegroundBrush"" Value=""White"" />
                                                        </Style>
                                                      </ListBox.ItemContainerStyle>
                                                      <ListBoxItem Content=""Ada"" />
                                                      <ListBoxItem Content=""Grace"" />
                                                      <ListBoxItem Content=""Alan"" />
                                                    </ListBox>"))));

            await CaptureAsync("helper", "sliderhelper",
                Showcase(
                    ("default", Xaml(@"<Slider Width=""190"" Maximum=""100"" Value=""40"" />")),
                    ("Thumb and track brushes", Xaml(@"<Slider Width=""190"" Maximum=""100"" Value=""40""
                                                               mah:SliderHelper.ThumbFillBrush=""#2E7D32""
                                                               mah:SliderHelper.TrackValueFillBrush=""#2E7D32""
                                                               mah:SliderHelper.TrackFillBrush=""#C8E6C9"" />"))));

            await CaptureAsync("helper", "tabcontrolhelper-underlined",
                Showcase(
                    ("None (default)", Tabs(@"mah:TabControlHelper.Underlined=""None""")),
                    ("TabItems", Tabs(@"mah:TabControlHelper.Underlined=""TabItems""")),
                    ("SelectedTabItem", Tabs(@"mah:TabControlHelper.Underlined=""SelectedTabItem""")),
                    ("TabPanel", Tabs(@"mah:TabControlHelper.Underlined=""TabPanel"""))));

            await CaptureAsync("helper", "tabcontrolhelper-underline",
                Showcase(
                    ("UnderlineSelectedBrush", Tabs(@"mah:TabControlHelper.Underlined=""SelectedTabItem"" mah:TabControlHelper.UnderlineSelectedBrush=""#2E7D32""")),
                    ("UnderlinePlacement=Top", Tabs(@"mah:TabControlHelper.Underlined=""SelectedTabItem"" mah:TabControlHelper.UnderlinePlacement=""Top"""))));

            await CaptureAsync("helper", "comboboxhelper",
                Showcase(
                    ("default", Xaml(@"<ComboBox Width=""190"" IsEditable=""True"" Text=""metro"" />")),
                    ("CharacterCasing=Upper", Xaml(@"<ComboBox Width=""190"" IsEditable=""True"" mah:ComboBoxHelper.CharacterCasing=""Upper"" Text=""METRO"" />"))));

            await CaptureAsync("helper", "datepickerhelper",
                Showcase(
                    ("default", Xaml(@"<DatePicker Width=""190"" />")),
                    // The style's default content is Path geometry rendered by
                    // the default template, so plain content needs the template
                    // cleared as well or nothing is drawn.
                    ("DropDownButtonContent", Xaml(@"<DatePicker Width=""190""
                                                                 mah:DatePickerHelper.DropDownButtonContent=""&#xE787;""
                                                                 mah:DatePickerHelper.DropDownButtonContentTemplate=""{x:Null}""
                                                                 mah:DatePickerHelper.DropDownButtonFontFamily=""Segoe MDL2 Assets""
                                                                 mah:DatePickerHelper.DropDownButtonFontSize=""16"" />"))));

            await CaptureAsync("helper", "expanderhelper",
                Showcase(
                    ("ExpandDirection=Down", Xaml(@"<Expander Width=""190"" Header=""Details"" IsExpanded=""True"">
                                                      <TextBlock Margin=""8"" Text=""Expander content"" />
                                                    </Expander>")),
                    ("ExpandDirection=Right", Xaml(@"<Expander Height=""120"" Header=""Details"" ExpandDirection=""Right"" IsExpanded=""True"">
                                                       <TextBlock Margin=""8"" VerticalAlignment=""Center"" Text=""Expander content"" />
                                                     </Expander>"))));

            await CaptureAsync("helper", "scrollviewerhelper",
                Showcase(
                    ("default", Scroller(string.Empty)),
                    ("VerticalScrollBarOnLeftSide", Scroller(@"mah:ScrollViewerHelper.VerticalScrollBarOnLeftSide=""True"""))));

            await CaptureAsync("helper", "datagridhelper",
                Showcase(
                    ("default", Grid(string.Empty)),
                    ("CellPadding + ColumnHeaderPadding",
                        Grid(@"mah:DataGridHelper.CellPadding=""10 6"" mah:DataGridHelper.ColumnHeaderPadding=""10 8"""))));
        }

        private static FrameworkElement Tabs(string attributes)
        {
            return Xaml($@"<TabControl Width=""230"" Height=""110"" {attributes}>
                             <TabItem Header=""One""><TextBlock Margin=""8"" Text=""First"" /></TabItem>
                             <TabItem Header=""Two""><TextBlock Margin=""8"" Text=""Second"" /></TabItem>
                           </TabControl>");
        }

        private static FrameworkElement Scroller(string attributes)
        {
            return Xaml($@"<ScrollViewer Width=""170"" Height=""100"" VerticalScrollBarVisibility=""Visible"" {attributes}>
                             <StackPanel>
                               <TextBlock Margin=""6"" Text=""First line"" />
                               <TextBlock Margin=""6"" Text=""Second line"" />
                               <TextBlock Margin=""6"" Text=""Third line"" />
                               <TextBlock Margin=""6"" Text=""Fourth line"" />
                               <TextBlock Margin=""6"" Text=""Fifth line"" />
                             </StackPanel>
                           </ScrollViewer>");
        }

        private static FrameworkElement Grid(string attributes)
        {
            return Xaml($@"<DataGrid Width=""250"" Height=""130"" AutoGenerateColumns=""False"" CanUserAddRows=""False"" {attributes}>
                             <DataGrid.ItemsSource>
                               <x:Array Type=""sys:String"">
                                 <sys:String>Ada Lovelace</sys:String>
                                 <sys:String>Grace Hopper</sys:String>
                                 <sys:String>Alan Turing</sys:String>
                               </x:Array>
                             </DataGrid.ItemsSource>
                             <DataGrid.Columns>
                               <DataGridTextColumn Width=""*"" Binding=""{{Binding}}"" Header=""Name"" />
                               <DataGridTextColumn Binding=""{{Binding Length}}"" Header=""Length"" />
                             </DataGrid.Columns>
                           </DataGrid>");
        }

        // ---------------------------------------------------------------- capture

        private static FrameworkElement Showcase(params (string Caption, FrameworkElement View)[] items)
        {
            var row = new StackPanel { Orientation = Orientation.Horizontal };

            foreach (var (caption, view) in items)
            {
                var column = new StackPanel { Margin = new Thickness(10), VerticalAlignment = VerticalAlignment.Top };
                column.Children.Add(new TextBlock
                                    {
                                        Text = caption,
                                        FontSize = 12,
                                        FontWeight = FontWeights.SemiBold,
                                        Margin = new Thickness(2, 0, 2, 8),
                                        Foreground = new SolidColorBrush(Color.FromRgb(0x49, 0x50, 0x57))
                                    });
                view.HorizontalAlignment = HorizontalAlignment.Left;
                column.Children.Add(view);
                row.Children.Add(column);
            }

            return new Border
                   {
                       Background = new SolidColorBrush(Color.FromRgb(0xF8, 0xF9, 0xFA)),
                       Padding = new Thickness(6),
                       Child = row
                   };
        }

        private static async Task CaptureAsync(string section, string name, FrameworkElement root)
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

            // Whether a control wears the focus border depends on where keyboard
            // focus landed, which is not something the scenario says. Clear it.
            Keyboard.ClearFocus();
            FocusManager.SetFocusedElement(window, null);

            await window.Dispatcher.InvokeAsync(() => { }, DispatcherPriority.ContextIdle);

            root.UpdateLayout();

            var width = (int)Math.Ceiling(root.ActualWidth * Scale);
            var height = (int)Math.Ceiling(root.ActualHeight * Scale);
            var bitmap = new RenderTargetBitmap(width, height, 96 * Scale, 96 * Scale, PixelFormats.Pbgra32);
            bitmap.Render(root);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            var directory = Path.Combine(outputRoot, section, "images");
            Directory.CreateDirectory(directory);

            var path = Path.Combine(directory, name + ".png");
            using (var stream = File.Create(path))
            {
                encoder.Save(stream);
            }

            Console.WriteLine($"{section}/{name}.png  {width}x{height}");
            window.Close();
        }
    }
}
