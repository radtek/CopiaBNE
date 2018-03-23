using BNE.BLL;
using BNE.BLL.Common;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Master;
using Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web
{
    public partial class CiaPlanosNovo2 : BasePagePagamento
    {

        #region Enumeradores
        private enum EnumBotaoClicado
        {
            AplicarCodigoDesconto,
            PlanoBasico,
            PlanoIndicado,
            PlanoPlus,
            PlanoCustomizado,
            Login
        }

        public enum PlanoEscolhido
        {
            Nenhum,
            Basico,
            Indicado,
            Plus,
            Customizado
        }
        #endregion

        #region Variaveis

        String _nomePessoa, _numRG, _razaoSocial, _descRua, _nomeCidade, _estado, _email, _numeroRua, _numeroCEP;
        int _numeroParcelas, _quantidadeUsuarios, _quantidadeVisualizacoes, _quantidadeSms, _tempoPlano;
        decimal _valorPlano, _numCPF, _numCNPJ;

        #endregion

        #region Propriedades

        #region ValorContrato - Variavel 43

        public decimal ValorContrato
        {
            get
            {
                return Convert.ToInt32(ViewState[Chave.Temporaria.Variavel43.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel43.ToString(), value);
            }
        }

        #endregion

        #region PrazoContrato - Variavel 44
        public int PrazoContrato
        {
            get
            {
                return Convert.ToInt32(ViewState[Chave.Temporaria.Variavel44.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel44.ToString(), value);
            }
        }
        #endregion

        #region QuantidadeSmsContrato - variavel 45
        public int QuantidadeSmsContrato
        {
            get
            {
                return Convert.ToInt32(ViewState[Chave.Temporaria.Variavel45.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel45.ToString(), value);
            }
        }
        #endregion

        #region PrazoValidadeContrato - Variavel 46

        public int PrazoValidadeContrato
        {
            get
            {
                return Convert.ToInt32(ViewState[Chave.Temporaria.Variavel46.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel46.ToString(), value);
            }
        }

        #endregion

        #region PlanoBasicoIdentificador
        /// <summary>
        /// Propriedade que armazena e recupera o IdPlano
        /// </summary>
        public int PlanoBasicoIdentificador
        {
            get
            {
                return Convert.ToInt32(ViewState[Chave.Temporaria.VendaPlanoCIA_PlanoBasicoIdentificador.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.VendaPlanoCIA_PlanoBasicoIdentificador.ToString(), value);
            }
        }
        #endregion

        #region PlanoIndicadoIdentificador
        /// <summary>
        /// Propriedade que armazena e recupera o IdPlano
        /// </summary>
        public int PlanoIndicadoIdentificador
        {
            get
            {
                return Convert.ToInt32(ViewState[Chave.Temporaria.VendaPlanoCIA_PlanoIndicadoIdentificador.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.VendaPlanoCIA_PlanoIndicadoIdentificador.ToString(), value);
            }
        }
        #endregion

        #region PlanoPlusIdentificador
        /// <summary>
        /// Propriedade que armazena e recupera o IdPlano
        /// </summary>
        public int PlanoPlusIdentificador
        {
            get
            {
                return Convert.ToInt32(ViewState[Chave.Temporaria.VendaPlanoCIA_PlanoPlusIdentificador.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.VendaPlanoCIA_PlanoPlusIdentificador.ToString(), value);
            }
        }
        #endregion

        #region PlanoCustomizadoIdentificador
        /// <summary>
        /// Propriedade que armazena e recupera o IdPlano
        /// </summary>
        public int PlanoCustomizadoIdentificador
        {
            get
            {
                return Convert.ToInt32(ViewState[Chave.Temporaria.VendaPlanoCIA_PlanoCustomizadoIdentificador.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.VendaPlanoCIA_PlanoCustomizadoIdentificador.ToString(), value);
            }
        }
        #endregion

        #region QuantidadeSMS - Variavel 4
        public int QuantidadeSMS
        {
            get
            {
                return Convert.ToInt32(ViewState[Chave.Temporaria.Variavel4.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel4.ToString(), value);
            }
        }
        #endregion

        #region . - Variavel 6
        public int QuantidadePrazoBoleto
        {
            get
            {
                return Convert.ToInt32(Session[Chave.Temporaria.Variavel6.ToString()]);
            }
            set
            {
                Session.Add(Chave.Temporaria.Variavel6.ToString(), value);
            }
        }
        #endregion

        #region BotaoClicado - Variavel 7
        private EnumBotaoClicado BotaoClicado
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel7.ToString()] != null)
                    return (EnumBotaoClicado)ViewState[Chave.Temporaria.Variavel7.ToString()];
                else return EnumBotaoClicado.Login;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel7.ToString(), value);
            }
        }
        #endregion

        #region Parametros - Variavel 8
        private Dictionary<Enumeradores.Parametro, string> Parametros
        {
            get
            {
                if (null == ViewState[Chave.Temporaria.Variavel8.ToString()])
                {
                    var listaParametros = new List<Enumeradores.Parametro>
                    {
                        Enumeradores.Parametro.VendaPlanoCIA_PlanoBasicoIdentificador,
                        Enumeradores.Parametro.VendaPlanoCIA_PlanoIndicadoIdentificador,
                        Enumeradores.Parametro.VendaPlanoCIA_PlanoPlusIdentificadorNovo,
                        Enumeradores.Parametro.VendaPlanoCIA_PlanoBasicoValorSemDesconto,
                        Enumeradores.Parametro.VendaPlanoCIA_PlanoIndicadoValorSemDesconto,
                        Enumeradores.Parametro.VendaPlanoCIA_PlanoPlusValorSemDescontoNovo
                    };

                    ViewState[Chave.Temporaria.Variavel8.ToString()] =
                        Parametro.ListarParametros(listaParametros);
                }

                return ViewState[Chave.Temporaria.Variavel8.ToString()] as Dictionary<Enumeradores.Parametro, string>;
            }
        }
        #endregion

        #region TempoPlano - Variavel 23
        public string TempoPlano
        {
            get
            {
                return Convert.ToString(Session[Chave.Temporaria.Variavel23.ToString()]);
            }

            set
            {
                Session.Add(Chave.Temporaria.Variavel23.ToString(), value);
            }
        }
        #endregion

        #region TempoPlanoExtenso - Variavel 24
        public string TempoPlanoExtenso
        {
            get
            {
                return Convert.ToString(Session[Chave.Temporaria.Variavel24.ToString()]);
            }

            set
            {
                Session.Add(Chave.Temporaria.Variavel24.ToString(), value);
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (base.IdFilial.HasValue && (new Filial(base.IdFilial.Value).EmpresaEmAuditoria() || new Filial(base.IdFilial.Value).EmpresaBloqueada()))
                {
                    Session.Add(Chave.Temporaria.MensagemPermissao.ToString(), MensagemAviso._202406);
                    //Redirect("Default.aspx");
                }

                UIHelper.CarregarDropDownList(ddlPlanoCustomizado, Plano.ListarPlanosVendaCIAPorVendedor(), "Idf_Plano", "Des_Plano");
                LimparSessionPagamento();
                PreencherCampos();
            }

            var master = (Principal)Page.Master;

            if (master != null)
                master.LoginEfetuadoSucesso += master_LoginEfetuadoSucesso;

            InicializarBarraBusca(TipoBuscaMaster.Curriculo, false, "CIAPlanosNovo");

            SetarIdPlanoNaSessao(PlanoEscolhido.Nenhum);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Registra eventos controle", "LocalPageLoad();", true);
        }
        #endregion

        #region txtCodigoCredito_TextChanged
        protected void txtCodigoCredito_TextChanged(object sender, EventArgs e)
        {
            BotaoClicado = EnumBotaoClicado.AplicarCodigoDesconto;

            if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
                ValidarCodigoDesconto();
            else
                ExibirLogin();
        }
        #endregion

        #region Cliques Botão Compra
        protected void lkbPlanoAtual_Click(object sender, EventArgs e)
        {
            //Redirect(GetRouteUrl())
        }

        protected void lkbPlanoBasico_Click(object sender, EventArgs e)
        {
            ProcessoCompra(PlanoEscolhido.Basico);
        }

        protected void lkbPlanoIndicado_Click(object sender, EventArgs e)
        {
            ProcessoCompra(PlanoEscolhido.Indicado);
        }

        protected void lkbPlanoPlus_Click(object sender, EventArgs e)
        {
            ProcessoCompra(PlanoEscolhido.Plus);
        }

        protected void lkbPlanoCustomizado_Click(object sender, EventArgs e)
        {
            ProcessoCompra(PlanoEscolhido.Customizado);
        }
        #endregion

        #region master_LoginEfetuadoSucesso
        protected void master_LoginEfetuadoSucesso()
        {

            if (UrlDestino.Value == "/cadastro-de-curriculo-gratis")
            {
                Redirect("/cadastro-de-curriculo-gratis");
            }

            if (base.IdFilial.Value > 0)
            {
                if (new Filial(base.IdFilial.Value).EmpresaEmAuditoria() || new Filial(base.IdFilial.Value).EmpresaBloqueada())
                {
                    Session.Add(Chave.Temporaria.MensagemPermissao.ToString(), MensagemAviso._202406);
                    Redirect("Default.aspx");
                    return;
                }
            }
            else if (base.IdFilial.Value == 0)
            {
                Response.Redirect("/vip");
            }

            // prosseguir com o que o cliente desejava fazer antes de aparecer a tela de login
            switch (BotaoClicado)
            {
                case EnumBotaoClicado.AplicarCodigoDesconto:
                    ValidarCodigoDesconto();
                    break;
                case EnumBotaoClicado.PlanoBasico:
                    ProcessoCompra(PlanoEscolhido.Basico);
                    break;
                case EnumBotaoClicado.PlanoIndicado:
                    ProcessoCompra(PlanoEscolhido.Indicado);
                    break;
                case EnumBotaoClicado.PlanoPlus:
                    ProcessoCompra(PlanoEscolhido.Plus);
                    break;
                case EnumBotaoClicado.PlanoCustomizado:
                    ProcessoCompra(PlanoEscolhido.Customizado);
                    break;
                case EnumBotaoClicado.Login:
                    Response.Redirect("/Escolha-de-Plano-CIA");
                    break;
                default:
                    throw new InvalidOperationException("BotaoClicado contém valor não previsto anteriormente");
            }
        }
        #endregion

        #region btnValidarCodigoCredito_Click
        protected void btnValidarCodigoCredito_Click(object sender, EventArgs e)
        {
            BotaoClicado = EnumBotaoClicado.AplicarCodigoDesconto;

            if (String.IsNullOrEmpty(txtCodigoCredito.Text))
            {
                base.ExibirMensagem("Informe o código de desconto!", TipoMensagem.Aviso, false);
                return;
            }

            if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
                ValidarCodigoDesconto();
            else
                ExibirLogin();
        }
        #endregion

        #region btnContratoBasico_Click
        protected void btnContratoBasico_Click(object sender, EventArgs e)
        {
            AtualizaValoresContrato(PlanoEscolhido.Basico);
            VerContrato();
        }
        #endregion

        #region btnContratoIndicado_Click
        protected void btnContratoIndicado_Click(object sender, EventArgs e)
        {
            AtualizaValoresContrato(PlanoEscolhido.Indicado);
            VerContrato();
        }
        #endregion

        #region btnContratoPlus_Click
        protected void btnContratoPlus_Click(object sender, EventArgs e)
        {
            AtualizaValoresContrato(PlanoEscolhido.Plus);
            VerContrato();
        }
        #endregion

        #region ddlPlanoCustomizado_OnSelectedIndexChanged
        protected void ddlPlanoCustomizado_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PreencherCamposCustomizado();
        }
        #endregion

        #endregion

        #region Metodos

        #region AbrirModal

        protected void AbrirModal(object sender, EventArgs e)
        {
            EnvioEmail();
        }

        #endregion

        #region EnvioEmail

        private void EnvioEmail()
        {
            ucModalEnvioEmail.MostrarModal(BNE.Web.UserControls.Modais.EnvioEmail.TipoEnvioEmail.EmailCompraCIA);
            pnlL.Visible = true;
            upL.Update();
        }
        #endregion

        #region VerificarEdicao
        private bool VerificarEdicao(PlanoEscolhido planoEscolhido)
        {
            Plano objPlanoBasico = Plano.LoadObject(PlanoBasicoIdentificador);
            Plano objPlanoIndicado = Plano.LoadObject(PlanoIndicadoIdentificador);
            Plano objPlanoPlus = Plano.LoadObject(PlanoPlusIdentificador);
            Plano objPlanoCustomizado = Plano.LoadObject(PlanoCustomizadoIdentificador);

            decimal precoBasico;
            decimal precoIndicado;
            decimal precoPlus;
            decimal precoCustomizado;

            int smsBasico;
            int smsIndicado;
            int smsPlus;
            int smsCustomizado;

            int prazobasico;
            int prazoIndicado;
            int prazoPlus;
            int prazoCustomizado;

            // verifica preco
            if ((!Decimal.TryParse(txtPlanoBasicoPor.Text, out precoBasico) && planoEscolhido == PlanoEscolhido.Basico) ||
                (!Decimal.TryParse(txtPlanoIndicadoPor.Text, out precoIndicado) && planoEscolhido == PlanoEscolhido.Indicado) ||
                (!Decimal.TryParse(txtPlanoPlusPor.Text, out precoPlus) && planoEscolhido == PlanoEscolhido.Plus) ||
                (!Decimal.TryParse(txtPlanoCustomizadoPor.Text, out precoCustomizado) && planoEscolhido == PlanoEscolhido.Customizado))
            {
                ExibirMensagem("Preço inválido", TipoMensagem.Erro);
                SetarFocoNoErro(txtPlanoBasicoPor, txtPlanoIndicadoPor, txtPlanoPlusPor, txtPlanoCustomizadoPor, planoEscolhido);
                return false;
            }

            if ((precoBasico < objPlanoBasico.ValorBaseMinimo.Value && planoEscolhido == PlanoEscolhido.Basico && !ckbAplicarDesconto.Checked) ||
                (precoIndicado < objPlanoIndicado.ValorBaseMinimo.Value && planoEscolhido == PlanoEscolhido.Indicado && !ckbAplicarDesconto.Checked) ||
                (precoPlus < objPlanoPlus.ValorBaseMinimo.Value && planoEscolhido == PlanoEscolhido.Plus && !ckbAplicarDesconto.Checked) ||
                (precoCustomizado < objPlanoCustomizado.ValorBaseMinimo.Value && planoEscolhido == PlanoEscolhido.Customizado && !ckbAplicarDesconto.Checked) ||
                ((!ckbAplicarDesconto.Checked && precoBasico < (objPlanoBasico.ValorBase * objPlanoBasico.ValorDescontoMaximo / 100)) && planoEscolhido == PlanoEscolhido.Basico) ||
                ((!ckbAplicarDesconto.Checked && precoIndicado < (objPlanoIndicado.ValorBase * objPlanoIndicado.ValorDescontoMaximo / 100)) && planoEscolhido == PlanoEscolhido.Indicado) ||
                ((!ckbAplicarDesconto.Checked && precoPlus < (objPlanoPlus.ValorBase * objPlanoPlus.ValorDescontoMaximo / 100)) && planoEscolhido == PlanoEscolhido.Plus) ||
                ((!ckbAplicarDesconto.Checked && precoCustomizado < (objPlanoCustomizado.ValorBase * objPlanoCustomizado.ValorDescontoMaximo / 100)) && planoEscolhido == PlanoEscolhido.Customizado))
            {
                ExibirMensagem("Preço abaixo do mínimo", TipoMensagem.Erro);
                SetarFocoNoErro(txtPlanoBasicoPor, txtPlanoIndicadoPor, txtPlanoPlusPor, txtPlanoCustomizadoPor, planoEscolhido);
                return false;
            }

            // verifica sms
            if ((!Int32.TryParse(txtPlanoBasicoSms.Text, out smsBasico) && planoEscolhido == PlanoEscolhido.Basico) ||
                (!Int32.TryParse(txtPlanoIndicadoSms.Text, out smsIndicado) && planoEscolhido == PlanoEscolhido.Indicado) ||
                (!Int32.TryParse(txtPlanoPlusSms.Text, out smsPlus) && planoEscolhido == PlanoEscolhido.Plus) ||
                (!Int32.TryParse(txtPlanoCustomizadoSms.Text, out smsCustomizado) && planoEscolhido == PlanoEscolhido.Customizado))
            {
                ExibirMensagem("Quantidade de SMS inválida", TipoMensagem.Erro);
                SetarFocoNoErro(txtPlanoBasicoSms, txtPlanoIndicadoSms, txtPlanoPlusSms, txtPlanoCustomizadoSms, planoEscolhido);
                return false;
            }

            if ((smsBasico > objPlanoBasico.QuantidadeSMSMaxima.Value && planoEscolhido == PlanoEscolhido.Basico) ||
                (smsIndicado > objPlanoIndicado.QuantidadeSMSMaxima.Value && planoEscolhido == PlanoEscolhido.Indicado) ||
                (smsPlus > objPlanoPlus.QuantidadeSMSMaxima.Value && planoEscolhido == PlanoEscolhido.Plus) ||
                (smsCustomizado > objPlanoCustomizado.QuantidadeSMSMaxima.Value && planoEscolhido == PlanoEscolhido.Customizado))
            {
                ExibirMensagem("Quantidade de SMS acima da máxima", TipoMensagem.Erro);
                SetarFocoNoErro(txtPlanoBasicoSms, txtPlanoIndicadoSms, txtPlanoPlusSms, txtPlanoCustomizadoSms, planoEscolhido);
                return false;
            }

            // verifica prazo boleto
            if ((!Int32.TryParse(txtPlanoBasicoPrazo.Text, out prazobasico) && planoEscolhido == PlanoEscolhido.Basico) ||
                (!Int32.TryParse(txtPlanoIndicadoPrazo.Text, out prazoIndicado) && planoEscolhido == PlanoEscolhido.Indicado) ||
                (!Int32.TryParse(txtPlanoPlusPrazo.Text, out prazoPlus) && planoEscolhido == PlanoEscolhido.Plus) ||
                (!Int32.TryParse(txtPlanoCustomizadoPrazo.Text, out prazoCustomizado) && planoEscolhido == PlanoEscolhido.Customizado))
            {
                ExibirMensagem("Prazo de boleto inválido", TipoMensagem.Erro);
                SetarFocoNoErro(txtPlanoBasicoPrazo, txtPlanoIndicadoPrazo, txtPlanoPlusPrazo, txtPlanoCustomizadoPrazo, planoEscolhido);
                return false;
            }

            if ((prazobasico > objPlanoBasico.QuantidadePrazoBoletoMaxima.Value && planoEscolhido == PlanoEscolhido.Basico) ||
                (prazoIndicado > objPlanoIndicado.QuantidadePrazoBoletoMaxima.Value && planoEscolhido == PlanoEscolhido.Indicado) ||
                (prazoPlus > objPlanoPlus.QuantidadePrazoBoletoMaxima.Value && planoEscolhido == PlanoEscolhido.Plus) ||
                (prazoCustomizado > objPlanoCustomizado.QuantidadePrazoBoletoMaxima.Value && planoEscolhido == PlanoEscolhido.Customizado))
            {
                ExibirMensagem("Prazo de boleto acima do máximo", TipoMensagem.Erro);
                SetarFocoNoErro(txtPlanoBasicoPrazo, txtPlanoIndicadoPrazo, txtPlanoPlusPrazo, txtPlanoCustomizadoPrazo, planoEscolhido);
                return false;
            }

            // armazena valores para a criação do Plano Adquirido, mais tarde
            if (planoEscolhido == PlanoEscolhido.Basico)
            {
                QuantidadeSMS = smsBasico;
                base.ValorBasePlano.Value = precoBasico;
                base.PrazoBoleto.Value = prazobasico;
            }
            else if (planoEscolhido == PlanoEscolhido.Indicado)
            {
                QuantidadeSMS = smsIndicado * objPlanoIndicado.QuantidadeParcela;
                base.ValorBasePlano.Value = precoIndicado;
                base.PrazoBoleto.Value = prazoIndicado;
            }
            else if (planoEscolhido == PlanoEscolhido.Plus)
            {
                QuantidadeSMS = smsPlus * objPlanoPlus.QuantidadeParcela;
                base.ValorBasePlano.Value = precoPlus;
                base.PrazoBoleto.Value = prazoPlus;
            }
            else if (planoEscolhido == PlanoEscolhido.Customizado)
            {
                QuantidadeSMS = smsCustomizado * objPlanoCustomizado.QuantidadeParcela;
                base.ValorBasePlano.Value = precoCustomizado;
                base.PrazoBoleto.Value = prazoCustomizado;
            }

            return true;
        }
        #endregion

        #region ProcessoCompra
        //protected void btnContinuar_Click(object sender, EventArgs e)
        //{
        //    BotaoClicado = EnumBotaoClicado.Continuar;

        //    if (!cbContrato.Checked)
        //        base.ExibirMensagem("Para finalizar a compra é necessário que aceite os termos de contrato !", TipoMensagem.Aviso, false);
        //    else if (ckbAplicarDesconto.Checked && string.IsNullOrWhiteSpace(txtMotivoVendaAbaixoMinimo.Text))
        //        base.ExibirMensagem("Para finalizar a venda é necessário informar o motivo da venda abaixo do valor mínimo no plano!", TipoMensagem.Aviso, false);
        //    else
        //    {
        //        if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
        //        {
        //            //empresa bloqueada ou aguardando publicação não pode efetuar compra.
        //            Filial objFilial = Filial.LoadObject(base.IdFilial.Value);
        //            if (!objFilial.SituacaoFilial.IdSituacaoFilial.Equals((int)Enumeradores.SituacaoFilial.AguardandoPublicacao))
        //            {
        //                ProcessoCompra();
        //            }
        //            else
        //            {
        //                Control auditoria = Page.LoadControl("~/UserControls/Modais/EmpresaAguardandoPublicacao.ascx");
        //                cphModaisEmpresa.Controls.Add(auditoria);

        //                ((UserControls.Modais.EmpresaBloqueadaAguardandoPub)auditoria).MostrarModal();

        //                upModaisEmpresa.Update();
        //            }
        //        }
        //        else
        //            ExibirLogin();
        //    }

        //}

        private void ProcessoCompra(PlanoEscolhido planoEscolhido)
        {
            //if (!cbContrato.Checked)
            //{
            //    base.ExibirMensagem("Para finalizar a compra é necessário que aceite os termos de contrato !", TipoMensagem.Aviso, false);
            //    return;
            //}

            if (ckbAplicarDesconto.Checked && string.IsNullOrWhiteSpace(txtMotivoVendaAbaixoMinimo.Text))
            {
                base.ExibirMensagem("Para finalizar a venda é necessário informar o motivo da venda abaixo do valor mínimo no plano!", TipoMensagem.Aviso, false);
                return;
            }

            if (!base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
            {
                ExibirLogin();
                return;
            }

            Filial objFilial = Filial.LoadObject(base.IdFilial.Value);
            if (objFilial.SituacaoFilial.IdSituacaoFilial.Equals((int)Enumeradores.SituacaoFilial.AguardandoPublicacao))
            {
                Control auditoria = Page.LoadControl("~/UserControls/Modais/EmpresaAguardandoPublicacao.ascx");
                cphModaisEmpresa.Controls.Add(auditoria);

                ((UserControls.Modais.EmpresaBloqueadaAguardandoPub)auditoria).MostrarModal();

                upModaisEmpresa.Update();

                return;
            }

            SetarIdPlanoNaSessao(planoEscolhido);

            if (PermitirEdicao())
            {
                if (!VerificarEdicao(planoEscolhido))
                {
                    // a mensagem de erro é gerada dentro do VerificarEdicao()
                    return;
                }
            }
            else if (ConcederBeneficio())
            {
                if (!CalcularBeneficio())
                {
                    ExibirMensagem("Erro no cálculo do benefício", TipoMensagem.Erro);
                    return;
                }
            }

            NotificarVendaPlanoAbaixoMinimo();

            CriarPlanoAdquirido();

            Redirecionar(planoEscolhido);

        }
        #endregion

        #region CriarPlanoAdquirido
        public void CriarPlanoAdquirido()
        {
            UsuarioFilialPerfil objUsuarioFilialPerfil = null;
            Plano objPlano = null;
            Filial objFilial = null;
            UsuarioFilial objUsuarioFilial = null;

            // Carrega Plano de acordo com plano escolhido na session
            if (base.PagamentoIdentificadorPlano.HasValue)
            {
                objPlano = Plano.LoadObject(base.PagamentoIdentificadorPlano.Value);
                objPlano.QuantidadeSMS = QuantidadeSMS;
            }

            // Carrega Usuario Filial Perfil session
            if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
            {
                objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(base.IdUsuarioFilialPerfilLogadoEmpresa.Value);
                UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(objUsuarioFilialPerfil.IdUsuarioFilialPerfil, out objUsuarioFilial);
            }

            // Carrega Filial Session
            if (base.IdFilial.HasValue)
                objFilial = Filial.LoadObject(base.IdFilial.Value);

            // Atualiza Valor Base Plano de acordo valor gravado na session
            if (base.ValorBasePlano.HasValue)
                objPlano.ValorBase = base.ValorBasePlano.Value;

            var objPlanoAdquirido = PlanoAdquirido.CriarPlanoAdquiridoPJ(objUsuarioFilialPerfil, objFilial, objUsuarioFilial, objPlano, base.PrazoBoleto.Value);

            base.PagamentoIdentificadorPlanoAdquirido.Value = objPlanoAdquirido.IdPlanoAdquirido;
        }
        #endregion

        #region NotificarVendaPlanoAbaixoMinimo
        /// <summary>
        /// Fluxo acionado quando um vendedor realiza uma venda de plano abaixo do mínimo configurado para aquele plano, respeitando a coluna Vlr_Desconto_Maximo
        /// </summary>
        private void NotificarVendaPlanoAbaixoMinimo()
        {
            if (base.PagamentoIdCodigoDesconto.HasValue)
            {
                var objCodigoDesconto = CodigoDesconto.LoadObject(base.PagamentoIdCodigoDesconto.Value);

                if (objCodigoDesconto.UsuarioFilialPerfil != null)
                {
                    var objFilial = Filial.LoadObject(base.IdFilial.Value);
                    var objPlano = new Plano(base.PagamentoIdentificadorPlano.Value);

                    objCodigoDesconto.UsuarioFilialPerfil.CompleteObject();
                    objFilial.Endereco.CompleteObject();
                    objFilial.Endereco.Cidade.CompleteObject();

                    var parametros = new
                    {
                        DescricaoPlano = objPlano.DescricaoPlano,
                        CodigoPlano = objPlano.IdPlano,
                        NomeEmpresa = string.Format("{0} - {1}", objFilial.CNPJ, objFilial.NomeFantasia),
                        ValorPago = base.ValorBasePlano.Value,
                        Justificativa = txtMotivoVendaAbaixoMinimo.Text,
                        Vendedor = objCodigoDesconto.UsuarioFilialPerfil.PessoaFisica.NomeCompleto,
                        ValorBase = objPlano.RecuperarValor(),
                        Cidade =
                            UIHelper.FormatarCidade(objFilial.Endereco.Cidade.NomeCidade,
                                objFilial.Endereco.Cidade.Estado.SiglaEstado)
                    };

                    string descricao = string.Empty;

                    if (ckbAplicarDesconto.Checked)
                        descricao =
                            parametros.ToString(
                                "O vendedor {Vendedor} solicitou a venda do plano {DescricaoPlano} (Código - {CodigoPlano}) pelo valor de R$ {ValorPago} e sua justificativa foi {Justificativa}.");
                    else
                        descricao =
                            parametros.ToString(
                                "O vendedor {Vendedor} solicitou a venda do plano {DescricaoPlano} (Código - {CodigoPlano}) pelo valor de R$ {ValorPago}.");

                    FilialObservacao.SalvarCRM(descricao, objFilial,
                        new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value));

                    var valoresDestinatarios = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.EmailVendaCiaAbaixoMinimo);
                    var emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens);

                    if (!string.IsNullOrWhiteSpace(valoresDestinatarios))
                    {
                        string assunto;
                        var carta = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.VendaPlanoVendedorAbaixoMinimo,out assunto);
                        
                        carta = parametros.ToString(carta);

                        if (!ckbAplicarDesconto.Checked)
                            //Quando o vendedor vende abaixo do mínimo para o plano escolhido (Vlr_Plano_Base), é enviado um e-mail para informar o financeiro
                            assunto = string.Concat("Aviso de venda ", parametros.Vendedor);

                        MensagemCS.EnvioDeEmailComValidacao(TipoEnviadorEmail.Fila,assunto, carta,Enumeradores.CartaEmail.VendaPlanoVendedorAbaixoMinimo, emailRemetente,valoresDestinatarios);
                    }
                }
            }
        }
        #endregion EnviarEmailAvisoVendaPlanoAbaixoMinimo

        #region CalcularBeneficio
        private bool CalcularBeneficio()
        {
            try
            {
                Plano objPlano = Plano.LoadObject(base.PagamentoIdentificadorPlano.Value);
                CodigoDesconto objCodigoDesconto = new CodigoDesconto(base.PagamentoIdCodigoDesconto.Value);

                int qtdeSMS;
                decimal valorPlano = objPlano.ValorBase;
                if (objPlano.IdPlano == PlanoBasicoIdentificador)
                {
                    // a quantidade de SMS para o plano pré-pago é diferente dos demais planos
                    qtdeSMS = objPlano.QuantidadeSMS;
                    objCodigoDesconto.CalcularSMS(ref qtdeSMS, objPlano);
                    objCodigoDesconto.CalcularDesconto(ref valorPlano, objPlano);
                    QuantidadeSMS = qtdeSMS;
                }
                else
                {
                    qtdeSMS = objPlano.QuantidadeSMS / objPlano.QuantidadeParcela;
                    objCodigoDesconto.CalcularSMS(ref qtdeSMS, objPlano);
                    objCodigoDesconto.CalcularDesconto(ref valorPlano, objPlano);
                    QuantidadeSMS = qtdeSMS * objPlano.QuantidadeParcela;
                }

                base.ValorBasePlano.Value = Decimal.Round(valorPlano);
                base.PrazoBoleto.Value = objPlano.QuantidadePrazoBoletoMaxima.Value;

                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region SetarFocoNoErro
        private void SetarFocoNoErro(Control controleBasico, Control controleIndicado, Control controlePlus, Control controleCustomizado, PlanoEscolhido planoEscolhido)
        {
            if (planoEscolhido == PlanoEscolhido.Basico)
                controleBasico.Focus();
            else if (planoEscolhido == PlanoEscolhido.Indicado)
                controleIndicado.Focus();
            else if (planoEscolhido == PlanoEscolhido.Plus)
                controlePlus.Focus();
            else if (planoEscolhido == PlanoEscolhido.Customizado)
                controleCustomizado.Focus();
            else
                throw new InvalidOperationException("Nenhum plano escolhido! Não é possível focar o erro");
        }
        #endregion

        #region SetarIdPlanoNaSessao
        private void SetarIdPlanoNaSessao(PlanoEscolhido planoEscolhido)
        {
            switch (planoEscolhido)
            {
                case PlanoEscolhido.Basico:
                    base.PagamentoIdentificadorPlano.Value = PlanoBasicoIdentificador;
                    break;
                case PlanoEscolhido.Indicado:
                    base.PagamentoIdentificadorPlano.Value = PlanoIndicadoIdentificador;
                    break;
                case PlanoEscolhido.Plus:
                    base.PagamentoIdentificadorPlano.Value = PlanoPlusIdentificador;
                    break;
                case PlanoEscolhido.Customizado:
                    base.PagamentoIdentificadorPlano.Value = PlanoCustomizadoIdentificador;
                    break;
                default:
                    base.PagamentoIdentificadorPlano.Clear();
                    break;
            }
        }
        #endregion

        #region Redirecionar
        private void Redirecionar(PlanoEscolhido planoEscolhido)
        {
            String url;
            // Seta url para montar retorno da pagina a ser redirecionada a aplicacao após finalizar operações na cielo.
            if (String.IsNullOrEmpty(Request.Url.Query))
                url = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "");
            else
                url = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "").Replace(Request.Url.Query, "");

            string urlRetorno = string.Empty;

            if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
                urlRetorno = url + "/SalaSelecionadorPlanoIlimitado.aspx";

            base.PagamentoUrlRetorno.Value = urlRetorno;
            var r = Rota.RecuperarURLRota(Enumeradores.RouteCollection.PagamentoPlanoCIA);

#if DEBUG
            Redirect(String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), r));
#else
            Redirect(String.Format("https://{0}/{1}", UIHelper.RecuperarURLAmbiente(), r));
#endif
        }
        #endregion

        #region AtivarEdicao
        private void AtivarEdicao(bool ativa)
        {

            //ativa campos
            txtPlanoBasicoPor.Visible =
            txtPlanoBasicoSms.Visible =
            txtPlanoBasicoPrazo.Visible =
            txtPlanoIndicadoPor.Visible =
            txtPlanoIndicadoPrazo.Visible =
            txtPlanoIndicadoSms.Visible =
            txtPlanoPlusPor.Visible =
            txtPlanoPlusPrazo.Visible =
            txtPlanoPlusSms.Visible =
            pnlPrazoBoleto.Visible = pnlPlanoBasicoPrazo.Visible = pnlPlanoIndicadoPrazo.Visible = pnlPlanoPlusPrazo.Visible = ativa;

            lblPlanoBasicoPor.Visible =
            lblPlanoBasicoSms.Visible =
            lblPlanoIndicadoPor.Visible =
            lblPlanoIndicadoSms.Visible =
            lblPlanoPlusPor.Visible =
            lblPlanoPlusSms.Visible = !ativa;

            pnlPlanoCustomizado.Visible = ativa;
        }
        #endregion

        #region ConcederBeneficio
        private bool ConcederBeneficio()
        {
            if (base.PagamentoIdCodigoDesconto.HasValue)
            {
                CodigoDesconto objCodigoDesconto =
                    CodigoDesconto.LoadObject(base.PagamentoIdCodigoDesconto.Value);

                return objCodigoDesconto.VantagemCIA();
            }

            return false;
        }
        #endregion

        #region PermitirEdicao
        private bool PermitirEdicao()
        {
            if (base.PagamentoIdCodigoDesconto.HasValue)
            {
                CodigoDesconto objCodigoDesconto =
                    CodigoDesconto.LoadObject(base.PagamentoIdCodigoDesconto.Value);

                return objCodigoDesconto.EdicaoPlanoCIA();
            }

            return false;
        }
        #endregion

        #region ValidarCodigoDesconto
        private void ValidarCodigoDesconto()
        {
            LimparSessionPagamento();
            PreencherCampos();

            if (string.IsNullOrEmpty(txtCodigoCredito.Text))
                return;

            CodigoDesconto codigo;
            if (!BLL.CodigoDesconto.CarregarPorCodigo(txtCodigoCredito.Text, out codigo))
            {
                ExibirMensagem("Código promocional inválido", TipoMensagem.Erro);
                return;
            }

            if (codigo.JaUtilizado())
            {
                ExibirMensagem("Código promocional já utilizado", TipoMensagem.Erro);
                txtCodigoCredito.Text = String.Empty;
                return;
            }

            if (!codigo.DentroValidade())
            {
                ExibirMensagem("Código promocional fora da validade", TipoMensagem.Erro);
                txtCodigoCredito.Text = String.Empty;
                return;
            }

            TipoCodigoDesconto tipoCodigoDesconto;
            if (!codigo.TipoDescontoDefinido(out tipoCodigoDesconto))
            {
                ExibirMensagem("Código promocional inválido", TipoMensagem.Erro);
                txtCodigoCredito.Text = String.Empty;
                return;
            }

            if (!codigo.EdicaoPlanoCIA() && !codigo.VantagemCIA())
            {
                ExibirMensagem("Código promocional inválido", TipoMensagem.Erro);
                txtCodigoCredito.Text = String.Empty;
                return;
            }

            // seta sessao com as informacoes sobre o desconto
            base.PagamentoIdCodigoDesconto.Value = codigo.IdCodigoDesconto;
            ExibirMensagem(null, TipoMensagem.Erro);

            // preenche novamente a tela de acordo com o codigo informado
            PreencherCampos();
        }
        #endregion

        #region PreencherCampos
        private void PreencherCampos()
        {
            PlanoBasicoIdentificador = Convert.ToInt32(Parametros[Enumeradores.Parametro.VendaPlanoCIA_PlanoBasicoIdentificador]);
            PlanoIndicadoIdentificador = Convert.ToInt32(Parametros[Enumeradores.Parametro.VendaPlanoCIA_PlanoIndicadoIdentificador]);
            PlanoPlusIdentificador = Convert.ToInt32(Parametros[Enumeradores.Parametro.VendaPlanoCIA_PlanoPlusIdentificadorNovo]);

            Plano objPlanoBasico = Plano.LoadObject(PlanoBasicoIdentificador);
            Plano objPlanoIndicado = Plano.LoadObject(PlanoIndicadoIdentificador);
            Plano objPlanoPlus = Plano.LoadObject(PlanoPlusIdentificador);

            decimal precoComDescontoBasico = objPlanoBasico.ValorBase;
            decimal precoComDescontoIndicado = objPlanoIndicado.ValorBase;
            decimal precoComDescontoPlus = objPlanoPlus.ValorBase;

            decimal precoSemDescontoBasico = Convert.ToDecimal(Parametros[Enumeradores.Parametro.VendaPlanoCIA_PlanoBasicoValorSemDesconto]);
            decimal precoSemDescontoIndicado = Convert.ToDecimal(Parametros[Enumeradores.Parametro.VendaPlanoCIA_PlanoIndicadoValorSemDesconto]);
            decimal precoSemDescontoPlus = Convert.ToDecimal(Parametros[Enumeradores.Parametro.VendaPlanoCIA_PlanoPlusValorSemDescontoNovo]);

            int smsBasico = objPlanoBasico.QuantidadeSMS;
            int smsIndicado = objPlanoIndicado.QuantidadeSMS / objPlanoIndicado.QuantidadeParcela;
            int smsPlus = objPlanoPlus.QuantidadeSMS / objPlanoPlus.QuantidadeParcela;

            int smsBasicoBonus = objPlanoBasico.QuantidadeSMS;
            int smsIndicadoBonus = objPlanoIndicado.QuantidadeSMS / objPlanoIndicado.QuantidadeParcela;
            int smsPlusBonus = objPlanoPlus.QuantidadeSMS / objPlanoPlus.QuantidadeParcela;

            int prazoBasico = objPlanoBasico.QuantidadePrazoBoletoMaxima.Value;
            int prazoIndicado = objPlanoIndicado.QuantidadePrazoBoletoMaxima.Value;
            int prazoPlus = objPlanoPlus.QuantidadePrazoBoletoMaxima.Value;

            int visualizacaoBasico = objPlanoBasico.QuantidadeVisualizacao;

            if (base.PagamentoIdCodigoDesconto.HasValue)
            {
                // calcula beneficio 
                CodigoDesconto objCodigoDesconto =
                    new CodigoDesconto(base.PagamentoIdCodigoDesconto.Value);

                objCodigoDesconto.CalcularSMS(ref smsBasicoBonus, objPlanoBasico);
                objCodigoDesconto.CalcularSMS(ref smsIndicadoBonus, objPlanoIndicado);
                objCodigoDesconto.CalcularSMS(ref smsPlusBonus, objPlanoPlus);

                objCodigoDesconto.CalcularDesconto(ref precoComDescontoBasico, objPlanoBasico);
                objCodigoDesconto.CalcularDesconto(ref precoComDescontoIndicado, objPlanoIndicado);
                objCodigoDesconto.CalcularDesconto(ref precoComDescontoPlus, objPlanoPlus);

                //Verifica se aplicou desconto para dispara animação.
                if (precoComDescontoBasico != objPlanoBasico.ValorBase ||
                    precoComDescontoIndicado != objPlanoIndicado.ValorBase ||
                    precoComDescontoPlus != objPlanoPlus.ValorBase)
                    ScriptManager.RegisterStartupScript(this, GetType(), "click()", "javaScript:CodigoDescontoAplicarAnimacao();", true);

            }

            CultureInfo formatter = CultureInfo.CurrentCulture;

            //Basico
            litNomePlanoBasico.Text = objPlanoBasico.DescricaoPlano;
            litPlanoBasicoVisualizacao.Text = objPlanoBasico.QuantidadeVisualizacao.ToString(formatter);
            litPlanoBasicoEmail.Text = objPlanoBasico.QuantidadeVisualizacao.ToString(formatter);
            lblPlanoBasicoPor.Text = precoComDescontoBasico.ToString("0", formatter);
            txtPlanoBasicoPor.Text = precoComDescontoBasico.ToString("0", formatter);
            litPlanoBasicoPorCentavos.Text = ((int)((precoComDescontoBasico % 1) * 100)).ToString(formatter);
            litPlanoBasicoDe.Text = precoSemDescontoBasico.ToString("0", formatter);
            lblPlanoBasicoSms.Text = smsBasico.ToString(formatter);
            txtPlanoBasicoSms.Text = smsBasico.ToString(formatter);
            txtPlanoBasicoPrazo.Text = prazoBasico.ToString(formatter);
            lblPlanoBasicoSms.Text = smsBasicoBonus.ToString(formatter);
            txtPlanoBasicoSms.Text = smsBasicoBonus.ToString(formatter);

            //Indicado
            litNomePlanoIndicado.Text = objPlanoIndicado.DescricaoPlano;
            litPlanoIndicadoVisualizacao.Text = objPlanoIndicado.QuantidadeVisualizacao.ToString(formatter);
            litPlanoIndicadoEmail.Text = objPlanoIndicado.QuantidadeVisualizacao.ToString(formatter);
            lblPlanoIndicadoPor.Text = precoComDescontoIndicado.ToString("0", formatter);
            txtPlanoIndicadoPor.Text = precoComDescontoIndicado.ToString("0", formatter);
            litPlanoIndicadoPorCentavos.Text = ((int)((precoComDescontoIndicado % 1) * 100)).ToString(formatter);
            litPlanoIndicadoDe.Text = precoSemDescontoIndicado.ToString("0", formatter);
            lblPlanoIndicadoSms.Text = smsIndicado.ToString(formatter);
            txtPlanoIndicadoSms.Text = smsIndicado.ToString(formatter);
            txtPlanoIndicadoPrazo.Text = prazoIndicado.ToString(formatter);
            lblPlanoIndicadoSms.Text = smsIndicadoBonus.ToString(formatter);
            txtPlanoIndicadoSms.Text = smsIndicadoBonus.ToString(formatter);

            //Plus
            litNomePlanoPlus.Text = objPlanoPlus.DescricaoPlano;
            litPlanoPlusVisualizacao.Text = objPlanoPlus.QuantidadeVisualizacao.ToString(formatter);
            litPlanoPlusEmail.Text = objPlanoPlus.QuantidadeVisualizacao.ToString(formatter);
            lblPlanoPlusPor.Text = precoComDescontoPlus.ToString("0", formatter);
            txtPlanoPlusPor.Text = precoComDescontoPlus.ToString("0", formatter);
            litPlanoPlusPorCentavos.Text = ((int)((precoComDescontoPlus % 1) * 100)).ToString(formatter);
            litPlanoPlusDe.Text = precoSemDescontoPlus.ToString("0", formatter);
            lblPlanoPlusSms.Text = smsPlus.ToString(formatter);
            txtPlanoPlusSms.Text = smsPlus.ToString(formatter);
            txtPlanoPlusPrazo.Text = prazoPlus.ToString(formatter);
            lblPlanoPlusSms.Text = smsPlusBonus.ToString(formatter);
            txtPlanoPlusSms.Text = smsPlusBonus.ToString(formatter);

            //Customizado
            PreencherCamposCustomizado();

            // ativa ou não a edição de dados na tela
            if (TipoPlanoEditavel())
                AtivarEdicao(PermitirEdicao());

            upPnlPrazoBoleto.Update();
            upPnlPlanoBasicoPrazo.Update();
            upPnlPlanoIndicadoPrazo.Update();
            upPnlPlanoPlusPrazo.Update();
            upPlanoBasicoPor.Update();
            upPlanoIndicadoPor.Update();
            upPlanoPlusPor.Update();
            upPlanoBasicoSMS.Update();
            upPlanoIndicadoSMS.Update();
            upPlanoPlusSMS.Update();

            base.PrazoBoleto.Value = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeDiasVencimentoAposEmissaoBoletoPJ));
        }
        #endregion

        #region PreencherCamposCustomizado
        private void PreencherCamposCustomizado()
        {
            PlanoCustomizadoIdentificador = Convert.ToInt32(ddlPlanoCustomizado.SelectedValue);
            Plano objPlano = Plano.LoadObject(PlanoCustomizadoIdentificador);
            decimal precoSemDesconto = objPlano.ValorBase;
            decimal precoComDesconto = objPlano.ValorBase;
            int sms = objPlano.QuantidadeSMS / objPlano.QuantidadeParcela;
            int smsBonus = objPlano.QuantidadeSMS / objPlano.QuantidadeParcela;
            int prazo = objPlano.QuantidadePrazoBoletoMaxima.Value;

            if (base.PagamentoIdCodigoDesconto.HasValue)
            {
                // calcula beneficio 
                var objCodigoDesconto = new CodigoDesconto(base.PagamentoIdCodigoDesconto.Value);
                objCodigoDesconto.CalcularSMS(ref smsBonus, objPlano);
                objCodigoDesconto.CalcularDesconto(ref precoComDesconto, objPlano);
            }

            CultureInfo formatter = CultureInfo.CurrentCulture;

            //Basico
            txtPlanoCustomizadoPor.Text = precoComDesconto.ToString("0", formatter);
            txtPlanoCustomizadoSms.Text = sms.ToString(formatter);
            txtPlanoCustomizadoPrazo.Text = prazo.ToString(formatter);
            txtPlanoCustomizadoSms.Text = smsBonus.ToString(formatter);

            upPlanoCustomizado.Update();
        }
        #endregion

        #region GerarPDFContrato

        public void GerarPDFContrato(string razaoSocial, decimal numCNPJ, string descRua, string numeroRua, string estado,
                                     string nomeCidade, string numCEP, string nomePessoa, string numRG, decimal numCPF,
                                     decimal valorPlano, int numeroParcelas, int tempoPlano, int quantidadeVisualizacoes,
                                     int quantidadeSms, int quantidadeTanque, int quantidadeUsuarios)
        {

            var pdf = GerarContratoPlano.ContratoPadraoPdf(razaoSocial, numCNPJ, descRua, numeroRua, estado,
                                                           nomeCidade, numCEP, nomePessoa, numRG, numCPF, valorPlano,
                                                           numeroParcelas, tempoPlano, quantidadeSms, quantidadeTanque, quantidadeUsuarios);

            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=Contrato_BNE.pdf");
            Response.OutputStream.Write(pdf, 0, pdf.Length);
            Response.End();
        }

        #endregion

        #region TipoPlanoEditavel

        public bool TipoPlanoEditavel()
        {
            if (txtCodigoCredito.Text != "")
            {
                CodigoDesconto codigo = null;
                if (!BLL.CodigoDesconto.CarregarPorCodigo(txtCodigoCredito.Text, out codigo))
                {
                    ExibirMensagem("Código inválido", TipoMensagem.Erro);
                    return false;
                }

                if (string.IsNullOrEmpty(txtCodigoCredito.Text))
                    return false;

                if (codigo.TipoCodigoDesconto.IdTipoCodigoDesconto.Equals(90001))
                    return true;
            }

            return false;
        }

        #endregion

        #region AtualizaValoresContrato
        public void AtualizaValoresContrato(PlanoEscolhido planoEscolhido)
        {
            SetarIdPlanoNaSessao(planoEscolhido);

            Plano objPlano = Plano.LoadObject(base.PagamentoIdentificadorPlano.Value);

            decimal preco = 0;
            int sms = 0;
            int prazo = 0;

            if (planoEscolhido == PlanoEscolhido.Indicado)
            {
                decimal.TryParse(lblPlanoIndicadoPor.Text, out preco);
                Int32.TryParse(lblPlanoIndicadoSms.Text, out sms);
                Int32.TryParse(txtPlanoIndicadoPrazo.Text, out prazo);

                if (base.PagamentoIdCodigoDesconto.HasValue && TipoPlanoEditavel())
                {
                    decimal.TryParse(txtPlanoIndicadoPor.Text, out preco);
                    Int32.TryParse(txtPlanoIndicadoSms.Text, out sms);
                    Int32.TryParse(txtPlanoIndicadoPrazo.Text, out prazo);
                }
            }

            if (planoEscolhido == PlanoEscolhido.Plus)
            {
                decimal.TryParse(lblPlanoPlusPor.Text, out preco);
                Int32.TryParse(lblPlanoPlusSms.Text, out sms);
                Int32.TryParse(txtPlanoPlusPrazo.Text, out prazo);

                if (base.PagamentoIdCodigoDesconto.HasValue && TipoPlanoEditavel())
                {
                    decimal.TryParse(txtPlanoPlusPor.Text, out preco);
                    Int32.TryParse(txtPlanoPlusSms.Text, out sms);
                    Int32.TryParse(txtPlanoPlusPrazo.Text, out prazo);
                }
            }

            if (planoEscolhido == PlanoEscolhido.Customizado)
            {
                if (base.PagamentoIdCodigoDesconto.HasValue && TipoPlanoEditavel())
                {
                    decimal.TryParse(txtPlanoCustomizadoPor.Text, out preco);
                    Int32.TryParse(txtPlanoCustomizadoSms.Text, out sms);
                    Int32.TryParse(txtPlanoCustomizadoPrazo.Text, out prazo);
                }
            }

            ValorContrato = preco;
            QuantidadeSmsContrato = sms;
            //PrazoContrato = prazo;
            PrazoContrato = objPlano.QuantidadeParcela;
            PrazoValidadeContrato = Convert.ToInt32(objPlano.QuantidadeDiasValidade / 30);
        }
        #endregion

        #region VerContrato
        private void VerContrato()
        {
            int idUsuario = 0;

            if (IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
                idUsuario = IdUsuarioFilialPerfilLogadoEmpresa.Value;

            if (IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
                idUsuario = IdUsuarioFilialPerfilLogadoUsuarioInterno.Value;

            UsuarioFilialPerfil objUsuarioFilialPerfil;
            if (idUsuario > 0)
            {
                objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(idUsuario);

                _quantidadeUsuarios = objUsuarioFilialPerfil.Filial.RecuperarQuantidadeAcessosAdquiridos();
                int quantidadeSMSTanque = objUsuarioFilialPerfil.Filial.RecuperarCotaSMSTanque();

                objUsuarioFilialPerfil.Filial.CarregarDadosUsuarioResponsavel(out _nomePessoa, out _numCPF);
                objUsuarioFilialPerfil.Filial.RecuperarConteudoFilialParaContratoPorFilial(out _razaoSocial, out _numCNPJ, out _descRua, out _numeroRua, out _nomeCidade, out _estado, out _numeroCEP);

                if (base.PagamentoIdentificadorPlano.HasValue)
                {
                    var objPlano = Plano.LoadObject(base.PagamentoIdentificadorPlano.Value);
                    if (objPlano.FlagEnviarContrato)
                    {
                        _numeroParcelas = PrazoContrato;
                        _valorPlano = ValorContrato;
                        _quantidadeSms = QuantidadeSmsContrato;
                        _tempoPlano = PrazoValidadeContrato;

                        GerarPDFContrato(_razaoSocial, _numCNPJ, _descRua, _numeroRua, _estado, _nomeCidade, _numeroCEP, _nomePessoa, _numRG, _numCPF, _valorPlano, _numeroParcelas, _tempoPlano, _quantidadeVisualizacoes, _quantidadeSms, quantidadeSMSTanque, _quantidadeUsuarios);
                    }
                    else
                    {
                        base.ExibirMensagem("Não há necessidade de contrato para este plano!", TipoMensagem.Aviso, false);
                    }
                }
                else
                {
                    base.ExibirMensagem("Selecione um plano para visualizar o contrato!", TipoMensagem.Aviso, false);
                }
            }
        }
        #endregion

        #endregion

    }
}