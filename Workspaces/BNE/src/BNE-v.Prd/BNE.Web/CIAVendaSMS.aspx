<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="CIAVendaSMS.aspx.cs" Inherits="BNE.Web.CIAVendaSMS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/FormaPagamento.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/Sala_Selecionador.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicScript runat="server" Src="/js/local/FormaPagamentoSMS.js" type="text/javascript" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <asp:Panel ID="pnlFormaPagamentoSMS" CssClass="painel_padrao_sala_selecionador"
        runat="server">
        <div class="qtd_sms_compra">
            <div class="linha forma_pagamento">
                <asp:Image ID="imgLogoSMS" runat="server" ImageUrl="~/img/comprar_sms_pagamento_conteudo.png" />
                <div class="mini_form">
                    <asp:Label ID="lblQtdSMS" Text="Quantidade de SMS" runat="server" CssClass="label_principal" />
                    <asp:UpdatePanel ID="upQtdSMS" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="container_campo">
                                <componente:ValorDecimal ID="txtQuantidadeSMS" runat="server" OnValorAlterado="txtQuantidadeSMS_ValorAlterado"
                                    CasasDecimais="0" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Label ID="lblValorSMSX" Text="X" runat="server" CssClass="label_principal no_margin" />
                    <asp:Label ID="lblValorSMS" Text="0,40" runat="server" CssClass="label_principal no_margin" />
                    <asp:Label ID="lblValorSMSIgual" Text="=" runat="server" CssClass="label_principal no_margin" />
                    <asp:UpdatePanel ID="upValorTotal" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                        <ContentTemplate>
                            <asp:Label ID="lblValorSMSTotal" Text="=" runat="server" CssClass="label_principal no_margin" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="btn_continuar_cinza">
                <asp:ImageButton ID="btnContinuar" runat="server" CausesValidation="false" OnClick="btnContinuar_Click"
                    AlternateText="Continuar" ImageUrl="/img/pacotes_bne/btn_continuar_cinza.png" />
            </div>
        </div>
    </asp:Panel>
    <%-- Botão de Voltar --%>
    <asp:Panel ID="Panel2" runat="server" CssClass="painel_botoes">
        <asp:Button ID="btnVoltar" runat="server" CausesValidation="false" CssClass="botao_padrao"
            OnClick="btnVoltar_Click" Text="Voltar" />
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
