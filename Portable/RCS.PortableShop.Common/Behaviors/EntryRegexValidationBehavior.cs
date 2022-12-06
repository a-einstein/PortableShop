using System.Text.RegularExpressions;

namespace RCS.PortableShop.Common.Behaviors
{
    // This might get replaced by Xamarin.CommunityToolkit.TextValidationBehavior, but it is interesting to keep as an example anyway.
    // Note this is a BindableObject.
    public class EntryRegexValidationBehavior : Behavior<Entry>
    {
        #region Behavior
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += HandleTextChanged;
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= HandleTextChanged;
        }

        private void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            // Except testing the empty string.
            // This could also be put in the Expression.
            IsValid = string.IsNullOrEmpty(e.NewTextValue) ||
                      Regex.IsMatch(e.NewTextValue, Expression, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));

            // Use BackgroundColor as it stands out more.
            // This could be replaced by an entire style.
            // Do not mark an empty string.
            (sender as Entry).BackgroundColor = string.IsNullOrEmpty(e.NewTextValue) || IsValid
                ?/* Color.Default*/ Colors.White // HACK.
                : InvalidBackgroundColour;
        }
        #endregion

        #region IsValid
        private static readonly BindablePropertyKey IsValidPropertyKey =
            BindableProperty.CreateReadOnly(nameof(IsValid), typeof(bool), typeof(EntryRegexValidationBehavior), false);

        private static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

        public bool IsValid
        {
            get => (bool)GetValue(IsValidProperty);
            private set => SetValue(IsValidPropertyKey, value);
        }
        #endregion

        #region Parameters
        // TODO Passing parameters into this Behavior currently does not work and may be buggy in Xamarin.
        // Values are hard coded in XAML now.
        // The setup here may be simplified in the future.

        public string Expression { get; set; }

        public Color InvalidBackgroundColour { get; set; }
        #endregion
    }
}
