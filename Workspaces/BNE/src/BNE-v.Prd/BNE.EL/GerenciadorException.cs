using log4net;
using SharedKernel.Logger;
using System;
using System.Threading;

namespace BNE.EL
{
    public static class GerenciadorException
    {
        private static LogRepository logRep = new LogRepository();
        private static ILog log = logRep.GetLogger(typeof(GerenciadorException).FullName);

        public static string GravarAviso(string aviso)
        {
            log.Warn(aviso);
            return string.Empty;
        }

        public static string GravarInformacao(string info)
        {
            log.Warn(info);
            return string.Empty;
        }

        #region GravarExcecao
        public static string GravarExcecao(Exception ex)
        {
            if (ex is ThreadAbortException)
                return string.Empty;

            log.Error(null, ex);
            return Employer.PlataformaLog.LogError.WriteLog(ex).ToString();
        }
        public static string GravarExcecao(Exception ex, string customMessage)
        {
            if (ex is ThreadAbortException)
                return string.Empty;

            log.Error(customMessage, ex);
            return Employer.PlataformaLog.LogError.WriteLog(ex, customMessage).ToString();
        }
        public static string GravarExcecao(Exception ex, out string errorMessage)
        {
            if (ex is ThreadAbortException)
            {
                errorMessage = string.Empty;
                return string.Empty;
            }
          
            log.Error(null, ex);
            return Employer.PlataformaLog.LogError.WriteLog(ex, out errorMessage).ToString();
        }
        public static string GravarExcecao(Exception ex, out string errorMessage, string customMessage)
        {
            if (ex is ThreadAbortException)
            {
                errorMessage = string.Empty;
                return string.Empty;
            }

            log.Error(customMessage, ex);
            return Employer.PlataformaLog.LogError.WriteLog(ex, out errorMessage, customMessage).ToString();
        }
        #endregion

    }

}
