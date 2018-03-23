<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTelaPagamento.ascx.cs" Inherits="BNE.Web.UserControls.ucTelaPagamento" %>
<section id="payment-stage" next-color="#e1e1e1">

    <!-- Header -->
    <div id="mobile-payment-header">
        <div id="mobile-payment-header-content" class="container">
            <div id="logo">
                <img src="img/logo-bne-pf.png" />
            </div>
            <div id="secure">
                Você está em um ambiente seguro!<img src="img/lock.png" width="9" height="12" />
            </div>
        </div>
    </div>
    <!-- Texto superior -->
    <div id="mobile-payment-intro" class="container-small">
        <h5 class="resumo">
            <span id="litNomeCliente"></span>, você está adquirindo o <strong>
                <span id="ltNomePlano"></span></strong>.</h5>
        <h5>Qual será a forma de pagamento?</h5>
    </div>
    <!-- Formas de pagamento -->

    <div id="mobile-payment-methods" class="container-small ">

        <div class="payment-methods panel-group" id="accordion">
            <!-- Liberação Imediata -->
            <div class="validation-time">
                <p>Liberação Imediata</p>
            </div>
            <!-- Cartão de crédito -->

            <div id="updCartaoDeCredito">
                <div class="payment-method-type" data-toggle="collapse" href="#credito" data-parent="#accordion">
                    <p>Cartão de Crédito</p>
                    <img id="img-bandeira" src="img/method-all.png" />
                </div>
                <div class="payment-method-desc row panel-collapse collapse" id="credito">
                    <div class="col-xs-12 form-group" id="campo_cartao_credito">
                        <label for="num-cartao">
                            <p>Número do cartão</p>
                        </label>
                        <input id="txtNumeroCartao" type="tel" onblur="valida_cartao();" placeholder="Número do Cartão" required="required" class="form-control input-lg cc-number" />
                        <span class="help-block" id="erro_cartao_credito"></span>
                    </div>
                    <div class="col-xs-6 col-sm-4 form-group" id="campo_mes_validade">
                        <label for="mes-validade">
                            <p>Mês</p>
                        </label>
                        <input id="txtMesValidade" type="tel" class="form-control input-lg" placeholder="Mês" onblur="valida_mes_e_ano();" required="required" maxlength="2" />
                        <span class="help-block" id="erro_mes_validade"></span>
                    </div>
                    <div class="col-xs-6 col-sm-4 form-group" id="campo_ano_validade">
                        <label for="ano-validade">
                            <p>Ano</p>
                        </label>
                        <input id="txtAnoValidade" type="tel" class="form-control input-lg" placeholder="Ano" onblur="valida_mes_e_ano();" required="required" maxlength="2" />
                        <span class="help-block" id="erro_ano_validade"></span>
                    </div>
                    <div class="col-xs-6 col-xs-offset-3 col-sm-offset-0 col-sm-4 form-group" id="campo_cod_seg">
                        <label for="cod-seguranca">
                            <p>Cód. Segurança</p>
                        </label>
                        <input id="txtCodigoSeguranca" type="tel" class="form-control input-lg cc-cvc" onblur="valida_cvc();" placeholder="CVV" maxlength="3" required="required" />
                    </div>

                    <div class="col-xs-offset-0 col-xs-12 col-sm-offset-2 col-sm-8">
                        <asp:Button ID="btFinalizarCartaoDeCredito" CssClass="btn btn-block btn-lg btn-success" runat="server" data-loading-text="Carregando..." ValidationGroup="CartaoDeCredito" type="button" Text="Finalizar compra" OnClientClick="Pagamento_Cartao_De_Credito();"></asp:Button>
                    </div>
                </div>
            </div>

            <!-- Débito Auto -->

            <div id="updDebitoOnline">
                <div class="payment-method-type" data-toggle="collapse" href="#debtauto" data-parent="#accordion">
                    <p>Débito Automático</p>
                    <img src="img/method-bb.png">
                    <img src="img/method-bradesco.png">
                </div>
                <div class="payment-method-desc row panel-collapse collapse" id="debtauto">
                    <div class="col-xs-offset-0 col-xs-12 col-sm-offset-2 col-sm-8">
                        <div class="btn btn-block btn-lg btn-bb">
                            <div data-toggle="modal" data-target="#redirectModalBB">
                                <input type="image" id="btPagamentoBB" data-loading-text="Carregando..." src="img/btn-bb.png" />
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-offset-0 col-xs-12 col-sm-offset-2 col-sm-8">
                        <div class="btn btn-block btn-lg btn-bradesco">
                            <div data-toggle="modal" data-target="#redirectModalBradesco">
                                <input type="image" id="btPagamentoBradesco" data-loading-text="Carregando..." src="img/btn-bradesco.png" />
                            </div>

                        </div>
                    </div>
                </div>
            </div>

            <!-- Liberação 72 horas -->
            <div class="validation-time">
                <p>Liberação em até 72 horas</p>
            </div>
            <!-- Boleto -->

            <div id="updBoleto">

                <div class="payment-method-type" data-toggle="collapse" href="#boleto" data-parent="#accordion" id="boletoCollapseType" onclick="Pagamento_Boleto_Copiar();">
                    <p>Boleto</p>
                    <img src="img/method-boleto.png">
                </div>
                <div class="payment-method-desc row panel-collapse collapse" id="boleto">
                    <div class="col-xs-offset-0 col-xs-12 ">
                        <h5>Utilize o número do código de barras abaixo</br> para realizar o pagamento:</h5>
                    </div>
                    <div class="col-xs-offset-0 col-xs-12" style="text-align: center;">
                        <textarea id="copyTxtArea" rows="2"></textarea>
                    </div>
                    <div class="col-xs-offset-0 col-xs-12 col-sm-offset-2 col-sm-8">
                        <input id="btnEnviarPorEmail" type="button" class="btn btn-block btn-lg btn-primary" data-loading-text="Enviando Boleto..." value="Enviar por e-mail" onclick="Pagamento_Boleto_EnviarEmail();" />
                    </div>
                    <div class="col-xs-offset-0 col-xs-12 col-sm-offset-2 col-sm-8">
                        <input id="btnCopiar" type="button" class="btn btn-block btn-lg btn-info" data-loading-text="Gerando Boleto..." value="Copiar código de Barras"
                            data-clipboard-action="copy" data-clipboard-target="#copyTxtArea" />
                    </div>
                </div>
            </div>

            <!-- Débito em conta -->

            <div id="updDebitoEmConta">

                <div class="payment-method-type" data-toggle="collapse" href="#debtconta" data-parent="#accordion">
                    <p>Débito em Conta</p>
                    <img src="img/method-hsbc.png">
                </div>
                <div class="payment-method-desc row panel-collapse collapse" id="debtconta">
                    <div class="col-xs-4 form-group input-lg">
                        <label class="radio-inline">
                            <input type="radio" name="tpdebt" id="debt_cpf" value="CPF" runat="server" checked onclick="On_Change_Por_Radio_CNPJ_CPF()" />
                            <p>CPF</p>
                        </label>
                    </div>
                    <div class="col-xs-8 form-group input-lg">
                        <label class="radio-inline">
                            <input type="radio" name="tpdebt" id="debt_cnpj" value="CNPJ" runat="server" onclick="On_Change_Por_Radio_CNPJ_CPF()" />
                            <p>CNPJ</p>
                        </label>
                    </div>
                    <div class="col-xs-12 form-group" id="campo_cpf_cnpj">
                        <asp:TextBox ID="txtCPFouCNPJHSBC" runat="server" CssClass="form-control input-lg" ValidationGroup="DebitoHSBC" />
                        <asp:RequiredFieldValidator runat="server" ID="txtCPFouCNPJ_required" ControlToValidate="txtCPFouCNPJHSBC" ErrorMessage="*Campo Obrigatório!" ForeColor="Red" ValidationGroup="DebitoHSBC" />
                        <asp:CustomValidator ID="txtCPFouCNPJ_CustomValidator" runat="server" EnableClientScript="true" ForeColor="Red" ClientValidationFunction="validar_cnpj_ou_cpf" ControlToValidate="txtCPFouCNPJHSBC" Display="Dynamic" />
                    </div>
                    <div class="col-xs-12 col-sm-12 form-group" id="campo_agencia">
                        <label for="agencia">
                            <p>Agência</p>
                        </label>
                        <asp:TextBox ID="txtAgenciaDebitoHSBC" runat="server" CssClass="form-control input-lg" placeholder="Agencia" ValidationGroup="DebitoHSBC" />
                        <asp:RequiredFieldValidator runat="server" ID="txtCPFDebitoHSBC_required" ControlToValidate="txtAgenciaDebitoHSBC" ErrorMessage="*Campo Obrigatório!" ForeColor="Red" ValidationGroup="DebitoHSBC" />
                    </div>
                    <div class="col-xs-8 col-sm-6 form-group" id="campo_conta_corrente">
                        <label for="conta-corrente">
                            <p>Conta Corrente</p>
                        </label>
                        <asp:TextBox ID="txtContaCorrenteDebitoHSBC" runat="server" CssClass="form-control input-lg" placeholder="Conta Corrente" ValidationGroup="DebitoHSBC" />
                        <asp:RequiredFieldValidator runat="server" ID="txtContaCorrenteDebitoHSBC_required" ControlToValidate="txtContaCorrenteDebitoHSBC" ErrorMessage="*Campo Obrigatório!" ForeColor="Red" ValidationGroup="DebitoHSBC" />
                    </div>
                    <div class="col-xs-4 col-sm-6 form-group" id="campo_codigo_seguranca">
                        <label for="digito">
                            <p>Dígito</p>
                        </label>
                        <asp:TextBox ID="txtDigitoDebitoHSBC" runat="server" CssClass="form-control input-lg" placeholder="Número do Cartão" MaxLength="16" ValidationGroup="DebitoHSBC" />
                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="txtDigitoDebitoHSBC" ErrorMessage="*Campo Obrigatório!" ForeColor="Red" ValidationGroup="DebitoHSBC" />
                    </div>
                    <div class="col-xs-12">
                        <p style="color: red"><span id="textError"></span></p>
                    </div>
                    <div class="col-xs-offset-0 col-xs-12 col-sm-offset-2 col-sm-8">
                        <asp:Button ID="btnFinalizarDebitoHSBC" CssClass="btn btn-block btn-lg btn-success" ValidationGroup="DebitoHSBC" data-loading-text="Carregando..." runat="server" Text="Finalizar compra" OnClientClick="Pagamento_HSBC();"></asp:Button>
                    </div>
                </div>
            </div>

            <!-- Pagseguro -->
            <div id="updPagseguro">
                <a href="#">
                    <div class="payment-method-type" data-toggle="modal" data-target="#redirectModalPagSeguro">
                        <p>Pagseguro</p>
                        <img src="img/method-pagseguro.png" />
                    </div>
                </a>
            </div>

            <!-- Paypal -->
            <div id="updPaypal">
                <a href="#">
                    <div class="payment-method-type" data-toggle="modal" data-target="#redirectModalPayPal">
                        <p>Paypal</p>
                        <img src="img/method-paypal.png" />
                    </div>
                </a>
            </div>
        </div>
    </div>
    <!-- Footer -->
    <div id="mobile-payment-footer">
        <p>Banco Nacional de Empregos © 2016</p>
    </div>
</section>
