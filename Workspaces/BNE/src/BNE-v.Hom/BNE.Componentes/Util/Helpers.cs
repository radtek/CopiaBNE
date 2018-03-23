using System.Drawing;
using System.IO;

namespace BNE.Componentes.Util
{
    public class Helpers
    {
        public static byte[] ImageToByte(Image img)
        {
            var converter = new ImageConverter();
            return (byte[]) converter.ConvertTo(img, typeof (byte[]));
        }
        public static Image ByteToImage(byte[] ba)
        {
            using (var ms = new MemoryStream(ba))
            {
                return Image.FromStream(ms);
            }
        }
    }
}