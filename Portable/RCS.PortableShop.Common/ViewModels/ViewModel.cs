using RCS.PortableShop.Resources;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Page = RCS.PortableShop.Common.Pages.Page;
using View = RCS.PortableShop.Common.Views.View;

namespace RCS.PortableShop.Common.ViewModels
{
    public abstract class ViewModel : BindableObject, INotifyPropertyChanged
    {
        #region Construction
        protected virtual void SetCommands() { }
        #endregion

        #region Refresh
        public static readonly BindableProperty AwaitingProperty =
            BindableProperty.Create(nameof(Awaiting), typeof(bool), typeof(ViewModel), false);

        public virtual bool Awaiting
        {
            get => (bool)GetValue(AwaitingProperty);
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

            await Clear().ConfigureAwait(true);
            UpdateTitle();

            if (await Initialize().ConfigureAwait(true))
            {
                await Read().ConfigureAwait(true);
                UpdateTitle();
            }

            Awaiting = false;
        }

        protected virtual async Task Clear() { }

        private bool initialized;

        protected virtual async Task<bool> Initialize()
        {
            if (!initialized)
                await Task.Run(() =>
                {
                    SetCommands();
                    initialized = true;
                }
                ).ConfigureAwait(true);

            return initialized;
        }

        // Keep virtual as not all derivatives can have a sensible action.
        protected virtual async Task Read()
        {
            // Non action.
            await Task.Delay(0).ConfigureAwait(true);
        }

        protected void UpdateTitle()
        {
            Title = MakeTitle();
        }

        // TODO Apparently the explicit translation is superfluous. Check this for xaml and possibly cleanup.
        // TranslateExtension.ProvideValue(Labels.Shop) as string;
        protected static readonly string TitleDefault = Labels.Shop;

        public virtual string MakeTitle() { return TitleDefault; }

        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create(nameof(Title), typeof(string), typeof(ViewModel), propertyChanged: TitleChanged, defaultValue: TitleDefault);

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        // Note this is particularly needed for the chaining within the ShoppingWrapperViewModel,
        // as the Binding does not use the Title property, but the SetValue method directly.
        // So calling RaisePropertyChanged within the Title property does not work.
        private static void TitleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as ViewModel)?.RaisePropertyChanged(nameof(Title));
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

        protected static Shell Shell => Application.Current.MainPage as Shell;
        private static INavigation Navigation => Application.Current.MainPage.Navigation;

        protected static async Task PopToRoot()
        {
            await Navigation.PopToRootAsync().ConfigureAwait(true);
        }

        // Note that a potential Color parameter cannot have a default value.
        protected static async Task PushPage(Xamarin.Forms.View view, string title = null)
        {
            var page = new ContentPage() { Content = view, Title = title };

            await Navigation.PushAsync(page).ConfigureAwait(true);
        }

        protected static async Task PushPage(View view)
        {
            var page = new Page();
            page.SetBinding(Xamarin.Forms.Page.TitleProperty, new Binding() { Path = nameof(Title), Source = view.ViewModel });
            page.Content = view;

            // Note this works for UWP only since Xamarin.Forms 4.8.
            // https://github.com/xamarin/Xamarin.Forms/issues/8498
            await Navigation.PushAsync(page).ConfigureAwait(true);

            await view.Refresh().ConfigureAwait(true);
        }
        #endregion
    }
}