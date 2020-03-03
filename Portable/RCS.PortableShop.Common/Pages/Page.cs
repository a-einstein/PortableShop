using RCS.PortableShop.Common.Extensions;
using System.Threading.Tasks;
using Xamarin.Forms;
using View = RCS.PortableShop.Common.Views.View;

namespace RCS.PortableShop.Common.Pages
{
    // TODO Maybe change this name as it is already used by Xamarin.
    public class Page : ContentPage
    {
        // Put calls to virtual methods here to avoid them during construction.
        // Note this event includes reappearing during Navigation or Resume.
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await Initialize().ConfigureAwait(true);
        }

        private bool initialized;

        protected virtual async Task Initialize()
        {

            if (!initialized)
                await Task.Run(() =>
                {
                    Adorn();
                    initialized = true;
                }
                ).ConfigureAwait(true);
        }

        // Force new type here.
        public new View Content
        {
            get { return base.Content as View; }
            set
            {
                base.Content = value;

                // Directly bind the Title. 
                SetBinding(TitleProperty, new Binding() { Path = "Title", Source = Content.ViewModel });
            }
        }

        private void Adorn()
        {
            // TODO Since applying Shell, icons are not displayed, though the commands work.
            // https://github.com/xamarin/Xamarin.Forms/issues/7351
            ToolbarItems.Add(new ToolbarItem("R", "Refresh.png", async () => await Content.ViewModel.Refresh().ConfigureAwait(true), priority: 10));
        }

        protected async Task Refresh()
        {
            await Initialize().ConfigureAwait(true);

            // Use IfNotNull because of ConfigureAwait.
            Content.IfNotNull(async content => await content.Refresh().ConfigureAwait(true));
        }
    }
}
