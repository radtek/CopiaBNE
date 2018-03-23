using System;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web
{
    public partial class CadastroCurriculoRevisao : BasePage
    {

        #region IdPessoaFisica - Variável 1
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Pessoa.
        /// </summary>
        protected int? IdPessoaFisica
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel1.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel1.ToString()].ToString());

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

        #region MostrarDegustacaoCandidatura
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Pessoa.
        /// </summary>
        public bool? MostrarDegustacaoCandidatura
        {
            get
            {
                if (Session[Chave.Permanente.MostrarModalDegustacaoCandidatura.ToString()] != null)
                    return Boolean.Parse(Session[Chave.Permanente.MostrarModalDegustacaoCandidatura.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    Session.Add(Chave.Permanente.MostrarModalDegustacaoCandidatura.ToString(), value);
                else
                    Session.Remove(Chave.Permanente.MostrarModalDegustacaoCandidatura.ToString());
            }
        }
        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IdPessoaFisica.HasValue)
                ucConferirDados.IdPessoaFisica = IdPessoaFisica;
            else
                Redirect(Page.GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoMini.ToString(), null));

            ucConferirDados.DadosSalvosSucesso += ucConferirDados_DadosSalvosSucesso;
            ucModalConfirmacao.eventFechar += ucModalConfirmacao_FecharModal;
            ucModalDegustacaoCandidatura.UsarBonus += ucModalDegustacaoCandidatura_UsarBonus;
        }
        #endregion

        #region ucConferirDados_DadosSalvosSucesso
        /// <summary>
        /// Evento acionado quando o cadastro é salvo com sucesso.
        /// </summary>
        void ucConferirDados_DadosSalvosSucesso()
        {
            ucModalConfirmacao.PreencherCampos("Confirmação de cadastro", "Seu currículo foi cadastrado com sucesso!", false);
            ucModalConfirmacao.MostrarModal();
        }
        #endregion

        #endregion

        #region Métodos

        #region ucModalConfirmacao_FecharModal
        void ucModalConfirmacao_FecharModal()
        {
            if (IdPessoaFisica.HasValue)
            {
                if (MostrarDegustacaoCandidatura.HasValue && MostrarDegustacaoCandidatura.Value)
                {
                    ucModalDegustacaoCandidatura.Inicializar(true, false, true, null);
                    MostrarDegustacaoCandidatura = null;
                }
                else
                    base.RedirecionarCandidatoPesquisaVaga(new PessoaFisica(IdPessoaFisica.Value));
            }
        }
        #endregion

        #region ucModalDegustacaoCandidatura_UsarBonus
        void ucModalDegustacaoCandidatura_UsarBonus()
        {
            if (IdPessoaFisica.HasValue)
            {
                base.RedirecionarCandidatoPesquisaVaga(new PessoaFisica(IdPessoaFisica.Value));
            }
        }
        #endregion

        #endregion

    }
}