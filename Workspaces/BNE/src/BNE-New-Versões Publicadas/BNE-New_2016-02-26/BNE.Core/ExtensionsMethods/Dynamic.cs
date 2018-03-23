using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;

namespace BNE.Core.ExtensionsMethods
{
    public static class Dynamic
    {
        public static dynamic ToDynamic(this object value)
        {
            IDictionary<string, object> expando = new ExpandoObject();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType()))
                expando.Add(property.Name, property.GetValue(value));

            return (ExpandoObject)expando;
        }
    }
}
