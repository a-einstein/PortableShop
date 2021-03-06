﻿using RCS.PortableShop.Common.ViewModels;
using RCS.PortableShop.Model;
using RCS.PortableShop.Resources;
using System.Collections.Generic;

namespace RCS.PortableShop.ViewModels
{
    public class SettingsViewModel : ViewModel
    {
        public override string MakeTitle()
        {
            return Labels.Settings;
        }

        public List<Settings.ServiceType> ServiceTypes => Settings.ServiceTypes;

        public Settings.ServiceType ServiceType
        {
            get => Settings.ServiceTypeSelected;
            set
            {
                Settings.ServiceTypeSelected = value;
                OnPropertyChanged();
            }
        }
    }
}
