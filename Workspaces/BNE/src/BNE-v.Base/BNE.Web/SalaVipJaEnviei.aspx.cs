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
    public partial class SalaVipJaEnviei : BasePage
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

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();
            ucJaEnviei.EventVerEmpresa += ucJaEnviei_EventVerEmpresa;
            ucJaEnviei.EventEmprimir += ucJaEnviei_EventEmprimir;
            PropriedadeAjustarTopoUsuarioCandidato(true);
        }
        #endregion

        #region ucJaEnviei_EventVerEmpresa
        void ucJaEnviei_EventVerEmpresa(int idFilial, bool flagConfidencial, int idVaga)
        {
            ucVerDadosEmpresa.FlagConfidencial = flagConfidencial;
            ucVerDadosEmpresa.IdFilial = idFilial;
            Vaga objVaga = Vaga.LoadObject(idVaga);
            ucVerDadosEmpresa.MostrarModal(objVaga);
        }
        #endregion

        #region  ucJaEnviei_EventEmprimir
        void ucJaEnviei_EventEmprimir(int idVaga)
        {
            //TODO: O que faz isso?

            Vaga objVaga = Vaga.LoadObject(idVaga);
            Curriculo objCurriculo = Curriculo.LoadObject(base.IdCurriculo.Value);
            VagaCandidato objVagaCandidato;
            if (VagaCandidato.CarregarPorVagaCurriculo(objVaga.IdVaga, objCurriculo.IdCurriculo, out objVagaCandidato))
            {
                string protocolo = String.Format("{0}{1}{2}", objCurriculo.IdCurriculo, objVaga.CodigoVaga, objVagaCandidato.DataCadastro.ToString("yyyyMMdd"));
                ucModalConfirmacaoEnvioCurriculo.Inicializar(objCurriculo.PessoaFisica.PrimeiroNome, protocolo);
                ucModalConfirmacaoEnvioCurriculo.MostrarModal();
            }
        }
        #endregion

        #endregion

        #region Metodos

        #region Inicializar
        public void Inicializar()
        {
            AjustarPermissoes();

            AjustarTituloTela("Já Enviei");
            InicializarBarraBusca(TipoBuscaMaster.Vaga, true, "Já Enviei");
            ucJaEnviei.Inicializar();
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