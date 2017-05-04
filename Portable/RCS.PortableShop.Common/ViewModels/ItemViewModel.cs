using RCS.AdventureWorks.Common.DomainClasses;
using Xamarin.Forms;

namespace RCS.PortableShop.Common.ViewModels
{
    public abstract class ItemViewModel<I> : ViewModel where I : DomainClass
    {
        public int? ItemId
        {
            get { return Item?.Id; }
            set { Refresh(value); }
        }

        public static readonly BindableProperty ItemProperty =
            BindableProperty.Create(nameof(Item), typeof(I), typeof(ItemViewModel<I>));

        public I Item
        {
            get { return (I)GetValue(ItemProperty); }
            set
            {
                SetValue(ItemProperty, value);
                RaisePropertyChanged(nameof(Item));
            }
        }

        public override void Refresh()
        {
            Refresh(ItemId);
        }

        public abstract void Refresh(object Id);
    }
}