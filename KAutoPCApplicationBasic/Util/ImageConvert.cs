using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Data;
using System.Globalization;

namespace KAutoPCApplicationBasic
{
    public static class GetOriginalPoint
    {
        public static void SetPoint(System.Windows.Controls.Image image, int xInput, int yInput, out int xOutput, out int yOutput)
        {
            xOutput = (int)((xInput) * (image.Source.Width) / (image.ActualWidth));
            yOutput = (int)((yInput) * (image.Source.Height) / (image.ActualWidth));
        }
        public static void SetWidthHeight(System.Windows.Controls.Image image, int rectangleX,int rectangleY, out int xWidth, out int yHeight)
        {
            xWidth = (int)((rectangleX) * (image.Source.Width) / (image.ActualWidth));
            yHeight = (int)((rectangleY) * (image.Source.Height) / (image.ActualWidth));
        }
        public static void SaveBitMapImagetoFile(this BitmapImage image, string filePath)
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }
        public static void SaveBitmapSourceToJpg(BitmapSource image, string filename)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            using (var fileStream = new FileStream(filename, FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }

    }
    #region Image Convert
    //public class KImageConvert : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (value == null)
    //        {
    //            return new BitmapImage();
    //        }
    //        MemoryStream ms = new MemoryStream();
    //        ((System.Drawing.Bitmap)value).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
    //        BitmapImage image = new BitmapImage();
    //        image.BeginInit();
    //        ms.Seek(0, SeekOrigin.Begin);
    //        image.StreamSource = ms;
    //        image.EndInit();
    //        return image;

    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    } 
        #endregion
        //public static BitmapSource Convert(System.Drawing.Bitmap bitmap)
        //{
        //    var bitmapData = bitmap.LockBits(
        //        new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
        //        System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

        //    var bitmapSource = BitmapSource.Create(
        //        bitmapData.Width, bitmapData.Height,
        //        bitmap.HorizontalResolution, bitmap.VerticalResolution,
        //        PixelFormats.Bgr24, null,
        //        bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

        //    bitmap.UnlockBits(bitmapData);

        //    return bitmapSource;
        //}
        //public static BitmapImage ToWpfBitmap(this Bitmap bitmap)
        //{
        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        bitmap.Save(stream, ImageFormat.Bmp);

        //        stream.Position = 0;
        //        BitmapImage result = new BitmapImage();
        //        result.BeginInit();
        //        // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
        //        // Force the bitmap to load right now so we can dispose the stream.
        //        result.CacheOption = BitmapCacheOption.OnLoad;
        //        result.StreamSource = stream;
        //        result.EndInit();
        //        result.Freeze();
        //        return result;
        //    }

        //}
        //public static BitmapImage ConvertToBitMapImage(Bitmap src)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
        //    BitmapImage image = new BitmapImage();
        //    image.BeginInit();
        //    ms.Seek(0, SeekOrigin.Begin);
        //    image.StreamSource = ms;
        //    image.EndInit();
        //    return image;
        //}


    
}
