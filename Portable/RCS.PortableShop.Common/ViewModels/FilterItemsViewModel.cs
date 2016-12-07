using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using static Xamarin.Forms.BindableProperty;

namespace RCS.PortableShop.Common.ViewModels
{
    public abstract class FilterItemsViewModel<T, V, W> : ItemsViewModel<T>
    {
        protected abstract Task InitializeFilters();

        public ObservableCollection<V> MasterFilterItems { get; } = new ObservableCollection<V>();

        public static readonly BindableProperty MasterFilterValueProperty =
            BindableProperty.Create(nameof(MasterFilterValue), typeof(V), typeof(FilterItemsViewModel<T, V, W>), propertyChanging : new BindingPropertyChangingDelegate(OnMasterFilterValueChanged));

        public V MasterFilterValue
        {
            get { return (V)GetValue(MasterFilterValueProperty); }
            set { SetValue(MasterFilterValueProperty, value); }
        }


        // Note this function does NOT filter Items, just updates DetailFilterItems and DetailFilterValue.
        // Currently the FilterCommand is just bound to a Button, implying it always has to be activated explicitly.
        public static void OnMasterFilterValueChanged(BindableObject bindableObject, object oldValue, object newValue)
        {
            var viewModel = bindableObject as FilterItemsViewModel<T, V, W>;

            /*
            viewModel.SetDetailFilterItems();
            viewModel.DetailFilterValue = viewModel.DetailFilterItems.FirstOrDefault();
            */
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

        public static readonly BindableProperty DetailFilterValueProperty =
            BindableProperty.Create(nameof(DetailFilterValue), typeof(W), typeof(FilterItemsViewModel<T, V, W>));

        public W DetailFilterValue
        {
            get { return (W)GetValue(DetailFilterValueProperty); }
            set { SetValue(DetailFilterValueProperty, value); }
        }

        public static readonly BindableProperty TextFilterValueProperty =
            BindableProperty.Create(nameof(TextFilterValue), typeof(string), typeof(FilterItemsViewModel<T, V, W>));

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
            FilterCommand = new Command(Refresh);
            DetailsCommand = new Command<T>(ShowDetails);
        }
    }
}
