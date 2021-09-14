using RCS.PortableShop.Resources;
using System.Threading.Tasks;
using Xamarin.Forms;
using Page = RCS.PortableShop.Common.Pages.Page;
using View = RCS.PortableShop.Common.Views.View;

namespace RCS.PortableShop.Common.ViewModels
{
    // Note that BindableObject handles PropertyChanged, but is limited to Xamarin.
    // TODO Possibly move on to Microsoft.Toolkit.Mvvm.ComponentModel.ObservableObject.
    public abstract class ViewModel : BindableObject
    {
        #region Construction
        protected virtual void SetCommands() { }
        #endregion

        #region Refresh
        private static readonly BindableProperty AwaitingProperty =
            BindableProperty.Create(nameof(Awaiting), typeof(bool), typeof(ViewModel), false);

        public bool Awaiting
        {
            get => (bool)GetValue(AwaitingProperty);
            set => SetValue(AwaitingProperty, value);
        }

        // Note that actions are deliberately put here instead of in constructor to avoid problems.
        public virtual async Task Refresh()
        {
            Awaiting = true;

            await Clear().ConfigureAwait(true);
            UpdateTitle();

            await Initialize().ConfigureAwait(true);
            await Read().ConfigureAwait(true);
            UpdateTitle();

            Awaiting = false;
        }

        protected virtual async Task Clear()
        {
            await VoidTask();
        }

        private bool initialized;

        protected virtual async Task<bool> Initialize()
        {
            if (!initialized)
            {
                initialized = true;

                await Task.Run(SetCommands).ConfigureAwait(true);
            }

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

        protected static readonly string TitleDefault = Labels.Shop;

        public virtual string MakeTitle() { return TitleDefault; }

        protected static readonly BindableProperty TitleProperty =
            BindableProperty.Create(nameof(Title), typeof(string), typeof(ViewModel), defaultValue: TitleDefault);

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
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

        #region Utility
        private static Task VoidTask()
        {
            // HACK.
            return Task.Run(() => { });
        }
        #endregion
    }
}