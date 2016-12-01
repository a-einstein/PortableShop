using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RCS.WpfShop.Common.ViewModels
{
    public abstract class FilterItemsViewModel<T, V, W> : ItemsViewModel<T>
    {
        protected abstract Task InitializeFilters();

        public ObservableCollection<V> MasterFilterItems { get; } = new ObservableCollection<V>();

        public static readonly DependencyProperty MasterFilterValueProperty =
            DependencyProperty.Register(nameof(MasterFilterValue), typeof(V), typeof(ItemsViewModel<T>), new PropertyMetadata(new PropertyChangedCallback(OnMasterFilterValueChanged)));

        public V MasterFilterValue
        {
            get { return (V)GetValue(MasterFilterValueProperty); }
            set { SetValue(MasterFilterValueProperty, value); }
        }

        // Note this function does NOT filter Items, just updates DetailFilterItems and DetailFilterValue.
        // Currently the FilterCommand is just bound to a Button, implying it always has to be activated explicitly.
        private static void OnMasterFilterValueChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            var viewModel = dependencyObject as FilterItemsViewModel<T, V, W>;

            viewModel.SetDetailFilterItems();
            viewModel.DetailFilterValue = viewModel.DetailFilterItems.FirstOrDefault();
        }

        // TODO Some sort of view would be more convenient.
        private void SetDetailFilterItems(bool addEmptyElement = true)
        {
            var detailFilterItemsSelection = detailFilterItemsSource.Where(DetailFilterItemsSelector());

            DetailFilterItems.Clear();

            // Note that the query is executed on the foreach.
            foreach (var item in detailFilterItemsSelection)
            {
                DetailFilterItems.Add(item);
            }

            // To trigger the enablement.
            RaisePropertyChanged(nameof(DetailFilterItems));
        }

        protected abstract Func<W, bool> DetailFilterItemsSelector(bool addEmptyElement = true);

        protected Collection<W> detailFilterItemsSource = new Collection<W>();
        public ObservableCollection<W> DetailFilterItems { get; } = new ObservableCollection<W>();

        public static readonly DependencyProperty DetailFilterValueProperty =
            DependencyProperty.Register(nameof(DetailFilterValue), typeof(W), typeof(ItemsViewModel<T>));

        public W DetailFilterValue
        {
            get { return (W)GetValue(DetailFilterValueProperty); }
            set { SetValue(DetailFilterValueProperty, value); }
        }

        public static readonly DependencyProperty TextFilterValueProperty =
            DependencyProperty.Register(nameof(TextFilterValue), typeof(string), typeof(ItemsViewModel<T>));

        public string TextFilterValue
        {
            get { return (string)GetValue(TextFilterValueProperty); }
            set { SetValue(TextFilterValueProperty, value); }
        }

        public ICommand FilterCommand { get; private set; }

        public ICommand DetailsCommand { get; private set; }

        protected abstract void ShowDetails(T overviewObject);

        protected override void SetCommands()
        {
            FilterCommand = new DelegateCommand(Refresh);
            DetailsCommand = new DelegateCommand<T>(ShowDetails);
        }
    }
}
