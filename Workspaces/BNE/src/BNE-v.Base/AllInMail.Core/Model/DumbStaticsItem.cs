using AllInMail.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllInMail.Core.Model
{
    public class DumbStaticsItem : IStaticsItem
    {
        public TimeSpan Average
        {
            get { return default(TimeSpan); }
        }

        public TimeSpan Max
        {
            get { return default(TimeSpan); }
        }

        public TimeSpan Min
        {
            get { return default(TimeSpan); }
        }

        public int TotalCount
        {
            get { return 0; }
        }

        public void Increment(TimeSpan timeSpan)
        {
        }
    }
}
