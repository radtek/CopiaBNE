<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModalDegustacaoCandidatura.ascx.cs"
    Inherits="BNE.Web.UserControls.Modais.ModalDegustacaoCandidatura" %>
<asp:HiddenField ID="hfModalDegustacaoCandidatura" runat="server" />
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/ModalDegustacaoCandidatura.css" type="text/css" rel="stylesheet" />
<asp:Panel ID="pnlModalDegustacaoCandidatura" runat="server" CssClass="modal_nova_candidatura"
    Style="display: none">
    <asp:UpdatePanel ID="upBtiFechar" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:ImageButton CssClass="alinha_btn_fechar_nova_modal" ID="btiFechar" alt="Fechar"
                ImageUrl="/img/modal_nova/btn_fechar_modal.png" runat="server" CausesValidation="false"
                OnClick="btiFechar_Click" Visible="False" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel CssClass="img_envelope_bne" runat="server" ID="pnlEnvelope">
        <asp:UpdatePanel ID="upEnvelope" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <%--imagem abaixo aparecerá nas modais azuis 3, 2, 1 --%>
                <asp:Image runat="server" ImageUrl="/img/modal_nova/img_envelope_bne.png" ID="imgEnvelope"
                    AlternateText="Candidaturas Gratuitas" />
                <%--imagem abaixo aparecerá na última modal de alerta; que pena!!! --%>
                <asp:Image runat="server" ImageUrl="/img/modal_nova/img_alerta_bne.png" ID="imgAlerta"
                    Visible="False" AlternateText="Alerta" />
                <%-- aqui dinâmico para as modais azuis: 3, 2, 1 conforme usuário for usando o bônus --%>
                <asp:Label runat="server" ID="litQuantidadeDegustacao" CssClass="label_quantidadeBonus"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:Panel class="voce_ganhou" runat="server" ID="pnlInformacao">
        <asp:UpdatePanel ID="upInformacao" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Literal runat="server" ID="litInformacao"></asp:Literal>
                <asp:Label runat="server" ID="lblInformacaoAdicional" CssClass="label_terminar_de_preencher_cv"
                    Visible="False"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlBotoes">
        <asp:UpdatePanel ID="upBotoes" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:LinkButton runat="server" ID="lbtUsarBonus" CausesValidation="False" Visible="False"
                    Text="Usar meu bônus" CssClass="alinha_btnUsarBonus"
                    OnClick="lbtUsarBonus_Click">
                </asp:LinkButton>
                <asp:LinkButton runat="server" ID="lbtNaoAgora" CausesValidation="False" Visible="False"
                    Text="Não agora" CssClass="alinha_btnUsarBonus complementa_btn_cinza_alinhamento"
                    OnClick="lbtNaoAgora_Click">
                </asp:LinkButton>
                <asp:LinkButton runat="server" ID="lbtQueroPassarParaVip" CausesValidation="False"
                    Text="Quero conhecer o <strong>VIP</strong>!" CssClass="alinha_btnQueroPassarParaVip"
                    OnClick="lbtQueroPassarParaVip_Click" Visible="False">
                </asp:LinkButton>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Panel>
<AjaxToolkit:ModalPopupExtender ID="mpeModalDegustacaoCandidatura" runat="server"
    PopupControlID="pnlModalDegustacaoCandidatura" TargetControlID="hfModalDegustacaoCandidatura" />
