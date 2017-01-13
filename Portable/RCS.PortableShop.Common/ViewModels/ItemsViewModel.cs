using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace RCS.PortableShop.Common.ViewModels
{
    public abstract class ItemsViewModel<T> : ViewModel
    {
        public ItemsViewModel()
        {
            Items.CollectionChanged += Items_CollectionChanged;
        }

        public static readonly BindableProperty ItemsProperty =
            BindableProperty.Create(nameof(Items), typeof(ObservableCollection<T>), typeof(ItemsViewModel<T>), defaultValue: new ObservableCollection<T>());

        // TODO Some sort of view would be more convenient to enable sorting in situ (filtering is no longer done so). But remember: that no longer applies when paging.
        public ObservableCollection<T> Items 
        {
            get { return (ObservableCollection<T>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
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
