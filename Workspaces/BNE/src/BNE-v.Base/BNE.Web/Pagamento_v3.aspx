<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true" CodeBehind="Pagamento_v3.aspx.cs" Inherits="BNE.Web.Pagamento_v31" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphHead" runat="server">
    <%--CSS--%>
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/Bootstrap/bootstrap.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/Pagamento/StylePagamento.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/icons/styles.css" type="text/css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphConteudo" runat="server">
    <div class="container span11">
        <%--Preço do Plano Escolhido--%>
        <div class="row clearfix">
            <div class="span8">
                <blockquote>
                    <p>Você escolheu o plano MENSAL  - R$ <strong id="">19,90</strong></p>
                </blockquote>
            </div>
        </div>
        <%--Labels--%>
        <div class="row clearfix">
            <div class="span5 text-center" style="background-color: #006F99; border-radius: 5px 5px 5px 5px">
                <h4 class="h4">Liberação Imediata</h4>
            </div>
            <div class="span6 text-center" style="background-color: #C5D9E2; border-radius: 5px 5px 5px 5px">
                <h4>Liberação em até 2 dias úteis</h4>
            </div>
        </div>
        <%--Botões--%>
        <div class="row clearfix btn-group" data-toggle="buttons-radio" style="margin-top: 4px">
            <div class="span5">
                <button type="button" class="btn btn-default btn-large btncCredit btnBlock active" id="cCredit" title="Cartão de Crédito"><span class="icon icon-credit-card"></span><small class="smallLabel">Cartão de Crédito</small></button>
                <button type="button" class="btn btn-default btn-large btnautomaticD btnBlock" id="automaticD" title="Débito Automático"><span class="icon icon-computer-accept"></span><small class="smallLabel">Débito Automático</small></button>
            </div>
            <div class="span6 pull-right">
                <button type="button" class="btn btn-default btn-large btnBoleto btnBlock" id="b" title="Boleto"><span class="icon icon-barcode"></span><small class="smallLabel">Boleto</small></button>
                <button type="button" class="btn btn-default btn-large btnPaypal btnBlock" id="payP" title="PayPal"><span class="icon icon-paypal"></span><small class="smallLabel">PayPal</small></button>
                <button type="button" class="btn btn-default btn-large btnPagseguro btnBlock" id="pagueS" title="PagSeguro"><span class="icon icon-pagseguro-2"></span><small class="smallLabel">PagSeguro</small></button>
            </div>
        </div>
        <%--Popovers de cada Forma de Pagamento--%>
        <div class="row clearfix">
            <div class="span12">
                <!--Cartão de Crédito-->
                <div class="popover popoverliberacaoImediata creditCard span12 fade in" style="display: block">
                    <div class="arrowSeta arrowCreditCard"></div>
                    <div class="span5 bandeiraCreditCard">
                        <p>
                            <img src="img/pagamento/imgMasterCard.png" alt="MASTERCARD" title="MASTERCARD" class="" style="display: block" /></p>
                        <p>
                            <img src="img/pagamento/imgVisaCard.png" alt="VISA" title="VISA" class="" style="display: none" /></p>
                    </div>
                    <div class="span7 liberacaoImediata">
                        <div id="payment-container">
                            <div class="group-payment">
                                <div class="payment-item">
                                    <asp:UpdatePanel ID="upCartaoCredito" runat="server" UpdateMode="Conditional" RenderMode="Inline" CssClass="">
                                        <ContentTemplate>


                                            <componente:AlfaNumerico ID="txtNumeroCartao" Tipo="AlfaNumerico" MensagemErroObrigatorio="Por favor, informe o número do cartão" MensagemErroFormato="Cartão de Crédito Inválido" MensagemErroValor="Cartão de Crédito Inválido"
                                                runat="server" ValidationGroup="CartaoDeCredito" placeholder="Número do Cartão de Crédito" ClientValidationFunction="valida_cartao" CssClass="validacao" />

                                            <div class="expiration-date">
                                                <label>Vencimento:</label>
                                                <asp:CustomValidator ID="CustomValidatorMesVencimento" runat="server" ControlToValidate="ddlMesVencimento" CssClass="validador" ValidationGroup="CartaoDeCredito" Text="Vencimento anterior ao mês atual" ClientValidationFunction="valida_vencimento_anterior"></asp:CustomValidator>
                                                <asp:CustomValidator ID="CustomValidatorAnoVencimento" runat="server" ControlToValidate="ddlAnoVencimento" CssClass="validador" ValidationGroup="CartaoDeCredito" Text="Vencimento anterior ao mês atual" ClientValidationFunction="valida_vencimento_anterior"></asp:CustomValidator>
                                                <asp:CustomValidator ID="CustomValidatorVencimento" runat="server" CssClass="validador" ValidationGroup="CartaoDeCredito" Text="Por favor, informe o vencimento" ClientValidationFunction="valida_vencimento"></asp:CustomValidator>
                                                <asp:DropDownList runat="server" ID="ddlMesVencimento" CausesValidation="true" AutoPostBack="false" CssClass="campo_obrigatorio">
                                                    <asp:ListItem Text="Mês" Value="00" disabled="true" Selected="True" class="invisible_option"></asp:ListItem>
                                                    <asp:ListItem Text="01" Value="01"></asp:ListItem>
                                                    <asp:ListItem Text="02" Value="02"></asp:ListItem>
                                                    <asp:ListItem Text="03" Value="03"></asp:ListItem>
                                                    <asp:ListItem Text="04" Value="04"></asp:ListItem>
                                                    <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                                    <asp:ListItem Text="06" Value="06"></asp:ListItem>
                                                    <asp:ListItem Text="07" Value="07"></asp:ListItem>
                                                    <asp:ListItem Text="08" Value="08"></asp:ListItem>
                                                    <asp:ListItem Text="09" Value="09"></asp:ListItem>
                                                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                    <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                    <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:DropDownList runat="server" ID="ddlAnoVencimento" CausesValidation="true" CssClass="campo_obrigatorio">
                                                    <asp:ListItem Text="Ano" Value="00" disabled="true" Selected="True" class="invisible_option"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="card-number">
                                                <componente:AlfaNumerico ID="txtCodigoVerificadorCartao" Tipo="AlfaNumerico" MensagemErroFormato="Código verificador incorreto" MensagemErroObrigatorio="Por favor, informe o código verificador"
                                                    runat="server" ValidationGroup="CartaoDeCredito" CssClass="validacao" />
                                                <p><i>?</i> <span>3 Digitos no verso do cartão</span></p>
                                            </div>
                                            <asp:Button runat="server" ID="btnFinalizarCartaoCredito2" CausesValidation="false" ValidationGroup="CartaoDeCredito" Text="Finalizar Compra" CssClass="btn btn-warning btn-large btn-block" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!--Débito automatico-->
                <div class="popover popoverliberacaoImediata aDebit span12 fade in" style="display: none">
                    <div class="arrowSeta arrowAutomaticD"></div>
                    <div class="span5 bandeiraDebit">
                        <p>
                            <img src="img/pagamento/imgBcoHSBC.png" style="display: block" title="" /></p>
                        <p>
                            <img src="img/pagamento/imgBcoBrasil.png" style="display: none" alt="" /></p>
                    </div>
                    <div class="span7 liberacaoImediata">
                        <div id="payment-container">
                            <div class="group-payment">
                                <div class="payment-item debitoHSBC">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                        <ContentTemplate>

                                            <componente:CPF runat="server" ID="txtCPFDebitoHSBC" Obrigatorio="true" ValidationGroup="DebitoAutomaticoHSBC" CssClass="validacao" MensagemErroObrigatorio="Por favor, informe o CPF" TextBoxCssClass="cpf_debito_hsbc campo_obrigatorio" />
                                            <asp:CustomValidator ID="CustomValidatorObrigaContaHSBC" runat="server" CssClass="validador" ValidationGroup="DebitoAutomaticoHSBC" Text="Por favor, indique o número da agência e conta com dígito verificador" ClientValidationFunction="valida_conta_corrente_hsbc_obrigatoria"></asp:CustomValidator>
                                            <asp:CustomValidator ID="CustomValidatorContaHSBC" runat="server" CssClass="validador" ValidationGroup="DebitoAutomaticoHSBC" Text="Conta corrente inválida. Poir favor verifique os dados informados." ClientValidationFunction="valida_conta_corrente_hsbc"></asp:CustomValidator>
                                            <asp:TextBox runat="server" ID="txtAgenciaDebitoHSBC" placeholder="Agência" CssClass="ag campo_obrigatorio" ValidationGroup="DebitoAutomaticoHSBC"></asp:TextBox>
                                            <div class="account">
                                                <asp:TextBox runat="server" ID="txtContaCorrenteDebitoHSBC" placeholder="Conta Corrente" CssClass="conta_corrente campo_obrigatorio" ValidationGroup="DebitoAutomaticoHSBC"></asp:TextBox>
                                                <asp:TextBox runat="server" ID="txtDigitoDebitoHSBC" placeholder="Dígito" CssClass="digito_conta campo_obrigatorio" ValidationGroup="DebitoAutomaticoHSBC" CausesValidation="true"></asp:TextBox>
                                            </div>

                                            <asp:Button runat="server" ID="btnFinalizarDebitoHSBC2" CausesValidation="false" ValidationGroup="DebitoAutomaticoHSBC" Text="Finalizar Compra" CssClass="btn btn-warning btn-large btn-block btn-clear" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <!--Boleto-->
                <div class="popover popover2DiasUteis boleto span12 fade in" style="display: none">
                    <div class="arrowSeta arrowBoleto"></div>
                    <div class="span5" style="display: none"></div>
                    <div class="span7">
                        <div id="payment-container">
                            <div class="payment-item payment-item-b adjust">

                                <dl>
                                    <dd class="adjust_dt  textFormat">- Você será redirecionado pelo <strong><em>BNE</em></strong> para a tela de impressão do <strong>boleto</strong>.
                                    </dd>
                                    <dd class="adjust_dd  textFormat">- Siga todas as instruções para concluir a compra do seu plano.
                                    </dd>
                                    <dd class="">
                                        <span class="icon icon-print"></span>- <strong>Imprima o boleto</strong> e pague no banco;
                                    </dd>
                                    <dd class="">
                                        <span class="icon icon-signal"></span>-  Se preferir; pague <strong>pela internet</strong> utilizando o código de barras do boleto;
                                    </dd>
                                    <dd class="">
                                        <span class="icon icon-calendar-1"></span>-  O prazo de validade do boleto é de <strong>3 dias úteis</strong>.
                                    </dd>
                                </dl>

                                <asp:Button runat="server" ID="btnPagamentoBoleto2" CausesValidation="false" ValidationGroup="Boleto" Text="Gerar Boleto" CssClass="btn btn-primary btn-large btn-block" />
                            </div>
                        </div>
                    </div>
                </div>

                <!--PayPal-->
                <div class="popover popover2DiasUteis payPal span12 fade in" style="display: none">
                    <div class="arrowSeta arrowPayPal"></div>
                    <div class="span5" style="display: none"></div>
                    <div class="span7">
                        <div class="payment-container">
                            <div class="payment-item payment-item-b adjust textFormat">
                                <dl>
                                    <dd class="">- Você será redirecionado pelo <strong><em>BNE</em></strong> para o site <strong>Paypal</strong> para para finalizar o pagamento.
                                    </dd>
                                    <dd>- Siga todas as instruções para concluir a compra do seu plano.
                                    </dd>
                                    <dd class="text-center">
                                        <img src="img/pagamento/imgPayPal.png" alt="" />
                                    </dd>

                                </dl>
                                <asp:Button runat="server" ID="btnPayPal2" CausesValidation="false" ValidationGroup="PayPal" Text="Finalizar Compra" CssClass="btn btn-primary btn-large btn-block" />
                            </div>
                        </div>
                    </div>
                </div>

                <!--PaySeguro-->
                <div class="popover popover2DiasUteis pagueSeguro span12 fade in" style="display: none">
                    <div class="arrowSeta arrowPagueSeguro"></div>
                    <div class="span5" style="display: none"></div>
                    <div class="span7">
                        <div class="payment-container">
                            <div class="payment-item payment-item-b adjust textFormat">
                                <dl>
                                    <dd class="">- Você será redirecionado pelo <strong><em>BNE</em></strong> para o site <strong>PagSeguro</strong> para para finalizar o pagamento.
                                    </dd>
                                    <dd>- Siga todas as instruções para concluir a compra do seu plano.
                                    </dd>
                                    <dd class="text-center">
                                        <img src="img/pagamento/imgPagSeguro.png" alt="" />
                                    </dd>
                                </dl>
                                <asp:Button runat="server" ID="btnPagSeguro2" CausesValidation="false" ValidationGroup="PagSeguro" Text="Finalizar Compra" CssClass="btn btn-primary btn-large btn-block pagSeguro" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphRodape" runat="server">
    <Employer:DynamicScript runat="server" Src="/js/local/Pagamento.js" type="text/javascript" />
    <Employer:DynamicScript runat="server" Src="/js/jquery.maskedinput.min.js" type="text/javascript" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <Employer:DynamicScript runat="server" Src="/js/bootstrap/jquery.min.js" type="text/javascript" />
    <Employer:DynamicScript runat="server" Src="/js/bootstrap/bootstrap.js" type="text/javascript" />
    <Employer:DynamicScript runat="server" Src="/js/local/Pagamento/ScriptsPagamento.js" type="text/javascript" />
</asp:Content>
