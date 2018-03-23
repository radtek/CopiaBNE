<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ucAlertarBNE.ascx.cs"
    Inherits="BNE.Web.UserControls.ucAlertarBNE" %>
<asp:UpdatePanel ID="upAlertarBNE" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="pnlAlertarBNE" runat="server" Style="display: none" CssClass="modal_conteudo candidato">
            <h2 class="titulo_modal">
                Alertar BNE
            </h2>
            <asp:UpdatePanel ID="upBtiFechar" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:ImageButton CssClass="botao_fechar_modal" ID="btiFechar" ImageUrl="/img/botao_padrao_fechar.gif"
                        runat="server" CausesValidation="false" OnClick="btiFechar_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <!-- FIM: Linha Imagens -->
            <div class="painel_destaque_padrao">
                <asp:Label ID="Label1" runat="server" Text="Avise nossa equipe que este currículo está com problema. Descreva o que acontece e faremos o possível para localizar o candidato e corrigir o cadastro."></asp:Label>
            </div>
            <div class="linha">
                <asp:Label ID="lblDescricao" AssociatedControlID="txtDescricao" CssClass="label_principal_modal_reduzida"
                    runat="server" Text="Descrição" />
                <div class="container_campo_modal">
                    <componente:AlfaNumerico ID="txtDescricao" runat="server" Obrigatorio="False" CssClassTextBox="textbox_padrao"
                        MaxLength="350" Rows="4" TextMode="MultiLine" ValidationGroup="Salvar" />
                </div>
            </div>
            <asp:Panel ID="pnlBotoes" CssClass="painel_botoes" runat="server">
                <asp:Button ID="btnSalvar" runat="server" Text="Enviar" CssClass="botao_padrao" CausesValidation="true"
                    ValidationGroup="Salvar" OnClick="btnSalvar_Click" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="botao_padrao"
                    CausesValidation="false" OnClick="btnCancelar_Click" Visible="false" />
            </asp:Panel>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:HiddenField runat="server" ID="hfAlertarBNE" />
<AjaxToolkit:ModalPopupExtender ID="mpeAlertarBNE" runat="server" PopupControlID="pnlAlertarBNE"
    TargetControlID="hfAlertarBNE" />
