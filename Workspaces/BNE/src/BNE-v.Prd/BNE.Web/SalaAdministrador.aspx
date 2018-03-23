<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="SalaAdministrador.aspx.cs" Inherits="BNE.Web.SalaAdministrador" %>

<%@ Register Src="UserControls/Forms/SalaAdministrador/MenuSalaAdministrador.ascx"
    TagName="MenuSalaAdministrador" TagPrefix="uc2" %>
<%@ Register Src="UserControls/Forms/SalaAdministrador/Dados.ascx" TagName="Dados"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/SalaAdministrador.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <asp:Panel ID="pnlSalaAdministrador" CssClass="painel_padrao_sala_adm" runat="server">
        <uc1:Dados ID="ucDados" runat="server" />
        <uc2:MenuSalaAdministrador ID="ucMenuSalaAdministrador" runat="server" />
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
