<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true" CodeBehind="PaginaErro.aspx.cs" Inherits="BNE.Web.PaginaErro" %>
<asp:Content ID="conHead" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/PaginaErro.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="conConteudo" ContentPlaceHolderID="cphConteudo" runat="server">
    <asp:Panel ID="pnlDadosBasicosCadastro" runat="server" CssClass="painel_padrao">
        <div class="painel_padrao_topo">&nbsp;</div>
        <div class="icone_erro"><img alt="" src="/img/img_icone_erro_128x128.png" /></div>
        <p class="texto_erro_acesso">Ocorreu um erro no seu acesso.</p>
        <p class="texto_tente_novamente">Tente novamente e se precisar entre em contato pelo Atendimento Online.</p>
    </asp:Panel>
</asp:Content>
<asp:Content ID="conRodape" ContentPlaceHolderID="cphRodape" runat="server"></asp:Content>
