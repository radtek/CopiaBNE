<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModalConfirmacao.ascx.cs"
    Inherits="BNE.Web.UserControls.Modais.ModalConfirmacao" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/ModalConfirmacaoEnvioCurriculo.css" type="text/css" rel="stylesheet" />
<asp:UpdatePanel ID="upModalSucesso" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <!-- Painel: Modal Sucesso -->
        <asp:HiddenField ID="hfModalSucesso" runat="server" />
        <asp:Panel ID="pnlModalSucesso" runat="server" CssClass="modal_confirmacao_registro candidato reduzida"
            Style="display: none">
            <asp:UpdatePanel ID="upConteudo" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="sucesso-icon" id="divSucess" runat="server"></div>
                    <div class="imagem_cadastro_alert" id="divAlert" visible="false" runat="server"></div>

                    <div class="container_confirmacao_candidatura">
                        <h2 class="titulo_modal">
                        <asp:Label ID="texto_confirmacao" runat="server"></asp:Label></h2>
                        <asp:UpdatePanel ID="upBtiFechar" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton CssClass=" modal_fechar" ID="btiFecha" runat="server" CausesValidation="false" OnClick="btiFecha_Click"></asp:LinkButton>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    
                        <p class="texto_enviado_sucesso">
                            <asp:Label ID="lblTitulo" runat="server" Visible="false"></asp:Label><br />
                            <asp:Label ID="lblTexto" runat="server"></asp:Label><br />
                            <asp:Label ID="lblTextoAuxiliar" runat="server" Visible="false" CssClass="texto_menor"></asp:Label><br />
                            <div class="btnVoltar" style="text-align:center;">
                            <asp:Button ID="btnVoltar" runat="server" Text="Voltar" CausesValidation="false"
                                OnClick="btnVoltar_Click"></asp:Button>
                               
                            <asp:LinkButton ID="btlCliqueAqui" runat="server" Text="Clique aqui" CausesValidation="false"
                                OnClick="btlCliqueAqui_Click"></asp:LinkButton>
                                 </div>
                        </p>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <!-- FIM: Painel Modal Curriculo -->
        </asp:Panel>
        <AjaxToolkit:ModalPopupExtender ID="mpeModalSucesso" runat="server" PopupControlID="pnlModalSucesso"
            TargetControlID="hfModalSucesso" />
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btiFecha" EventName="Click" />
    </Triggers>
</asp:UpdatePanel>

