using BNE.BLL;
using System;
using System.Web;

namespace BNE.Web.Handlers
{
    /// <summary>
    /// Summary description for LogoBNERegistrarQuemMeViu
    /// </summary>
    public class LogoBNERegistrarQuemMeViu : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var idFilial = (!string.IsNullOrEmpty(context.Request.QueryString["idFilial"]) ? Convert.ToInt32(context.Request.QueryString["idFilial"]) : 0);
            var idCurriculo = (!string.IsNullOrEmpty(context.Request.QueryString["idCurriculo"]) ? Convert.ToInt32(context.Request.QueryString["idCurriculo"]) : 0);
            var idVaga = (!string.IsNullOrEmpty(context.Request.QueryString["idVaga"]) ? Convert.ToInt32(context.Request.QueryString["idVaga"]) : 0);
            var idUsuarioFilialPerfl = (!string.IsNullOrEmpty(context.Request.QueryString["IdUsuarioFilial"]) ? Convert.ToInt32(context.Request.QueryString["idUsuarioFilial"]) : 0);

            if (idFilial > 0 && idCurriculo > 0)
            {
                CurriculoQuemMeViu.SalvarQuemMeViuEmail(new Filial(idFilial), new Curriculo(idCurriculo));
            }
            /*Salva a data da visualização do Cv*/
            if (idVaga > 0 && idCurriculo > 0 && idUsuarioFilialPerfl > 0 && idFilial > 0)
            {
                VagaCandidato.SalvarVisualizacaoCandidato(idCurriculo, idVaga, idFilial, idUsuarioFilialPerfl);
            }


            context.Response.Redirect("~/img/cv_email/px-handler.png");
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