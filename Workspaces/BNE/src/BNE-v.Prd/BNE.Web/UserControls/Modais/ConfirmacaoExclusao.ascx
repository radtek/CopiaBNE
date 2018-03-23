<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConfirmacaoExclusao.ascx.cs"
    Inherits="BNE.Web.UserControls.Modais.ConfirmacaoExclusao" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/ModalConfirmacaoEnvioCurriculo.css" type="text/css" rel="stylesheet" />
<asp:Panel ID="pnlConfirmacaoExclusao" runat="server" CssClass="modal_confirmacao_acao"
    Style="display: none">
    <asp:UpdatePanel ID="upBtiFechar" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:ImageButton CssClass="botao_fechar_modal" ID="btiFechar" ImageUrl="/img/botao_padrao_fechar.gif"
                runat="server" CausesValidation="false" OnClick="btiFechar_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upConfirmacaoExclusao" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div>
                <h2 class="titulo_modal">
                    <asp:Label ID="lblTitulo" runat="server"></asp:Label>
                </h2>
            </div>
            <div class="texto_confirmacao">
                <asp:Label ID="lblDescricao" runat="server"></asp:Label>
            </div>
            <asp:Panel ID="pnlBotoes" CssClass="painel_botoes" runat="server">
                <asp:Button ID="btnConfirmar" runat="server" Text="Confirmar" CssClass="botao_padrao"
                    OnClick="btnConfirmar_Click" CausesValidation="false" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="botao_padrao"
                    OnClick="btnCancelar_Click"  CausesValidation="false"/>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:HiddenField ID="hfVariavel" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeConfirmacaoExclusao" runat="server" TargetControlID="hfVariavel"
    PopupControlID="pnlConfirmacaoExclusao">
</AjaxToolkit:ModalPopupExtender>
