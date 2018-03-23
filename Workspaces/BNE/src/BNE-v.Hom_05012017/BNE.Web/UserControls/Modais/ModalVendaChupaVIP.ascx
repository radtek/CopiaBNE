<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ModalVendaChupaVIP.ascx.cs"
    Inherits="BNE.Web.UserControls.Modais.ModalVendaChupaVIP" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/ModalVendaChupaVIP.css" type="text/css" rel="stylesheet" />
<asp:Panel ID="pnlEmpresaChupaVIP" runat="server" CssClass="modal_nova_amarela" Style="display: none">
    <asp:ImageButton CssClass="botao_fechar_modal" ID="btiFechar" ImageUrl="/img/modal_nova/btn_fechar_modal.png"
        runat="server" CausesValidation="false" />
    <div class="modal_nova_imagem">
        <asp:Image runat="server" ImageUrl="/img/modal_nova/img_alerta_bne.png" ID="imgAlerta"
            AlternateText="Alerta" />
    </div>
    <div class="modal_nova_conteudo">
        <asp:UpdatePanel runat="server" ID="upChupaVIP" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel runat="server" ID="pnlSMS" Visible="False">
                    <p>
                        Você já chegou ao limite de envio de SMS diário disponível para o plano básico!
                        <div class="labelMaioresInformacoes">
                            Para maiores informações ligue: <strong>0800 41 2400</strong>
                        </div>
                    </p>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlVisualizacao" Visible="False">
                    <p>
                        Você já chegou ao limite de visualizações diárias disponível para o plano básico!
                        <div class="labelMaioresInformacoes">
                            Para maiores informações ligue: <strong>0800 41 2400</strong>
                        </div>
                    </p>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%--<asp:LinkButton runat="server" ID="btlQueroComprarPlano" CausesValidation="False"
            Text="Quero comprar!" OnClick="btlQueroComprarPlano_Click" CssClass="botao_quero_comprar">
        </asp:LinkButton>--%>
    </div>
</asp:Panel>
<asp:HiddenField ID="hf" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeEmpresaChupaVIP" PopupControlID="pnlEmpresaChupaVIP"
    runat="server" CancelControlID="btiFechar" TargetControlID="hf">
</AjaxToolkit:ModalPopupExtender>
