using System;
using System.Collections.Generic;
using System.Globalization;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Resources;
using Resources;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web
{
    public partial class SalaVipMensagens : BasePage
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

        #region PageIndex - PageIndex
        /// <summary>
        /// Propriedade que armazena e recupera o PageIndex 
        /// </summary>
        public int PageIndex
        {
            get
            {
                if (ViewState[Chave.Temporaria.PageIndex.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.PageIndex.ToString()].ToString());
                return 0;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.PageIndex.ToString(), value);
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
            ucMensagensRecebidas.EventCorpoMensagensRecebidas += ucMensagens_CorpoMensagem;
            ucCorpoMensagem.EventoBotaoVoltar += ucCorpoMensagem_EventoBotaoVoltar;
        }

        #endregion

        #region ucCorpoMensagem_EventoBotaoVoltar
        void ucCorpoMensagem_EventoBotaoVoltar(TipoMensagemSala tipoMensagem, string desPesquisa)
        {
            PropriedadeTipoMensagem = tipoMensagem;
            DesPesquisa = desPesquisa;
            RedirectUrlOrigem = false;
        }
        #endregion

        #region ucMensagens_CorpoMensagem
        void ucMensagens_CorpoMensagem(int idMensagemCS, int pageIndexGridMensagem, TipoMensagemSala tipoMensagem, string desPesquisa)
        {
            pnlMensagensRecebidas.Visible = pnlMensagensRecebidas.Visible = false;
            pnlCorpoMensagem.Visible = true;
            PageIndex = pageIndexGridMensagem;
            ucCorpoMensagem.IdMensagemCS = idMensagemCS;
            ucCorpoMensagem.DesPesquisa = desPesquisa;
            ucCorpoMensagem.IdUsuarioFilialPerfil = base.IdUsuarioFilialPerfilLogadoCandidato.Value;
            ucCorpoMensagem.PropriedadeTipoMensagem = tipoMensagem;
            ucCorpoMensagem.PropriedadeMensagemSalaOrigem = MensagemSalaOrigem.SalaVip;
            ucCorpoMensagem.Inicializar();
            upSalaVipMensagem.Update();
        }
        #endregion

        #region lkMensagensRecebidas_Click
        protected void lkMensagensRecebidas_Click(object sender, EventArgs e)
        {
            pnlMensagensRecebidas.Visible = true;
            pnlCorpoMensagem.Visible = false;
            liMensagensRecebidas.Attributes["class"] = "selected";
            ucMensagensRecebidas.DesPesquisa = string.Empty;
            ucMensagensRecebidas.LimparCampo();
            ucMensagensRecebidas.Inicializar(base.IdUsuarioFilialPerfilLogadoCandidato.Value);
            RedirectUrlOrigem = true;
            upSalaVipMensagem.Update();
        }
        #endregion

        #region btnVoltar_Click
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            if (RedirectUrlOrigem)
            {
                Redirect(!string.IsNullOrEmpty(UrlOrigem) ? UrlOrigem : "Default.aspx");
            }
            else
            {
                RedirectUrlOrigem = true;
                lkMensagensRecebidas_Click(sender, e);
                ucMensagensRecebidas.PageIndex = PageIndex;
                ucMensagensRecebidas.DesPesquisa = DesPesquisa;
                ucMensagensRecebidas.CarregarCampo();
                ucMensagensRecebidas.CarregarGrid();
            }
            upSalaVipMensagem.Update();
        }
        #endregion

        #endregion

        #region Metodos

        #region Inicializar
        public void Inicializar()
        {
            AjustarPermissoes();

            InicializarBarraBusca(TipoBuscaMaster.Vaga, false, "SalaVipMensagens");
            AjustarTituloTela("Mensagens");

            if (Request.UrlReferrer != null)
                UrlOrigem = Request.UrlReferrer.AbsoluteUri;

            RedirectUrlOrigem = true;

            lkMensagensRecebidas.Text = string.Format("Mensagens Recebidas ({0})", MensagemCS.QuantidadeMensagensRecebidas(base.IdUsuarioFilialPerfilLogadoCandidato.Value).ToString(CultureInfo.CurrentCulture));
            pnlMensagensRecebidas.Visible = true;
            pnlCorpoMensagem.Visible = false;
            ucMensagensRecebidas.EnumMensagemSalaOrigem = MensagemSalaOrigem.SalaVip;
            ucMensagensRecebidas.DesPesquisa = string.Empty;
            ucMensagensRecebidas.Inicializar(base.IdUsuarioFilialPerfilLogadoCandidato.Value);
            upSalaVipMensagem.Update();
        }
        #endregion

        #region AjustarPermissoes
        /// <summary>
        /// Método responsável por ajustar as permissões da tela de acordo com o usuário filial perfil logado.
        /// </summary>
        private void AjustarPermissoes()
        {
            if (base.IdUsuarioFilialPerfilLogadoCandidato.HasValue)
            {
                Permissoes = UsuarioFilialPerfil.CarregarPermissoes(base.IdUsuarioFilialPerfilLogadoCandidato.Value, Enumeradores.CategoriaPermissao.SalaVIP);

                if (!Permissoes.Contains((int)Enumeradores.Permissoes.SalaVIP.AcessarTelaSalaVIP))
                {
                    Session.Add(Chave.Temporaria.MensagemPermissao.ToString(), MensagemAviso._300034);
                    Redirect(Configuracao.UrlAvisoAcessoNegado);
                }
            }
            else
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.LoginComercialCandidato.ToString(), null));
        }
        #endregion

        #endregion

    }
}