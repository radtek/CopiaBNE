<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="CadastroCurriculoDados.aspx.cs" Inherits="BNE.Web.CadastroCurriculoDados" %>

<%@ Register Src="UserControls/Forms/CadastroCurriculo/DadosPessoais.ascx" TagName="DadosPessoais"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <!-- UserControl DadosPessoais -->
    <uc1:DadosPessoais ID="ucDadosPessoais" runat="server" />
    <!-- FIM: UserControl DadosPessoais -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
