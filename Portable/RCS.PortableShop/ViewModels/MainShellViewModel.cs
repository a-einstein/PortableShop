﻿using CommunityToolkit.Mvvm.Input;
using RCS.PortableShop.Views;
using System.Windows.Input;
using Command = Microsoft.Maui.Controls.Command;

namespace RCS.PortableShop.ViewModels
{
    public class MainShellViewModel : ShoppingViewModel
    {
        #region Construction
        protected override void SetCommands()
        {
            base.SetCommands();

            // TODO MAUI Check out RelayCommand attribute, including CanExecute attribute.
            ShowProductsCommand = new AsyncRelayCommand(ShowProducts);
            OpenSupportCommand = new Command(OpenSupport);
            UpdateCommand = new Command(Update);
            SettingsCommand = new AsyncRelayCommand(OpenSettings);
        }
        #endregion

        #region Products

        private static readonly BindableProperty ShowProductsCommandProperty =
            BindableProperty.Create(nameof(ShowProductsCommand), typeof(ICommand), typeof(MainShellViewModel));

        public ICommand ShowProductsCommand
        {
            get => (ICommand)GetValue(ShowProductsCommandProperty);
            private set => SetValue(ShowProductsCommandProperty, value);
        }

        private static async Task ShowProducts()
        {
            Shell.FlyoutIsPresented = false;
            await PopToRoot().ConfigureAwait(true);
        }
        #endregion

        #region Cart
        protected override async Task ShowCart()
        {
            Shell.FlyoutIsPresented = false;
            await base.ShowCart().ConfigureAwait(true);
        }
        #endregion

        #region Support

        private static readonly BindableProperty OpenSupportCommandProperty =
            BindableProperty.Create(nameof(OpenSupportCommand), typeof(ICommand), typeof(MainShellViewModel));

        public ICommand OpenSupportCommand
        {
            get => (ICommand)GetValue(OpenSupportCommandProperty);
            private set => SetValue(OpenSupportCommandProperty, value);
        }

        private static void OpenSupport()
        {
            // TODO Make this Configurable.
            OpenWeb("https://a-einstein.github.io/PortableShop/");
        }
        #endregion

        #region Update

        private static readonly BindableProperty UpdateCommandProperty =
             BindableProperty.Create(nameof(UpdateCommand), typeof(ICommand), typeof(MainShellViewModel));

        public ICommand UpdateCommand
        {
            get => (ICommand)GetValue(UpdateCommandProperty);
            set => SetValue(UpdateCommandProperty, value);
        }

        private static void Update()
        {
            // Note this is only very rough and may fail on the still open application and other technicalities afterwards.

            // TODO Check versions first. Proceed with either reporting no update needed, or consent to close the application. Even better: check automatically at startup.

            // TODO Make this Configurable.
            OpenWeb("https://github.com/a-einstein/PortableShop/releases");
        }
        #endregion

        #region Settings
        private static readonly BindableProperty SettingsCommandProperty =
             BindableProperty.Create(nameof(SettingsCommand), typeof(ICommand), typeof(MainShellViewModel));

        public ICommand SettingsCommand
        {
            get => (ICommand)GetValue(SettingsCommandProperty);
            set => SetValue(SettingsCommandProperty, value);
        }

        private static async Task OpenSettings()
        {
            // TODO Open this IN flyout?
            Shell.FlyoutIsPresented = false;

            var settingsView = new SettingsView() { ViewModel = new SettingsViewModel() };
            await PushPage(settingsView).ConfigureAwait(true);
        }
        #endregion

        #region Utilities
        private static void OpenWeb(string webAddress)
        {
            Shell.FlyoutIsPresented = false;

            Browser.OpenAsync(new Uri(webAddress));
        }
        #endregion
    }
}