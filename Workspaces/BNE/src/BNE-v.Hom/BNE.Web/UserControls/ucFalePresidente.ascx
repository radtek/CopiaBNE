<%@ Control
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="ucFalePresidente.ascx.cs"
    Inherits="BNE.Web.UserControls.FalePresidente" %>
<%@ Register
    Src="~/UserControls/Modais/ModalConfirmacao.ascx"
    TagName="ModalConfirmacao"
    TagPrefix="uc2" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/FalePresidente.css" type="text/css" rel="stylesheet" />
<Employer:DynamicScript runat="server" Src="/js/local/UserControls/FalePresidente.js" type="text/javascript" />

<h1>Fale com o
    Presidente</h1>
<div class="painel_padrao">
    <div class="painel_padrao_topo">
        &nbsp;
    </div>
    <div class="coluna_esquerda presidente">
        <img src="/img/falepresidente/img_fale_presidente_maars.jpg"
            alt="Marcos Aurélio - Presidente do BNE">
    </div>
    <div class="coluna_direita">
        <p class="texto_ola">
            Olá!
        </p>
        <p class="texto_mensagem_presidente">
            Eu sou o Marcos
            Aurélio e
            tenho orgulho
            de ser o Presidente
            do <strong>Banco
                Nacional de
                Empregos.</strong>
            <br />
            Hoje, ele
            é um dos sites
            mais importantes
            do Brasil
            e isso deve-se
            à união de
            todos nós:
            eu, as empresas-clientes,
            a equipe do
            BNE e é lógico
            você.<br />
            Por isso sinta-se
            a vontade:
            dê seu depoimento,
            sua sugestão
            e até reclame
            se quiser!
            Eu quero saber
            o que você
            pensa!
        </p>
        <div class="assinatura">
            <img src="img/falepresidente/img_assinatura_presidente.png"
                alt="Assinatura Presidente">
            <br>
            <span>Marcos Aurélio</span>
        </div>
        <div class="formulario_pagina">
            <div class="linha">
                <asp:Label
                    ID="Label1"
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
                    ID="Label2"
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
                    ID="lblCPF"
                    runat="server"
                    CssClass="label_principal"
                    Text="CPF"
                    AssociatedControlID="txtCPF"></asp:Label>
                <asp:UpdatePanel
                    ID="upCPF"
                    UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <componente:CPF
                            ID="txtCPF"
                            runat="server"
                            ValidationGroup="Enviar"></componente:CPF>
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
                            Rows="5"
                            Columns="200"
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
        </div>
    </div>
</div>
<div class="painel_botoes">
    <asp:Button
        ID="btnEnviar"
        runat="server"
        Text="Enviar"
        CssClass="botao_padrao enviar"
        ValidationGroup="Enviar"
        CausesValidation="true"
        OnClick="btnEnviar_Click" />
    <asp:Button
        ID="btnVoltar"
        runat="server"
        Text="Voltar"
        CssClass="botao_padrao"
        CausesValidation="false"
        OnClick="btnVoltar_Click" />
</div>
<%--Painel Confirmacao Envio--%>
<uc2:ModalConfirmacao
    ID="ucModalConfirmacao"
    runat="server" />
<%-- FIM: Painel Confirmacao Envio--%>
