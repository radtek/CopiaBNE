<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="SalaSelecionadorMensagens.aspx.cs" Inherits="BNE.Web.SalaSelecionadorMensagens" %>

<%@ Register Src="UserControls/Forms/Mensagens/MensagensRecebidas.ascx" TagName="MensagensRecebidas"
    TagPrefix="uc4" %>
<%@ Register Src="UserControls/Forms/Mensagens/MensagensEnviadas.ascx" TagName="MensagensEnviadas"
    TagPrefix="uc5" %>
<%@ Register Src="UserControls/Forms/Mensagens/CorpoMensagem.ascx" TagName="CorpoMensagem"
    TagPrefix="uc6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicScript runat="server" Src="/js/local/SalaSelecionadorMensagens.js" type="text/javascript" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/Sala_Selecionador.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/SalaSelecionadorMensagens.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/MeuPlano.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <asp:UpdatePanel ID="upSalaSelecionadoraMensagem" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="painel_padrao_sala_selecionador">
                <asp:Panel ID="pnlContainerMensagens" CssClass="container_mensagens" runat="server">
                    <asp:Panel ID="pnlTipoMensagem" CssClass="tipo_mensagem" runat="server">
                        <h2>
                            Mensagens</h2>
                        <ul class="menu_mensagens">
                            <li id="liMensagensRecebidas" runat="server" class="selected">
                                <i class="fa fa-envelope-square"></i><span>
                                    <asp:LinkButton ID="lkMensagensRecebidas" runat="server" OnClick="lkMensagensRecebidas_Click"></asp:LinkButton>
                                </span></li>
                            <li id="liMensagensEnviadas" runat="server">
                                <i class="fa fa-paper-plane-o"></i> <span><asp:LinkButton
                                    ID="lkMensagensEnviadas" runat="server" OnClick="lkMensagensEnviadas_Click"></asp:LinkButton></span></li>
                        </ul>
                        </span>
                    </asp:Panel>
                    <asp:Panel ID="pnlMensagensRecebidas" CssClass="preview_mensagens" runat="server">
                        <uc4:MensagensRecebidas ID="ucMensagensRecebidas" runat="server" />
                    </asp:Panel>
                    <asp:Panel ID="pnlMensagensEnviadas" CssClass="preview_mensagens" runat="server">
                        <uc5:MensagensEnviadas ID="ucMensagensEnviadas" runat="server" />
                    </asp:Panel>
                    <asp:Panel ID="pnlCorpoMensagem" CssClass="preview_mensagens" runat="server">
                        <uc6:CorpoMensagem ID="ucCorpoMensagem" runat="server" />
                    </asp:Panel>
                </asp:Panel>
            </div>
            <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
                <asp:Button ID="btnVoltar" runat="server" CssClass="botao_padrao" Text="Voltar" CausesValidation="false"
                    OnClick="btnVoltar_Click" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
