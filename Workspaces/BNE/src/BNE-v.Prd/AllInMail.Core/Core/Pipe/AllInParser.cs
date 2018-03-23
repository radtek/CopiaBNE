using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllInMail.Core
{
    public class AllInParser
    {
        public readonly static DefPreParser DatePtBrFormat = new DefinitionParser<DateTime>(a => a.ToString("dd/MM/yyyy"));

        public readonly static DefPreParser DatePtBrCustomFormat = new DefinitionParser<DateTime>(a => a.ToString("dd-MM-yyyy"));

        public readonly static DefPreParser DatePrBrNullableFormat = new DefinitionParser<DateTime?>(a => !a.HasValue ? string.Empty : a.Value.ToString("dd/MM/yyyy"));

        public readonly static DefPreParser DateNullablePtBrCustomFormat = new DefinitionParser<DateTime?>(a => !a.HasValue ? string.Empty : a.Value.ToString("dd-MM-yyyy"));

        public readonly static DefPreParser IntNullableFormat = new DefinitionParser<int?>(a => !a.HasValue ? string.Empty : a.Value.ToString());

        public readonly static DefPreParser BoolFormat = new DefinitionParser<bool>(a => a ? "1" : "0");

        public readonly static DefPreParser BoolNullableFormat = new DefinitionParser<bool?>(a => a.HasValue ? (a.Value ? "1" : "0") : string.Empty);

        public readonly static DefPreParser StringNullFormat = new DefinitionParser<string>(a => a == null ? string.Empty : a);
    }
}
