using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using System.Data;
using Telerik.Web.UI;
using BNE.BLL;
using Resources;
using BNE.EL;

namespace BNE.Web.UserControls.Forms.SalaAdministrador
{
    public partial class PresidenteRespAutoVisualizarTodas : BaseUserControl
    {
        #region Propriedades

        #region IdfRespostaAutomatica
        /// <summary>
        /// Propriedade que armazena e recupera o ID do Agradecimento
        /// </summary>
        protected int IdfRespostaAutomatica
        {
            get
            {
                if (Session["IdfRespostaAutomatica"] != null)
                    return Int32.Parse(Session["IdfRespostaAutomatica"].ToString());
                else
                    return 0;
            }
            set
            {
                Session.Add("IdfRespostaAutomatica", value);
            }
        }
        #endregion

        #endregion

        #region Eventos
     
        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #endregion

        #region btnEditarRespostaAutomatica_Click
        
        protected void btnEditarRespostaAutomatica_Click(object sender, ImageClickEventArgs e)
        {
            RespostaAutomatica objRespostaAutomatica = RespostaAutomatica.LoadObject(Convert.ToInt32(((ImageButton)sender).CommandArgument.ToString()));
            upRespostaAutomatica.Visible = false;
            upEditarRespostaAutomatica.Visible = true;
            ucPresidenteEditarRespostaAutomatica.PreencherCampos(objRespostaAutomatica);

            upPrincipal.Update();

         
        }
        #endregion

        #region btnExcluirRespostaAutomatica_Click
        
        protected void btnExcluirRespostaAutomatica_Click(object sender, ImageClickEventArgs e)
        {
            IdfRespostaAutomatica = Convert.ToInt32(((ImageButton)sender).CommandArgument.ToString());
            try
            {
                Agradecimento.Delete(IdfRespostaAutomatica);

                ucModalConfirmacao.PreencherCampos(MensagemAviso._23012, MensagemAviso._24023, false, "OK");
                ucModalConfirmacao.MostrarModal();

                CarregarGrid();

            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion
       
        #endregion

        #region Metodos
        
        #region CarregarGrid
     
        public void CarregarGrid()
        {
            UIHelper.CarregarRadGrid(gvVisualizarTodas, RespostaAutomatica.ListarTodas());
        }

        #endregion

        #endregion
    }
}