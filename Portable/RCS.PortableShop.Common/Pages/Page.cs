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
            {
                initialized = true;

                await Task.Run(Adorn).ConfigureAwait(true);
            }
        }

        // Force new type here.
        public new View Content
        {
            get => base.Content as View;
            set
            {
                base.Content = value;

                // Directly bind the Title. 
                SetBinding(TitleProperty, new Binding() { Path = "Title", Source = Content.ViewModel });
            }
        }

        private void Adorn()
        {
            // This is particularly added to recover from a service problem.
            // Note chose white icon, because colours were not inverted on UWP, as on Android.
            // TODO Recovering does not succeed, check return statuses along the chain.
            ToolbarItems.Add(new ToolbarItem(
            null,
            "Refresh.png",
            async () => await Refresh())
            );
        }

        protected async Task Refresh()
        {
            await Initialize().ConfigureAwait(true);

            // Note Content.IfNotNull could be used here.
            await Content.Refresh().ConfigureAwait(true);
        }
    }
}
