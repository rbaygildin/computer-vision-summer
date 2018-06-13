using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GrayScaleImage
{
    public struct Pixel
    {
        public byte R{get;set;}
        public byte G{get;set;}
        public byte B{get;set;}
        public byte Alpha{get;set;}
        public static Pixel operator -(Pixel a, Pixel b)
        {
            return new Pixel()
            {
                R = (byte)Math.Abs(a.R - b.R),
                G = (byte)Math.Abs(a.G - b.G),
                B = (byte)Math.Abs(a.B - b.B),
                Alpha = (byte)Math.Abs(a.Alpha - b.Alpha)
            };
        }
    }
    // class for Images conversion to byte[], BitmapImage and BitmapSource
    class ImageConvertor
    {

        // converts filename to BitmapImage
        public static BitmapImage FilenameToImage(string filename)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(filename, UriKind.RelativeOrAbsolute);
            image.EndInit();
            return image;
        }

        // converts only JPG image to byte[]
        public static byte[] ImageToByteArray(string filename)
        {
            JpegBitmapDecoder myImage = new JpegBitmapDecoder(new Uri(filename, UriKind.RelativeOrAbsolute), BitmapCreateOptions.DelayCreation, BitmapCacheOption.OnLoad);
            byte[] myImageBytes = new byte[myImage.Frames[0].PixelWidth * 4 * myImage.Frames[0].PixelHeight];
            myImage.Frames[0].CopyPixels(myImageBytes, myImage.Frames[0].PixelWidth * 4, 0);
            return myImageBytes;
        }

        // converts byte[] to BitmapSource
        public static BitmapSource ByteArrayToImage(byte[] data, int w, int h, int ch)
        {
            PixelFormat format = PixelFormats.Default;

            if (ch == 1) 
                format = PixelFormats.Gray8; //grey scale image 0-255
            if (ch == 3) 
                format = PixelFormats.Bgr24; //RGB
            if (ch == 4) 
                format = PixelFormats.Bgr32; //RGB + alpha

            WriteableBitmap wbm = new WriteableBitmap(w, h, 96, 96, format, null);
            wbm.WritePixels(new Int32Rect(0, 0, w, h), data, ch * w, 0);

            return wbm;
        }
        public static Pixel[,] GetPixelArrayFromImage(byte[] imageBytes, int xsize, int ysize)
        {
            Pixel[,] pixels = new Pixel[ysize, xsize];
            int x = -1;
            int y = -1;
            for (int i = 0; i < imageBytes.Length; i += 4)
            {
                if (i % (xsize * 4) == 0)
                    y++;
                x = ++x % xsize;
                pixels[y, x] = new Pixel()
                {
                    B = imageBytes[i],
                    G = imageBytes[i + 1],
                    R = imageBytes[i + 2],
                    Alpha = imageBytes[i + 3]
                };     
            }
            return pixels;
        }
    }
}
