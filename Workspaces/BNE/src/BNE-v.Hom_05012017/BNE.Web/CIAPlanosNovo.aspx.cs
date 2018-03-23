using BNE.BLL;
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
using BNE.Web.UserControls.Modais;
using Enumeradores = BNE.BLL.Enumeradores;
using BNE.BLL.Assincronos;

namespace BNE.Web
{
    public partial class CiaPlanosNovo : BasePagePagamento
    {

        #region Enumeradores
        private enum EnumBotaoClicado
        {
            AplicarCodigoDesconto,
            PlanoBasico,
            PlanoIndicado,
            PlanoCustomizado,
            PlanoCem,
            Login
        }

        public enum PlanoEscolhido
        {
            Nenhum,
            Basico,
            Indicado,
            Customizado,
            Plano100
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

        #region QuantidadeVisualizacoesContrato - variavel 45
        public int QuantidadeVisualizacoesContrato
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

        #region QuantidadeSmsContrato - variavel 47
        public int QuantidadeSmsContrato
        {
            get
            {
                return Convert.ToInt32(ViewState[Chave.Temporaria.Variavel47.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel47.ToString(), value);
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

        #region PlanoBasicoIdentificador
        /// <summary>
        /// Propriedade que armazena e recupera o IdPlano
        /// </summary>
        public int PlanoCemIdentificador
        {
            get
            {
                return Convert.ToInt32(ViewState[Chave.Temporaria.VendaPlanoCIA_PlanoCemIdentificador.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.VendaPlanoCIA_PlanoCemIdentificador.ToString(), value);
            }
        }
        #endregion

        #region idPlano
        /// <summary>
        /// Propriedade que armazena e recupera o IdPlano
        /// </summary>
        public int? idPlano
        {
            get
            {
                if(ViewState[Chave.Temporaria.IdPlano.ToString()] != null)
                    return Convert.ToInt32(ViewState[Chave.Temporaria.IdPlano.ToString()]);

                return null;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.IdPlano.ToString(), value);
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
                        Enumeradores.Parametro.VendaPlanoCIA_PlanoPlusValorSemDescontoNovo,
                        Enumeradores.Parametro.VendaPlanoCIA_PlanoCem
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
                InicializarLigarAgora();
            }

            var master = (Principal)Page.Master;

            if (master != null)
                master.LoginEfetuadoSucesso += master_LoginEfetuadoSucesso;

            InicializarBarraBusca(TipoBuscaMaster.Curriculo, false, "CIAPlanosNovo");

            if(!idPlano.HasValue)
                SetarIdPlanoNaSessao(PlanoEscolhido.Nenhum);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Registra eventos controle", "LocalPageLoad();", true);

            if (base.IdFilial.HasValue)
            {
                PlanoAdquirido objPlanoAdquirido;
                if (PlanoAdquirido.CarregarPlanoAdquiridoPorSituacao(new Filial(base.IdFilial.Value), (int)Enumeradores.PlanoSituacao.Liberado, out objPlanoAdquirido))
                    Redirect(GetRouteUrl(Enumeradores.RouteCollection.ProdutoCIAPlano.ToString(), null)); //Não permitir nova compra de plano pelo usuário, caso já possua Plano Adquirido e esteja Liberado
            }
        }
        #endregion

        #region txtCodigoCredito_TextChanged
        protected void txtCodigoCredito_TextChanged(object sender, EventArgs e)
        {
            BotaoClicado = EnumBotaoClicado.AplicarCodigoDesconto;

            if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
                ValidarCodigoDesconto();
            else
                ExibirLoginEmpresa();
        }
        #endregion

        #region Cliques Botão Compra
        protected void lkbPlanoAtual_Click(object sender, EventArgs e)
        {
            //Redirect(GetRouteUrl())
        }

        protected void lkbPlanoBasico_Click(object sender, EventArgs e)
        {
            BotaoClicado = EnumBotaoClicado.PlanoBasico;
            GravarLogCRM(PlanoEscolhido.Basico);
            ProcessoCompra(PlanoEscolhido.Basico);
        }

        protected void lnkPlanoCem_Click(object sender, EventArgs e)
        {
            BotaoClicado = EnumBotaoClicado.PlanoCem;
            GravarLogCRM(PlanoEscolhido.Plano100);
            ProcessoCompra(PlanoEscolhido.Plano100);
        }

        protected void lkbPlanoIndicado_Click(object sender, EventArgs e)
        {
            GravarLogCRM(PlanoEscolhido.Indicado);

            ProcessoCompra(PlanoEscolhido.Indicado);
        }

        protected void lkbPlanoCustomizado_Click(object sender, EventArgs e)
        {
            GravarLogCRM(PlanoEscolhido.Customizado);
            ProcessoCompra(PlanoEscolhido.Customizado);
        }
        #endregion

        #region [ GravarLogCRM ]
        public void GravarLogCRM(Enum planoEsconhido)
        {
            Filial objFilial = null;

            if (!base.IdFilial.HasValue)
                return;

            objFilial = BLL.Filial.LoadObject(Convert.ToInt32(base.IdFilial.Value));

            string nomeCliente = PessoaFisica.LoadObject(base.IdPessoaFisicaLogada.Value).NomeCompleto;

            string msg = string.Format("Usuário {0} selecionou a opção de compra {1} em {2}.", nomeCliente, planoEsconhido.ToString(), DateTime.Now.ToString("dd/MM/yyyy"));
            MensagemAssincronoLogCRM.SalvarModificacoesFilialCRM(msg, objFilial, null, null);

            EnviarEmailVendedor(nomeCliente, planoEsconhido.ToString(), objFilial);
        }
        #endregion

        private void EnviarEmailVendedor(string nomeCliente,string plano, Filial objFilial)
        {
            string from = BNE.BLL.Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens, null);
            string to = objFilial.Vendedor().EmailVendedor;
            string Subject = string.Format("O Cliente {0}, clicou para compar o plano {1}.", nomeCliente, plano);
            string message = string.Format("O Cliente {0}, da empresa:{2} - CNPJ:{3} clicou para compar o plano {1}.", nomeCliente, plano, objFilial.NomeFantasia, objFilial.CNPJ);

            //Enviar Email para o Vendedor
            EmailSenderFactory
                .Create(TipoEnviadorEmail.Fila)
                .Enviar(Subject, message, null, from, to);
        }

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
                case EnumBotaoClicado.PlanoCustomizado:
                    ProcessoCompra(PlanoEscolhido.Customizado);
                    break;
                case EnumBotaoClicado.Login:
                    Response.Redirect("/Escolha-de-Plano-CIA");
                    break;
                case EnumBotaoClicado.PlanoCem:
                    ProcessoCompra(PlanoEscolhido.Plano100);
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
                ExibirLoginEmpresa();
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

        #region btnContratoBasico_Click
        protected void VerContratoBasico_Click(object sender, EventArgs e)
        {
            AtualizaValoresContratoRecorrente(PlanoEscolhido.Basico);
            VerContratoPlanoRecorrente();
        }
        #endregion

        #region btnContratoIndicado_Click
        protected void VerContratoIndicado_Click(object sender, EventArgs e)
        {
            AtualizaValoresContratoRecorrente(PlanoEscolhido.Indicado);
            VerContratoPlanoRecorrente();
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

        #region VerificarEdicaoCustomizado
        private bool VerificarEdicaoCustomizado(PlanoEscolhido planoEscolhido)
        {
            Plano objPlanoCustomizado = Plano.LoadObject(PlanoCustomizadoIdentificador);

            //Customizado
            int smsCustomizado;
            int prazoCustomizado;
            decimal precoCustomizado;

            // verifica preco
            if ((!Decimal.TryParse(txtPlanoCustomizadoPor.Text, out precoCustomizado) && planoEscolhido == PlanoEscolhido.Customizado))
            {
                ExibirMensagem("Preço inválido", TipoMensagem.Erro);
                SetarFocoNoErro(null, null, txtPlanoCustomizadoPor, planoEscolhido);
                return false;
            }

            if ((precoCustomizado < objPlanoCustomizado.ValorBaseMinimo.Value && planoEscolhido == PlanoEscolhido.Customizado && !ckbAplicarDesconto.Checked) ||
                ((!ckbAplicarDesconto.Checked && precoCustomizado < (objPlanoCustomizado.ValorBase * objPlanoCustomizado.ValorDescontoMaximo / 100)) && planoEscolhido == PlanoEscolhido.Customizado))
            {
                ExibirMensagem("Preço abaixo do mínimo", TipoMensagem.Erro);
                SetarFocoNoErro(null, null, txtPlanoCustomizadoPor, planoEscolhido);
                return false;
            }

            // verifica sms
            if ((!Int32.TryParse(txtPlanoCustomizadoSms.Text, out smsCustomizado) && planoEscolhido == PlanoEscolhido.Customizado))
            {
                ExibirMensagem("Quantidade de SMS inválida", TipoMensagem.Erro);
                SetarFocoNoErro(null,null, txtPlanoCustomizadoSms, planoEscolhido);
                return false;
            }

            if ((smsCustomizado > objPlanoCustomizado.QuantidadeSMSMaxima.Value && planoEscolhido == PlanoEscolhido.Customizado))
            {
                ExibirMensagem("Quantidade de SMS acima da máxima", TipoMensagem.Erro);
                SetarFocoNoErro(null, null, txtPlanoCustomizadoSms, planoEscolhido);
                return false;
            }

            // verifica prazo boleto
            if ((!Int32.TryParse(txtPlanoCustomizadoPrazo.Text, out prazoCustomizado) && planoEscolhido == PlanoEscolhido.Customizado))
            {
                ExibirMensagem("Prazo de boleto inválido", TipoMensagem.Erro);
                SetarFocoNoErro(null, null, txtPlanoCustomizadoPrazo, planoEscolhido);
                return false;
            }

            if ((prazoCustomizado > objPlanoCustomizado.QuantidadePrazoBoletoMaxima.Value && planoEscolhido == PlanoEscolhido.Customizado))
            {
                ExibirMensagem("Prazo de boleto acima do máximo", TipoMensagem.Erro);
                SetarFocoNoErro(null, null, txtPlanoCustomizadoPrazo, planoEscolhido);
                return false;
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

        private void ProcessoCompra(PlanoEscolhido planoEscolhido)
        {
            if (ckbAplicarDesconto.Checked && string.IsNullOrWhiteSpace(txtMotivoVendaAbaixoMinimo.Text))
            {
                base.ExibirMensagem("Para finalizar a venda é necessário informar o motivo da venda abaixo do valor mínimo no plano!", TipoMensagem.Aviso, false);
                return;
            }

            if (!base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
            {
                //Pagamento-CIA
                ExibirLoginEmpresa();
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

            SetMotivoVendaAbaixoMinimo(txtMotivoVendaAbaixoMinimo.Text);
            SetarIdPlanoNaSessao(planoEscolhido);

            if (PermitirEdicao())
            {
                if (!VerificarEdicaoCustomizado(planoEscolhido))
                    return;
            }
            else if (ConcederBeneficio())
            {
                if (!CalcularBeneficio())
                {
                    ExibirMensagem("Erro no cálculo do benefício", TipoMensagem.Erro);
                    return;
                }
            }

            CriarPlanoAdquirido();

            Redirecionar(planoEscolhido);

        }
        #endregion

        #region SetMotivoVendaAbaixoMinimo
        private void SetMotivoVendaAbaixoMinimo(string motivoVendaAbaixoMinimo)
        {
            base.PagamentoJustificativaAbaixoMinimo.Value = motivoVendaAbaixoMinimo;
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
                UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value, out objUsuarioFilial);
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
        private void SetarFocoNoErro(Control controleBasico, Control controleIndicado, Control controleCustomizado, PlanoEscolhido planoEscolhido)
        {
            if (planoEscolhido == PlanoEscolhido.Basico)
                controleBasico.Focus();
            else if (planoEscolhido == PlanoEscolhido.Indicado)
                controleIndicado.Focus();
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
                case PlanoEscolhido.Customizado:
                    base.PagamentoIdentificadorPlano.Value = PlanoCustomizadoIdentificador;
                    break;
                case PlanoEscolhido.Plano100:
                    base.PagamentoIdentificadorPlano.Value = PlanoCemIdentificador;
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
            Redirect(String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), r));
#endif
        }
        #endregion

        #region AtivarEdicaoCustomizado
        private void AtivarEdicaoCustomizado(bool ativa)
        {
            //ativa campos
            pnlPlanoCustomizado.Visible = ativa;
        }
        #endregion

        #region ConcederBeneficio
        private bool ConcederBeneficio()
        {
            if (base.PagamentoIdCodigoDesconto.HasValue)
            {
                CodigoDesconto objCodigoDesconto = CodigoDesconto.LoadObject(base.PagamentoIdCodigoDesconto.Value);

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



            if (!codigo.EdicaoPlanoCIA() && !codigo.VantagemCIA() && !codigo.Desconto03PlanoCIA() && !codigo.Desconto15PlanoCIA() && !codigo.Desconto40PlanoCIA())
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
        private void ModificarCupomAplicado()
        {
            main.Attributes["class"] = "escolha-de-plano cupomAplicado";
            //updPnlConteudo.Update();
        }
        #endregion

        #region PreencherCampos
        private void PreencherCampos()
        {
            CarregarCamposBasico();
            //CarregarCamposIndicado();
            PreencherCamposCustomizado();


            if (TipoPlanoEditavel())
                AtivarEdicaoCustomizado(PermitirEdicao());
            else
                AtivarEdicaoCustomizado(false);

            base.PrazoBoleto.Value = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeDiasVencimentoAposEmissaoBoletoPJ));
        }

        #endregion

        #region CarregarCamposBasico
        private void CarregarCamposBasico()
        {
            if (idPlano.HasValue){
                PlanoBasicoIdentificador = idPlano.Value;
                upCodigoDesconto.Visible = false;
                if (PlanoBasicoIdentificador.Equals(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PlanoCIAExperimente200))))
                    divPlanoCem.Visible = false;
            }
            else
            {
                upCodigoDesconto.Visible = true;
                PlanoBasicoIdentificador = Convert.ToInt32(Parametros[Enumeradores.Parametro.VendaPlanoCIA_PlanoBasicoIdentificador]);
            }

            PlanoCemIdentificador = Convert.ToInt32(Parametros[Enumeradores.Parametro.VendaPlanoCIA_PlanoCem]);
            #region [PlanoBasico]
            Plano objPlanoBasico = Plano.LoadObject(PlanoBasicoIdentificador);

            decimal precoComDescontoBasico = objPlanoBasico.ValorBase;
            //decimal precoSemDescontoBasico = Convert.ToDecimal(Parametros[Enumeradores.Parametro.VendaPlanoCIA_PlanoBasicoValorSemDesconto]);
            decimal precoSemDescontoBasico = objPlanoBasico.ValorBase;

            int smsBasico = objPlanoBasico.QuantidadeSMS;
            int smsBasicoBonus = objPlanoBasico.QuantidadeSMS;
            //int prazoBasico = objPlanoBasico.QuantidadePrazoBoletoMaxima != null ? objPlanoBasico.QuantidadePrazoBoletoMaxima.Value : 0;

            if (base.PagamentoIdCodigoDesconto.HasValue)
            {
                CodigoDesconto objCodigoDesconto = new CodigoDesconto(base.PagamentoIdCodigoDesconto.Value);

                objCodigoDesconto.CalcularSMS(ref smsBasicoBonus, objPlanoBasico);
                objCodigoDesconto.CalcularDesconto(ref precoComDescontoBasico, objPlanoBasico);

                if (precoComDescontoBasico != objPlanoBasico.ValorBase)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "click()", "javaScript:CodigoDescontoAplicarAnimacao();", true);
                    ModificarCupomAplicado();
                }

            }

            CultureInfo formatter = CultureInfo.CurrentCulture;

            litNomePlanoBasico.Text = String.Join("", System.Text.RegularExpressions.Regex.Split(objPlanoBasico.DescricaoPlano, @"[\d]"));
            litPlanoBasicoQtde.Text = String.Join("", System.Text.RegularExpressions.Regex.Split(objPlanoBasico.DescricaoPlano, @"[^\d]"));
            litPlanoBasicoDe.Text = objPlanoBasico.ValorDe.ToString("N2", formatter);
            litPlanoBasicoPor.Text = precoSemDescontoBasico.ToString("N2", formatter);

            //litPlano1Por.Text = 


            lblPlanoBasicoSms.Text = smsBasico.ToString(formatter);
            lblPlanoBasicoSms.Text = smsBasicoBonus.ToString(formatter);

            lblPlanoBasicoVisualizacao.Text = objPlanoBasico.QuantidadeVisualizacao.ToString(formatter);
            lblPlanoBasicoEmail.Text = objPlanoBasico.QuantidadeVisualizacao.ToString(formatter);

            litPlanoBasicoPor.Text = Math.Floor(precoComDescontoBasico).ToString("N2", formatter);

            if (objPlanoBasico.ValorDe > objPlanoBasico.ValorBase)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "click()", "javaScript:CodigoDescontoAplicarAnimacao();", true);
                ModificarCupomAplicado();
            }

            upPlanoBasicoPreco.Update();
            #endregion

            #region [PlanoCem]
            Plano objPlanoCem = Plano.LoadObject(PlanoCemIdentificador);

            precoComDescontoBasico = objPlanoCem.ValorBase;
            //decimal precoSemDescontoBasico = Convert.ToDecimal(Parametros[Enumeradores.Parametro.VendaPlanoCIA_PlanoBasicoValorSemDesconto]);
            precoSemDescontoBasico = objPlanoCem.ValorBase;

            smsBasico = objPlanoCem.QuantidadeSMS;
            smsBasicoBonus = objPlanoCem.QuantidadeSMS;
            //int prazoBasico = objPlanoBasico.QuantidadePrazoBoletoMaxima != null ? objPlanoBasico.QuantidadePrazoBoletoMaxima.Value : 0;

            if (base.PagamentoIdCodigoDesconto.HasValue)
            {
                CodigoDesconto objCodigoDesconto = new CodigoDesconto(base.PagamentoIdCodigoDesconto.Value);

                objCodigoDesconto.CalcularSMS(ref smsBasicoBonus, objPlanoCem);
                objCodigoDesconto.CalcularDesconto(ref precoComDescontoBasico, objPlanoCem);

                if (precoComDescontoBasico != objPlanoBasico.ValorBase)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "click()", "javaScript:CodigoDescontoAplicarAnimacao();", true);
                    ModificarCupomAplicado();
                }

            }

            lblPlanoCemVisualizacao.Text = objPlanoCem.QuantidadeVisualizacao.ToString(formatter);
            lblPlanoCemSms.Text = smsBasico.ToString(formatter);
            lblPlanoCemEmail.Text = objPlanoCem.QuantidadeVisualizacao.ToString(formatter);

            litNomePlanoCem.Text = String.Join("", System.Text.RegularExpressions.Regex.Split(objPlanoCem.DescricaoPlano, @"[\d]"));
            litPlanoCemQtde.Text = String.Join("", System.Text.RegularExpressions.Regex.Split(objPlanoCem.DescricaoPlano, @"[^\d]"));
            litPlanoCemDe.Text = objPlanoCem.ValorDe.ToString("N2", formatter);
            litPlanoCemPor.Text = precoSemDescontoBasico.ToString("N2", formatter);

            
            //lblPlanoBasicoSms.Text = smsBasicoBonus.ToString(formatter);

            litPlanoCemPor.Text = Math.Floor(precoComDescontoBasico).ToString("N2", formatter);

            if (objPlanoCem.ValorDe > objPlanoCem.ValorBase)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "click()", "javaScript:CodigoDescontoAplicarAnimacao();", true);
                ModificarCupomAplicado();
            }

            upPlanoCemPreco.Update();
            #endregion
            
        }
        #endregion

        #region CarregarCamposIndicado
        //private void CarregarCamposIndicado()
        //{
        //    PlanoIndicadoIdentificador = Convert.ToInt32(Parametros[Enumeradores.Parametro.VendaPlanoCIA_PlanoIndicadoIdentificador]);

        //    Plano objPlanoIndicado = Plano.LoadObject(PlanoIndicadoIdentificador);

        //    decimal precoComDescontoIndicado = objPlanoIndicado.ValorBase;
        //    //decimal precoSemDescontoIndicado = Convert.ToDecimal(Parametros[Enumeradores.Parametro.VendaPlanoCIA_PlanoIndicadoValorSemDesconto]);
        //    decimal precoSemDescontoIndicado = objPlanoIndicado.ValorBase;

        //    int smsIndicado = objPlanoIndicado.QuantidadeSMS;
        //    int smsIndicadoBonus = objPlanoIndicado.QuantidadeSMS;
        //    int prazoIndicado = objPlanoIndicado.QuantidadePrazoBoletoMaxima.Value;

        //    if (base.PagamentoIdCodigoDesconto.HasValue)
        //    {
        //        CodigoDesconto objCodigoDesconto = new CodigoDesconto(base.PagamentoIdCodigoDesconto.Value);

        //        objCodigoDesconto.CalcularSMS(ref smsIndicadoBonus, objPlanoIndicado);
        //        objCodigoDesconto.CalcularDesconto(ref precoComDescontoIndicado, objPlanoIndicado);

        //        if (precoComDescontoIndicado != objPlanoIndicado.ValorBase)
        //        {
        //            ScriptManager.RegisterStartupScript(this, GetType(), "click()", "javaScript:CodigoDescontoAplicarAnimacao();", true);
        //            ModificarCupomAplicado();
        //        }

        //    }
        //    CultureInfo formatter = CultureInfo.CurrentCulture;

        //    litNomePlanoIndicado.Text = String.Join("", System.Text.RegularExpressions.Regex.Split(objPlanoIndicado.DescricaoPlano, @"[\d]"));
        //    litPlanoIndicadoQtde.Text = String.Join("", System.Text.RegularExpressions.Regex.Split(objPlanoIndicado.DescricaoPlano, @"[^\d]"));
        //    litPlanoIndicadoPor.Text = litPlanoIndicadoDe.Text = precoSemDescontoIndicado.ToString("N2", formatter);

        //    lblPlanoIndicadoSms.Text = smsIndicado.ToString(formatter);
        //    lblPlanoIndicadoSms.Text = smsIndicadoBonus.ToString(formatter);

        //    lblPlanoIndicadoVisualizacao.Text = objPlanoIndicado.QuantidadeVisualizacao.ToString(formatter);
        //    lblPlanoIndicadoEmail.Text = objPlanoIndicado.QuantidadeVisualizacao.ToString(formatter);

        //    litPlanoIndicadoPor.Text = Math.Floor(precoComDescontoIndicado).ToString("N2", formatter);

        //    upPlanoIndicadoPreco.Update();

        //}
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
                                     decimal valorPlano, int numeroParcelas, int tempoPlano,
                                     int quantidadeSms, int quantidadeTanque, int quantidadeUsuarios)
        {

            var pdf = GerarContratoPlano.ContratoPadraoPdf(razaoSocial, numCNPJ, descRua, numeroRua, estado,
                                                           nomeCidade, numCEP, nomePessoa, numRG, numCPF,
                                                           valorPlano, numeroParcelas, tempoPlano,
                                                           quantidadeSms, quantidadeTanque, quantidadeUsuarios);

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

                        GerarPDFContrato(_razaoSocial, _numCNPJ, _descRua, _numeroRua, _estado, _nomeCidade, _numeroCEP, _nomePessoa, _numRG, _numCPF, _valorPlano, _numeroParcelas, _tempoPlano, _quantidadeSms, quantidadeSMSTanque, _quantidadeUsuarios);
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

        #region VerContratoPlanoRecorrente
        private void VerContratoPlanoRecorrente()
        {
            int idUsuario = 0;

            if (IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
                idUsuario = IdUsuarioFilialPerfilLogadoEmpresa.Value;

            if (IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
                idUsuario = IdUsuarioFilialPerfilLogadoUsuarioInterno.Value;

            var objPlano = Plano.LoadObject(base.PagamentoIdentificadorPlano.Value);

            _quantidadeVisualizacoes = objPlano.QuantidadeVisualizacao;
            _numeroParcelas = PrazoContrato;
            _valorPlano = ValorContrato;
            _quantidadeSms = QuantidadeSmsContrato;
            _quantidadeUsuarios = 1;

            var templateContrato = PlanoAdquiridoContrato.RetornaTemplateContrato(objPlano);

            UsuarioFilialPerfil objUsuarioFilialPerfil;
            if (idUsuario > 0)
            {
                objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(idUsuario);

                _quantidadeUsuarios = objUsuarioFilialPerfil.Filial.RecuperarQuantidadeAcessosAdquiridos();

                objUsuarioFilialPerfil.Filial.CarregarDadosUsuarioResponsavel(out _nomePessoa, out _numCPF);
                objUsuarioFilialPerfil.Filial.RecuperarConteudoFilialParaContratoPorFilial(out _razaoSocial, out _numCNPJ, out _descRua, out _numeroRua, out _nomeCidade, out _estado, out _numeroCEP);

                GerarPDFContratoPlanoRecorrente(_razaoSocial, _numCNPJ, _descRua, _numeroRua, _estado, _nomeCidade, _numeroCEP, _nomePessoa, _numRG, _numCPF, _valorPlano, _quantidadeVisualizacoes, _quantidadeSms, _quantidadeUsuarios, templateContrato);
            }
            else
            {
                _numCNPJ = _numCPF = 0;
                _razaoSocial = _descRua = _numeroRua = _estado = _nomeCidade = _numeroCEP = _nomePessoa = _numRG = "xxxxx";

                GerarPDFContratoPlanoRecorrente(_razaoSocial, _numCNPJ, _descRua, _numeroRua, _estado, _nomeCidade, _numeroCEP, _nomePessoa, _numRG, _numCPF, _valorPlano, _quantidadeVisualizacoes, _quantidadeSms, _quantidadeUsuarios, templateContrato: templateContrato);
            }
        }
        #endregion

        #region GerarPDFContratoPlanoRecorrente
        public void GerarPDFContratoPlanoRecorrente(string razaoSocial, decimal numCNPJ, string descRua, string numeroRua, string estado,
                                     string nomeCidade, string numCEP, string nomePessoa, string numRG, decimal numCPF,
                                     decimal valorPlano, int quantidadeVisualizacoes, int quantidadeSms, int quantidadeUsuarios, Enumeradores.TemplateContrato templateContrato)
        {

            var pdf = GerarContratoPlano.ContratoPadraoPdf_PlanoRecorrenteCia(razaoSocial, numCNPJ, descRua, numeroRua, estado,
                                                           nomeCidade, numCEP, nomePessoa, numRG, numCPF,
                                                           valorPlano, quantidadeVisualizacoes, quantidadeSms, quantidadeUsuarios, templateContrato);

            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=Contrato_BNE.pdf");
            Response.OutputStream.Write(pdf, 0, pdf.Length);
            Response.End();
        }

        #endregion

        #region AtualizaValoresContratoRecorrente
        public void AtualizaValoresContratoRecorrente(PlanoEscolhido planoEscolhido)
        {
            SetarIdPlanoNaSessao(planoEscolhido);

            Plano objPlano = Plano.LoadObject(base.PagamentoIdentificadorPlano.Value);

            decimal preco = 0;
            int sms = 0;
            int prazo = 0;
            //int visualizacao;

            if (planoEscolhido == PlanoEscolhido.Basico)
            {
                Int32.TryParse(lblPlanoBasicoSms.Text, out sms);
                decimal.TryParse(litPlanoBasicoPor.Text, out preco);
            }
            else if (planoEscolhido == PlanoEscolhido.Plano100)
            {
                Int32.TryParse(lblPlanoCemSms.Text, out sms);
                decimal.TryParse(litPlanoCemPor.Text, out preco);
            }
           

            //Descomente caso for adicionar mais um plano na tela
            //if (planoEscolhido == PlanoEscolhido.Indicado)
            //{
            //    Int32.TryParse(lblPlanoIndicadoSms.Text, out sms);
            //    decimal.TryParse(litPlanoIndicadoPor.Text, out preco);

            //    if (base.PagamentoIdCodigoDesconto.HasValue && TipoPlanoEditavel())
            //        Int32.TryParse(lblPlanoIndicadoSms.Text, out sms);
            //}

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
            PrazoContrato = objPlano.QuantidadeParcela;
            PrazoValidadeContrato = Convert.ToInt32(objPlano.QuantidadeDiasValidade / 30);
        }
        #endregion

        #region InicializarLigarAgora
        public void InicializarLigarAgora()
        {
            var objWebCallBack_Dependencia = new ucWebCallBack_Modais();
            var objRetornoStatusCIA = objWebCallBack_Dependencia.RetornarStatus(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PilotoDeFila_Cia));

            if (objRetornoStatusCIA != null && objRetornoStatusCIA.disponivel > 0)
            {
                btlLigarAgora.Attributes.Add("data-target", "#myModalComercial");
            }
            else
            {
                var objRetornoStatusAtendimento = objWebCallBack_Dependencia.RetornarStatus(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PilotoDeFila_Atendimento));
                if (objRetornoStatusAtendimento != null && objRetornoStatusAtendimento.disponivel > 0)
                {
                    btlLigarAgora.Attributes.Add("data-target", "#myModalComercial");
                }
                else
                {
                    btlLigarAgora.Attributes.Add("data-target", "#myModalMensagem");
                }
            }
        }
        #endregion

        protected void VerContrato1006_Click(object sender, EventArgs e)
        {
            AtualizaValoresContratoRecorrente(PlanoEscolhido.Plano100);
            VerContratoPlanoRecorrente();
        }

    

        #endregion


    }
}