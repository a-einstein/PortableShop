using RCS.PortableShop.Resources;
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

            await Initialize();
        }

        private bool initialized;

        protected virtual async Task Initialize()
        {
            if (!initialized)
            {
                Adorn();

                initialized = true;
            }
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
            // TODO Add application icon here for better layout?
            // TODO As of using Shell the icons are not displayed, though the commands work.
            ToolbarItems.Add(new ToolbarItem("R", "Refresh.png", async () => await Content.ViewModel.Refresh(), priority: 10));
        }

        public async Task Refresh()
        {
            await Initialize();
            await Content?.Refresh();
        }
    }
}
