using System;

namespace BNE.ExtensionsMethods
{
    public static class ObjectExtension
    {
        public static T GetAttribute<T>(this object instance) where T : Attribute
        {
            var type = instance.GetType();
            return type.GetAttribute<T>(instance);
        }
    }
}