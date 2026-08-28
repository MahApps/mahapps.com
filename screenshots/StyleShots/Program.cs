using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using MahApps.Metro.Controls;

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
        private static string frameRoot;

        [STAThread]
        public static void Main(string[] args)
        {
            outputRoot = Array.Find(args, a => !a.StartsWith("--")) ?? "shots";
            frameRoot = Array.Find(args, a => a.StartsWith("--frames="))?.Substring("--frames=".Length);

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
                        await ToolTipFiguresAsync();
                        await TextFiguresAsync();
                        await RangeSliderFiguresAsync();
                        await SliderFiguresAsync();
                        await ProgressRingFiguresAsync();
                        await GifFramesAsync();
                        await MetroProgressBarFiguresAsync();
                        await ProgressBarFiguresAsync();
                        await HyperlinkFiguresAsync();
                        await GridSplitterFiguresAsync();
                        await GroupBoxFiguresAsync();
                        await ExpanderFiguresAsync();
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

        // MetroProgressBar and ProgressRing both animate from a VisualState
        // storyboard, and both are started controllable - the first by the
        // control itself, the second by the VisualStateManager. Left running,
        // the dots land wherever the render happens to catch them and the
        // figure changes on every run; seeking the clock to a fixed time and
        // pausing it pins them.
        private static readonly List<(Control Control, string State, TimeSpan At)> pendingSeeks = new();

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

        // The Clean and Visual Studio styles live in dictionaries Controls.xaml
        // does not merge, and the VS one brings its own colours. Merging them
        // globally would recolour every other figure, so each scenario carries
        // the dictionary it needs - which is also how the pages tell you to do it.
        private const string CleanResources = @"
            <ResourceDictionary Source=""pack://application:,,,/MahApps.Metro;component/Styles/Clean/Controls.xaml"" />";

        private const string VsResources = @"
            <ResourceDictionary>
              <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source=""pack://application:,,,/MahApps.Metro;component/Styles/VS/Controls.xaml"" />
                <ResourceDictionary Source=""pack://application:,,,/MahApps.Metro;component/Styles/VS/Colors.xaml"" />
              </ResourceDictionary.MergedDictionaries>
            </ResourceDictionary>";

        private static async Task ToolTipFiguresAsync()
        {
            await CaptureAsync("styles", "tooltip-default",
                Showcase(
                    ("the default tooltip", await TipAsync(@"Content=""Save the current document"""))));

            await CaptureAsync("styles", "tooltip-casing",
                Showcase(
                    ("Normal, the default", await TipAsync(@"Content=""Save the document""")),
                    ("Upper", await TipAsync(@"Content=""Save the document"" mah:ControlsHelper.ContentCharacterCasing=""Upper""")),
                    ("Lower", await TipAsync(@"Content=""Save the document"" mah:ControlsHelper.ContentCharacterCasing=""Lower"""))));

            await CaptureAsync("styles", "tooltip-colours",
                Showcase(
                    ("default", await TipAsync(@"Content=""Save the document""")),
                    ("Background, BorderBrush and Foreground",
                        await TipAsync(@"Content=""Save the document""
                              Background=""{DynamicResource MahApps.Brushes.Accent}""
                              BorderBrush=""{DynamicResource MahApps.Brushes.AccentBase}""
                              Foreground=""{DynamicResource MahApps.Brushes.IdealForeground}""")),
                    ("BorderThickness = 0, Padding = 10 6",
                        await TipAsync(@"Content=""Save the document"" BorderThickness=""0"" Padding=""10 6"""))));

            await CaptureAsync("styles", "tooltip-content",
                Showcase(
                    ("a panel instead of a string",
                        await TipAsync(string.Empty, @"<StackPanel MaxWidth=""220"">
                                              <TextBlock FontWeight=""SemiBold"" Text=""Save"" />
                                              <TextBlock Margin=""0 2 0 0"" TextWrapping=""Wrap""
                                                         Text=""Writes the current document to disk. Ctrl+S does the same."" />
                                            </StackPanel>"))));
        }

        // A ToolTip cannot be given a parent - WPF throws - so it has to be
        // opened in the popup it makes for itself and photographed there. The
        // bitmap then goes into a Showcase like any other element, at exactly
        // the scale it was rendered at, so nothing is resampled.
        private static async Task<FrameworkElement> TipAsync(string attributes, string content = null)
        {
            var tip = (ToolTip)XamlReader.Parse(
                content is null
                    ? $@"<ToolTip {Xmlns} {attributes} />"
                    : $@"<ToolTip {Xmlns} {attributes}>{content}</ToolTip>");

            var anchor = new Border { Width = 10, Height = 10 };
            var host = new Window
                       {
                           Content = anchor,
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

            tip.PlacementTarget = anchor;
            tip.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            tip.IsOpen = true;

            // The template root starts at Opacity 0 and the Open visual state
            // fades it in over 0.3s. Opening it is not always enough to get
            // that state to run off-screen, so once it has had its time the
            // fade is finished by hand - an open tooltip is opaque.
            await Task.Delay(600);
            await host.Dispatcher.InvokeAsync(() => { }, DispatcherPriority.ContextIdle);

            if (tip.Template?.FindName("Root", tip) is FrameworkElement root && root.Opacity < 1)
            {
                root.BeginAnimation(UIElement.OpacityProperty, null);
                root.Opacity = 1;
            }

            await host.Dispatcher.InvokeAsync(() => { }, DispatcherPriority.ContextIdle);
            tip.UpdateLayout();

            var bitmap = Render(tip);

            tip.IsOpen = false;
            host.Close();

            return new Image { Source = bitmap, Width = bitmap.Width, Height = bitmap.Height };
        }

        private static async Task TextFiguresAsync()
        {
            await CaptureAsync("styles", "text-label-textblock",
                Showcase(
                    ("Label", Inherited(@"<Label Background=""#FFE3F2FD"" Content=""Sample text"" />")),
                    ("TextBlock", Inherited(@"<TextBlock Background=""#FFE3F2FD"" Text=""Sample text"" />"))));

            await CaptureAsync("styles", "text-label-casing",
                Showcase(
                    ("Normal, the default", Inherited(@"<Label Content=""Sample text"" mah:ControlsHelper.ContentCharacterCasing=""Normal"" />")),
                    ("Upper", Inherited(@"<Label Content=""Sample text"" mah:ControlsHelper.ContentCharacterCasing=""Upper"" />")),
                    ("Lower", Inherited(@"<Label Content=""Sample text"" mah:ControlsHelper.ContentCharacterCasing=""Lower"" />"))));

            await CaptureAsync("styles", "text-label-accesskey",
                Showcase(
                    (@"Content=""_Name"", RecognizesAccessKey at its default of True",
                        Inherited(@"<Label Content=""_Name"" />")),
                    ("RecognizesAccessKey = False",
                        Inherited(@"<Label Content=""_Name"" mah:ControlsHelper.RecognizesAccessKey=""False"" />"))));

            await CaptureAsync("styles", "text-label-chip",
                Showcase(
                    ("a plain Label", Inherited(@"<Label Content=""Draft"" />")),
                    ("Background, CornerRadius and Padding",
                        Inherited(@"<Label Content=""Draft""
                                          Background=""{DynamicResource MahApps.Brushes.Accent}""
                                          Foreground=""{DynamicResource MahApps.Brushes.IdealForeground}""
                                          Padding=""10 3""
                                          mah:ControlsHelper.CornerRadius=""8"" />")),
                    ("IsEnabled = False", Inherited(@"<Label Content=""Draft"" IsEnabled=""False"" />"))));

            await CaptureAsync("styles", "text-textblock-styles",
                Showcase(
                    ("MahApps.Styles.TextBlock", Inherited(@"<TextBlock Text=""Sample text"" Style=""{DynamicResource MahApps.Styles.TextBlock}"" />")),
                    (".Watermark", Inherited(@"<TextBlock Text=""Sample text"" Style=""{DynamicResource MahApps.Styles.TextBlock.Watermark}"" />")),
                    (".AutoCollapsing", Inherited(@"<TextBlock Text=""Sample text"" Style=""{DynamicResource MahApps.Styles.TextBlock.AutoCollapsing}"" />")),
                    (".AutoCollapsing with no text", Inherited(@"<TextBlock Text="""" Style=""{DynamicResource MahApps.Styles.TextBlock.AutoCollapsing}"" />"))));

            await CaptureAsync("styles", "text-font-sizes",
                Showcase(
                    ("the sizes in Fonts.xaml",
                        Inherited(@"<StackPanel>
                                      <TextBlock Margin=""0 0 0 4"" FontSize=""{DynamicResource MahApps.Font.Size.Header}"" Text=""Header, 40"" />
                                      <TextBlock Margin=""0 0 0 4"" FontSize=""{DynamicResource MahApps.Font.Size.SubHeader}"" Text=""SubHeader, 29.333"" />
                                      <TextBlock Margin=""0 0 0 4"" FontSize=""{DynamicResource MahApps.Font.Size.Window.Title}"" Text=""Window.Title, 16"" />
                                      <TextBlock Margin=""0 0 0 4"" FontSize=""{DynamicResource MahApps.Font.Size.Default}"" Text=""Default, 14"" />
                                      <TextBlock Margin=""0 0 0 4"" FontSize=""{DynamicResource MahApps.Font.Size.Content}"" Text=""Content, 12 - what a MetroWindow hands down"" />
                                      <TextBlock FontSize=""{DynamicResource MahApps.Font.Size.FloatingWatermark}"" Text=""FloatingWatermark, 10"" />
                                    </StackPanel>"))));
        }

        // A bare Window is not a MetroWindow, so nothing hands the figures the
        // font size and foreground that plain text actually inherits in an
        // application. These two attached values are what the MetroWindow style
        // sets, so the figures show what a reader would really see.
        private static FrameworkElement Inherited(string inner)
        {
            return Xaml($@"<Grid TextElement.FontSize=""{{DynamicResource MahApps.Font.Size.Content}}""
                                TextElement.Foreground=""{{DynamicResource MahApps.Brushes.ThemeForeground}}"">
                             {inner}
                           </Grid>");
        }

        private static async Task RangeSliderFiguresAsync()
        {
            await CaptureAsync("controls", "rangeslider-styles",
                Showcase(
                    ("MahApps.Styles.RangeSlider.Win10, the implicit one", Range(string.Empty)),
                    ("MahApps.Styles.RangeSlider", Range(@"Style=""{DynamicResource MahApps.Styles.RangeSlider}"""))));

            await CaptureAsync("controls", "rangeslider-ticks",
                Showcase(
                    ("None", Range(@"TickFrequency=""10""")),
                    ("Both", Range(@"TickFrequency=""10"" TickPlacement=""Both""")),
                    ("IsSelectionRangeEnabled, 20 to 80",
                        Range(@"TickFrequency=""10"" TickPlacement=""Both""
                                IsSelectionRangeEnabled=""True"" SelectionStart=""20"" SelectionEnd=""80"""))));

            await CaptureAsync("controls", "rangeslider-minrangewidth",
                Showcase(
                    ("LowerValue = UpperValue = 50, MinRangeWidth at its default of 30",
                        Range(string.Empty, @"LowerValue=""50"" UpperValue=""50""")),
                    ("the same, MinRangeWidth = 0",
                        Range(@"MinRangeWidth=""0""", @"LowerValue=""50"" UpperValue=""50"""))));

            await CaptureAsync("controls", "rangeslider-brushes",
                Showcase(
                    ("default", Range(string.Empty)),
                    ("SliderHelper brushes",
                        Range(@"mah:SliderHelper.ThumbFillBrush=""#FF1B5E20""
                                mah:SliderHelper.TrackValueFillBrush=""#FF43A047""
                                mah:SliderHelper.TrackFillBrush=""#FFC8E6C9""")),
                    ("IsEnabled = False", Range(@"IsEnabled=""False"""))));

            await CaptureAsync("controls", "rangeslider-vertical",
                Showcase(
                    ("Win10",
                        Xaml(@"<mah:RangeSlider Height=""110"" Orientation=""Vertical""
                                                Minimum=""0"" Maximum=""100"" LowerValue=""30"" UpperValue=""70"" />")),
                    ("MahApps.Styles.RangeSlider",
                        Xaml(@"<mah:RangeSlider Height=""110"" Orientation=""Vertical""
                                                Minimum=""0"" Maximum=""100"" LowerValue=""30"" UpperValue=""70""
                                                Style=""{DynamicResource MahApps.Styles.RangeSlider}"" />"))));
        }

        private static FrameworkElement Range(string attributes, string values = @"LowerValue=""30"" UpperValue=""70""")
        {
            // Minimum and Maximum spelled out every time: unlike the Slider
            // styles, neither RangeSlider style sets them, so they come from
            // RangeBase as 0 and 1.
            return Xaml($@"<mah:RangeSlider Width=""190"" Minimum=""0"" Maximum=""100""
                                            {values} {attributes} />");
        }

        private static async Task SliderFiguresAsync()
        {
            await CaptureAsync("styles", "slider-styles",
                Showcase(
                    ("MahApps.Styles.Slider.Win10, the implicit one", Slide(@"Value=""40""")),
                    ("MahApps.Styles.Slider", Slide(@"Value=""40"" Style=""{DynamicResource MahApps.Styles.Slider}""")),
                    ("MahApps.Styles.Slider.Flat", Slide(@"Value=""40"" Style=""{DynamicResource MahApps.Styles.Slider.Flat}"""))));

            await CaptureAsync("styles", "slider-ticks",
                Showcase(
                    ("None", Slide(@"Value=""40"" TickFrequency=""10""")),
                    ("TopLeft", Slide(@"Value=""40"" TickFrequency=""10"" TickPlacement=""TopLeft""")),
                    ("BottomRight", Slide(@"Value=""40"" TickFrequency=""10"" TickPlacement=""BottomRight""")),
                    ("Both", Slide(@"Value=""40"" TickFrequency=""10"" TickPlacement=""Both"""))));

            await CaptureAsync("styles", "slider-flat",
                Showcase(
                    ("no ticks",
                        Slide(@"Value=""40"" Style=""{DynamicResource MahApps.Styles.Slider.Flat}""")),
                    ("TickPlacement = Both",
                        Slide(@"Value=""40"" TickFrequency=""10"" TickPlacement=""Both"" Style=""{DynamicResource MahApps.Styles.Slider.Flat}""")),
                    ("recoloured through BorderBrush and Foreground",
                        Slide(@"Value=""40"" Style=""{DynamicResource MahApps.Styles.Slider.Flat}""
                                BorderBrush=""#FF1B5E20"" Foreground=""#FF66BB6A"" Background=""#FFC8E6C9"""))));

            await CaptureAsync("styles", "slider-brushes",
                Showcase(
                    ("default", Slide(@"Value=""40""")),
                    ("SliderHelper brushes",
                        Slide(@"Value=""40""
                                mah:SliderHelper.ThumbFillBrush=""#FF1B5E20""
                                mah:SliderHelper.TrackValueFillBrush=""#FF43A047""
                                mah:SliderHelper.TrackFillBrush=""#FFC8E6C9""")),
                    ("IsEnabled = False", Slide(@"Value=""40"" IsEnabled=""False"""))));

            await CaptureAsync("styles", "slider-custom-template",
                Showcase(
                    ("a derived style whose own Template only survives horizontally",
                        Xaml(@"<StackPanel>
                                 <StackPanel.Resources>
                                   <Style x:Key=""Probe"" BasedOn=""{StaticResource MahApps.Styles.Slider.Win10}"" TargetType=""Slider"">
                                     <Setter Property=""Template"">
                                       <Setter.Value>
                                         <ControlTemplate TargetType=""Slider"">
                                           <Border Background=""Red"" />
                                         </ControlTemplate>
                                       </Setter.Value>
                                     </Setter>
                                   </Style>
                                 </StackPanel.Resources>
                                 <Slider Width=""120"" Height=""14"" Margin=""0 0 0 8"" Value=""40"" Style=""{StaticResource Probe}"" />
                                 <Slider Width=""14"" Height=""80"" Orientation=""Vertical"" Value=""40"" Style=""{StaticResource Probe}"" />
                               </StackPanel>"))));

            await CaptureAsync("styles", "slider-vertical",
                Showcase(
                    ("Win10",
                        Xaml(@"<Slider Height=""110"" Orientation=""Vertical"" Value=""40"" />")),
                    ("MahApps.Styles.Slider",
                        Xaml(@"<Slider Height=""110"" Orientation=""Vertical"" Value=""40""
                                       Style=""{DynamicResource MahApps.Styles.Slider}"" />")),
                    ("MahApps.Styles.Slider.Flat",
                        Xaml(@"<Slider Height=""110"" Orientation=""Vertical"" Value=""40""
                                       Style=""{DynamicResource MahApps.Styles.Slider.Flat}"" />"))));
        }

        private static FrameworkElement Slide(string attributes)
        {
            return Xaml($@"<Slider Width=""190"" {attributes} />");
        }

        private static async Task ProgressRingFiguresAsync()
        {
            await CaptureAsync("controls", "progressring-islarge",
                Showcase(
                    (@"IsLarge=""True"" (the default)", Ring(@"IsLarge=""True""")),
                    (@"IsLarge=""False""", Ring(@"IsLarge=""False"""))));

            await CaptureAsync("controls", "progressring-isactive",
                Showcase(
                    (@"IsActive=""True"" (the default)", Ring(@"IsActive=""True""")),
                    (@"IsActive=""False""", Ring(@"IsActive=""False"""))));

            await CaptureAsync("controls", "progressring-sizes",
                Showcase(
                    ("24", Ring(@"Width=""24"" Height=""24""")),
                    ("40", Ring(@"Width=""40"" Height=""40""")),
                    ("60, the default", Ring(string.Empty)),
                    ("100", Ring(@"Width=""100"" Height=""100"""))));

            await CaptureAsync("controls", "progressring-scale",
                Showcase(
                    ("0.5", Ring(@"EllipseDiameterScale=""0.5""")),
                    ("1, the default", Ring(string.Empty)),
                    ("2", Ring(@"EllipseDiameterScale=""2"""))));

            await CaptureAsync("controls", "progressring-brushes",
                Showcase(
                    ("default", Ring(string.Empty)),
                    ("Foreground", Ring(@"Foreground=""#FF107C10""")),
                    ("Background, BorderBrush, BorderThickness, Padding",
                        Ring(@"Background=""{DynamicResource MahApps.Brushes.Gray10}""
                               BorderBrush=""{DynamicResource MahApps.Brushes.Accent}""
                               BorderThickness=""1""
                               Padding=""8"""))));
        }

        // Frames for the animated figures. Off by default - pass
        // --frames=<dir> - because it renders a window per frame and the GIFs
        // are assembled from the PNGs afterwards by make-gif.py.
        //
        // The period of each one is the natural duration of the storyboard
        // that drives it, so sampling exactly that span makes the loop
        // seamless. Delays are centiseconds in a GIF, so the frame counts are
        // chosen to land on a whole number of 50ms steps.
        private static async Task GifFramesAsync()
        {
            if (string.IsNullOrEmpty(frameRoot))
            {
                return;
            }

            // ProgressRing: the last child of the Active storyboard to finish
            // is E6's opacity animation, which begins at 0.835 and runs 3.47.
            await FramesAsync("progressring", 86, 4.305, at =>
                {
                    var ring = (ProgressRing)XamlReader.Parse($@"<mah:ProgressRing {Xmlns} />");
                    pendingSeeks.Add((ring, "Active", at));
                    return ring;
                });

            // MetroProgressBar: MainDoubleAnim is the longest child at 3.917.
            await FramesAsync("metroprogressbar", 78, 3.917, at =>
                {
                    var bar = (MetroProgressBar)XamlReader.Parse(
                        $@"<mah:MetroProgressBar {Xmlns} Width=""190"" Height=""12"" IsIndeterminate=""True"" />");
                    pendingSeeks.Add((bar, "Indeterminate", at));
                    return bar;
                });

            // ProgressBar: one pass of the gradient, 20px in 0.35s.
            await FramesAsync("progressbar", 7, 0.35, at =>
                {
                    var bar = (ProgressBar)XamlReader.Parse(
                        $@"<ProgressBar {Xmlns} Width=""190"" Height=""12"" IsIndeterminate=""True"" />");
                    pendingSeeks.Add((bar, "Indeterminate", at));
                    return bar;
                });
        }

        private static async Task FramesAsync(string name, int count, double period, Func<TimeSpan, FrameworkElement> make)
        {
            var directory = Path.Combine(frameRoot, name);

            for (var i = 0; i < count; i++)
            {
                await CaptureFrameAsync(
                    Path.Combine(directory, $"frame{i:D3}.png"),
                    new Border
                    {
                        Background = new SolidColorBrush(Color.FromRgb(0xF8, 0xF9, 0xFA)),
                        Padding = new Thickness(8),
                        Child = make(TimeSpan.FromSeconds(period * i / count))
                    });
            }

            Console.WriteLine($"{count} {name} frames -> {directory}");
        }

        private static FrameworkElement Ring(string attributes)
        {
            var ring = (ProgressRing)XamlReader.Parse($@"<mah:ProgressRing {Xmlns} {attributes} />");
            // 1.9s is the point in the loop where the dots are spread
            // widest; earlier they bunch into a short arc.
            pendingSeeks.Add((ring, "Active", TimeSpan.FromSeconds(1.9)));
            return ring;
        }

        private static async Task MetroProgressBarFiguresAsync()
        {
            await CaptureAsync("controls", "metroprogressbar-values",
                Showcase(
                    ("Value = 0", MetroBar(@"Value=""0""")),
                    ("Value = 35", MetroBar(@"Value=""35""")),
                    ("Value = 70", MetroBar(@"Value=""70""")),
                    ("Value = 100", MetroBar(@"Value=""100"""))));

            await CaptureAsync("controls", "metroprogressbar-vs-progressbar",
                Showcase(
                    ("ProgressBar", Xaml(@"<ProgressBar Width=""190"" Height=""12"" Value=""70"" />")),
                    ("MetroProgressBar", MetroBar(@"Value=""70"""))));

            await CaptureAsync("controls", "metroprogressbar-track",
                Showcase(
                    ("default", MetroBar(@"Value=""55""")),
                    ("Background", MetroBar(@"Value=""55"" Background=""{DynamicResource MahApps.Brushes.Gray8}""")),
                    ("Foreground", MetroBar(@"Value=""55"" Background=""{DynamicResource MahApps.Brushes.Gray8}"" Foreground=""#FF107C10"""))));

            await CaptureAsync("controls", "metroprogressbar-ellipses",
                Showcase(
                    ("automatic", Dots(@"")),
                    ("EllipseDiameter = 8", Dots(@"EllipseDiameter=""8""")),
                    ("EllipseDiameter = 8, EllipseOffset = 16", Dots(@"EllipseDiameter=""8"" EllipseOffset=""16"""))));

            await CaptureAsync("controls", "metroprogressbar-vertical",
                Showcase(
                    ("Horizontal", MetroBar(@"Value=""60""")),
                    ("Vertical",
                        Xaml(@"<mah:MetroProgressBar Width=""6"" Height=""90"" Orientation=""Vertical"" Value=""60""
                                                    Background=""{DynamicResource MahApps.Brushes.Gray8}"" />"))));
        }

        private static FrameworkElement MetroBar(string attributes)
        {
            return Xaml($@"<mah:MetroProgressBar Width=""190"" {attributes} />");
        }

        // No Background here: the indeterminate storyboard fades DeterminateRoot
        // - which is what holds PART_Track - to zero, so the track is not on
        // screen at all while the dots run.
        private static FrameworkElement Dots(string attributes)
        {
            var bar = (MetroProgressBar)XamlReader.Parse(
                $@"<mah:MetroProgressBar {Xmlns} Width=""190"" Height=""12"" IsIndeterminate=""True"" {attributes} />");
            pendingSeeks.Add((bar, "Indeterminate", TimeSpan.FromSeconds(1.5)));
            return bar;
        }

        // Both templates hang their VisualStateGroups off their root element,
        // which is where the storyboard's clock lives, so the same code pins
        // either one.
        private static void SeekState(Control control, string stateName, TimeSpan at)
        {
            control.ApplyTemplate();

            if (VisualTreeHelper.GetChildrenCount(control) == 0
                || VisualTreeHelper.GetChild(control, 0) is not FrameworkElement root)
            {
                return;
            }

            var storyboard = VisualStateManager.GetVisualStateGroups(root)
                                               ?.OfType<VisualStateGroup>()
                                               .SelectMany(group => group.States.OfType<VisualState>())
                                               .FirstOrDefault(state => state.Name == stateName)
                                               ?.Storyboard;

            storyboard?.SeekAlignedToLastTick(root, at, TimeSeekOrigin.BeginTime);
            storyboard?.Pause(root);
        }

        private static async Task ProgressBarFiguresAsync()
        {
            await CaptureAsync("styles", "progressbar-values",
                Showcase(
                    ("Value = 0", Bar(@"Value=""0""")),
                    ("Value = 35", Bar(@"Value=""35""")),
                    ("Value = 70", Bar(@"Value=""70""")),
                    ("Value = 100", Bar(@"Value=""100"""))));

            // The template fills the indicator from MahApps.Brushes.Progress,
            // not from a TemplateBinding on Foreground, so the middle panel is
            // the point of the figure: nothing happens.
            await CaptureAsync("styles", "progressbar-brushes",
                Showcase(
                    ("default", Bar(@"Value=""70""")),
                    ("Foreground = Red", Bar(@"Value=""70"" Foreground=""Red""")),
                    ("MahApps.Brushes.Progress replaced",
                        Xaml(@"<ProgressBar Width=""190"" Height=""12"" Value=""70"">
                                 <ProgressBar.Resources>
                                   <SolidColorBrush x:Key=""MahApps.Brushes.Progress"" Color=""#FF107C10"" />
                                 </ProgressBar.Resources>
                               </ProgressBar>"))));

            await CaptureAsync("styles", "progressbar-track",
                Showcase(
                    ("default", Bar(@"Value=""55""")),
                    ("Background and BorderBrush",
                        Bar(@"Value=""55"" Background=""#FFEDE7F6"" BorderBrush=""#FF7E57C2""")),
                    ("BorderThickness = 0, Height = 6",
                        Xaml(@"<ProgressBar Width=""190"" Height=""6"" Value=""55"" BorderThickness=""0"" />"))));

            await CaptureAsync("styles", "progressbar-vertical",
                Showcase(
                    ("Horizontal", Bar(@"Value=""60""")),
                    ("Vertical",
                        Xaml(@"<ProgressBar Width=""12"" Height=""90"" Orientation=""Vertical"" Value=""60"" />"))));
        }

        private static FrameworkElement Bar(string attributes)
        {
            return Xaml($@"<ProgressBar Width=""190"" Height=""12"" {attributes} />");
        }

        private static async Task HyperlinkFiguresAsync()
        {
            // IsMouseOver cannot be forced on a Hyperlink and an off-screen
            // render has no pointer, so the middle panel paints the brush the
            // trigger would apply rather than pretending to be a real hover.
            // The caption says so.
            await CaptureAsync("styles", "hyperlink-states",
                Showcase(
                    ("normal", Xaml(@"<TextBlock FontSize=""14""><Hyperlink>MahApps.Metro</Hyperlink></TextBlock>")),
                    ("mouse over, the brush the trigger applies",
                        Xaml(@"<TextBlock FontSize=""14""><Hyperlink Foreground=""{DynamicResource MahApps.Brushes.Highlight}"">MahApps.Metro</Hyperlink></TextBlock>")),
                    ("disabled", Xaml(@"<TextBlock FontSize=""14""><Hyperlink IsEnabled=""False"">MahApps.Metro</Hyperlink></TextBlock>"))));

            await CaptureAsync("styles", "hyperlink-inline",
                Showcase(
                    ("a link inside running text",
                        Xaml(@"<TextBlock Width=""330"" FontSize=""14"" TextWrapping=""Wrap"">
                                 The source code is <Hyperlink>hosted on GitHub</Hyperlink> and includes
                                 everything needed to build it yourself.
                               </TextBlock>"))));
        }

        private static async Task GridSplitterFiguresAsync()
        {
            await CaptureAsync("styles", "gridsplitter-directions",
                Showcase(
                    ("between columns, Width set", Xaml(@"
                        <Grid Width=""260"" Height=""110"">
                          <Grid.ColumnDefinitions>
                            <ColumnDefinition Width=""*"" />
                            <ColumnDefinition Width=""Auto"" />
                            <ColumnDefinition Width=""*"" />
                          </Grid.ColumnDefinitions>
                          <Border Background=""{DynamicResource MahApps.Brushes.Gray10}"">
                            <TextBlock Margin=""10"" VerticalAlignment=""Center"" Text=""left"" />
                          </Border>
                          <GridSplitter Grid.Column=""1"" Width=""4"" />
                          <Border Grid.Column=""2"" Background=""{DynamicResource MahApps.Brushes.Gray10}"">
                            <TextBlock Margin=""10"" VerticalAlignment=""Center"" Text=""right"" />
                          </Border>
                        </Grid>")),
                    ("between rows, Height set", Xaml(@"
                        <Grid Width=""200"" Height=""130"">
                          <Grid.RowDefinitions>
                            <RowDefinition Height=""*"" />
                            <RowDefinition Height=""Auto"" />
                            <RowDefinition Height=""*"" />
                          </Grid.RowDefinitions>
                          <Border Background=""{DynamicResource MahApps.Brushes.Gray10}"">
                            <TextBlock Margin=""10"" VerticalAlignment=""Center"" Text=""top"" />
                          </Border>
                          <GridSplitter Grid.Row=""1"" Height=""4"" />
                          <Border Grid.Row=""2"" Background=""{DynamicResource MahApps.Brushes.Gray10}"">
                            <TextBlock Margin=""10"" VerticalAlignment=""Center"" Text=""bottom"" />
                          </Border>
                        </Grid>"))));

            await CaptureAsync("styles", "gridsplitter-look",
                Showcase(
                    ("default, 4 wide", Xaml(@"
                        <Grid Width=""220"" Height=""90"">
                          <Grid.ColumnDefinitions>
                            <ColumnDefinition Width=""*"" />
                            <ColumnDefinition Width=""Auto"" />
                            <ColumnDefinition Width=""*"" />
                          </Grid.ColumnDefinitions>
                          <Border Background=""{DynamicResource MahApps.Brushes.Gray10}"" />
                          <GridSplitter Grid.Column=""1"" Width=""4"" />
                          <Border Grid.Column=""2"" Background=""{DynamicResource MahApps.Brushes.Gray10}"" />
                        </Grid>")),
                    ("wider and recoloured", Xaml(@"
                        <Grid Width=""220"" Height=""90"">
                          <Grid.ColumnDefinitions>
                            <ColumnDefinition Width=""*"" />
                            <ColumnDefinition Width=""Auto"" />
                            <ColumnDefinition Width=""*"" />
                          </Grid.ColumnDefinitions>
                          <Border Background=""{DynamicResource MahApps.Brushes.Gray10}"" />
                          <GridSplitter Grid.Column=""1"" Width=""10"" Background=""{DynamicResource MahApps.Brushes.Accent}"" />
                          <Border Grid.Column=""2"" Background=""{DynamicResource MahApps.Brushes.Gray10}"" />
                        </Grid>"))));
        }

        private static async Task GroupBoxFiguresAsync()
        {
            await CaptureAsync("styles", "groupbox-styles",
                Showcase(
                    ("implicit style", Xaml(@"<GroupBox Width=""190"" Header=""Details"">
                                                <TextBlock Margin=""4"" Text=""some content"" />
                                              </GroupBox>")),
                    ("MahApps.Styles.GroupBox.Clean",
                        Xaml($@"<GroupBox Width=""190"" Header=""Details"" Style=""{{DynamicResource MahApps.Styles.GroupBox.Clean}}"">
                                  <GroupBox.Resources>{CleanResources}</GroupBox.Resources>
                                  <TextBlock Margin=""4"" Text=""some content"" />
                                </GroupBox>")),
                    // The Visual Studio style is built for the dark VS theme -
                    // on white its header all but disappears - so this panel
                    // gets the backdrop it was drawn for.
                    ("MahApps.Styles.GroupBox.VisualStudio, on a dark backdrop",
                        Xaml($@"<Border Background=""#2D2D30"" Padding=""12"">
                                  <GroupBox Width=""190"" Header=""Details"" Style=""{{DynamicResource MahApps.Styles.GroupBox.VisualStudio}}"">
                                    <GroupBox.Resources>{VsResources}</GroupBox.Resources>
                                    <TextBlock Margin=""4"" Text=""some content"" />
                                  </GroupBox>
                                </Border>"))));

            await CaptureAsync("styles", "groupbox-casing",
                Showcase(
                    ("Upper (default)", Xaml(@"<GroupBox Width=""170"" Header=""Details"" mah:ControlsHelper.ContentCharacterCasing=""Upper"">
                                                 <TextBlock Margin=""4"" Text=""some content"" />
                                               </GroupBox>")),
                    ("Normal", Xaml(@"<GroupBox Width=""170"" Header=""Details"" mah:ControlsHelper.ContentCharacterCasing=""Normal"">
                                        <TextBlock Margin=""4"" Text=""some content"" />
                                      </GroupBox>")),
                    ("Lower", Xaml(@"<GroupBox Width=""170"" Header=""Details"" mah:ControlsHelper.ContentCharacterCasing=""Lower"">
                                       <TextBlock Margin=""4"" Text=""some content"" />
                                     </GroupBox>"))));

            await CaptureAsync("styles", "groupbox-header",
                Showcase(
                    ("default", Xaml(@"<GroupBox Width=""190"" Header=""Details"">
                                         <TextBlock Margin=""4"" Text=""some content"" />
                                       </GroupBox>")),
                    ("recoloured header", Xaml(@"<GroupBox Width=""190"" Header=""Details""
                                                           mah:HeaderedControlHelper.HeaderBackground=""#2E7D32""
                                                           mah:HeaderedControlHelper.HeaderForeground=""White""
                                                           mah:HeaderedControlHelper.HeaderFontSize=""16""
                                                           mah:HeaderedControlHelper.HeaderMargin=""10 6"">
                                                    <TextBlock Margin=""4"" Text=""some content"" />
                                                  </GroupBox>"))));
        }

        private static async Task ExpanderFiguresAsync()
        {
            await CaptureAsync("styles", "expander-styles",
                Showcase(
                    ("implicit style", Xaml(@"<Expander Width=""190"" Header=""Details"" IsExpanded=""True"">
                                                <TextBlock Margin=""8"" Text=""some content"" />
                                              </Expander>")),
                    ("MahApps.Styles.Expander.VisualStudio, on a dark backdrop",
                        Xaml($@"<Border Background=""#2D2D30"" Padding=""12"">
                                  <Expander Width=""190"" Header=""Details"" IsExpanded=""True"" Style=""{{DynamicResource MahApps.Styles.Expander.VisualStudio}}"">
                                    <Expander.Resources>{VsResources}</Expander.Resources>
                                    <TextBlock Margin=""8"" Text=""some content"" />
                                  </Expander>
                                </Border>"))));

            await CaptureAsync("styles", "expander-directions",
                Showcase(
                    ("Down (default)", Xaml(@"<Expander Width=""150"" Header=""Details"" IsExpanded=""True"">
                                                <TextBlock Margin=""8"" Text=""content"" />
                                              </Expander>")),
                    ("Up", Xaml(@"<Expander Width=""150"" Header=""Details"" ExpandDirection=""Up"" IsExpanded=""True"">
                                    <TextBlock Margin=""8"" Text=""content"" />
                                  </Expander>")),
                    ("Right", Xaml(@"<Expander Height=""110"" Header=""Details"" ExpandDirection=""Right"" IsExpanded=""True"">
                                       <TextBlock Margin=""8"" VerticalAlignment=""Center"" Text=""content"" />
                                     </Expander>")),
                    ("Left", Xaml(@"<Expander Height=""110"" Header=""Details"" ExpandDirection=""Left"" IsExpanded=""True"">
                                      <TextBlock Margin=""8"" VerticalAlignment=""Center"" Text=""content"" />
                                    </Expander>"))));

            // The built-in Left and Right header styles leave the text
            // horizontal in a narrow vertical band. Rotating it is the caller's
            // job. Doing it through HeaderTemplate rather than by putting a
            // TextBlock in Header keeps both the inherited foreground and the
            // style's upper-casing - an element assigned to Header loses both.
            await CaptureAsync("styles", "expander-vertical-header",
                Showcase(
                    ("Right, header rotated -90", Xaml(@"<Expander Height=""170"" ExpandDirection=""Right"" IsExpanded=""True"" Header=""Details"">
                                                           <Expander.HeaderTemplate>
                                                             <DataTemplate>
                                                               <TextBlock VerticalAlignment=""Center"" Text=""{Binding}"">
                                                                 <TextBlock.LayoutTransform>
                                                                   <RotateTransform Angle=""-90"" />
                                                                 </TextBlock.LayoutTransform>
                                                               </TextBlock>
                                                             </DataTemplate>
                                                           </Expander.HeaderTemplate>
                                                           <TextBlock Margin=""12"" VerticalAlignment=""Center"" Text=""some content"" />
                                                         </Expander>")),
                    ("Left, header rotated 90", Xaml(@"<Expander Height=""170"" ExpandDirection=""Left"" IsExpanded=""True"" Header=""Details"">
                                                         <Expander.HeaderTemplate>
                                                           <DataTemplate>
                                                             <TextBlock VerticalAlignment=""Center"" Text=""{Binding}"">
                                                               <TextBlock.LayoutTransform>
                                                                 <RotateTransform Angle=""90"" />
                                                               </TextBlock.LayoutTransform>
                                                             </TextBlock>
                                                           </DataTemplate>
                                                         </Expander.HeaderTemplate>
                                                         <TextBlock Margin=""12"" VerticalAlignment=""Center"" Text=""some content"" />
                                                       </Expander>"))));

            await CaptureAsync("styles", "expander-collapsed",
                Showcase(
                    ("collapsed", Xaml(@"<Expander Width=""190"" Header=""Details"">
                                           <TextBlock Margin=""8"" Text=""some content"" />
                                         </Expander>")),
                    ("expanded", Xaml(@"<Expander Width=""190"" Header=""Details"" IsExpanded=""True"">
                                          <TextBlock Margin=""8"" Text=""some content"" />
                                        </Expander>"))));
        }

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

        private static Task CaptureAsync(string section, string name, FrameworkElement root)
        {
            return CaptureAsync(root, element => SaveAsync(section, name, element));
        }

        private static Task CaptureFrameAsync(string path, FrameworkElement root)
        {
            return CaptureAsync(root, element => SaveToPathAsync(path, element, quiet: true));
        }

        private static async Task CaptureAsync(FrameworkElement root, Func<FrameworkElement, Task> save)
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

            foreach (var (control, state, at) in pendingSeeks)
            {
                SeekState(control, state, at);
            }

            pendingSeeks.Clear();

            // Nothing here animates, but the floating watermark and the reveal
            // button appear through the bindings above; let them settle.
            await Task.Delay(400);

            // Whether a control wears the focus border depends on where keyboard
            // focus landed, which is not something the scenario says. Clear it.
            Keyboard.ClearFocus();
            FocusManager.SetFocusedElement(window, null);

            await window.Dispatcher.InvokeAsync(() => { }, DispatcherPriority.ContextIdle);

            root.UpdateLayout();

            await save(root);

            window.Close();
        }

        private static Task SaveAsync(string section, string name, FrameworkElement element)
        {
            var directory = Path.Combine(outputRoot, section, "images");
            Directory.CreateDirectory(directory);

            return SaveToPathAsync(Path.Combine(directory, name + ".png"), element, quiet: false, label: $"{section}/{name}.png");
        }

        private static RenderTargetBitmap Render(FrameworkElement element)
        {
            var bitmap = new RenderTargetBitmap((int)Math.Ceiling(element.ActualWidth * Scale),
                                                (int)Math.Ceiling(element.ActualHeight * Scale),
                                                96 * Scale,
                                                96 * Scale,
                                                PixelFormats.Pbgra32);
            bitmap.Render(element);
            return bitmap;
        }

        private static Task SaveToPathAsync(string path, FrameworkElement element, bool quiet, string label = null)
        {
            var bitmap = Render(element);
            var width = bitmap.PixelWidth;
            var height = bitmap.PixelHeight;

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            using (var stream = File.Create(path))
            {
                encoder.Save(stream);
            }

            if (!quiet)
            {
                Console.WriteLine($"{label ?? path}  {width}x{height}");
            }

            return Task.CompletedTask;
        }
    }
}
