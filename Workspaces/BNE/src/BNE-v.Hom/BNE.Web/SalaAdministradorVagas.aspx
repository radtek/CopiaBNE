<%@ Page
    Title=""
    Language="C#"
    MasterPageFile="~/Master/Principal.Master"
    AutoEventWireup="true"
    CodeBehind="SalaAdministradorVagas.aspx.cs"
    Inherits="BNE.Web.SalaAdministradorVagas" %>

<%@ Register
    Src="UserControls/Forms/SalaAdministrador/Vagas.ascx"
    TagName="Vagas"
    TagPrefix="uc1" %>

<%@ Register
    Src="UserControls/Modais/ConfirmacaoExclusao.ascx"
    TagName="ConfirmacaoExclusao"
    TagPrefix="uc1" %>

<asp:Content
    ID="Content1"
    ContentPlaceHolderID="cphHead"
    runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/SalaAdministrador.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/SalaAdministradorVagas.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <uc1:Vagas
        ID="ucVagas"
        runat="server" />
    <uc1:ConfirmacaoExclusao
        ID="ucConfirmacaoExclusao"
        runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
