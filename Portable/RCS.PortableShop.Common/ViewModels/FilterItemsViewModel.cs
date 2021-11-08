using RCS.AdventureWorks.Common.DomainClasses;
using RCS.PortableShop.Resources;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RCS.PortableShop.Common.ViewModels
{
    public abstract class FilterItemsViewModel<TItem, TMasterFilterItem, TDetailFilterItem> : 
        ItemsViewModel<TItem>
        where TItem : DomainClass
        where TMasterFilterItem : DomainClass
        where TDetailFilterItem : DomainClass
    {
        #region Construction
        protected override void SetCommands()
        {
            base.SetCommands();

            // Note that allowsMultipleExecutions replaces the former use of the Awaiting property.
            FilterCommand = new AsyncCommand(RefreshView, FilterCanExecute, allowsMultipleExecutions: false);
        }
        #endregion

        #region Refresh
        private static readonly BindableProperty MessageProperty =
            BindableProperty.Create(nameof(Message), typeof(string), typeof(FilterItemsViewModel<TItem, TMasterFilterItem, TDetailFilterItem>));

        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
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

            return baseInitialized && filterInitialized;
        }

        protected override async Task Read()
        {
            // TODO This was intended to (also) be shown by the ActivityIndicator, but that currently does not work.
            Message = Labels.Searching;

            var task = ReadFiltered();
            await task.ConfigureAwait(true);
            var succeeded = task.Status != TaskStatus.Faulted;

            Message = succeeded && ItemsCount == 0 ? Labels.NotFound : string.Empty;
        }

        public override string MakeTitle()
        {
            var title = !string.IsNullOrEmpty(DetailFilterValue?.Name)
               ? DetailFilterValue?.Name
               : MasterFilterValue?.Name;

            return ItemsCount != 0 && !string.IsNullOrEmpty(title) ? title : TitleDefault;
        }
        #endregion

        #region Filtering

        private bool filterChanged;
        protected bool FilterChanged
        {
            get => filterChanged;
            set
            {
                filterChanged = value;

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    FilterCommand.RaiseCanExecuteChanged();
                });
            }
        }

        protected abstract Task<bool> InitializeFilters();

        public ObservableCollection<TMasterFilterItem> MasterFilterItems { get; } = new ObservableCollection<TMasterFilterItem>();

        private static readonly BindableProperty MasterFilterValueProperty =
            BindableProperty.Create(nameof(MasterFilterValue), typeof(TMasterFilterItem), typeof(FilterItemsViewModel<TItem, TMasterFilterItem, TDetailFilterItem>), propertyChanged: OnMasterFilterValueChanged);

        public virtual TMasterFilterItem MasterFilterValue
        {
            get => (TMasterFilterItem)GetValue(MasterFilterValueProperty);
            set => SetValue(MasterFilterValueProperty, value);
        }

        // Note this function does NOT filter Items, just updates DetailFilterItems and DetailFilterValue.
        // Currently the FilterCommand is just bound to a Button, implying it always has to be activated explicitly.
        private static void OnMasterFilterValueChanged(BindableObject bindableObject, object oldValue, object newValue)
        {
            var viewModel = bindableObject as FilterItemsViewModel<TItem, TMasterFilterItem, TDetailFilterItem>;

            viewModel?.SetDetailFilterItems();
            viewModel.DetailFilterValue = viewModel.DetailFilterItems.FirstOrDefault();

            viewModel.FilterChanged = true;
        }

        // TODO Some sort of view would be more convenient.
        private void SetDetailFilterItems()
        {
            DetailFilterItems.Clear();

            var detailFilterItemsSelection = DetailFilterItemsSource.Where(DetailFilterItemsSelector());

            // Note that the query is executed on the foreach.
            foreach (var item in detailFilterItemsSelection)
            {
                DetailFilterItems.Add(item);
            }

            // Extra event. For some bindings (ItemsSource) those from ObservableCollection are enough, but for others (IsEnabled) this is needed.
            OnPropertyChanged(nameof(DetailFilterItems));
        }

        protected abstract Func<TDetailFilterItem, bool> DetailFilterItemsSelector(bool addEmptyElement = true);

        protected Collection<TDetailFilterItem> DetailFilterItemsSource { get; } = new Collection<TDetailFilterItem>();

        public ObservableCollection<TDetailFilterItem> DetailFilterItems { get; } = new ObservableCollection<TDetailFilterItem>();

        private static readonly BindableProperty DetailFilterValueProperty =
            BindableProperty.Create(nameof(DetailFilterValue), typeof(TDetailFilterItem), typeof(FilterItemsViewModel<TItem, TMasterFilterItem, TDetailFilterItem>), propertyChanged: OnDetailFilterValueChanged);

        // Note DetailFilterValue should only have a value if MasterFilterValue has.
        public virtual TDetailFilterItem DetailFilterValue
        {
            get => (TDetailFilterItem)GetValue(DetailFilterValueProperty);
            set => SetValue(DetailFilterValueProperty, value);
        }

        private static void OnDetailFilterValueChanged(BindableObject bindableObject, object oldValue, object newValue)
        {
            var viewModel = bindableObject as FilterItemsViewModel<TItem, TMasterFilterItem, TDetailFilterItem>;

            viewModel.FilterChanged = true;
        }

        private static readonly BindableProperty TextFilterValueProperty =
            BindableProperty.Create(nameof(TextFilterValue), typeof(string), typeof(FilterItemsViewModel<TItem, TMasterFilterItem, TDetailFilterItem>), propertyChanged: OnTextFilterValueChanged, defaultBindingMode: BindingMode.TwoWay);

        public virtual string TextFilterValue
        {
            get => (string)GetValue(TextFilterValueProperty);
            set => SetValue(TextFilterValueProperty, value);
        }

        private static void OnTextFilterValueChanged(BindableObject bindableObject, object oldValue, object newValue)
        {
            var viewModel = bindableObject as FilterItemsViewModel<TItem, TMasterFilterItem, TDetailFilterItem>;

            viewModel.FilterChanged = true;
        }

        protected virtual bool FilterCanExecute()
        {
            return FilterChanged;
        }

        protected abstract Task<bool> ReadFiltered();

        private static readonly BindableProperty FilterCommandProperty =
            BindableProperty.Create(nameof(FilterCommand), typeof(IAsyncCommand), typeof(FilterItemsViewModel<TItem, TMasterFilterItem, TDetailFilterItem>));

        public IAsyncCommand FilterCommand
        {
            get => (IAsyncCommand)GetValue(FilterCommandProperty);
            private set => SetValue(FilterCommandProperty, value);
        }

        public override async Task RefreshView()
        {
            await base.RefreshView();

            FilterChanged = false;
        }
        #endregion
    }
}