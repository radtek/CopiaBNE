<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true" CodeBehind="SalaAdministradorQuemMeViu.aspx.cs"
    Inherits="BNE.Web.SalaAdministradorQuemMeViu" %>

<%@ Register Src="UserControls/Forms/SalaAdministrador/QuemMeViu.ascx" TagName="QuemMeViu" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/SalaAdministrador.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <uc1:QuemMeViu ID="ucQuemMeViu" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
