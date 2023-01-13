using System.Windows.Input;

namespace RCS.PortableShop.Common.Controls
{
    // Thanks for the base idea to http://stackoverflow.com/questions/29693721/xamarin-forms-content-of-a-button
    // Note TapGestureRecognizer of ContentButton does not work if BackgroundColor is set in Content! https://bugzilla.xamarin.com/show_bug.cgi?id=25943
    public partial class ContentButton : ContentView
    {
        public ContentButton()
        {
            InitializeComponent();
        }

        public event EventHandler TappedHandler;

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ContentButton));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ContentButton));

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            TappedHandler?.Invoke(this, e);
        }
    }
}
