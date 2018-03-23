using System.Globalization;
using System.Text;

namespace BNE.BLL.Common.Helpers
{
    public class Formatting
    {
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

        #region RemoverAcentos
        /// <summary>
        /// Método que remove os acentos de uma string
        /// </summary>
        /// <param name="texto">Texto a ser removido a acentuação</param>
        /// <returns>String sem acentos</returns>
        public static string RemoverAcentos(string texto)
        {
            string s = texto.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (char t in s)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(t);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(t);
                }
            }
            return sb.ToString();
        }
        #endregion

        #region RetornarPrimeiroNome
        /// <param name="nomeCompleto">String nome completo</param>
        /// <returns>Primeiro nome</returns>
        public static string RetornarPrimeiroNome(string nomeCompleto)
        {
            if (string.IsNullOrWhiteSpace(nomeCompleto))
                return string.Empty;

            if (nomeCompleto.IndexOf(' ') != -1)
                return nomeCompleto.Substring(0, nomeCompleto.IndexOf(' '));

            return nomeCompleto;
        }
        #endregion
    }
}