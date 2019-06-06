using RCS.PortableShop.Resources;
using Xamarin.Forms;

namespace RCS.PortableShop.Main
{
    public partial class MainShell : Shell
    {
        public MainShell()
        {
            InitializeComponent();

            // TODO The version has to get shared with the Android manifest (to start with).
            aboutLabel.Text = string.Format(Labels.AboutText, Labels.Shop, Labels.Developer, "0.10.0");
        }
    }
}