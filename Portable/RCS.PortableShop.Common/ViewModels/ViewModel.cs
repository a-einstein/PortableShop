using System.ComponentModel;
using Xamarin.Forms;

namespace RCS.PortableShop.Common.ViewModels
{
    public abstract class ViewModel : BindableObject, INotifyPropertyChanged
    {
        public ViewModel()
        {
            SetCommands();
        }

        protected abstract void SetCommands();

        protected const string databaseErrorMessage = "Error retrieving data from database.";

        public abstract void Refresh();

        public static readonly BindableProperty AwaitingProperty =
            BindableProperty.Create(nameof(Awaiting), typeof(bool), typeof(ViewModel), defaultValue: false);

        public bool Awaiting
        {
            get { return (bool)GetValue(AwaitingProperty); }
            set
            {
                SetValue(AwaitingProperty, value);

                // TODO Should this be needed?
                RaisePropertyChanged(nameof(Awaiting));
            }
        }

        protected static bool NullOrEmpty(string value)
        {
            return (value == null || value.Trim() == string.Empty);
        }

        public new event PropertyChangedEventHandler PropertyChanged;

        // This is needed  for intermediate value changes.
        // An initial binding usually works without, even without being a BindableProperty.
        // TODO This seems superfluous for a BindableProperty.
        // This signal can be particularly useful if a collection is entirely replaced, as the formerly bound collection no longer can.
        protected void RaisePropertyChanged(string propertyName)
        {
            // TODO This does not work for the inherited PropertyChanged.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public INavigation Navigation { get; set; }

        // Note that a potential Color parameter cannot have a default value.
        protected void PushPage(View view, string title = null)
        {
            var page = new ContentPage() { Content = view, Title = title };

            Navigation.PushAsync(page);
        }

        protected void PushPage(Views.View view, string title = null)
        {
            var page = new ContentPage() { Content = view, Title = title };
            
            page.ToolbarItems.Add(new ToolbarItem("R", "Refresh.png", view.ViewModel.Refresh));

            Navigation.PushAsync(page);
        }
    }
}
