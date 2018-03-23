using System;
using System.Collections.Generic;
using Enumeradores = BNE.BLL.Enumeradores;
using BNE.Web.Code;
using BNE.BLL;
using BNE.Web.Code.Enumeradores;
using Resources;

namespace BNE.Web.UserControls.Forms.SalaAdministrador
{
    public partial class ConfiguracoesValores : BaseUserControl
    {

        #region Propridades

        #region DicParametros
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        protected Dictionary<Enumeradores.Parametro, string> DicParametros
        {
            get
            {
                return (Dictionary<Enumeradores.Parametro, string>)(ViewState[Chave.Temporaria.Variavel1.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel1.ToString(), value);
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {

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
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion
        
        #endregion

        #region Métodos

        #region Inicializar
        public void Inicializar()
        {
            CarregarDicionario();
            PreencherCampos();
        }
        #endregion

        #region PreencherCampos
        private void PreencherCampos()
        {
            txtValorSalarioMínimo.Valor = Convert.ToDecimal(DicParametros[Enumeradores.Parametro.SalarioMinimoNacional]);
            txtLimiteDiasAtualizarCurriculo.Valor = DicParametros[Enumeradores.Parametro.DiasLimiteAtualizacaoCurriculo];
            txtIdadeMinima.Valor = DicParametros[Enumeradores.Parametro.IdadeMinima];
            txtIdadeMaxima.Valor = DicParametros[Enumeradores.Parametro.IdadeMaxima];
            txtLimiteDiasAcessarCurriculoDescontar.Valor = DicParametros[Enumeradores.Parametro.PeriodoReVisualizacaoCurriculo];
            txtQuantidadeDiasEmissaoVencimentoboletoPF.Valor = DicParametros[Enumeradores.Parametro.QuantidadeDiasVencimentoAposEmissaoBoletoPF];
            txtQuantidadeDiasVagaAnunciadaAtiva.Valor = DicParametros[Enumeradores.Parametro.QuantidadeDiasPrazoVaga];
            txtQuantidadeLimiteUsuariosEmpresa.Valor = DicParametros[Enumeradores.Parametro.QuantidadeLimiteUsuarios];
            txtQuantidadeLimiteUsuarioMasterEmpresa.Valor = DicParametros[Enumeradores.Parametro.QuantidadeLimiteUsuariosMaster];
            txtQuantidadeDiaEmissaoVencimentoBoletoPJ.Valor = DicParametros[Enumeradores.Parametro.QuantidadeDiasVencimentoAposEmissaoBoletoPJ];
            txtQuantidadeMinimaSMSVenda.Valor = DicParametros[Enumeradores.Parametro.QuantidadeSMSMinima];
            txtValorSMSAvulso.Valor = Convert.ToDecimal(DicParametros[Enumeradores.Parametro.ValorSMSAvulso]);
        }
        #endregion

        #region CarregarDicionario
        private void CarregarDicionario()
        {
            var parametros = new List<Enumeradores.Parametro>
                {
                    Enumeradores.Parametro.SalarioMinimoNacional,
                    Enumeradores.Parametro.DiasLimiteAtualizacaoCurriculo,
                    Enumeradores.Parametro.IdadeMinima,
                    Enumeradores.Parametro.IdadeMaxima,
                    Enumeradores.Parametro.PeriodoReVisualizacaoCurriculo,
                    Enumeradores.Parametro.QuantidadeDiasVencimentoAposEmissaoBoletoPF,
                    Enumeradores.Parametro.QuantidadeDiasPrazoVaga,
                    Enumeradores.Parametro.QuantidadeLimiteUsuarios,
                    Enumeradores.Parametro.QuantidadeLimiteUsuariosMaster,
                    Enumeradores.Parametro.QuantidadeDiasVencimentoAposEmissaoBoletoPJ,
                    Enumeradores.Parametro.QuantidadeSMSMinima,
                    Enumeradores.Parametro.ValorSMSAvulso
                };

            DicParametros = Parametro.ListarParametros(parametros);
        }
        #endregion

        #region Salvar
        private void Salvar()
        {
            DicParametros[Enumeradores.Parametro.SalarioMinimoNacional] = txtValorSalarioMínimo.Valor.Value.ToString();
            DicParametros[Enumeradores.Parametro.DiasLimiteAtualizacaoCurriculo] = txtLimiteDiasAtualizarCurriculo.Valor;
            DicParametros[Enumeradores.Parametro.IdadeMinima] = txtIdadeMinima.Valor;
            DicParametros[Enumeradores.Parametro.IdadeMaxima] = txtIdadeMaxima.Valor;
            DicParametros[Enumeradores.Parametro.PeriodoReVisualizacaoCurriculo] = txtLimiteDiasAcessarCurriculoDescontar.Valor;
            DicParametros[Enumeradores.Parametro.QuantidadeDiasVencimentoAposEmissaoBoletoPF] = txtQuantidadeDiasEmissaoVencimentoboletoPF.Valor;
            DicParametros[Enumeradores.Parametro.QuantidadeDiasPrazoVaga] = txtQuantidadeDiasVagaAnunciadaAtiva.Valor;
            DicParametros[Enumeradores.Parametro.QuantidadeLimiteUsuarios] = txtQuantidadeLimiteUsuariosEmpresa.Valor;
            DicParametros[Enumeradores.Parametro.QuantidadeLimiteUsuariosMaster] = txtQuantidadeLimiteUsuarioMasterEmpresa.Valor;
            DicParametros[Enumeradores.Parametro.QuantidadeDiasVencimentoAposEmissaoBoletoPJ] = txtQuantidadeDiaEmissaoVencimentoBoletoPJ.Valor;
            DicParametros[Enumeradores.Parametro.QuantidadeSMSMinima] = txtQuantidadeMinimaSMSVenda.Valor;
            DicParametros[Enumeradores.Parametro.ValorSMSAvulso] = txtValorSMSAvulso.Valor.Value.ToString();

            Parametro.SalvarParametros(DicParametros);
        }
        #endregion

        #endregion

    }
}