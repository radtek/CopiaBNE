<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IndicarEmpresa.ascx.cs"
    Inherits="BNE.Web.UserControls.Modais.IndicarEmpresa" %>
<Employer:DynamicScript runat="server" Src="/js/local/UserControls/IndicarEmpresa.js" type="text/javascript" />
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/ModalConfirmacaoEnvioCurriculo.css" type="text/css" rel="stylesheet" />
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/IndicarEmpresa.css" type="text/css" rel="stylesheet" />
<asp:Panel ID="pnlIndicarEmpresa" CssClass="modal_indicar_empresa candidato reduzida"
    Style="display: none" runat="server">
    <h2 class="titulo_modal">
        <span>Indicação de Empresa </span>
    </h2>
    <asp:UpdatePanel ID="upBtiFechar" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:ImageButton CssClass="botao_fechar_modal" ID="btiFechar" ImageUrl="/img/botao_padrao_fechar.gif"
                runat="server" CausesValidation="false" OnClick="btiFechar_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upPainelBotoes" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <asp:Panel runat="server" CssClass="panel_padrao">
                <p class="textocomplementar">
                    Trabalhe onde quiser! O BNE ajudará você.
                </p>
                <p class="textoform">
                    Preencha os campos abaixo:
                </p>
                <div class="linha modal">
                    <asp:Label ID="lblNomeEmpresa" CssClass="label_principal" Text="Nome da Empresa"
                        runat="server" AssociatedControlID="txtNomeEmpresa"></asp:Label>
                    <div class="container_campo">
                        <componente:AlfaNumerico ID="txtNomeEmpresa" runat="server" Columns="60" MaxLength="60"
                            ValidationGroup="SalvarEmpresa" />
                    </div>
                </div>
                <div class="linha modal">
                    <asp:Label ID="lblCidade" CssClass="label_principal" Text="Cidade/UF" runat="server"
                        AssociatedControlID="txtCidade"></asp:Label>
                    <div class="container_campo">
                        <div>
                            <asp:RequiredFieldValidator ID="rfvCidade" runat="server" ControlToValidate="txtCidade"
                                ValidationGroup="SalvarEmpresa"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cvCidade" runat="server" ErrorMessage="Cidade Inválida."
                                ClientValidationFunction="cvCidade_Validate" ControlToValidate="txtCidade" ValidationGroup="SalvarEmpresa"></asp:CustomValidator>
                        </div>
                        <asp:TextBox ID="txtCidade" runat="server" CssClass="textbox_padrao campo_obrigatorio"
                            Columns="40"></asp:TextBox>
                        <AjaxToolkit:AutoCompleteExtender ID="aceCidade" runat="server" TargetControlID="txtCidade"
                            ServiceMethod="ListarCidadesSiglaEstadoPorNomeParcialEstado">
                        </AjaxToolkit:AutoCompleteExtender>
                    </div>
                </div>
                <asp:Panel CssClass="painel_botoes" runat="server">
                    <asp:Button ID="btnEnviar" runat="server" Text="Enviar" OnClick="btnEnviar_Click"
                        CssClass="botao_padrao" ValidationGroup="SalvarEmpresa" CausesValidation="true" />
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:HiddenField ID="hfVariavel" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeIndicarEmpresa" runat="server" PopupControlID="pnlIndicarEmpresa"
    TargetControlID="hfVariavel">
</AjaxToolkit:ModalPopupExtender>
