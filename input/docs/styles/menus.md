Title: Menus
Description: The Menu styles
---

You can use the ContextMenu as you are familiar with it:

```xml
<ContextMenu>
  <MenuItem Command="ApplicationCommands.New" />
  <MenuItem Command="ApplicationCommands.Delete" />
  <MenuItem Command="ApplicationCommands.Print" />
</ContextMenu>
```

## ContextMenu with CompositeCollection

If you want to use a part of the ContextMenu which is declared as resource and is shared you have to use a `CompositeCollection` as follows:

```xml
<CompositeCollection x:Key="ContextMenuBase"
                     x:Shared="False">
  <MenuItem Command="ApplicationCommands.New" />
  <MenuItem Command="ApplicationCommands.Delete" />
  <Separator />
  <StaticResource ResourceKey="ContextMenuItemRefresh" />
  <Separator />
</CompositeCollection>


<ContextMenu>
  <ContextMenu.ItemsSource>
    <CompositeCollection>
      <CollectionContainer Collection="{StaticResource ContextMenuBase}"></CollectionContainer>
      <MenuItem Command="ApplicationCommands.Print" />
    </CompositeCollection>
  </ContextMenu.ItemsSource>
</ContextMenu>
```

When you run the application it could be that you will get a lot of binding errors like this:

:::{.alert .alert-danger}
System.Windows.Data Error: 4 : Cannot find source for binding with reference 'RelativeSource FindAncestor, AncestorType='System.Windows.Controls.ItemsControl', AncestorLevel='1''. BindingExpression:Path=VerticalContentAlignment; DataItem=null; target element is 'MenuItem' (Name=''); target property is 'VerticalContentAlignment' (type 'VerticalAlignment')
:::

This is a known issue in WPF and can be fixed with this workaround:

```xml
<Style TargetType="{x:Type MenuItem}"
       BasedOn="{StaticResource {x:Type MenuItem}}">
  <Setter Property="HorizontalContentAlignment" Value="Left" />
  <Setter Property="VerticalContentAlignment" Value="Center" />
</Style>
```

The drawback is that you loose the ability to contol the content-alignment of the MenuItem by setting the content-alignment properties on a `ContextMenu`.
