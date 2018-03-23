<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPaginacao.ascx.cs" Inherits="BNE.Web.UserControls.ucPaginacao" %>
<asp:Panel ID="pnlUCPaginacao" runat="server">
    <asp:ImageButton ID="btiPrimeira" runat="server" OnClick="btiPrimeira_Click" CausesValidation="false" ImageUrl="/img/icones/ico_nav_primeira_24x24.gif" />
    <asp:ImageButton ID="btiAnterior" runat="server" OnClick="btiAnterior_Click" CausesValidation="false" ImageUrl="/img/icones/ico_nav_anterior_24x24.gif" />
    <asp:Label ID="lblEstadoPaginacao" runat="server"></asp:Label>
    <asp:ImageButton ID="btiProxima" runat="server" OnClick="btiProxima_Click" CausesValidation="false" ImageUrl="/img/icones/ico_nav_proxima_24x24.gif" />
    <asp:ImageButton ID="btiUltima" runat="server" OnClick="btiUltima_Click" CausesValidation="false" ImageUrl="/img/icones/ico_nav_ultima_24x24.gif" />
    &nbsp;<asp:Label ID="lblIrPagina" runat="server" Text="Ir para página"></asp:Label>
    &nbsp;<asp:TextBox ID="txtNovaPagina" runat="server" Columns="2" CssClass="textbox_padrao"></asp:TextBox>
    <asp:Button ID="btnAtualizarPagina" runat="server" Text="OK" CausesValidation="false" CssClass="botao_padrao" OnClick="btnAtualizarPagina_Click" />
</asp:Panel>
