using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Faculti.Helpers
{
    public class Picture
    {
        /// <summary>
        /// Resizes the image to the specified width and height.
        /// </summary>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using var wrapMode = new ImageAttributes();
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            }

            return destImage;
        }

        public static BitmapImage ChooseFromFile(BitmapImage defImg)
        {
            BitmapImage pic = defImg;

            try
            {
                FileDialog fldlg = new OpenFileDialog
                {
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                    Filter = "Image File (*.jpg;*.bmp;*.gif;*.png)|*.jpg;*.bmp;*.gif;*.png"
                };


                if (fldlg.ShowDialog() == true)
                {
                    string imgPath = fldlg.FileName;
                    pic = new BitmapImage(new Uri(@$"{imgPath}", UriKind.Relative));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error browsing image!");
            }

            return pic;
        }

        public static BitmapImage ImageToBitmap(Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = ms;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        public static Bitmap BitmapToImage(BitmapImage? image)
        {
            // copy to byte array
            int stride = image.PixelWidth * 4;
            byte[] buffer = new byte[stride * image.PixelHeight];
            image.CopyPixels(buffer, stride, 0);

            // create bitmap
            Bitmap bitmap =
                new Bitmap(
                    image.PixelWidth,
                    image.PixelHeight,
                    PixelFormat.Format32bppArgb);

            // lock bitmap data
            BitmapData bitmapData =
               bitmap.LockBits(
                    new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.WriteOnly,
                    bitmap.PixelFormat);

            // copy byte array to bitmap data
            Marshal.Copy(
                buffer, 0, bitmapData.Scan0, buffer.Length);

            // unlock
            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }
    }
}
