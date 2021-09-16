using RCS.AdventureWorks.Common.DomainClasses;
using System.Diagnostics;
using Xamarin.Forms;

namespace RCS.PortableShop.GuiModel
{
    /// <summary>
    /// Wrapper around CartItem.
    /// </summary>
    [DebuggerDisplay("{Name} : {ProductListPrice} x {Quantity} = {Value}")]
    public class GuiCartItem : BindableObject
    {
        #region Construction
        public GuiCartItem(CartItem cartItem)
        {
            this.CartItem = cartItem;

            SetValue();
        }

        /// <summary>
        /// Local copy, as a reference into the repository is not possible. 
        /// </summary>
        public CartItem CartItem { get; }

        private void SetValue()
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

                // Need this because this is no BindableProperty, as I also want to use CartItem.Quantity instead of duplicating it.
                OnPropertyChanged(nameof(Quantity));

                SetValue();
            }
        }

        private static readonly BindableProperty ValueProperty =
            BindableProperty.Create(nameof(Value), typeof(decimal), typeof(GuiCartItem));

        public decimal Value
        {
            get => (decimal)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
        #endregion
    }
}