<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="CIAProduto.aspx.cs" Inherits="BNE.Web.CIAProduto" %>

<%@ Register Src="UserControls/Modais/ucWebCallBack.ascx" TagName="WebCallBack" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/CIAProdutoNovo.css" type="text/css" rel="stylesheet" />
    <link href="css/local/Bootstrap/bootstrap.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <div class="painel_padrao">
        <asp:HyperLink ID="hlContinuar" runat="server" Text="Conheça os planos" CssClass="label_verde_continuar"
            NavigateUrl="/Escolha-de-Plano-CIA">
        <div class="btn_verde">
        </div>
        </asp:HyperLink>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
    <asp:UpdatePanel ID="upWebCallBack" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlWebCallBack" runat="server" Visible="true">
                <uc:WebCallBack ID="ucWebCallBack" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
