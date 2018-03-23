<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ucAlertarBNE.ascx.cs"
    Inherits="BNE.Web.UserControls.ucAlertarBNE" %>
<asp:UpdatePanel ID="upAlertarBNE" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="pnlAlertarBNE" runat="server" Style="display: none" CssClass="modal_conteudo candidato">
            <div class="titulo_modal">
                <h2>Denunciar Currículo</h2>
                <asp:UpdatePanel runat="server" ID="upFechar">
                    <ContentTemplate>
                        <asp:LinkButton CssClass="fechar" ID="btiFechar" runat="server" CausesValidation="false" OnClick="btlFechar_Click">
                            <span aria-hidden="true">×</span>
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="conteudo-modal">
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
