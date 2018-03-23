using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsJob.Domain
{
    public class Phrase
    {
        private static Random r = new Random();
        private static List<Model.Phrase> _phrases = null;

        public static List<Model.Phrase> Phrases
        {
            get
            {
                if (_phrases == null)
                {
                    using (var db = new Data.WhatsJobsContext())
                    {
                        _phrases = (from p in db.Phrase
                                    where p.Ativo
                                    select p).ToList();
                    }
                }
                return _phrases;
            }
        }

        public static Model.Phrase GetRandom()
        {
            return Phrases.ElementAt(r.Next(0, Phrases.Count()));
        }
    }
}
