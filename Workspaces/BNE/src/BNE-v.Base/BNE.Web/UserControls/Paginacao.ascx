<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Paginacao.ascx.cs" Inherits="BNE.Web.UserControls.Paginacao" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Paginacao.css" type="text/css" rel="stylesheet" />
<asp:Panel ID="pnlPaginacao" runat="server" CssClass="Paginacao">
    <asp:ImageButton ID="btiPrimeira" runat="server" OnClick="btiPrimeira_Click" CausesValidation="false" ImageUrl="~/img/global/icones/ico_nav_primeira_24x24.gif" />
    <asp:ImageButton ID="btiAnterior" runat="server" OnClick="btiAnterior_Click" CausesValidation="false" ImageUrl="~/img/global/icones/ico_nav_anterior_24x24.gif" />
    <asp:Panel ID="pnlPaginacaoNumerica" runat="server" CssClass="PnlPaginacaoNumerica">
    </asp:Panel>
    <asp:ImageButton ID="btiProxima" runat="server" OnClick="btiProxima_Click" CausesValidation="false" ImageUrl="~/img/global/icones/ico_nav_proxima_24x24.gif" />
    <asp:ImageButton ID="btiUltima" runat="server" OnClick="btiUltima_Click" CausesValidation="false" ImageUrl="~/img/global/icones/ico_nav_ultima_24x24.gif" />
    <b><asp:Literal ID="lblEstadoPaginacao" runat="server"></asp:Literal></b>
</asp:Panel>
