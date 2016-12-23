using RCS.AdventureWorks.Common.DomainClasses;
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
            BindableProperty.Create(nameof(Item), typeof(T), typeof(ItemViewModel<T>));

        public T Item
        {
            get { return (T)GetValue(ItemProperty); }
            set
            {
                SetValue(ItemProperty, value);

                // TODO This should not be necessary.
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