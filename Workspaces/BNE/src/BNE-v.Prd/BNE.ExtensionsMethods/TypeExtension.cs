using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BNE.ExtensionsMethods
{
    /// <summary>
    ///     Extensões para serem usadas com Enumerations
    /// </summary>
    public static class TypeExtension
    {
        public static string GetDescriptionAttribute(this Type type, object instance)
        {
            var des = GetAttribute<DescriptionAttribute>(type, instance);
            return des != null ? des.Description : string.Empty;
        }

        public static T GetAttribute<T>(this Type type, object instance) where T : Attribute
        {
            var memInfo = type.GetMember(instance.ToString());
            if (memInfo.Length == 0)
            {
                return null;
            }
            foreach (var m in memInfo)
            {
                try
                {
                    var att = m.GetCustomAttributes(typeof(T), true);
                    if (att.Length > 0)
                    {
                        return (T) att[0];
                    }
                }
                catch (ArgumentNullException)
                {
                }
            }

            return null;
        }

        public static T GetAttribute<T>(this Type type, bool inherit = false) where T : Attribute
        {
            return type.GetCustomAttributes(typeof(T), inherit).FirstOrDefault() as T;
        }

        public static List<string> GetEnumDescriptions(this Type type)
        {
            var values = Enum.GetValues(type);
            var enumDescriptions = new List<string>(values.Length);

            foreach (var v in values)
            {
                var vs = v.GetType().GetDescriptionAttribute(v);
                if (!string.IsNullOrEmpty(vs))
                {
                    enumDescriptions.Add(vs);
                }
            }

            return enumDescriptions;
        }

        public static List<KeyValuePair<string, string>> GetEnumValuesDescriptions(this Type type)
        {
            var values = Enum.GetValues(type);
            var enumValuesDescriptions = new List<KeyValuePair<string, string>>(values.Length);

            foreach (var v in values)
            {
                string vkey;

                try
                {
                    vkey = ((int) v).ToString();
                }
                catch
                {
                    vkey = v.ToString();
                }

                var vs = v.GetType().GetDescriptionAttribute(v);
                enumValuesDescriptions.Add(new KeyValuePair<string, string>(vkey, vs));
            }

            return enumValuesDescriptions;
        }
    }
}