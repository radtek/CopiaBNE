using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace BNE.Core.Common
{
    public class Utils
    {

        #region FormatarExibicaoSalario
        /// <summary>
        /// Formatar exibição do salário
        /// </summary>
        /// <param name="valorSalarioDe"></param>
        /// <param name="valorSalarioAte"></param>
        /// <returns></returns>
        public static string FormatarExibicaoSalario(decimal? valorSalarioDe, decimal? valorSalarioAte)
        {
            if (valorSalarioDe == 0 && valorSalarioAte == 0)
                return "Salário a combinar";

            if (valorSalarioDe == 0 || valorSalarioAte == 0)
                return string.Format("Salário de R$ {0}", (valorSalarioDe != 0 ? ((decimal)valorSalarioDe).ToString("N2") : ((decimal)valorSalarioAte).ToString("N2")));

            if (valorSalarioDe.Equals(valorSalarioAte))
                return string.Format("Salário de R$ {0}", ((decimal)valorSalarioDe).ToString("N2"));

            return string.Format(CultureInfo.CurrentCulture, "Salário de R$ {0} a R$ {1}", ((decimal)valorSalarioDe).ToString("N2"), ((decimal)valorSalarioAte).ToString("N2"));
        }
        #endregion

        #region LimparMascaraCPFCNPJCEP
        /// <summary>
        /// Limpar os caracteres especiais do CPF ou CNPJ
        /// </summary>
        /// <returns>CPF/CNPJ limpo</returns>
        public static string LimparMascaraCPFCNPJCEP(string inputString)
        {
            if (string.IsNullOrWhiteSpace(inputString))
                return string.Empty;

            inputString = inputString.Trim();
            return new Regex(@"[^0-9]").Replace(inputString, string.Empty);
        }
        #endregion

        #region LimparMascaraTelefone
        /// <summary>
        /// Limpar os caracteres especiais do telefone
        /// </summary>
        /// <returns>CPF/CNPJ limpo</returns>
        public static string LimparMascaraTelefone(string inputString)
        {
            if (string.IsNullOrWhiteSpace(inputString))
                return string.Empty;

            inputString = inputString.Trim();

            return new string(inputString.Where(char.IsDigit).ToArray());
        }
        #endregion

        #region FormatarCidade
        public static string FormatarCidade(string nomeCidade, string siglaEstado)
        {
            if (string.IsNullOrEmpty(nomeCidade))
                return string.Empty;

            if (string.IsNullOrEmpty(siglaEstado))
                return nomeCidade;

            return string.Format("{0}/{1}", nomeCidade, siglaEstado);
        }
        #endregion

        #region FormataTelefone
        public static string FormataTelefone(string telefone)
        {
            telefone = telefone.Insert(0, "(").Insert(3, ")").Insert(8, "-");
            return telefone;
        }
        #endregion

        #region ToJSON
        /// <summary>
        /// Convert a coleção para JSON
        /// </summary>
        /// <returns>A string xml que representa a coleção</returns>
        public static String ToJSON(object anObject)
        {
            String res = JsonConvert.SerializeObject(anObject, new IsoDateTimeConverter());
            if (String.IsNullOrEmpty(res))
                return String.Empty;

            return res;
        }
        #endregion

        #region ToBase64
        /// <summary>
        /// Convert a string para base64
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string ToBase64(string texto)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(texto);

            return Convert.ToBase64String(bytes);
        }
        #endregion

        #region FromBase64
        /// <summary>
        /// Convert a string para base64
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string FromBase64(string texto)
        {
            byte[] bytes = Convert.FromBase64String(texto);

            return Encoding.ASCII.GetString(bytes);
        }
        #endregion

        #region RecuperarIP
        public static string RecuperarIP(out string ipparalogar)
        {
            ipparalogar = string.Empty;

            var httpContext = HttpContext.Current;

            string ip = string.Empty;

            if (httpContext != null)
            {
                ip = httpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                ipparalogar = string.Format("{0} {1}", "HTTP_X_FORWARDED_FOR", ip);

                if (string.IsNullOrEmpty(ip))
                {
                    ip = httpContext.Request.ServerVariables["REMOTE_ADDR"];
                    ipparalogar = string.Format("{0} {1}", "REMOTE_ADDR", ip);
                }
            }

            if (!string.IsNullOrWhiteSpace(ip))
            {
                string[] addresses = ip.Split(',');
                if (addresses.Length != 0)
                {
                    if (addresses.Length > 2) //Se
                    {
                        ip = addresses[addresses.Length - 1];
                    }
                    if (ip != "::1")
                    {
                        string[] address = ip.Split(':');
                        ip = address[0];
                    }
                }
            }

            if (ip != null)
                return ip.Trim();

            return string.Empty;
        }
        #endregion

        #region RetornarPrimeiroNome
        /// <summary>
        /// Método utilizado para retornar o primeiro nome da pessoa.
        /// </summary>
        /// <param name="nome">String nome completo</param>
        /// <returns>Primeiro nome</returns>
        public static string RetornarPrimeiroNome(string nome)
        {
            if (nome.IndexOf(' ') != -1)
                return nome.Substring(0, nome.IndexOf(' '));

            return nome;
        }
        #endregion

        #region ValidarEmail
        /// <summary>
        /// Validar o e-mail
        /// </summary>
        /// <param name="email">Valor desejado a validar</param>
        /// <returns>'True' se valor é válido, caso contrário 'False'</returns>
        public static bool ValidarEmail(string email)
        {
            Regex objRegex = new Regex(RegexEmail);

            foreach (String e in email.Trim().Split(';'))
            {
                if (!objRegex.IsMatch(e.Trim()))
                    return false;
            }

            return true;
        }
        #endregion

        #region RegexEmail
        public static readonly string RegexEmail = @"^(\w+([-+.&apos;]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)$";
        #endregion

    }
}
