using RCS.PortableShop.Resources;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RCS.PortableShop.Common.Pages
{
    // TODO Maybe change this name as it is already used by Xamarin.
    public class Page : ContentPage
    {
        protected override void OnAppearing()
        {
            base.OnAppearing();

            Initialize();
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

        private void Adorn()
        {
            // TODO Currently timing may be unfortunate, overwriting externally set value.
            Title = Labels.Shop;

            // TODO Add application icon here for better layout?
            ToolbarItems.Add(new ToolbarItem("I", "About.png", About, priority: 90));
        }

        public async void About()
        {
            // TODO The version has to get shared with the Android manifest (to start with).
            await DisplayAlert(Labels.AboutLabel, string.Format(Labels.AboutText, Labels.Shop, Labels.Developer, "0.7.0"), Labels.Close);
        }
    }
}
