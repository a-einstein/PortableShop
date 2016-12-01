using RCS.AdventureWorks.Common.DomainClasses;
using System.Windows;

namespace RCS.WpfShop.Common.ViewModels
{
    public abstract class ItemViewModel<T> : ViewModel where T : DomainClass
    {
        public int? ItemId
        {
            get { return Item?.Id; }
            set { Refresh(value); }
        }

        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register(nameof(Item), typeof(T), typeof(ItemViewModel<T>), new PropertyMetadata(null));

        public T Item
        {
            get { return (T)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        public override void Refresh()
        {
            Refresh(ItemId);
        }

        public abstract void Refresh(object Id);
    }
}