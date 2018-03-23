using System.Web;

namespace BNE.Common
{
    public class Helper
    {

        #region RecuperarIP
        public static string RecuperarIP()
        {
            var httpContext = HttpContext.Current;

            string ip = string.Empty;

            if (httpContext != null)
            {
                ip = httpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (string.IsNullOrEmpty(ip))
                    ip = httpContext.Request.ServerVariables["REMOTE_ADDR"];
            }

            return ip;
        }
        #endregion

    }
}
