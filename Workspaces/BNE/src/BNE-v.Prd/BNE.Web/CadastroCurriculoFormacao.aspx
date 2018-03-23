<%@ Page Title=""
    Language="C#"
    MasterPageFile="~/Master/Principal.Master"
    AutoEventWireup="true"
    CodeBehind="CadastroCurriculoFormacao.aspx.cs"
    Inherits="BNE.Web.CadastroCurriculoFormacao" %>

<%@ Register
    Src="UserControls/Forms/CadastroCurriculo/FormacaoCursos.ascx"
    TagName="FormacaoCursos"
    TagPrefix="uc1" %>
<asp:Content
    ID="Content1"
    ContentPlaceHolderID="cphHead"
    runat="server">
</asp:Content>
<asp:Content
    ID="Content2"
    ContentPlaceHolderID="cphConteudo"
    runat="server">
    <!-- UserControl FormacaoCursos -->
    <uc1:FormacaoCursos
        ID="ucFormacaoCursos"
        runat="server" />
    <!-- FIM: UserControl FormacaoCursos -->
</asp:Content>
<asp:Content
    ID="Content3"
    ContentPlaceHolderID="cphRodape"
    runat="server">
</asp:Content>
