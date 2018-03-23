<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmpresaAguardandoPublicacao.ascx.cs" Inherits="BNE.Web.UserControls.Modais.EmpresaBloqueadaAguardandoPub" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/EmpresaBloqueada.css" type="text/css" rel="stylesheet" />
<asp:Panel ID="pnlEmpresaBloqueadaAguardandoPub" runat="server" CssClass="modal_nova" style="display: none;">
    <asp:ImageButton CssClass="botao_fechar_modal" ID="btiFechar" ImageUrl="/img/modal_nova/btn_fechar_modal.png"
        runat="server" CausesValidation="false" />
    <div class="modal_nova_imagem">
        <asp:Image runat="server" ImageUrl="/img/modal_nova/img_alerta_bne.png" ID="imgAlerta"
            AlternateText="Alerta" />
    </div>
    <div class="modal_nova_conteudo">
        <p>
            Olá, em breve o seu acesso completo estará disponível.
        </p>
        <p>
            Aguarde o contato de nossa equipe de Boas Vindas.
        </p>
        <p>
            Caso tenha urgência ligue 0800 41 2400</p>
    </div>
</asp:Panel>
<asp:HiddenField ID="hf" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeEmpresaBloqueadaAguardandoPub" PopupControlID="pnlEmpresaBloqueadaAguardandoPub"
    runat="server" CancelControlID="btiFechar" TargetControlID="hf">
</AjaxToolkit:ModalPopupExtender>
