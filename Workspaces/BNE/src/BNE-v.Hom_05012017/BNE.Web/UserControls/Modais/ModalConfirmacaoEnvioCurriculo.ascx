<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ModalConfirmacaoEnvioCurriculo.ascx.cs"
    Inherits="BNE.Web.UserControls.Modais.ModalConfirmacaoEnvioCurriculo" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/ModalConfirmacaoEnvioCurriculo.css" type="text/css" rel="stylesheet" />
<asp:HiddenField ID="hfModalConfirmacaoEnvioCurriculo" runat="server" />
<AjaxToolkit:ModalPopupExtender BehaviorID="mpeModalConfirmacaoEnvioCurriculoBehavior"
    ID="mpeModalConfirmacaoEnvioCurriculo" runat="server" PopupControlID="pnlModalConfirmacaoEnvioCurriculo"
    TargetControlID="hfModalConfirmacaoEnvioCurriculo">
</AjaxToolkit:ModalPopupExtender>
<asp:Panel ID="pnlModalConfirmacaoEnvioCurriculo" runat="server" CssClass="modal_nova_amarela"
    Style="display:none">
    <%--<h2 class="titulo_modal">
       <asp:UpdatePanel ID="upLitTitulo" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
                <asp:Literal runat="server" ID="litTitulo"></asp:Literal>
            </ContentTemplate>
        </asp:UpdatePanel>
    </h2>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:ImageButton CssClass="alinha_btn_fechar_nova_modal" ID="btiFechar" alt="Fechar"
                ImageUrl="/img/modal_nova/btn_amarelo_fechar_modal.png" runat="server" CausesValidation="false"
                OnClick="btiFechar_Click" Visible="True" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="upProtocolo" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlProtocoloCandidatura" CssClass="container_confirmacao">
                <div class="icone_confirmacao">
                    <img alt="Ícone Confirmação" src="/img/modal_nova/imgCurtirConfirmacao.png" />
                </div>
                <div class="texto_confirmacao">
                    <p class="texto_enviado_sucesso">
                        <asp:Literal ID="litNome" runat="server"></asp:Literal>!!!
                        Seu currículo foi enviado
                        com sucesso!</p>
                    <p class="texto_protocolo_envio">
                        Protocolo de envio: <strong>
                            <asp:Literal ID="litProtocolo" runat="server"></asp:Literal></strong></p>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
