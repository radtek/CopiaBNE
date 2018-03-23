<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="SalaAdministradorRelatorios.aspx.cs" Inherits="BNE.Web.SalaAdministradorRelatorios" %>
<%@ Register Src="UserControls/Forms/SalaAdministrador/Index.ascx" TagName="Index"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/Forms/SalaAdministrador/NovasEmpresas.ascx" TagName="NovasEmpresas" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/SalaAdministrador.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/SalaAdministradorRelatorios.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicScript runat="server" Src="/js/local/UserControls/NovasEmpresas.js" type="text/javascript" />
    <Employer:DynamicScript runat="server" Src="/js/jquery-ui-1.7.2.custom.min.js" type="text/javascript" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <div class="painel_padrao_sala_adm">
        <%-- Configuração do Layout do Menu Relatórios--%>
        <asp:Panel ID="pnlContainerMensagens" CssClass="container_carta" runat="server">
            <asp:UpdatePanel ID="upPnlTipoMensagem" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="pnlTipoMensagem" CssClass="tipo_carta" runat="server">
                        <ul class="menu_cartas2">
                            <li id="liPSalarial" runat="server"><span>
                                <asp:LinkButton ID="btlPSalarial" runat="server" Text="Pesquisa Salarial" OnClick="btlPSalarial_Click"></asp:LinkButton>
                            </span></li>
                            <li id="liNovasEmpresas" runat="server"><span>
                                <asp:LinkButton ID="btlNovasEmpresas" runat="server" Text="Empresas" OnClick="btlNovasEmpresas_Click"></asp:LinkButton>
                            </span></li>
                        </ul>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <%-- Configuração do Conteudo do Layout para Relatórios --%>
            <asp:UpdatePanel ID="upPnlIndex" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="pnlIndex" runat="server">
                        <uc1:Index ID="ucIndex" runat="server" />
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="upPnlNvsEmp" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="PnlNvsEmp" runat="server">
                        <uc2:NovasEmpresas ID="ucNvsEmp" runat="server" />
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
    <%-- Botão Voltar --%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
                <asp:Button ID="btnVoltar" runat="server" CssClass="botao_padrao" Text="Voltar" CausesValidation="false"
                    OnClick="btnVoltar_Click" /></asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
