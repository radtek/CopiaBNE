using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace BNE.Common
{
    public class Helper
    {

        #region RecuperarIP
        public static string RecuperarIP()
        {
            var httpContext = HttpContext.Current;

            string ip = string.Empty;

            if (httpContext != null)
            {
                ip = httpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (string.IsNullOrEmpty(ip))
                    ip = httpContext.Request.ServerVariables["REMOTE_ADDR"];
            }

            return ip;
        }
        #endregion

        #region AbreviarNome
        public static string AbreviarNome(string nomeCompleto)
        {
            var s = Regex.Replace(nomeCompleto.Trim(), " +", " ").Split(' ');
            var retorno = string.Empty;

            if (s.Length <= 0) return nomeCompleto;

            var i = 0;
            retorno = s[i++];

            for (; i < s.Length; i++)
            {
                if (s[0].Length <= 3)
                    continue;

                retorno += " " + s[i][0].ToString().ToUpper() + ".";
            }

            return retorno;
        }
        #endregion AbreviarNome

        #region AjustarString
        public static string AjustarString(string entrada)
        {
            //Remove espaços iniciais e finais.
            entrada = entrada.Trim();

            while (entrada.Contains("  "))
            {
                entrada = entrada.Replace("  ", " ");
            }

            //Coloca primeira letra para maiúscula.
            entrada = FormatarPrimeiraMaiscula(entrada);
            //removendo palavras auxiliares
            entrada = entrada.Replace(" De ", " de ");
            entrada = entrada.Replace(" Do ", " do ");
            entrada = entrada.Replace(" Dos ", " dos ");
            entrada = entrada.Replace(" Da ", " da ");
            entrada = entrada.Replace(" Para ", " para ");

            return entrada;
        }

        /// <summary>
        /// Método que formata a string para apenas a primeira letra maiúscula.
        /// </summary>
        /// <param name="entrada">Texto de entrada.</param>
        /// <returns>Texto formatado.</returns>
        private static string FormatarPrimeiraMaiscula(string entrada)
        {
            string[] palavras = entrada.Split(new[] { ' ' });

            entrada = palavras.Where(palavra => !palavra.Length.Equals(0)).Aggregate(string.Empty, (current, palavra) => current + (" " + palavra.Substring(0, 1).ToUpper() + palavra.Substring(1).ToLower()));

            if (entrada.Length.Equals(0))
                return string.Empty;

            return entrada.Substring(1);
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

        /// <summary>
        /// Extension for 'Object' that copies the properties to a destination object.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public static void CopyProperties(object source, object destination)
        {
            // If any this null throw an exception
            if (source == null || destination == null)
                throw new Exception("Source or/and Destination Objects are null");
            // Getting the Types of the objects
            Type typeDest = destination.GetType();
            Type typeSrc = source.GetType();

            // Iterate the Properties of the source instance and  
            // populate them from their desination counterparts  
            PropertyInfo[] srcProps = typeSrc.GetProperties();
            foreach (PropertyInfo srcProp in srcProps)
            {
                if (!srcProp.CanRead)
                {
                    continue;
                }
                PropertyInfo targetProperty = typeDest.GetProperty(srcProp.Name, BindingFlags.Instance);
                if (targetProperty == null)
                {
                    continue;
                }
                if (!targetProperty.CanWrite)
                {
                    continue;
                }
                if (targetProperty.GetSetMethod(true) != null && targetProperty.GetSetMethod(true).IsPrivate)
                {
                    continue;
                }
                if ((targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) != 0)
                {
                    continue;
                }
                if (!targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType))
                {
                    continue;
                }
                // Passed all tests, lets set the value
                targetProperty.SetValue(destination, srcProp.GetValue(source, null), null);
            }
        }
    }
}
