using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BNE.BLL.Custom
{
    public class Validacao
    {

        #region ValidarFormatoCPF
        /// <summary>
        /// Validar o formato do valor desejado
        /// </summary>
        /// <param name="valor">Valor desejado a validar</param>
        /// <returns>'True' se valor é válido, caso contrário 'False'</returns>
        private static bool ValidarFormatoCPF(string valor)
        {
            Int64 vlr;
            if (valor.Length.Equals(11) && Int64.TryParse(valor, out vlr))
            {
                for (int i = 1; i < 11; i++)
                {
                    if (!valor[0].Equals(valor[i]))
                        return true;
                }
                return false;
            }
            return false;
        }
        #endregion

        #region ValidarCalculoCPF
        /// <summary>
        /// Verifica se o CPF informado é válido
        /// </summary>
        /// <param name="valor">CPF para validação</param>
        /// <returns>Retorna true caso o CPF seja válido</returns>
        private static bool ValidarCalculoCPF(string valor)
        {
            int soma1 = 0;
            int soma2 = 0;
            for (int i = 0; i < 9; i++)
            {
                String dig_cpf = valor.Substring(i, 1);
                soma1 += int.Parse(dig_cpf) * (10 - i);
                soma2 += int.Parse(dig_cpf) * (11 - i);
            }
            int dv1 = (11 - (soma1 % 11));
            dv1 = dv1 >= 10 ? 0 : dv1;
            soma2 = soma2 + (dv1 * 2);
            int dv2 = (11 - (soma2 % 11));
            dv2 = dv2 >= 10 ? 0 : dv2;
            string dv = valor.Substring(9, 2);
            return dv.Equals(dv1.ToString() + dv2.ToString());
        }
        #endregion

        #region ValidarCPF
        public static bool ValidarCPF(string cpf)
        {
            cpf = LimparMascaraCPF(cpf);

            if (!ValidarFormatoCPF(cpf))
                return false;

            if (!ValidarCalculoCPF(cpf))
                return false;

            return true;
        }
        #endregion

        #region LimparMascaraCPF
        /// <summary>
        /// Limpar os caracteres especiais do Cnpj
        /// </summary>
        /// <returns>Cnpj limpo</returns>
        public static string LimparMascaraCPF(string cpf)
        {
            cpf = cpf.Trim();
            if (System.Text.RegularExpressions.Regex.IsMatch(cpf, RegexCPF))
            {
                string sPadrao = @"[.\\/-]";
                System.Text.RegularExpressions.Regex oRegEx = new System.Text.RegularExpressions.Regex(sPadrao);
                return oRegEx.Replace(cpf, "");
            }
            return cpf;
        }
        #endregion

        #region ValidarFormatoData
        /// <summary>
        /// Validar o formato do valor desejado
        /// </summary>
        /// <param name="data">Valor desejado a validar</param>
        /// <returns>'True' se valor é válido, caso contrário 'False'</returns>
        public static bool ValidarFormatoData(string data)
        {
            data = data.Trim();
            DateTime dt;
            if (DateTime.TryParse(data, new CultureInfo("pt-BR"), DateTimeStyles.None, out dt))
                return true;
            return false;
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

        #region RegexTelefone
        public static readonly string RegexTelefone = @"^\d{4,5}-\d{4}$";
        #endregion

        #region RegexEmail
        public static readonly string RegexEmail = @"^(\w+([-+.&apos;]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)$";
        #endregion

        #region RegexCPF
        public static readonly string RegexCPF = @"^\d{3}.\d{3}.\d{3}-\d{2}$";
        #endregion
    }
}
