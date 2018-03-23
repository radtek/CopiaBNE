<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="SalaVipMeuPlano.aspx.cs" Inherits="BNE.Web.SalaVipMeuPlano" %>

<%@ Register Src="~/UserControls/Modais/ModalConfirmacaoRetornoPagamento.ascx" TagName="ModalConfirmacaoRetornoPagamento"
    TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicScript runat="server" Src="/js/local/Forms/SalaVip.js" type="text/javascript" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/sala_vip.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/SalaVipMeuPlano.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <asp:UpdatePanel ID="upMeuPlano" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="painel_padrao_sala_vip">
                <div class="caracteristicas_plano">
                    <ul>
                        <li>
                            <asp:Label ID="lblDataPlanoAdquirido" Text="Plano Adquirido em: " runat="server"
                                CssClass="label_campo"></asp:Label>
                            <asp:Label ID="lblDataPlanoAdquiridoValor" runat="server"></asp:Label>
                        </li>
                        <li>
                            <asp:Label ID="lblPlanoValidade" Text="Plano Válido até: " runat="server" CssClass="label_campo"></asp:Label>
                            <asp:Label ID="lblPlanoValidadeValor" runat="server"></asp:Label>
                            <asp:Label ID="lblSeparador" Visible="false" runat="server" Text=" - "></asp:Label>
                            <asp:LinkButton ID="lnkRenovarPlano" Visible="false" Text="Renovar Plano" runat="server"
                                OnClick="lnkRenovarPlano_Click"></asp:LinkButton>
                            <asp:Label ID="lblPlanoRenovado" Text="( Plano Renovado )" Visible="false" runat="server"></asp:Label>
                        </li>
                        <li>
                            <asp:Label ID="lblPlanoValor" Text="Valor do Plano: " runat="server" CssClass="label_campo"></asp:Label>
                            <asp:Label ID="lblPlanoValorTexto" runat="server"></asp:Label>
                        </li>
                        <li>
                            <asp:Label ID="lblTipoPlano" runat="server" Text="Plano de Acesso: " CssClass="label_campo"></asp:Label>
                            <asp:Label ID="lblTipoPlanoValor" runat="server"></asp:Label>
                        </li>
                    </ul>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upErroTransacaoCartao" runat="server" UpdateMode="Conditional"
        Visible="false">
        <ContentTemplate>
            <div class="painel_padrao_sala_vip">
                <div class="caracteristicas_plano">
                    <ul>
                        <li>
                            <asp:Label ID="Label11" Text="Erro ao Efetuar Pagamento. " runat="server" CssClass="label_campo"></asp:Label>
                            <asp:Button ID="Button1" runat="server" CssClass="botao_padrao" Text="Tente Novamente"
                                CausesValidation="false" OnClick="btnTenteNovamente_Click" />
                        </li>
                    </ul>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
        <asp:Button ID="btnVoltar" runat="server" CssClass="botao_padrao" Text="Voltar" CausesValidation="false"
            OnClick="btnVoltar_Click" />
    </asp:Panel>
    <uc:ModalConfirmacaoRetornoPagamento ID="ucModalConfirmacaoRetornoPagamento" runat="server">
    </uc:ModalConfirmacaoRetornoPagamento>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
