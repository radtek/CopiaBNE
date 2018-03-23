using System;

namespace BNE.BLL.Common
{
    public class LoginAutomatico
    {

        public static string GerarHashAcessoLogin(decimal numeroCPF, DateTime dataNascimento, string url = null)
        {
            return GerarHashAcessoLoginAutomatico(numeroCPF, dataNascimento, url);
        }

        #region GerarHashAcessoLoginAutomatico
        private static string GerarHashAcessoLoginAutomatico(decimal numeroCPF, DateTime dataNascimento, string urlDestino)
        {
            if (urlDestino == null)
                urlDestino = string.Empty;

            var parametros = new
            {
                NumeroCPF = numeroCPF.ToString(),
                DataNascimento = dataNascimento.ToString("yyyy-MM-dd"),
                Url = urlDestino,
            };
            string json = Helpers.JSON.ToJson(parametros);
            return Helpers.Base64.ToBase64(json).Replace("/", "_");
        }
        #endregion

        #region GerarUrl
        /// <summary>
        /// Gera a url final para login automático com um url de destino ou querystring
        /// </summary>
        /// <param name="numeroCPF"></param>
        /// <param name="dataNascimento"></param>
        /// <param name="url"></param>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static string GerarUrl(decimal numeroCPF, DateTime dataNascimento, string url = null, string queryString = null)
        {
            return String.Format("http://www.bne.com.br/logar/{0}{1}", GerarHashAcessoLogin(numeroCPF, dataNascimento, url), queryString);
        }
        #endregion

    }
}
