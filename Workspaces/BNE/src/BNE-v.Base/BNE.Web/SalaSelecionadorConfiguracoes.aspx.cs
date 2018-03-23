using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Resources;
using Resources;
using System;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web
{
    public partial class SalaSelecionadorConfiguracoes : BasePage
    {

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                Inicializar();

            ucConfirmacaoExclusao.Confirmar += ucConfirmacaoExclusao_Confirmar;
        }
        #endregion

        #region btnSalvar_Click
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(reExclusaoCandidatura.Text))
                {
                    ucConfirmacaoExclusao.Inicializar("Atenção!", "Tem certeza que deseja cancelar o envio do retorno para os candidatos excluído das suas vagas?");
                    ucConfirmacaoExclusao.MostrarModal();
                }
                else
                {
                    Salvar();
                    ExibirMensagem(MensagemAviso._100001, TipoMensagem.Aviso);
                }

            }
            catch (Exception ex)
            {
                ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region ucConfirmacaoExclusao_Confirmar
        void ucConfirmacaoExclusao_Confirmar()
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

        #region Métodos

        #region Inicializar
        public void Inicializar()
        {
            AjustarPermissoes();
            PreencherCampos();
            InicializarBarraBusca(TipoBuscaMaster.Curriculo, false, "SalaSelecionarConfiguracoes");
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
                var permissoes = UsuarioFilialPerfil.CarregarPermissoes(base.IdUsuarioFilialPerfilLogadoEmpresa.Value, BLL.Enumeradores.CategoriaPermissao.SalaSelecionadora);

                if (!permissoes.Contains((int)BLL.Enumeradores.Permissoes.SalaSelecionadora.AcessarTelaSalaSelecionadora))
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
            var objFilial = new Filial(base.IdFilial.Value);

            #region Carta Exclusao Candidato Candidatura
            ParametroFilial objParametroFilial;
            if (!ParametroFilial.CarregarParametroPorFilial(Enumeradores.Parametro.EnviaCartaAgradecimentoCandidatura, objFilial, out objParametroFilial, null))
                objParametroFilial = new ParametroFilial { IdParametro = (int)Enumeradores.Parametro.EnviaCartaAgradecimentoCandidatura, IdFilial = base.IdFilial.Value };

            objParametroFilial.ValorParametro = string.IsNullOrEmpty(reExclusaoCandidatura.Content) ? string.Empty : UIHelper.IncluirTemplateRadEditor(reExclusaoCandidatura.Content);
            objParametroFilial.Save();
            #endregion Carta Exclusao Candidato Candidatura

            #region CartaApresentacao
            ParametroFilial objParametroFilialApresentacao;
            if (!ParametroFilial.CarregarParametroPorFilial(Enumeradores.Parametro.CartaApresentacao, objFilial, out objParametroFilialApresentacao))
                objParametroFilialApresentacao = new ParametroFilial { IdFilial = objFilial.IdFilial, IdParametro = (int)Enumeradores.Parametro.CartaApresentacao };

            objParametroFilialApresentacao.ValorParametro = reCartaApresentacao.Text;
            objParametroFilialApresentacao.Save();
            #endregion CartaApresentacao

            #region CartaAgradecimento
            ParametroFilial objParametroFilialAgradecimento;
            if (!ParametroFilial.CarregarParametroPorFilial(Enumeradores.Parametro.CartaAgradecimentoCandidatura, objFilial, out objParametroFilialAgradecimento))
                objParametroFilialAgradecimento = new ParametroFilial { IdFilial = objFilial.IdFilial, IdParametro = (int)Enumeradores.Parametro.CartaAgradecimentoCandidatura };

            objParametroFilialAgradecimento.ValorParametro = reAgradecimentoCandidatura.Text;
            objParametroFilialAgradecimento.Save();
            #endregion CartaAgradecimento

        }
        #endregion

        #region PreencherCampos
        private void PreencherCampos()
        {
            var objFilial = new Filial(base.IdFilial.Value);
            ParametroFilial objParametroFilial;
            if (ParametroFilial.CarregarParametroPorFilial(Enumeradores.Parametro.EnviaCartaAgradecimentoCandidatura, objFilial, out objParametroFilial, null))
                reExclusaoCandidatura.Content = UIHelper.ExtrairTemplateRadEditor(objParametroFilial.ValorParametro);
            else
                reExclusaoCandidatura.Content = UIHelper.ExtrairTemplateRadEditor(CartaEmail.RecuperarConteudo(Enumeradores.CartaEmail.AgradecimentoCandidatura));

            ParametroFilial objParametroFilialAgradecimento;
            if (ParametroFilial.CarregarParametroPorFilial(Enumeradores.Parametro.CartaAgradecimentoCandidatura, objFilial, out objParametroFilialAgradecimento, null))
                reAgradecimentoCandidatura.Content = objParametroFilialAgradecimento.ValorParametro;

            ParametroFilial objParametroFilialApresentacao;
            if (ParametroFilial.CarregarParametroPorFilial(Enumeradores.Parametro.CartaApresentacao, objFilial, out objParametroFilialApresentacao, null))
                reCartaApresentacao.Content = objParametroFilialApresentacao.ValorParametro;
        }
        #endregion

        #endregion

    }
}