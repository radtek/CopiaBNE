<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EnvioEmail.ascx.cs"
    Inherits="BNE.Web.UserControls.Modais.EnvioEmail" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/EnvioEmail.css" type="text/css" rel="stylesheet" />
<Employer:DynamicScript runat="server" Src="/js/local/UserControls/FalePresidente.js" type="text/javascript" />

<asp:Panel ID="pnlEnvioEmail" runat="server" CssClass="modal_email_sucesso" Style="display: none">
    <div>
        <h2 class="titulo_enviar_email">
            <asp:Label ID="lbTituloModal" runat="server" />
        </h2>

        <div class="limpa_float_envio_email">&nbsp;</div>

        <asp:UpdatePanel ID="upBtiFechar" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:ImageButton CssClass="botaoNovo_fechar_modal" ID="btiFechar" ImageUrl="/img/modal_nova/btnRedFecharModal.png"
                    runat="server" CausesValidation="false" OnClick="btiFechar_Click" />
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="upEnvioEmail" CssClass="containerConteudoModal" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlFormularioEmail" runat="server">
                    <p class="labelDigiteAbaixoAlinha">Preencha os campos abaixo e envie seu e-mail para
                        <asp:Label ID="lblTipoEnvio" runat="server"></asp:Label></p>
                    <div>
                        <asp:Label ID="lbNome" runat="server" CssClass="label_nome"
                            Text="Nome:" AssociatedControlID="txtNome" />
                        <div class="controle_email">
                            <div class="text_box_modal_email">
                                <componente:AlfaNumerico ID="txtNome"
                                    runat="server"
                                    Columns="40"
                                    MensagemErroFormato='<%$ Resources: MensagemAviso, _100107 %>'
                                    MensagemErroIntervalo='<%$ Resources: MensagemAviso, _100107 %>'
                                    MensagemErroValor='<%$ Resources: MensagemAviso, _100107 %>'
                                    ValidationGroup="Email"
                                    ClientValidationFunction="ValidarNome"
                                    Width="202px" />
                            </div>
                        </div>
                    </div>

                    <div style="clear:both">
                        <asp:Label ID="lbEmail" runat="server" CssClass="label_email"
                            Text="E-mail:" AssociatedControlID="txtEmail" />
                        <div class="controle_email">
                            <div class="text_box_modal_email">
                                <componente:AlfaNumerico
                                    ID="txtEmail"
                                    runat="server"
                                    Columns="40"
                                    MensagemErroFormato="Email inválido"
                                    ValidationGroup="Email"
                                    Width="202px" />
                            </div>
                        </div>
                    </div>

                    <div style="clear:both">
                        <asp:Label ID="lbMensagem" runat="server" CssClass="label_mensagem"
                            Text="Mensagem:" AssociatedControlID="txtMensagem" />
                        <div class="controle_email">
                            <div class="text_box_modal_email">
                                <componente:AlfaNumerico
                                    ID="txtMensagem"
                                    runat="server"
                                    Rows="5"
                                    Columns="200"
                                    MaxLength="1000"
                                    ValidationGroup="Email"
                                    CssClassTextBox="textbox_padrao multiline campo_obrigatorio"
                                    TextMode="Multiline"
                                    MensagemErroObrigatorio="Campo obrigatório"
                                    Width="202px" />
                            </div>
                        </div>
                    </div>

                    <asp:Button runat="server" ID="btnEnviar2" Text="Enviar e-mail" CssClass="botao_padrao botao_enviar_email" OnClick="btnEnviar_Click"
                        CausesValidation="true" ValidationGroup="Email" />

                </asp:Panel>

                <asp:Panel ID="pnlSucesso2" runat="server" Visible="false">
                    <div id="divImgConfirm" runat="server" class="imagem_confirm_envio_email">
                        <asp:Image ID="imgConfirm" runat="server" BorderWidth="0px" />
                    </div>
                    <p class="texto_cadastro_sucesso_envio_email">
                        <asp:Label ID="lblSucesso" runat="server"></asp:Label>
                    </p>
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger
                    ControlID="btnEnviar2"
                    EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Panel>

<asp:HiddenField ID="hfVariavel" runat="server" />
<asp:HiddenField ID="hfIdPlanoAdquirido" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeEnvioEmail" PopupControlID="pnlEnvioEmail"
    TargetControlID="hfVariavel" runat="server">
</AjaxToolkit:ModalPopupExtender>
