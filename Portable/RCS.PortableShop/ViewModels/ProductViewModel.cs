﻿using RCS.AdventureWorks.Common.DomainClasses;
using RCS.PortableShop.Common.ViewModels;
using RCS.PortableShop.Interfaces;
using RCS.PortableShop.Model;
using System.Windows.Input;
using Xamarin.Forms;

namespace RCS.PortableShop.ViewModels
{
    public class ProductViewModel : ItemViewModel<Product>, IShopper
    {
        #region Initialize
        protected override void SetCommands()
        {
            base.SetCommands();

            PhotoCommand = new Command<ImageSource>(ShowPhoto);
            CartCommand = new Command<Product>(CartProduct);
        }
        #endregion

        #region Filtering
        public override async void Refresh(object productId)
        {
            Item = await ProductsRepository.Instance.ReadDetails((int)productId);
        }
        #endregion

        #region Photo
        public ICommand PhotoCommand { get; private set; }

        // Use the existing ImageSource to avoid an unnecessary conversion.
        private void ShowPhoto(ImageSource imageSource)
        {
            var contentView = new ContentView()
            {
                BackgroundColor = (Color)Application.Current.Resources["ProductsLevel1Colour"],
                Content= new Image() { Source = imageSource }
            };

            PushPage(contentView, Item?.Name);
        }
        #endregion

        #region Shopping

        // Note this does not work as explicit interface implementation.
        public ICommand CartCommand { get; set; }

        private void CartProduct(Product product)
        {
            ShoppingCartViewModel.Instance.CartProduct(product);
        }
        #endregion
    }
}