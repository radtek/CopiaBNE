<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="SalaVipEscolherEmpresa.aspx.cs" Inherits="BNE.Web.SalaVipEscolherEmpresa" %>

<%@ Register Src="UserControls/Forms/SalaVip/EscolherEmpresa.ascx" TagName="EscolherEmpresa"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/sala_vip.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <uc1:EscolherEmpresa ID="EscolherEmpresa1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
