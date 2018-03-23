<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IndicarAmigo.ascx.cs" Inherits="BNE.Web.UserControls.IndicarAmigo" %>
<Employer:DynamicScript runat="server" Src="/js/local/UserControls/IndicarAmigo.js" type="text/javascript" />
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/ModalConfirmacaoEnvioCurriculo.css" type="text/css" rel="stylesheet" />
<asp:Panel ID="pnlIndicarAmigoModal" Style="display: none" CssClass="modal_conteudo 1234 empresa reduzida indique" runat="server">
    <h2 class="titulo_modal">
        <asp:Label ID="lblTitulo" runat="server">Indique o BNE</asp:Label>
    </h2>
     <asp:UpdatePanel ID="upIndicarAmigo" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
<asp:LinkButton CssClass="botao_fechar_modal" ID="btnFechar" runat="server" CausesValidation="false" OnClick="btnFechar_Click"><i class="fa fa-times-circle"></i> Fechar</asp:LinkButton>

        
  
            <div class="icone_enviar_amigo">
               
              <i class="fa fa-envelope-o"></i></div>
            <asp:Panel runat="server" ID="pnlFormularioEnviaAmigo" CssClass="FormularioEnviaAmigo">
                <div class="linha">
                    <asp:Label ID="lblNomeIndiqueBNE" CssClass="label_principal" Text="Seu Nome" runat="server"></asp:Label>
                    <div class="container_campo">
                        <div>
                            <asp:RequiredFieldValidator ID="rfNomeIndiqueBNE" ValidationGroup="EnviarIndicacaoBNE" runat="server" ControlToValidate="txtNomeIndiqueBNE"></asp:RequiredFieldValidator>
                        </div>
                        <asp:TextBox ID="txtNomeIndiqueBNE" CausesValidation="false" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="linha">
                    <asp:Label ID="lblEmailIndiqueBNE" CssClass="label_principal" Text="Seu E-mail" runat="server"></asp:Label>
                    <div class="container_campo">
                        <div>
                            <asp:RequiredFieldValidator ID="rfEmailIndiqueBNE" runat="server" ValidationGroup="EnviarIndicacaoBNE" ControlToValidate="txtEmailIndiqueBNE"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cvEmailIndiqueBNE" runat="server" ClientValidationFunction="cvEmailIndiqueBNE_Validate" ControlToValidate="txtEmailIndiqueBNE" ErrorMessage="Email Inválido"></asp:CustomValidator>
                        </div>
                        <asp:TextBox ID="txtEmailIndiqueBNE" CausesValidation="false" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="linha">
                    <asp:Label ID="lblNomeAmigoIndiqueBNE" CssClass="label_principal" Text="Nome do Amigo" runat="server"></asp:Label>
                    <div class="container_campo">
                        <div>
                            <asp:RequiredFieldValidator ID="rfNomeAmigoIndiqueBNE" ValidationGroup="EnviarIndicacaoBNE" runat="server" ControlToValidate="txtNomeAmigoIndiqueBNE"></asp:RequiredFieldValidator>
                        </div>
                        <asp:TextBox ID="txtNomeAmigoIndiqueBNE" CausesValidation="false" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="linha">
                    <asp:Label ID="lblEmailAmigoIndiqueBNE" CssClass="label_principal" Text="E-mail do Amigo" runat="server"></asp:Label>
                    <div class="container_campo">
                        <div>
                            <asp:RequiredFieldValidator ID="rfEmailAmigoIndiqueBNE" runat="server" ValidationGroup="EnviarIndicacaoBNE" ControlToValidate="txtEmailAmigoIndiqueBNE"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cvEmailAmigoIndiqueBNE" runat="server" ClientValidationFunction="cvEmailAmigoIndiqueBNE_Validate" ControlToValidate="txtEmailAmigoIndiqueBNE" ErrorMessage="Email Inválido"></asp:CustomValidator>
                        </div>
                        <asp:TextBox ID="txtEmailAmigoIndiqueBNE" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="linha">
                    <asp:Label ID="lblCelularAmigoIndiqueBNE" CssClass="label_principal" Text="Celular do Amigo" runat="server"></asp:Label>
                    <div class="container_campo">
                        <componente:Telefone ID="txtTelefoneCelularAmigoIndiqueBNE" MensagemErroFormatoFone='<%$ Resources: MensagemAviso, _100006 %>' runat="server" Tipo="Celular" ValidationGroup="EnviarIndicacaoBNE" Obrigatorio="true" />
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlBotoesIndicarAmigo" runat="server" CssClass="painel_botoes">
                <asp:Button ID="btnEnviarIndicacaoAmigoIndiqueBNE" Text="Indicar" ValidationGroup="EnviarIndicacaoBNE" runat="server"  CausesValidation="true" OnClick="btnEnviarIndicacaoAmigo_Click" CssClass="botao_padrao" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:HiddenField ID="hfVariavelModal" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeIndicarAmigoModal" runat="server" PopupControlID="pnlIndicarAmigoModal" TargetControlID="hfVariavelModal">
</AjaxToolkit:ModalPopupExtender>
