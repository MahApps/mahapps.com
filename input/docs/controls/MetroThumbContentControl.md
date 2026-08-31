Title: MetroThumbContentControl
Description: A content control that reports mouse and touch drags
---

`MetroThumbContentControl` is a `Thumb` that can hold arbitrary content. WPF's own `Thumb` is a `Control` with a template, so wrapping content in it is awkward; this one derives from [ContentControlEx](contentcontrolex) instead and raises the same three drag events.

```xml
<mah:MetroThumbContentControl DragDelta="OnDragDelta">
    <Border Background="{DynamicResource MahApps.Brushes.Accent}" Padding="12">
        <TextBlock Text="Drag me" />
    </Border>
</mah:MetroThumbContentControl>
```

```csharp
private void OnDragDelta(object sender, DragDeltaEventArgs e)
{
    Canvas.SetLeft(this.card, Canvas.GetLeft(this.card) + e.HorizontalChange);
    Canvas.SetTop(this.card, Canvas.GetTop(this.card) + e.VerticalChange);
}
```

The control has **no appearance of its own**. Its style is

```xml
<Style x:Key="MahApps.Styles.MetroThumbContentControl"
       BasedOn="{StaticResource MahApps.Styles.ContentControlEx}"
       TargetType="{x:Type mah:MetroThumbContentControl}" />
```

— the [ContentControlEx](contentcontrolex) style with nothing added. What you see is entirely your content.

## You are probably already using it

This is not a control most applications place by hand. It is what makes two familiar things work:

- **[MetroWindow](metrowindow)'s title bar.** `PART_TitleBar` in the window template *is* a `MetroThumbContentControl`, and dragging it is what moves the window. The handler checks `IsWindowDraggable`, ignores movements of 2px or less, and refuses to drag a maximized window unless the pointer is actually within `TitleBarHeight`.
- **[Flyout](flyouts) headers**, which use the same `IMetroThumb` interface to let a flyout be dragged by its header.

Both go through `IMetroThumb`, a small interface carrying just the three drag events and `MouseDoubleClick`. `MetroThumb` — a plain `Thumb` implementing the same interface — is the other implementation.

## Events and state

| | |
| --- | --- |
| `DragStarted` | bubbling; raised on left mouse down after capture |
| `DragDelta` | bubbling; raised on each move that actually changes the screen position |
| `DragCompleted` | bubbling; raised on release |
| `IsDragging` | read-only dependency property |
| `CancelDragAction()` | aborts a drag in progress |

All three are **bubbling** routed events, unlike `Thumb`'s, so a parent can handle them without attaching to the thumb itself.

:::{.alert .alert-info}
`DragDelta` and `DragCompleted` do not measure the same thing.

`DragDelta` reports the change in the **thumb's own coordinates**, which is what makes the usual `+= e.HorizontalChange` idiom work when you move the element in response to the event.

`DragCompleted` reports the total change in **screen coordinates**, taken with `PointToScreen`, so it is the whole distance travelled regardless of how the element moved along the way.
:::

A drag is abandoned automatically if the control loses mouse capture — a class handler on `LostMouseCapture` calls `CancelDragAction()`, which releases capture, clears `IsDragging` and raises `DragCompleted` with `Canceled` set.

## Touch

`OnPreviewTouchDown` captures the touch device, `OnPreviewTouchUp` releases it, and `OnLostTouchCapture` takes it back if it is lost while still down. That last part is what keeps a drag alive when a finger slides outside the control — without it a touch drag stops the moment the contact leaves the bounds.

## Inherited behaviour

Because it derives from [ContentControlEx](contentcontrolex), `ControlsHelper.ContentCharacterCasing` and `ControlsHelper.RecognizesAccessKey` apply to its content, and a string child is upper- or lower-cased by the same converters.

`Focusable` is overridden to `False` in the static constructor, so the control never takes focus — appropriate for something whose whole job is to be dragged.

It also supplies a `MetroThumbContentControlAutomationPeer`.

## Related

[ContentControlEx](contentcontrolex) is the base and supplies the style. [MetroWindow](metrowindow) and [Flyout](flyouts) are where the library uses it.
