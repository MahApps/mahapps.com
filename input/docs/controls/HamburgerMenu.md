Title: HamburgerMenu
Description: The HamburgerMenu control
---

The HamburgerMenu control provides an easy-to-use, side-bar menu which users can show or hide by using a Hamburger button. By tapping the icon, it opens up a side menu with a selection of options or additional pages.

The 3-line Hamburger menu icon, allows developers to pack more features into their apps or navigation. The tiny icon takes up a minimal amount of screen real estate and creates a clean, minimalist look.

Developers can place menu specific content, navigation, images, text or custom controls.

# Example

![HamburgerMenu](images/hamburgermenu.gif)

# Example Code

This sample demonstrates how to add custom menu items to the HamburgerMenu control.

```xml
<Controls:MetroWindow ...
                      xmlns:Controls="http://metro.mahapps.com/winfx/xaml/controls"
                      xmlns:iconPacks="http://metro.mahapps.com/winfx/xaml/iconpacks">

    <Controls:HamburgerMenu x:Name="HamburgerMenuControl"
                            DisplayMode="CompactOverlay"
                            HamburgerWidth="48"
                            ItemInvoked="HamburgerMenuControl_OnItemInvoked"
                            ItemTemplate="{StaticResource HamburgerMenuItem}"
                            OptionsItemTemplate="{StaticResource HamburgerOptionsMenuItem}">
        <!--  Header  -->
        <Controls:HamburgerMenu.HamburgerMenuHeaderTemplate>
            <DataTemplate>
                <TextBlock HorizontalAlignment="Center"
                           VerticalAlignment="Center"
                           FontSize="16"
                           Foreground="White"
                           Text="Pictures" />
            </DataTemplate>
        </Controls:HamburgerMenu.HamburgerMenuHeaderTemplate>

        <!--  Items  -->
        <Controls:HamburgerMenu.ItemsSource>
            <Controls:HamburgerMenuItemCollection>
                <Controls:HamburgerMenuGlyphItem Glyph="/Assets/Photos/BigFourSummerHeat.png" Label="Big four summer heat" />
                <Controls:HamburgerMenuGlyphItem Glyph="/Assets/Photos/BisonBadlandsChillin.png" Label="Bison badlands Chillin" />
                <Controls:HamburgerMenuGlyphItem Glyph="/Assets/Photos/GiantSlabInOregon.png" Label="Giant slab in Oregon" />
                <Controls:HamburgerMenuGlyphItem Glyph="/Assets/Photos/LakeAnnMushroom.png" Label="Lake Ann Mushroom" />
            </Controls:HamburgerMenuItemCollection>
        </Controls:HamburgerMenu.ItemsSource>

        <!--  Options  -->
        <Controls:HamburgerMenu.OptionsItemsSource>
            <Controls:HamburgerMenuItemCollection>

                <Controls:HamburgerMenuIconItem Label="About">
                    <Controls:HamburgerMenuIconItem.Icon>
                        <iconPacks:PackIconMaterial Width="22"
                                                    Height="22"
                                                    HorizontalAlignment="Center"
                                                    VerticalAlignment="Center"
                                                    Kind="Help" />
                    </Controls:HamburgerMenuIconItem.Icon>
                    <Controls:HamburgerMenuIconItem.Tag>
                        <TextBlock HorizontalAlignment="Center"
                                   VerticalAlignment="Center"
                                   FontSize="28"
                                   FontWeight="Bold">
                            About
                        </TextBlock>
                    </Controls:HamburgerMenuIconItem.Tag>
                </Controls:HamburgerMenuIconItem>

            </Controls:HamburgerMenuItemCollection>
        </Controls:HamburgerMenu.OptionsItemsSource>

        <!--  Content  -->
        <Controls:HamburgerMenu.ContentTemplate>
            <DataTemplate>
                <Grid x:Name="ContentGrid">
                    <Grid.RowDefinitions>
                        <RowDefinition Height="48" />
                        <RowDefinition />
                    </Grid.RowDefinitions>
                    <Border Grid.Row="0"
                            Margin="-1 0 -1 0"
                            Background="#7A7A7A">
                        <TextBlock x:Name="Header"
                                   HorizontalAlignment="Center"
                                   VerticalAlignment="Center"
                                   FontSize="24"
                                   Foreground="White"
                                   Text="{Binding Label}" />
                    </Border>
                    <Controls:TransitioningContentControl Grid.Row="1"
                                                          Content="{Binding}"
                                                          RestartTransitionOnContentChange="True"
                                                          Transition="Default">

                        <Controls:TransitioningContentControl.Resources>
                            <DataTemplate DataType="{x:Type Controls:HamburgerMenuGlyphItem}">
                                <Image Source="{Binding Glyph, Mode=OneWay, Converter={converters:NullToUnsetValueConverter}}" />
                            </DataTemplate>

                            <DataTemplate DataType="{x:Type Controls:HamburgerMenuIconItem}">
                                <ContentControl Content="{Binding Tag, Mode=OneWay}"
                                                Focusable="True"
                                                IsTabStop="False" />
                            </DataTemplate>
                        </Controls:TransitioningContentControl.Resources>

                    </Controls:TransitioningContentControl>
                </Grid>
            </DataTemplate>
        </Controls:HamburgerMenu.ContentTemplate>
    </Controls:HamburgerMenu>

</Controls:MetroWindow>
```

In order to render the items like in the sample above, you need to declare DataTemplate for both item types.

```xml
<Controls:MetroWindow.Resources>
    <ResourceDictionary>

        <!--  This is the template for all menu items. In this sample we use the glyph items.  -->
        <DataTemplate x:Key="HamburgerMenuItem" DataType="{x:Type Controls:HamburgerMenuGlyphItem}">
            <DockPanel Height="48" LastChildFill="True">
                <Grid x:Name="IconPart"
                      Width="{Binding RelativeSource={RelativeSource AncestorType={x:Type Controls:HamburgerMenu}}, Path=CompactPaneLength}"
                      DockPanel.Dock="Left">
                    <Image Margin="12"
                           HorizontalAlignment="Center"
                           VerticalAlignment="Center"
                           Source="{Binding Glyph}" />
                </Grid>
                <TextBlock x:Name="TextPart"
                           VerticalAlignment="Center"
                           FontSize="16"
                           Text="{Binding Label}" />
            </DockPanel>
            <DataTemplate.Triggers>
                <DataTrigger Binding="{Binding RelativeSource={RelativeSource AncestorType={x:Type Controls:HamburgerMenu}}, Path=PanePlacement}" Value="Right">
                    <Setter TargetName="IconPart" Property="DockPanel.Dock" Value="Right" />
                    <Setter TargetName="TextPart" Property="Margin" Value="8 0 0 0" />
                </DataTrigger>
            </DataTemplate.Triggers>
        </DataTemplate>

        <!--  This is the template for the option menu item  -->
        <DataTemplate x:Key="HamburgerOptionsMenuItem" DataType="{x:Type Controls:HamburgerMenuIconItem}">
            <DockPanel Height="48" LastChildFill="True">
                <ContentControl x:Name="IconPart"
                                Width="{Binding RelativeSource={RelativeSource AncestorType={x:Type Controls:HamburgerMenu}}, Path=CompactPaneLength}"
                                Content="{Binding Icon}"
                                DockPanel.Dock="Left"
                                Focusable="False"
                                IsTabStop="False" />
                <TextBlock x:Name="TextPart"
                           VerticalAlignment="Center"
                           FontSize="16"
                           Text="{Binding Label}" />
            </DockPanel>
            <DataTemplate.Triggers>
                <DataTrigger Binding="{Binding RelativeSource={RelativeSource AncestorType={x:Type Controls:HamburgerMenu}}, Path=PanePlacement}" Value="Right">
                    <Setter TargetName="IconPart" Property="DockPanel.Dock" Value="Right" />
                    <Setter TargetName="TextPart" Property="Margin" Value="8 0 0 0" />
                </DataTrigger>
            </DataTemplate.Triggers>
        </DataTemplate>

    </ResourceDictionary>
</Controls:MetroWindow.Resources>
```

You can navigate to the items by using the following code.

```csharp
private void HamburgerMenuControl_OnItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs e)
{
    this.HamburgerMenuControl.Content = e.InvokedItem;

    if (!e.IsItemOptions && this.HamburgerMenuControl.IsPaneOpen)
    {
        // close the menu if a item was selected
        // this.HamburgerMenuControl.IsPaneOpen = false;
    }
}
```

# Properties

# Events

# More Samples
