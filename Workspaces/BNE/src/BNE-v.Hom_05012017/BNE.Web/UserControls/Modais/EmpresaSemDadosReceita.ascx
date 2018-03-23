<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmpresaSemDadosReceita.ascx.cs" Inherits="BNE.Web.UserControls.Modais.EmpresaSemDadosReceita" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/EmpresaBloqueada.css" type="text/css" rel="stylesheet" />
<asp:Panel ID="pnlEmpresaSemDadosReceita" runat="server" CssClass="modal_nova" Style="display: none;">
    <asp:ImageButton CssClass="botao_fechar_modal" ID="btiFechar" ImageUrl="/img/modal_nova/btn_fechar_modal.png"
        runat="server" CausesValidation="false" />
    <div class="modal_nova_imagem">
        <asp:Image runat="server" ImageUrl="/img/modal_nova/img_alerta_bne.png" ID="imgAlerta"
            AlternateText="Alerta" />
    </div>
    <div class="modal_nova_conteudo">
        <p>Recebemos o cadastro de sua empresa!  </p>
        <p>
            Estamos realizando as liberações para seu acesso e entraremos em contato em seguida. 
        </p>
        <p>
            Este processo é rápido, por favor aguarde.
        </p>
    </div>
</asp:Panel>
<asp:HiddenField ID="hf" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeEmpresaSemDadosReceita" PopupControlID="pnlEmpresaSemDadosReceita"
    runat="server" CancelControlID="btiFechar" TargetControlID="hf">
</AjaxToolkit:ModalPopupExtender>
