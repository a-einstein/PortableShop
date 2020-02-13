using RCS.PortableShop.Common.ViewModels;
using RCS.PortableShop.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using static RCS.PortableShop.Model.ProductsServiceConsumer;

namespace RCS.PortableShop.ViewModels
{
    public class SettingsViewModel : ViewModel
    {
        public SettingsViewModel()
        {
            ServiceTypes = Enum.GetValues(typeof(ServiceType)).Cast<ServiceType>().ToList();
            ServiceType = (ServiceType)Preferences.Get(serviceTypeKey, 1);
        }

        public override string MakeTitle()
        {
            return Labels.Settings; ;
        }

        List<ServiceType> serviceTypes;
        public List<ServiceType> ServiceTypes
        {
            get => serviceTypes;
            private set
            {
                serviceTypes = value;

                OnPropertyChanged();
            }
        }

        const string serviceTypeKey = "ServiceType";
        ServiceType serviceType;
        public ServiceType ServiceType
        {
            get => serviceType;
            set
            {
                serviceType = value;
                Preferences.Set(serviceTypeKey, (int)value);

                OnPropertyChanged();
            }
        }
    }
}
