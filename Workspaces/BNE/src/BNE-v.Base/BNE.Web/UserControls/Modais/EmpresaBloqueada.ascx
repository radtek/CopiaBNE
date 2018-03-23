<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmpresaBloqueada.ascx.cs" Inherits="BNE.Web.UserControls.Modais.EmpresaBloqueada" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/EmpresaBloqueada.css" type="text/css" rel="stylesheet" />
<asp:Panel ID="pnlEmpresaBloqueada" runat="server" CssClass="modal_nova" style="display: none;">
    <asp:ImageButton CssClass="botao_fechar_modal" ID="btiFechar" ImageUrl="/img/modal_nova/btn_fechar_modal.png"
        runat="server" CausesValidation="false" />
    <div class="modal_nova_imagem">
        <asp:Image runat="server" ImageUrl="/img/modal_nova/img_alerta_bne.png" ID="imgAlerta"
            AlternateText="Alerta" />
    </div>
    <div class="modal_nova_conteudo">
        <p>
            Olá, sua empresa está bloqueada.
        </p>
        <p>
            Para mais informações ligue 0800 41 2400</p>
    </div>
</asp:Panel>
<asp:HiddenField ID="hf" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeEmpresaBloqueada" PopupControlID="pnlEmpresaBloqueada"
    runat="server" CancelControlID="btiFechar" TargetControlID="hf">
</AjaxToolkit:ModalPopupExtender>
