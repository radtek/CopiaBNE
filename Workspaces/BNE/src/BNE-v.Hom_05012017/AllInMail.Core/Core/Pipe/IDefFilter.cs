using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllInMail.Core
{
    public interface IDefFilter
    {
        object DoFilter(object value);

        string TargetProperty { get; set; }
    }
}
