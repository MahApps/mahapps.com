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
                        LoadCalendarStyles();
                        await DataGridFiguresAsync();
                        await CalendarFiguresAsync();
                        await CheckBoxFiguresAsync();
                        await RadioButtonFiguresAsync();
                        await ButtonFiguresAsync();
                        await TextBoxFiguresAsync();
                        await ComboBoxFiguresAsync();
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

        private static async Task CheckBoxFiguresAsync()
        {
            await CaptureAsync("styles", "checkbox-styles",
                Showcase(
                    ("implicit style", Xaml(@"<StackPanel Width=""150"">
                                                <CheckBox Content=""Unchecked"" />
                                                <CheckBox Margin=""0 8 0 0"" Content=""Checked"" IsChecked=""True"" />
                                              </StackPanel>")),
                    ("MahApps.Styles.CheckBox.Win10", Xaml(@"<StackPanel Width=""150"">
                                                <CheckBox Style=""{StaticResource MahApps.Styles.CheckBox.Win10}"" Content=""Unchecked"" />
                                                <CheckBox Margin=""0 8 0 0"" Style=""{StaticResource MahApps.Styles.CheckBox.Win10}"" Content=""Checked"" IsChecked=""True"" />
                                              </StackPanel>"))));

            await CaptureAsync("styles", "checkbox-states",
                Showcase(
                    ("Unchecked", Xaml(@"<CheckBox Content=""Unchecked"" />")),
                    ("Checked", Xaml(@"<CheckBox Content=""Checked"" IsChecked=""True"" />")),
                    ("Indeterminate", Xaml(@"<CheckBox Content=""Indeterminate"" IsChecked=""{x:Null}"" IsThreeState=""True"" />")),
                    ("Disabled", Xaml(@"<CheckBox Content=""Disabled"" IsChecked=""True"" IsEnabled=""False"" />"))));

            // The same box under the three groups of properties the page names:
            // size, shape, and the brushes for one state.
            await CaptureAsync("styles", "checkbox-custom",
                Showcase(
                    ("default", Xaml(@"<CheckBox Content=""Checked"" IsChecked=""True"" />")),
                    ("CheckSize", Xaml(@"<CheckBox mah:CheckBoxHelper.CheckSize=""26"" Content=""Checked"" IsChecked=""True"" />")),
                    ("+ CheckCornerRadius", Xaml(@"<CheckBox mah:CheckBoxHelper.CheckSize=""26""
                                                             mah:CheckBoxHelper.CheckCornerRadius=""13""
                                                             Content=""Checked"" IsChecked=""True"" />")),
                    ("+ recoloured", Xaml(@"<CheckBox mah:CheckBoxHelper.CheckSize=""26""
                                                      mah:CheckBoxHelper.CheckCornerRadius=""13""
                                                      mah:CheckBoxHelper.CheckBackgroundFillChecked=""#2E7D32""
                                                      mah:CheckBoxHelper.CheckBackgroundStrokeChecked=""#2E7D32""
                                                      mah:CheckBoxHelper.CheckGlyphForegroundChecked=""White""
                                                      Content=""Checked"" IsChecked=""True"" />"))));

            await CaptureAsync("styles", "checkbox-layout",
                Showcase(
                    ("default", Xaml(@"<StackPanel Width=""170"">
                                         <CheckBox Content=""Wrap lines"" IsChecked=""True"" />
                                         <CheckBox Margin=""0 8 0 0"" Content=""Show line numbers"" />
                                       </StackPanel>")),
                    ("ContentDirection=RightToLeft", Xaml(@"<StackPanel Width=""170"">
                                         <CheckBox mah:ToggleButtonHelper.ContentDirection=""RightToLeft"" Content=""Wrap lines"" IsChecked=""True"" />
                                         <CheckBox Margin=""0 8 0 0"" mah:ToggleButtonHelper.ContentDirection=""RightToLeft"" Content=""Show line numbers"" />
                                       </StackPanel>"))));
        }

        private const string RadioGroup = @"
                                <RadioButton {0} Content=""Small"" />
                                <RadioButton Margin=""0 8 0 0"" {0} Content=""Medium"" IsChecked=""True"" />
                                <RadioButton Margin=""0 8 0 0"" {0} Content=""Large"" />";

        // The WinUI calendar dictionary is loaded from the file the docs page
        // links to, rather than restated here, so the figure cannot show
        // something the reader would not get by using that file.
        private static void LoadCalendarStyles()
        {
            // Win10 merges WinUI, so loading it brings both families in.
            var path = Path.GetFullPath(Path.Combine("input", "assets", "xaml", "Controls.Calendar.Win10.xaml"));
            if (!File.Exists(path))
            {
                Console.WriteLine("NOTE: Controls.Calendar.Win10.xaml not found, the calendar figures will be skipped. Run from the repository root.");
                return;
            }

            Application.Current.Resources.MergedDictionaries.Add(
                new ResourceDictionary { Source = new Uri(path, UriKind.Absolute) });
        }

        private static async Task CalendarFiguresAsync()
        {
            const string month = @"DisplayDate=""2020-06-15"" SelectedDate=""2020-06-15""";

            await CaptureAsync("styles", "calendar-styles",
                Showcase(
                    ("MahApps.Styles.Calendar",
                        Xaml($@"<Calendar Style=""{{StaticResource MahApps.Styles.Calendar}}"" {month} />")),
                    ("MahApps.Styles.Calendar.WinUI",
                        Xaml($@"<Calendar Style=""{{StaticResource MahApps.Styles.Calendar.WinUI}}"" {month} />"))));

            await CaptureAsync("styles", "calendar-win10",
                Showcase(
                    ("MahApps.Styles.Calendar.Win10",
                        Xaml($@"<Calendar Style=""{{StaticResource MahApps.Styles.Calendar.Win10}}"" {month} />")),
                    ("MahApps.Styles.Calendar.WinUI",
                        Xaml($@"<Calendar Style=""{{StaticResource MahApps.Styles.Calendar.WinUI}}"" {month} />"))));

            await CaptureAsync("styles", "calendar-winui-modes",
                Showcase(
                    ("DisplayMode=Month",
                        Xaml($@"<Calendar Style=""{{StaticResource MahApps.Styles.Calendar.WinUI}}"" {month} />")),
                    ("DisplayMode=Year",
                        Xaml($@"<Calendar Style=""{{StaticResource MahApps.Styles.Calendar.WinUI}}"" DisplayMode=""Year"" {month} />")),
                    ("DisplayMode=Decade",
                        Xaml($@"<Calendar Style=""{{StaticResource MahApps.Styles.Calendar.WinUI}}"" DisplayMode=""Decade"" {month} />"))));

            await CaptureAsync("styles", "calendar-winui-states",
                Showcase(
                    ("blackout dates",
                        Xaml($@"<Calendar Style=""{{StaticResource MahApps.Styles.Calendar.WinUI}}"" {month}>
                                  <Calendar.BlackoutDates>
                                    <CalendarDateRange Start=""2020-06-20"" End=""2020-06-21"" />
                                    <CalendarDateRange Start=""2020-06-27"" End=""2020-06-28"" />
                                  </Calendar.BlackoutDates>
                                </Calendar>")),
                    ("IsEnabled=False",
                        Xaml($@"<Calendar Style=""{{StaticResource MahApps.Styles.Calendar.WinUI}}"" IsEnabled=""False"" {month} />"))));
        }

        private static async Task RadioButtonFiguresAsync()
        {
            await CaptureAsync("styles", "radiobutton-styles",
                Showcase(
                    ("implicit style", Xaml("<StackPanel Width=\"150\">" + string.Format(RadioGroup, string.Empty) + "</StackPanel>")),
                    ("MahApps.Styles.RadioButton.Win10",
                        Xaml("<StackPanel Width=\"150\">"
                             + string.Format(RadioGroup, @"Style=""{StaticResource MahApps.Styles.RadioButton.Win10}""")
                             + "</StackPanel>"))));

            await CaptureAsync("styles", "radiobutton-states",
                Showcase(
                    ("Unchecked", Xaml(@"<RadioButton Content=""Unchecked"" />")),
                    ("Checked", Xaml(@"<RadioButton Content=""Checked"" IsChecked=""True"" />")),
                    ("Indeterminate", Xaml(@"<RadioButton Content=""Indeterminate"" IsThreeState=""True"" IsChecked=""{x:Null}"" />")),
                    ("Disabled", Xaml(@"<RadioButton Content=""Disabled"" IsChecked=""True"" IsEnabled=""False"" />"))));

            await CaptureAsync("styles", "radiobutton-custom",
                Showcase(
                    ("default", Xaml(@"<RadioButton Content=""Selected"" IsChecked=""True"" />")),
                    ("RadioSize", Xaml(@"<RadioButton mah:RadioButtonHelper.RadioSize=""26"" Content=""Selected"" IsChecked=""True"" />")),
                    ("+ RadioCheckSize", Xaml(@"<RadioButton mah:RadioButtonHelper.RadioSize=""26""
                                                             mah:RadioButtonHelper.RadioCheckSize=""16""
                                                             Content=""Selected"" IsChecked=""True"" />")),
                    ("+ recoloured", Xaml(@"<RadioButton mah:RadioButtonHelper.RadioSize=""26""
                                                         mah:RadioButtonHelper.RadioCheckSize=""16""
                                                         mah:RadioButtonHelper.OuterEllipseCheckedFill=""#2E7D32""
                                                         mah:RadioButtonHelper.OuterEllipseCheckedStroke=""#2E7D32""
                                                         mah:RadioButtonHelper.CheckGlyphFill=""White""
                                                         mah:RadioButtonHelper.CheckGlyphStroke=""White""
                                                         Content=""Selected"" IsChecked=""True"" />"))));

            await CaptureAsync("styles", "radiobutton-layout",
                Showcase(
                    ("default", Xaml("<StackPanel Width=\"170\">" + string.Format(RadioGroup, string.Empty) + "</StackPanel>")),
                    ("ContentDirection=RightToLeft",
                        Xaml("<StackPanel Width=\"170\">"
                             + string.Format(RadioGroup, @"mah:ToggleButtonHelper.ContentDirection=""RightToLeft""")
                             + "</StackPanel>"))));
        }

        private static async Task ButtonFiguresAsync()
        {
            await CaptureAsync("styles", "buttons-square",
                Showcase(
                    ("implicit style", Xaml(@"<Button Width=""110"" Content=""Save"" />")),
                    ("Button.Square", Xaml(@"<Button Width=""110"" Style=""{StaticResource MahApps.Styles.Button.Square}"" Content=""Save"" />")),
                    ("Button.Square.Accent", Xaml(@"<Button Width=""110"" Style=""{StaticResource MahApps.Styles.Button.Square.Accent}"" Content=""Save"" />")),
                    ("Button.Square.Highlight", Xaml(@"<Button Width=""110"" Style=""{StaticResource MahApps.Styles.Button.Square.Highlight}"" Content=""Save"" />"))));

            await CaptureAsync("styles", "buttons-metrosquare",
                Showcase(
                    ("Button.MetroSquare", Xaml(@"<Button Width=""130"" Style=""{StaticResource MahApps.Styles.Button.MetroSquare}"" Content=""Save"" />")),
                    ("Button.MetroSquare.Accent", Xaml(@"<Button Width=""130"" Style=""{StaticResource MahApps.Styles.Button.MetroSquare.Accent}"" Content=""Save"" />"))));

            await CaptureAsync("styles", "buttons-circle-flat",
                Showcase(
                    ("Button.Circle", Xaml(@"<Button Width=""48"" Height=""48"" Style=""{StaticResource MahApps.Styles.Button.Circle}"">
                                               <TextBlock FontFamily=""Segoe MDL2 Assets"" FontSize=""18"" Text=""&#xE72C;"" />
                                             </Button>")),
                    ("Button.Flat", Xaml(@"<Button Width=""110"" Style=""{StaticResource MahApps.Styles.Button.Flat}"" Content=""Save"" />")),
                    ("Button.Chromeless", Xaml(@"<Button Width=""110"" Style=""{StaticResource MahApps.Styles.Button.Chromeless}"" Content=""Save"" />"))));

            await CaptureAsync("styles", "buttons-dialogs",
                Showcase(
                    ("Button.Dialogs", Xaml(@"<Button Width=""110"" Style=""{StaticResource MahApps.Styles.Button.Dialogs}"" Content=""Cancel"" />")),
                    ("Button.Dialogs.Accent", Xaml(@"<Button Width=""110"" Style=""{StaticResource MahApps.Styles.Button.Dialogs.Accent}"" Content=""OK"" />")),
                    ("Button.Dialogs.AccentHighlight", Xaml(@"<Button Width=""110"" Style=""{StaticResource MahApps.Styles.Button.Dialogs.AccentHighlight}"" Content=""OK"" />"))));

            await CaptureAsync("styles", "togglebutton-styles",
                Showcase(
                    ("implicit style", Xaml(@"<ToggleButton Width=""110"" Content=""Bold"" IsChecked=""True"" />")),
                    ("ToggleButton.Circle", Xaml(@"<ToggleButton Width=""48"" Height=""48"" IsChecked=""True"" Style=""{StaticResource MahApps.Styles.ToggleButton.Circle}"">
                                                     <TextBlock FontFamily=""Segoe MDL2 Assets"" FontSize=""18"" Text=""&#xE734;"" />
                                                   </ToggleButton>")),
                    ("ToggleButton.Flat", Xaml(@"<ToggleButton Width=""110"" Content=""Bold"" IsChecked=""True"" Style=""{StaticResource MahApps.Styles.ToggleButton.Flat}"" />"))));

            await CaptureAsync("styles", "togglebutton-states",
                Showcase(
                    ("unchecked", Xaml(@"<ToggleButton Width=""110"" Content=""Bold"" />")),
                    ("checked", Xaml(@"<ToggleButton Width=""110"" Content=""Bold"" IsChecked=""True"" />")),
                    ("indeterminate", Xaml(@"<ToggleButton Width=""110"" Content=""Bold"" IsThreeState=""True"" IsChecked=""{x:Null}"" />")),
                    ("disabled", Xaml(@"<ToggleButton Width=""110"" Content=""Bold"" IsChecked=""True"" IsEnabled=""False"" />"))));

            await CaptureAsync("styles", "togglebutton-icons",
                Showcase(
                    ("unchecked", Xaml(@"<ToggleButton Width=""48"" Height=""48"" Style=""{StaticResource MahApps.Styles.ToggleButton.Circle}"">
                                           <TextBlock FontFamily=""Segoe MDL2 Assets"" FontSize=""18"" Text=""&#xE734;"" />
                                         </ToggleButton>")),
                    ("checked", Xaml(@"<ToggleButton Width=""48"" Height=""48"" IsChecked=""True"" Style=""{StaticResource MahApps.Styles.ToggleButton.Circle}"">
                                         <TextBlock FontFamily=""Segoe MDL2 Assets"" FontSize=""18"" Text=""&#xE734;"" />
                                       </ToggleButton>")),
                    ("a Path instead of a glyph", Xaml(@"<ToggleButton Width=""48"" Height=""48"" IsChecked=""True"" Style=""{StaticResource MahApps.Styles.ToggleButton.Circle}"">
                                                           <ContentControl Width=""20"" Height=""20""
                                                                           Content=""M12,2L15,9L22,9L16,14L18,21L12,17L6,21L8,14L2,9L9,9Z""
                                                                           Style=""{DynamicResource MahApps.Styles.ContentControl.PathIcon}"" />
                                                         </ToggleButton>"))));
        }

        private const string ComboItems = @"
                                 <ComboBoxItem Content=""Ada Lovelace"" />
                                 <ComboBoxItem Content=""Grace Hopper"" />
                                 <ComboBoxItem Content=""Alan Turing"" />";

        private static async Task ComboBoxFiguresAsync()
        {
            await CaptureAsync("styles", "combobox-styles",
                Showcase(
                    ("default", Xaml($@"<ComboBox Width=""190"" SelectedIndex=""0"">{ComboItems}</ComboBox>")),
                    ("Watermark", Xaml($@"<ComboBox Width=""190"" mah:TextBoxHelper.Watermark=""Pick someone"">{ComboItems}</ComboBox>")),
                    ("ClearTextButton", Xaml($@"<ComboBox Width=""190"" SelectedIndex=""0"" mah:TextBoxHelper.ClearTextButton=""True"">{ComboItems}</ComboBox>"))));

            await CaptureAsync("styles", "combobox-editable",
                Showcase(
                    ("IsEditable", Xaml($@"<ComboBox Width=""190"" IsEditable=""True"" SelectedIndex=""0"">{ComboItems}</ComboBox>")),
                    ("UseFloatingWatermark", Xaml($@"<ComboBox Width=""190"" IsEditable=""True"" SelectedIndex=""0""
                                                               mah:TextBoxHelper.Watermark=""Pick someone""
                                                               mah:TextBoxHelper.UseFloatingWatermark=""True"">{ComboItems}</ComboBox>")),
                    ("CharacterCasing", Xaml($@"<ComboBox Width=""190"" IsEditable=""True"" Text=""ADA LOVELACE""
                                                          mah:ComboBoxHelper.CharacterCasing=""Upper"">{ComboItems}</ComboBox>"))));

            // The recipe the page prints for showing the clear button only once
            // something is selected. Rendering both states is what proves the
            // trigger does what the page claims.
            const string conditionalClear = @"
              <Grid.Resources>
                <Style x:Key=""ClearWhenSelected"" BasedOn=""{StaticResource MahApps.Styles.ComboBox}"" TargetType=""{x:Type ComboBox}"">
                  <Setter Property=""mah:TextBoxHelper.ClearTextButton"" Value=""True"" />
                  <Style.Triggers>
                    <DataTrigger Binding=""{Binding SelectedItem, RelativeSource={RelativeSource Self}, Converter={x:Static mah:IsNullConverter.Instance}}"" Value=""True"">
                      <Setter Property=""mah:TextBoxHelper.ClearTextButton"" Value=""False"" />
                    </DataTrigger>
                  </Style.Triggers>
                </Style>
              </Grid.Resources>";

            await CaptureAsync("styles", "combobox-clearbutton",
                Showcase(
                    ("nothing selected", Xaml($@"{conditionalClear}
                        <ComboBox Width=""190"" Style=""{{StaticResource ClearWhenSelected}}"" mah:TextBoxHelper.Watermark=""Pick someone"">{ComboItems}</ComboBox>")),
                    ("item selected", Xaml($@"{conditionalClear}
                        <ComboBox Width=""190"" Style=""{{StaticResource ClearWhenSelected}}"" SelectedIndex=""0"">{ComboItems}</ComboBox>"))));

            await CaptureDropDownAsync("styles", "combobox-dropdown",
                $@"<ComboBox Width=""190"" SelectedIndex=""0"">{ComboItems}</ComboBox>");

            // Grouping needs a grouped view, which is a CollectionViewSource
            // over real objects rather than anything XAML can spell inline. The
            // GroupStyle - the part the page prints - stays in the markup.
            await CaptureDropDownAsync("styles", "combobox-grouping",
                @"<ComboBox Width=""190"" DisplayMemberPath=""Title""
                            Style=""{StaticResource MahApps.Styles.ComboBox.Virtualized}"">
                    <ComboBox.GroupStyle>
                      <GroupStyle>
                        <GroupStyle.HeaderTemplate>
                          <DataTemplate>
                            <TextBlock Margin=""4 2"" FontWeight=""Bold"" Text=""{Binding Name}"" />
                          </DataTemplate>
                        </GroupStyle.HeaderTemplate>
                      </GroupStyle>
                    </ComboBox.GroupStyle>
                  </ComboBox>",
                combo =>
                    {
                        var albums = new[]
                                     {
                                         new Album { Title = "Kind of Blue", Genre = "Jazz" },
                                         new Album { Title = "A Love Supreme", Genre = "Jazz" },
                                         new Album { Title = "The Köln Concert", Genre = "Jazz" },
                                         new Album { Title = "Remain in Light", Genre = "Rock" },
                                         new Album { Title = "OK Computer", Genre = "Rock" }
                                     };

                        var view = new System.Windows.Data.CollectionViewSource { Source = albums };
                        view.GroupDescriptions.Add(new System.Windows.Data.PropertyGroupDescription(nameof(Album.Genre)));

                        combo.ItemsSource = view.View;
                        combo.SelectedIndex = 0;
                    });
        }

        // The drop-down lives in a Popup, which is its own window and so out of
        // reach of a RenderTargetBitmap of the page. The popup's child is an
        // ordinary element with a laid-out visual tree, though, so open the
        // drop-down and render that instead.
        public sealed class Album
        {
            public string Title { get; set; }

            public string Genre { get; set; }

            public double Price { get; set; }

            public bool InStock { get; set; }
        }

        private static Album[] Albums()
        {
            return new[]
                   {
                       new Album { Title = "Kind of Blue", Genre = "Jazz", Price = 12.99, InStock = true },
                       new Album { Title = "A Love Supreme", Genre = "Jazz", Price = 14.50, InStock = false },
                       new Album { Title = "Remain in Light", Genre = "Rock", Price = 11.00, InStock = true },
                       new Album { Title = "OK Computer", Genre = "Rock", Price = 15.75, InStock = true }
                   };
        }

        // A DataGrid needs real objects, so these scenarios set ItemsSource from
        // code and leave the columns - the part the pages print - in the XAML.
        private static FrameworkElement Grid(string attributes, string columns)
        {
            var grid = (DataGrid)XamlReader.Parse(
                $@"<DataGrid {Xmlns} AutoGenerateColumns=""False"" CanUserAddRows=""False"" IsReadOnly=""False"" {attributes}>
                     <DataGrid.Columns>{columns}</DataGrid.Columns>
                   </DataGrid>");
            grid.ItemsSource = Albums();
            return grid;
        }

        private const string AlbumColumns = @"
                       <DataGridTextColumn Width=""130"" Binding=""{Binding Title}"" Header=""Title"" />
                       <DataGridComboBoxColumn Width=""90"" Header=""Genre"" SelectedValueBinding=""{Binding Genre}"">
                         <DataGridComboBoxColumn.ItemsSource>
                           <x:Array Type=""sys:String"">
                             <sys:String>Jazz</sys:String>
                             <sys:String>Rock</sys:String>
                           </x:Array>
                         </DataGridComboBoxColumn.ItemsSource>
                       </DataGridComboBoxColumn>
                       <mah:DataGridNumericUpDownColumn Width=""90"" Binding=""{Binding Price}""
                                                        Header=""Price"" StringFormat=""C"" Minimum=""0"" />
                       <DataGridCheckBoxColumn Width=""90"" Binding=""{Binding InStock}"" Header=""In stock"" />";

        private static async Task DataGridFiguresAsync()
        {
            const string simple = @"
                       <DataGridTextColumn Width=""140"" Binding=""{Binding Title}"" Header=""Title"" />
                       <DataGridTextColumn Width=""90"" Binding=""{Binding Genre}"" Header=""Genre"" />
                       <DataGridCheckBoxColumn Width=""90"" Binding=""{Binding InStock}"" Header=""In stock"" />";

            await CaptureAsync("styles", "datagrid-styles",
                Showcase(
                    ("implicit style", Grid(@"Height=""150""", simple)),
                    ("MahApps.Styles.DataGrid.Azure",
                        Grid(@"Height=""150"" Style=""{StaticResource MahApps.Styles.DataGrid.Azure}""", simple))));

            await CaptureAsync("styles", "datagrid-columns",
                Showcase(
                    ("four column types, no ElementStyle set",
                        Grid(@"Height=""150""", AlbumColumns))));

            // The editing styles are ordinary styles on ordinary controls, so
            // they can be shown without driving a cell into edit mode.
            await CaptureAsync("styles", "datagrid-editing",
                Showcase(
                    ("TextBox.DataGrid.Editing",
                        Xaml(@"<TextBox Width=""150"" Style=""{StaticResource MahApps.Styles.TextBox.DataGrid.Editing}"" Text=""Kind of Blue"" />")),
                    ("ComboBox.DataGrid.Editing",
                        Xaml(@"<ComboBox Width=""110"" Style=""{StaticResource MahApps.Styles.ComboBox.DataGrid.Editing}"" SelectedIndex=""0"">
                                 <ComboBoxItem Content=""Jazz"" />
                                 <ComboBoxItem Content=""Rock"" />
                               </ComboBox>")),
                    ("NumericUpDown.DataGrid.Editing",
                        Xaml(@"<mah:NumericUpDown Width=""110"" Style=""{StaticResource MahApps.Styles.NumericUpDown.DataGrid.Editing}"" Value=""12.99"" StringFormat=""C"" />"))));
        }

        private static async Task CaptureDropDownAsync(string section, string name, string comboXaml, Action<ComboBox> configure = null)
        {
            var combo = (ComboBox)XamlReader.Parse($"<ComboBox {Xmlns}{comboXaml.Substring("<ComboBox".Length)}");
            configure?.Invoke(combo);

            var host = new Window
                       {
                           Content = new Border { Padding = new Thickness(16), Child = combo },
                           SizeToContent = SizeToContent.WidthAndHeight,
                           WindowStyle = WindowStyle.None,
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

            combo.IsDropDownOpen = true;
            await Task.Delay(600);
            await host.Dispatcher.InvokeAsync(() => { }, DispatcherPriority.ContextIdle);

            if (combo.Template?.FindName("PART_Popup", combo) is System.Windows.Controls.Primitives.Popup popup
                && popup.Child is FrameworkElement child)
            {
                child.UpdateLayout();
                await SaveAsync(section, name, child);
            }
            else
            {
                Console.WriteLine($"{section}/{name}: PART_Popup not found, skipped");
            }

            combo.IsDropDownOpen = false;
            host.Close();
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
            //
            // The month is pinned to one safely in the past. The DatePicker
            // style sets IsTodayHighlighted, so a calendar showing the current
            // month puts an accent fill on whatever day it happens to be - the
            // figure then changes every day the generator runs.
            await CaptureAsync("styles", "datepicker-calendar",
                Showcase(
                    ("MahApps.Styles.Calendar.Base",
                        Xaml(@"<Calendar Style=""{StaticResource MahApps.Styles.Calendar.Base}""
                                         DisplayDate=""2020-06-15"" SelectedDate=""2020-06-15"" />"))));
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

            await SaveAsync(section, name, root);

            window.Close();
        }

        private static Task SaveAsync(string section, string name, FrameworkElement element)
        {
            var width = (int)Math.Ceiling(element.ActualWidth * Scale);
            var height = (int)Math.Ceiling(element.ActualHeight * Scale);
            var bitmap = new RenderTargetBitmap(width, height, 96 * Scale, 96 * Scale, PixelFormats.Pbgra32);
            bitmap.Render(element);

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
            return Task.CompletedTask;
        }
    }
}
