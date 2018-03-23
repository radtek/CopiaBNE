using System.Linq;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Resources;
using System;
using System.Collections.Generic;
using BNE.BLL.Common;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web.UserControls.Modais
{
    public partial class EnvioEmail : System.Web.UI.UserControl
    {

        #region Enumeradores
        public enum TipoEnvioEmail
        {
            EmailSolicitacaoR1,
            EmailCompraCIA,
            /*
             * Com o novo fluxo para publicação imediata, o fluxo de divulgação em massa foi desativado. TASK #28579
             */
            //EmailWebEstagios,
            BoletoPagamento,
            ExtratoPlano
        }
        #endregion

        #region Propriedades
        public TipoEnvioEmail Tipo
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel1.ToString()] == null)
                    throw new InvalidOperationException("Não há tipo de envio de email no estado do componente!");

                return (TipoEnvioEmail)ViewState[Chave.Temporaria.Variavel1.ToString()];
            }

            set
            {
                ViewState[Chave.Temporaria.Variavel1.ToString()] = value;
            }
        }

        public string EnderecoEmail
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel3.ToString()] == null)
                    throw new InvalidOperationException("Não há endereço de email no estado do componente!");

                return ViewState[Chave.Temporaria.Variavel3.ToString()] as string;
            }

            set
            {
                ViewState[Chave.Temporaria.Variavel3.ToString()] = value;
            }
        }

        public string TemplateEmail
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel5.ToString()] == null)
                    throw new InvalidOperationException("Não há template de email no estado do componente!");

                return ViewState[Chave.Temporaria.Variavel5.ToString()] as string;
            }

            set
            {
                ViewState[Chave.Temporaria.Variavel5.ToString()] = value;
            }
        }

        public string TemplateAssunto
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel7.ToString()] == null)
                    throw new InvalidOperationException("Não há template de email no estado do componente!");

                return ViewState[Chave.Temporaria.Variavel7.ToString()] as string;
            }

            set
            {
                ViewState[Chave.Temporaria.Variavel7.ToString()] = value;
            }
        }

        public Dictionary<string, byte[]> ArquivoAnexo
        {
            get
            {
                return ViewState[Chave.Temporaria.Variavel8.ToString()] as Dictionary<string,byte[]>;
            }
            set
            {
                ViewState[Chave.Temporaria.Variavel8.ToString()] = value;
            }
        }
        #endregion

        public string HFIdPlanoAdquirido
        {
            get
            {
                return hfIdPlanoAdquirido.Value;
            }
            set
            {

                hfIdPlanoAdquirido.Value = value;
            }
        }

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                InicializarComponente();
        }
        #endregion

        #region btiFechar_Click
        protected void btiFechar_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            FecharModal();
        }
        #endregion

        #region btnEnviar_Click

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                /*
                 * Com o novo fluxo para publicação imediata, o fluxo de divulgação em massa foi desativado. TASK #28579
                 */
                //if (Tipo == TipoEnvioEmail.EmailWebEstagios)
                //{
                //    SessionVariable<int> IdFilial = new SessionVariable<int>(Chave.Permanente.IdFilial.ToString());
         
                //      if(IdFilial != null)
                //        ParametroFilial.SolicitaInfoEstag(txtMensagem.Valor,IdFilial.Value);
                //}

                var parametrosMensagem = new
                {
                    DataEnvio = DateTime.Today.ToShortDateString(),
                    Nome = txtNome.Valor,
                    Mensagem = txtMensagem.Valor
                };

                string assunto = parametrosMensagem.ToString(TemplateAssunto);
                string mensagem = parametrosMensagem.ToString(TemplateEmail);
                string para = EnderecoEmail;
                string de = txtEmail.Valor;

                if (Tipo == TipoEnvioEmail.BoletoPagamento || Tipo == TipoEnvioEmail.ExtratoPlano)
                {
                    para = txtEmail.Valor;
                    de = EnderecoEmail;
                }
                
                if (ArquivoAnexo != null)
                {

                    if (ArquivoAnexo.First().Value == null)
                    {
                        byte[] pdf = BoletoBancario.MontarBoletoBytes(Convert.ToInt32(HFIdPlanoAdquirido),Enumeradores.FormatoBoleto.PDF);
                        EmailSenderFactory
                        .Create(TipoEnviadorEmail.Fila)
                        .Enviar(assunto, mensagem,null, de, para, "boletos.pdf", pdf);
                    }
                    else
                    {
                        EmailSenderFactory
                        .Create(TipoEnviadorEmail.Fila)
                        .Enviar(assunto, mensagem, null, de, para, ArquivoAnexo.First().Key, ArquivoAnexo.First().Value);

                    }
                }
                else
                {
                    EmailSenderFactory
                        .Create(TipoEnviadorEmail.Fila)
                        .Enviar(assunto, mensagem,null, de, para);
                }

                MostrarConfirmacao("E-mail enviado com sucesso!", "imgSucesso");                
            }
            catch (Exception ex)
            {
                MostrarConfirmacao(ex.ToString(), "img_alerta_bne_pq");
            }
        }

        #endregion

        #endregion

        #region Métodos

        #region FecharModal
        public void FecharModal()
        {
            mpeEnvioEmail.Hide();
        }
        #endregion

        #region MostrarModal
        public void MostrarModal(TipoEnvioEmail tipoEnvioEmail)
        {
            mpeEnvioEmail.Show();

            InicializarModal(tipoEnvioEmail);
        }
        #endregion

        #region MostrarConfirmacao
        public void MostrarConfirmacao(String msg, String img)
        {
            lblSucesso.Text = msg;
            pnlFormularioEmail.Visible = false;
            pnlSucesso2.Visible = true;

            imgConfirm.ImageUrl = "/img/modal_nova/" + img + ".png";
        }
        #endregion

        #region InicializarModal

        private void InicializarModal(TipoEnvioEmail tipoEnvioEmail)
        {
            pnlFormularioEmail.Visible = true;
            pnlSucesso2.Visible = false;

            txtNome.Valor = String.Empty;
            txtEmail.Valor = String.Empty;
            txtMensagem.Valor = String.Empty;

            txtEmail.ExpressaoValidacao = Configuracao.regexEmail;

            // tipo de envio de e-mail
            Tipo = tipoEnvioEmail;
            var parametros = new List<Enumeradores.Parametro>
                    {
                        Enumeradores.Parametro.EmailMensagens, 
                        Enumeradores.Parametro.EmailContatoR1,
                        Enumeradores.Parametro.EmailWebEstagiosQueroContratar
                    };
            Dictionary<Enumeradores.Parametro, string> valoresParametros = Parametro.ListarParametros(parametros);

            string templateAssunto;

            switch (Tipo)
            {
                case EnvioEmail.TipoEnvioEmail.EmailCompraCIA:
                    lblTipoEnvio.Text = "Equipe BNE";
                    EnderecoEmail = valoresParametros[Enumeradores.Parametro.EmailContatoR1];
                    TemplateEmail = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.SolicitarContatoCIA, out templateAssunto);
                    TemplateAssunto = templateAssunto;
                    break;
                case EnvioEmail.TipoEnvioEmail.EmailSolicitacaoR1:
                    lblTipoEnvio.Text = "Recrutamento R1";
                    EnderecoEmail = valoresParametros[Enumeradores.Parametro.EmailContatoR1];
                    TemplateEmail = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.SolicitarContatoR1, out templateAssunto);
                    TemplateAssunto = templateAssunto;
                    break;
                /*
                 * Com o novo fluxo para publicação imediata, o fluxo de divulgação em massa foi desativado. TASK #28579
                 */
                //case EnvioEmail.TipoEnvioEmail.EmailWebEstagios:
                //    lblTipoEnvio.Text = "o Web Estágios, retornaremos em seguida";
                //    EnderecoEmail = valoresParametros[Enumeradores.Parametro.EmailWebEstagiosQueroContratar];
                //    TemplateEmail = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.ConteudoWebEstagiosIntegracaoQueroContratar, out templateAssunto);
                //    TemplateAssunto = templateAssunto;
                //    break;
                case EnvioEmail.TipoEnvioEmail.BoletoPagamento:
                    lblTipoEnvio.Text = "receber os boletos no e-mail";
                    EnderecoEmail = valoresParametros[Enumeradores.Parametro.EmailMensagens];
                    TemplateEmail = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.BoletosParaPagamento, out templateAssunto);
                    TemplateAssunto = templateAssunto;
                    break;
                case EnvioEmail.TipoEnvioEmail.ExtratoPlano:
                    lblTipoEnvio.Text = "extrato do meu plano";
                    EnderecoEmail = valoresParametros[Enumeradores.Parametro.EmailMensagens];
                    break;
                default:
                    throw new InvalidOperationException("Tipo de envio de email informado é inválido");
            }
        }

        #endregion

        #region InicializarComponente
        private void InicializarComponente()
        {

        }
        #endregion

        #endregion

    }
}