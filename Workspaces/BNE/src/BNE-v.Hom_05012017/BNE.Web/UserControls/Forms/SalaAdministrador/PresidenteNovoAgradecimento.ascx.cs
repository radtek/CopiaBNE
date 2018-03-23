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
using BNE.Web.Resources;

namespace BNE.Web.UserControls.Forms.SalaAdministrador
{
    public partial class NovoAgradecimento : BaseUserControl
    {
        #region Eventos

        #region Page_Load
        
        protected void Page_Load(object sender, EventArgs e)
        {
                revEmail.ValidationExpression = Configuracao.regexEmail;
                Ajax.Utility.RegisterTypeForAjax(typeof(NovoAgradecimento));
                ScriptManager.RegisterStartupScript(this, this.GetType(), "truncate", "employer.util.findControl('txtAgradecimentoNovo').attr('MaxLength','1000');", true);
        }

        #endregion

        #region btnSalvar_Click
        
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                Agradecimento objAgradecimento = new Agradecimento();
                objAgradecimento.NomePessoa = txtNomeNovo.Text;
                objAgradecimento.EmailPessoa = txtEmailNovo.Text;
                
                Cidade objCidade = null;
                String nomeCidade = txtCidade.Text.Split('/')[0];
                if (Cidade.CarregarPorNome(nomeCidade, out objCidade))
                    objAgradecimento.Cidade = objCidade;
                else
                    objAgradecimento.Cidade = null;

                objAgradecimento.DescricaoMensagem = txtAgradecimentoNovo.Text;
                objAgradecimento.FlagAuditado = rdbSimNovo.Checked;
                objAgradecimento.Save();

                //mensagem sucesso
                ucModalConfirmacao.PreencherCampos(MensagemAviso._23012, MensagemAviso._100001, false, "OK");
                ucModalConfirmacao.MostrarModal();
                //ucPresidente

                //limpar campos
                txtAgradecimentoNovo.Text = "";
                txtCidade.Text = "";
                txtEmailNovo.Text = "";
                txtNomeNovo.Text = "";

            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
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