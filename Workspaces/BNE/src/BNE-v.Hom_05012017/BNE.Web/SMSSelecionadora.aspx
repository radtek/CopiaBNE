<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true" CodeBehind="SMSSelecionadora.aspx.cs" Inherits="BNE.Web.SMSSelecionadora" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphExperimentos" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphHead" runat="server">
    <style>
        .sms-selecionadora a { font-size: 0; margin: 0 auto; padding: 0; display: block; text-align: center; }
    </style>
    <link href="/css/local/Bootstrap/bootstrap.css" rel="stylesheet" />
    <script src="/js/bootstrap/bootstrap.js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphConteudo" runat="server">
    <div class="sms-selecionadora">
        <a id="lkb1" href="#" onClick="AbrirModalVideoSMSSelecionadora(true);"><asp:Image runat="server" ImageUrl="/img/sms-selecionadora/sms-selecionadora-01.jpg"/></a>
        <a id="lkb2" href="#" onClick="AbrirModalVideoSMSSelecionadora(true);"><asp:Image runat="server" ImageUrl="/img/sms-selecionadora/sms-selecionadora-02.jpg"/></a>
        <a id="lkb3" href="#" onClick="AbrirModalVideoSMSSelecionadora(true);"><asp:Image runat="server" ImageUrl="/img/sms-selecionadora/sms-selecionadora-03.jpg"/></a>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
