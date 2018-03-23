<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="SalaVip.aspx.cs" Inherits="BNE.Web.SalaVip" %>

<%@ Register Src="UserControls/Forms/SalaVip/Dados.ascx" TagName="Dados" TagPrefix="uc1" %>
<%@ Register Src="UserControls/Forms/SalaVip/MenuSalaVip.ascx" TagName="MenuSalaVip"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicScript runat="server" Src="/js/local/Forms/SalaVip.js" type="text/javascript" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/sala_vip.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <asp:Panel ID="pnlImgSejaVip" runat="server" Visible="false">
        <a href="/vip" onclick="trackOutboundLink(this, 'Anuncios', 'Click', 'Banner_Sala_Candidato'); return false;">
            <img alt="Seja VIP!" src="img/seja-vip.jpg" />
        </a>
    </asp:Panel>
    <div class="painel_padrao_sala_vip">
        <uc1:Dados ID="ucDados" runat="server" />
        <uc2:MenuSalaVip ID="ucTabs" runat="server" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
