<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BloquearCandidato.ascx.cs" Inherits="BNE.Web.UserControls.Modais.BloquearCandidato" %>
<asp:Panel ID="pnlBloquearCandidato" CssClass="modal_conteudo candidato reduzida bloquear_candidato"
    Style="display: none;" runat="server">
    <asp:UpdatePanel ID="upBloquearCandidato" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <h2 class="titulo_modal">
                <asp:Label ID="Label1" runat="server" Text="Bronquinha"></asp:Label>
            </h2>
            <asp:UpdatePanel ID="upBtiFechar" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:ImageButton CssClass="botao_fechar_modal" ID="btiFechar" ImageUrl="/img/botao_padrao_fechar.gif"
                        runat="server" CausesValidation="false" OnClick="btiFechar_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:Panel CssClass="coluna_esquerda bloqueio" ID="pnlColunaEsquerda" runat="server">
                <asp:Panel CssClass="painel_bronquinha" ID="pnlEsquerdaBloquearCandidato" runat="server">
                    <asp:Image CssClass="logo_empresa" ID="imgLogo" ImageUrl="/img/icn_bronquinha.png" AlternateText="" runat="server" />
                </asp:Panel>
            </asp:Panel>
            <asp:Panel CssClass="painel_empresa_confidencial painel_bloquear_candidato" ID="pnlBloquearCandidatoCentro"
                runat="server">
                <div class="linha">
                    <div class="label_principal modal">
                        <asp:Literal ID="litTextoNomeCandidato" runat="server"></asp:Literal></div>
                    <div class="container_campo modal">
                        <asp:Literal ID="lblNomeCandidato" runat="server"></asp:Literal></div>
                </div>
                <div class="linha">
                    <asp:Label runat="server" ID="lblInformeMotivo" Text="Informe abaixo o motivo:" CssClass="label_principal modal" />
                    <br /><asp:RequiredFieldValidator runat="server" ID="rfMotivo" ControlToValidate="tbxMotivo" Text="Campo Obrigatório." ValidationGroup="Bloquear"></asp:RequiredFieldValidator>
                    <telerik:RadTextBox runat="server" ID="tbxMotivo" TextMode="MultiLine" ValidationGroup="Bloquear" MaxLength="512"
                        EmptyMessage="" CssClass="textbox_padrao bloquear_candidato">
                    </telerik:RadTextBox>
                </div>
                <asp:Panel CssClass="painel_botoes" ID="pnlBloquearDesbloquear1" runat="server">
                    <asp:Button ID="btnBloquearCandidato" runat="server" Text="Bloquear" CssClass="botao_padrao" CausesValidation="true"
                        ValidationGroup="Bloquear" OnClick="btnBloquearCandidato_Click" />
                    <asp:Button ID="btnBloqueadoCandidato" runat="server" Text="Desbloquear" CssClass="botao_padrao" CausesValidation="true"
                        ValidationGroup="Bloquear" OnClick="btnDesbloquearCandidato_Click" />
                </asp:Panel>
            </asp:Panel>
            <asp:Panel CssClass="painel_empresa_confidencial painel_confirmar_bloqueio" ID="pnlPerguntaBloquear"
                runat="server">
                <div class="pergunta_bloquear">
                    <asp:Literal ID="litPerguntaBloquear" runat="server"></asp:Literal>
                </div>
                <asp:Panel CssClass="painel_botoes" ID="pnlBotaoBloquieoDesbloqueio" runat="server">
                    <asp:Button ID="btnSimBloquear" runat="server" Text="Sim" CssClass="botao_padrao" CausesValidation="true"
                        ValidationGroup="Bloquear" OnClick="btnSimBloquear_Click" />
                    <asp:Button ID="btnNaoBloquear" runat="server" Text="Não" CssClass="botao_padrao" CausesValidation="true"
                        ValidationGroup="Bloquear" OnClick="btnNaoBloquear_Click" />
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:HiddenField ID="hfVariavel" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeBloquearCandidato" TargetControlID="hfVariavel" PopupControlID="pnlBloquearCandidato"
    runat="server">
</AjaxToolkit:ModalPopupExtender>
