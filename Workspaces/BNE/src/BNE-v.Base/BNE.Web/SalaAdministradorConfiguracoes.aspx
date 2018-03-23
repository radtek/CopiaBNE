<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="SalaAdministradorConfiguracoes.aspx.cs" Inherits="BNE.Web.SalaAdministradorConfiguracoes" %>

<%@ Register Src="UserControls/Forms/SalaAdministrador/EmailRetornoPF.ascx" TagName="EmailRetornoPF"
    TagPrefix="uc2" %>
<%@ Register Src="UserControls/Forms/SalaAdministrador/Index.ascx" TagName="Index"
    TagPrefix="uc3" %>
<%@ Register Src="UserControls/Forms/SalaAdministrador/EmailPadrao.ascx" TagName="EmailPadrao"
    TagPrefix="uc1" %>
<%@ Register Src="UserControls/Forms/SalaAdministrador/ConfiguracoesValores.ascx"
    TagName="ConfiguracoesValores" TagPrefix="uc8" %>
<%@ Register Src="UserControls/Forms/SalaAdministrador/sms.ascx" TagName="SMS" TagPrefix="uc7" %>
<%@ Register Src="UserControls/Forms/SalaAdministrador/Noticias.ascx" TagName="Noticias"
    TagPrefix="uc4" %>
<%@ Register Src="UserControls/Forms/SalaAdministrador/EmailParaFilial.ascx" TagName="EmailParaFilial"
    TagPrefix="uc5" %>
<%@ Register Src="UserControls/Forms/SalaAdministrador/Usuarios.ascx" TagName="Usuarios"
    TagPrefix="uc6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/SalaAdministrador.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/SalaAdministradorConfiguracoes.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicScript runat="server" Src="/js/local/UserControls/ContagemCaracter.js" type="text/javascript" />
    <Employer:DynamicScript runat="server" Src="/js/local/UserControls/EmailParaFilial.js" type="text/javascript" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <div class="painel_padrao_sala_adm" style="margin-top:-20px">
        <%-- Configuração do Layout do Menu Configurações--%>
        <asp:Panel ID="pnlContainerMensagens" CssClass="container_carta" runat="server">
            <asp:UpdatePanel ID="upPnlTipoMensagem" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="pnlTipoMensagem" CssClass="tipo_carta" runat="server">
                        <h2>
                            Cartas</h2>
                        <ul class="menu_carta">
                            <li id="liCartasRecebidas" runat="server" class="selected">
                                <img src="img/icn_msg_recebidas.png" alt="Carta Recebida" />
                                <span>
                                    <asp:LinkButton ID="btlEmail" runat="server" Text="E-mail Padrão" OnClick="btlEmail_Click"></asp:LinkButton>
                                </span></li>
                            <li id="liMensagensEnviadas" runat="server">
                                <img src="img/icn_sms_azul.png" alt="SMS" />
                                <span>
                                    <asp:LinkButton ID="btlSMS" runat="server" Text="SMS" OnClick="btlSMS_Click"></asp:LinkButton>
                                </span></li>
                        </ul>
                        <ul class="menu_cartas2">
                            <li id="liValores" runat="server"><span>
                                <asp:LinkButton ID="btlValores" runat="server" Text="Valores" OnClick="btlValores_Click"></asp:LinkButton>
                            </span></li>
                            <li id="liEmailRetorno" runat="server"><span>
                                <asp:LinkButton ID="btlEmailRetorno" runat="server" Text="E-mail Padrão" OnClick="btlEmailRetorno_Click"></asp:LinkButton>
                            </span></li>
                            <li id="liNoticias" runat="server"><span>
                                <asp:LinkButton ID="btlNoticias" runat="server" Text="Notícias" OnClick="btlNoticias_Click"></asp:LinkButton>
                            </span></li>
                            <li id="liEmailParaFilial" runat="server"><span>
                                <asp:LinkButton ID="btlEmailParaFilial" runat="server" Text="Email para Filial" OnClick="btlEmailParaFilial_Click"></asp:LinkButton>
                            </span></li>
                            <li id="li1" runat="server"><span>
                                <asp:LinkButton ID="btlUsuarios" runat="server" Text="Usuários" OnClick="btlUsuarios_Click"></asp:LinkButton>
                            </span></li>
                        </ul>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <%-- Configuração do Conteudo do Layout das Configurações de Email Padrão --%>
            <asp:UpdatePanel ID="upPnlIndex" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="pnlIndex" runat="server">
                        <uc3:Index ID="ucIndex" runat="server" />
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="upPnlSMS" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="pnlSMS" runat="server">
                        <uc7:SMS ID="ucSMS" runat="server" />
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="upPnlNoticias" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="pnlNoticias" runat="server">
                        <uc4:Noticias ID="ucNoticias" runat="server" />
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="upPnlEmailPadrao" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="pnlEmailPadrao" runat="server">
                        <uc1:EmailPadrao ID="ucEmailPadrao" runat="server" />
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="upPnlEmailRetorno" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="pnlEmailRetorno" runat="server">
                        <uc2:EmailRetornoPF ID="ucEmailRetorno" runat="server" />
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="upPnlConfiguracoesValores" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="pnlConfiguracoesValores" runat="server">
                        <uc8:ConfiguracoesValores ID="ucConfiguracoesValores" runat="server" />
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="upPnlEmailParaFilial" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="pnlEmailParaFilial" runat="server">
                        <uc5:EmailParaFilial ID="ucEmailParaFilial" runat="server" />
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="upPnlUsuarios" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="pnlUsuarios" runat="server">
                        <uc6:Usuarios ID="ucUsuarios" runat="server" />
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
