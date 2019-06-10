using RCS.PortableShop.Common.ViewModels;
using RCS.PortableShop.Resources;
using RCS.PortableShop.ViewModels;
using Xamarin.Forms;

namespace RCS.PortableShop.Main
{
    public partial class MainShell : Shell
    {
        public MainShell()
        {
            InitializeComponent();

            // TODO The version has to get shared with the Android manifest (to start with).
            // TODO This could move to MainShellViewModel too.
            aboutLabel.Text = string.Format(Labels.AboutText, Labels.Shop, Labels.Developer, "0.10.0");

            // Note OnAppearing is not reached, OnNavigated already from InitializeComponent. Therefor this shortcut.
            BindingContext = new MainShellViewModel();
            (BindingContext as ViewModel)?.Refresh();

            // TODO This results is a corrupt Navigation.NavigationStack.
            CurrentItem = new MainPage();
        }
    }
}