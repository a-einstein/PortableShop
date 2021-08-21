using System;
using System.Globalization;
//using System.IO;
//using Windows.UI.Xaml.Media.Imaging;

namespace RCS.PortableShop.Common.Converters
{
    public class ByteArrayToBitmapImageConverter : SingleDirectionConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value);
        }

        private static /*BitmapImage*/ object Convert(object value)
        {
            var byteArray = value as byte[];

            /*
            if (byteArray != null)
            {
                MemoryStream memoryStream = new MemoryStream(byteArray);

                BitmapImage bitmapImage = new BitmapImage();

                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream; // Bijeffect: omzetting JPEG -> ARGB.
                bitmapImage.EndInit();

                return bitmapImage;
            }
            */

            return null;
        }
    }
}
