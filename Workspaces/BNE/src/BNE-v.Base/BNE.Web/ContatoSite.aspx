<%@ Page Title=""
    Language="C#"
    MasterPageFile="~/Master/Principal.Master"
    AutoEventWireup="true"
    CodeBehind="ContatoSite.aspx.cs"
    Inherits="BNE.Web.ContatoSite" %>

<%@ Register
    Src="UserControls/Modais/ModalConfirmacao.ascx"
    TagName="ModalConfirmacao"
    TagPrefix="uc1" %>
<asp:Content
    ID="Content1"
    ContentPlaceHolderID="cphHead"
    runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/ContatoSite.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicScript runat="server" Src="/js/local/Forms/ContatoSite.js" type="text/javascript" />
</asp:Content>
<asp:Content
    ID="Content2"
    ContentPlaceHolderID="cphConteudo"
    runat="server">
    <h1>Contato</h1>
    <p class="texto_destaque_padronizacao">
        O BNE está
        sempre pronto
        para atender
        você!
    </p>
    <div class="painel_padrao">
        <div class="painel_padrao_topo">
            &nbsp;
        </div>
        <div class="coluna_esquerda">
            <asp:Image
                ID="Image1"
                ImageUrl="/img/contatosite/img_contato_numero.png"
                runat="server" />
            <div>
                <a href="mailto:atendimento@bne.com.br">
                    <img alt="atendimento@bne.com.br"
                        src="/img/contatosite/btn_contato_email.png"
                        class="img" />
                </a>
            </div>
            <div>
                <asp:UpdatePanel
                    ID="upAtendimentoOnline"
                    runat="server"
                    RenderMode="Inline"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:ImageButton
                            ImageUrl="/img/contatosite/btn_contato_atendimento_online.png"
                            ID="btiAtendimentoOnline"
                            runat="server"
                            OnClick="btiAtendimentoOnline_Click" />
                        
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="coluna_direita">
            <div class="linha">
                <asp:Label
                    ID="lblNome"
                    CssClass="label_principal"
                    runat="server"
                    Text="Nome"
                    AssociatedControlID="txtNome"></asp:Label>
                <asp:UpdatePanel
                    ID="upNome"
                    UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <componente:AlfaNumerico
                            ID="txtNome"
                            runat="server"
                            Columns="40"
                            MensagemErroFormato='<%$ Resources: MensagemAviso, _100107 %>'
                            MensagemErroIntervalo='<%$ Resources: MensagemAviso, _100107 %>'
                            MensagemErroValor='<%$ Resources: MensagemAviso, _100107 %>'
                            ValidationGroup="Enviar"
                            ClientValidationFunction="ValidarNome" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger
                            ControlID="btnEnviar"
                            EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="linha">
                <asp:Label
                    ID="lblEmail"
                    runat="server"
                    CssClass="label_principal"
                    Text="E-mail"
                    AssociatedControlID="txtEmail"></asp:Label>
                <asp:UpdatePanel
                    ID="upEmail"
                    UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <componente:AlfaNumerico
                            ID="txtEmail"
                            runat="server"
                            Columns="40"
                            MensagemErroFormato="Email inválido"
                            ValidationGroup="Enviar" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger
                            ControlID="btnEnviar"
                            EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="linha">
                <asp:Label
                    ID="lblCidade"
                    runat="server"
                    CssClass="label_principal"
                    Text="Cidade"
                    AssociatedControlID="txtCidade"></asp:Label>
                <div>
                    <asp:RequiredFieldValidator
                        ID="rfvCidade"
                        runat="server"
                        ControlToValidate="txtCidade"
                        ValidationGroup="Enviar"></asp:RequiredFieldValidator>
                    <asp:CustomValidator
                        ID="cvCidade"
                        runat="server"
                        ErrorMessage="Cidade Inválida."
                        ClientValidationFunction="cvCidade_Validate"
                        ControlToValidate="txtCidade"
                        ValidationGroup="Enviar"></asp:CustomValidator>
                </div>
                <asp:UpdatePanel
                    ID="upCidade"
                    UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <asp:TextBox
                            ID="txtCidade"
                            runat="server"
                            Columns="25"></asp:TextBox>
                        <AjaxToolkit:AutoCompleteExtender
                            ID="aceCidade"
                            runat="server"
                            TargetControlID="txtCidade"
                            ServiceMethod="ListarCidadesSiglaEstadoPorNomeParcialEstado">
                        </AjaxToolkit:AutoCompleteExtender>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger
                            ControlID="btnEnviar"
                            EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="linha">
                <asp:Label
                    ID="Label4"
                    runat="server"
                    CssClass="label_principal"
                    Text="Assunto"
                    AssociatedControlID="txtAssunto"></asp:Label>
                <asp:UpdatePanel
                    ID="upAssunto"
                    UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <componente:AlfaNumerico
                            ID="txtAssunto"
                            runat="server"
                            ValidationGroup="Enviar" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger
                            ControlID="btnEnviar"
                            EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="linha">
                <asp:Label
                    ID="lblMensagem"
                    CssClass="label_principal"
                    runat="server"
                    Text="Mensagem"
                    AssociatedControlID="txtMensagem"></asp:Label>
                <asp:UpdatePanel
                    ID="upMensagem"
                    UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <componente:AlfaNumerico
                            ID="txtMensagem"
                            runat="server"
                            Rows="9"
                            Columns="100"
                            MaxLength="1000"
                            ValidationGroup="Enviar"
                            CssClassTextBox="textbox_padrao multiline campo_obrigatorio"
                            TextMode="Multiline"
                            MensagemErroObrigatorio="Campo obrigatório" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger
                            ControlID="btnEnviar"
                            EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="painel_botoes">
                <%--<asp:UpdatePanel
                    ID="upEnviar"
                    UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>--%>
                <asp:Button
                    ID="btnEnviar"
                    CssClass="botao_padrao enviar_contato"
                    runat="server"
                    Text="Enviar"
                    ValidationGroup="Enviar"
                    OnClick="btnEnviar_Click" />
                <%-- </ContentTemplate>
                </asp:UpdatePanel>--%>
            </div>
        </div>
    </div>
    <asp:Panel
        runat="server"
        CssClass="painel_botoes"
        ID="pnlBotoes">
        <asp:Button
            runat="server"
            ID="btnVoltar"
            Text="Voltar"
            CausesValidation="false"
            CssClass="botao_padrao" 
            OnClick="btnVoltar_Click" />
    </asp:Panel>
</asp:Content>
<asp:Content
    ID="Content3"
    ContentPlaceHolderID="cphRodape"
    runat="server">
    <uc1:ModalConfirmacao
        ID="ucModalConfirmacao"
        runat="server" />
</asp:Content>
