using BNE.BLL;
using System;
using System.Web;

namespace BNE.Web.Handlers
{

    public class HandlerFotos : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //context.Response.Cache.SetCacheability(HttpCacheability.Server);
            //context.Response.BufferOutput = false;

            //if (context.Request.QueryString["LinkOrigem"] != null)
            //{
            //    int idOrigem;
            //    if (Int32.TryParse(context.Request.QueryString["LinkOrigem"], out idOrigem))
            //    {
            //        OrigemFilial objOrigemFilial = OrigemFilial.CarregarPorOrigem(idOrigem);
            //        if (objOrigemFilial.Persisted)
            //        {
            //            byte[] byteArray = objOrigemFilial.ImagemLogo;
            //            context.Response.OutputStream.Write(byteArray, 0, byteArray.Length);
            //        }       
            //    }                
            //}
            
            //TODO: Gieyson
            //string idFoto = context.Request.QueryString["ID"];

            //if (!string.IsNullOrEmpty(idFoto))
            //{
            //    byte[] byteArray = PessoaFisicaFoto.RecuperarArquivo(Convert.ToInt32(idFoto));
            //    context.Response.OutputStream.Write(byteArray, 0, byteArray.Length); 
            //}
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }        
    }
}