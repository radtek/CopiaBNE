<%@ Page
    Title=""
    Language="C#"
    MasterPageFile="~/Master/Principal.Master"
    AutoEventWireup="true"
    CodeBehind="SalaAdministradorEmpresas.aspx.cs"
    Inherits="BNE.Web.SalaAdministradorEmpresas" %>

<%@ Register
    Src="UserControls/Forms/SalaAdministrador/Empresas.ascx"
    TagName="Empresas"
    TagPrefix="uc1" %>

<asp:Content
    ID="Content1"
    ContentPlaceHolderID="cphHead"
    runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/SalaAdministrador.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/SalaAdministradorEmpresas.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <uc1:Empresas ID="ucEmpresas" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
