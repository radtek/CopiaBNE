using System;
using System.Collections.Generic;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Resources;
using Resources;

namespace BNE.Web
{
    public partial class SalaAdministradorEdicaoCV : BasePage
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

        #region Métodos

        #region Inicializar
        public void Inicializar()
        {
            AjustarPermissoes();
            AjustarTituloTela("Edição de Currículos");
            ucEdicaoCV.Inicializar();
        }
        #endregion

        #region AjustarPermissoes
        /// <summary>
        /// Método responsável por ajustar as permissões da tela de acordo com o susuário filial perfil logado.
        /// </summary>
        private void AjustarPermissoes()
        {
            if (base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
            {
                Permissoes = UsuarioFilialPerfil.CarregarPermissoes(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value, BLL.Enumeradores.CategoriaPermissao.Administrador);

                if (!Permissoes.Contains((int)BLL.Enumeradores.Permissoes.Administrador.AcessarTelaSalaAdministradorEdicaoCV))
                {
                    Session.Add(Chave.Temporaria.MensagemPermissao.ToString(), MensagemAviso._300034);
                    Redirect(Configuracao.UrlAvisoAcessoNegado);
                }
            }
            else
                Redirect(Configuracao.UrlAvisoAcessoNegado);
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
                Inicializar();

            ucEdicaoCV.EventEditarCurriculo += ucEdicaoCV_EventEditarCurriculo;
            InicializarBarraBusca(TipoBuscaMaster.Vaga, false, "EdicaoCV");
        }
        #endregion

        #region ucEdicaoCV_EventEditarCurriculo
        void ucEdicaoCV_EventEditarCurriculo(int idCurriculo)
        {
            Session.Add(Chave.Temporaria.Variavel1.ToString(), idCurriculo);

            Redirect("CadastroCurriculoCompleto.aspx");
        }
        #endregion

        #endregion

    }
}