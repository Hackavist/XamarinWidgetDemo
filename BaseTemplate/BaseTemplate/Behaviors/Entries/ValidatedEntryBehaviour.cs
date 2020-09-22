using Xamarin.Forms;

namespace WidgetDemo.Behaviors.Entries
{
    public class ValidatedEntryBehaviour : BehaviorBase<Entry>
    {
        #region StaticFields

        public static readonly BindableProperty IsValidProperty = BindableProperty.Create(nameof(IsValid), typeof(bool),
            typeof(ValidatedEntryBehaviour), true, BindingMode.Default, null,
            (bindable, oldValue, newValue) => IsValidChanged(bindable, newValue));

        #endregion

        #region Properties

        public bool IsValid
        {
            get => (bool)GetValue(IsValidProperty);
            set => SetValue(IsValidProperty, value);
        }

        #endregion

        #region StaticMethods

        private static void IsValidChanged(BindableObject bindable, object newValue)
        {
            if (bindable is ValidatedEntryBehaviour isValidBehavior && newValue is bool IsValid)
                isValidBehavior.AssociatedObject.TextColor = IsValid ? Color.Default : Color.Red;
        }

        #endregion
    }
}