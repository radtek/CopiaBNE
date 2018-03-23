<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Login.ascx.cs" Inherits="BNE.Web.UserControls.Login" %>
<%@ Register Src="Modais/TrocarEmpresa.ascx" TagName="TrocarEmpresa" TagPrefix="uc1" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Login.css" type="text/css" rel="stylesheet" />
<div class="painel_login_padrao">
    <asp:Panel runat="server" ID="pnlLoginEsquerda" DefaultButton="btnEntrar" CssClass="painel_login_esquerda">
        <div class="painel_login">
            <div class="painel_login_campos row">
                <p class="col-md-12 center" id="titulo_solicita_dados">
                    Informe seus dados para acessar:
                </p>
                <!-- Linha CPF -->
                <div class="col-md-12">
                    <asp:Label runat="server" ID="lblCPF" CssClass="label_principal modal_login" Text="CPF"
                        AssociatedControlID="txtCPF" />
                    <div class="container_campo_login inputlogin">
                        <asp:UpdatePanel ID="upCpf" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <componente:CPF ID="txtCPF" runat="server" MensagemErroFormato='<%$ Resources: MensagemAviso, _103701 %>'
                                    ValidationGroup="Entrar" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <!-- Linha Data de Nascimento -->
                <div class="col-md-12">
                    <asp:Label runat="server" ID="lblDataNascimento" CssClass="label_principal modal_login  "
                        Text="Data de Nascimento" AssociatedControlID="txtDataNascimento" />
                    <div class="container_campo_login inputlogin">
                        <asp:UpdatePanel ID="upDataNascimento" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <Componentes:DataTextBox runat="server" ID="txtDataNascimento" Tamanho="10" ValidationGroup="Entrar"
                                    MensagemErroFormato='<%$ Resources: MensagemAviso, _103702 %>' />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <!-- Painel Informação AD -->
                <div class="col-md-12">                    
                    <asp:UpdatePanel ID="upInformacaoAD" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel ID="pnlInformacaoAD" runat="server">
                                <!-- Linha Usuário -->
                                <%--<div class="linha">
                                <asp:Label runat="server" ID="lblUsuario" CssClass="label_principal modal_login"
                                    Text="Nome de Usuário" AssociatedControlID="txtNomeUsuario" />
                                <div class="container_campo">
                                    <componente:AlfaNumerico ID="txtNomeUsuario" runat="server" Columns="10" MaxLength="50"
                                        Obrigatorio="True" />
                                </div>
                            </div>--%>
                                <!-- Linha Senha -->
                                <div class="linha">
                                    <asp:Label runat="server" ID="Label1" CssClass="label_principal modal_login" Text="Senha"
                                        AssociatedControlID="txtSenhaUsuario" />
                                    <div class="container_campo_login">
                                        <componente:AlfaNumerico ID="txtSenhaUsuario" runat="server" Columns="10" MaxLength="10"
                                            Obrigatorio="True" TextMode="Password" />
                                    </div>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="txtDataNascimento" EventName="ValorAlterado" />
                            <asp:AsyncPostBackTrigger ControlID="btnEntrar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <!--  Mantenha Logado -->
                <div class="col-md-12">
                    <asp:CheckBox runat="server" ID="ckMantenhaLogado" Text="Mantenha-me logado" TextAlign="Right"
                        CssClass="mantenha_logado" />
                </div>
            </div>
            <!-- Painel Para Informação Autenticação no Sistema -->
            <asp:UpdatePanel ID="upAvisoAutenticacao" runat="server" UpdateMode="Conditional"
                Visible="false" >
                <ContentTemplate>
                    <asp:Panel ID="pnlInformacaoAutenticacao" runat="server" CssClass="painel_destaque_padrao col-md-12">
                        <asp:Label ID="lblAvisoAutenticacao" runat="server"></asp:Label>
                    </asp:Panel>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="txtDataNascimento" EventName="ValorAlterado" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <!--  Cadastre-se já -->
        <asp:UpdatePanel ID="upLinkButtons" runat="server" UpdateMode="Conditional" >
            <ContentTemplate>
                <asp:Panel ID="pnlCadastreJa" runat="server" CssClass="painel_cadastreja col-md-12">
                    <asp:LinkButton ID="btlCadastrar" runat="server" Text="Cadastre-se já" OnClick="btlCadastrar_Click"
                        CausesValidation="false"></asp:LinkButton>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <!-- Panel Botoes -->
        <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
            <asp:UpdatePanel ID="upBtnEntrar" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="btnEntrar" runat="server" Text="Confirmar" CssClass="botao_padrao"
                        ValidationGroup="Entrar" CausesValidation="True" OnClick="btnEntrar_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:Button ID="btnCancelar" runat="server" Text="Voltar" CssClass="botao_padrao"
                CausesValidation="false" OnClick="btnCancelar_Click" Visible="false" />
        </asp:Panel>
    </asp:Panel>
    <asp:Panel class="painel_login_direita row" runat="server" ID="pnlLoginFacebook">
        <p class="col-md-12 center" style="padding-top:24px;">
            Ou entre com:
        </p>
        <div class="container_botao_facebook col-md-12">
            <asp:UpdatePanel ID="upBtnEntrarFacebook" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:ImageButton runat="server" OnClientClick="BNEFacebook.EfetuarLogin();" CssClass="botao_entrar_facebook"
                        AlternateText="Facebook" CausesValidation="False" ImageUrl="/img/botao_facebook.gif"
                        ID="btnEntrarFacebook" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </asp:Panel>
</div>
<uc1:TrocarEmpresa ID="ucModalTrocarEmpresa" runat="server" />
