using System;
using Newtonsoft.Json;

namespace BNE.BLL.Custom
{
    public class LoginAutomatico
    {

        #region GerarHashAcessoLogin
        //public static string GerarHashAcessoLogin(PessoaFisica objPessoaFisica)
        //{
        //    return GerarHashAcessoLoginAutomatico(objPessoaFisica.CPF, objPessoaFisica.DataNascimento, string.Empty);
        //}
        #endregion

        #region GerarHashAcessoLogin
        public static string GerarHashAcessoLogin(PessoaFisica objPessoaFisica, string url = null)
        {
            return GerarHashAcessoLoginAutomatico(objPessoaFisica.CPF, objPessoaFisica.DataNascimento, url);
        }
        #endregion

        #region RecuperarInformacaoHash
        public static LoginAutomatico RecuperarInformacaoHash(string hashAcesso)
        {
            if (string.IsNullOrEmpty(hashAcesso))
                return null;

            var json = Helper.FromBase64(hashAcesso);

            var loginAutomatico = JsonConvert.DeserializeObject<LoginAutomatico>(json);

            return loginAutomatico;
        }
        #endregion

        #region GerarHashAcessoLoginAutomatico
        public static string GerarHashAcessoLoginAutomatico(decimal numeroCPF, DateTime dataNascimento, string urlDestino)
        {
            if (urlDestino == null)
                urlDestino = string.Empty;

            var parametros = new LoginAutomatico
            {
                NumeroCPF = numeroCPF,
                DataNascimento = dataNascimento,
                Url = urlDestino,

            };
            string json = Helper.ToJSON(parametros);
            return Helper.ToBase64(json);
        }
        #endregion

        #region GerarHashAcessoLoginAutomatico
        public static string GerarHashAcessoLogin(PessoaFisica objPessoaFisica, int idVaga)
        {
            var parametros = new LoginAutomatico
            {
                NumeroCPF = objPessoaFisica.CPF,
                DataNascimento = objPessoaFisica.DataNascimento,
                IdVaga = idVaga
            };
            string json = Helper.ToJSON(parametros);
            return Helper.ToBase64(json);
        }
        #endregion

        public int? IdPessoFisica { get; set; }
        public int? IdVaga { get; set; }
        public string Url { get; set; }
        public decimal NumeroCPF { get; set; }
        public DateTime DataNascimento { get; set; }

    }
}
