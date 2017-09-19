using RCS.PortableShop.Resources;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RCS.PortableShop.Common.ViewModels
{
    public abstract class ViewModel : BindableObject, INotifyPropertyChanged
    {
        #region Construct
        public ViewModel()
        {
            // TODO Still need to address reported code analysis issue here. So far, alternatives caused real problems.
            SetCommands();
        }

        protected virtual void SetCommands() { }
        #endregion

        #region Refresh
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

        public virtual async void Refresh()
        {
            Awaiting = true;

            Clear();

            if (await Initialize())
                await Read();

            Awaiting = false;

            return;
        }

        protected virtual async Task<bool> Initialize() { return true; }

        protected virtual void Clear() { }

        protected virtual async Task<bool> Read() { return true; }
        #endregion

        #region Events
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
        #endregion

        #region Navigation
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
            // TODO Add About here too. Combine with MainPage code.
 
            Navigation.PushAsync(page);
        }
        #endregion
    }
}
