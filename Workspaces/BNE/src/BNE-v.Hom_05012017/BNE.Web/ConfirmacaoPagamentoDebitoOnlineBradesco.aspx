<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true" CodeBehind="ConfirmacaoPagamentoDebitoOnlineBradesco.aspx.cs" Inherits="BNE.Web.ConfirmacaoPagamentoDebitoOnlineBradesco" %>

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
    <link href="http://maxcdn.bootstrapcdn.com/font-awesome/4.2.0/css/font-awesome.min.css" rel="stylesheet" />
    <link href='http://fonts.googleapis.com/css?family=Roboto:400,400italic,300italic,500,500italic,700,700italic,900,300,100italic' rel='stylesheet' type='text/css'>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <asp:Panel ID="pnlPlanoLiberado" runat="server" CssClass="pnlConfirmacaoCartao">

        <%--                    VIP             --%>
        <div class="tela_pag" runat="server" id="divPlanoLiberadoVip">
            <div class="bredcrumb" runat="server" id="divBreadCrumb">
                <span class="tit_bdc">Passos para ser VIP:</span>
                <span class="txt_bdc">1 - Escolha o Plano</span>
                <span class="txt_bdc">/</span>
                <span class="txt_bdc">2 - Formas de Pagamento</span>
                <span class="txt_bdc">/</span>
                <span class="txt_bdc"><strong><u>3 - Confirmação</u></strong></span>
                <span class="txt_bdc"><strong><u>-</u></strong></span>
                <span class="txt_bdc"><u>Parabéns, você é <strong>VIP!</strong></u> <i class="fa fa-trophy"></i></span>
            </div>
            <br>
            <br>
            <br>
            <i class="fa fa-check-circle"></i>
            <p class="txt_verd">Pagamento realizado com sucesso</p>
            <h2 class="msg"><strong>Parabéns!</strong> Agora você é <strong class="verde">VIP!</strong> <i class="fa fa-trophy maior_ic"></i></h2>
            <%--<p class="tela_pag">Aproveite seus <strong>90 dias</strong></p>
		    <p class="tela_pag">Seu <strong class="verde">VIP</strong> é valido até <strong>25/09/2015</strong></p>--%>
            <a href="http://www.bne.com.br/vagas-de-emprego" class="btn_vervagas">Ver Vagas</a>
            <br>
            <br>
            <a href="Default.aspx" class="btn_irhome">Ir para minha sala VIP</a>
        </div>

        <%--                CIA                 --%>
        <div class="tela_pag" runat="server" id="divPlanoLiberadoCia">
            <div class="bredcrumb" runat="server" id="div1">
                <span class="tit_bdc">Passos:</span>
                <span class="txt_bdc">1 - Escolha o Plano</span>
                <span class="txt_bdc">/</span>
                <span class="txt_bdc">2 - Formas de Pagamento</span>
                <span class="txt_bdc">/</span>
                <span class="txt_bdc"><strong><u>3 - Confirmação </u></strong><i class="fa fa-trophy"></i></span>
            </div>
            <br>
            <br>
            <br>
            <i class="fa fa-check-circle"></i>
            <p class="txt_verd">Pagamento realizado com sucesso</p>
            <h2 class="msg"><strong>Parabéns!</strong><i class="fa fa-trophy maior_ic"></i></h2>
            <br>
            <br>
            <asp:Button ID="btn_sala_vip" runat="server" class="btn_irhome" Text="Ir para minha sala selecionadora" OnClick="BtnIrParaSalaVipClick" />
        </div>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
