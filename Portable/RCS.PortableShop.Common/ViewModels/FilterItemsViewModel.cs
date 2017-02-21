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
        #region Initialization
        protected override void SetCommands()
        {
            FilterCommand = new Command(Refresh);
            DetailsCommand = new Command<T>(ShowDetails);
        }
        #endregion

        #region Filtering
        protected abstract Task InitializeFilters();

        public static readonly BindableProperty MasterFilterItemsProperty =
            BindableProperty.Create(nameof(MasterFilterItems), typeof(ObservableCollection<V>), typeof(FilterItemsViewModel<T, V, W>), defaultValue: new ObservableCollection<V>());

        // Note there seems to be an issue with updating bindings by ObservableCollection, or on the particular controls. See consequences elsewhere.
        public ObservableCollection<V> MasterFilterItems
        {
            get { return (ObservableCollection<V>)GetValue(MasterFilterItemsProperty); }
            set
            {
                SetValue(MasterFilterItemsProperty, value);
                RaisePropertyChanged(nameof(MasterFilterItems));
            }
        }

        public static readonly BindableProperty MasterFilterValueProperty =
            BindableProperty.Create(nameof(MasterFilterValue), typeof(V), typeof(FilterItemsViewModel<T, V, W>), propertyChanged : new BindingPropertyChangedDelegate(OnMasterFilterValueChanged));

        public V MasterFilterValue
        {
            get { return (V)GetValue(MasterFilterValueProperty); }
            set
            {
                SetValue(MasterFilterValueProperty, value);
                RaisePropertyChanged(nameof(MasterFilterValue));
            }
        }

        // Note this function does NOT filter Items, just updates DetailFilterItems and DetailFilterValue.
        // Currently the FilterCommand is just bound to a Button, implying it always has to be activated explicitly.
        public static void OnMasterFilterValueChanged(BindableObject bindableObject, object oldValue, object newValue)
        {
            var viewModel = bindableObject as FilterItemsViewModel<T, V, W>;

            viewModel.SetDetailFilterItems();
            viewModel.DetailFilterValue = viewModel.DetailFilterItems.FirstOrDefault();
        }

        // TODO Some sort of view would be more convenient.
        private void SetDetailFilterItems(bool addEmptyElement = true)
        {
            var detailFilterItemsSelection = detailFilterItemsSource.Where(DetailFilterItemsSelector());

            ObservableCollection<W> detailFilterItems = new ObservableCollection<W>(); ;

            // Note that the query is executed on the foreach.
            foreach (var item in detailFilterItemsSelection)
            {
                detailFilterItems.Add(item);
            }

            // Do an assignment, as just changing the ObservableCollection plus even a PropertyChanged does not work. There seems to be no good way to handle CollectionChanged. 
            // TODO maybe follow the approach on ItemsViewModel.Items.
            DetailFilterItems = detailFilterItems;
        }

        protected abstract Func<W, bool> DetailFilterItemsSelector(bool addEmptyElement = true);

        protected Collection<W> detailFilterItemsSource = new Collection<W>();

        public static readonly BindableProperty DetailFilterItemsProperty =
            BindableProperty.Create(nameof(DetailFilterItems), typeof(ObservableCollection<W>), typeof(FilterItemsViewModel<T, V, W>), defaultValue: new ObservableCollection<W>());

        // Note there seems to be an issue with updating bindings by ObservableCollection, or on the particular controls. See consequences elsewhere.
        public ObservableCollection<W> DetailFilterItems
        {
            get { return (ObservableCollection<W>)GetValue(DetailFilterItemsProperty); }
            set
            {
                SetValue(DetailFilterItemsProperty, value);
                RaisePropertyChanged(nameof(DetailFilterItems));
            }
        }

        public static readonly BindableProperty DetailFilterValueProperty =
            BindableProperty.Create(nameof(DetailFilterValue), typeof(W), typeof(FilterItemsViewModel<T, V, W>));

        public W DetailFilterValue
        {
            get { return (W)GetValue(DetailFilterValueProperty); }
            set
            {
                SetValue(DetailFilterValueProperty, value);
                RaisePropertyChanged(nameof(DetailFilterValue));
            }
        }

        public static readonly BindableProperty TextFilterValueProperty =
            BindableProperty.Create(nameof(TextFilterValue), typeof(string), typeof(FilterItemsViewModel<T, V, W>));

        public string TextFilterValue
        {
            get { return (string)GetValue(TextFilterValueProperty); }
            set
            {
                SetValue(TextFilterValueProperty, value);
                RaisePropertyChanged(nameof(TextFilterValue));
            }
        }

        public ICommand FilterCommand { get; private set; }
        #endregion

        #region Details
        public ICommand DetailsCommand { get; private set; }

        protected abstract void ShowDetails(T overviewObject);
        #endregion

    }
}
