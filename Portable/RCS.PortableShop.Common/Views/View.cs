﻿using RCS.PortableShop.Common.Extensions;
using RCS.PortableShop.Common.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

// Arbitrarily put here for the assembly.
// Also check comments on XamlCompilation elsewhere.
[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace RCS.PortableShop.Common.Views
{
    // TODO Maybe change this name as it is already used by Xamarin.
    // This no longer can be abstract as it needs a default constructor for the regions.
    public class View : ContentView
    {
        public ViewModel ViewModel
        {
            get { return BindingContext as ViewModel; }
            set { BindingContext = value; }
        }

        public async Task Refresh()
        {
            await Task.Run(() =>
            {
                // Use this because of the ConfigureAwait.
                ViewModel.IfNotNull(async viewModel => await viewModel.Refresh().ConfigureAwait(true));
            }
            ).ConfigureAwait(true);
        }

        protected static void Orientate(ref StackLayout stack, ref int preservedWidth, ref int preservedHeight, double width, double height)
        {
            //Make comparison more robust.
            int newWidth = (int)Math.Round(width);
            int newHeight = (int)Math.Round(height);
            int voidValue = -1;

            // Prevent unnecessary orientation changes.
            if (newWidth != voidValue && newHeight != voidValue &&
                (preservedWidth != newWidth || preservedHeight != newHeight))
            {
                var newOrientation = newWidth < newHeight
                    ? StackOrientation.Vertical
                    : StackOrientation.Horizontal;

                if (stack.Orientation != newOrientation)
                    stack.Orientation = newOrientation;

                preservedWidth = newWidth;
                preservedHeight = newHeight;
            }
        }
    }
}
