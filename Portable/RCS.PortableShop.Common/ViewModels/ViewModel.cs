using System.ComponentModel;
using Xamarin.Forms;

namespace RCS.PortableShop.Common.ViewModels
{
    public abstract class ViewModel : BindableObject, INotifyPropertyChanged
    {
        public ViewModel()
        {
            SetCommands();
        }

        protected virtual void SetCommands() { }

        protected const string databaseErrorMessage = "Error retrieving data from database.";

        public virtual void Refresh() { }

        protected static bool NullOrEmpty(string value)
        {
            return (value == null || value.Trim() == string.Empty);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // This signal can be particularly useful if a collection is entirely replaced, as the formerly bound collection no longer can.
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
