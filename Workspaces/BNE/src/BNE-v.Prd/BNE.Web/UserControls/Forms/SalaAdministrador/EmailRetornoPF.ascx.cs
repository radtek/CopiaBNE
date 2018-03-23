using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Enumeradores = BNE.BLL.Enumeradores;
using BNE.BLL;
using BNE.EL;
using Resources;
using BNE.Web.Resources;

namespace BNE.Web.UserControls.Forms.SalaAdministrador
{
    public partial class EmailRetornoPF : BaseUserControl
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
                base.ExibirMensagem(MensagemAviso._100001, Code.Enumeradores.TipoMensagem.Aviso);
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
            AjustarValidacao();
        }
        #endregion

        #region AjustarValidacao
        private void AjustarValidacao()
        {
            txtConfirmaCadastroEmpresa.ExpressaoValidacao = Configuracao.regexEmail;
            txtEnvioSolicitacaoR1.ExpressaoValidacao = Configuracao.regexEmail;
            txtFalePresidente.ExpressaoValidacao = Configuracao.regexEmail;
            txtAgradecimento.ExpressaoValidacao = Configuracao.regexEmail;
            txtContato.ExpressaoValidacao = Configuracao.regexEmail;
            txtPecaAjuda.ExpressaoValidacao = Configuracao.regexEmail;
            txtConfirmacaoCadastroCurriculo.ExpressaoValidacao = Configuracao.regexEmail;
            txtPassoaPassodoVIP.ExpressaoValidacao = Configuracao.regexEmail;
            txtAbandonoCompraPF.ExpressaoValidacao = Configuracao.regexEmail;
            txtBoletoRenovacaoVIP.ExpressaoValidacao = Configuracao.regexEmail;
            txtConviteAtualizarCurriculo.ExpressaoValidacao = Configuracao.regexEmail;
            txtConfimacaoPublicacaoVagas.ExpressaoValidacao = Configuracao.regexEmail;
            txtConfirmacaoCriacaoSiteTrabalheConosco.ExpressaoValidacao = Configuracao.regexEmail;
            txtAbandonoCompraPJ.ExpressaoValidacao = Configuracao.regexEmail;
        }
        #endregion

        #region PreencherCampos
        private void PreencherCampos()
        {
            txtConfirmaCadastroEmpresa.Valor = DicParametros[Enumeradores.Parametro.EmailInclusaoEmpresa];
            txtEnvioSolicitacaoR1.Valor = DicParametros[Enumeradores.Parametro.EmailSolicitacaoR1];
            txtFalePresidente.Valor = DicParametros[Enumeradores.Parametro.EmailFalePresidente];
            txtAgradecimento.Valor = DicParametros[Enumeradores.Parametro.EmailAgradecimento];
            txtContato.Valor = DicParametros[Enumeradores.Parametro.EmailDestinoFaleConosco];
            txtPecaAjuda.Valor = DicParametros[Enumeradores.Parametro.EmailPecaAjuda];
            txtConfirmacaoCadastroCurriculo.Valor = DicParametros[Enumeradores.Parametro.EmailConfirmacaoCadastroCurriculo];
            txtPassoaPassodoVIP.Valor = DicParametros[Enumeradores.Parametro.EmailPassoaPassoVip];
            txtAbandonoCompraPF.Valor = DicParametros[Enumeradores.Parametro.EmailAbandonoCompraPF];
            txtBoletoRenovacaoVIP.Valor = DicParametros[Enumeradores.Parametro.EmailBoletoRenovacaoVIP];
            txtConviteAtualizarCurriculo.Valor = DicParametros[Enumeradores.Parametro.EmailConviteAtualizarCurriculo];
            txtConfimacaoPublicacaoVagas.Valor = DicParametros[Enumeradores.Parametro.EmailConfirmacaoPublicacaoVagas];
            txtConfirmacaoCriacaoSiteTrabalheConosco.Valor = DicParametros[Enumeradores.Parametro.EmailConfirmacaoCriacaoSTC];
            txtAbandonoCompraPJ.Valor = DicParametros[Enumeradores.Parametro.EmailAbandonoCompraPJ];
        }
        #endregion

        #region CarregarDicionario
        private void CarregarDicionario()
        {
            List<Enumeradores.Parametro> parametros = new List<Enumeradores.Parametro>();
            parametros.Add(Enumeradores.Parametro.EmailInclusaoEmpresa);
            parametros.Add(Enumeradores.Parametro.EmailSolicitacaoR1);
            parametros.Add(Enumeradores.Parametro.EmailFalePresidente);
            parametros.Add(Enumeradores.Parametro.EmailAgradecimento);
            parametros.Add(Enumeradores.Parametro.EmailDestinoFaleConosco);
            parametros.Add(Enumeradores.Parametro.EmailPecaAjuda);
            parametros.Add(Enumeradores.Parametro.EmailConfirmacaoCadastroCurriculo);
            parametros.Add(Enumeradores.Parametro.EmailPassoaPassoVip);
            parametros.Add(Enumeradores.Parametro.EmailAbandonoCompraPF);
            parametros.Add(Enumeradores.Parametro.EmailBoletoRenovacaoVIP);
            parametros.Add(Enumeradores.Parametro.EmailConviteAtualizarCurriculo);
            parametros.Add(Enumeradores.Parametro.EmailConfirmacaoPublicacaoVagas);
            parametros.Add(Enumeradores.Parametro.EmailConfirmacaoCriacaoSTC);
            parametros.Add(Enumeradores.Parametro.EmailAbandonoCompraPJ);
            
            DicParametros = Parametro.ListarParametros(parametros);
        }
        #endregion

        #region Salvar
        private void Salvar()
        {
            DicParametros[Enumeradores.Parametro.EmailInclusaoEmpresa] = txtConfirmaCadastroEmpresa.Valor;
            DicParametros[Enumeradores.Parametro.EmailSolicitacaoR1] = txtEnvioSolicitacaoR1.Valor;
            DicParametros[Enumeradores.Parametro.EmailFalePresidente] = txtFalePresidente.Valor;
            DicParametros[Enumeradores.Parametro.EmailAgradecimento] = txtAgradecimento.Valor;
            DicParametros[Enumeradores.Parametro.EmailDestinoFaleConosco] = txtContato.Valor;
            DicParametros[Enumeradores.Parametro.EmailPecaAjuda] = txtPecaAjuda.Valor;
            DicParametros[Enumeradores.Parametro.EmailConfirmacaoCadastroCurriculo] = txtConfirmacaoCadastroCurriculo.Valor;
            DicParametros[Enumeradores.Parametro.EmailPassoaPassoVip] = txtPassoaPassodoVIP.Valor;
            DicParametros[Enumeradores.Parametro.EmailAbandonoCompraPF] = txtAbandonoCompraPF.Valor;
            DicParametros[Enumeradores.Parametro.EmailBoletoRenovacaoVIP] = txtBoletoRenovacaoVIP.Valor;
            DicParametros[Enumeradores.Parametro.EmailConviteAtualizarCurriculo] = txtConviteAtualizarCurriculo.Valor;
            DicParametros[Enumeradores.Parametro.EmailConfirmacaoPublicacaoVagas] = txtConfimacaoPublicacaoVagas.Valor;
            DicParametros[Enumeradores.Parametro.EmailConfirmacaoCriacaoSTC] = txtConfirmacaoCriacaoSiteTrabalheConosco.Valor;
            DicParametros[Enumeradores.Parametro.EmailAbandonoCompraPJ] = txtAbandonoCompraPJ.Valor;

            Parametro.SalvarParametros(DicParametros);
        }
        #endregion

        #endregion

    }
}