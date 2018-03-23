<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true" CodeBehind="SalaVipQuemMeViu.aspx.cs"
    Inherits="BNE.Web.SalaVipQuemMeViu" %>

<%@ Register Src="UserControls/Forms/SalaVip/QuemMeViu.ascx" TagName="QuemMeViu" TagPrefix="uc1" %>
<%@ Register Src="UserControls/Modais/VerDadosEmpresa.ascx" TagName="VerDadosEmpresa" TagPrefix="uc2" %>
<%@ Register Src="UserControls/Modais/ModalConfirmacao.ascx" TagName="ModalConfirmacao" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/sala_vip.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/SalaVipQuemMeViu.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <uc3:ModalConfirmacao ID="ucModalConfirmacao" runat="server" />
    <uc1:QuemMeViu ID="ucQuemMeViu" runat="server" />
    <uc2:VerDadosEmpresa ID="ucVerDadosEmpresa" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
