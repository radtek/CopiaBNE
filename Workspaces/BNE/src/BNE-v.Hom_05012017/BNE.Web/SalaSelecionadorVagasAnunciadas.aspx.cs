using System;
using System.Collections.Generic;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Resources;
using Resources;
using Enumeradores = BNE.BLL.Enumeradores;
using BNE.BLL.Custom.Email;

namespace BNE.Web
{
    public partial class SalaSelecionadorVagasAnunciadas : BasePage
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

        #region IdVaga - Variavel1
        /// <summary>
        /// Propriedade que armazena e recupera o IdPesquisaCurriculo
        /// </summary>
        protected int IdVaga
        {
            get
            {
                return Convert.ToInt32(ViewState[Chave.Temporaria.Variavel1.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel1.ToString(), value);
            }
        }
        #endregion

        #region TituloTelaCVsRecebidos - Variável 13
        /// <summary>
        /// </summary>
        public bool TituloTelaCVsRecebidos
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel13.ToString()] != null)
                    return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel13.ToString()]);
                return false;
            }
        }
        #endregion

        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AjustarPermissoes();

                if (base.STC.Value)
                {
                    if (TituloTelaCVsRecebidos)
                        AjustarTituloTela("CVs Recebidos");
                    else
                        AjustarTituloTela("Administre aqui suas vagas");
                }
                else
                {
                    if (TituloTelaCVsRecebidos)
                    {
                        AjustarTituloTela("CVs Recebidos");
                        ExibirMenuSecaoEmpresa();
                    }
                    else
                    {
                        AjustarTituloTela("Administre aqui suas vagas");
                        ExibirMenuSecaoEmpresa();
                    }
                }
            }

            #region Envio de email para empresas que não tem vagas cadastradas

            int qtdVagasAnunciadas = Vaga.RecuperarQtdVagasFilial(base.IdFilial.Value);

            if (qtdVagasAnunciadas == 0) 
            {
                string assunto = string.Empty;
                string mensagem = ConteudoHTML.RecuperaValorConteudo(Enumeradores.ConteudoHTML.EdicaoCadastroVaga);
                string emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.ContaPadraoEnvioEmail);



                var objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(base.IdUsuarioFilialPerfilLogadoEmpresa.Value);
                objUsuarioFilialPerfil.PessoaFisica.CompleteObject();

                EmailSenderFactory
                    .Create(TipoEnviadorEmail.Fila)
                    .Enviar(null, null, objUsuarioFilialPerfil, assunto, mensagem, null, emailRemetente, objUsuarioFilialPerfil.PessoaFisica.EmailPessoa);
            }

            #endregion

            InicializarBarraBusca(TipoBuscaMaster.Curriculo, false, "SalaSelecionadorVagasAnunciadas");
            ucMinhasVagas.EventExcluir += ucMinhasVagas_EventExcluir;
            ucConfirmacaoExclusao.Confirmar += ucConfirmacaoExclusao_Confirmar;
        }
        #endregion

        #region ucVagasArquivadas_EventExcluir
        void ucMinhasVagas_EventExcluir(int idVaga)
        {
            IdVaga = idVaga;
            ucConfirmacaoExclusao.Inicializar("Atenção!", "Tem certeza que deseja excluir este registro?");
            ucConfirmacaoExclusao.MostrarModal();
        }
        #endregion

        #region ucConfirmacaoExclusao_Confirmar
        void ucConfirmacaoExclusao_Confirmar()
        {
            ucMinhasVagas.ExcluirVaga(IdVaga);
            ucMinhasVagas.CarregarGridVagas();
            ucMinhasVagas.CarregarGridVagasCampanha();
            ucMinhasVagas.CarregarComboFuncoes();
            ucConfirmacaoExclusao.FecharModal();
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
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.LoginComercialEmpresa.ToString(), new { Destino = Enumeradores.LoginEmpresaDestino.VagasAnunciadas }));
        }
        #endregion

    }
}