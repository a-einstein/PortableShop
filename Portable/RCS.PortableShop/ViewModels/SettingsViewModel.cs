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

        #region Settings 
        public List<Theme> Themes => Settings.Themes;

        // Note no BindableProperty because Settings is the underlying datastructure.
        public Theme Theme
        {
            get => Settings.Theme;
            set
            {
                Settings.Theme = value;
                OnPropertyChanged();
            }
        }

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

        public IList<CultureInfo> Cultures => Settings.Cultures;

        // Note no BindableProperty because Settings is the underlying datastructure.
        public CultureInfo Culture
        {
            get => Settings.Culture;
            set
            {
                Settings.Culture = value;
                OnPropertyChanged();
            }
        }
        #endregion
    }
}
