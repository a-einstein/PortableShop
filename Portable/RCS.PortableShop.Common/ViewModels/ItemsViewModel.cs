using System.Collections.ObjectModel;
using System.Windows;

namespace RCS.WpfShop.Common.ViewModels
{
    public abstract class ItemsViewModel<T> : ViewModel
    {
        public ItemsViewModel()
        {
            Items = new ObservableCollection<T>();
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register(nameof(Items), typeof(ObservableCollection<T>), typeof(ItemsViewModel<T>));

        // TODO Some sort of view would be more convenient to enable sorting in situ (filtering is no longer done so). But remember: that no longer applies when paging.
        public ObservableCollection<T> Items
        {
            get { return (ObservableCollection<T>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // Convenience property to signal changes.
        // Note that just binding on Items.Count does not work.
        public int ItemsCount { get { return Items != null ? Items.Count : 0; } }
    }
}
