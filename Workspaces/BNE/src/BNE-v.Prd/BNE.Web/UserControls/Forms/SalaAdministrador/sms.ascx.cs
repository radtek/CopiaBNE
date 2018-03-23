using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.BLL;
using BNE.EL;
using Resources;

namespace BNE.Web.UserControls.Forms.SalaAdministrador
{
    public partial class sms : BaseUserControl
    {

        #region Propridades

        #region IdSMS
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        protected int? IdSMS
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel1.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel1.ToString()].ToString());
                else
                    return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel1.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel1.ToString());
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

        #region btnSalvar_Click
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                Salvar();
                //LimparCampos();
                base.ExibirMensagemConfirmacao("Confirmação de Cadastro", "Cadastro atualizado com sucesso", false);
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region rcbSMS_SelectedIndexChanged
        protected void rcbSMS_SelectedIndexChanged(object o, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            IdSMS = Convert.ToInt32(rcbSMS.SelectedValue);
            PreencherCampos();
        }
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        public void Inicializar()
        {
            UIHelper.CarregarRadComboBox(rcbSMS, ConteudoHTML.ListarConteudosSMS(), "Idf_Conteudo", "Nme_Conteudo");
            rcbSMS.SelectedIndex = 1;
            IdSMS = Convert.ToInt32(rcbSMS.SelectedValue);
            PreencherCampos();
            upPnlSMS.Update();
        }
        #endregion

        #region PreencherCampos
        private void PreencherCampos()
        {
            ConteudoHTML objConteudoHTML = ConteudoHTML.LoadObject(IdSMS.Value);
            txtSMS.Text = objConteudoHTML.ValorConteudo;
            AjustarContagemCaracter(txtSMS.Text.Length);
            upPnlSMS.Update();
        }
        #endregion

        #region AjustarContagemCaracter
        /// <summary>
        /// Chama o JavaScript e passa os parametros necessários
        /// </summary>
        /// <param name="menorValor"></param>
        public void AjustarContagemCaracter(int caracteresMensagem)
        {
            int valorFixo = 140;
            int valorAtual = valorFixo - caracteresMensagem;
            lblTotalCaracteres.Text = valorAtual + " caracteres";

            txtSMS.Attributes.Add("onkeyup", "return Contador(this,'" + lblTotalCaracteres.ClientID + "'," + valorFixo + ")");
            txtSMS.Attributes.Add("onKeyPress", "return LimitaCaracteres(this," + valorFixo + ")");

            upContagemCaracter.Update();
        }
        #endregion

        #region Salvar
        private void Salvar()
        {
            ConteudoHTML objConteudoHTML = ConteudoHTML.LoadObject(IdSMS.Value);
            objConteudoHTML.ValorConteudo = txtSMS.Text;
            objConteudoHTML.Save();
        }
        #endregion

        #endregion
        
    }
}