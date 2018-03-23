using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WhatsJob.Domain
{
    public class StopWord
    {
        private static List<Regex> _stopWords = null;

        public static List<Regex> StopWords
        {
            get
            {
                if (_stopWords == null)
                {
                    List<Regex> lst = null;
                    using (var db = new Data.WhatsJobsContext())
                    {
                        var sws = from sw in db.StopWord
                                  where sw.Ativo
                                  select sw;
                        lst = new List<Regex>();
                        foreach (var sw in sws)
                        {
                            lst.Add(Util.GetAccentInsensitiveRegex(@"\b" + sw.Word + @"\b"));
                        }
                    }
                    _stopWords = lst;
                }
                return _stopWords;
            }
        }

        public static string RemoveStopWords(string text){
            foreach (var sw in StopWords)
            {
                text = sw.Replace(text, "");
            }
            return text;
        }
    }
}
