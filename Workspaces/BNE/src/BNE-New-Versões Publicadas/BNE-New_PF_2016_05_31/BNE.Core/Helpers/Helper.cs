using System;
using System.Text;

namespace BNE.Core.Helpers
{
    public class Helper
    {

        #region ToBase64
        /// <summary>
        /// Convert a string to base64
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        [Obsolete("Usar o método que está no Common > Utils")]
        public static string ToBase64(string texto)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(texto);
            return Convert.ToBase64String(bytes);
        }
        #endregion

    }
}
