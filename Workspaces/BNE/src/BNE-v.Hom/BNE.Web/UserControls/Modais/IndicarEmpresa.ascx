<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IndicarEmpresa.ascx.cs"
    Inherits="BNE.Web.UserControls.Modais.IndicarEmpresa" %>
<Employer:DynamicScript runat="server" Src="/js/local/UserControls/IndicarEmpresa.js" type="text/javascript" />
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/ModalConfirmacaoEnvioCurriculo.css" type="text/css" rel="stylesheet" />
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/IndicarEmpresa.css" type="text/css" rel="stylesheet" />
<asp:Panel ID="pnlIndicarEmpresa" CssClass="modal_indicar_empresa candidato reduzida"
    Style="display: none; height: auto !important;" runat="server">
    <h2 class="titulo_modal_nova">
        <span>Indicação de Empresa </span>
    </h2>
    <asp:UpdatePanel ID="upBtiFechar" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:ImageButton CssClass="modal_fechar" ID="btiFechar" ImageUrl="/img/modal_nova/btn_amarelo_fechar_modal.png"
                runat="server" CausesValidation="false" OnClick="btiFechar_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upPainelBotoes" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <asp:Panel runat="server" CssClass="panel_padrao modal_corpo">
                <p class="textocomplementar">
                    Trabalhe onde quiser! O BNE ajudará você.
                </p>
                <p class="textoform">
                    Preencha os campos abaixo:
                </p>
                <table>
                    <tr>

                        <div class="linha ">
                            <td>
                                <asp:Label ID="lblNomeEmpresa" CssClass="label_principal" Text="Nome da Empresa"
                                    runat="server" AssociatedControlID="txtNomeEmpresa"></asp:Label>
                            </td>
                            <td>
                                <div class="container_campo">
                                    <componente:AlfaNumerico ID="txtNomeEmpresa" runat="server" Columns="60" MaxLength="60" Width="343px"
                                        ValidationGroup="SalvarEmpresa" />
                                </div>
                            </td>
                        </div>

                    </tr>
                    <tr>
                        <div class="linha ">
                            <td>
                                <asp:Label ID="lblCidade" CssClass="label_principal" Text="Cidade/UF" runat="server"
                                    AssociatedControlID="txtCidade"></asp:Label>
                            </td>
                            <td>
                                <div class="container_campo">
                                    <div>
                                        <asp:RequiredFieldValidator ID="rfvCidade" runat="server" ControlToValidate="txtCidade"
                                            ValidationGroup="SalvarEmpresa"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvCidade" runat="server" ErrorMessage="Cidade Inválida."
                                            ClientValidationFunction="cvCidade_Validate" ControlToValidate="txtCidade" ValidationGroup="SalvarEmpresa"></asp:CustomValidator>
                                    </div>
                                    <asp:TextBox ID="txtCidade" runat="server" Width="343px" CssClass="textbox_padrao campo_obrigatorio"
                                        Columns="40"></asp:TextBox>
                                </div>
                            </td>
                        </div>
                    </tr>
                </table>
                <asp:Panel CssClass="painel_botoes" runat="server">
                    <div class="btnVoltar">
                        <asp:Button ID="btnEnviar" runat="server" Text="Enviar" OnClick="btnEnviar_Click"
                            ValidationGroup="SalvarEmpresa" CausesValidation="true" />
                    </div>
                </asp:Panel>

            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:HiddenField ID="hfVariavel" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeIndicarEmpresa" runat="server" PopupControlID="pnlIndicarEmpresa"
    TargetControlID="hfVariavel">
</AjaxToolkit:ModalPopupExtender>
