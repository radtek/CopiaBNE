<%@ Page Title=""
    Language="C#"
    MasterPageFile="~/Master/Principal.Master"
    AutoEventWireup="true"
    CodeBehind="CadastroCurriculoComplementar.aspx.cs"
    Inherits="BNE.Web.CadastroCurriculoComplementar" %>

<%@ Register
    Src="UserControls/Forms/CadastroCurriculo/DadosComplementares.ascx"
    TagName="DadosComplementares"
    TagPrefix="uc1" %>
<%@ Register
    Src="UserControls/Modais/ModalConfirmacao.ascx"
    TagName="ModalConfirmacao"
    TagPrefix="uc2" %>
<asp:Content
    ID="Content1"
    ContentPlaceHolderID="cphHead"
    runat="server">
</asp:Content>
<asp:Content
    ID="Content2"
    ContentPlaceHolderID="cphConteudo"
    runat="server">
    <!-- UserControl DadosComplementares -->
    <uc1:DadosComplementares
        ID="ucDadosComplementares"
        runat="server" />
    <!-- FIM: UserControl DadosComplementares -->
</asp:Content>
<asp:Content
    ID="Content3"
    ContentPlaceHolderID="cphRodape"
    runat="server">
    <!-- Painel: Confirmacao Cadastro -->
    <uc2:ModalConfirmacao
        ID="ucModalConfirmacao"
        runat="server" />
    <!-- FIM Painel: Confirmacao -->
</asp:Content>
