<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="Pagamento_v2.aspx.cs" Inherits="BNE.Web.Pagamento_v2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/Pagamento.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
        <div id="payment-container">
            <div class="group-payment">
                <div class="payment-item">
                    <h3>Cartão de Crédito <i class="i-master"></i><i class="i-visa"></i></h3>
                    <h4>Liberação Imediata</h4>
                    <asp:UpdatePanel ID="upCartaoCredito" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                        <ContentTemplate>
                            <componente:AlfaNumerico ID="txtNumeroCartao" Tipo="AlfaNumerico" MensagemErroObrigatorio="Por favor, informe o número do cartão" MensagemErroFormato="Cartão de Crédito Inválido" MensagemErroValor="Cartão de Crédito Inválido"
                                runat="server" ValidationGroup="CartaoDeCredito" placeholder="Número do Cartão de Crédito" ClientValidationFunction="valida_cartao" CssClass="validacao" />
                            <div class="expiration-date">
                                <label>Vencimento</label>
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
                            <asp:Button runat="server" ID="btnFinalizarCartaoCredito" CausesValidation="true" ValidationGroup="CartaoDeCredito" Text="Finalizar" OnClick="btnFinalizarCartaoCredito_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <div class="payment-item debitoHSBC">
                <h3>Débito Automático</h3>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                        <ContentTemplate>
                        <div id="opcaoDebito" runat="server" visible="true">
                            <h4>Escolha uma das opções para efetuar o Pagamento.</h4>
                            <asp:ImageButton runat="server" ID="btPagamentoHSBC" CssClass="btn_pg_banco" CausesValidation="true" ValidationGroup="DebitoAutomaticoHSBC" ImageUrl="~/img/btn_pg_hsbc.png" OnClick="btPagamentoHSBC_Click" />
                            <asp:ImageButton runat="server" ID="btPagamentoBB" CssClass="btn_pg_banco" CausesValidtion="true" ImageUrl="~/img/btn_pg_bb.png" OnClick="ButtonBB_Click" />
                        </div>
                        <div id="debitoHSBC" runat="server" visible="false">
                            <h4>Liberação Imediata</h4>
                            <componente:CPF runat="server" ID="txtCPFDebitoHSBC" Obrigatorio="true" ValidationGroup="DebitoAutomaticoHSBC" CssClass="validacao" MensagemErroObrigatorio="Por favor, informe o CPF" TextBoxCssClass="cpf_debito_hsbc campo_obrigatorio" />
                            <asp:CustomValidator ID="CustomValidatorObrigaContaHSBC" runat="server" CssClass="validador" ValidationGroup="DebitoAutomaticoHSBC" Text="Por favor, indique o número da agência e conta com dígito verificador" ClientValidationFunction="valida_conta_corrente_hsbc_obrigatoria"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidatorContaHSBC" runat="server" CssClass="validador" ValidationGroup="DebitoAutomaticoHSBC" Text="Conta corrente inválida. Poir favor verifique os dados informados." ClientValidationFunction="valida_conta_corrente_hsbc"></asp:CustomValidator>
                            <asp:TextBox runat="server" ID="txtAgenciaDebitoHSBC" placeholder="Agência" CssClass="ag campo_obrigatorio" ValidationGroup="DebitoAutomaticoHSBC"></asp:TextBox>
                            <div class="account">
                                <asp:TextBox runat="server" ID="txtContaCorrenteDebitoHSBC" placeholder="Conta Corrente" CssClass="conta_corrente campo_obrigatorio" ValidationGroup="DebitoAutomaticoHSBC"></asp:TextBox>
                                <asp:TextBox runat="server" ID="txtDigitoDebitoHSBC" placeholder="Dígito" CssClass="digito_conta campo_obrigatorio" ValidationGroup="DebitoAutomaticoHSBC" CausesValidation="true"></asp:TextBox>
                            </div>
                            <div style="text-align:center">
                                <asp:Button runat="server" ID="bntVoltarDebito" CausesValidation="true" Text="Voltar" OnClick="bntVoltarDebito_Click" />
                            <asp:Button runat="server" ID="btnFinalizarDebitoHSBC" CausesValidation="true" ValidationGroup="DebitoAutomaticoHSBC" Text="Finalizar" OnClick="btnFinalizarDebito_Click" />
                            </div>
                        </div>
                        </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btPagamentoHSBC" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="bntVoltarDebito" EventName="Click" />
                    </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="group-payment">
                <div class="payment-item payment-item-b">
                    <h3>PagSeguro</h3>
                    <h4>Liberação em até dois dias Úteis</h4>
                    <i class="i-pagseguro"></i>
                    <i class="i-pagsegurob"></i>
                    <p>
                        Você será redirecionado pelo <strong>BNE</strong> ao site do <strong>PagSeguro</strong> para finalizar o pagamento. Siga todas as instruções para concluir a compra do seu plano.
                    </p>
                    <asp:Button runat="server" ID="btnPagSeguro" CausesValidation="true" ValidationGroup="PagSeguro" Text="Continuar" OnClick="btnPagSeguro_Click" />
                </div>

                <div class="payment-item payment-item-b">
                    <h3>PayPal</h3>
                    <h4>Liberação em até dois dias Úteis</h4>
                    <i class="i-paypal"></i>
                    <p>
                        Você será redirecionado pelo <strong>BNE</strong> ao site do <strong>PayPal</strong> para finalizar o pagamento. Siga todas as instruções para concluir a compra do seu plano.
                    </p>
                    <asp:Button runat="server" ID="btnPayPal" CausesValidation="true" ValidationGroup="PayPal" Text="Continuar" OnClick="btnPayPal_Click" />
                </div>

                <div class="payment-item payment-item-b">
                    <h3>Boleto  <i class="i-boleto"></i></h3>
                    <h4>Liberação em até dois dias Úteis</h4>

                    <p>
                        Você será redirecionado pelo <strong>BNE</strong> para a tela de impressão do <strong>boleto</strong>. Siga todas as instruções para concluir a compra do seu plano.

                    </p>
                    <asp:Button runat="server" ID="btnPagamentoBoleto" CausesValidation="true" ValidationGroup="Boleto" Text="Continuar" OnClick="btnPagamentoBoleto_Click" />
                </div>
            </div>

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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
    <Employer:DynamicScript runat="server" Src="/js/local/Pagamento.js" type="text/javascript" />
    <Employer:DynamicScript runat="server" Src="/js/jquery.maskedinput.min.js" type="text/javascript" />
</asp:Content>
