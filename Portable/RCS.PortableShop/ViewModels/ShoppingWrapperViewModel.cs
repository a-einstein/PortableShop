using View = RCS.PortableShop.Common.Views.View;

namespace RCS.PortableShop.ViewModels
{
    // TODO Might as well keep this view and just change MainRegionContent.
    public class ShoppingWrapperViewModel : ShoppingViewModel
    {
        #region Construction

        private static readonly BindableProperty WrappedContentProperty =
            BindableProperty.Create(nameof(WrappedContent), typeof(View), typeof(ShoppingWrapperViewModel));

        // TODO This might better be moved to the View to better separate the two kind of objects.
        // TODO Once a View is assigned to MainView instead of MainViewModel the latter can be made implicit too.
        public View WrappedContent
        {
            get => (View)GetValue(WrappedContentProperty);
            set
            {
                SetValue(WrappedContentProperty, value);

                // Chain the Title properties.
                SetBinding(TitleProperty, new Binding() { Path = "Title", Source = WrappedContent.ViewModel });
            }
        }
        #endregion

        #region Refresh
        public override async Task RefreshView()
        {
            if (await Initialize().ConfigureAwait(true))
                await WrappedContent.ViewModel.RefreshView().ConfigureAwait(true);
        }

        public override string MakeTitle() { return WrappedContent?.ViewModel.MakeTitle(); }
        #endregion
    }
}
