<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ModalVendaRSM.ascx.cs"
    Inherits="BNE.Web.UserControls.Modais.ModalVendaRSM" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/ModalVendaRSM.css" type="text/css" rel="stylesheet" />
<asp:Panel ID="pnlVendaRSM" runat="server" CssClass="modal_nova_amarela" Style="display: none">
   
    <asp:LinkButton CssClass="botao_fechar_modal" ID="btiFechar"
        runat="server" CausesValidation="false"><i class="fa fa-times-circle"></i></asp:LinkButton> 

    <div class="modal_nova_imagem">

        <i class="fa fa-exclamation-triangle"></i>

    </div>
    <div class="modal_nova_conteudo">
        <p>
            O Relatório Salarial de Mercado com a pesquisa salarial está disponível para os
            Clientes VIP.
        </p>
        <p>
            Seja VIP e confira!
        </p>
        <asp:LinkButton runat="server" ID="btlQueroComprarPlano" CausesValidation="False"
            Text="Saiba mais" OnClick="btlQueroComprarPlano_Click" CssClass="botao_quero_comprar">
        </asp:LinkButton>
        <asp:LinkButton runat="server" ID="btlNaoAgora" CausesValidation="False" Text="Não agora"
            OnClick="btlNaoAgora_Click" CssClass="botao_agora_nao">
        </asp:LinkButton>
    </div>
</asp:Panel>
<asp:HiddenField ID="hf" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeVendaRSM" PopupControlID="pnlVendaRSM" runat="server"
    CancelControlID="btiFechar" TargetControlID="hf">
</AjaxToolkit:ModalPopupExtender>
