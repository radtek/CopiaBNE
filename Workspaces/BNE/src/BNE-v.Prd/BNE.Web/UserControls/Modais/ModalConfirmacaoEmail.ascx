<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ModalConfirmacaoEmail.ascx.cs"
    Inherits="BNE.Web.UserControls.Modais.ModalConfirmacaoEmail" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/ModalConfirmacaoEmail.css" type="text/css" rel="stylesheet" />
<asp:Panel ID="pnlConfirmacaoEmail" runat="server" CssClass="modal_nova_amarela ajust_modal" Style="display: none;">
    <asp:ImageButton CssClass="botao_fechar_modal" ID="btiFechar" ImageUrl="/img/modal_nova/btn_fechar_modal.png"
        runat="server" CausesValidation="false" OnClick="btiFechar_OnClick" />
    <div class="modal_nova_imagem">
        <asp:Image runat="server" ImageUrl="/img/img_icone_check_128x128.png" ID="imgConfirmacao"
            AlternateText="Email confirmado" />
    </div>
    <div class="modal_nova_conteudo">
        <p>
            Seu e-mail foi confirmado com sucesso.
        </p>
        <p>
            Veja as vagas no seu perfil.
        </p>
        <asp:LinkButton runat="server" ID="btlVagasNoPerfil" CausesValidation="False"
            Text="Vagas no seu perfil" OnClick="btlVagasNoPerfil_Click" CssClass="botao_quero_comprar">
        </asp:LinkButton>
    </div>
</asp:Panel>
<asp:HiddenField ID="hf" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeConfirmacaoEmail" PopupControlID="pnlConfirmacaoEmail" runat="server" TargetControlID="hf">
</AjaxToolkit:ModalPopupExtender>
