<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true" CodeBehind="ConfirmacaoPagamento.aspx.cs"
    Inherits="BNE.Web.ConfirmacaoPagamento" %>
<%@ Register Src="~/UserControls/Modais/EnvioEmail.ascx" TagPrefix="uc3" TagName="EnvioEmail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <!-- Rastreamento de transações - GA -->
    <script type="text/javascript">
        ga('require', 'ecommerce');

        ga('ecommerce:addTransaction', {
            'id': <%=IdPlanoAdquirido%>,                     // Transaction ID. Required.
            'affiliation': 'BNE',   // Affiliation or store name.
            'revenue': <%=VlrPagamento%>,               // Grand Total.
            'shipping': '0',                  // Shipping.
            'tax': '0'                     // Tax.
        });

        // add item might be called for every item in the shopping cart
        // where your ecommerce engine loops through each item in the cart and
        // prints out _addItem for each
        ga('ecommerce:addItem', {
            'id': <%=IdPlanoAdquirido%>, // Transaction ID. Required.
            'name': '<%=NomePlano%>',      // Product name. Required.
            'sku': <%=IdPlano%>,         // SKU/code.
            'category': '<%=TipoPlano%>',  // Category or variation.
            'price': <%=VlrPagamento%>,  // Unit price.
            'quantity': '1'              // Quantity.
        });

        // Custom Dimension,forma de pagamento
        ga('set', 'dimension1', '<%=FormaPagamento%>');

        ga('ecommerce:send');
    </script>
    <!-- END Rastreamento de transações - GA -->
    <!-- Adwords -->
    <script type="text/javascript">
        /* <![CDATA[ */
        var google_conversion_id = 998802791;
        var google_conversion_language = "en";
        var google_conversion_format = "2";
        var google_conversion_color = "ffffff";
        var google_conversion_label = "ZO7mCNnwqAwQ54qi3AM";
        var google_remarketing_only = false;
        /* ]]> */
    </script>
    <script type="text/javascript" src="//www.googleadservices.com/pagead/conversion.js">
    </script>
    <noscript>
        <div style="display: inline;">
            <img height="1" width="1" style="border-style: none;" alt="" src="//www.googleadservices.com/pagead/conversion/998802791/?label=ZO7mCNnwqAwQ54qi3AM&amp;guid=ON&amp;script=0" />
        </div>
    </noscript>
    <!-- END Adwords -->
    <Employer:DynamicScript runat="server" Src="/js/local/BoletoBancario.js" type="text/javascript" />
    <%--<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js"></script>--%>


    <!-- Latest compiled and minified CSS -->
    <%--<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css">--%>

    <!-- Optional theme -->
    <%--<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap-theme.min.css">--%>
    <link href="css/local/ConfirmacaoPagamento2.css" rel="stylesheet" />
<%--    <link href="http://maxcdn.bootstrapcdn.com/font-awesome/4.2.0/css/font-awesome.min.css" rel="stylesheet"/>
	<link href='http://fonts.googleapis.com/css?family=Roboto:400,400italic,300italic,500,500italic,700,700italic,900,300,100italic' rel='stylesheet' type='text/css'>--%>
	
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
   
    <div class="bredcrumb noPrint" runat="server" id="divBreadCrumb">
		<span class="tit_bdc">Passos para ser VIP:</span>
		<span class="txt_bdc">1 - Escolha o Plano</span>
		<span class="txt_bdc">/</span>
		<span class="txt_bdc">2 - Formas de Pagamento</span>
		<span class="txt_bdc">/</span>
		<span class="txt_bdc"><strong><u>3 - Confirmação</u></strong></span>
		<span class="txt_bdc"><strong><u>-</u></strong></span>
		<span class="txt_bdc"><u>Parabéns, você é <strong>VIP!</strong></u> <i class="fa fa-trophy"></i> </span>
	</div>
    <asp:Panel ID="pnlBoleto" runat="server" CssClass="pnlBoleto">
        <div class="noPrint">
            <h1>Obrigado por confiar em nossos serviços!</h1>
            <h3>Efetue o pagamento do boleto e tenha acesso completo ao plano.</h3>
        </div>
        <div>
        <div class="col-md-5 well noPrint">
            <h3>Para sua comodidade, pague em seu banco.</h3>
            <ul class="bancos">
                <li><a href="http://www.itau.com.br" target="_blank" title="Ir para o Itaú" OnClick="trackEvent('Compra VIP', 'Boleto', 'Itaú');"><img src="img/bancos/itau_icon.png" /></a></li>
                <li><a href="http://www.bb.com.br" target="_blank" title="Ir para o BB" OnClick="trackEvent('Compra VIP', 'Boleto', 'BB');"><img src="img/bancos/bb_icon.png" /></a></li>
                <li><a href="http://www.bradesco.com.br" target="_blank" title="Ir para o Bradesco" OnClick="trackEvent('Compra VIP', 'Boleto', 'Bradesco');"><img src="img/bancos/bradesco_icon.png" /></a></li>
                <li><a href="http://www.caixa.gov.br" target="_blank" title="Ir para a Caixa" OnClick="trackEvent('Compra VIP', 'Boleto', 'Caixa');"><img src="img/bancos/caixa_icon.png" /></a></li>
                <li><a href="http://www.santander.com.br" target="_blank" title="Ir para o Santander" OnClick="trackEvent('Compra VIP', 'Boleto', 'Santander');"><img src="img/bancos/santander_icon.png" /></a></li>
                <li><a href="http://www.hsbc.com.br" target="_blank" title="Ir para o HSBC" OnClick="trackEvent('Compra VIP', 'Boleto', 'HSBC');"><img src="img/bancos/hsbc_icon.png" /></a></li>
            </ul>
        </div>
         <div class="col-md-5 well well-h3 noPrint">
            <h3 style="margin-top:25px;">Precisa de Liberação Antecipada?</h3>
             <h3>Envie um e-mail com o comprovante para</h3>
             <h3><b>boleto@bne.com.br</b></h3>
        </div>
            </div>
        <div class="painel_botoes noPrint">
            <asp:Button ID="Button1" runat="server" CssClass="azul_m" Text="Imprimir" OnClientClick="return ImprimirBoletoBancario()" />
            <asp:Button ID="hlDownloadTopo" runat="server" CssClass="azul_m" Text="Salvar PDF" OnClick="hlDownload_Click" />
            <asp:Button ID="Button2" runat="server" CssClass="azul_m" Text="Enviar por E-mail" OnClick="btnEnviarPorEmail_Click" />
        </div>
        <div class="container_boleto" >
            <asp:Image ID="imgBoleto" runat="server" />
        </div>
        <div class="painel_botoes noPrint">
            <asp:Button ID="btnImprimir" runat="server" CssClass="azul_m" Text="Imprimir" OnClientClick="return ImprimirBoletoBancario()" />
            <asp:Button ID="hlDownload" runat="server" CssClass="azul_m" Text="Salvar PDF" OnClick="hlDownload_Click" />
            <asp:Button ID="btnEnviarPorEmail" runat="server" CssClass="azul_m" Text="Enviar por E-mail" OnClick="btnEnviarPorEmail_Click" />
          </div>
    </asp:Panel>

    <asp:Panel ID="pnlPlanoLiberado" runat="server" CssClass="pnlConfirmacaoCartao">
        <div class="tela_pag" runat="server" id="divPlanoLiberadoVip">
		<br><br><br>
		    <i class="fa fa-4x fa-chevron-circle-down"></i> 
		    <p class="txt_verd">Pagamento realizado com sucesso</p>
		    <h2 class="msg"><strong>Parabéns!</strong> Agora você é <strong class="verde">VIP!</strong> <i class="fa fa-trophy maior_ic"></i> </h2>
		    <%--<p class="tela_pag">Aproveite seus <strong>90 dias</strong></p>
		    <p class="tela_pag">Seu <strong class="verde">VIP</strong> é valido até <strong>25/09/2015</strong></p>--%>
             <a href="http://www.bne.com.br/vagas-de-emprego" class="btn_vervagas">Ver Vagas</a> <Br><Br>
		    <%--<a href="http://www.bne.com.br/vagas-de-emprego"  class="btn_vervagas">Ver Vagas</a> --%>
            <Br><Br>
		    <a href="SalaVip.aspx" class="btn_irhome">Ir para minha sala VIP</a> 
	    </div>
        <div runat="server" id="divPlanoLiberadoCia">
            <div class="icone_confirmacao" style="float: left;">
                <img alt=""
                    src="/img/img_modal_confirmacao_envio_curriculo_icone.png" />
            </div>
            <p class="texto_pagamento_sucesso">
                Pagamento realizado com sucesso!
            </p>
            <p class="texto_utilizar_servicos">
                Seu pagamento foi realizado e a partir de agora você já pode utilizar os serviços exclusivos do BNE. 
                Caso precise de ajuda, utilize o Atendimento Online.
            </p>
            <p><asp:Button ID="btn_sala_vip" runat="server" CssClass="botao_padrao" Text="Ir para minha sala VIP" OnClick="BtnIrParaSalaVipClick" /></p>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlAguardandoIntermediador" runat="server" CssClass="pnlAguardandoIntermediador">
        <div>
            <div class="icone_confirmacao" style="float: left;">
                <img alt=""
                    src="/img/img_modal_confirmacao_envio_curriculo_icone.png" />
            </div>
            <p class="texto_pagamento_sucesso">
                Pagamento registrado com sucesso!
            </p>
            <p class="texto_utilizar_servicos">
                Seu pagamento foi registrado no
                <asp:Label runat="server" ID="lblNomeIntermediador"></asp:Label>. Assim que recebermos a confirmação de pagamento, seu plano será Liberado.
                Caso precise de ajuda, utilize o Atendimento Online.
            </p>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlDebitoRecorrente_Aguardando" runat="server" CssClass="pnlAguardandoIntermediador">
        <div class="tela_pag" runat="server" id="divDebitoRecorrente_AguardandoVIP">
		<br><br><br>
		    <i class="fa fa-spinner fa-pulse fa-spin"></i>
		    <p class="txt_azul">Aguardando confirmação do pagamento!</p>
		    <h2 class="msg">Falta pouco para você ser  <strong class="verde">VIP!</strong> <i class="fa fa-trophy maior_ic"></i> </h2>
		    <p class="tela_pag"><strong>Quase lá...</strong><br> Seu pagamento será confirmado em até <strong>3 dias.</strong></p>
		    <%--<p class="tela_pag">Aproveite seus <strong>90 dias</strong></p>
		    <p class="tela_pag">Seu <strong class="verde">VIP</strong> é valido até <strong>25/09/2015</strong></p>--%>
		    <a href="http://www.bne.com.br/vagas-de-emprego" class="btn_vervagas">Ver Vagas</a> <Br><Br>
		    <a href="SalaVip.aspx" class="btn_irhome">Ir para minha sala VIP</a> 
	    </div>
        <div runat="server" id="divDebitoRecorrente_AguardandoCIA">
            <div class="icone_confirmacao" style="float: left;">
                <img alt=""
                    src="/img/img_modal_confirmacao_envio_curriculo_icone.png" />
            </div>
            <p class="texto_pagamento_sucesso">
                Pagamento registrado com sucesso!
            </p>
            <p class="texto_utilizar_servicos">
                Seu pagamento foi registrado. Assim que recebermos a confirmação do débito, seu plano será Liberado. Certifique-se que o saldo seja suficiente para que o pagamento seja finalizado.
                Caso precise de ajuda, utilize o Atendimento Online.
            </p>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlDebitoRecorrente_Liberado" runat="server" CssClass="pnlAguardandoIntermediador">
        <div class="tela_pag" runat="server" id="divDebitoRecorrente_LiberadoVIP">
		    <i class="fa fa-spinner fa-pulse fa-spin" style="margin-top:24px"></i>
		    <p class="txt_azul">Pagamento registrado com sucesso!!</p>
		    <p style="padding: 0 70px 0 70px">O pagamento foi registrado e seu plano deverá ser liberado após a confirmação do banco. O débito deve ocorrer em sua conta em até 48 horas. Certifique-se que o saldo seja suficiente para que o pagamento seja finalizado.</p>
		    <p class="tela_pag">Caso precise de ajuda, utilize o Atendimento Online.</p>
		    <%--<p class="tela_pag">Aproveite seus <strong>90 dias</strong></p>
		    <p class="tela_pag">Seu <strong class="verde">VIP</strong> é valido até <strong>25/09/2015</strong></p>--%>
		   <%-- <a href="http://www.bne.com.br/vagas-de-emprego" class="btn_vervagas">Ver Vagas</a> <Br><Br>
		    <a href="SalaVip.aspx" class="btn_irhome">Ir para minha sala VIP</a> --%>
	    </div>
    </asp:Panel>

     <asp:Panel ID="pnlPremiumCandidatura" runat="server" Visible="false" CssClass="pnlConfirmacaoCartao">
        <div class="tela_pag" runat="server" id="div1">
		<br><br><br>
		    <i class="fa fa-4x fa-chevron-circle-down"></i> 
		    <p class="txt_verd">Pagamento realizado com sucesso</p>
		    <h2 class="msg"><strong>Parabéns!</strong> Você se candidatou a vaga de <strong class="verde">
                <asp:Label ID="lblFuncao" runat="server"></asp:Label></strong> em <strong class="verde">
                <asp:Label ID="lblCidade" runat="server"></asp:Label></strong> <i class="fa fa-trophy maior_ic"></i> </h2><br /><br />
		    <%--<p class="tela_pag">Aproveite seus <strong>90 dias</strong></p>
		    <p class="tela_pag">Seu <strong class="verde">VIP</strong> é valido até <strong>25/09/2015</strong></p>--%>
            <asp:LinkButton ID="lnkVerVaga" runat="server" CssClass="btn_vervagas" Text="Ver Vaga"></asp:LinkButton><br />
            <asp:LinkButton ID="lnkVerVagas" runat="server" CssClass="btn_vervagas" Text="Retornar para Pesquisa de Vaga"></asp:LinkButton>
            <Br><Br>
		   
	    </div>
        <%--<div runat="server" id="div2">
            <div class="icone_confirmacao" style="float: left;">
                <img alt=""
                    src="/img/img_modal_confirmacao_envio_curriculo_icone.png" />
            </div>
            <p class="texto_pagamento_sucesso">
                Pagamento realizado com sucesso!
            </p>
            <p class="texto_utilizar_servicos">
                Seu pagamento foi realizado e a partir de agora você já pode utilizar os serviços exclusivos do BNE. 
                Caso precise de ajuda, utilize o Atendimento Online.
            </p>
            <%--<p><asp:Button ID="Button3" runat="server" CssClass="botao_padrao" Text="Ir para minha sala VIP" OnClick="BtnIrParaSalaVipClick" /></p>--%>
        </div>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
    <uc3:EnvioEmail runat="server" ID="ucEnvioEmail" />
</asp:Content>
