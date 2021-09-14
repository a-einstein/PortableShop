using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RCS.PortableShop.Common.Controls
{
    // HACK Prevents compilation error for unknown reason.
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class ClearableEntry : CustomContentView
    {
        #region Construction
        public ClearableEntry()
        {
            InitializeComponent();

            ClearCommand = new Command(Clear);
        }
        #endregion

        #region Text
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(ClearableEntry), defaultBindingMode: BindingMode.TwoWay);

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        #endregion

        #region Placeholder
        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(ClearableEntry));

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }
        #endregion

        #region Command
        private static readonly BindableProperty ClearCommandProperty =
            BindableProperty.Create(nameof(ClearCommand), typeof(ICommand), typeof(ClearableEntry));

        public ICommand ClearCommand
        {
            get => (ICommand)GetValue(ClearCommandProperty);
            set => SetValue(ClearCommandProperty, value);
        }

        private void Clear()
        {
            Text = default;
        }
        #endregion

        #region Behaviors
        // TODO Strangely, in Debug this does not seem to get set, though the behaviour may actually work.
        public IList<Behavior> EntryBehaviors
        {
            get => entry.Behaviors;
            set
            {
                entry.Behaviors.Clear();

                foreach (var behavior in value)
                {
                    entry.Behaviors.Add(behavior);
                }
            }
        }
        #endregion
    }
}
