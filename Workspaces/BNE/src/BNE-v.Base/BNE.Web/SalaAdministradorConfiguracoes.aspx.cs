using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Resources;
using Resources;

namespace BNE.Web
{
    public partial class SalaAdministradorConfiguracoes : BasePage
    {

        #region Propriedades

        #region Permissoes - Variável Permissoes
        /// <summary>
        /// Propriedade que armazena e recupera as Permissoes
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

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();

            ucUsuarios.Voltar += ucUsuarios_Voltar;
        }
        #endregion

        #region btlEmail_Click
        protected void btlEmail_Click(object sender, EventArgs e)
        {
            ucEmailPadrao.Inicializar();
            AjustarTodosPaineis(pnlEmailPadrao);
        }
        #endregion

        #region btlSMS_Click
        protected void btlSMS_Click(object sender, EventArgs e)
        {
            ucSMS.Inicializar();
            AjustarTodosPaineis(pnlSMS);
        }
        #endregion

        #region btlValores_Click
        protected void btlValores_Click(object sender, EventArgs e)
        {
            ucConfiguracoesValores.Inicializar();
            AjustarTodosPaineis(pnlConfiguracoesValores);
        }
        #endregion

        #region btlEmailRetorno_Click
        protected void btlEmailRetorno_Click(object sender, EventArgs e)
        {
            ucEmailRetorno.Inicializar();
            AjustarTodosPaineis(pnlEmailRetorno);
        }
        #endregion

        #region btlNoticias_Click
        protected void btlNoticias_Click(object sender, EventArgs e)
        {
            ucNoticias.Inicializar();
            AjustarTodosPaineis(pnlNoticias);
        }
        #endregion

        #region btlEmailParaFilial_Click
        protected void btlEmailParaFilial_Click(object sender, EventArgs e)
        {
            ucEmailParaFilial.Inicializar();
            AjustarTodosPaineis(pnlEmailParaFilial);
        }
        #endregion

        #region btnVoltar_Click
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            if (ucNoticias.EmEdicao)
            {
                ucNoticias.Inicializar();
                AjustarTodosPaineis(pnlNoticias);
            }
            else
                Redirect("SalaAdministrador.aspx");
        }
        #endregion

        #region ucUsuarios_Voltar
        void ucUsuarios_Voltar()
        {
            AjustarTodosPaineis(pnlIndex);
        }
        #endregion

        #region btlUsuarios_Click
        protected void btlUsuarios_Click(object sender, EventArgs e)
        {
            ucUsuarios.Inicializar();
            AjustarTodosPaineis(pnlUsuarios);
        }
        #endregion
        
        #endregion

        #region Métodos

        private void Inicializar()
        {
            AjustarPermissoes();
            AjustarTituloTela("Configurações");
            InicializarBarraBusca(TipoBuscaMaster.Vaga, false, "Configurações");
            AjustarTodosPaineis(pnlIndex);
        }

        #region AjustarPermissoes
        /// <summary>
        /// Método responsável por ajustar as permissões da tela de acordo com o susuário filial perfil logado.
        /// </summary>
        private void AjustarPermissoes()
        {
            if (base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
            {
                Permissoes = UsuarioFilialPerfil.CarregarPermissoes(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value, BLL.Enumeradores.CategoriaPermissao.Administrador);

                if (!Permissoes.Contains((int)BLL.Enumeradores.Permissoes.Administrador.AcessarTelaSalaAdministradorConfiguracoes))
                {
                    Session.Add(Chave.Temporaria.MensagemPermissao.ToString(), MensagemAviso._300034);
                    Redirect(Configuracao.UrlAvisoAcessoNegado);
                }
            }
            else
                Redirect(Configuracao.UrlAvisoAcessoNegado);
        }
        #endregion

        private void AjustarTodosPaineis(Panel pnlVisivel)
        {
            pnlIndex.Visible = false;
            pnlSMS.Visible = false;
            pnlNoticias.Visible = false;
            pnlEmailPadrao.Visible = false;
            pnlConfiguracoesValores.Visible = false;
            pnlEmailRetorno.Visible = false;
            pnlUsuarios.Visible = false;
            pnlEmailParaFilial.Visible = false;

            if (pnlVisivel != null)
                pnlVisivel.Visible = true;

            upPnlIndex.Update();
            upPnlSMS.Update();
            upPnlNoticias.Update();
            upPnlEmailPadrao.Update();
            upPnlConfiguracoesValores.Update();
            upPnlEmailRetorno.Update();
            upPnlUsuarios.Update();
            upPnlEmailParaFilial.Update();
        }

        #endregion

    }
}