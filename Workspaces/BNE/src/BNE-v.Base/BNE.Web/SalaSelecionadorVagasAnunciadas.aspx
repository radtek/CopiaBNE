<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="SalaSelecionadorVagasAnunciadas.aspx.cs" Inherits="BNE.Web.SalaSelecionadorVagasAnunciadas" %>

<%@ Register Src="UserControls/Forms/SalaSelecionador/MinhasVagas.ascx" TagName="MinhasVagas"
    TagPrefix="uc3" %>
<%@ Register Src="UserControls/Modais/ConfirmacaoExclusao.ascx" TagName="ConfirmacaoExclusao"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicScript runat="server" Src="/js/local/SalaSelecionadorVagasAnunciadas.js" type="text/javascript" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/Sala_Selecionador.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/SalaSelecionadorVagasAnunciadas.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <div class="painel_padrao_topo_sala_selecionador">
        &nbsp;</div>
    <asp:Panel ID="pnlConteudo" runat="server">
        <uc3:MinhasVagas ID="ucMinhasVagas" runat="server" />
        <uc1:ConfirmacaoExclusao ID="ucConfirmacaoExclusao" runat="server" />
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
