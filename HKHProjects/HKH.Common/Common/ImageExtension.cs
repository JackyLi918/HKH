using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKH.Common
{
    public static class ImageExtension
    {
        public static string ToHexString(this Image image)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                var newImg = new Bitmap(image);
                var format = image.RawFormat;
                newImg.Save(stream, format);
                byte[] bytes = new byte[stream.Length];
                stream.Position = 0;
                stream.Read(bytes, 0, (int)stream.Length);

                return bytes.ToHexString();
            }
        }

        public static Image ToImage(this string hexString)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                var bytes = hexString.ToByteArray();
                stream.Write(bytes, 0, bytes.Length);
                return Image.FromStream(stream);
            }
        }
    }
}
