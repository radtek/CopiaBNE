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
        <asp:UpdatePanel ID="upCoin" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <%--imagem abaixo aparecerá nas modais azuis 3, 2, 1 --%>
                <asp:Image runat="server" class="coin-icon" ImageUrl="/img/modal_nova/1-candidaturas-coin.png" ID="imgCoin"
                    AlternateText="Candidaturas Gratuitas" />

                <%--imagem abaixo aparecerá na última modal de alerta; que pena!!! --%>
                <asp:Image runat="server" class="vip-pass-icon" ImageUrl="/img/modal_nova/vip-pass.png" ID="imgAlerta"
                    Visible="False" AlternateText="Alerta" />
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
                <div class="cancelar" style="text-align:center;">
                    <a class="b-close">
                        <asp:Button runat="server" ID="btnEntendi" OnClick="btnOK_Click" Visible="false" Text="OK, ENTENDI!" />
                    </a>
                </div>
                <table class="table_botoes">
                    <tr>
                        <td style="float: right;">
                            <div class="cancelar">
                                <asp:Button runat="server" ID="btnUsarBonus" CausesValidation="False" Visible="False"
                                    Text="NÃO AGORA"
                                    OnClick="lbtUsarBonus_Click"></asp:Button>
                                <asp:Button runat="server" ID="btnNaoAgora" CausesValidation="False" Visible="False"
                                    Text="NÃO AGORA"
                                    OnClick="lbtNaoAgora_Click"></asp:Button>

                            </div>
                        </td>
                        <td style="width: 10px"></td>
                        <td>
                            <div class="btnVoltar alinhar">

                                <asp:Button runat="server" ID="btnQueroPassarParaVip" CausesValidation="False"
                                    Text="QUERO SER VIP!"
                                    OnClick="lbtQueroPassarParaVip_Click" Visible="False"></asp:Button>
                            </div>
                        </td>
                    </tr>
                </table>

            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Panel>
<AjaxToolkit:ModalPopupExtender ID="mpeModalDegustacaoCandidatura" runat="server"
    PopupControlID="pnlModalDegustacaoCandidatura" TargetControlID="hfModalDegustacaoCandidatura" />
