using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LanHouse.Business.EL
{
    public static class GerenciadorException
    {
        #region GravarExcecao
        public static string GravarExcecao(Exception ex)
        {
            if (ex is ThreadAbortException)
                return string.Empty;

            return Employer.PlataformaLog.LogError.WriteLog(ex).ToString();
        }
        public static string GravarExcecao(Exception ex, string customMessage)
        {
            if (ex is ThreadAbortException)
                return string.Empty;

            return Employer.PlataformaLog.LogError.WriteLog(ex, customMessage).ToString();
        }
        public static string GravarExcecao(Exception ex, out string errorMessage)
        {
            if (ex is ThreadAbortException)
            {
                errorMessage = string.Empty;
                return string.Empty;
            }
            return Employer.PlataformaLog.LogError.WriteLog(ex, out errorMessage).ToString();
        }
        public static string GravarExcecao(Exception ex, out string errorMessage, string customMessage)
        {
            if (ex is ThreadAbortException)
            {
                errorMessage = string.Empty;
                return string.Empty;
            }
            return Employer.PlataformaLog.LogError.WriteLog(ex, out errorMessage, customMessage).ToString();
        }
        #endregion
    }
}
