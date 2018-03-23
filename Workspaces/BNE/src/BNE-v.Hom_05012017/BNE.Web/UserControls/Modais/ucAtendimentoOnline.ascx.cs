using System;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;

namespace BNE.Web.UserControls.Modais
{
    public partial class ucAtendimentoOnline : BaseUserControl
    {

        #region LocalizadoRodape - Variável 1
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public bool LocalizadoRodape
        {
            get
            {
                return (Boolean)(ViewState[Chave.Temporaria.Variavel1.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel1.ToString(), value);
            }
        }
        #endregion

        #region AjustarSOS
        public void AjustarSOS()
        {
            aSOS.Visible = false;
            aSOS.CausesValidation = false;
            aSOSRodape.Visible = false;
            aSOSRodape.CausesValidation = false;

            if (IdFilial.HasValue)
            {
                pnlWebCallBack.Visible = true;
                DefineModalParaExibir();
                upWebCallBack.Update();
            }
            else
            {
                pnlSOS.Visible = true;

                if (LocalizadoRodape)
                    aSOSRodape.Visible = true;
                else
                    aSOS.Visible = true;

                var idAtendimento = "265420800";

                //if (IdFilial.HasValue)
                //    idAtendimento = "248659200";

                litImagem.Text = string.Format("<img src=\"http://cloud.aloweb.com.br/img/0.gif\" border=\"0\" alt=\"Atendimento on line\" id=\"image49939200_{0}\" />", idAtendimento);
                litScript.Text = string.Format("<script type=\"text/javascript\" src=\"http://cloud.aloweb.com.br/img/49939200_{0}.js\"></script>", idAtendimento);

                upSOS.Update();
            }

        }
        #endregion

        protected void DefineModalParaExibir()
        {
            try
            {
                var nomeModalComercial = "modalComercial" + Guid.NewGuid();
                var nomeModalMensagem = "modalMensagem" + Guid.NewGuid();
                ucWebCallBack_Modais objWebCallBack_Dependencia = new Modais.ucWebCallBack_Modais();
                var objRetornoStatusCIA = objWebCallBack_Dependencia.RetornarStatus(BLL.Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.PilotoDeFila_Cia));

                ucWebCallBack_Modais.SetModalComercialId(nomeModalComercial);
                ucWebCallBack_Modais.SetModalMensagemId(nomeModalMensagem);

                if (objRetornoStatusCIA != null && objRetornoStatusCIA.disponivel > 0)
                {
                    modalzinha.Attributes.Add("data-target", "#" + nomeModalComercial);
                    //modalzinha.Attributes.Add("data-target", "#myModalComercial");
                }
                else
                {
                    var objRetornoStatusAtendimento = objWebCallBack_Dependencia.RetornarStatus(BLL.Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.PilotoDeFila_Atendimento));
                    if (objRetornoStatusAtendimento != null && objRetornoStatusAtendimento.disponivel > 0)
                    {
                        modalzinha.Attributes.Add("data-target", "#" + nomeModalComercial);
                        //modalzinha.Attributes.Add("data-target", "#myModalComercial");
                    }
                    else
                    {
                        modalzinha.Attributes.Add("data-target", "#" + nomeModalMensagem);
                        //modalzinha.Attributes.Add("data-target", "#myModalMensagem");
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