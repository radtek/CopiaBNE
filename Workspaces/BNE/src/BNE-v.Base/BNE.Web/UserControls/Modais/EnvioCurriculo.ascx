<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EnvioCurriculo.ascx.cs"
    Inherits="BNE.Web.UserControls.Modais.EnvioCurriculo" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/EnvioCurriculo.css" type="text/css" rel="stylesheet" />
<Employer:DynamicScript runat="server" Src="/js/local/UserControls/EnvioCurriculo.js" type="text/javascript" />
<asp:Panel ID="pnlEnvioCurriculo" runat="server" CssClass="modal_conteudo modal_envio_curriculo empresa reduzida"
    Style="display: none">
    <asp:UpdatePanel ID="upEnvioCurriculo" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
           <div class="head-modal modal-a">
             
               <asp:LinkButton CssClass="botao_fechar_modal" ID="btiFechar" OnClick="btiFechar_Click"
                CausesValidation="false" runat="server"><i class="fa fa-times-circle"></i> Fechar</asp:LinkButton>
            <h2>
                <asp:Label ID="lblTitulo" runat="server" Text="Enviar Currículo" />
            </h2>

               </div>

           
            <asp:UpdatePanel ID="upEnvioPara" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <%--Linha Envio Para--%>
                    <div class="linha">
                        <asp:Label ID="lblPara" runat="server" CssClass="label_principal envio_curriculo"
                            AssociatedControlID="txtEnvioPara" Text="Para"></asp:Label>
                        <div class="container_campo_modal">
                            <div>
                                <asp:RequiredFieldValidator ID="rfvEnvioPara" ValidationGroup="EnviarCurriculo" runat="server"
                                    ControlToValidate="txtEnvioPara"></asp:RequiredFieldValidator>

                                <asp:CustomValidator ID="cvEmailEnviarPara" ValidationGroup="EnviarCurriculo" ControlToValidate="txtEnvioPara"
                                    runat="server" ClientValidationFunction="cvEmailEnviarPara_Validate" Text="Email Inválido"></asp:CustomValidator>

                            </div>
                            <asp:TextBox ID="txtEnvioPara" runat="server" MaxLength="100" CssClass="textbox_padrao envio_curriculo campo_obrigatorio"></asp:TextBox>
                            <AjaxToolkit:TextBoxWatermarkExtender ID="txtEnvioPara_TextBoxWatermarkExtender"
                                runat="server" Enabled="True" TargetControlID="txtEnvioPara" WatermarkText="Digite o e-mail ou e-mails separados por ;">
                            </AjaxToolkit:TextBoxWatermarkExtender>
                        </div>
                    </div>
                    <%--FIM Linha Envio Para--%>
                </ContentTemplate>
            </asp:UpdatePanel>
            <%--Linha Assunto--%>
            <div class="linha">
                <asp:Label ID="lblAssunto" runat="server" CssClass="label_principal envio_curriculo"
                    AssociatedControlID="txtAssunto" Text="Assunto"></asp:Label>
                <div class="container_campo_modal">
                    <componente:AlfaNumerico ID="txtAssunto" runat="server" ValidationGroup="EnviarCurriculo"
                        CssClassRequiredField="validador" ContemIntervalo="false" Obrigatorio="true"
                        Tipo="AlfaNumerico" />
                </div>
            </div>
            <%--FIM Linha Assunto--%>
            <%--Linha Mensagem--%>
            <div class="linha">
                <asp:Label ID="lblMensagem" runat="server" CssClass="label_principal envio_curriculo"
                    AssociatedControlID="txtMensagem" Text="Mensagem"></asp:Label>
                <div class="container_campo_modal">
                    <asp:TextBox ID="txtMensagem" runat="server" CssClass="textarea_padrao" TextMode="MultiLine"
                        Rows="3" MaxLength="2000"></asp:TextBox>
                </div>
            </div>
            <%--FIM Linha Mensagem--%>
            <%-- Chekbox Ocultar --%>
            <asp:UpdatePanel ID="upCheckBox" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="container_checkbox">
                        <asp:CheckBox ID="cbOcultarFoto" CssClass="chekbox_ocultar_foto" runat="server" Text="Ocultar foto do candidato" />
                        <asp:CheckBox ID="cbOcultarDados" runat="server" Text="Ocultar dados de contato" />
                        <asp:CheckBox ID="cbOcultarObservacoes" runat="server" Text="Ocultar observações"
                            Checked="True" />
                        <div id="containerFormaEnvio" runat="server">
                            <h2 class="titulo_painel_padrao_modal empresa">
                                <asp:Label ID="lblFormaEnvio" runat="server" Text="Enviar como"></asp:Label>
                            </h2>
                            <asp:CheckBox CssClass="chekbox_aberto_email" ID="cbAbertoEmail" runat="server" Text="Aberto no e-mail"
                                Checked="true" />
                            <asp:CheckBox CssClass="chekbox_anexo_pdf" ID="cbAnexoPdf" runat="server" Text="Anexo com o PDF"
                                Checked="true" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <%--FIM Linha RadioButtons--%>
            <%-- Painel botoes --%>
            <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes centro">
                <asp:Button ID="btnEnviarCurriculo" runat="server" Text="Enviar" ValidationGroup="EnviarCurriculo"
                    CssClass="botao_padrao" OnClick="btnEnviarCurriculo_Click" CausesValidation="true" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%-- FIM: Painel botoes --%>
</asp:Panel>
<asp:HiddenField ID="hfVariavel" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeEnvioCurriculo" PopupControlID="pnlEnvioCurriculo"
    TargetControlID="hfVariavel" runat="server">
</AjaxToolkit:ModalPopupExtender>
