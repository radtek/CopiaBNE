using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.BLL;
using Resources;
using BNE.Web.Code;
using BNE.EL;

namespace BNE.Web.UserControls.Forms.SalaAdministrador
{
    public partial class NovaRespostaAutomatica : BaseUserControl
    {
        #region Eventos

        
        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "truncate", "employer.util.findControl('txtDescRespostaAutomatica').attr('MaxLength','5000');", true);
        }
        #endregion

        #region btnSalvar_Click
        
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                RespostaAutomatica objRespostaAutomatica = new RespostaAutomatica();
                objRespostaAutomatica.TituloRespostaAutomatica = txtTitulo.Text;
                objRespostaAutomatica.DescricaoRespostaAutomatica = txtDescRespostaAutomatica.Text;
                objRespostaAutomatica.Save();

                //mensagem sucesso
                ucModalConfirmacao.PreencherCampos(MensagemAviso._23012, MensagemAviso._100001, false, "OK");
                ucModalConfirmacao.MostrarModal();

                //limpar campos
                txtDescRespostaAutomatica.Text = "";
                txtTitulo.Text = "";
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #endregion

      
    }
}