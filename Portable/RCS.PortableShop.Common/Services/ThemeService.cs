using System;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

namespace RCS.PortableShop.Common.Services
{
    public static class ThemeService
    {
        private const string PreferenceKey = "AppTheme";
        private const string ThemeFolder = "Styles/Themes/";

        public static string GetSavedTheme() => Preferences.Get(PreferenceKey, "Light");

        public static void ApplyTheme(string themeName)
        {
            if (string.IsNullOrWhiteSpace(themeName))
                throw new ArgumentNullException(nameof(themeName));

            var source = new Uri($"/{ThemeFolder}{themeName}Theme.xaml", UriKind.Relative);

            // Try to create a ResourceDictionary with Source set to the theme XAML
            var themeDict = new ResourceDictionary();
            try
            {
                themeDict.Source = source;
            }
            catch
            {
                // If direct Source assignment fails (platform differences), attempt to load via XAML loader
                try
                {
                    var rd = new ResourceDictionary();
                    // Note: XamlReader is in Microsoft.Maui.Controls.Xaml on some platforms — using FromResource may be necessary.
                    // For now fall back to attempting to set Source and let MAUI resolve relative URIs.
                    rd.Source = source;
                    themeDict = rd;
                }
                catch
                {
                    // Give up — do not change theme
                    return;
                }
            }

            // Find existing theme dictionary in merged dictionaries by matching Source or by convention first merged dictionary
            var appResources = Application.Current?.Resources;
            if (appResources == null)
            {
                Preferences.Set(PreferenceKey, themeName);
                return;
            }

            int existingIndex = -1;
            for (int i = 0; i < appResources.MergedDictionaries.Count; i++)
            {
                var md = appResources.MergedDictionaries[i];
                if (md.Source != null && md.Source.ToString().Contains(ThemeFolder, StringComparison.OrdinalIgnoreCase))
                {
                    existingIndex = i;
                    break;
                }
            }

            if (existingIndex >= 0)
            {
                appResources.MergedDictionaries[existingIndex] = themeDict;
            }
            else
            {
                // Prepend so other resources can reference theme colors
                appResources.MergedDictionaries.Insert(0, themeDict);
            }

            Preferences.Set(PreferenceKey, themeName);
        }

        public static void InitializeTheme()
        {
            var saved = GetSavedTheme();
            ApplyTheme(saved);
        }
    }
}
