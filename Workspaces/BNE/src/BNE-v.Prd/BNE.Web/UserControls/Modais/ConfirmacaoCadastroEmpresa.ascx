<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ConfirmacaoCadastroEmpresa.ascx.cs"
    Inherits="BNE.Web.UserControls.Modais.ConfirmacaoCadastroEmpresa" %>
<asp:Panel ID="pnlConfirmacaoCadastroEmpresa" runat="server" CssClass="modal_nova"
    Style="display: none">
    <asp:UpdatePanel ID="upBtiFechar" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:ImageButton CssClass="botao_fechar_modal" ID="btiFechar" ImageUrl="/img/modal_nova/btn_fechar_modal.png"
                runat="server" CausesValidation="false" OnClick="btiFechar_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="modal_nova_imagem">
        <asp:Image runat="server" ImageUrl="/img/icone_cadastro_sucesso.png" ID="imgAlerta"
            AlternateText="Alerta" />
    </div>
    <div class="modal_nova_conteudo">
        <p>
            Bem vindo ao BNE!
        </p>
        <p>
            Seu cadastro foi efetivado com sucesso.
        </p>
        <p>
            Temporariamente algumas funcionalidades estarão desabilitadas, em breve nossa equipe
            fará contato para a liberação completa.
        </p>
        <p>
            Caso tenha urgência, ligue 0800 41 2400
        </p>
    </div>
</asp:Panel>
<asp:HiddenField ID="hfVariavel" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeConfirmacaoCadastroEmpresa" PopupControlID="pnlConfirmacaoCadastroEmpresa"
    runat="server" TargetControlID="hfVariavel">
</AjaxToolkit:ModalPopupExtender>
