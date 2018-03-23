using AllInMail.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllInMail.Core.Model
{
    public class StarterDefaultSettings : IStarterSettings
    {
        public int BatchSize
        {
            get;
            set;
        }

        public int BufferSize
        {
            get;
            set;
        }

        public DateTime? FromDate
        {
            get;
            set;
        }

        public int MaxQuantity
        {
            get;
            set;
        }

        public int StartAboveTargetId
        {
            get;
            set;
        }

        public bool WriterHeader
        {
            get;
            set;
        }
    }
}
