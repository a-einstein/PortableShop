﻿using System;

namespace RCS.PortableShop.Localization
{
    public class PlatformCulture
    {
        public PlatformCulture(string platformCultureString)
        {
            if (string.IsNullOrEmpty(platformCultureString))
                throw new ArgumentException("Expected culture identifier", nameof(platformCultureString));

            PlatformString = platformCultureString.Replace("_", "-"); // .NET expects dash, not underscore
            var dashIndex = PlatformString.IndexOf("-", StringComparison.Ordinal);

            if (dashIndex > 0)
            {
                var parts = PlatformString.Split('-');
                LanguageCode = parts[0];
                LocaleCode = parts[1];
            }
            else
            {
                LanguageCode = PlatformString;
                LocaleCode = string.Empty;
            }
        }

        private string PlatformString { get; }

        public string LanguageCode { get; }

        private string LocaleCode { get; }

        public override string ToString()
        {
            return PlatformString;
        }
    }
}
