using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Resources;
using Resources;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web
{
    public partial class SalaSelecionadorMensagens : BasePage
    {

        #region Propriedades

        #region UrlOrigem - Variável 1
        /// <summary>
        /// </summary>
        public string UrlOrigem
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel1.ToString()] != null)
                    return (ViewState[Chave.Temporaria.Variavel1.ToString()]).ToString();
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

        #region RedirectUrlOrigem - Variável 2
        /// <summary>
        /// </summary>
        public bool RedirectUrlOrigem
        {
            get
            {
                return (Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel2.ToString()]));
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel2.ToString(), value);
            }
        }
        #endregion

        #region PropriedadeTipoMensagem - Variável 3
        /// <summary>
        /// </summary>
        public TipoMensagemSala PropriedadeTipoMensagem
        {
            get
            {
                return (TipoMensagemSala)(ViewState[Chave.Temporaria.Variavel3.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel3.ToString(), value);
            }
        }
        #endregion

        #region Permissoes - Variável Permissoes
        /// <summary>
        /// Propriedade que armazena e recupera o IdPesquisaCurriculo
        /// </summary>
        protected List<int> Permissoes
        {
            get
            {
                return (List<int>)ViewState[Chave.Temporaria.Permissoes.ToString()];
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Permissoes.ToString(), value);
            }
        }
        #endregion

        #region DesPesquisa - Variável 4
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public string DesPesquisa
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel4.ToString()] != null)
                    return ViewState[Chave.Temporaria.Variavel4.ToString()].ToString();
                return string.Empty;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel4.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel4.ToString());
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                Inicializar();
            ucMensagensEnviadas.EventCorpoMensagensEnviadas += ucMensagens_CorpoMensagem;
            ucCorpoMensagem.EventoBotaoVoltar += ucCorpoMensagem_EventoBotaoVoltar;
            ucCorpoMensagem.EventoDownloadAnexo += ucCorpoMensagem_Download;
        }

        #endregion

        #region ucCorpoMensagem_EventoBotaoVoltar
        /// <summary>
        /// Muda o redirecionamento do redirect do botão
        /// </summary>
        void ucCorpoMensagem_EventoBotaoVoltar(TipoMensagemSala tipoMensagem, string desPesquisa)
        {
            PropriedadeTipoMensagem = tipoMensagem;
            DesPesquisa = desPesquisa;
            RedirectUrlOrigem = false;
        }
        #endregion

        #region ucCorpoMensagem_Download
        /// <summary>
        /// Abre a janela de download do arquivo anexo
        /// </summary>
        void ucCorpoMensagem_Download(string url)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AbrirPopup", string.Format("AbrirPopup('{0}', 600, 800);", url), true);
        }
        #endregion

        #region ucMensagens_CorpoMensagem
        void ucMensagens_CorpoMensagem(int idMensagemCS, TipoMensagemSala tipoMensagem, string desPesquisa)
        {
            pnlMensagensEnviadas.Visible = false;
            pnlCorpoMensagem.Visible = true;
            ucCorpoMensagem.IdMensagemCS = idMensagemCS;
            ucCorpoMensagem.IdUsuarioFilialPerfil = base.IdUsuarioFilialPerfilLogadoEmpresa.Value;
            ucCorpoMensagem.DesPesquisa = desPesquisa;
            ucCorpoMensagem.PropriedadeTipoMensagem = tipoMensagem;
            ucCorpoMensagem.PropriedadeMensagemSalaOrigem = MensagemSalaOrigem.SalaSelecionador;
            ucCorpoMensagem.Inicializar();
            upSalaSelecionadoraMensagem.Update();
        }
        #endregion

        #region lkMensagensEnviadas_Click
        protected void lkMensagensEnviadas_Click(object sender, EventArgs e)
        {
            pnlMensagensEnviadas.Visible = true;
            pnlCorpoMensagem.Visible = false;
            liMensagensEnviadas.Attributes["class"] = "selected";
            ucMensagensEnviadas.LimparCampo();
            ucMensagensEnviadas.DesPesquisa = string.Empty;
            ucMensagensEnviadas.Inicializar();
            RedirectUrlOrigem = true;
            upSalaSelecionadoraMensagem.Update();
        }
        #endregion

        #region btnVoltar_Click
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            if (RedirectUrlOrigem)
            {
                if (!string.IsNullOrEmpty(UrlOrigem))
                    Redirect(UrlOrigem);
                else
                    Redirect("Default.aspx");
            }
            else
            {
                RedirectUrlOrigem = true;
                if (PropriedadeTipoMensagem.Equals(TipoMensagemSala.MensagensEnviadas))
                {
                    lkMensagensEnviadas_Click(sender, e);
                    ucMensagensEnviadas.DesPesquisa = DesPesquisa;
                    ucMensagensEnviadas.CarregarCampo();
                    ucMensagensEnviadas.CarregarGrid();
                }
            }
            upSalaSelecionadoraMensagem.Update();
        }
        #endregion

        #endregion

        #region Metodos

        #region Inicializar
        public void Inicializar()
        {
            AjustarPermissoes();

            InicializarBarraBusca(TipoBuscaMaster.Vaga, false, "SalaSelecionadoraMensagens");
            AjustarTituloTela("Mensagens");

            if (Request.UrlReferrer != null)
                UrlOrigem = Request.UrlReferrer.AbsoluteUri;

            //Controle do evento do botão pois o user control do corpo de mensagem usa o mesmo botão,
            //mas a função é diferente
            RedirectUrlOrigem = true;
            pnlMensagensEnviadas.Visible = true;
            pnlCorpoMensagem.Visible = false;
            ucMensagensEnviadas.DesPesquisa = string.Empty;
            ucMensagensEnviadas.Inicializar();
            lkMensagensEnviadas.Text = "Mensagens Enviadas (" + ucMensagensEnviadas.GetTotalMensagem() + ")";
            
            upSalaSelecionadoraMensagem.Update();
        }
        #endregion

        #region AjustarPermissoes
        /// <summary>
        /// Método responsável por ajustar as permissões da tela de acordo com o susuário filial perfil logado.
        /// </summary>
        private void AjustarPermissoes()
        {
            if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
            {
                Permissoes = UsuarioFilialPerfil.CarregarPermissoes(base.IdUsuarioFilialPerfilLogadoEmpresa.Value, BLL.Enumeradores.CategoriaPermissao.SalaSelecionadora);

                if (!Permissoes.Contains((int)BLL.Enumeradores.Permissoes.SalaSelecionadora.AcessarTelaSalaSelecionadora))
                {
                    Session.Add(Chave.Temporaria.MensagemPermissao.ToString(), MensagemAviso._300034);
                    Redirect(Configuracao.UrlAvisoAcessoNegado);
                }
            }
            else
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.LoginComercialEmpresa.ToString(), null));
        }
        #endregion

        #endregion

    }
}