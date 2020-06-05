Title: FlipView
Description: A control with swipe navigation
---

The `FlipView` control behaves like an ItemsControl and shows items one by one on swipe gesture. Also navigation buttons will be available to navigate using mouse.

It is inspired by Windows 8 / WinRT's control of the same name. However, ours was written from the ground-up to support the `MahApps.Metro` infrastructure. 

## Getting started

The `FlipView` control works similar to the regular `Selector` (`ItemsControl`) control. Place your content inside of it's `Items` property and it will allow to *flip* through them.

The following code was taken from our main [MetroDemo](https://github.com/MahApps/MahApps.Metro).

```xml
<mah:FlipView x:Name="FlipView1st"
              Height="200"
              Margin="0 0 5 0"
              Foreground="{DynamicResource MahApps.Brushes.ThemeBackground}"
              SelectionChanged="FlipView_SelectionChanged">
    <mah:FlipView.Items>
        <Grid Background="#2E8DEF">
            <iconPacks:PackIconModern Width="60"
                                      Height="60"
                                      HorizontalAlignment="Center"
                                      VerticalAlignment="Center"
                                      Kind="FoodCupcake" />
        </Grid>
        <Grid Background="#00A600">
            <iconPacks:PackIconModern Width="60"
                                      Height="60"
                                      HorizontalAlignment="Center"
                                      VerticalAlignment="Center"
                                      Kind="Xbox" />
        </Grid>
        <Grid Background="#BF1E4B">
            <iconPacks:PackIconModern Width="60"
                                      Height="60"
                                      HorizontalAlignment="Center"
                                      VerticalAlignment="Center"
                                      Kind="ChessHorse" />
        </Grid>
    </mah:FlipView.Items>
</mah:FlipView>
```

![flipview](images/flipview.png)

## The Banner

The banner on the bottom of the `FlipView` can be shown and hidden using the `IsBannerEnabled` property. You may change the banner text using the `BannerText` property. We use that in code behind to change the banner based on the selected item.

```csharp
private void FlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
{
    var flipview = ((FlipView)sender);
    switch (flipview.SelectedIndex)
    {
        case 0:
            flipview.BannerText = "Cupcakes!";
            break;
        case 1:
            flipview.BannerText = "Xbox!";
            break;
        case 2:
            flipview.BannerText = "Chess!";
            break;
    }
}
```

## The Control Buttons

The *control buttons* (the next and previous buttons) allow the user to flip through the items using their mouse. The buttons can be disabled by calling `HideControlButtons` and renabled by calling `ShowControlButtons`.

The user can also flip through the items using the arrows on their keyboard.

## Automated scrolling (batteries not included)

Disabling the control buttons is useful when you want to provide an automated scrolling experience. This can be implemented by using a timer and by incrementing `SelectedIndex`.
