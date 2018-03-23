<%@ Control
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="ModalConfirmacaoRetornoPagamento.ascx.cs"
    Inherits="BNE.Web.UserControls.Modais.ModalConfirmacaoRetornoPagamento" %>
<%-- Inclusão do CSS específico pelo C# no evento Page_PreInit --%>
<AjaxToolkit:ModalPopupExtender
    BehaviorID="mpeModalConfirmacaoRetornoPagamentoBehavior"
    ID="mpeModalConfirmacaoRetornoPagamento"
    PopupControlID="pnlModalConfirmacaoRetornoPagamento"
    runat="server"
    TargetControlID="hfModalConfirmacaoRetornoPagamento">
</AjaxToolkit:ModalPopupExtender>
<asp:Panel
    ID="pnlModalConfirmacaoRetornoPagamento"
    runat="server"
    CssClass="modal_conteudo candidato reduzida"
    Style="display: none">
    <h2 class="titulo_modal">
        <span>Pagamento
            realizado</span>
    </h2>
    <asp:UpdatePanel
        ID="upBtiFechar"
        runat="server"
        UpdateMode="Conditional">
        <ContentTemplate>
            <asp:ImageButton
                CssClass="botao_fechar_modal"
                ID="btiFechar"
                ImageUrl="/img/botao_padrao_fechar.gif"
                runat="server"
                CausesValidation="false"
                OnClick="btiFechar_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="icone_confirmacao">
        <img alt=""
            src="/img/img_modal_confirmacao_envio_curriculo_icone.png" />
    </div>
    <p class="texto_pagamento_sucesso">
        Pagamento
        realizado
        com sucesso!</p>
    <p class="texto_utilizar_servicos">
        Seu pagamento
        foi realizado
        e a partir
        de agora você
        já pode utilizar
        os serviços
        exclusivos
        do BNE. Caso
        precise de
        ajuda, utilize
        o Atendimento
        Online.</p>
    <asp:Panel
        ID="pnlBotoes"
        runat="server"
        CssClass="painel_botoes">
        <asp:Button
            ID="btnVoltarSite"
            runat="server"
            CssClass="botao_padrao"
            Text="Voltar para o site"
            
            CausesValidation="false" 
            onclick="btnVoltarSite_Click" />
    </asp:Panel>
</asp:Panel>
<asp:HiddenField
    ID="hfModalConfirmacaoRetornoPagamento"
    runat="server" />
