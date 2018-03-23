using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace BNE.Web.Code
{
    public class ImageController
    {

        #region ImageToByteArray
        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public byte[] ImageToByteArray(Image image)
        {
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream();
                image.Save(ms, ImageFormat.Jpeg);
            }
            finally
            {
                if (ms != null)
                    ms.Dispose();
            }
            return ms.ToArray();
        }
        #endregion

        #region ByteArrayToImage
        /// <summary>
        /// 
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        public static Image ByteArrayToImage(byte[] byteArray)
        {
            MemoryStream ms = null;
            Image image = null;
            try
            {
                ms = new MemoryStream(byteArray);
                image = Image.FromStream(ms);
            }
            finally
            {
                if (ms != null)
                    ms.Dispose();
                if (image != null)
                    image.Dispose();
            }
            return image;
        }
        #endregion

        #region ResizeImage
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Image ResizeImage(Image image, int width, int height)
        {
            int oWidth = image.Width;	// largura original
            int oHeight = image.Height;	// altura original

            if (oWidth > width || oHeight > height)
            {

                // redimensiona se necessario
                if (oWidth > oHeight)
                {
                    // imagem horizontal
                    height = (oHeight * width) / oWidth;
                }
                if (oHeight > height)
                {
                    // imagem vertical
                    width = (oWidth * height) / oHeight;
                }
            }

            // cria a copia da imagem
            Image imgThumb = image.GetThumbnailImage(width, height, null, new IntPtr(0));
            return imgThumb;
        }
        #endregion

    }
}
