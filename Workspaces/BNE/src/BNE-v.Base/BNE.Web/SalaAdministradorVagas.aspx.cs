﻿using System;
using System.Collections.Generic;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Resources;
using Resources;

namespace BNE.Web
{
    public partial class SalaAdministradorVagas : BasePage
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

        #region Metodos

        #region Inicializar
        public void Inicializar()
        {
            AjustarPermissoes();
            AjustarTituloTela("Vagas");
            InicializarBarraBusca(TipoBuscaMaster.Vaga, false, "Vagas");
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

                if (!Permissoes.Contains((int)BLL.Enumeradores.Permissoes.Administrador.AcessarTelaSalaAdministradorVagas))
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

            ucVagas.EventExcluir += ucMinhasVagas_EventExcluir;
            ucConfirmacaoExclusao.Confirmar += ucConfirmacaoExclusao_Confirmar;
        }
        #endregion 

        #region ucConfirmacaoExclusao_Confirmar
        void ucConfirmacaoExclusao_Confirmar()
        {
            ucVagas.ExcluirVaga(base.IdVaga.Value);            
        }
        #endregion

        #region ucVagasArquivadas_EventExcluir
        void ucMinhasVagas_EventExcluir(int idVaga)
        {
            base.IdVaga.Value = idVaga;
            ucConfirmacaoExclusao.Inicializar("Atenção!", "Tem certeza que deseja excluir esta vaga?");
            ucConfirmacaoExclusao.MostrarModal();
        }
        #endregion

        #endregion 
    }
}