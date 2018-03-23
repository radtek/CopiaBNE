using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Resources;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web.UserControls.Modais
{
    public partial class CompartilhamentoVaga : BaseUserControl
    {
        #region Classe privada

        [Serializable]
        private class RepeaterEmail
        {
            public Guid Guid { get; set; }
            public string Email { get; set; }
            public bool FlagInativo { get; set; }
        }

        #endregion

        #region Constantes
        private const int MAXIMA_QTDE_EMAILS = 5;
        #endregion

        #region Propriedades

        #region DadosRepeater - Variável 2
        private List<RepeaterEmail> DadosRepeater
        {
            get 
            {
                if (ViewState[Chave.Temporaria.Variavel2.ToString()] == null)
                {
                    ViewState[Chave.Temporaria.Variavel2.ToString()] =
                        new List<RepeaterEmail>();
                }

                return (List<RepeaterEmail>)ViewState[Chave.Temporaria.Variavel2.ToString()];
            }
        }
        #endregion

        #region Vaga - Variável 4
        public int IdVaga
        {
            get
            {
                return Convert.ToInt32(ViewState[Chave.Temporaria.Variavel4.ToString()]);
            }

            set
            {
                ViewState[Chave.Temporaria.Variavel4.ToString()] = value;
            }
        }
        #endregion

        #region UrlVaga - Variavel 6
        public string UrlVaga
        {
            get
            {
                return ViewState[Chave.Temporaria.Variavel6.ToString()].ToString();
            }

            set
            {
                ViewState[Chave.Temporaria.Variavel6.ToString()] = value;
            }
        }
        #endregion

        #region UsuarioFilialPerfil - Variavel 8
        public int IdUsuarioFilialPerfil
        {
            get
            {
                return Convert.ToInt32(ViewState[Chave.Temporaria.Variavel8.ToString()]);
            }

            set
            {
                ViewState[Chave.Temporaria.Variavel8.ToString()] = value;
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        /// <summary>
        /// Método executado quando a página é carregada
        /// </summary>
        /// <param name="sender">Objeto Correspondente</param>
        /// <param name="e">Argumento do Evento</param>
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

        #region rptEmpresas_ItemCommand
        protected void rptEmails_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("DeletarEmail"))
            {
                Guid guidSelecionada =
                    Guid.Parse(e.CommandArgument.ToString());

                var selecao =
                    DadosRepeater
                        .FirstOrDefault(repeater => repeater.Guid.Equals(guidSelecionada));

                if (null != selecao)
                {
                    //Inativa o email selecionado
                    selecao.FlagInativo = true;
                    AtacharDadosAoRepeater();
                }
            }

            if (NaoUltrapassouQtdeMaximaEmails())
                SetarFocoCaixaEmail();
            else
                DesabilitarCaixaEmail();
        }
        #endregion

        #region btnEnviar_Click
        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            string emailDestinatario =
                txtEmailDestinatario.Text.Trim();

            if (!String.IsNullOrEmpty(emailDestinatario) && 
                null == DadosRepeater.FirstOrDefault(repeater => repeater.Email.Equals(emailDestinatario)))
            {
                DadosRepeater.Add(new RepeaterEmail
                {
                    Guid = Guid.NewGuid(),
                    Email = emailDestinatario,
                    FlagInativo = false
                });
            }

            List<string> listaDeEmails =
                DadosRepeater
                    .Select(repeater => repeater.Email)
                    .ToList();

            if (EnviarMensagem() > 0)
            {
                EnviarConfirmacao(listaDeEmails);

                pnlEmailsDestinatario.Visible = false;
                pnlSucesso.Visible = true;
            }
            else
                FecharModal();
        }
        #endregion

        #region btlCliqueAqui_Click
        protected void btlCliqueAqui_Click(object sender, EventArgs e)
        {
            FecharModal();
        }
        #endregion

        #region btnAdicionarEmail_Click
        protected void btnAdicionarEmail_Click(object sender, EventArgs e)
        {
            string emailDestinatario =
                txtEmailDestinatario.Text.Trim();

            if (String.IsNullOrWhiteSpace(emailDestinatario))
            {
                // sai do método, porque o email passado está vazio
                txtEmailDestinatario.Focus();
                return;
            }

            ///Verifica se o email já esta listado, caso sim e esteja inativo, ativa
            var emailJaExiste =
                DadosRepeater.FirstOrDefault(
                    repeater =>
                        repeater.Email.Equals(emailDestinatario, StringComparison.CurrentCultureIgnoreCase));

            if (null == emailJaExiste)
            {
                // email nao existe, entao cria
                DadosRepeater.Add(new RepeaterEmail
                {
                    Guid = Guid.NewGuid(),
                    Email = emailDestinatario,
                    FlagInativo = false
                });
            }
            else
                // email ja existe, entao pega ele e flaga como ativo
                emailJaExiste.FlagInativo = false;

            // como houve mudança nos dados do repeater, faz o attach
            AtacharDadosAoRepeater();

            //Verifica a quantidade de emails adicionados no repeater
            if (NaoUltrapassouQtdeMaximaEmails())
                SetarFocoCaixaEmail();
            else
                DesabilitarCaixaEmail();
        }
        #endregion

        #endregion

        #region Delegates
        public delegate void delegateEnviarConfirmacao(List<string> emailsDestinatarios);
        public event delegateEnviarConfirmacao EnviarConfirmacao;
        #endregion

        #region Métodos

        #region InicializarComponente
        private void InicializarComponente()
        {

        }
        #endregion

        #region Inicializar
        /// <summary>
        /// Inicializar para enviar emails
        /// </summary>
        /// <param name="idUsuarioFilialPerfil">UsuarioFilialPerfil do usuario logado</param>
        /// <param name="idVaga">Vaga</param>
        /// <param name="urlVaga">Url da Vaga</param>
        public void Inicializar(int idUsuarioFilialPerfil, int idVaga, string urlVaga)
        {
            lblTituloModal.Text = "";
            lblSucesso.Text = "E-mail enviado com sucesso";
            IdUsuarioFilialPerfil = idUsuarioFilialPerfil;
            IdVaga = idVaga;
            UrlVaga = urlVaga;

            if (NaoUltrapassouQtdeMaximaEmails())
                SetarFocoCaixaEmail();
            else
                DesabilitarCaixaEmail();
        }

        /// <summary>
        /// Inicializar para mostrar "publicado com sucesso"
        /// </summary>
        public void Inicializar()
        {
            lblTituloModal.Text = "";
            lblSucesso.Text = "Vaga Compartilhada com sucesso!";
            pnlEmailsDestinatario.Visible = false;
            pnlSucesso.Visible = true;
        }
        #endregion

        #region Resetar
        public void Resetar()
        {
            DadosRepeater.Clear();
            AtacharDadosAoRepeater();
        }
        #endregion

        #region MostrarModal
        public void MostrarModal()
        {
            mpeCompartilhamentoVaga.Show();
        }
        #endregion

        #region FecharModal
        public void FecharModal()
        {
            mpeCompartilhamentoVaga.Hide();

            // restaura visibilidade dos paineis dinamicos
            pnlEmailsDestinatario.Visible = true;
            pnlSucesso.Visible = false;
        }
        #endregion

        #region EnviarMensagem
        /// <summary>
        /// Método responsável por enviar mensagem
        /// </summary>
        public int EnviarMensagem()
        {
            int qtdeEmails = 0;

            PessoaFisica objPessoaFisica = BLL.PessoaFisica.LoadObject(IdPessoaFisicaLogada.Value);

            Vaga objVaga =
                new Vaga(IdVaga);

            var repeaters =
                DadosRepeater
                    .Where(repeater => !repeater.FlagInativo);

            foreach (var repeater in repeaters)
            {
                MensagemCS.EnviarCompartilhamentoVaga(objPessoaFisica, objVaga, UrlVaga, repeater.Email);
                qtdeEmails++;
            }

            return qtdeEmails;
        }
        #endregion

        #region SetarFocoCaixaEmail
        private void SetarFocoCaixaEmail()
        {
            txtEmailDestinatario.Enabled = btnAdicionarEmail.Enabled = true;
            txtEmailDestinatario.Text = String.Empty;
            txtEmailDestinatario.Focus();
        }
        #endregion

        #region DesabilitarCaixaEmail
        private void DesabilitarCaixaEmail()
        {
            txtEmailDestinatario.Enabled = btnAdicionarEmail.Enabled = false;
        }
        #endregion

        #region AtacharDadosAoRepeater
        private void AtacharDadosAoRepeater()
        {
            rptEmails.DataSource =
                DadosRepeater
                    .Where(repeater => !repeater.FlagInativo)
                    .OrderBy(repeater => repeater.Email);

            rptEmails.DataBind();
        }
        #endregion

        #region NaoUltrapassouQtdeMaximaEmails
        private bool NaoUltrapassouQtdeMaximaEmails()
        {
            return DadosRepeater.Count(repeater => !repeater.FlagInativo) < MAXIMA_QTDE_EMAILS;
        }
        #endregion

        #endregion
    }
}