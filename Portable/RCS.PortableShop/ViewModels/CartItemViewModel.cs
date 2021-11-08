using RCS.AdventureWorks.Common.DomainClasses;
using RCS.PortableShop.Common.ViewModels;
using System.Diagnostics;
using Xamarin.Forms;

namespace RCS.PortableShop.ViewModels
{
    /// <summary>
    /// Single level Viewmodel on CartItem.
    /// </summary>
    [DebuggerDisplay("{Name} : {ProductListPrice} x {Quantity} = {Value}")]
    public class CartItemViewModel : ViewModel
    {
        #region Construction
        public CartItemViewModel(CartItem cartItem)
        {
            CartItem = cartItem;

            // Update binding.
            Quantity = CartItem.Quantity;
        }

        /// <summary>
        /// Reference into the repository. 
        /// The model for this object.
        /// </summary>
        public CartItem CartItem { get; }

        private void UpdateValue()
        {
            Value = ProductListPrice * Quantity;
        }
        #endregion

        #region Bindable but not updateable.
        public int? Id => CartItem.Id;
        public string Name => CartItem.Name;
        public int ProductId => CartItem.ProductId;
        public string ProductSize => CartItem.ProductSize;
        public string ProductSizeUnitMeasureCode => CartItem.ProductSizeUnitMeasureCode;
        public string ProductColor => CartItem.ProductColor;
        public decimal ProductListPrice => CartItem.ProductListPrice;
        #endregion

        #region Bindable and updateable.
        public int Quantity
        {
            get => CartItem.Quantity;
            set
            {
                CartItem.Quantity = value;

                // Do this before the PropertyChanged, to be available too.
                UpdateValue();

                // Need this because this is no BindableProperty, for I also want to use CartItem.Quantity instead of duplicating it.
                OnPropertyChanged(nameof(Quantity));
            }
        }

        private static readonly BindableProperty ValueProperty =
            BindableProperty.Create(nameof(Value), typeof(decimal), typeof(CartItemViewModel));

        public decimal Value
        {
            get => (decimal)GetValue(ValueProperty);
            private set => SetValue(ValueProperty, value);
        }
        #endregion
    }
}