using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RCS.PortableShop.Common.ViewModels
{
    public abstract class ItemsViewModel<TItem> : ViewModel
    {
        #region Construction
        protected ItemsViewModel()
        {
            Items.CollectionChanged += Items_CollectionChanged;
        }
        #endregion

        #region Items
        public ObservableCollection<TItem> Items { get; } = new ObservableCollection<TItem>();

        protected virtual void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ItemsCount = Items?.Count ?? 0;
        }

        private static readonly BindableProperty ItemsCountProperty =
            BindableProperty.Create(nameof(ItemsCount), typeof(int), typeof(ItemsViewModel<TItem>));

        public int ItemsCount
        {
            get => (int)GetValue(ItemsCountProperty);
            set => SetValue(ItemsCountProperty, value);
        }
        #endregion

        #region Refresh
        protected override async Task Clear()
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                // Use ToArray to prevent iteration problems in the original list.
                foreach (var item in Items.ToArray())
                {
                    // Remove separately to enable Items_CollectionChanged.
                    Items.Remove(item);
                }
            }).ConfigureAwait(true);
        }
        #endregion
    }
}