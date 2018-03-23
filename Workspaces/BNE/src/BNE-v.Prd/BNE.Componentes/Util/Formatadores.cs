using System;
using System.Text.RegularExpressions;

namespace BNE.Componentes.Util
{
    /// <summary>
    /// Formatadores genéricos
    /// </summary>
    public static class Formatadores
    {

        #region FormatarCNPJ
        /// <summary>
        /// Formata o número de CNPJ
        /// </summary>
        /// <param name="value">O número de CNPJ</param>
        /// <returns>O CNPJ em formato de string formatada</returns>
        public static String FormatarCNPJ(string value)
        {
            if (String.IsNullOrEmpty(value))
                return "";
            var r = new Regex("[^0-9]");
            Decimal val = Convert.ToDecimal(r.Replace(value, String.Empty));
            return FormatarCNPJ(val);
        }
        #endregion

        #region FormatarCNPJ
        /// <summary>
        /// Formata o número de CNPJ
        /// </summary>
        /// <param name="value">O número de CNPJ</param>
        /// <returns>O CNPJ em formato de string formatada</returns>
        public static String FormatarCNPJ(decimal value)
        {
            //if (value == Decimal.Zero)
            //    return "";
            return String.Format("{0:00000000000000}", value).Insert(2, ".").Insert(6, ".").Insert(10, "/").Insert(15, "-");
        }
        #endregion
        /*
        #region FormatarCPF
        public static String FormatarCPFSemPonto(decimal value)
        {
            return String.Format("{0:00000000000}", value);
        }

        /// <summary>
        /// Formata o número de CPF
        /// </summary>
        /// <param name="value">O número de CPF</param>
        /// <returns>O CPF em formato de string formatada</returns>
        public static String FormatarCPF(decimal value)
        {
            return String.Format("{0:00000000000}", value).Insert(3, ".").Insert(7, ".").Insert(11, "-");
        }
        #endregion

        #region FormatarCPF
        /// <summary>
        /// Formata o número de CPF
        /// </summary>
        /// <param name="value">O número de CPF</param>
        /// <returns>O CPF em formato de string formatada</returns>
        public static String FormatarCPF(string value)
        {
            Regex r = new Regex("[^0-9]");
            Decimal val = Convert.ToDecimal(r.Replace(value, String.Empty));
            return FormatarCPF(val);
        }
        #endregion

        #region FormatarCEI
        /// <summary>
        /// Formata o número de CEI
        /// </summary>
        /// <param name="value">O número de CEI</param>
        /// <returns>O CEI em formato de string formatada</returns>
        public static String FormatarCEI(String value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            Regex r = new Regex("[^0-9]");
            Decimal val = Convert.ToDecimal(r.Replace(value, String.Empty));
            return FormatarCEI(val);
        }
        #endregion

        #region FormatarCEI
        /// <summary>
        /// Formata o número de CEI
        /// </summary>
        /// <param name="value">O número de CEI</param>
        /// <returns>O CEI em formato de string formatada</returns>
        public static String FormatarCEI(decimal value)
        {
            return String.Format("{0:000000000000}", value).Insert(3, ".").Insert(7, ".").Insert(12, "/");
        }
        #endregion

        #region FormatarTelefone
        public static String FormatarTelefone(String ddd, String telefone, Boolean celular = false)
        {
            if (String.IsNullOrEmpty(ddd) && String.IsNullOrEmpty(telefone))
                return String.Empty;

            if ("11".Equals(ddd) && celular && !String.IsNullOrEmpty(telefone))
            {
                if (!telefone.StartsWith("9") || telefone.Length != 9)
                    return String.Format("({0}) 9{1}-{2}",
                ddd,
                telefone.SubstringEmpty(0, 4),
                telefone.SubstringEmpty(4));
                else
                    return String.Format("({0}) {1}-{2}",
               ddd,
               telefone.SubstringEmpty(0, 5),
               telefone.SubstringEmpty(5));

            }

            return String.Format("({0}) {1}-{2}",
                ddd,
                telefone.SubstringEmpty(0, 4),
                telefone.SubstringEmpty(4));
        }
        #endregion

        #region FormatarTelefone
        public static String FormatarTelefone(String numero)
        {
            return FormatarTelefone(
                Formatadores.SomenteNumeros(numero).SubstringEmpty(0, 2),
                Formatadores.SomenteNumeros(numero).SubstringEmpty(2));
        }
        #endregion
        */
        #region SomenteNumeros
        /// <summary>
        /// Deixa somente números em uma String
        /// </summary>
        /// <param name="valor">A string a ser processada</param>
        /// <returns>Uma string contendo apenas números</returns>
        public static String SomenteNumeros(String valor)
        {
            var r = new Regex("[^0-9]");
            return r.Replace(valor, String.Empty);
        }
        #endregion
        /*
        #region FormatarCEP        
        public static String FormatarCEP(String cep)
        {
            String val = Formatadores.SomenteNumeros(cep);
            if (val.Length != 8)
                return "";

            return val.Substring(0, 2) + "." + val.Substring(2, 3) + "-" + val.Substring(5, 3);
        }
        #endregion
         * */
    }
}
