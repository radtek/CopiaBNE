using System.Threading;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.Web.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Resources;
using System.Globalization;

namespace BNE.Web.UserControls.Modais
{
    public partial class QueroContratarWebEstagios : BaseUserControl
    {
        #region [ Propriedades ]
        private int IdCurriculoVisualizacaoCurriculo
        {
            get
            {
                var value = ViewState[Chave.Temporaria.Variavel1.ToString()];
                return value != null ? Int32.Parse(value.ToString()) : -1;
            }
            set
            {
                ViewState[Chave.Temporaria.Variavel1.ToString()] = value;
            }
        }
        #endregion

        #region [ FecharModal ]
        public void FecharModal()
        {
            mpeEnvioEmail.Hide();
        }
        #endregion

        #region [ MostrarModal ]
        public void MostrarModal(int curriculoId)
        {
            IdCurriculoVisualizacaoCurriculo = curriculoId;
            mpeEnvioEmail.Show();
        }
        #endregion

        #region [ Eventos Tela ]
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Inicializar()
        {
            txtValorBolsa.ExpressaoValidacao = Configuracao.RegexValor;

            pnlContratacao.Visible = true;
            pnlEnviado.Visible = false;

            txtNomeDaMae.Text = String.Empty;
            txtBeneficios.Text = String.Empty;
            rdlValorList.SelectedIndex = -1;
            txtValorBolsa.Valor = String.Empty;

            upEnvioContratacao.Update();
        }

        protected void btnConfirmar_OnClick(object sender, EventArgs e)
        {
            int filialId;
            int curriculoId;
            if (!VerificaSeEnvioDeContratacaoValido(out filialId, out curriculoId))
                return;

            var dados = new QueroContratarCurriculoEstudante()
                {
                    Beneficios = txtBeneficios.Text,
                    NomeDaMae = txtNomeDaMae.Text,
                    ValorBolsa = Convert.ToDecimal(txtValorBolsa.Valor.Trim()),
                    TipoBolsa =
                        rdlValorList.SelectedIndex == 1
                            ? QueroContratarCurriculoEstudante.BolsaTipo.ValorPorHora
                            : QueroContratarCurriculoEstudante.BolsaTipo.ValorFixo
                };

            if (!EnviarEmailContratacao(dados, filialId, curriculoId))
                return;

            pnlContratacao.Visible = false;
            pnlEnviado.Visible = true;
        }

        protected void btiFechar_Click(object sender, ImageClickEventArgs e)
        {
            FecharModal();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            txtValorBolsa.ExpressaoValidacao = Configuracao.RegexValor;
        }
        #endregion

        private bool VerificaSeEnvioDeContratacaoValido(out int idFilial, out int idCurriculo)
        {
            var textoValorBolsa = txtValorBolsa.Valor;
            decimal valor;
            if (!decimal.TryParse(textoValorBolsa, NumberStyles.Currency, CultureInfo.GetCultureInfo("pt-br").NumberFormat, out valor))
            {
                idFilial = -1;
                idCurriculo = -1;
                ExibirMensagem("Valor da bolsa inválido, favor digitar em formato correto.", TipoMensagem.Aviso);
                return false;
            }

            idFilial = IdFilial.ValueOrDefault;
            if (idFilial <= 0)
            {
                idCurriculo = -1;
                ExibirMensagem("Sessão expirada, favor efetuar o login novamente.", TipoMensagem.Aviso);
                return false;
            }

            idCurriculo = IdCurriculoVisualizacaoCurriculo;

            if (idCurriculo <= 0)
            {
                ExibirMensagem("Sessão expirada ou currículo é inválido, favor acessar o contéudo novamente.",
                               TipoMensagem.Aviso);
                return false;
            }
            return true;
        }


        private bool EnviarEmailContratacao(QueroContratarCurriculoEstudante queroContratarDados, int filialId, int curriculoId)
        {
            try
            {
                string error;
                if (!queroContratarDados.EnviarEmailQueroContratar(filialId, curriculoId, out error))
                {
                    ExibirMensagem(error, TipoMensagem.Erro);
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (ex is ThreadAbortException)
                    return false;

                EL.GerenciadorException.GravarExcecao(ex);
                ExibirMensagem(string.Format("Falha ao enviar contratação ({0}).", ex.Message), TipoMensagem.Erro);
                return false;
            }

            return true;
        }
    }
}