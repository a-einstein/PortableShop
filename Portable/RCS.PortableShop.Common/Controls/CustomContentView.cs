using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RCS.PortableShop.Common.Controls
{
    public abstract class CustomContentView : ContentView, INotifyPropertyChanged
    {
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            // Set the inner BindingContext of the controls to this, while the outer BindingContext of this is set externally.
            Content.BindingContext = this;
        }

        public new event PropertyChangedEventHandler PropertyChanged;

        // This is needed  for intermediate value changes.
        // An initial binding usually works without, even without being a BindableProperty.
        // TODO This seems superfluous for a BindableProperty.
        protected void RaisePropertyChanged(string propertyName)
        {
            // Note the thread is particularly relevant for UWP.
            MainThread.BeginInvokeOnMainThread(() =>
            {
                // TODO This does not work for the inherited PropertyChanged.
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            });
        }
    }
}