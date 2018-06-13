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
    }
}
