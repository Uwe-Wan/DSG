using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace DSG.WinUI.Behavior
{
    public class DropDownButtonContextMenuBehavior : Behavior<Button>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.AddHandler(Button.ClickEvent, new RoutedEventHandler(AssociatedObject_Click), true);
        }

        public void AssociatedObject_Click(object sender, RoutedEventArgs e)
        {
            Button source = sender as Button;
            ContextMenu parent = source.Parent as ContextMenu;
            if (parent != null)
            {
                parent.IsOpen = false;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.RemoveHandler(Button.ClickEvent, new RoutedEventHandler(AssociatedObject_Click));
        }
    }
}
