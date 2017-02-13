﻿using System.ComponentModel;
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

        public static readonly BindableProperty AwaitingProperty =
            BindableProperty.Create(nameof(Awaiting), typeof(bool), typeof(ViewModel), defaultValue: false);

        public bool Awaiting
        {
            get { return (bool)GetValue(AwaitingProperty); }
            set
            {
                SetValue(AwaitingProperty, value);

                // TODO Should this be needed?
                RaisePropertyChanged(nameof(Awaiting));
            }
        }

        protected static bool NullOrEmpty(string value)
        {
            return (value == null || value.Trim() == string.Empty);
        }

        // TODO Check the inherited PropertyChanged.
        public event PropertyChangedEventHandler PropertyChanged;

        // This signal can be particularly useful if a collection is entirely replaced, as the formerly bound collection no longer can.
        protected void RaisePropertyChanged(string propertyName)
        {
            // TODO This does not work for the inherited PropertyChanged.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public INavigation Navigation { get; set; }

        // Note that a potential Color parameter cannot have a default value.
        protected void PushPage(View view, string title = null)
        {
            var page = new ContentPage() { Content = view, Title = title };

            Navigation.PushAsync(page);
        }
    }
}
