using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RCS.PortableShop.Common.ViewModels
{
    public abstract class ItemsViewModel<TItem> : ViewModel
    {
        #region Construction

        protected ItemsViewModel()
        {
            // This is still needed after initialization, it cannot be set on the ItemsProperty.
            SetItemsCollectionChanged();
        }
        #endregion

        #region Items
        public static readonly BindableProperty ItemsProperty =
            BindableProperty.Create(nameof(Items), typeof(ObservableCollection<TItem>), typeof(ItemsViewModel<TItem>), new ObservableCollection<TItem>());

        // TODO Some sort of view would be more convenient to enable sorting in situ (filtering is no longer done so). But remember: that no longer applies when paging.
        public ObservableCollection<TItem> Items
        {
            get => (ObservableCollection<TItem>)GetValue(ItemsProperty);

            /*
            TODO Preferable get rid of this kind of setters (warning CA2227). 
            Thereby rationalizing the use of ObservableCollection versus PropertyChanged etcetera in general.
            See other comments.
            */
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

        protected virtual void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(ItemsCount));
        }

        // Convenience property to signal changes.
        // Note that just binding on Items.Count does not work.
        public int ItemsCount => Items?.Count ?? 0;

        #endregion

        #region Refresh
        protected override async Task Clear()
        {
            Items.Clear();
        }
        #endregion
    }
}