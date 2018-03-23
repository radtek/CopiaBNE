<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="SalaAdministradorPresidente.aspx.cs" Inherits="BNE.Web.SalaAdministradorPresidente" %>
<%@ Register Src="UserControls/Forms/SalaAdministrador/Presidente.ascx" TagName="PresidenteAgradecimento" TagPrefix="uc1" %>
<%@ Register Src="UserControls/Forms/SalaAdministrador/PresidenteRespostaAutomatica.ascx" TagName="PresidenteRespostaAutomatica" TagPrefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicScript runat="server" Src="/js/local/Forms/SalaAdministrador/CadastroAgradecimento.js" type="text/javascript" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/SalaAdministrador.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/SalaAdministradorConfiguracoes.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <div class="painel_padrao_sala_adm">
        <%-- Configuração do Layout do Menu Configurações--%>
        <asp:UpdatePanel runat="server" ID="upContainerMensagens" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlContainerMensagens" CssClass="container_carta" runat="server">
                    <asp:Panel ID="pnlTipoMensagem" CssClass="container_presidente" runat="server">
                        <ul class="menu_presidente">
                            <li id="liRespostasAutomaticasSel" runat="server" class="selected"><span>
                                <asp:LinkButton ID="btnRespostasAutomaticaSel" runat="server"
                                    Text="Respostas Automáticas" OnClick="btnRespostasAutomaticas_Click"></asp:LinkButton></span></li>
                            <li id="liRespostasAutomaticas" runat="server" visible="false"><span>
                                <asp:LinkButton ID="btnRespostasAutomatica" runat="server"
                                    Text="Respostas Automáticas" OnClick="btnRespostasAutomaticas_Click"></asp:LinkButton></span></li>
                            <li id="liAgradecimentoSel" runat="server" class="selected" visible="false"><span>
                                <asp:LinkButton ID="btnAgradecimentoSel" runat="server" Text="Agradecimento"
                                    OnClick="btnAgradecimento_Click"></asp:LinkButton></span></li>
                            <li id="liAgradecimento" runat="server"><span>
                                <asp:LinkButton ID="btnAgradecimento" runat="server" Text="Agradecimento"
                                    OnClick="btnAgradecimento_Click"></asp:LinkButton></span></li>
                        </ul>
                    </asp:Panel>
                    <asp:UpdatePanel ID="upConteudo" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <uc1:PresidenteAgradecimento ID="ucPresidenteAgradecimento" runat="server" Visible="false" />
                            <uc4:PresidenteRespostaAutomatica ID="ucPresidenteRespostaAutomatica" runat="server" Visible="true" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
        <asp:Button ID="btnVoltar" runat="server" CssClass="botao_padrao painelinterno" Text="Voltar"
            CausesValidation="false" OnClick="btnVoltar_Click" />
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
