using Xamarin.Forms;

namespace RCS.PortableShop.Common.Styles
{
    public partial class Stylesheet : ResourceDictionary
    {
        public Stylesheet()
        {
            // HACK Needed this file and constructor to enable merging.
            // https://forums.xamarin.com/discussion/84382/how-to-get-resourcedictionary-mergedwith-working
            InitializeComponent();
        }
    }
}
