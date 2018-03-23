using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BNE.BLL.Common.Helpers
{
    public class JSON
    {
        #region ToJSON
        /// <summary>
        ///     Convert a coleção para JSON
        /// </summary>
        /// <returns>A string xml que representa a coleção</returns>
        public static string ToJson(object anObject)
        {
            var res = JsonConvert.SerializeObject(anObject, new IsoDateTimeConverter());
            if (string.IsNullOrEmpty(res))
                return string.Empty;

            return res;
        }
        #endregion
    }
}