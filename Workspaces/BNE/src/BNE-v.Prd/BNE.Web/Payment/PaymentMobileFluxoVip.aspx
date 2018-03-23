<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PaymentMobileFluxoVip.aspx.cs" Inherits="BNE.Web.Payment.PaymentMobileFluxoVip" %>
<!DOCTYPE html>
<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title runat="server">BNE - Mobyle Payment</title>
    <!-- Adjust Screen -->
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes">
    <!-- Roboto Font and Google icons 
    <link rel="stylesheet" href="https://fonts.googleapis.com/icon?family=Material+Icons">  -->
    <link href='https://fonts.googleapis.com/css?family=Roboto:400,300,500,700' rel='stylesheet' type='text/css' />

    <!-- Bootstrap  -->
    <link rel="stylesheet" href="css/bootstrap.min.css" />
    <!-- Main CSS (sass compiled)> -->
    <link rel="stylesheet" href="css/styles.css" />
	
	<script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <script src="//code.jquery.com/ui/1.11.4/jquery-ui.min.js"></script>
	<script type="text/javascript">
        (function (i, s, o, g, r, a, m) {
            i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
                (i[r].q = i[r].q || []).push(arguments)
            }, i[r].l = 1 * new Date(); a = s.createElement(o),
            m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
        })(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');

        var uri = document.baseURI || document.URL;
        var baseAddress = uri.replace("http://", "").replace("https://", "");
        if (baseAddress.indexOf("www.bne.com.br") == 0 || baseAddress.indexOf("bne.com.br") == 0)
            ga('create', 'UA-1937941-6', 'auto');
        else
            ga('create', 'UA-1937941-8', 'auto');

        ga('send', 'pageview');

        //Não exibir breadcrumb em mobile
        jQuery(document).ready(function ($) {
            if (navigator.userAgent.match(/Mobi/)) {
                $('#navBreadcrumbForms').css('visibility', 'hidden').css('display', 'none');
            }

            if ($("#hRecorrente").val() == "true")
                $("#divLiberacaoHoras").hide();
            else
                $("#divLiberacaoHoras").show();

        });
    </script>
</head>

<body class="mobile-payment" onload="window.scrollTo(0, 1);">
    <!-- VIP Bem-vindo -->

    <form runat="server" autocomplete="off">

        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    <asp:HiddenField runat="server"  ID="hRecorrente"/>
        <div class="fluxo-vip">
            <section id="welcome-stage" class="active">
                <div id="welcome-name" class="container-small">
                    <asp:UpdatePanel ID="updTextAcessoTopo" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                        <ContentTemplate>
                            <h4>Olá, <strong>
                                <asp:Literal runat="server" ID="litNomeClienteTopo"></asp:Literal></strong></h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div id="welcome-message" class="container-small">
                    <h2>AQUI COMEÇA UMA<br />
                        <strong>NOVA FASE</strong> NA SUA<br />
                        <strong>CARREIRA</strong>!</h2>
                </div>
                <div class="stage-main-img">
                    <img class="person" src="img/welcome-stage.png">
                </div>
                <input type="image" src="img/down-arrow.png" class="down-arrow" id="pgdown-01" onclick="scroll_to_anchor('advantages01-stage'); return false;" />
            </section>
            <!-- VIP Vantagens 01 -->
            <section id="advantages01-stage" next-color="#42a5f5">
                <div class="container-small advantage-txt">
                    <h2>QUEM É VIP TEM<br />
                        ACESSO <strong>ILIMITADO</strong> A<br />
                        <strong>TODAS AS VAGAS</strong>...</h2>
                </div>
                <input type="image" src="img/down-arrow.png" class="down-arrow" id="pgdown-02" onclick="scroll_to_anchor('advantages02-stage'); return false;" />
                <div class="stage-main-img advantage-img">
                    <img class="person" src="img/advantages-01.png">
                </div>
            </section>
            <!-- VIP Vantagens 02 -->
            <section id="advantages02-stage" next-color="#1976d2">
                <div class="container-small advantage-txt">
                    <h2>RECEBE UM AVISO<br />
                        A CADA <strong>VISUALIZAÇÃO</strong><br />
                        NO SEU CURRÍCULO...</h2>
                </div>
                <input type="image" src="img/down-arrow.png" class="down-arrow" id="pgdown-03" onclick="scroll_to_anchor('summary-stage'); return false;" />
                <div class="stage-main-img advantage-img">
                    <img class="person" src="img/advantages-02.png">
                </div>
            </section>
            <!-- Sumário de compra -->
            <section id="summary-stage" next-color="#1565c0">
                <div class="container-small advantage-txt">
                    <h2>ESTÁ <strong>SEMPRE NO</strong><br />
                        <strong>TOPO</strong> DAS PESQUISAS<br />
                        E MUITO <strong>MAIS</strong>!</h2>
                </div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                    <ContentTemplate>
                        <div class="container-small advantage-txt">
                            <asp:Literal runat="server" ID="ltValorPlanoTopo"></asp:Literal>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="container row">
                    <div class="col-xs-offset-0 col-xs-12 col-sm-offset-2 col-sm-8">
                        <input type="submit" class="btn btn-block btn-lg btn-info" value="OK, CONTINUAR" onclick="scroll_to_anchor('payment-stage'); return false;" />
                        <div class="stage-main-img advantage-img">
                        </div>
                    </div>
                    </div>
            </section>
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
                <div id="mobile-payment-intro" class="container">
            <asp:UpdatePanel ID="updTextAcesso" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                <ContentTemplate>
                    <h4>
                        <asp:Literal runat="server" ID="litNomeCliente"></asp:Literal>, você está adquirindo o <strong>
                                   <asp:Literal runat="server" ID="ltNomePlano"></asp:Literal></strong> por
                            <asp:Literal runat="server" ID="ltValorPlano"></asp:Literal>.</h4>
                </ContentTemplate>
            </asp:UpdatePanel>
            <h5 runat="server" id="primeiraGratis" style="margin-bottom:8px;">Lembrando que o primeiro mês é por nossa conta!</h5>
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
         
                <asp:UpdatePanel ID="updCartaoDeCredito" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                    <ContentTemplate>
                        <div class="payment-method-type" data-toggle="collapse" href="#credito" data-parent="#accordion">
                            <p>Cartão de Crédito</p>
                            <img class="bandeiraImg" id="imageCartaoTodos" src="../Payment/img/method-all.png" style="width:140px;"  />
                            <img class="bandeiraImg" id="imageCartao" style="display:none" src="../Payment/img/method-all.png"  />
                        </div>
                        <div class="payment-method-desc row panel-collapse collapse" id="credito">

                   <%--         <div class="col-xs-12 form-group">
                                <label for="bandeira-cartao">Bandeira do Cartão:</label>
                                
                            </div>--%>

                                         <div class="form-group" id="campo_bandeira_cartao">
                                <label for="nome-cartao">
                                    <p>Bandeira do Cartão</p>
                                </label>
                                <%--<asp:TextBox runat="server" ID="TextBox1" CssClass="form-control input-lg" placeholder="Nome do Titular do Cartão" MaxLength="200"></asp:TextBox>--%>
                                             <select class="form-control" id="selBandeira" class="form-control input-lg" onchange="AlteraBandeira()"  >
                                    <option disabled selected value="0">Bandeira do Cartão</option>
                                               <option value="aura">Aura</option>
                                               <option value="amex">Amex</option>
                                               <option value="dinners">Dinners</option>
                                               <option value="discover">Discover</option>
                                               <option value="elo">Elo</option>
                                               <option value="hipercard">Hipercard</option>
                                               <option value="master">Master</option>
                                               <option value="visa">Visa</option>
                                </select>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtNomeCartao" ErrorMessage="*Campo Obrigatório!" ForeColor="Red" ValidationGroup="CartaoDeCredito"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidator1" runat="server" EnableClientScript="True" ErrorMessage="*Dígite um nome válido" ForeColor="Red" ClientValidationFunction="valida_nome_cartao" ControlToValidate="txtNomeCartao" Display="Dynamic"></asp:CustomValidator>
                            </div>

                            <div class="form-group" id="campo_cartao_nome">
                                <label for="nome-cartao">
                                    <p>Nome do Titular do Cartão</p>
                                </label>
                                <asp:TextBox runat="server" ID="txtNomeCartao" CssClass="form-control input-lg" placeholder="Nome do Titular do Cartão" MaxLength="200"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="txtNomeCartao_required" ControlToValidate="txtNomeCartao" ErrorMessage="*Campo Obrigatório!" ForeColor="Red" ValidationGroup="CartaoDeCredito"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="txtNomeCartao_customValidation" runat="server" EnableClientScript="True" ErrorMessage="*Dígite um nome válido" ForeColor="Red" ClientValidationFunction="valida_nome_cartao" ControlToValidate="txtNomeCartao" Display="Dynamic"></asp:CustomValidator>
                            </div>

                            <div class="col-xs-12 form-group" id="campo_cartao_credito">
                                <label for="num-cartao">
                                    <p>Número do cartão</p>
                                </label>
                                <asp:TextBox ID="txtNumeroCartao" runat="server" onkeyup="ValidaBandeira();" CssClass="form-control input-lg" placeholder="Número do Cartão" />
                                <asp:RequiredFieldValidator runat="server" ID="txtNumeroCartao_required" ControlToValidate="txtNumeroCartao" ErrorMessage="*Campo Obrigatório!" ForeColor="Red" ValidationGroup="CartaoDeCredito" />
                                <asp:CustomValidator ID="txtNumeroCartao_customValidation" runat="server" EnableClientScript="true" ErrorMessage="*Dígite um número de cartão válido" ForeColor="Red" ClientValidationFunction="valida_cartao" ControlToValidate="txtNumeroCartao" Display="Dynamic" />
                            </div>
                            <div class="col-xs-6 col-sm-4 form-group" id="campo_mes_validade">
                                <label for="mes-validade">
                                    <p>Mês</p>
                                </label>
                                <asp:TextBox ID="txtMesValidade" runat="server" CssClass="form-control input-lg" placeholder="Mês" />
                                <asp:RequiredFieldValidator runat="server" ID="txtMesValidade_required" ControlToValidate="txtMesValidade" ErrorMessage="*Campo Obrigatório!" ForeColor="Red" ValidationGroup="CartaoDeCredito" />
                            </div>
                            <div class="col-xs-6 col-sm-4 form-group" id="campo_ano_validade">
                                <label for="ano-validade">
                                    <p>Ano</p>
                                </label>
                                <asp:TextBox ID="txtAnoValidade" CssClass="form-control input-lg" runat="server" placeholder="Ano" />
                                <asp:RequiredFieldValidator runat="server" ID="txtAnoValidade_required" ControlToValidate="txtAnoValidade" ErrorMessage="*Campo Obrigatório!" ForeColor="Red" ValidationGroup="CartaoDeCredito" />
                            </div>
                            <div class="col-xs-6 col-xs-offset-3 col-sm-offset-0 col-sm-4 form-group" id="campo_cod_seg">
                                <label for="cod-seguranca">
                                    <p>Cód. Segurança</p>
                                </label>
                                <asp:TextBox ID="txtCodigoSeguranca" CssClass="form-control input-lg" runat="server" placeholder="CVV" MaxLength="3" ValidationGroup="CartaoDeCredito" />
                                <asp:RequiredFieldValidator runat="server" ID="txtCodigoSeguranca_required" ControlToValidate="txtCodigoSeguranca" ErrorMessage="*Campo Obrigatório!" ForeColor="Red" ValidationGroup="CartaoDeCredito" />
                            </div>
                            <div class="col-md-12">
                                <asp:CustomValidator ID="txtMesValidade_custom" runat="server" ControlToValidate="txtMesValidade" ForeColor="Red" ValidationGroup="CartaoDeCredito" ErrorMessage="O mês foi preenchido incorretamente!" ClientValidationFunction="valida_range_mes" /><br />
                                <asp:CustomValidator ID="txtAnoValidade_custom" runat="server" ControlToValidate="txtAnoValidade" ForeColor="Red" ValidationGroup="CartaoDeCredito" ErrorMessage="O ano foi preenchido incorretamente!" ClientValidationFunction="valida_range_ano" />
                            </div>
                             <div runat="server" id="div_finalizar_Cartao1" class="col-xs-offset-0 col-xs-12 col-sm-offset-2 col-sm-8">
                                        <asp:Button ID="btFinalizarCartaoDeCredito" CssClass="btn btn-block btn-lg btn-success" runat="server" data-loading-text="Carregando..." ValidationGroup="CartaoDeCredito" type="button" Text="Finalizar compra" OnClientClick="Pagamento_Cartao_De_Credito();"></asp:Button>
                                    </div>
                                    <div runat="server" id="div_finalizar_Cartao2" class="col-xs-offset-0 col-xs-12 col-sm-offset-2 col-sm-8">
                                        <asp:Button ID="btFinalizarCartaoDeCredito_2" CssClass="btn btn-block btn-lg btn-success" runat="server" data-loading-text="Carregando..." ValidationGroup="CartaoDeCredito" type="button" Text="Finalizar compra" OnClientClick="Pagamento_Cartao_De_Credito_Gratis();"></asp:Button>
                                    </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
   
                <!-- Débito Auto -->
               
                <asp:UpdatePanel ID="updDebitoOnline" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                    <ContentTemplate>
                        <div class="payment-method-type" data-toggle="collapse" href="#debtauto" data-parent="#accordion">
                            <p>Débito Automático</p>
                            <img src="img/method-bb.png">
                            <img src="img/method-bradesco.png">
                        </div>
                        <div class="payment-method-desc row panel-collapse collapse" id="debtauto">
                            <div class="col-xs-offset-0 col-xs-12 col-sm-offset-2 col-sm-8">
                                <div class="btn btn-block btn-lg btn-bb">
                                    <a href="#">
                                        <div data-toggle="modal" data-target="#redirectModalBB">
                                            <asp:ImageButton runat="server" ID="btPagamentoBB" data-loading-text="Carregando..." CausesValidtion="true" ImageUrl="img/btn-bb.png" />
                                        </div>
                                    </a>
                                </div>
                            </div>
                            <div class="col-xs-offset-0 col-xs-12 col-sm-offset-2 col-sm-8">
                                <div class="btn btn-block btn-lg btn-bradesco">
                                    <a href="#">
                                        <div data-toggle="modal" data-target="#redirectModalBradesco">
                                            <asp:ImageButton runat="server" ID="btPagamentoBradesco" data-loading-text="Carregando..." CausesValidtion="true" ImageUrl="img/btn-bradesco.png" />
                                        </div>
                                    </a>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
     
                <!-- Liberação 72 horas -->
                <div id="divLiberacaoHoras" class="validation-time">
                    <p>Liberação em até 72 horas</p>
                </div>
                <!-- Boleto -->
                
                <asp:UpdatePanel ID="updBoleto" runat="server" UpdateMode="Conditional" RenderMode="Inline" >
                    <ContentTemplate>
                      
                            <div class="payment-method-type" data-toggle="collapse" href="#boleto" data-parent="#accordion" id="boletoCollapseType" onclick="Pagamento_Boleto_Copiar();">
                                <p>Boleto</p>
                                <img src="img/method-boleto.png">
                            </div>
                            <div class="payment-method-desc row panel-collapse collapse" id="boleto">
								<div class="col-xs-offset-0 col-xs-12 ">
                                    <h5>Utilize o número do código de barras abaixo</br> para realizar o pagamento:</h5>
                                </div>
                                <div class="col-xs-offset-0 col-xs-12" style="text-align:center;">
									<textarea id="copyTxtArea" rows="2"></textarea>
								</div>
								<div class="col-xs-offset-0 col-xs-12 col-sm-offset-2 col-sm-8">
                                    <input id="btnEnviarPorEmail" type="button" class="btn btn-block btn-lg btn-primary" data-loading-text="Enviando Boleto..." value="Enviar por e-mail" onclick="Pagamento_Boleto_EnviarEmail();"></input>
                                </div>
                                <div class="col-xs-offset-0 col-xs-12 col-sm-offset-2 col-sm-8">
                                    <input id="btnCopiar" type="button" class="btn btn-block btn-lg btn-info" data-loading-text="Gerando Boleto..." value="Copiar código de Barras"
                                        data-clipboard-action="copy" data-clipboard-target="#copyTxtArea" ></input>
                                </div>
                            </div>
                       
                    </ContentTemplate>
                </asp:UpdatePanel>
     
                <!-- Débito em conta -->
       
                <asp:UpdatePanel ID="updDebitoEmConta" runat="server" href="#debtconta" UpdateMode="Conditional" RenderMode="Inline">
                    <ContentTemplate>
                       
                            <div class="payment-method-type" data-toggle="collapse" href="#debtconta" data-parent="#accordion">
                                <p>Débito em Conta</p>
                                <img src="img/method-hsbc.png">
                            </div>
                            <div class="payment-method-desc row panel-collapse collapse" id="debtconta">
                                <div class="col-xs-4 form-group input-lg">
                                    <label class="radio-inline">
                                        <input type="radio" name="tpdebt" id="debt_cpf" value="CPF" runat="server" checked="true" onclick="On_Change_Por_Radio_CNPJ_CPF()" />
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
                                    <asp:RequiredFieldValidator runat="server" ID="txtCPFDebitoHSBC_required" ControlToValidate="txtAgenciaDebitoHSBC" ErrorMessage="*Campo Obrigatório!" ForeColor="Red"  ValidationGroup="DebitoHSBC" />
                                </div>
                                <div class="col-xs-8 col-sm-6 form-group" id="campo_conta_corrente">
                                    <label for="conta-corrente">
                                        <p>Conta Corrente</p>
                                    </label>
                                    <asp:TextBox ID="txtContaCorrenteDebitoHSBC" runat="server" CssClass="form-control input-lg" placeholder="Conta Corrente" ValidationGroup="DebitoHSBC" />
                                    <asp:RequiredFieldValidator runat="server" ID="txtContaCorrenteDebitoHSBC_required" ControlToValidate="txtContaCorrenteDebitoHSBC" ErrorMessage="*Campo Obrigatório!" ForeColor="Red" ValidationGroup="DebitoHSBC" />
                                </div>
                                <div class="col-xs-4 col-sm-6 form-group" id="campo_codigo_seguranca" _>
                                    <label for="digito">
                                        <p>Dígito</p>
                                    </label>
                                    <asp:TextBox ID="txtDigitoDebitoHSBC" runat="server" CssClass="form-control input-lg" placeholder="Número do Cartão" MaxLength="16" ValidationGroup="DebitoHSBC" />
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="txtDigitoDebitoHSBC" ErrorMessage="*Campo Obrigatório!" ForeColor="Red" ValidationGroup="DebitoHSBC" />
                                </div>
                                <%--<div class="col-xs-12"><p style="color:red"><span id="textError"></span></p></div>--%>
                                <div class="col-xs-offset-0 col-xs-12 col-sm-offset-2 col-sm-8">
                                    <asp:Button ID="btnFinalizarDebitoHSBC" CssClass="btn btn-block btn-lg btn-success" ValidationGroup="DebitoHSBC" data-loading-text="Carregando..." runat="server" Text="Finalizar compra" OnClientClick="Pagamento_HSBC();"></asp:Button>
                                </div>
                            </div>
                      
                    </ContentTemplate>
                </asp:UpdatePanel>
               
                <!-- Pagseguro -->
                <asp:UpdatePanel ID="updPagseguro" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                    <ContentTemplate>
                        <a href="#">
                            <div class="payment-method-type" data-toggle="modal" data-target="#redirectModalPagSeguro">
                                <p>Pagseguro</p>
                                <img src="img/method-pagseguro.png">
                            </div>
                        </a>
                    </ContentTemplate>
                </asp:UpdatePanel>
                   
                <!-- Paypal -->
                <asp:UpdatePanel ID="updPaypal" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                    <ContentTemplate>
                            <a href="#">
                                <div class="payment-method-type" data-toggle="modal" data-target="#redirectModalPayPal">
                                    <p>Paypal</p>
                                    <img src="img/method-paypal.png">
                                </div>
                            </a>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
                </div>
        <!-- Footer -->
                <div id="mobile-payment-footer">
            <p>Banco Nacional de Empregos © 2016</p>
                </div>
        </section>
        </div>
    </form>

    <!-- Modal Bradesco -->
    <div class="modal fade" id="redirectModalBradesco" tabindex="-1" role="dialog" aria-labelledby="redirectModalLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="redirectModalLabelBradesco">Finalizando a compra</h4>
                </div>
                <div class="modal-body">
                    Você será redirecionado para o Bradesco para finalizar a compra.   
                </div>
                <div class="modal-footer">
                    <button id="btBradesc" class="btn btn-success btn-block" data-loading-text="Carregando..." onclick="Pagamento_Bradesco();">OK, entendi!</button>
                    
                </div>
            </div>
        </div>
    </div>

    <!-- Modal BB -->
    <div class="modal fade" id="redirectModalBB" tabindex="-1" role="dialog" aria-labelledby="redirectModalLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="redirectModalLabelBB">Finalizando a compra</h4>
                </div>
                <div class="modal-body">
                    Você será redirecionado para o Banco do Brasil para finalizar a compra.   
                </div>
                <div class="modal-footer">
                    <button id="btBB" class="btn btn-success btn-block" data-loading-text="Carregando..." onclick="Pagamento_Banco_Do_Brasil();">OK, entendi!</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal PayPal -->
    <div class="modal fade" id="redirectModalPayPal" tabindex="-1" role="dialog" aria-labelledby="redirectModalLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="redirectModalLabelPayPal">Finalizando a compra</h4>
                </div>
                <div class="modal-body">
                    Você será redirecionado para o PayPal para finalizar a compra.   
                </div>
                <div class="modal-footer">
                    <button id="btPayPal" class="btn btn-success btn-block" data-loading-text="Carregando..." onclick="Pagamento_PayPal();">OK, entendi!</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal PagSeguro -->
    <div class="modal fade" id="redirectModalPagSeguro" tabindex="-1" role="dialog" aria-labelledby="redirectModalLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="redirectModalLabelPagSeguro">Finalizando a compra</h4>
                </div>
                <div class="modal-body">
                    Você será redirecionado para o Pagseguro para finalizar a compra.   
                </div>
                <div class="modal-footer">
                    <button id="btPagSeguro" class="btn btn-success btn-block" data-loading-text="Carregando..." onclick="Pagamento_PagSeguro();">OK, entendi"</button>
                </div>
            </div>
        </div>
    </div>

    <!--Boleto-->
    <div class="modal fade" id="redirectModalBoleto" tabindex="-1" role="dialog" aria-labelledby="redirectModalLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="redirectModalLabelBoleto">Mensagem enviada com sucesso!</h4>
                </div>
            </div>
        </div>
    </div>

    <input id="txtPlanoAdquirido" type="hidden" runat="server" />
    <input id="txtPessoaFisica" type="hidden" runat="server" />
   
    <!-- jQuery -->
    <script src="js/jquery.min.js"></script>

    <!-- Bootstrap JS -->
    <script src="js/bootstrap.min.js"></script>
    <!-- Scroll  Accordion -->
    <script src="js/scroll-accordion.js"></script>
    <!--Mascara-->
    <script src="js/jquery.maskedinput.min.js"></script>
    <script src="js/jquery.validate.min.js"></script>
    <script src="js/clipboard.min.js"></script>
    <!--Pagamento-->
    <script src="js/pagamento-validation.js"></script>
    <script src="js/pagamento-mobile.js"></script>
</body>
</html>
