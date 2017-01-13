﻿using System;
using System.Globalization;
using System.IO;
using Xamarin.Forms;

namespace RCS.PortableShop.Common.Converters
{
    class ByteArrayToImageSourceConverter : SingleDirectionConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value);
        }

        public static ImageSource Convert(object value)
        {
            byte[] byteArray = value as byte[];

            if (byteArray != null)
            {
                var imageSource = ImageSource.FromStream(() => new MemoryStream(byteArray));
                return imageSource;
            }

            return null;
        }        
    }
}