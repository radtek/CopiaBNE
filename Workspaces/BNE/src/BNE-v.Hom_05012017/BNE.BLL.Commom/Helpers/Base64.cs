using System;
using System.Text;

namespace BNE.BLL.Common.Helpers
{
    public class Base64
    {
        #region ToBase64
        /// <summary>
        ///     Convert a string para base64
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string ToBase64(string texto)
        {
            var bytes = Encoding.ASCII.GetBytes(texto);

            return Convert.ToBase64String(bytes);
        }
        #endregion
    }
}