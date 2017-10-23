using RCS.AdventureWorks.Common.DomainClasses;
using Xamarin.Forms;

namespace RCS.PortableShop.Common.ViewModels
{
    public abstract class ItemViewModel<I> : ViewModel where I : DomainClass
    {
        #region Item
        // Store the ID separately to enable a retry on an interrupted Refresh.
        // Note that a generic property is impossible, so the DomainClass is needed, determine the property's type.
        public int? ItemId { get; set; }

        public static readonly BindableProperty ItemProperty =
            BindableProperty.Create(nameof(Item), typeof(I), typeof(ItemViewModel<I>));

        public I Item
        {
            get { return (I)GetValue(ItemProperty); }
            set
            {
                SetValue(ItemProperty, value);
                RaisePropertyChanged(nameof(Item));

                UpdateTitle();
            }
        }
        #endregion

        #region Refresh
        public override string MakeTitle() { return (!string.IsNullOrEmpty(Item?.Name)) ? Item?.Name : TitleDefault; }

        #endregion
    }
}