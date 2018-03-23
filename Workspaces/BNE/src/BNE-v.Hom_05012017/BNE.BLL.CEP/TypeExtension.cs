using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BNE.CEP
{
    /// <summary>
    /// Extensões para serem usadas com Enumerations
    /// </summary>
    public static class TypeExtension
    {
        /// <summary>
        /// Retorna o valor do atributo Description
        /// </summary>
        /// <param name="instance">A instância a ser extendida</param>
        /// <returns>O valor do atributo Description</returns>
        public static String GetDescriptionAttribute(this Type type, object instance)
        {
            var des = GetAttribute<DescriptionAttribute>(type, instance);
            return des != null ? des.Description : string.Empty;
        }

        public static T GetAttribute<T>(this Type type, object instance) where T : Attribute
        {
            var memInfo = type.GetMember(instance.ToString());
            if (memInfo.Length == 0)
                return null;
            foreach (var m in memInfo)
            {
                try
                {
                    var att = m.GetCustomAttributes(typeof(T), true);
                    if (att.Length > 0)
                        return (T)att[0];
                }
                catch (ArgumentNullException) { }
            }

            return null;
        }

        public static T GetAttribute<T>(this Type tipo, bool inherit = false) where T : Attribute
        {
            return tipo.GetCustomAttributes(typeof(T), inherit).FirstOrDefault() as T;            
        }

        public static List<string> GetEnumDescriptions(this Type tipo)
        {
            var lsValores = Enum.GetValues(tipo);
            var lsVDesc = new List<string>(lsValores.Length);

            foreach(var v in lsValores)
            {
                var vs = v.GetType().GetDescriptionAttribute(v);
                if (!string.IsNullOrEmpty(vs))
                    lsVDesc.Add(vs);
            }

            return lsVDesc;
        }

        public static List<KeyValuePair<string, string>> GetEnumValuesDescriptions(this Type tipo)
        {
            var lsValores = Enum.GetValues(tipo);
            var lsKv = new List<KeyValuePair<string, string>>(lsValores.Length);

            foreach (var v in lsValores)
            {
                string vkey = string.Empty;
                
                try { vkey = ((int)v).ToString(); }
                catch { vkey = v.ToString(); }

                var vs = v.GetType().GetDescriptionAttribute(v);
                lsKv.Add(new KeyValuePair<string, string>(vkey, vs));
            }

            return lsKv;
        }
    }
}
