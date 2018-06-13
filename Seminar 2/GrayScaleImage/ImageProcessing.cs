using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrayScaleImage
{
    class ImageProcessing
    {
        // converts image to gray-scale
        public byte[] setGrayscale(byte[] originalImage)
        {
            /* 
             * TODO: создать новый byte[], который будет создрежать градации серого
             * TODO: записать в него значения по формуле: x = 0.299R + 0.587G + 0.114B
             * TODO: вернуть это значение
             * note: в С# дефолтным сичтается формат Bgra
             */
            List<byte> new_image = new List<byte>();
            for (int i = 0; i < originalImage.Length; i += 4)
            {
                double sum = 0.299 * originalImage[i] + 0.587 * originalImage[i + 1] + 0.114 * originalImage[i + 2];
                new_image.Add((byte)sum);
            }
            return new_image.ToArray();
        }

        // inverts image
        public byte[] setInvert(byte[] originalImage)
        {
            int i = -1;
            return Array.ConvertAll<byte, byte>(originalImage, x =>
            {
                if (++i % 4 != 3)
                    return (byte)(255 - x);
                return x;
            });
        }

        // shows only red channel
        public byte[] setRedFilter(byte[] originalImage)
        {
            int i = -1;
            return Array.ConvertAll<byte, byte>(originalImage, x =>
            {
                if (++i % 4 != 2)
                    return 0;
                return x;
            });
        }
        public byte[] FindContours(byte[] originalImage, int xsize, int ysize)
        {
            byte[] preimage = setGrayscale(originalImage);
            Pixel[,] pixels = ImageConvertor.GetPixelArrayFromImage(preimage, xsize, ysize);
            Pixel[,] new_pixels = new Pixel[pixels.GetLength(0), pixels.GetLength(1)];
            for (int y = 0; y < pixels.GetLength(0) - 1; y++)
            {
                for (int x = 0; x < pixels.GetLength(1) - 1; x++)
                {
                    Pixel temp1 = pixels[y, x] - pixels[y + 1, x + 1];
                    Pixel temp2 = pixels[y, x + 1] - pixels[y + 1, x];
                    new_pixels[y, x] = new Pixel()
                    {
                        R = (byte)(Math.Sqrt(temp1.R * temp1.R + temp2.R * temp2.R)),
                        G = (byte)(Math.Sqrt(temp1.G * temp1.G + temp2.G * temp2.G)),
                        B = (byte)(Math.Sqrt(temp1.B * temp1.B + temp2.B * temp2.B)),
                        Alpha = (byte)(Math.Sqrt(temp1.Alpha * temp1.Alpha + temp2.Alpha * temp2.Alpha)),
                    };
                }
            }
            List<byte> result = new List<byte>();
            for(int y = 0; y < new_pixels.GetLength(0); y++)
                for (int x = 0; x < new_pixels.GetLength(1); x++)
                {
                    byte result_sum = (byte)((new_pixels[y, x].R + new_pixels[y, x].G + new_pixels[y, x].B + new_pixels[y, x].Alpha) / 4);
                    //result.Add(pixels[y, x].B);
                    //result.Add(pixels[y, x].G);
                    //result.Add(pixels[y, x].R);
                    //result.Add(pixels[y, x].Alpha);
                    result.Add(result_sum);
                    result.Add(result_sum);
                    result.Add(result_sum);
                    result.Add(result_sum);
                }
            return result.ToArray();
        }
    }
}
