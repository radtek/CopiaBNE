<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true" CodeBehind="ConfirmacaoPagamentoDebitoOnlineBB.aspx.cs" Inherits="BNE.Web.ConfirmacaoPagamentoDebitoOnlineBB" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <script type="text/javascript" src="//www.googleadservices.com/pagead/conversion.js">
    </script>
    <noscript>
        <div style="display: inline;">
            <img height="1" width="1" style="border-style: none;" alt="" src="//www.googleadservices.com/pagead/conversion/998802791/?label=ZO7mCNnwqAwQ54qi3AM&amp;guid=ON&amp;script=0" />
        </div>
    </noscript>
    <!-- END Adwords -->
   
    <link href="css/local/ConfirmacaoPagamento2.css" rel="stylesheet" />
    <%--<link href="http://maxcdn.bootstrapcdn.com/font-awesome/4.2.0/css/font-awesome.min.css" rel="stylesheet"/>--%>
	<%--<link href='http://fonts.googleapis.com/css?family=Roboto:400,400italic,300italic,500,500italic,700,700italic,900,300,100italic' rel='stylesheet' type='text/css'>--%>
	
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <div class="bredcrumb" runat="server" id="divBreadCrumb">
		<span class="tit_bdc">Passos para ser VIP:</span>
		<span class="txt_bdc">1 - Escolha o Plano</span>
		<span class="txt_bdc">/</span>
		<span class="txt_bdc">2 - Formas de Pagamento</span>
		<span class="txt_bdc">/</span>
		<span class="txt_bdc"><strong><u>3 - Confirmação</u></strong></span>
		<span class="txt_bdc"><strong><u>-</u></strong></span>
		<span class="txt_bdc"><u>Parabéns, você é <strong>VIP!</strong></u> <i class="fa fa-trophy"></i> </span>
	</div>
    <asp:Panel ID="pnlDebitoRecorrente_Aguardando_vip" runat="server" CssClass="pnlAguardandoIntermediador">
        <div class="tela_pag" runat="server" id="divDebitoRecorrente_AguardandoVIP">
		<br /><br /><br />
		    <i class="fa fa-spinner fa-pulse fa-spin"></i>
		    <p class="txt_azul">Aguardando confirmação do pagamento!</p>
		    <h2 class="msg">Falta pouco para você ser  <strong class="verde">VIP!</strong> <i class="fa fa-trophy maior_ic"></i> </h2>
		    <br /><br />
            <p class="tela_pag"><strong class="text-quasela">Quase lá...</strong><br /> Seu pagamento está sendo processado e em alguns minutos você receberá um email de confirmação</p>
		    <%--<p class="tela_pag">Aproveite seus <strong>90 dias</strong></p>
		    <p class="tela_pag">Seu <strong class="verde">VIP</strong> é valido até <strong>25/09/2015</strong></p>--%>
		    <%--<a href="http://www.bne.com.br/vagas-de-emprego" class="btn_vervagas">Ver Vagas</a> <Br><Br>--%>
		    
             <a href="Default.aspx" class="btn_irhome">Home</a> 
	    </div>
    </asp:Panel>
    <asp:Panel ID="pnlDebitoRecorrente_Aguardando_selecionadora" runat="server" CssClass="pnlAguardandoIntermediador">
        <div class="tela_pag" runat="server" id="divDebitoRecorrente_AguardandoSelecionadora">
		<br /><br /><br />
		    <i class="fa fa-spinner fa-pulse fa-spin"></i>
		    <p class="txt_azul">Aguardando confirmação do pagamento!</p>
		    <h2 class="msg">Falta pouco... </h2>
		    <br /><br />
            <p class="tela_pag"><strong class="text-quasela">Quase lá...</strong><br /> Seu pagamento está sendo processado e em alguns minutos você receberá um email de confirmação</p>
		    <%--<p class="tela_pag">Aproveite seus <strong>90 dias</strong></p>
		    <p class="tela_pag">Seu <strong class="verde">VIP</strong> é valido até <strong>25/09/2015</strong></p>--%>
		    <%--<a href="http://www.bne.com.br/vagas-de-emprego" class="btn_vervagas">Ver Vagas</a> <Br><Br>--%>
		    
             <a href="Default.aspx" class="btn_irhome">Home</a> 
	    </div>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
