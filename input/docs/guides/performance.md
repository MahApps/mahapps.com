Order: 30
Title: Performance with many views
Description: Why an application can slow down as views come and go, and what to do about it
---

An application that opens and closes a lot of views can grow slower the longer it runs. Switching to a view that took a moment at first takes several seconds later on, and nothing about that view has changed.

This is a WPF thing rather than a MahApps one, but MahApps styles run into it readily, so it is worth knowing about.

## What happens

A `DynamicResource` is not looked up once and forgotten. WPF keeps a deferred reference to it, and a `ResourceDictionary` keeps a list of those. Every element that holds such a reference adds an entry, and the list is walked when a resource changes.

MahApps styles use `DynamicResource` heavily, and the helper attached properties add more of them: every brush a control might need is set, whether that control reads it or not. In the project linked below, a handful of views is enough to put eighty thousand entries in that list.

[oatkins](https://github.com/oatkins) built a project that shows it, and reported it to the WPF team as [dotnet/wpf#4468](https://github.com/dotnet/wpf/issues/4468) and to us as [MahApps.Metro#4100](https://github.com/MahApps/MahApps.Metro/issues/4100).

## On .NET 10 it is gone

[dotnet/wpf#5610](https://github.com/dotnet/wpf/pull/5610) changed how that list is kept, and [dotnet/wpf#9501](https://github.com/dotnet/wpf/pull/9501) added a switch to turn the change off again. One round of that repro, measured on two runtimes:

| runtime | one round took | deferred references afterwards |
| --- | --- | --- |
| .NET 8 | 11.14 s | 80000 |
| .NET 10 | 3.20 s | 0 |

So on .NET 10 there is nothing to do. The list does not fill any more.

## Before .NET 10, and on .NET Framework

On .NET Framework 4.8 and the older .NET versions this does not go away, and two things help.

### Walk the dictionaries once at startup

Reading every entry of every merged dictionary once makes the resources real instead of deferred, and the list stays short. The reporter measured about a second for a large set of dictionaries, paid once while the application starts:

```csharp
using System.Collections;
using System.Windows;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        WalkDictionary(this.Resources);
    }

    private static void WalkDictionary(ResourceDictionary resources)
    {
        foreach (DictionaryEntry entry in resources)
        {
        }

        foreach (var merged in resources.MergedDictionaries)
        {
            WalkDictionary(merged);
        }
    }
}
```

The empty loop body is the whole point: asking for the entries is what resolves them.

### Keep the tabs you switch between

Where views are tabs, the cheapest answer is not to rebuild them at all. [MetroTabControl](../controls/MetroTabControl) can keep each tab's content alive:

```xml
<mah:MetroTabControl KeepVisualTreeInMemoryWhenChangingTabs="True" />
```

That trades memory for time, and it only helps for switching between tabs that already exist. Views that genuinely come and go still add their references.

## Should you avoid the helper properties?

No. They are what lets you restyle a control from the outside without replacing its template, and on .NET 10 they no longer cost anything unusual. If you are on an older runtime and one particular view is the problem, replacing a busy helper property with a `DynamicResource` inside your own copy of the template will save you those references, at the price of a template to maintain.
