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
        public static readonly BindableProperty QuantityProperty =
            BindableProperty.Create(nameof(Quantity), typeof(int), typeof(CartItemViewModel), 0,
                // This basically does the same as in the Stepper (see source).
                // It is needed to keep the Entry in sync.
                coerceValue: (bindable, value) =>
                {
                    var cartItemViewModel = (CartItemViewModel)bindable;

                    var newValue = (int)value;
                    var clampedValue = Clamp(newValue, 0, 10);

                    if (clampedValue != newValue)
                        // Despite no actual change of value, notify controls (currently Entry).
                        cartItemViewModel.OnPropertyChanged(nameof(Quantity));

                    return clampedValue;
                },
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    var cartItemViewModel = (CartItemViewModel)bindable;

                    cartItemViewModel.CartItem.Quantity = (int)newValue;

                    // Do this before the PropertyChanged, to be available too.
                    cartItemViewModel.UpdateValue();

                    // Note the event is needed in CartViewModel.
                    cartItemViewModel.OnPropertyChanged(nameof(Quantity));
                }
            );

        public int Quantity
        {
            get { return (int)GetValue(QuantityProperty); }
            set { SetValue(QuantityProperty, value); }
        }

        private static readonly BindableProperty ValueProperty =
            BindableProperty.Create(nameof(Value), typeof(decimal), typeof(CartItemViewModel));

        public decimal Value
        {
            get => (decimal)GetValue(ValueProperty);
            private set => SetValue(ValueProperty, value);
        }
        #endregion

        #region Utility
        /* 
        Note https://stackoverflow.com/questions/2683442/where-can-i-find-the-clamp-function-in-net
        - NumericExtensions.Clamp is internal!
        - Math.Clamp does not exist in .Net Standard 2.0.
        TODO Make this more generally available, if needed.
        */
        public static int Clamp(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }
        #endregion
    }
}