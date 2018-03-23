using System;
using System.Collections.Generic;
using System.Web.UI;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Resources;
using Resources;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web
{
    public partial class ContatoSite : BasePage
    {

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                InicializarBarraBusca(TipoBuscaMaster.Vaga, false, "ContatoSite");

            txtEmail.ExpressaoValidacao = Configuracao.regexEmail;
            Ajax.Utility.RegisterTypeForAjax(typeof(ContatoSite));
        }
        #endregion

        #region btiAtendimentoOnline_Click
        protected void btiAtendimentoOnline_Click(object sender, ImageClickEventArgs e)
        {
            if (base.IdFilial.HasValue)
                ScriptManager.RegisterStartupScript(upAtendimentoOnline, upAtendimentoOnline.GetType(), "AbrirURL", "javascript: AbrirPopup('AbrirAtendimentoOnLine.aspx?TIPO=2',600, 800)", true); //Empresa
            else if (base.IdPessoaFisicaLogada.HasValue)
                ScriptManager.RegisterStartupScript(upAtendimentoOnline, upAtendimentoOnline.GetType(), "AbrirURL", "javascript: AbrirPopup('AbrirAtendimentoOnLine.aspx?TIPO=1',600, 800)", true); //Usuario    
            else
                ScriptManager.RegisterStartupScript(upAtendimentoOnline, upAtendimentoOnline.GetType(), "AbrirURL", "javascript: AbrirPopup('AbrirAtendimentoOnLine.aspx?TIPO=3',600, 800)", true); //Não Logado
        }
        #endregion

        #region btnEnviar_Click
        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                var parametros = new List<Enumeradores.Parametro>
                    {
                        Enumeradores.Parametro.EmailMensagens, 
                        Enumeradores.Parametro.EmailDestinoFaleConosco
                    };
                Dictionary<Enumeradores.Parametro, string> valoresParametros = Parametro.ListarParametros(parametros);

                string templateAssunto;
                string templateMensagem = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.FaleConosco, out templateAssunto);

                var parametrosMensagem = new
                {
                    DataEnvio = DateTime.Today.ToShortDateString(),
                    Nome = txtNome.Valor,
                    Email = txtEmail.Valor,
                    NomeCidade = txtCidade.Text,
                    Mensagem = txtMensagem.Valor,
                    Assunto = txtAssunto.Valor
                };

                string assunto = parametrosMensagem.ToString(templateAssunto);
                string mensagem = parametrosMensagem.ToString(templateMensagem);

                string para = valoresParametros[Enumeradores.Parametro.EmailDestinoFaleConosco];
                string de = txtEmail.Valor;

                EmailSenderFactory
                    .Create(TipoEnviadorEmail.Smtp)
                    .Enviar(assunto, mensagem, de, para);

                LimparCampos();

                ucModalConfirmacao.PreencherCampos("Confirmação de Envio", MensagemAviso._203016, false);
                ucModalConfirmacao.MostrarModal();
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btnVoltar_Click
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Redirect("Default.aspx");
        }
        #endregion

        #endregion

        #region Métodos

        #region LimparCampos
        private void LimparCampos()
        {
            txtNome.Valor = txtEmail.Valor = txtCidade.Text = txtAssunto.Valor = txtMensagem.Valor = String.Empty;
        }
        #endregion

        #endregion

        #region AjaxMethods

        #region ValidarCidade
        /// <summary>
        /// Validar cidade
        /// </summary>
        /// <param name="valor"></param>
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