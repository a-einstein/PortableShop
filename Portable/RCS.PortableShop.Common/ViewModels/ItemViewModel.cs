using RCS.AdventureWorks.Common.DomainClasses;
using Xamarin.Forms;

namespace RCS.PortableShop.Common.ViewModels
{
    public abstract class ItemViewModel<I> : ViewModel where I : DomainClass
    {
        private int? itemId;

        // Note that a generic property is impossible, so the DomainClass is needed, determine the property's type.
        public int? ItemId
        {
            get { return itemId; }
            set
            {
                // Store the ID separately to enable a retry on an interrupted Refresh.
                itemId = value;

                Refresh();
            }
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
    }
}