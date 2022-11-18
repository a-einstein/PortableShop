using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace RCS.PortableShop.Common.Controls
{
    // Note this is a BindableObject.
    public abstract class CustomContentView : ContentView
    {
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            // Set the inner BindingContext of the controls to this, while the outer BindingContext of this is set externally.
            Content.BindingContext = this;
        }
    }
}