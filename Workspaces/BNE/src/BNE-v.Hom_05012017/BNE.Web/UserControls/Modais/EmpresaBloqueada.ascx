<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmpresaBloqueada.ascx.cs" Inherits="BNE.Web.UserControls.Modais.EmpresaBloqueada" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/EmpresaBloqueada.css" type="text/css" rel="stylesheet" />
<asp:Panel ID="pnlEmpresaBloqueada" runat="server" CssClass="modal_confirmacao_registro candidato reduzida"
    Style="display: none; background-color: #ebeff2 !important; height: auto !important;">

    <asp:ImageButton CssClass="modal_fechar" ID="btiFechar" ImageUrl="/img/modal_nova/btn_amarelo_fechar_modal.png"
        runat="server" CausesValidation="false" />

    <asp:Panel CssClass="coluna_esquerda bloqueio" ID="pnlColunaEsquerda" runat="server">
        <asp:Panel CssClass="painel_bronquinha" ID="pnlEsquerdaBloquearCandidato" runat="server">
              <div class="alert-icon" id="divAlert" runat="server"></div>
        </asp:Panel>
    </asp:Panel>
    <div class="container_confirmacao_candidatura">
        <asp:Panel ID="pnlBloqueada" runat="server" Visible="false">
            <div class="container_confirmacao_candidatura">
                <p class="texto_modal_empresa_bloqueada">
                    <asp:Label ID="Label1" runat="server" Text="O acesso para sua empresa possui um bloqueio!"></asp:Label>
                <br /><br />
                
                    Para reativar ligue 0800 41 2400
                </p>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlAuditoria" runat="server" Visible="false">
            <div class="container_confirmacao_candidatura">
              <p class="texto_modal_empresa_bloqueada">
                    <asp:Label ID="Label2" runat="server" Text="Pronto, recebemos suas informações!"></asp:Label>
                <br><br />
                    Faremos contato em horário comercial para finalizar a ativação do seu acesso, por favor aguarde.
                </p>
            </div>
        </asp:Panel>

    </div>
</asp:Panel>
<asp:HiddenField ID="hf" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeEmpresaBloqueada" PopupControlID="pnlEmpresaBloqueada"
    runat="server" CancelControlID="btiFechar" TargetControlID="hf">
</AjaxToolkit:ModalPopupExtender>
