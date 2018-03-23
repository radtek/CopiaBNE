<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="Curriculo.aspx.cs" Inherits="BNE.Web.curriculo.Curriculo" %>

<%@ Register Src="~/UserControls/VisualizacaoCurriculo.ascx" TagName="VisualizacaoCurriculo"
    TagPrefix="UserControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <div id="conteudo" class="conteudo">
        <UserControl:VisualizacaoCurriculo ID="ucVisualizacaoCurriculo" runat="server" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
