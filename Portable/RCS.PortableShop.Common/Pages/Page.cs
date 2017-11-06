﻿using RCS.PortableShop.Resources;
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
            ToolbarItems.Add(new ToolbarItem("R", "Refresh.png", async () => await Content.ViewModel.Refresh(), priority: 10));
            ToolbarItems.Add(new ToolbarItem("I", "About.png", About, priority: 90));
        }

        public async void About()
        {
            // TODO The version has to get shared with the Android manifest (to start with).
            await DisplayAlert(Labels.AboutLabel, string.Format(Labels.AboutText, Labels.Shop, Labels.Developer, "0.9.0"), Labels.Close);
        }

        public async Task Refresh()
        {
            await Initialize();
            await Content?.Refresh();
        }
    }
}