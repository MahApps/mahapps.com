Title: MetroProgressBar
---

The `MetroProgressBar` is an alternative progress bar with a different indeterminate style. Instead of creating a normal `ProgressBar` use:

```xml
<mah:MetroProgressBar Width="250"
                      Margin="4"
                      Maximum="100"
                      Minimum="0"
                      Value="42" />
```

or

```xml
<mah:MetroProgressBar Width="250"
                      Margin="4"
                      IsIndeterminate="True" />
```
Its indeterminate state looks like a `ProgressRing` but not circular.

![](images/metroprogressbar.png)
