using RCS.AdventureWorks.Common.DomainClasses;
using RCS.PortableShop.Resources;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using static Xamarin.Forms.BindableProperty;

namespace RCS.PortableShop.Common.ViewModels
{
    public abstract class FilterItemsViewModel<I, FM, FD> : ItemsViewModel<I>
        where I : DomainClass
        where FM : DomainClass
        where FD : DomainClass
    {
        #region Construction
        protected override void SetCommands()
        {
            base.SetCommands();

            FilterCommand = new Command(async () => await Refresh());
            DetailsCommand = new Command<I>(ShowDetails);
        }
        #endregion

        #region Refresh
        public static readonly BindableProperty MessageProperty =
            BindableProperty.Create(nameof(Message), typeof(string), typeof(FilterItemsViewModel<I, FM, FD>));

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set
            {
                SetValue(MessageProperty, value);
                RaisePropertyChanged(nameof(Message));
            }
        }

        private bool filterInitialized;

        protected override async Task<bool> Initialize()
        {
            var baseInitialized = await base.Initialize();

            if (baseInitialized && !filterInitialized)
            {
                Message = Labels.Initializing;

                // TODO This was intended to (also) be shown by the ActivityIndicator, but that currently does not work.
                filterInitialized = await InitializeFilters();

                Message = string.Empty;
            }

            return (baseInitialized && filterInitialized);
        }

        protected override async Task<bool> Read()
        {
            // TODO This was intended to (also) be shown by the ActivityIndicator, but that currently does not work.
            Message = Labels.Searching;

            var succeeded = await ReadFiltered();

            Message = (succeeded && ItemsCount == 0) ? Labels.NotFound : string.Empty;

            return succeeded;
        }

        public override string MakeTitle()
        {
            var title = (!string.IsNullOrEmpty(DetailFilterValue?.Name))
               ? DetailFilterValue?.Name
               : MasterFilterValue?.Name;

            return (ItemsCount != 0 && !string.IsNullOrEmpty(title)) ? title : TitleDefault;
        }
        #endregion

        #region Filtering
        protected abstract Task<bool> InitializeFilters();

        public static readonly BindableProperty MasterFilterItemsProperty =
            BindableProperty.Create(nameof(MasterFilterItems), typeof(ObservableCollection<FM>), typeof(FilterItemsViewModel<I, FM, FD>), defaultValue: new ObservableCollection<FM>());

        // Note there seems to be an issue with updating bindings by ObservableCollection, or on the particular controls. See consequences elsewhere.
        public ObservableCollection<FM> MasterFilterItems
        {
            get { return (ObservableCollection<FM>)GetValue(MasterFilterItemsProperty); }
            set
            {
                SetValue(MasterFilterItemsProperty, value);
                RaisePropertyChanged(nameof(MasterFilterItems));
            }
        }

        public static readonly BindableProperty MasterFilterValueProperty =
            BindableProperty.Create(nameof(MasterFilterValue), typeof(FM), typeof(FilterItemsViewModel<I, FM, FD>), propertyChanged: new BindingPropertyChangedDelegate(OnMasterFilterValueChanged));

        public FM MasterFilterValue
        {
            get { return (FM)GetValue(MasterFilterValueProperty); }
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
            var viewModel = bindableObject as FilterItemsViewModel<I, FM, FD>;

            viewModel.SetDetailFilterItems();
            viewModel.DetailFilterValue = viewModel.DetailFilterItems.FirstOrDefault();
        }

        // TODO Some sort of view would be more convenient.
        private void SetDetailFilterItems(bool addEmptyElement = true)
        {
            var detailFilterItemsSelection = detailFilterItemsSource.Where(DetailFilterItemsSelector());

            ObservableCollection<FD> detailFilterItems = new ObservableCollection<FD>(); ;

            // Note that the query is executed on the foreach.
            foreach (var item in detailFilterItemsSelection)
            {
                detailFilterItems.Add(item);
            }

            // Do an assignment, as just changing the ObservableCollection plus even a PropertyChanged does not work. There seems to be no good way to handle CollectionChanged. 
            // TODO maybe follow the approach on ItemsViewModel.Items.
            DetailFilterItems = detailFilterItems;
        }

        protected abstract Func<FD, bool> DetailFilterItemsSelector(bool addEmptyElement = true);

        protected Collection<FD> detailFilterItemsSource = new Collection<FD>();

        public static readonly BindableProperty DetailFilterItemsProperty =
            BindableProperty.Create(nameof(DetailFilterItems), typeof(ObservableCollection<FD>), typeof(FilterItemsViewModel<I, FM, FD>), defaultValue: new ObservableCollection<FD>());

        // Note there seems to be an issue with updating bindings by ObservableCollection, or on the particular controls. See consequences elsewhere.
        public ObservableCollection<FD> DetailFilterItems
        {
            get { return (ObservableCollection<FD>)GetValue(DetailFilterItemsProperty); }
            set
            {
                SetValue(DetailFilterItemsProperty, value);
                RaisePropertyChanged(nameof(DetailFilterItems));
            }
        }

        public static readonly BindableProperty DetailFilterValueProperty =
            BindableProperty.Create(nameof(DetailFilterValue), typeof(FD), typeof(FilterItemsViewModel<I, FM, FD>));

        public FD DetailFilterValue
        {
            get { return (FD)GetValue(DetailFilterValueProperty); }
            set
            {
                SetValue(DetailFilterValueProperty, value);
                RaisePropertyChanged(nameof(DetailFilterValue));
            }
        }

        public static readonly BindableProperty TextFilterValueProperty =
            BindableProperty.Create(nameof(TextFilterValue), typeof(string), typeof(FilterItemsViewModel<I, FM, FD>));

        public string TextFilterValue
        {
            get { return (string)GetValue(TextFilterValueProperty); }
            set
            {
                SetValue(TextFilterValueProperty, value);
                RaisePropertyChanged(nameof(TextFilterValue));
            }
        }

        protected abstract Task<bool> ReadFiltered();

        public static readonly BindableProperty FilterCommandProperty =
            BindableProperty.Create(nameof(FilterCommand), typeof(ICommand), typeof(FilterItemsViewModel<I, FM, FD>));

        public ICommand FilterCommand
        {
            get { return (ICommand)GetValue(FilterCommandProperty); }
            private set
            {
                SetValue(FilterCommandProperty, value);
                RaisePropertyChanged(nameof(FilterCommand));
            }
        }
        #endregion

        #region Details
        public static readonly BindableProperty DetailsCommandProperty =
            BindableProperty.Create(nameof(DetailsCommand), typeof(ICommand), typeof(FilterItemsViewModel<I, FM, FD>));

        public ICommand DetailsCommand
        {
            get { return (ICommand)GetValue(DetailsCommandProperty); }
            private set
            {
                SetValue(DetailsCommandProperty, value);
                RaisePropertyChanged(nameof(DetailsCommand));
            }
        }

        protected abstract void ShowDetails(I overviewObject);
        #endregion
    }
}