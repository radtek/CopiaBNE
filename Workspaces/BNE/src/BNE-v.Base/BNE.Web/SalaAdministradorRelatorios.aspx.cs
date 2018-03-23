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
    public partial class SalaAdministradorRelatorios : BasePage
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
        }
        #endregion

        #region btlPSalarial_Click
        protected void btlPSalarial_Click(object sender, EventArgs e)
        {
            Redirect("RelatorioAmplitudeSalarial.aspx");
        }
        #endregion

        #region btlNovasEmpresas_Click
        protected void btlNovasEmpresas_Click(object sender, EventArgs e)
        {
            AjustarTodosPaineis(PnlNvsEmp);
        }
        #endregion

        #region btnVoltar_Click
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Redirect("SalaAdministrador.aspx");
        }
        #endregion

        #endregion

        #region Métodos

        private void Inicializar()
        {
            AjustarPermissoes();
            AjustarTituloTela("Relatórios");
            InicializarBarraBusca(TipoBuscaMaster.Vaga, false, "Relatórios");
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
            PnlNvsEmp.Visible = false;

            if (pnlVisivel != null)
                pnlVisivel.Visible = true;

            upPnlIndex.Update();
            upPnlNvsEmp.Update();
        }

        #endregion

    }
}