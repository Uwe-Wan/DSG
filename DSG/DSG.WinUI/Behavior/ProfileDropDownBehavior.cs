using DSG.BusinessEntities.GenerationProfiles;
using DSG.Common;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace DSG.WinUI.Behavior
{
    public class ProfileDropDownBehavior : Behavior<Button>
    {
        public string SelectedProfileName
        {
            get { return (string)GetValue(ProfileProperty); }
            set { SetValue(ProfileProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Words.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProfileProperty =
            DependencyProperty.Register(nameof(SelectedProfileName), typeof(string), typeof(ProfileDropDownBehavior), new PropertyMetadata(default(string)));

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.AddHandler(Button.ClickEvent, new RoutedEventHandler(AssociatedObject_Click), true);
        }

        public void AssociatedObject_Click(object sender, RoutedEventArgs e)
        {
            SelectedProfileName = Text.InsertName;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.RemoveHandler(Button.ClickEvent, new RoutedEventHandler(AssociatedObject_Click));
        }
    }
}
