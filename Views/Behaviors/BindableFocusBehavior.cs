using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Views.Behaviors
{
    public class BindableFocusBehavior : Behavior<Control>
    {
        public static readonly DependencyProperty HasFocusProperty =
            DependencyProperty.Register("HasFocus", typeof(bool), typeof(BindableFocusBehavior),
                new PropertyMetadata(default(bool), HasFocusUpdated));

        protected override void OnAttached()
        {
            SetFocus();
        }

        private static void HasFocusUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BindableFocusBehavior) d).SetFocus();
        }

        public bool HasFocus
        {
            get { return (bool) GetValue(HasFocusProperty); }
            set { SetValue(HasFocusProperty, value); }
        }

        private void SetFocus()
        {
            if (AssociatedObject == null)
            {
                return;
            }
            if (HasFocus)
            {
                AssociatedObject.Focus();
            }
        }
    }
}
