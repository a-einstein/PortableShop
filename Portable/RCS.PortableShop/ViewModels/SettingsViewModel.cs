using RCS.PortableShop.Common.Styles.Themes;
using RCS.PortableShop.Common.ViewModels;
using RCS.PortableShop.Model;
using RCS.PortableShop.Resources;

namespace RCS.PortableShop.ViewModels
{
    public class SettingsViewModel : ViewModel
    {
        #region Refresh
        public override string MakeTitle()
        {
            return Labels.Settings;
        }
        #endregion

        #region Theme
        public IList<ThemeInfo> ThemeInfos { get; } =
        [
            new ThemeInfo(Theme.Light, Labels.ThemeLight),
            new ThemeInfo(Theme.Dark, Labels.ThemeDark)
        ];

        // Note no BindableProperty because Settings is the underlying datastructure.
        public ThemeInfo ThemeInfo
        {
            get => ThemeInfos.FirstOrDefault(element => element.Theme == Settings.Theme);
            set
            {
                Settings.Theme = value.Theme;
                OnPropertyChanged();
            }
        }
        #endregion

        #region DataService
        public List<ServiceType> ServiceTypes => Settings.ServiceTypes;

        // Note no BindableProperty because Settings is the underlying datastructure.
        public ServiceType ServiceType
        {
            get => Settings.ServiceType;
            set
            {
                Settings.ServiceType = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Culture
        public IList<CultureInfo> CultureInfos => Settings.CultureInfos;

        // Note no BindableProperty because Settings is the underlying datastructure.
        public CultureInfo CultureInfo
        {
            get => Settings.CultureInfo;
            set
            {
                Settings.CultureInfo = value;
                OnPropertyChanged();
            }
        }
        #endregion
    }
}
