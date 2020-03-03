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
    public abstract class FilterItemsViewModel<TItem, TMasterFilterItem, TDetailFilterItem> : ItemsViewModel<TItem>
        where TItem : DomainClass
        where TMasterFilterItem : DomainClass
        where TDetailFilterItem : DomainClass
    {
        #region Construction
        protected override void SetCommands()
        {
            base.SetCommands();

            FilterCommand = new Command(async () => await Refresh().ConfigureAwait(true), FilterCanExecute);
            DetailsCommand = new Command<TItem>(ShowDetails);
        }
        #endregion

        #region Refresh
        public static readonly BindableProperty MessageProperty =
            BindableProperty.Create(nameof(Message), typeof(string), typeof(FilterItemsViewModel<TItem, TMasterFilterItem, TDetailFilterItem>));

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
            var baseInitialized = await base.Initialize().ConfigureAwait(true);

            if (baseInitialized && !filterInitialized)
            {
                Message = Labels.Initializing;

                // TODO This was intended to (also) be shown by the ActivityIndicator, but that currently does not work.
                filterInitialized = await InitializeFilters().ConfigureAwait(true);

                Message = string.Empty;
            }

            return (baseInitialized && filterInitialized);
        }

        protected override async Task<bool> Read()
        {
            // TODO This was intended to (also) be shown by the ActivityIndicator, but that currently does not work.
            Message = Labels.Searching;

            var succeeded = await ReadFiltered().ConfigureAwait(true);

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

        public override bool Awaiting
        {
            get => base.Awaiting;
            set
            {
                base.Awaiting = value;

                // Needed BeginInvokeOnMainThread to avoid Android exception about Looper threads because of ActivityIndicator.
                Device.BeginInvokeOnMainThread(() => (FilterCommand as Command)?.ChangeCanExecute());
            }
        }
        #endregion

        #region Filtering
        protected abstract Task<bool> InitializeFilters();

        public static readonly BindableProperty MasterFilterItemsProperty =
            BindableProperty.Create(nameof(MasterFilterItems), typeof(ObservableCollection<TMasterFilterItem>), typeof(FilterItemsViewModel<TItem, TMasterFilterItem, TDetailFilterItem>), defaultValue: new ObservableCollection<TMasterFilterItem>());

        // Note there seems to be an issue with updating bindings by ObservableCollection, or on the particular controls. See consequences elsewhere.
        public ObservableCollection<TMasterFilterItem> MasterFilterItems
        {
            get { return (ObservableCollection<TMasterFilterItem>)GetValue(MasterFilterItemsProperty); }
            set
            {
                SetValue(MasterFilterItemsProperty, value);
                RaisePropertyChanged(nameof(MasterFilterItems));
            }
        }

        public static readonly BindableProperty MasterFilterValueProperty =
            BindableProperty.Create(nameof(MasterFilterValue), typeof(TMasterFilterItem), typeof(FilterItemsViewModel<TItem, TMasterFilterItem, TDetailFilterItem>), propertyChanged: new BindingPropertyChangedDelegate(OnMasterFilterValueChanged));

        public virtual TMasterFilterItem MasterFilterValue
        {
            get { return (TMasterFilterItem)GetValue(MasterFilterValueProperty); }
            set
            {
                SetValue(MasterFilterValueProperty, value);
                (FilterCommand as Command)?.ChangeCanExecute();
                RaisePropertyChanged(nameof(MasterFilterValue));
            }
        }

        // Note this function does NOT filter Items, just updates DetailFilterItems and DetailFilterValue.
        // Currently the FilterCommand is just bound to a Button, implying it always has to be activated explicitly.
        private static void OnMasterFilterValueChanged(BindableObject bindableObject, object oldValue, object newValue)
        {
            var viewModel = bindableObject as FilterItemsViewModel<TItem, TMasterFilterItem, TDetailFilterItem>;

            viewModel?.SetDetailFilterItems();
            viewModel.DetailFilterValue = viewModel.DetailFilterItems.FirstOrDefault();
        }

        // TODO Some sort of view would be more convenient.
        private void SetDetailFilterItems()
        {
            var detailFilterItemsSelection = DetailFilterItemsSource.Where(DetailFilterItemsSelector());

            var detailFilterItems = new ObservableCollection<TDetailFilterItem>(); ;

            // Note that the query is executed on the foreach.
            foreach (var item in detailFilterItemsSelection)
            {
                detailFilterItems.Add(item);
            }

            // Do an assignment, as just changing the ObservableCollection plus even a PropertyChanged does not work. There seems to be no good way to handle CollectionChanged. 
            // TODO maybe follow the approach on ItemsViewModel.Items.
            DetailFilterItems = detailFilterItems;
        }

        protected abstract Func<TDetailFilterItem, bool> DetailFilterItemsSelector(bool addEmptyElement = true);

        protected Collection<TDetailFilterItem> DetailFilterItemsSource { get; } = new Collection<TDetailFilterItem>();

        public static readonly BindableProperty DetailFilterItemsProperty =
            BindableProperty.Create(nameof(DetailFilterItems), typeof(ObservableCollection<TDetailFilterItem>), typeof(FilterItemsViewModel<TItem, TMasterFilterItem, TDetailFilterItem>), defaultValue: new ObservableCollection<TDetailFilterItem>());

        // Note there seems to be an issue with updating bindings by ObservableCollection, or on the particular controls. See consequences elsewhere.
        public virtual ObservableCollection<TDetailFilterItem> DetailFilterItems
        {
            get { return (ObservableCollection<TDetailFilterItem>)GetValue(DetailFilterItemsProperty); }
            set
            {
                SetValue(DetailFilterItemsProperty, value);
                RaisePropertyChanged(nameof(DetailFilterItems));
            }
        }

        public static readonly BindableProperty DetailFilterValueProperty =
            BindableProperty.Create(nameof(DetailFilterValue), typeof(TDetailFilterItem), typeof(FilterItemsViewModel<TItem, TMasterFilterItem, TDetailFilterItem>));

        // Note DetailFilterValue should only have a value if MasterFilterValue has.
        public TDetailFilterItem DetailFilterValue
        {
            get { return (TDetailFilterItem)GetValue(DetailFilterValueProperty); }
            set
            {
                SetValue(DetailFilterValueProperty, value);
                RaisePropertyChanged(nameof(DetailFilterValue));
            }
        }

        public static readonly BindableProperty TextFilterValueProperty =
            BindableProperty.Create(nameof(TextFilterValue), typeof(string), typeof(FilterItemsViewModel<TItem, TMasterFilterItem, TDetailFilterItem>));

        public virtual string TextFilterValue
        {
            get { return (string)GetValue(TextFilterValueProperty); }
            set
            {
                SetValue(TextFilterValueProperty, value);
                (FilterCommand as Command)?.ChangeCanExecute();
                RaisePropertyChanged(nameof(TextFilterValue));
            }
        }

        protected abstract bool FilterCanExecute();

        protected abstract Task<bool> ReadFiltered();

        public static readonly BindableProperty FilterCommandProperty =
            BindableProperty.Create(nameof(FilterCommand), typeof(ICommand), typeof(FilterItemsViewModel<TItem, TMasterFilterItem, TDetailFilterItem>));

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

        #region Navigation
        public static readonly BindableProperty DetailsCommandProperty =
            BindableProperty.Create(nameof(DetailsCommand), typeof(ICommand), typeof(FilterItemsViewModel<TItem, TMasterFilterItem, TDetailFilterItem>));

        public ICommand DetailsCommand
        {
            get { return (ICommand)GetValue(DetailsCommandProperty); }
            private set
            {
                SetValue(DetailsCommandProperty, value);
                RaisePropertyChanged(nameof(DetailsCommand));
            }
        }

        protected abstract void ShowDetails(TItem overviewObject);
        #endregion
    }
}