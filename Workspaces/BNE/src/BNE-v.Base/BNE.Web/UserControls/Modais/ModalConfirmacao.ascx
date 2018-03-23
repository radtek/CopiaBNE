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
                    <h2 class="titulo_modal">
                        <asp:Label ID="lblTitulo" runat="server"></asp:Label></h2>
                    <asp:UpdatePanel ID="upBtiFechar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ImageButton CssClass="botao_fechar_modal" ID="btiFechar" ImageUrl="/img/botao_padrao_fechar.gif"
                                runat="server" CausesValidation="false" OnClick="btiFechar_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="imagem_cadastro_sucesso">
                    </div>
                    <p class="texto_cadastro_sucesso">
                        <asp:Label ID="lblTexto" runat="server"></asp:Label>
                        <asp:Label ID="lblTextoAuxiliar" runat="server" Visible="false" CssClass="texto_menor"></asp:Label>
                        <asp:LinkButton ID="btlCliqueAqui" runat="server" Text="Clique aqui" CausesValidation="false"
                            OnClick="btlCliqueAqui_Click"></asp:LinkButton>
                    </p>
                </ContentTemplate>
            </asp:UpdatePanel>
            <!-- FIM: Painel Modal Curriculo -->
        </asp:Panel>
        <AjaxToolkit:ModalPopupExtender ID="mpeModalSucesso" runat="server" PopupControlID="pnlModalSucesso"
            TargetControlID="hfModalSucesso" />
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btiFechar" EventName="Click" />
    </Triggers>
</asp:UpdatePanel>
