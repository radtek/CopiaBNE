<%@ Page Title=""
    Language="C#"
    MasterPageFile="~/Master/Principal.Master"
    AutoEventWireup="true"
    CodeBehind="SalaVipJaEnviei.aspx.cs"
    Inherits="BNE.Web.SalaVipJaEnviei" %>

<%@ Register
    Src="UserControls/Forms/SalaVip/JaEnviei.ascx"
    TagName="JaEnviei"
    TagPrefix="uc1" %>
<%@ Register
    Src="UserControls/Modais/VerDadosEmpresa.ascx"
    TagName="VerDadosEmpresa"
    TagPrefix="uc2" %>
<%@ Register
    Src="UserControls/Modais/ModalConfirmacaoEnvioCurriculo.ascx"
    TagName="ModalConfirmacaoEnvioCurriculo"
    TagPrefix="uc3" %>
<asp:Content
    ID="Content1"
    ContentPlaceHolderID="cphHead"
    runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/sala_vip.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/SalaVipJaEnviei.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content
    ID="Content2"
    ContentPlaceHolderID="cphConteudo"
    runat="server">
    <uc2:VerDadosEmpresa
        ID="ucVerDadosEmpresa"
        runat="server" />
    <uc1:JaEnviei
        ID="ucJaEnviei"
        runat="server" />
</asp:Content>
<asp:Content
    ID="Content3"
    ContentPlaceHolderID="cphRodape"
    runat="server">
    <uc3:ModalConfirmacaoEnvioCurriculo
        ID="ucModalConfirmacaoEnvioCurriculo"
        runat="server" />
</asp:Content>
