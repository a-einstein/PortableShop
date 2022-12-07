namespace RCS.PortableShop.Common.Styles
{
    public partial class Stylesheet : ResourceDictionary
    {
        public Stylesheet()
        {
            // HACK Still needed this file and constructor to enable merging using ResourceDictionary.MergedDictionaries.
            InitializeComponent();
        }
    }
}
