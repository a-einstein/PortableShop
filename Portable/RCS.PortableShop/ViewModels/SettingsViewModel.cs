using RCS.PortableShop.Common.ViewModels;
using RCS.PortableShop.Model;
using RCS.PortableShop.Resources;
using System.Collections.Generic;

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

        public IList<Culture> Cultures => Settings.Cultures;

        // Note no BindableProperty because Settings is the underlying datastructure.
        public Culture Culture
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
