using Xamarin.Forms;

namespace RCS.PortableShop.Resources
{
    public class Wrapper : BindableObject
    {
        private static Labels labels = new Labels();
        public Labels Labels { get { return labels; } }

        public static readonly BindableProperty BindableLabelsProperty = BindableProperty.Create("BindableLabels", typeof(Labels), typeof(Wrapper), new Labels());

        public Labels BindableLabels
        {
            get { return (Labels)GetValue(BindableLabelsProperty); }
        }

        public string PublicLabel1
        {
            get { return "PublicLabel!"; }
        }

        public string PublicLabel2
        {
            get { return "PublicLabel2!"; }
        }

        public static string StaticLabel1
        {
            get { return "StaticLabel1!"; }
        }
     }
}
