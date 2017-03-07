using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace RCS.PortableShop.Common.Behaviors
{
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

        void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            IsValid = (Regex.IsMatch(e.NewTextValue, Expression, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));

            // Use BackgroundColor as it stands out more.
            // This could be replaced by an entire style.
            (sender as Entry).BackgroundColor = IsValid
                ? Color.Default
                : InvalidBackgroundColour;
        }
        #endregion

        #region IsValid
        static readonly BindablePropertyKey IsValidPropertyKey =
            BindableProperty.CreateReadOnly(nameof(IsValid), typeof(bool), typeof(EntryRegexValidationBehavior), false);

        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

        public bool IsValid
        {
            get { return (bool)base.GetValue(IsValidProperty); }
            private set { base.SetValue(IsValidPropertyKey, value); }
        }
        #endregion

        #region Parameters
        public string Expression { get; set; }

        // This needs to be a BindableProperty to enable assignment from resources. https://bugzilla.xamarin.com/show_bug.cgi?id=31547
        public static BindableProperty InvalidBackgroundColourProperty =
            BindableProperty.Create(nameof(InvalidBackgroundColour), typeof(Color), typeof(EntryRegexValidationBehavior), Color.Default);

        public Color InvalidBackgroundColour
        {
            get { return (Color)GetValue(InvalidBackgroundColourProperty); }
            set { SetValue(InvalidBackgroundColourProperty, value); }
        }
        #endregion
    }
}
