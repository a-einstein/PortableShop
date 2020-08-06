using RCS.PortableShop.Resources;
using RCS.PortableShop.ViewModels;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RCS.PortableShop.Main
{
    public partial class MainShell : Shell
    {
        public MainShell()
        {
            InitializeComponent();

            // TODO This could move to MainShellViewModel.
            // Note the 'Version name' in the AndroidManifest should be increased with each merge as a policy. 
            aboutLabel.Text = string.Format(Labels.AboutText, Labels.Shop, Labels.Developer, AppInfo.Version);
        }

        // After InitializeComponent
        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);

            // TODO This whole lot appears to be no loger needed. How? What is better?

            var viewModel = new MainShellViewModel();

            Task.Run(async () =>
            {
                await (viewModel?.Refresh()).ConfigureAwait(true);
            }).ConfigureAwait(true);

            // TODO There intermittently is a NullReferenceException here.
            BindingContext = viewModel;

            // TODO This resulted is a corrupt Navigation.NavigationStack.
            //CurrentItem = new MainPage();
        }

        // After OnNavigating
        protected override void OnNavigated(ShellNavigatedEventArgs args)
        {
            base.OnNavigated(args);
        }

        // Not yet reached
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}