using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Employer.Componentes.UI.Web.Extensions;

namespace Employer.Componentes.UI.Web.Util
{
    /// <summary>
    /// Formatadores genéricos
    /// </summary>
    #pragma warning disable 1591
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
            Regex r = new Regex("[^0-9]");
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

        static int[] _Ddds9Digitos = new int[] {
            12, 13, 14, 15, 16, 17, 18, 19, 11,
            21, 22, 24, 27, 28,
            //41, 42, 43, 44, 45, 46, 47, 48, 49, 51, 53, 54, 55, 
            61, 62, 63, 64, 65, 66, 67, 68, 69, //Até 31/2016
            31, 32, 33, 34, 35, 37, 38, 71, 73, 74, 75, 77, 79, //Novo até 31/2015
            81, 82, 83, 84, 85, 86, 87, 88, 89,
            91, 92, 93, 94, 95, 96, 97, 98, 99 };

        public static bool isDdds9Digitos(int iddd)
        {
            return _Ddds9Digitos.Contains(iddd);
        }

        public static String FormatarTelefone(String ddd, String telefone, Boolean celular = false)
        {
            int iddd = 0;
            if (!int.TryParse(ddd, out iddd)  && String.IsNullOrEmpty(telefone))
                return String.Empty;

            if (isDdds9Digitos(iddd) && celular && !String.IsNullOrEmpty(telefone))
            {
                if (telefone.Length != 9)
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

        #region SomenteNumeros
        /// <summary>
        /// Deixa somente números em uma String
        /// </summary>
        /// <param name="valor">A string a ser processada</param>
        /// <returns>Uma string contendo apenas números</returns>
        public static String SomenteNumeros(String valor)
        {
            Regex r = new Regex("[^0-9]");
            return r.Replace(valor, String.Empty);
        }
        #endregion

        #region FormatarCEP        
        public static String FormatarCEP(String cep)
        {
            String val = Formatadores.SomenteNumeros(cep);
            if (val.Length != 8)
                return "";

            return val.Substring(0, 2) + "." + val.Substring(2, 3) + "-" + val.Substring(5, 3);
        }
        #endregion
    }
}
