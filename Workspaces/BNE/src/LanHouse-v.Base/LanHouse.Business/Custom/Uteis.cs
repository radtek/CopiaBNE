using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business.Custom
{
    public class Uteis
    {
        #region CalcularIdade
        /// <summary>
        /// Calcula uma idade a partir de daus datas
        /// </summary>
        /// <param name="dataNascimento"></param>
        /// <param name="dataAtual"></param>
        /// <returns></returns>
        public static int CalcularIdade(DateTime dataNascimento, DateTime dataAtual)
        {
            int idade = dataAtual.Year - dataNascimento.Year;
            if (dataAtual.Month < dataNascimento.Month || (dataAtual.Month == dataNascimento.Month && dataAtual.Day < dataNascimento.Day))
            {
                idade--;
            }

            return idade;
        }

        #endregion

        #region ReduzirImagem
        //public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        //{
        //    var ratioX = (double)maxWidth / image.Width;
        //    var ratioY = (double)maxHeight / image.Height;
        //    var ratio = Math.Min(ratioX, ratioY);

        //    var newWidth = (int)(image.Width * ratio);
        //    var newHeight = (int)(image.Height * ratio);

        //    var newImage = new Bitmap(newWidth, newHeight);
        //    Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
        //    return newImage;
        //}
        #endregion
    }
}
