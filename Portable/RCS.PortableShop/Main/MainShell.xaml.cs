using RCS.PortableShop.Common.ViewModels;
using RCS.PortableShop.Resources;
using RCS.PortableShop.ViewModels;
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

            // Note OnAppearing is not reached, OnNavigated already from InitializeComponent. Therefor this shortcut.
            BindingContext = new MainShellViewModel();
            (BindingContext as ViewModel)?.Refresh();

            // TODO This resulted is a corrupt Navigation.NavigationStack.
            //CurrentItem = new MainPage();
        }
    }
}