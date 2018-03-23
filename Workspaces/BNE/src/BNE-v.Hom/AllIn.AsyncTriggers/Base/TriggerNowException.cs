using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllInTriggers.Base
{
    public class TriggerNowException : Exception
    {
        private readonly string _realStackTrace;
        public TriggerNowException(Exception ex)
            : base(ex.Message, ex)
        {
            _realStackTrace = System.Environment.StackTrace;
        }

        public override string StackTrace
        {
            get
            {
                return _realStackTrace;
            }
        }
    }

}
