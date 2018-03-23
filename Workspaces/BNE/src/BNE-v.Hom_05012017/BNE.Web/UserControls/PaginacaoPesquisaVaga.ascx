<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PaginacaoPesquisaVaga.ascx.cs" Inherits="BNE.Web.UserControls.PaginacaoPesquisaVaga" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/PaginacaoPesquisaVaga.css" type="text/css" rel="stylesheet" />
<Employer:DynamicScript runat="server" Src="/js/local/UserControls/PaginacaoPesquisaVaga.js" type="text/javascript" />
<asp:Panel ID="pnlPaginacao" runat="server" CssClass="painel_paginacao">
    <div class="coluna_controles_paginacao">
        <asp:ImageButton ID="btiPrimeira" runat="server" OnClick="btiPrimeira_Click" ToolTip="Primeira página" CausesValidation="false" ImageUrl="/img/btn_telerik_paginacao_primeira_off.png" />
        <asp:ImageButton ID="btiAnterior" runat="server" OnClick="btiAnterior_Click" ToolTip="Página anterior" CausesValidation="false" ImageUrl="/img/btn_telerik_paginacao_anterior_off.png" />
        <asp:Panel ID="pnlPaginacaoNumerica" runat="server" CssClass="PnlPaginacaoNumerica"></asp:Panel>
        <asp:ImageButton ID="btiProxima" runat="server" OnClick="btiProxima_Click" ToolTip="Próxima página" CausesValidation="false" ImageUrl="/img/btn_telerik_paginacao_proxima_off.png" />
        <asp:ImageButton ID="btiUltima" runat="server" OnClick="btiUltima_Click" ToolTip="Última página" CausesValidation="false" ImageUrl="/img/btn_telerik_paginacao_ultima_off.png" />
    </div>
    <div class="coluna_estado_paginacao">
        <span class="estado_paginacao">
            <asp:Literal ID="lblEstadoPaginacao" runat="server"></asp:Literal></span>
    </div>
</asp:Panel>
