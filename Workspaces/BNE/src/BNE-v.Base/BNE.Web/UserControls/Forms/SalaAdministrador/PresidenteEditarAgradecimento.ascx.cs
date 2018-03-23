using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.BLL;
using BNE.Web.Code;
using BNE.EL;
using BNE.Web.Resources;
using Resources;


namespace BNE.Web.UserControls.Forms.SalaAdministrador
{
    public partial class EditarAgradecimento : BaseUserControl
    {
        #region Propriedades

        #region IdfAgradecimento
        /// <summary>
        /// Propriedade que armazena e recupera o ID do Agradecimento
        /// </summary>
        protected int IdfAgradecimento
        {
            get
            {
                if (Session["IdfAgradecimento"] != null)
                    return Int32.Parse(Session["IdfAgradecimento"].ToString());
                else
                    return 0;
            }
            set
            {
                Session.Add("IdfAgradecimento", value);
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            revEmail.ValidationExpression = Configuracao.regexEmail;
            Ajax.Utility.RegisterTypeForAjax(typeof(EditarAgradecimento));
            ScriptManager.RegisterStartupScript(this, this.GetType(), "truncate", "employer.util.findControl('txtAgradecimentoEditar').attr('MaxLength','1000');", true);
        }
        #endregion

        #region btnSalvar_Click
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                Agradecimento objAgradecimento = Agradecimento.LoadObject(IdfAgradecimento);
                objAgradecimento.NomePessoa = txtNomeEditar.Text;
                objAgradecimento.EmailPessoa = txtEmailEditar.Text;
                string nomeCidade;
                if (!String.IsNullOrEmpty(txtCidade.Text))
                {
                    nomeCidade = txtCidade.Text.Split('/')[0];


                    Cidade objCidade = null;
                    if (Cidade.CarregarPorNome(nomeCidade, out objCidade))
                        objAgradecimento.Cidade = objCidade;
                    else
                        objAgradecimento.Cidade = null;
                }
                else
                    objAgradecimento.Cidade = null;
                objAgradecimento.DescricaoMensagem = txtAgradecimentoEditar.Text;
                objAgradecimento.FlagAuditado = rdbSimEditar.Checked;
                objAgradecimento.Save();

                //mensagem atualizado com sucesso
                ucModalConfirmacao.PreencherCampos(MensagemAviso._23012, MensagemAviso._24024, false, "OK");
                ucModalConfirmacao.MostrarModal();

                //limpar campos
                txtAgradecimentoEditar.Text = "";
                txtCidade.Text = "";
                txtEmailEditar.Text = "";
                txtNomeEditar.Text = "";
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region rdbSimEditar_CheckedChanged
        protected void rdbSimEditar_CheckedChanged(object sender, EventArgs e)
        {

            rdbSimEditar.Checked = ((RadioButton)sender).Checked;
            rdbNaoEditar.Checked = !((RadioButton)sender).Checked;

        }
        #endregion

        #region rdbNaoEditar_CheckedChanged
        protected void rdbNaoEditar_CheckedChanged(object sender, EventArgs e)
        {
            rdbSimEditar.Checked = !((RadioButton)sender).Checked;
            rdbNaoEditar.Checked = ((RadioButton)sender).Checked;
        }
        #endregion

        #endregion

        #region Metodos

        #region PreencherCampos

        public void PreencherCampos(Agradecimento objAgradecimento)
        {
            LimparCampos();

            IdfAgradecimento = objAgradecimento.IdAgradecimento;
            txtNomeEditar.Text = objAgradecimento.NomePessoa;
            txtEmailEditar.Text = objAgradecimento.EmailPessoa;
            objAgradecimento.Cidade.CompleteObject();
            objAgradecimento.Cidade.Estado.CompleteObject();
            txtCidade.Text = objAgradecimento.Cidade.NomeCidade + "/" + objAgradecimento.Cidade.Estado.SiglaEstado;
            txtAgradecimentoEditar.Text = objAgradecimento.DescricaoMensagem;

            rdbNaoEditar.Checked = !objAgradecimento.FlagAuditado;
            rdbSimEditar.Checked = objAgradecimento.FlagAuditado;
        }
        #endregion

        #region LimparCampos

        private void LimparCampos()
        {
            IdfAgradecimento = 0;
            txtNomeEditar.Text = "";
            txtEmailEditar.Text = "";
            txtCidade.Text = "";
            txtAgradecimentoEditar.Text = "";
        }

        #endregion

        #endregion

        #region AjaxMethods

        #region ValidarCidade
        /// <summary>
        /// Validar cidade
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="siglaUF"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public static bool ValidarCidade(string valor)
        {
            valor = valor.Trim();

            if (string.IsNullOrEmpty(valor))
                return true;

            Cidade objCidade;
            return Cidade.CarregarPorNome(valor, out objCidade);
        }
        #endregion

        #endregion
    }
}