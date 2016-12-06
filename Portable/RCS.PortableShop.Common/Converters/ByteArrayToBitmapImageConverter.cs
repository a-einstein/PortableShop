using System;
using System.Globalization;
using System.IO;
using System.Windows.Media.Imaging;

namespace RCS.WpfShop.Common.Converters
{
    public class ByteArrayToBitmapImageConverter : SingleDirectionConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value);
        }

        public static BitmapImage Convert(object value)
        {
            byte[] byteArray = value as byte[];

            if (byteArray != null)
            {
                MemoryStream memoryStream = new MemoryStream(byteArray);

                BitmapImage bitmapImage = new BitmapImage();

                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream; // Bijeffect: omzetting JPEG -> ARGB.
                bitmapImage.EndInit();

                return bitmapImage;
            }

            return null;
        }
    }
}
