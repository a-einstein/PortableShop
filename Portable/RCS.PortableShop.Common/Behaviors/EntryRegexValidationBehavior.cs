using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace RCS.PortableShop.Common.Behaviors
{
    // This might get replaced by Xamarin.CommunityToolkit.TextValidationBehavior, but it is interesting to keep as an example anyway.
    public class EntryRegexValidationBehavior : Behavior<Entry>, INotifyPropertyChanged
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
            IsValid =   string.IsNullOrEmpty(e.NewTextValue) || 
                        Regex.IsMatch(e.NewTextValue, Expression, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));

            // Use BackgroundColor as it stands out more.
            // This could be replaced by an entire style.
            // Do not mark an empty string.
            (sender as Entry).BackgroundColor = string.IsNullOrEmpty(e.NewTextValue) || IsValid
                ? Color.Default
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
            private set
            {
                SetValue(IsValidPropertyKey, value);
                RaisePropertyChanged(nameof(IsValid));
            }
        }
        #endregion

        #region Parameters

        // TODO Passing parameters into this Behavior currently does not work and may be buggy in Xamarin.
        // Values are hard coded in XAML now.
        // The setup here may be simplified in the future.

        //public string Expression { get; set; }

        // Initial value always true.
        public static readonly BindableProperty ExpressionProperty =
            BindableProperty.Create(nameof(Expression), typeof(string), typeof(EntryRegexValidationBehavior), ".*");

        public string Expression
        {
            get => (string)GetValue(ExpressionProperty);
            set
            {
                SetValue(ExpressionProperty, value);
                RaisePropertyChanged(nameof(Expression));
            }
        }

        public static readonly BindableProperty InvalidBackgroundColourProperty =
            BindableProperty.Create(nameof(InvalidBackgroundColour), typeof(Color), typeof(EntryRegexValidationBehavior), Color.Default);

        public Color InvalidBackgroundColour
        {
            get => (Color)GetValue(InvalidBackgroundColourProperty);
            set
            {
                SetValue(InvalidBackgroundColourProperty, value);
                RaisePropertyChanged(nameof(InvalidBackgroundColour));
            }
        }
        #endregion

        #region INotifyPropertyChanged
        // TODO This may be moved to a future base class.

        public new event PropertyChangedEventHandler PropertyChanged;

        // This is needed  for intermediate value changes.
        // An initial binding usually works without, even without being a BindableProperty.
        // TODO This seems superfluous for a BindableProperty.
        // This signal can be particularly useful if a collection is entirely replaced, as the formerly bound collection no longer can.
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
