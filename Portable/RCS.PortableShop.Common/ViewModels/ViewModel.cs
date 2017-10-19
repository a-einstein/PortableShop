using RCS.PortableShop.Resources;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Page = RCS.PortableShop.Common.Pages.Page;

namespace RCS.PortableShop.Common.ViewModels
{
    public abstract class ViewModel : BindableObject, INotifyPropertyChanged
    {
        #region Construct
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

        // Note that actions are deliberately put here instead of in constructor to avoid problems.
        public virtual async Task Refresh()
        {
            Awaiting = true;

            Clear();

            UpdateTitle();

            if (await Initialize())
            {
                await Read();

                UpdateTitle();
            }

            Awaiting = false;
        }

        private bool initialized;

        protected virtual async Task<bool> Initialize()
        {
            if (!initialized)
            {
                SetCommands();

                initialized = true;
            }

            return initialized;
        }

        // TODO >> Force Page in constructor parameter? Consider ImplicitModelView.
        public void Adorn()
        {
            Page?.ToolbarItems.Add(new ToolbarItem("R", "Refresh.png", async () => await Refresh(), priority: 10));
        }

        protected virtual void Clear() { }

        protected virtual async Task<bool> Read() { return true; }

        private void UpdateTitle()
        {
            Page.Title = Title;
        }

        // TODO Apparently the explicit translation is superfluous. Check this for xaml and possibly cleanup.
        // TranslateExtension.ProvideValue(Labels.Shop) as string;
        protected string TitleDefault = Labels.Shop;

        public virtual string Title { get { return TitleDefault; } }
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
        public virtual Page Page { get; set; }
 
        // Note that a potential Color parameter cannot have a default value.
        protected async Task PushPage(View view, string title = null)
        {
            var page = new ContentPage() { Content = view, Title = title };

            await Page.Navigation.PushAsync(page);
        }

        protected async Task PushPage(Views.View view)
        {
            var page = new Page();

            view.ViewModel.Page = page;

            page.Content = view;

            await Page.Navigation.PushAsync(page);

            await view.Refresh();
        }
        #endregion
    }
}
