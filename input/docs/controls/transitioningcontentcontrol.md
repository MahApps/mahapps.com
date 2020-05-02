Title: TransitioningContentControl
---

The `TransitioningContentControl` was taken from Silverlight (specifically [this](https://github.com/jenspettersson/WPF-Controls) port), it's great for switching content smoothly around.  
At it's core, `TransitioningContentControl` is a `ContentControl`, so only one child element can be displayed at a time. When you change the content, an animation is played switching the old content to the new one.

```xml
<mah:TransitioningContentControl x:Name="transitioningControl"
                                 Width="250"
                                 Height="250"
                                 Content="First"
                                 Transition="Left" />
```

Built in there are several transitions:  

```csharp
public enum TransitionType
{
  /// <summary>
  /// Use the VisualState DefaultTransition
  /// </summary>
  Default,

  /// <summary>
  /// Use the VisualState Normal
  /// </summary>
  Normal,

  /// <summary>
  /// Use the VisualState UpTransition
  /// </summary>
  Up,

  /// <summary>
  /// Use the VisualState DownTransition
  /// </summary>
  Down,

  /// <summary>
  /// Use the VisualState RightTransition
  /// </summary>
  Right,

  /// <summary>
  /// Use the VisualState RightReplaceTransition
  /// </summary>
  RightReplace,

  /// <summary>
  /// Use the VisualState LeftTransition
  /// </summary>
  Left,

  /// <summary>
  /// Use the VisualState LeftReplaceTransition
  /// </summary>
  LeftReplace,

  /// <summary>
  /// Use a custom VisualState, the name must be set using CustomVisualStatesName property
  /// </summary>
  Custom
}
```

## Custom Transition

It's possible to use a custom transition if the given ones are not enough.

```xml
<mah:TransitioningContentControl x:Name="customTransitioning"
                                 Width="250"
                                 Height="50"
                                 Content="First"
                                 CustomVisualStatesName="CustomTransition"
                                 Transition="Custom">
    <mah:TransitioningContentControl.CustomVisualStates>
        <VisualState x:Name="CustomTransition">
            <Storyboard>
                <DoubleAnimationUsingKeyFrames BeginTime="00:00:00"
                                               Storyboard.TargetName="CurrentContentPresentationSite"
                                               Storyboard.TargetProperty="(UIElement.Opacity)">
                    <SplineDoubleKeyFrame KeyTime="00:00:00" Value="0" />
                    <SplineDoubleKeyFrame KeyTime="00:00:00.5" Value="0" />
                    <EasingDoubleKeyFrame KeyTime="00:00:01" Value="1">
                        <EasingDoubleKeyFrame.EasingFunction>
                            <SineEase />
                        </EasingDoubleKeyFrame.EasingFunction>
                    </EasingDoubleKeyFrame>
                </DoubleAnimationUsingKeyFrames>
                <DoubleAnimationUsingKeyFrames BeginTime="00:00:00"
                                               Storyboard.TargetName="PreviousContentPresentationSite"
                                               Storyboard.TargetProperty="(UIElement.Opacity)">
                    <SplineDoubleKeyFrame KeyTime="00:00:00" Value="1" />
                    <SplineDoubleKeyFrame KeyTime="00:00:00.5" Value="0" />
                </DoubleAnimationUsingKeyFrames>
            </Storyboard>
        </VisualState>
    </mah:TransitioningContentControl.CustomVisualStates>
</mah:TransitioningContentControl>
```
