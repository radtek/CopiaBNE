<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="CadastroCurriculoRevisao.aspx.cs" Inherits="BNE.Web.CadastroCurriculoRevisao" %>

<%@ Register Src="UserControls/Forms/CadastroCurriculo/ConferirDados.ascx" TagName="ConferirDados"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/Modais/ModalConfirmacao.ascx" TagName="ModalConfirmacao"
    TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/Modais/ModalDegustacaoCandidatura.ascx" TagName="ModalDegustacaoCandidatura"
    TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <!-- UserControl ConferirDados -->
    <uc1:ConferirDados ID="ucConferirDados" runat="server" />
    <!-- FIM: UserControl ConferirDados -->
    <uc2:ModalConfirmacao ID="ucModalConfirmacao" runat="server" />
    <uc:ModalDegustacaoCandidatura ID="ucModalDegustacaoCandidatura" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
