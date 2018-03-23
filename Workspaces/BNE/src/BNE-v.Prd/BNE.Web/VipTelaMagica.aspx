<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="VipTelaMagica.aspx.cs" Inherits="BNE.Web.VipTelaMagica" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <link href="//fonts.googleapis.com/css?family=Open+Sans+Condensed:300" rel='stylesheet' type='text/css' />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/VipTelaMagica.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <asp:LinkButton ID="lnkBtn" runat="server" OnClick="btnContinuar_Click">
        <div class="containerImgTelaMagica"></div>
    </asp:LinkButton>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
