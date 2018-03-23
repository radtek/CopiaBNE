using System;
using System.Web;
using System.Web.SessionState;

namespace BNE.Componentes.Handlers
{
    /// <summary>
    /// Http Handler para retornar o captcha da receita federal
    /// </summary>
    public class HandlerCaptchaReceitaFederal : IHttpHandler, IRequiresSessionState
    {

        public static string ChaveCaptchaReceitaFederal = "199aa6f3-599f-4040-a1e5-428f3f0e8479_Receita";

        #region IsReusable
        /// <summary>
        /// Define se o Handler é ou não reutilizável
        /// </summary>
        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }
        #endregion 

        #region ProcessRequest
        /// <summary>
        /// Processa a requisição
        /// </summary>
        /// <param name="context">O contexto da requisição</param>
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                context.Response.ContentType = "image/gif";
                var b = (byte[]) context.Session[HandlerCaptchaReceitaFederal.ChaveCaptchaReceitaFederal];
                context.Response.BinaryWrite(b);
                context.Response.End();
            }
            catch
            {
                //PlataformaLog.LogError.WriteLog(ex);
            }
        }
        #endregion 
    }
}
