Title: ColorPicker
Description: The documentation for the ColorPicker, ColorCanvas, ColorPallette and ColorEyeDropper
---
**On this page**
<!-- Start Document Outline -->

* [Introduction](#introduction)
* [ColorNamesDictionary and ColorHelper](#colornamesdictionary-and-colorhelper)
	* [Looking up a colors name](#looking-up-a-colors-name)
	* [Getting a color from a given name](#getting-a-color-from-a-given-name)
	* [How to provide custom color names](#how-to-provide-custom-color-names)
	* [Provide translations for a language of your choice](#provide-translations-for-a-language-of-your-choice)
* [ColorCanvas](#colorcanvas)
	* [The user interface](#the-user-interface)
	* [Properties](#properties)
	* [Events](#events)
	* [DynamicResources](#dynamicresources)
	* [Example](#example)

<!-- End Document Outline -->
---

# Introduction

The `ColorPicker`-Controls can be used to select any `System.Windows.Media.Color` (in the following lines called `Color`). There are four different controls available to select the a `Color`:

| Control           | Use case                                  |
|-------------------|------------------------------------------|
| `ColorCanvas`     | Select a `Color` by specifying its [ARGB](https://en.wikipedia.org/wiki/RGB_color_model) or [AHSV](https://en.wikipedia.org/wiki/HSL_and_HSV) channels |
| `ColorPalette`    | Select a `Color` from predefined colors presented in a special `ListBox` |
| `ColorEyeDropper` | Select a `Color` from anywhere on the screen |
| `ColorPicker`     | A combination of the three controls above presented in a `ComboBox`-like control |

# ColorNamesDictionary and ColorHelper
Colors may have a name which is localize-able if you want to. The color names are stored in a `Dictionary<Color?,string>` which is used to get the name of the provided `Color`. 

## Looking up a colors name
If you want to look up a name of a given `Color` use this line: 

```c#
// Note: If you set the second parameter to 'null' the default dictionary will be used. 
string nameOfMyColor = MahApps.Metro.Controls.ColorHelper.GetColorName(myColor, theDictionaryToUse);
```

## Getting a color from a given name
You can also get back the color by looking up its name. The routine will first check the dictionary for the first occurrence of the given name and if it was not found it will try to use [`ColorConverter.ConvertFromString`](docs.microsoft.com/en-us/dotnet/api/system.windows.media.colorconverter.convertfromstring) which also accepts the HTML-Notation of the `Color`. If the `Color` cannot be found it will return null. Blow you can find some examples.

```
// Note: If you set the second parameter to 'null' the default dictionary will be used.  
Color? myColor = MahApps.Metro.Controls.ColorHelper.ColorFromString(myColorName, theDictionaryToUse);

// this will look up the German word "Blau" and returns blue color
Color? myColor = MahApps.Metro.Controls.ColorHelper.ColorFromString("Blau", null);

// this will look up the HTML-notation "#FF000000" and returns black color
Color? myColor = MahApps.Metro.Controls.ColorHelper.ColorFromString("#FF000000", null);
```


## How to provide custom color names
You can change the build in dictionary via `MahApps.Metro.Controls.ColorHelper.ColorNamesDictionary` by adding, removing or changing any dictionary entry. You can also create your own `Dictionary<Color?,string>` anywhere in your `Model` or `ViewModel` to provide your own color names. That way you can use also different dictionaries for different `ColorPicker`. 

Example:
```c#
Dictionary<Color?,string> myColorNames = new Dictionary<Color?,string>()
{
    { Colors.Green, "every thing is fine" }, 
    { Colors.Yellow, "warning" },
    { Colors.Red, "error" }
};
```
## Provide translations for a language of your choice
You can help providing translations to the build in dictionary. Currently implemented language:
- English
- German

To do so we recommend using the [ResXManager](https://marketplace.visualstudio.com/items?itemName=TomEnglert.ResXManager) and please read [this guide](https://github.com/MahApps/MahApps.Metro/wiki/Contributing)

# ColorCanvas
The `ColorCanvas` lets the user select a `Color` by either manipulating the ARGB-Values, manipulating the HSV-Values, entering the color name or HTML-notation or picking a color from the screen

## The user interface
![](images/ColorPicker_ColorCanvas_Numbered.png)

| No. | Description                              |
|-----|------------------------------------------|
| 01  | red color channel from 0 to 255          |
| 02  | green color channel from 0 to 255        |
| 03  | blue color channel from 0 to 255         |
| 04  | hue channel from 0° to 360°              |
| 05  | saturation channel from 0% to 100%       |
| 06  | value channel from 0% to 100 %           |
| 07  | alpha channel from 0 to 255              |
| 08  | the color name or html-notation          |
| 09  | a `ColorEyeDropper` to select a color from the screen |
| 10  | select the saturation by moving the cross-hair from horizontally and the value by moving the cross-hair vertically |

## Properties
Use the following properties to handle the color selection 

| Property             | Type                       | Description                              |
|----------------------|----------------------------|------------------------------------------|
| SelectedColor        | `Color?`                     | Gets or sets the selected color          |
| DefaultColor         | `Color?`                     | Gets or sets the default color if the `SelectedColor` is `null` |
| SelectedHSVColor     | `HSVColor`                   | Gets the selected color as `HSVColor`    |
| ColorName            | `string`                     | Gets or sets the name of the selected color [(see also)](#colornamesdictionary-and-colorhelper) |
| ColorNamesDictionary | `Dictionary<Color?, string>` | Gets or sets the `Dictionary<Color?, string>` used to get or set the `ColorName` [(see also)](#colornamesdictionary-and-colorhelper)|
| A                    | `byte`                       | Gets or sets the alpha channel           |
| R                    | `byte`                       | Gets or sets the red channel             |
| G                    | `byte`                       | Gets or sets the green channel           |
| B                    | `byte`                       | Gets or sets the blue-channel            |
| Hue                  | `double`                     | Gets or sets the hue channel             |
| Saturation           | `double`                     | Gets or sets the Saturation-channel      |
| Value                | `double`                     | Gets or sets the Value-channel           |

Use the following properties to translate the user interface

| Property               | Type   | Description                              |
|------------------------|--------|------------------------------------------|
| LabelAlphaChannel      | `string` | Gets or sets the `Label` for the alpha-channel |
| LabelRedChannel        | `string` | Gets or sets the `Label` for the red-channel |
| LabelGreenChannel      | `string` | Gets or sets the `Label` for the green-channel |
| LabelBlueChannel       | `string` | Gets or sets the `Label` for the blue-channel |
| LabelHueChannel        | `string` | Gets or sets the `Label` for the hue-channel |
| LabelSaturationChannel | `string` | Gets or sets the `Label` for the saturation-channel |
| LabelValueChannel      | `string` | Gets or sets the `Label` for the value-channel |
| LabelColorPreview      | `string` | Gets or sets the `Label` for the preview field |
| LabelColorName         | `string` | Gets or sets the `Label` for the color name |

## Events
| Event                | Description                            |
|----------------------|----------------------------------------|
| SelectedColorChanged | occurs when the SelectedColor changed |

## DynamicResources
You can override the following `Resources` to modify the appearance of the `ColorCanvas` further. 

| Key                                      | Type         | Description                              |
|------------------------------------------|--------------|------------------------------------------|
| `MahApps.Brushes.Tile`                     | `Brush`        | overrides the checkered `Brush` which is visible if the color is transparent |
| `MahApps.DataTemplates.ColorPicker.NoColor` | `DataTemplate` | overrides the `DataTemplate` of the preview if no color is selected |
| `MahApps.Styles.Slider.ColorComponent.ARGB` | `Style`        | overrides the Style of the A-, R-, G- and B-`Slider` |
| `MahApps.Styles.Slider.ColorComponent.Hue` | `Style`        | overrides the `Style` of the Hue-Slider    |
| `MahApps.Styles.Slider.ColorComponent.SV`  | `Style`        | overrides the `Style` of the S- and V-`Slider` |
| `MahApps.Styles.ColorEyeDropper.ColorCanvas` | `Style`        | overrides the Style of the `ColorEyeDropper` |


## Example

```xaml
<!-- make sure to add the right namespace -->
<!-- xmlns:mah="http://metro.mahapps.com/winfx/xaml/controls" -->

<mah:ColorCanvas x:Name="ColorCanvasExample"
                 SelectedColor="Blue"
                 DefaultColor="Transparent"
                 LabelAlphaChannel="Alpha"
                 LabelBlueChannel="Blue"
                 LabelGreenChannel="Green"
                 LabelRedChannel="Red" />
```

# ColorPalette
The `ColorPalette` can be used to present the user a swatch of predefined colors. As this control is derived from `System.Windows.Controls.ListBox` you can use all functionality know from the `ListBox`-Control

## The user interface

![](images/ColorPicker_ColorPalette_Numbered.png)

| No | Description                              |
|----|------------------------------------------|
| 01 | The header of the `ColorPalette`         |
| 02 | Displays the available colors            |
| 03 | The currently selected color is highlighted |

## Properties
The available colors can be either added directly to the `Items`-property or by binding to the `ItemsSource`-property. The selection can be handled by binding to `SelectedValue`, `SelectedItem` or `SelectedIndex`. 

In addition to this the `ColorPalette` provides the following properties.

| Property             | Type                       | Description                              |
|----------------------|----------------------------|------------------------------------------|
| Header               | `object`                     | Gets or sets the header content          |
| HeaderTemplate       | `DataTemplate`               | Gets or sets the header template         |
| ColorNamesDictionary | `Dictionary<Color?, string>` | Gets or sets the `Dictionary<Color?, string>` used to get or set the ColorName [(see also)](#colornamesdictionary-and-colorhelper) |

## DynamicResources
| Key                                      | Type         | Description                              |
|------------------------------------------|--------------|------------------------------------------|
| `MahApps.Sizes.ColorListBox.ItemWidth`     | `double`       | overrides the width of the items         |
| `MahApps.Sizes.ColorListBox.ItemHeight`    | `double`       | overrides the height of the items        |
| `MahApps.Brushes.Tile.Small`               | `Brush`        | overrides the checkered Brush which is visible if the color is transparent |
| `MahApps.Styles.ListBoxItem.ColorPaletteItem` | `Style`        | overrides the `Style` of the items       |
| `MahApps.Templates.ColorPaletteItem`       | `DataTemplate` | overrides the `DataTemplate` of the items |

## Example
The below example shows how to use the `ColorPalette` via setting the `ItemsSource` to a build in `ColorPalette`. 

> Note: MahApps provides [build in color palettes](#build-in-color-palettes)

```xaml
<!-- make sure to add the right namespace -->
<!-- xmlns:mah="http://metro.mahapps.com/winfx/xaml/controls" -->

<mah:ColorPalette Header="An Example Palette" 
                  ItemsSource="{x:Static mah:BuildInColorPalettes.WpfColorsPalette}" />
``` 

The second example shows how to add colors directly in `XAML`

```xaml
<!-- make sure to add the right namespace -->
<!-- xmlns:mah="http://metro.mahapps.com/winfx/xaml/controls" -->

<mah:ColorPalette Header="A second Example Palette" >
    <Color>Red</Color>
    <Color>Green</Color>
    <Color>Blue</Color>
</mah:ColorPalette>
```

## Build in color palettes 
MahApps provides the following build in ColorPalettes:
- WpfColorsPalette (all colors in `Sytem.Windows.Media.Colors`)
- StandardColorsPalette (the primary colors)
- RecentColors (used to store the recently selected colors)

All build in `ColorPalettes` are internally an `ObservableCollection`, so you modify them to your needs.

# ColorEyeDropper
The `ColorEyeDropper` lets the user select a `Color` with the mouse directly from anywhere on the screen, even outside of your `Application`. As this control is derived from `System.Windows.Controls.Button` you can use all functionality know from the `Button`-Control.

## The user interface
If the user presses the left mouse button on the `ColorEyeDropper` the cursor will change to an eye-dropper symbol and a preview popup will appear. Now the user needs to keep the mouse pressed while dragging the mouse to the desired pixel. A magnifier with a cross hair helps getting the exact pixel position. As soon as the user releases the left mouse button the `SelectedColor`-property will get updated. 

![](images/ColorPicker_ColorEyeDropper_Numbered.png)

| No | Description                              |
|----|------------------------------------------|
| 01 | The `Button` to start the `ColorEyeDropper` |
| 02 | The cursor while selecting a color       |
| 03 | A preview of the selected color          |
| 04 | A magnifier with a cross hair            |

**See it in action:**
![](images/ColorPicker_ColorEyeDropper_Running.png)

## Properties
In addition to all known `Button`-properties the `ColorEyeDropper` supports the properties listed below.

| Property                    | Type         | Description                              |
|-----------------------------|--------------|------------------------------------------|
| SelectedColor               | `Color?`       | Gets or sets the selected color          |
| PreviewImageOuterPixelCount | `int`          | Gets or sets how many pixels the preview magnifier should render around the curent mouse position. The default is 2 |
| EyeDropperCursor            | `Cursor`       | Gets or sets the `Cursor` when in selection mode |
| PreviewContentTemplate      | `DataTemplate` | Gets or sets the `DataTemplate` of the preview `Popup` |

## DynamicResources
For this control there is no `DynamicResource` which can be overridden beside the know ones.

## Example
```xaml
<!-- make sure to add the right namespace -->
<!-- xmlns:mah="http://metro.mahapps.com/winfx/xaml/controls" -->

<mah:ColorEyeDropper Content="This is my EyeDropper"
                     SelectedColor="{Binding Path=MyColorToBind}" />
```