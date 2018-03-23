using BNE.BLL;
using BNE.Web.Code;
using System;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web.UserControls.Modais
{
    public partial class ucWebCallBack : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Inicializar();
            }
        }

        public void Inicializar()
        {
            DefineModalParaExibir();
        }

        protected void DefineModalParaExibir()
        {
            try
            {
                ucWebCallBack_Modais objWebCallBack_Dependencia = new Modais.ucWebCallBack_Modais();
                var objRetornoStatusCIA = objWebCallBack_Dependencia.RetornarStatus(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PilotoDeFila_Cia));

                if (objRetornoStatusCIA != null && objRetornoStatusCIA.disponivel > 0)
                {
                    modalzinha.Attributes.Add("data-target", "#myModalComercial");
                }
                else
                {
                    var objRetornoStatusAtendimento = objWebCallBack_Dependencia.RetornarStatus(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PilotoDeFila_Atendimento));
                    if (objRetornoStatusAtendimento != null && objRetornoStatusAtendimento.disponivel > 0)
                    {
                        modalzinha.Attributes.Add("data-target", "#myModalComercial");
                    }
                    else
                    {
                        modalzinha.Attributes.Add("data-target", "#myModalMensagem");
                    }
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
        }
    }
}