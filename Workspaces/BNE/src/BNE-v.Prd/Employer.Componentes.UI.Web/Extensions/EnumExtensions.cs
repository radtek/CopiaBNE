using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Employer.Componentes.UI.Web.Extensions
{
    /// <summary>
    /// Extensões para serem usadas com Enumerations
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Retorna o valor do atributo Description
        /// </summary>
        /// <param name="instance">A instância a ser extendida</param>
        /// <returns>O valor do atributo Description</returns>
        public static String GetDescription (this Enum instance)
        {
            var type = instance.GetType();
            var memInfo = type.GetMember(instance.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            return ((DescriptionAttribute)attributes[0]).Description;

        }
    }
}
