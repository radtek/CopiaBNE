using System;
using System.Collections.ObjectModel;

namespace BNE.Componentes.Extensions
{
    /// <summary>
    /// Extensões para String
    /// </summary>
    public static class StringExtensions
    {
        #region Minify
        /// <summary>
        /// Minimiza um texto excluindo espaços duplos, enters, tabs e finais de linha. Usado para otmização de Javascript.
        /// Obervações:
        /// Esse método ainda não trata comentários no javascript, logo, podem haver erros na hora da 
        /// otimização. Se for necessário usar comentários no código javascript, use o modelo de comentários "/* */"
        /// </summary>
        /// <param name="value">A instância a ser extendida</param>
        /// <returns>O texto minimizado</returns>
        public static String Minify(this String value)
        {
            String v = value.Replace("\n\r", String.Empty);
            v = v.Replace("\r", String.Empty);
            v = v.Replace("\n", String.Empty);
            v = v.Replace("\t", String.Empty);
            String oldv;
            do
            {
                oldv = (String)v.Clone();
                v = v.Replace("  ", " ");
            } while (!oldv.Equals(v));

            // Os caracteres [].-" foram excluídos devido a compatibilidade com o CSS
            const string special = "{}();,=+/><|&";
            foreach (char c in special)
            {
                v = v.Replace(String.Concat(c, " "), c.ToString());
                v = v.Replace(String.Concat(" ", c), c.ToString());
            }
            return v;
        }
        #endregion 

        #region ToJavascript
        /// <summary>
        /// Retorna o texto javasript minimizado contendo as tags script
        /// </summary>
        /// <param name="value">A instância a ser extendida</param>
        /// <returns>O texto javascript</returns>
        public static String ToJavascript(this String value)
        {
            return "<script type=\"text/javascript\">" + value.Minify() + "</script>";
        }
        #endregion 

        #region In
        /// <summary>
        /// Retorna true caso a string esteja dentro de uma das opções aceitas. 
        /// <remarks>
        /// As comparações são por padrão case insensitive
        /// </remarks>
        /// </summary>
        /// <param name="value">A instância a ser extendida</param>
        /// <param name="listOfOptions">A lista de opções aceitas</param>
        /// <returns>True se a validação foi bem sucedida</returns>
        public static Boolean In(this String value, String[] listOfOptions)
        {
            if (listOfOptions.Length == 0)
                throw new ArgumentOutOfRangeException("listOfOptions");

            foreach (String s in listOfOptions)
            {
                if (value.Equals(s, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }
        #endregion 

        #region ContainsAny
        /// <summary>
        /// Verifica se a String possúi qualquer uma das ocorrências parametrizadas
        /// <remarks>
        /// As comparações são case insensitive
        /// </remarks>
        /// </summary>
        /// <param name="value">A instância a ser extendida</param>
        /// <param name="listOfOptions">A lista de opções aceitas</param>
        /// <returns>True se foi encontrada alguma ocorrência</returns>
        public static Boolean ContainsAny(this String value, String[] listOfOptions)
        {
            foreach (String item in listOfOptions)
            {
                if (value.ToLowerInvariant().Contains(item.ToLowerInvariant()))
                    return true;
            }
            return false;
        }
        #endregion 

        #region ReplaceAll
        /// <summary>
        /// Substitui todas as ocorrências de oldPattern por newPattern em todos os ítens do array parametrizado
        /// </summary>
        /// <param name="value">A instância a ser extendida</param>
        /// <param name="oldPattern">O texto a ser substituído</param>
        /// <param name="newPattern">O novo texto</param>
        /// <returns>Um novo array de Strings contendo as substituições</returns>
        public static String[] ReplaceAll(this String[] value, String oldPattern, String newPattern)
        {
            var newArr = new String[value.Length];

            for (int i = 0; i < value.Length; i++)
            {
                newArr[i] = value[i].Replace(oldPattern, newPattern);
            }

            return newArr;
        }
        #endregion 

        #region Split
        /// <summary>
        /// Quebra uma string usando outra string como separador
        /// </summary>
        /// <param name="value">A instância a ser extendida</param>
        /// <param name="separator">A string a ser usada como separador</param>
        /// <returns>Um array dos fragmentos separados</returns>
        public static String[] Split(this String value, String separator)
        {
            var fragments = new Collection<string>();

            int index;

            while ((index = value.IndexOf(separator, StringComparison.OrdinalIgnoreCase)) > -1)
            {
                fragments.Add(value.Substring(0, index));
                value = value.Substring(index + separator.Length);
            }

            if (value.Length > 0)
                fragments.Add(value);

            if (fragments.Count == 0)
                fragments.Add(value);

            var newArray = new String[fragments.Count];

            fragments.CopyTo(newArray, 0);

            return newArray;
        }
        #endregion 

        #region SubstringEmpty
        public static String SubstringEmpty(this String value, int index, int count=0)
        {
            try
            {
                if (count != 0)
                    return value.Substring(index, count);

                return value.Substring(index);
            }
            catch
            {
                return String.Empty;
            }
        }
        #endregion 

    }
}
