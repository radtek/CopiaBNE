using System;
using Newtonsoft.Json;

namespace BNE.BLL.Custom
{
    public class LoginAutomatico
    {
        //TODO Quebrando um pouco a logica separando em dois arquivos. O outro está em um projeto separado. BNE.BLL.Common

        #region GerarHashAcessoLogin
        public static string GerarHashAcessoLogin(PessoaFisica objPessoaFisica, string url = null)
        {
            return Common.LoginAutomatico.GerarHashAcessoLogin(objPessoaFisica.CPF, objPessoaFisica.DataNascimento, url);
        }
        public static string GerarHashAcessoLogin(decimal numeroCPF, DateTime dataNascimento, string url = null)
        {
            return Common.LoginAutomatico.GerarHashAcessoLogin(numeroCPF, dataNascimento, url);
        }
        #endregion

        #region RecuperarInformacaoHash
        public static LoginAutomatico RecuperarInformacaoHash(string hashAcesso)
        {
            if (string.IsNullOrEmpty(hashAcesso))
                return null;

            var json = Helper.FromBase64(hashAcesso.Replace("_", "/"));

            var loginAutomatico = JsonConvert.DeserializeObject<LoginAutomatico>(json);

            return loginAutomatico;
        }
        #endregion

        public int? IdPessoFisica { get; set; }
        public int? IdVaga { get; set; }
        public string Url { get; set; }
        public decimal NumeroCPF { get; set; }
        public DateTime DataNascimento { get; set; }

    }
}
