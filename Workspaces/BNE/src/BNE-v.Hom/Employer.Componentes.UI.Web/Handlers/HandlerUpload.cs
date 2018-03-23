using System;
using System.Web;
using System.IO;

namespace Employer.Componentes.UI.Web.Handlers
{
    /// <summary>
    /// Handler de Upload de arquivo usado no componente EmployerMultipleUpload
    /// </summary>
    public class HandlerUpload : IHttpHandler
    {
        /// <summary>
        /// You will need to configure this handler in the web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        /// <inheritdoc/>
        public void ProcessRequest(HttpContext context)
        {
            DirectoryInfo dicUp = new DirectoryInfo(context.Server.MapPath("~/Temp"));
            if (!dicUp.Exists)
                dicUp.Create();

            var IdArquivo = context.Request.QueryString["IdArquivos"];

            //var arquivosAntigos = dicUp.GetFiles(string.Format("Arquivo_{0}*", IdArquivo));
            //for (int i = 0; arquivosAntigos.Length > i; i++)
            //{
            //    var arquivo = arquivosAntigos[i];
            //    arquivo.Delete();
            //}

            for (int i = 0; context.Request.Files.Count > i; i++)
            {
                var file = context.Request.Files[i];

                var ponto = file.FileName.LastIndexOf('.');
                var extensao = file.FileName.Substring(ponto);
                var nomeAmigavel = file.FileName.Substring(0, ponto);

                int numAqruivo = i;
                string nomeArquivo = string.Empty;
                do
                {
                    nomeArquivo = Path.Combine(dicUp.FullName, string.Format("Arquivo_{0}_{1}NomeAmigavel_{3}{2}", 
                        IdArquivo, numAqruivo, extensao,
                        nomeAmigavel));

                    numAqruivo++;
                } while (File.Exists(nomeArquivo));

                file.SaveAs(nomeArquivo);
            }
        }

        #endregion
    }
}
