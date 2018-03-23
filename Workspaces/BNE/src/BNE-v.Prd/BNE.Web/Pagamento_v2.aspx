<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="Pagamento_v2.aspx.cs" Inherits="BNE.Web.Pagamento_v2" %>

<%@ Register Src="UserControls/Modais/ucTermosDeUso.ascx" TagName="Dados" TagPrefix="uc1" %>





<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <link href="icons/styles.css" rel="stylesheet">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/Pagamento.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicScript runat="server" Src="/js/jquery.simulate.js" type="text/javascript" />



</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <script src="https://assets.pagar.me/js/pagarme.min.js"></script>
    <!-- Ambiente Seguro -->
    <div class="cert_seg">
        <span><i class="fa fa-lock"></i>Você está em um ambiente seguro</span>
    </div>
    <!-- Breadcrumb 
    <div class="bredcrumb" runat="server" id="divBreadCrumb">
        <span class="tit_bdc">Passos para ser VIP:</span>
        <span class="txt_bdc">1 - Escolha o Plano </span>
        <span class="txt_bdc">/</span>
        <span class="txt_bdc"><strong><u><a href="javascript:history.go(-1);">2 - Formas de Pagamento</a></u></strong></span>
        <span class="txt_bdc">/</span>
        <span class="txt_bdc">3 - Confirmação</span>
        <span class="txt_bdc">-</span>
        <span class="txt_bdc">Parabéns, você é <strong>VIP!</strong> <i class="fa fa-trophy"></i></span>
    </div>-->
    <!-- Plano Selecionado -->
    <asp:UpdatePanel ID="updTextAcesso" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div id="formasPagtoIntro">
                <h4 class="center">
                    <asp:Label ID="lblPlano" runat="server" Text="Você está adquirindo o"></asp:Label>
                    <asp:Label ID="lblPremium" runat="server" Visible="false" Text="Você escolheu uma candidatura avulsa por:"></asp:Label>
                    <strong>
                        <asp:Literal runat="server" ID="ltNomePlano"></asp:Literal>
                        <asp:Literal runat="server" ID="ltValorPlano"></asp:Literal>
                    </strong>
                </h4>
                <h4 class="center">Qual será a forma de pagamento?
                </h4>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


    <div class="panel-group container_small formasPagtoCollapse" id="accordion" role="tablist" aria-multiselectable="true">
        <div class="liberacao" id="divLiberacaoImediata">
            <p>Liberação <strong>imediata</strong></p>
        </div>
        <!-- Cartão de Crédito -->
        <div class="panel panel-default" id="divCartaoCredito">
            <a class="panel-heading panel-title" id="headingCartaoCredito" role="tab" data-toggle="collapse" data-parent="#accordion" href="#collapseCartaoCredito" aria-expanded="true" aria-controls="collapseCartaoCredito">
                <h6>Cartão de Crédito</h6>
                <div>
                    <img class="bandeiraImg" id="imageCartao" src="Payment/img/method-all.png" />
                </div>
            </a>
            <div id="collapseCartaoCredito" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingCartaoCredito">
                <div class="panel-body">
                    <asp:UpdatePanel ID="upCartaoCredito" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                        <ContentTemplate>

                            <%--<section class="col-md-4 selec_bandeiras">
                                <span >Selecione a Bandeira</span>
                                <div>
                                    <input type="radio" name="cartaoCredito" id="visa" value="Visa" checked >
                                    <label for="visa" class="bandeira">
                                        <img src="/img/visa.png" >
                                    </label>
                                </div>
                                <div>
                                    <input type="radio" name="cartaoCredito" id="master" value="Master" >
                                    <label for="master" class="bandeira">
                                        <img src="/img/master.png">
                                    </label>
                                </div>
                            </section>--%>
                            <!-- Dados do Cartão -->
                            <section class="col-md-12 dados_cartao">
                                <!-- Bandeira -->
                                <div class="row">
                                    <div class="col-xs-12 col-xs-offset-0 col-sm-10 col-sm-offset-1">
                                        <div class="nome-campo">Bandeira do Cartão:</div>
                                        <%--<asp:CustomValidator ID="cvBandeira" ClientIDMode="Static" runat="server" ControlToValidate="selBandeira"  CssClass="validador  col-md-12" ValidationGroup="CartaoDeCredito" Text="Selecionar bandeira" ClientValidationFunction="valida_bandeira"></asp:CustomValidator>--%>
                                        <div>
                                            <select id="selBandeira" onchange="AlteraBandeira()">
                                                <option disabled selected value="0">Selecione</option>
                                                <option value="aura">Aura</option>
                                                <option value="amex">Amex</option>
                                                <option value="dinners">Dinners</option>
                                                <option value="discover">Discover</option>
                                                <option value="elo">Elo</option>
                                                <option value="hipercard">Hipercard</option>
                                                <option value="master">Master</option>
                                                <option value="visa">Visa</option>
                                            </select>
                                        </div>
                                    </div>
                                </div>

                                <!-- Número do Cartão -->
                                <div class="row">
                                    <div class="col-xs-12 col-xs-offset-0 col-sm-10 col-sm-offset-1">
                                        <div class="nome-campo">Número do Cartão:</div>
                                        <div>
                                            <componente:AlfaNumerico ID="txtNumeroCartao" OnKeyUpClient="ValidaBandeira();" ValidateRequestMode="Disabled" Tipo="AlfaNumerico" MensagemErroObrigatorio="Por favor, informe o número do cartão"
                                                MensagemErroFormato="Cartão de Crédito Inválido" MensagemErroValor="Cartão de Crédito Inválido" ValidationGroup="CartaoDeCredito"
                                                runat="server" Placeholder="Informe o número do cartão" ClientValidationFunction="valida_cartao" />
                                        </div>
                                    </div>
                                </div>
                                <!-- Titular do Cartão -->
                                <div class="row">
                                    <div class="col-xs-12 col-xs-offset-0 col-sm-10 col-sm-offset-1">
                                        <div class="nome-campo">Nome do Titular do Cartão:</div>
                                        <div>
                                            <componente:AlfaNumerico ID="txtNomeCartao" Tipo="AlfaNumerico" MensagemErroObrigatorio="Por favor, informe o nome no cartão"
                                                MensagemErroFormato="Cartão de Crédito Inválido" MensagemErroValor="Cartão de Crédito Inválido" ValidationGroup="CartaoDeCredito"
                                                runat="server" Placeholder="Informe o nome no cartão" />
                                        </div>

                                        <%--ClientValidationFunction="valida_cartao"--%>

                                        <%--     <componente:AlfaNumerico runat="server"  Tipo="AlfaNumerico" ID="txtNomeCartao" MensagemErroObrigatorio="Por favor, informe o nome no cartão"
                                            MensagemErroFormato="Nome Cartão Inválido" MensagemErroValor="Nome no Cartão Inválido" ValidationGroup="CartaoDeCredito"
                                            placeholder="Informe o nome no cartão" class="nome_car" />--%>
                                    </div>
                                </div>
                                <!-- Validade -->
                                <div class="row">
                                    <div class="col-xs-12 col-xs-offset-0 col-sm-10 col-sm-offset-1">
                                        <div class="nome-campo">Validade:</div>
                                        <div>
                                            <asp:CustomValidator ID="CustomValidatorVencimento" runat="server" CssClass="validador col-md-12" ValidationGroup="CartaoDeCredito" Text="Por favor, informe o vencimento" ClientValidationFunction="valida_vencimento"></asp:CustomValidator>
                                            <asp:CustomValidator ID="CustomValidatorMesVencimento" runat="server" ControlToValidate="ddlMesVencimento" CssClass="validador  col-md-12" ValidationGroup="CartaoDeCredito" Text="Vencimento anterior ao mês atual" ClientValidationFunction="valida_vencimento_anterior"></asp:CustomValidator>
                                            <asp:CustomValidator ID="CustomValidatorAnoVencimento" runat="server" ControlToValidate="ddlAnoVencimento" CssClass="validador" ValidationGroup="CartaoDeCredito" Text="Vencimento anterior ao mês atual" ClientValidationFunction="valida_vencimento_anterior"></asp:CustomValidator>
                                        </div>
                                        <div class="half-inputs">
                                            <div>
                                                <asp:DropDownList runat="server" ID="ddlMesVencimento" CausesValidation="true" AutoPostBack="false">
                                                    <asp:ListItem Text="Mês" Value="00" disabled="true" Selected="True" class="invisible_option"></asp:ListItem>
                                                    <asp:ListItem Text="Janeiro" Value="01"></asp:ListItem>
                                                    <asp:ListItem Text="Fevereiro" Value="02"></asp:ListItem>
                                                    <asp:ListItem Text="Março" Value="03"></asp:ListItem>
                                                    <asp:ListItem Text="Abril" Value="04"></asp:ListItem>
                                                    <asp:ListItem Text="Maio" Value="05"></asp:ListItem>
                                                    <asp:ListItem Text="Junho" Value="06"></asp:ListItem>
                                                    <asp:ListItem Text="Julho" Value="07"></asp:ListItem>
                                                    <asp:ListItem Text="Agosto" Value="08"></asp:ListItem>
                                                    <asp:ListItem Text="Setembro" Value="09"></asp:ListItem>
                                                    <asp:ListItem Text="Outubro" Value="10"></asp:ListItem>
                                                    <asp:ListItem Text="Novembro" Value="11"></asp:ListItem>
                                                    <asp:ListItem Text="Dezembro" Value="12"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div>
                                                <asp:DropDownList runat="server" ID="ddlAnoVencimento" CausesValidation="true">
                                                    <asp:ListItem Text="Ano" Value="00" disabled="true" Selected="True" class="invisible_option"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <!-- Cód Verificador -->
                                <div class="row">
                                    <div class="col-xs-12 col-xs-offset-0 col-sm-10 col-sm-offset-1">
                                        <div class="nome-campo">Código verificador:</div>
                                        <div>
                                            <componente:AlfaNumerico ID="txtCodigoVerificadorCartao" Tipo="AlfaNumerico" MensagemErroFormato="Código verificador incorreto" MensagemErroObrigatorio="Por favor, informe o código verificador"
                                                runat="server" ValidationGroup="CartaoDeCredito" />
                                        </div>
                                    </div>
                                    <%--<i class="fa fa-question-circle"></i>
                                    <div class="md_atent">
                                        <img class="img_t3" src="img/card_cod.png">
                                    </div>--%>
                                </div>
                                <!-- Finalizar -->
                                <div class="row">
                                    <div class="col-xs-12 col-xs-offset-0 col-sm-8 col-sm-offset-2  actionFrmpagto">
                                        <asp:Button runat="server" ID="btnFinalizarCartaoCredito" class="btn btn-accent full" ClientIDMode="Static" CausesValidation="true"  ValidationGroup="CartaoDeCredito" Text="Finalizar Compra" OnClick="btnFinalizarCartaoCredito_Click" />
                                    </div>
                                </div>
                                <!-- Msg Cancelamento -->
                                <div class="msg-cancelamento">
                                    <p class="center">
                                        Cancelamento de plano simples, a qualquer momento
                                        <asp:Panel ID="divInfoCancelamentoCartaoCredito" runat="server" ClientIDMode="Static">
                                            <Componentes:BalaoSaibaMais ID="BalaoSaibaMais_CancelamentoCartaoCredito" runat="server" ToolTipText="Sem burocracia, você pode encerrar seu plano no final do período contratado, optando pelo encerramento da recorrência. Na opção ''meu plano'', selecione ''cancelar plano''."
                                                Text="Saiba mais" CssClass="custom_pag_balao" ToolTipTitle="Info Cancelamento" ShowOnMouseover="True" />

                                        </asp:Panel>
                                    </p>

                                </div>
                            </section>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <asp:Panel runat="server" ID="pnlOutrasFormasPagamento">
            <!-- Débito automático -->
            <div class="panel panel-default" id="divDebitoAutomatico">
                <a class="panel-heading panel-title" id="headingDebitoAuto" role="tab" data-toggle="collapse" data-parent="#accordion" href="#collapseDebitoAuto" aria-expanded="true" aria-controls="collapseDebitoAuto">
                    <h6>Débito automático</h6>
                    <div>
                        <img class="bandeiraImg" src="Payment/img/method-bb.png" />
                        <img class="bandeiraImg" src="Payment/img/method-bradesco.png" />
                    </div>

                </a>
                <div id="collapseDebitoAuto" class="panel-collapse collapse " role="tabpanel" aria-labelledby="headingDebitoAuto">
                    <div class="panel-body">
                        <div class="cartao_credito">
                            <div class="selec_OpcaoPagto_title">Selecione o Banco:</div>
                            <div class="selec_Opcao--desc">(Você será direcionado para o site do banco)</div>
                            <ul class="selec_Banco">
                                <li class="selec_Banco_tp">
                                    <asp:ImageButton runat="server" ID="bb" CssClass="btn_pg_banco" CausesValidtion="true" ImageUrl="~/img/pagamento/selec_Opcao_BancoBrasil.png" OnClick="ButtonBB_Click" Style="margin-right: 10px;" />
                                </li>
                                <li class="selec_Banco_tp">
                                    <asp:ImageButton runat="server" ID="btPagamentoBradesco" CssClass="btn_pg_banco" CausesValidtion="true" ImageUrl="~/img/pagamento/selec_Opcao_BancoBradesco.png" OnClick="btPagamentoBradesco_Click" />
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
            <div class="liberacao" id="divLiberacao2Dias">
                <p>Liberação em até<strong> 2 dias úteis</strong></p>
            </div>
            <!-- Boleto -->
            <div class="panel panel-default" id="divBoleto">
                <a class="panel-heading panel-title" id="headingBoleto" role="tab" data-toggle="collapse" data-parent="#accordion" href="#collapseBoleto" aria-expanded="true" aria-controls="collapseBoleto">
                    <h6>Boleto</h6>
                    <img class="bandeiraImg" src="Payment/img/method-boleto.png" />
                </a>
                <div id="collapseBoleto" class="panel-collapse collapse " role="tabpanel" aria-labelledby="headingBoleto">
                    <div class="panel-body">

                        <div class="center"><strong>Você será redirecionado pelo BNE para a tela de impressão do boleto.</strong></div>
                        <div class="center">Siga todas as instruções para concluir a compra do seu plano.</div>
                        <%--<div class="boleto-list-items">
                        <div >
                            <i class="fa fa-print"></i>
                            <p>Imprima o boleto...</p>
                        </div>
                        <i class="fa fa-long-arrow-right"></i>
                        <div>
                            <i class="fa fa-desktop"></i>
                            <p>...pague pela internet utilizando o código de barras do boleto...</p>
                        </div>
                        <i class="fa fa-long-arrow-right"></i>
                        <div>
                            <i class="fa fa-calendar"></i>
                            <p>...o prazo de <strong>validade do boleto é de 1 dia útil</strong>.</p>
                        </div>
                    </div>--%>
                        <div class="col-xs-12 col-xs-offset-0 col-sm-8 col-sm-offset-2 actionFrmpagto">
                            <%--<asp:Button class="btn btn-accent full" runat="server" ID="btnPagamentoBoleto" OnClientClick="return true;"  CausesValidation="false"  Text="Gerar Boleto" OnClick="btnPagamentoBoleto_Click" />--%>
                            <asp:Button class="btn btn-accent full" runat="server" ID="btnPagamentoBoleto" OnClientClick="return true;" CausesValidation="false" Text="Gerar Boleto" OnClick="btnPagamentoBoleto_Click" />

                        </div>

                    </div>
                </div>
            </div>
            <!-- PayPal -->
            <div class="panel panel-default" id="divPayPal">
                <a class="panel-heading panel-title" id="headingPayPal" role="tab" data-toggle="collapse" data-parent="#accordion" href="#collapsePayPal" aria-expanded="true" aria-controls="collapsePayPal">
                    <h6>PayPal</h6>
                    <img class="bandeiraImg" src="Payment/img/method-paypal.png" />
                </a>
                <div id="collapsePayPal" class="panel-collapse collapse " role="tabpanel" aria-labelledby="headingPayPal">
                    <div class="panel-body">
                        <div class="center"><strong>Você será redirecionado pelo BNE para o site do Paypal para finalizar o pagamento.</strong></div>
                        <div class="center">Siga todas as instruções para concluir a compra do seu plano.</div>
                        <div class="center">
                            <img class="img_t4" src="img/paypal.png">
                        </div>
                        <div class="col-xs-12 col-xs-offset-0 col-sm-8 col-sm-offset-2 actionFrmpagto">
                            <asp:Button class="btn btn-accent full" runat="server" ID="btnPayPal" CausesValidation="true" ValidationGroup="PayPal" Text="Finalizar Compra" OnClick="btnPayPal_Click" />
                        </div>

                    </div>
                </div>
            </div>
            <!-- PagSeguro -->
            <div class="panel panel-default" id="divPagSeguro">
                <a class="panel-heading panel-title" id="headingPagSeguro" role="tab" data-toggle="collapse" data-parent="#accordion" href="#collapsePagSeguro" aria-expanded="true" aria-controls="collapsePagSeguro">
                    <h6>PagSeguro</h6>
                    <img class="bandeiraImg" src="Payment/img/method-pagseguro.png" />
                </a>
                <div id="collapsePagSeguro" class="panel-collapse collapse " role="tabpanel" aria-labelledby="headingPagSeguro">
                    <div class="panel-body">

                        <div class="center"><strong>Você será redirecionado pelo BNE para o site do PagSeguro para finalizar o pagamento.</strong></div>
                        <div class="center">Siga todas as instruções para concluir a compra do seu plano.</div>
                        <div class="center">
                            <img class="img_t4" src="img/pagseguro.png">
                        </div>
                        <div class="col-xs-12 col-xs-offset-0 col-sm-8 col-sm-offset-2 actionFrmpagto">
                            <asp:Button class="btn btn-accent full" runat="server" ID="btnPagSeguro" CausesValidation="true" ValidationGroup="PagSeguro" Text="Finalizar Compra" OnClick="btnPagSeguro_Click" />
                        </div>

                    </div>
                </div>
            </div>
            <!-- Débito em Conta -->
            <div class="panel panel-default" id="divDebitoConta">
                <a class="panel-heading panel-title" id="headingDebConta" role="tab" data-toggle="collapse" data-parent="#accordion" href="#collapseDebConta" aria-expanded="true" aria-controls="collapseDebConta">
                    <h6>Débito em Conta</h6>
                    <img class="bandeiraImg" src="Payment/img/method-hsbc.png" />
                </a>
                <div id="collapseDebConta" class="panel-collapse collapse " role="tabpanel" aria-labelledby="headingDebConta">
                    <div class="panel-body">
                        <div class="dados_cartao">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                <ContentTemplate>
                                    <!-- Radio Pessoa -->
                                    <div class="row">
                                        <div class="col-xs-6 col-xs-offset-0 col-sm-5 col-sm-offset-1 ">
                                            <asp:RadioButton ID="radbtnCPF" runat="server" Text="CPF" GroupName="TipoPessoa" AutoPostBack="true" OnCheckedChanged="radbtnCPF_OR_CNPJ_CheckedChanged" Checked="true" />
                                        </div>
                                        <div class="col-xs-6 col-sm-5 ">
                                            <asp:RadioButton ID="radbtnCNPJ" runat="server" Text="CNPJ" GroupName="TipoPessoa" AutoPostBack="true" OnCheckedChanged="radbtnCPF_OR_CNPJ_CheckedChanged" />
                                        </div>
                                    </div>
                                    <!-- CPF -->
                                    <div class="row">
                                        <div class="col-xs-12 col-xs-offset-0 col-sm-10 col-sm-offset-1 ">
                                            <div class="nome-campo">
                                                <asp:Label ID="lblTextCPFouCNPJ" runat="server" Text="CPF do titular:"></asp:Label></div>
                                            <componente:CPF runat="server" ID="txtCPFDebitoHSBC" Obrigatorio="true" ValidationGroup="DebitoAutomaticoHSBC" MensagemErroObrigatorio="Por favor, informe o CPF" />
                                            <componente:CNPJ runat="server" ID="txtCNPJDebitoHSBC" Obrigatorio="true" ValidationGroup="DebitoAutomaticoHSBC" MensagemErroObrigatorio="Por favor, informe o CNPJ" Visible="false" />
                                        </div>
                                    </div>
                                    <!-- Ag -->
                                    <div class="row">
                                        <div class="col-xs-12 col-xs-offset-0 col-sm-10 col-sm-offset-1 ">
                                            <div class="nome-campo">Agência:</div>
                                            <asp:CustomValidator ID="CustomValidatorObrigaContaHSBC" runat="server" CssClass="validador" ValidationGroup="DebitoAutomaticoHSBC" Text="Por favor, indique o número da agência e conta com dígito verificador" ClientValidationFunction="valida_agencia_hsbc_obrigatoria"></asp:CustomValidator>
                                            <asp:TextBox runat="server" ID="txtAgenciaDebitoHSBC" placeholder="Agência" ValidationGroup="DebitoAutomaticoHSBC"></asp:TextBox>
                                        </div>
                                    </div>
                                    <!-- Conta Corrente / Dígito -->
                                    <div class="row">
                                        <div class="col-xs-7 col-xs-offset-0 col-sm-6 col-sm-offset-1">
                                            <div class="nome-campo">Conta Corrente:</div>
                                            <asp:CustomValidator ID="CustomValidatorContaHSBC" runat="server" CssClass="validador" ValidationGroup="DebitoAutomaticoHSBC" Text="Conta corrente inválida. Por favor verifique os dados informados." ClientValidationFunction="valida_conta_corrente_hsbc"></asp:CustomValidator>
                                            <asp:TextBox runat="server" ID="txtContaCorrenteDebitoHSBC" placeholder="Conta Corrente" ValidationGroup="DebitoAutomaticoHSBC"></asp:TextBox>
                                        </div>
                                        <div class="col-xs-5 col-sm-4">
                                            <div class="nome-campo">Dígito: </div>
                                            <asp:TextBox runat="server" ID="txtDigitoDebitoHSBC" class="digito_conta" placeholder="Dígito" ValidationGroup="DebitoAutomaticoHSBC" CausesValidation="true"></asp:TextBox>
                                        </div>

                                    </div>
                                    <!-- Finalizar -->
                                    <div class="row">
                                        <div class="col-xs-12 col-xs-offset-0 col-sm-8 col-sm-offset-2 actionFrmpagto">
                                            <asp:Button runat="server" ID="btnFinalizarDebitoHSBC" class="btn btn-accent full" CausesValidation="true" ValidationGroup="DebitoAutomaticoHSBC" Text="Finalizar Compra" OnClick="btnFinalizarDebito_Click" />
                                        </div>
                                    </div>
                                    <!-- Msg Cancelamento -->
                                    <div class="msg-cancelamento">
                                        <p class="center">
                                            Cancelamento de plano simples, a qualquer momento
                                            <asp:Panel ID="divInfoCancelamentoHSBC" runat="server" ClientIDMode="Static">
                                                <Componentes:BalaoSaibaMais ID="BalaoSaibaMais_CancelamentoHsbc" runat="server" ToolTipText="Sem burocracia, você pode encerrar seu plano no final do período contratado, optando pelo encerramento da recorrência. Na opção ''meu plano'', selecione ''cancelar plano''."
                                                    Text="Saiba mais" CssClass="balao_pag_customizado" ToolTipTitle="Info Cancelamento" CssClassLabel="balao_saiba_mais" ShowOnMouseover="True" Style="margin-left: 355px; margin-top: -10px;" />
                                            </asp:Panel>
                                        </p>

                                    </div>
                                    <%--<div id="pnBB">
                                        <asp:ImageButton runat="server" ID="btPagamentoBB" CssClass="btn_pg_banco" CausesValidtion="true" ImageUrl="~/img/btn_pg_bb.png" OnClick="ButtonBB_Click" />
                                    </div>--%>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
    <asp:UpdatePanel runat="server" ID="upCodigoCredito">
        <ContentTemplate>
            <asp:Panel ID="pnlCodigoCredito" runat="server" Visible="false" CssClass="codigo_credito">
                <div class="cadeado">
                    <img src="img/voce_esta_em_um_site_seguro.png" />
                </div>
                <div class="containerIsenrirCupom">
                    <div class="groupAll">
                        <asp:Label ID="lblCodigoCredito" AssociatedControlID="txtCodigoCredito" Text="Código promocional:"
                            runat="server">
                            <asp:TextBox ID="txtCodigoCredito" runat="server" Columns="40" MaxLength="200" CssClass="textbox_padrao complementaTextbox"
                                AutoPostBack="true" OnTextChanged="txtCodigoCredito_TextChanged"></asp:TextBox>
                            &nbsp;<asp:Button ID="btnValidarCodigoCredito" runat="server" CssClass="buttonStyle"
                                Text="Aplicar" OnClick="btnValidarCodigoCredito_Click" />
                        </asp:Label>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel runat="server" Visible="false" ID="updTermoUso">
        <ContentTemplate>
            <asp:Panel runat="server">
                <asp:CheckBox runat="server" ID="chkTermoAceita" Checked="True" />
                <asp:LinkButton ID="btnLink" Text="Concordo com as políticas descritas no termo de uso." Style="text-decoration: underline" OnClientClick="AbrirModalTermosDeUso()" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <uc1:Dados runat="server" ID="ucTermo" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
    <Employer:DynamicScript runat="server" Src="/js/local/Pagamento.js" type="text/javascript" />
    <Employer:DynamicScript runat="server" Src="/js/jquery.maskedinput.min.js" type="text/javascript" />
    <script type="text/javascript">

        function AbrirModalTermosDeUso() {
            $("#modalTermosDeUso").show();
            document.getElementById('spModal').style.display = 'block';
        }

        function FecharModalTermosDeUso() {
            $("#modalTermosDeUso").hide();
            document.getElementById('spModal').style.display = 'none';
        }



     <%--   setTimeout(function () { CarregarTela(); }, 1);

        function CarregarTela() {
            console.log('<%= HabilitarApenasCartaoCredito %>' == '1');


            if ('<%= HabilitarApenasCartaoCredito %>' == '1') {
                console.log('apenas cartão');
                $('#lblFormasPgtoDisponiveis').text('* O pagamento deste plano pode ser realizado apenas via cartão de crédito.');
                $("#collapseCartaoCredito").collapse("show");
                return; //não efetua processo de trocas de aba caso somente cartão possa ser exibido
            }
        }--%>

        function CarregarTelaOld() {
            //$(".abas2 li:first div").addClass("selected");//Defini a primeira aba como selecionada
            //$("#contentTabs .conteudo:nth-child(1)").show();//Mostra a div relacionada com a aba selecionada
            //$(".formas_pag").show();
            $(".aba").click(function () {

                <%--if ('<%= HabilitarApenasCartaoCredito %>' == '1') {
                    $('#lblFormasPgtoDisponiveis').text('* O pagamento deste plano pode ser realizado apenas via cartão de crédito.');
                    $("#collapseCartaoCredito").collapse("show");
                    return; //não efetua processo de trocas de aba caso somente cartão possa ser exibido
                }

                $(".aba").removeClass("selected");// Remove estilo 'selected' de todas as abas
                $(this).addClass("selected");// Adcionar estilo 'selected' na aba clicada

                var indice = $(this).parent().index();// Retorna o indice da aba clicada
                indice++;//Como o indice comeca a contar do 0 devemos incrementar mais 1 para conseguimos pegar mostrar o conteudo da aba clicada
                $("#contentTabs .conteudo").hide();// Oculta todos os conteudo das abas anteriores
                $("#contentTabs .conteudo:nth-child(" + indice + ")").show();// Através do indice eu mostro o conteudo da aba clicada

                if (indice == 3) {
                    $("#conteudoBoleto").show();
                }
                $(".formas_pag").show();--%>
            });



        }

    </script>

    <script type="text/javascript">
        $(document).ready(function () {

            /*dados cartao*******/
            $("#cphConteudo_txtNomeCartao_txtValor").keyup(function () {
                var valor = $("#cphConteudo_txtNomeCartao_txtValor").val().replace(/[^a-zA-Z" "]+/g, '');
                $("#cphConteudo_txtNomeCartao_txtValor").val(valor);
            });

            $("#cphConteudo_txtNomeCartao_txtValor").blur(function () {
                var valor = $("#cphConteudo_txtNomeCartao_txtValor").val().toUpperCase();
                $("#cphConteudo_txtNomeCartao_txtValor").val(valor);
            });

            $("#cphConteudo_txtNomeCartao_txtValor").attr("placeholder", "Nome do Cartão");
            /*dados cartao*******/

            if ('<%= HabilitarApenasCartaoCredito %>' == '1') {
                TratativaCartaoCredito();
            }


            if ('<%= HabilitarFormasPagamentoPlanosRecorrentes %>' == 1) {
                if ('<%=FormaDePagamentoBoleto%>' == 'True') {
                    TratativoBoleto();
                }
                else {
                    TratativaCartaoCredito();
                }
            }

            if ('<%= DesabilitarTodosOsPagamentos %>' == '1') {

            }
        });

        function TratativaBoleto() {
            $("#divBoleto").collapse("show");
            $("#divCartaoCredito").remove();
            $("#divLiberacaoImediata").remove();
            RemoverComuns();
        }


        function TratativaCartaoCredito() {
            $("#divLiberacao2Dias").remove();
            $("#divBoleto").remove();
            RemoverComuns();

        }
        function RemoverComuns() {
            $("#divDebitoAutomatico").remove();
            $("#divPayPal").remove();
            $("#divPagSeguro").remove();
            $("#divDebitoConta").remove();
        }

        function RemoverTodos() {
            RemoverComuns();
            TratativaCartaoCredito();
            $("#divCartaoCredito").remove();
            $("#divLiberacaoImediata").remove();
        }

    </script>
</asp:Content>
