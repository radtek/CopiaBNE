<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true" CodeBehind="ConfirmacaoEmail.aspx.cs" Inherits="BNE.Web.ConfirmacaoEmail" %>

<%@ Register Src="UserControls/Modais/ModalConfirmacaoEmail.ascx" TagName="ModalConfirmacaoEmail" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphExperimentos" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphConteudo" runat="server">
    <uc1:ModalConfirmacaoEmail ID="ModalConfirmacaoEmail" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
