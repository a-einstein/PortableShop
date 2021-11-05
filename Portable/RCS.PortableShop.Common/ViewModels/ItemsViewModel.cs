using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
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

        protected override void SetCommands()
        {
            base.SetCommands();

            DetailsCommand = new AsyncCommand<TItem>(ShowDetails);
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
        protected override async Task ClearView()
        {
            await base.ClearView().ConfigureAwait(true);

            await MainThread.InvokeOnMainThreadAsync(() => Items?.Clear()).ConfigureAwait(true);
        }
        #endregion

        #region Navigation
        private static readonly BindableProperty DetailsCommandProperty =
            BindableProperty.Create(nameof(DetailsCommand), typeof(ICommand), typeof(ItemsViewModel<TItem>));

        public ICommand DetailsCommand
        {
            get => (ICommand)GetValue(DetailsCommandProperty);
            private set => SetValue(DetailsCommandProperty, value);
        }

        protected virtual async Task ShowDetails(TItem overviewObject)
        {
            await VoidTask();
        }
        #endregion
    }
}