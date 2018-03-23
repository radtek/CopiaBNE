using System;
using System.Collections.Generic;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Resources;
using Resources;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web
{
    public partial class SiteTrabalheConoscoSalaSelecionadorConteudos : BasePage
    {
        #region Propriedades

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

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                Inicializar();
        }

        #endregion

        #region Métodos

        #region Inicializar
        public void Inicializar()
        {
            AjustarPermissoes();

            PreencherCampos();

            InicializarBarraBusca(TipoBuscaMaster.Curriculo, false, "SiteTrabalheConosco");
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

        #region Salvar
        private void Salvar()
        {
            OrigemFilial objOrigemFilial;
            if (OrigemFilial.CarregarPorOrigem(base.IdOrigem.Value, out objOrigemFilial))
            {
                objOrigemFilial.DescricaoPaginaInicial = rePaginaInicial.Content;
                objOrigemFilial.DescricaoMensagemCandidato = reBoasVindasCandidato.Content;
                objOrigemFilial.Save();
            }
        }
        #endregion

        #region PreencherCampos
        private void PreencherCampos()
        {
            OrigemFilial objOrigemFilial;
            if (OrigemFilial.CarregarPorOrigem(base.IdOrigem.Value, out objOrigemFilial))
            {
                rePaginaInicial.Content = objOrigemFilial.DescricaoPaginaInicial;
                reBoasVindasCandidato.Content = objOrigemFilial.DescricaoMensagemCandidato;
            }
        }
        #endregion

        #region btnSalvar_Click
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                Salvar();

                ExibirMensagem(MensagemAviso._100001, TipoMensagem.Aviso);
            }
            catch (Exception ex)
            {
                ExibirMensagemErro(ex);
            }
        }
        #endregion

        #endregion
    }
}