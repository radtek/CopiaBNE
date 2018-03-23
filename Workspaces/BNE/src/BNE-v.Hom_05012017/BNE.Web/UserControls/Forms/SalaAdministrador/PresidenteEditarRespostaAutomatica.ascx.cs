using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.Web.Code;
using BNE.EL;
using Resources;
using BNE.BLL;

namespace BNE.Web.UserControls.Forms.SalaAdministrador
{
    public partial class EditarRespostaAutomatica : BaseUserControl
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "truncate", "employer.util.findControl('txtDescRespostaAutomatica').attr('MaxLength','5000');", true);
        }

        #endregion

        #region btnSalvar_Click
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                RespostaAutomatica objRespostaAutomatica = RespostaAutomatica.LoadObject(IdfRespostaAutomatica);
                objRespostaAutomatica.TituloRespostaAutomatica = txtTitulo.Text;
                objRespostaAutomatica.DescricaoRespostaAutomatica = txtDescRespostaAutomatica.Text;
                objRespostaAutomatica.Save();


                //mensagem atualizado com sucesso
                ucModalConfirmacao.PreencherCampos(MensagemAviso._23012, MensagemAviso._24024, false, "OK");
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

        #region Metodos

        #region PreencherCampos

        public void PreencherCampos(RespostaAutomatica objRespostaAutomatica)
        {
            LimparCampos();

            IdfRespostaAutomatica = objRespostaAutomatica.IdRespostaAutomatica;
            txtTitulo.Text = objRespostaAutomatica.TituloRespostaAutomatica;
            txtDescRespostaAutomatica.Text = objRespostaAutomatica.DescricaoRespostaAutomatica;

        }
        #endregion

        #region LimparCampos

        private void LimparCampos()
        {
            IdfRespostaAutomatica = 0;
            txtTitulo.Text = "";
            txtDescRespostaAutomatica.Text = "";
        }

        #endregion

       

        #endregion

      
    }
}