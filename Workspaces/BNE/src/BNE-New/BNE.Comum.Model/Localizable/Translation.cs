using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BNE.Comum.Model.Localizable
{
    public class Translation
    {
        public string Type { get; set; }

        public string PrimaryKeyValue { get; set; }

        public string FieldName { get; set; }

        public string LanguageCode { get; set; }

        public string Text { get; set; }

    }
}
