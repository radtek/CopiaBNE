using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllInMail.Core
{
    public class AllInFilter
    {
        public static DefPreFilterBase<string> ReplaceStringFilter(string toReplace, string changeTo, string propTarget = null)
        {
            if (propTarget == null)
                return new DefPreFilterBase<string>(a => (a ?? string.Empty).Replace(toReplace, changeTo));

            return new DefPreFilterBase<string>(propTarget, a => (a ?? string.Empty).Replace(toReplace, changeTo));
        }
        public static DefTextSize GetSizeFilter(int size, string propTarget = null)
        {
            if (propTarget == null)
                return new DefTextSize(size);

            return new DefTextSize(propTarget, size);
        }
    }
}
