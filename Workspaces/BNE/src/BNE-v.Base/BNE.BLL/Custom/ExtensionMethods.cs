using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace BNE.BLL.Custom
{
    public static class ExtensionMethods
    {
        readonly static Dictionary<string, string> invalidCaracteres = new Dictionary<string, string>()
        {
            {" ", "-"},
            {"c#", "(csharp)"},
            {"#", "(rt)"},
            {"&", "(and)"},
            {"\\", "-"},
            {".", "(dot)"},
            {":", "(ddot)"},
            {"<", "(lt)"},
            {">", "(gt)"},
            {"?", "(qm)"},
            {"@", "(at)"},
            {"%", "(perc)"},
            {"+", "(plus)"},
            {";", "(scolon)"},
            {"=", "(eq)"},
            {"$", "(dlr)"},
            {",", "(colon)"},
            {"~", "(til)"},
            {"^", "(circ)"},
            {"`", "(crase)"},
            {"[", "(aconch)"},
            {"]", "(fconch)"},
            {"{", "(achave)"},
            {"}", "(fchave)"},
            {"|", "(or)"},
            {"\"", "(dquote)"},
            {"'", "(quote)"},
        };

        #region Partition
        public static List<DataTable> Partition(this DataTable dataTable, double partitionSize)
        {
            var numRows = Math.Ceiling((double)dataTable.Rows.Count);
            var dataTablesCount = Convert.ToInt32(numRows / partitionSize);
            var listDataTable = new List<DataTable>(dataTablesCount);
            var partition = Convert.ToInt32(Math.Round(partitionSize, MidpointRounding.ToEven));

            for (var i = 0; i < dataTablesCount; i++)
            {
                listDataTable.Add(dataTable.Clone());

                var listDataRow = Partition(dataTable, i * partition, i * partition + partition);

                foreach (DataRow dr in listDataRow)
                {
                    listDataTable[i].ImportRow(dr);
                }
            }

            return listDataTable;
        }
        private static List<DataRow> Partition(DataTable dataTable, int index, int endIndex)
        {
            var listDataRow = new List<DataRow>();
            for (var i = index; i < endIndex && i < dataTable.Rows.Count; i++)
            {
                listDataRow.Add(dataTable.Rows[i]);
            }
            return listDataRow;
        }
        #endregion

        #region NormalizarURL
        /// <summary>
        /// Extension method de string para padronizar as palavaras de URL
        /// </summary>
        /// <param name="s">Texto que será manipulado</param>
        /// <returns></returns>
        public static string NormalizarURL(this string s)
        {
            s = Helper.RemoverAcentos(s);

            var sb = new StringBuilder(s.ToLower());

            foreach (var item in invalidCaracteres)
            {
                sb.Replace(item.Key, item.Value);
            }

            return sb.ToString();
        }
        #endregion

        #region DesnormalizarURL
        /// <summary>
        /// Extension method de string para padronizar as palavaras de URL
        /// </summary>
        /// <param name="s">Texto que será manipulado</param>
        /// <returns></returns>
        public static string DesnormalizarURL(this string s)
        {
            foreach (var item in invalidCaracteres)
            {
                s = s.Replace(item.Value, item.Key);
            }

            return s;
        }
        #endregion

        #region Truncate
        public static string Truncate(this string str, int length)
        {
            if (string.IsNullOrWhiteSpace(str))
                return string.Empty;

            str = str.Trim();

            if (str.Length <= length)
                return str;

            return string.Format("{0}{1}", str.Substring(0, length), "...");
        }
        #endregion

        #region ReplaceBreaks - Extensionmethod - Replace Quebras por <br/>
        public static string ReplaceBreaks(this string texto)
        {
            return texto.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");
        }
        #endregion

        #region UndoReplaceBreaks
        public static string UndoReplaceBreaks(this string texto)
        {
            return texto.Replace("<br/>", "\r\n");
        }
        #endregion

        #region ToExpando
        public static ExpandoObject ToExpando(this object anonymousObject)
        {
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(anonymousObject))
            {
                var obj = propertyDescriptor.GetValue(anonymousObject);
                expando.Add(propertyDescriptor.Name, obj);
            }

            return (ExpandoObject)expando;
        }
        #endregion

        #region HighlightText
        /// <summary>
        /// Adiciona um span com a class definida levando em conta a keyword a ser encontrada.
        /// </summary>
        /// <param name="text">O texto</param>
        /// <param name="keyword">A palavra-chave a ser pesquisada no texto</param>
        /// <param name="cssClass">Classe que vai conter a formatação highlight</param>
        /// <returns></returns>
        public static string HighlightText(this string text, string keyword, string cssClass)
        {
            var keywords = keyword.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            return keywords.Select(kw => "\\b" + kw.Trim() + "\\b").Aggregate(text, (current, pattern) => Regex.Replace(Helper.RemoverAcentos(current), Helper.RemoverAcentos(pattern), string.Format("<span class=\"{0}\">{1}</span>", cssClass, pattern.Replace("\\b", string.Empty)), RegexOptions.IgnoreCase | RegexOptions.CultureInvariant));
        }
        #endregion

    }
}
