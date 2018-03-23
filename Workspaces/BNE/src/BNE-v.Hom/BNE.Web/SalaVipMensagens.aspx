<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="SalaVipMensagens.aspx.cs" Inherits="BNE.Web.SalaVipMensagens" %>

<%@ Register Src="UserControls/Forms/Mensagens/MensagensRecebidas.ascx" TagName="MensagensRecebidas"
    TagPrefix="uc1" %>
<%@ Register Src="UserControls/Forms/Mensagens/CorpoMensagem.ascx" TagName="CorpoMensagem"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/sala_vip.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/SalaVipMensagens.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <asp:UpdatePanel ID="upSalaVipMensagem" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="painel_padrao_sala_vip">
                <asp:Panel ID="pnlContainerMensagens" CssClass="container_mensagens" runat="server">
                    <asp:Panel ID="pnlTipoMensagem" CssClass="tipo_mensagem" runat="server">
                        <h2>
                            Mensagens</h2>
                        <ul class="menu_mensagens">
                            <li id="liMensagensRecebidas" runat="server" class="selected">
                                <img src="img/icn_msg_recebidas.png" alt="Mensagens Recebidas" /><span>
                                    <asp:LinkButton ID="lkMensagensRecebidas" runat="server" OnClick="lkMensagensRecebidas_Click"></asp:LinkButton>
                                </span></li>
                        </ul>
                        </span>
                    </asp:Panel>
                    <asp:Panel ID="pnlMensagensRecebidas" CssClass="preview_mensagens" runat="server">
                        <uc1:MensagensRecebidas ID="ucMensagensRecebidas" runat="server" />
                    </asp:Panel>
                    <asp:Panel ID="pnlCorpoMensagem" CssClass="preview_mensagens" runat="server">
                        <uc2:CorpoMensagem ID="ucCorpoMensagem" runat="server" />
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
