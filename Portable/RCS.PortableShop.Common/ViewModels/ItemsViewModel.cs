using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace RCS.PortableShop.Common.ViewModels
{
    public abstract class ItemsViewModel<I> : ViewModel
    {
        public ItemsViewModel()
        {
            // This is still needed after initialization, it cannot be set on the ItemsProperty.
            SetItemsCollectionChanged();
        }

        public static readonly BindableProperty ItemsProperty =
            BindableProperty.Create(nameof(Items), typeof(ObservableCollection<I>), typeof(ItemsViewModel<I>), defaultValue: new ObservableCollection<I>());

        // TODO Some sort of view would be more convenient to enable sorting in situ (filtering is no longer done so). But remember: that no longer applies when paging.
        public ObservableCollection<I> Items
        {
            get { return (ObservableCollection<I>)GetValue(ItemsProperty); }
            set
            {
                SetValue(ItemsProperty, value);
                SetItemsCollectionChanged();
            }
        }

        private void SetItemsCollectionChanged()
        {
            Items.CollectionChanged += Items_CollectionChanged;
        }

        private void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(ItemsCount));
        }

        // Convenience property to signal changes.
        // Note that just binding on Items.Count does not work.
        public int ItemsCount { get { return Items?.Count ?? 0; } }
    }
}
