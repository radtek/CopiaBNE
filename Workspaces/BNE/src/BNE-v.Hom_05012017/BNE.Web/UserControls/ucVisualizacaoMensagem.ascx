<%@ Control
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="ucVisualizacaoMensagem.ascx.cs"
    Inherits="BNE.Web.UserControls.ucVisualizacaoMensagem" %>
<asp:UpdatePanel
    ID="upMensagem"
    runat="server"
    UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel
            runat="server"
            ID="pnlMensagem"
            CssClass="modal_conteudo reduzida"
            Style="display: none">
            <h2 class="titulo_modal">
                <asp:Label
                    ID="lblTituloMensagem"
                    runat="server"
                    Text="Mensagem"></asp:Label></h2>
            <%--Linha Códgio --%>
            <div class="linha">
                <asp:Label
                    ID="lblCodigo"
                    Text="Código"
                    CssClass="label_principal bold"
                    AssociatedControlID="lblCodigoValor"
                    runat="server" />
                <div class="container_campo">
                    <asp:Label
                        ID="lblCodigoValor"
                        Text=""
                        runat="server" />
                </div>
                 </div>
                 
                 <div class="linha">
                <asp:Label
                    runat="server"
                    ID="lblCandidato"
                    Text="Candidato"
                       CssClass="label_principal bold"
                    AssociatedControlID="lblCandidatoValor" />
                    <div class="container_campo">
                <asp:Label
                    ID="lblCandidatoValor"
                    Text=""
                    runat="server" />
                    </div>
                    </div>
                    
                    <div class="linha">
                <asp:Label
                    runat="server"
                    ID="lblEnvio"
                     CssClass="label_principal bold"
                    Text="Envio"
                    AssociatedControlID="lblEnvioValor" />
                    <div class="container_campo">
                <asp:Label
                    ID="lblEnvioValor"
                    Text=""
                    runat="server" />
                    </div>
                    </div>
           
            <%--FIM: Linha Códgio --%>
            <%--Linha Tipo --%>
            <div class="linha">
                <asp:Label
                    ID="lblTipo"
                    Text="Tipo"
                     CssClass="label_principal bold"
                    AssociatedControlID="lblTipoValor"
                    runat="server" />
                <div class="container_campo">
                    <asp:Label
                        ID="lblTipoValor"
                        Text=""
                        runat="server" />
                </div>
                
                <div class="linha">
                <asp:Label
                    runat="server"
                    ID="lblFormaEnvio"
                     CssClass="label_principal bold"
                    Text="Forma de Envio"
                    AssociatedControlID="lblFormaEnvioValor" />
              <div class="container_campo">
                <asp:Label
                    ID="lblFormaEnvioValor"
                    Text=""
                    runat="server" />
                    </div>
                    </div>
                    
                    <div class="linha">
                <asp:Label
                    runat="server"
                      CssClass="label_principal bold"
                    ID="lblUsuario"
                    Text="Usuário"
                    AssociatedControlID="lblUsuarioValor" />
                    <div class="container_campo">
                <asp:Label
                    ID="lblUsuarioValor"
                    Text=""
                    runat="server" />
                    </div>
            </div>
            <%--FIM: Linha Tipo --%>
            <%--Linha Mensagem --%>
            <div class="linha">
                <asp:Label
                    ID="lblMensagem"
                    Text="Mensagem"
                      CssClass="label_principal bold"
                    AssociatedControlID="lblMensagemValor"
                    runat="server" />
                <div class="container_campo mensagem">
                    <asp:Label
                        ID="lblMensagemValor"
                        Text=""
                        runat="server" />
                </div>
            </div>
            <%--Linha Mensagem --%>
            <!-- Painel botoes -->
            <asp:UpdatePanel
                ID="upBotoes"
                runat="server"
                UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel
                        runat="server"
                        CssClass="painel_botoes"
                        ID="pnlBotoes">
                        <asp:Button
                            ID="btnFechar"
                            runat="server"
                            CssClass="botao_padrao"
                            CausesValidation="false"
                            Text="Fechar" />
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <!-- FIM: Painel botoes -->
        </asp:Panel>
        <asp:HiddenField
            runat="server"
            ID="hfAux" />
        <AjaxToolkit:ModalPopupExtender
            ID="mpeModalMensagem"
            runat="server"
            PopupControlID="pnlMensagem"
            TargetControlID="hfAux"
            CancelControlID="btnFechar" />
    </ContentTemplate>
</asp:UpdatePanel>
