using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.Web.Code;
using BNE.BLL;
using BNE.Web.Code.Enumeradores;
using BNE.EL;
using Resources;

namespace BNE.Web.UserControls.Forms.SalaAdministrador
{
    public partial class EmailPadrao : BaseUserControl
    {

        #region Propridades

        #region IdEmail
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        protected int? IdEmail
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

        #region rcbEmail_SelectedIndexChanged
        protected void rcbEmail_SelectedIndexChanged(object o, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            IdEmail = Convert.ToInt32(rcbEmail.SelectedValue);
            PreencherCampos();
        }
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        public void Inicializar()
        {
            UIHelper.CarregarRadComboBox(rcbEmail, ConteudoHTML.ListarConteudosEmail(), "Idf_Conteudo", "Nme_Conteudo");
            rcbEmail.SelectedIndex = 1;
            IdEmail = Convert.ToInt32(rcbEmail.SelectedValue);
            PreencherCampos();
            upPnlEmail.Update();
        }
        #endregion

        #region PreencherCampos
        private void PreencherCampos()
        {
            ConteudoHTML objConteudoHTML = ConteudoHTML.LoadObject(IdEmail.Value);
            reEmail.Content = objConteudoHTML.ValorConteudo;
            upPnlEmail.Update();
        }
        #endregion

        #region Salvar
        private void Salvar()
        {
            ConteudoHTML objConteudoHTML = ConteudoHTML.LoadObject(IdEmail.Value);
            objConteudoHTML.ValorConteudo = reEmail.Content;
            objConteudoHTML.Save();
        }
        #endregion

        #endregion

    }
}