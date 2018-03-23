using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllInMail.Core
{
    public abstract class AllInDefConverterBase<T> : IAllInDefConverter<T>
    {
        readonly Lazy<string[]> _definitionFields;
        public AllInDefConverterBase()
        {
            _definitionFields = new Lazy<string[]>(BuildDefinedFields);
        }


        public abstract string Parse(T modelToParse);

        public abstract string Delimiter { get; set; }

        public abstract string GetDeclaration();

        private string[] BuildDefinedFields()
        {
            return (GetDeclaration() ?? string.Empty).Split(';');
        }

        public virtual string[] GetDefiniedFields()
        {
            return _definitionFields.Value;
        }

        public virtual string Parse(object model)
        {
            if (!(model is T))
                throw new InvalidCastException("original");

            return Parse((T)model);
        }
    }

}
