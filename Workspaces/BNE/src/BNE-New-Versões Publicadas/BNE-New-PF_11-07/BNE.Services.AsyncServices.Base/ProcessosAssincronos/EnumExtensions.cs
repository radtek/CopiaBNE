using System;
using System.ComponentModel;

namespace BNE.Services.AsyncServices.Base.ProcessosAssincronos
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum instance)
        {
            return ((DescriptionAttribute)instance.GetType().GetMember(instance.ToString())[0].GetCustomAttributes(typeof(DescriptionAttribute), false)[0]).Description;
        }
    }
}
