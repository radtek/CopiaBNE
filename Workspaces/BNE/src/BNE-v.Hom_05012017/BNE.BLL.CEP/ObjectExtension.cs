using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BNE.CEP
{
    public static class ObjectExtension
    {
        public static T GetAttribute<T>(this object instance) where T : Attribute
        {
            Type type = instance.GetType();
            return TypeExtension.GetAttribute<T>(type, instance);
        }
    }
}
