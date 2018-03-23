using AllInTriggers.Base;
using AllInTriggers.Helper;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AllInTriggers.Base
{
    public abstract class AllInRequestBase
    {
        public abstract string BaseUrl { get; }
        public abstract string ResourceRequest { get; set; }
        public abstract IEnumerable<KeyValuePair<string, string>> UrlSegment { get; set; }
        public abstract Method Method { get; set; }
        public abstract string Execute();
        public abstract Task<string> ExecuteAsync();
    }
}
