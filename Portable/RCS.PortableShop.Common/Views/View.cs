using RCS.PortableShop.Common.ViewModels;
using System;
using Xamarin.Forms;

namespace RCS.PortableShop.Common.Views
{
    // This no longer can be abstract as it needs a default constructor for the regions.
    public class View : ContentView
    {
        public ViewModel ViewModel
        {
            get { return BindingContext as ViewModel; }
            set
            {
                BindingContext = value;
                ViewModel.Navigation = Navigation;
            }
        }

        // TODO Prevent service call reloading data.
        // TODO Prevent reset to overview from details view.
        protected void Orientate(ref StackLayout stack, ref int preservedWidth, ref int preservedHeight, double width, double height)
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
