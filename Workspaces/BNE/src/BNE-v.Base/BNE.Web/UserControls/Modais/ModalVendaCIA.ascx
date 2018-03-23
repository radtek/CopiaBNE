<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ModalVendaCIA.ascx.cs"
    Inherits="BNE.Web.UserControls.Modais.ModalVendaCIA" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/ModalVendaCIA.css" type="text/css" rel="stylesheet" />
<asp:Panel ID="pnlVendaCIA" runat="server" CssClass="modal_nova_amarela" Style="display: none">
    <asp:ImageButton CssClass="botao_fechar_modal" ID="btiFechar" ImageUrl="/img/modal_nova/btn_fechar_modal.png"
        runat="server" CausesValidation="false" />
    <div class="modal_nova_imagem">
        <asp:Image runat="server" ImageUrl="/img/modal_nova/img_alerta_bne.png" ID="imgAlerta"
            AlternateText="Alerta" />
    </div>
    <div class="modal_nova_conteudo">
        <p>
            Somente os currículos VIP são de acesso livre.
        </p>
        <p>
            Saiba como ver os contatos dos demais currículos:
        </p>
        <asp:LinkButton runat="server" ID="btlQueroComprarPlano" CausesValidation="False"
            Text="Continuar" OnClick="btlQueroComprarPlano_Click" CssClass="botao_quero_comprar">
        </asp:LinkButton>
    </div>
</asp:Panel>
<asp:HiddenField ID="hf" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeVendaCIA" PopupControlID="pnlVendaCIA" runat="server"
    CancelControlID="btiFechar" TargetControlID="hf">
</AjaxToolkit:ModalPopupExtender>
