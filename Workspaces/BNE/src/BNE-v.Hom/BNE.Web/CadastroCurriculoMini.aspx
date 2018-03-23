<%@ Page Title=""
Language="C#"
MasterPageFile="~/Master/Principal.Master"
AutoEventWireup="true"
CodeBehind="CadastroCurriculoMini.aspx.cs"
Inherits="BNE.Web.CadastroCurriculoMini" %>

<%@ Register
Src="UserControls/Forms/CadastroCurriculo/MiniCurriculo.ascx"
TagName="MiniCurriculo"
TagPrefix="uc1" %>
<asp:Content
    ID="Content1"
    ContentPlaceHolderID="cphHead"
    runat="server">
    <%--<link href="css/global/custom.css" rel="stylesheet" />--%>
    <%--<link href="css/global/font-awesome.min.css" rel="stylesheet" />--%>
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/cadastro_curriculo_mini.css" type="text/css" rel="stylesheet"/>
</asp:Content>
<asp:Content
    ID="Content2"
    ContentPlaceHolderID="cphConteudo"
    runat="server">
    <!-- UserControl MiniCurriculo -->
    <div class="wrapper-center">
    <uc1:MiniCurriculo
        ID="ucMiniCurriculo"
        runat="server"/>

    </div>
    <!-- FIM: UserControl cMiniCurriculo -->
</asp:Content>
<asp:Content
    ID="Content3"
    ContentPlaceHolderID="cphRodape"
    runat="server">
</asp:Content>