using RCS.AdventureWorks.Common.DomainClasses;
using System.Windows;
using Windows.UI.Xaml;
using Xamarin.Forms;

namespace RCS.PortableShop.Common.ViewModels
{
    public abstract class ItemViewModel<T> : ViewModel where T : DomainClass
    {
        public int? ItemId
        {
            get { return Item?.Id; }
            set { Refresh(value); }
        }

        public static readonly BindableProperty ItemProperty =
            BindableProperty.Create(nameof(Item), typeof(T), typeof(ItemViewModel<T>), new PropertyMetadata(null));

        public T Item { get; set; }

        public override void Refresh()
        {
            Refresh(ItemId);
        }

        public abstract void Refresh(object Id);
    }
}