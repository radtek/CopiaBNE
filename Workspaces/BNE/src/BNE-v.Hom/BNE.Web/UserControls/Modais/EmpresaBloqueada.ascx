<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmpresaBloqueada.ascx.cs" Inherits="BNE.Web.UserControls.Modais.EmpresaBloqueada" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/EmpresaBloqueada.css" type="text/css" rel="stylesheet" />
<asp:Panel ID="pnlEmpresaBloqueada" runat="server" Style="display: none;" CssClass="modal_conteudo empresa">
    <div class="titulo_modal">
        <h2></h2>
        <asp:LinkButton CssClass="fechar" ID="btlFechar" runat="server" CausesValidation="false">
            <span aria-hidden="true">×</span>
        </asp:LinkButton>
    </div>
    <div class="conteudo-modal">
        <asp:UpdatePanel runat="server" ID="upConteudoBloqueio" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlBloqueada" runat="server" Visible="false">
                    <div class="container_confirmacao_candidatura">
                        <p class="texto_modal_empresa_bloqueada">
                            <asp:Label ID="Label1" runat="server" Text="O acesso para sua empresa possui um bloqueio!"></asp:Label>
                            <br />
                            <br />

                            Para reativar ligue 0800 41 2400
                        </p>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlAuditoria" runat="server" Visible="false">
                    <div class="container_confirmacao_candidatura">
                        <p class="texto_modal_empresa_bloqueada">
                            <asp:Label ID="Label2" runat="server" Text="Pronto, recebemos suas informações!"></asp:Label>
                            <br>
                            <br />
                            Faremos contato em horário comercial para finalizar a ativação do seu acesso, por favor aguarde.
                        </p>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Panel>
<asp:HiddenField ID="hf" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeEmpresaBloqueada" PopupControlID="pnlEmpresaBloqueada"
    runat="server" CancelControlID="btlFechar" TargetControlID="hf">
</AjaxToolkit:ModalPopupExtender>
