using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;

namespace RCS.PortableShop.Model
{
    public static class Settings
    {
        public enum ServiceType
        {
            WCF,
            WebApi
        }

        public static List<ServiceType> ServiceTypes { get; } = Enum.GetValues(typeof(ServiceType)).Cast<ServiceType>().ToList();

        const string serviceTypeKey = "ServiceType";

        private static ServiceType? serviceTypeSelected;

        // Not entirely happy using Xamarin.Essentials here.
        // But need to initialize here for service calls not to fail, 
        // and do not want to ignore an already stored value.
        public static ServiceType ServiceTypeSelected
        {
            get
            {
                if (!serviceTypeSelected.HasValue)
                    serviceTypeSelected = (ServiceType)Preferences.Get(serviceTypeKey, (int)ServiceType.WebApi);

                return serviceTypeSelected.Value;
            }
            set
            {
                serviceTypeSelected = value;
                Preferences.Set(serviceTypeKey, (int)value);
            }
        }
    }
}
