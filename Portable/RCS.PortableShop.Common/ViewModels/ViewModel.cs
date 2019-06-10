using RCS.PortableShop.Resources;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Page = RCS.PortableShop.Common.Pages.Page;
using View = RCS.PortableShop.Common.Views.View;

// Arbitrarily put here for the assembly.
// Also check comments on XamlCompilation elsewhere.
[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace RCS.PortableShop.Common.ViewModels
{
    public abstract class ViewModel : BindableObject, INotifyPropertyChanged
    {
        #region Construction
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

        protected virtual void Clear() { }

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

        protected virtual async Task<bool> Read() { return true; }

        protected void UpdateTitle()
        {
            Title = MakeTitle();
        }

        // TODO Apparently the explicit translation is superfluous. Check this for xaml and possibly cleanup.
        // TranslateExtension.ProvideValue(Labels.Shop) as string;
        protected static string TitleDefault = Labels.Shop;

        public virtual string MakeTitle() { return TitleDefault; }

        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create(nameof(Title), typeof(string), typeof(ViewModel), propertyChanged: TitleChanged, defaultValue: TitleDefault);

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Note this is particularly needed for the chaining within the ShoppingWrapperViewModel,
        // as the Binding does not use the Title property, but the SetValue method directly.
        // So calling RaisePropertyChanged within the Title property does not work.
        private static void TitleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as ViewModel).RaisePropertyChanged(nameof(Title));
        }
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
        // TODO The use of classes from Xamarin.Forms here is a bit of a hack. Better keep this independent.

        public Shell Shell { get { return Application.Current.MainPage as Shell; } }
        public INavigation Navigation { get { return Application.Current.MainPage.Navigation; } }

        protected async Task PopToRoot()
        {
            await Navigation.PopToRootAsync();
        }

        // Note that a potential Color parameter cannot have a default value.
        protected async Task PushPage(Xamarin.Forms.View view, string title = null)
        {
            var page = new ContentPage() { Content = view, Title = title };

            await Navigation.PushAsync(page);
        }

        protected async Task PushPage(View view)
        {
            var page = new Page();
            page.SetBinding(Page.TitleProperty, new Binding() { Path = nameof(Title), Source = view.ViewModel });
            page.Content = view;

            await Navigation.PushAsync(page);
            await view.Refresh();
        }
        #endregion
    }
}