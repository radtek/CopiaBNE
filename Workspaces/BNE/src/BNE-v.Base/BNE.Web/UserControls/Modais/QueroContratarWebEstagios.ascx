<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QueroContratarWebEstagios.ascx.cs" Inherits="BNE.Web.UserControls.Modais.QueroContratarWebEstagios" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/QueroContratarEstagiario.css" type="text/css" rel="stylesheet" />
<Employer:DynamicScript runat="server" Src="/js/local/QueroContratarEstagiario.js" type="text/javascript" />
<asp:Panel runat="server" ID="pnlPrincipal" CssClass="modal_quero_contratar_estagiario" Style="display: none">
    <asp:UpdatePanel ID="upBtiFechar" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:ImageButton CssClass="botaoNovo_fechar_modal" ID="btiFechar" ImageUrl="/img/modal_nova/btnRedFecharModal.png"
                runat="server" CausesValidation="false" OnClick="btiFechar_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="upEnvioContratacao" CssClass="containerConteudoModal" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlContratacao">
                <h2>Dados de Contratação</h2>
                <p class="texto_marcadores_obrigatorio confirmar_contratacao">
                    Os campos marcados com um
        <asp:Image runat="server" ID="imgObrigatorio" ImageUrl="~/img/icone_obrigatorio.gif" />
                    são obrigatórios para o cadastro de seu currículo.
                </p>


                <div class="linha">
                    <h3>Estagiário</h3>
                </div>
                <div class="linha">
                    <asp:Label ID="Label1" runat="server" CssClass="label_quero_contratar" for="txtNomeDaMae" Text="Nome da Mãe:"></asp:Label>
                    <div class="confirmar_contratacao_texto">
                        <asp:RequiredFieldValidator Display="Dynamic" runat="server" ValidationGroup="ConfirmarContratacao" ID="rvNomeDaMae" ControlToValidate="txtNomeDaMae" CssClass="validador"></asp:RequiredFieldValidator>
                        <asp:CustomValidator runat="server" Display="Dynamic" ID="cvValidacaoNome" ErrorMessage="Por favor digite o nome e sobrenome sem abreviações!" ValidationGroup="ConfirmarContratacao" ControlToValidate="txtNomeDaMae" CssClass="validador" ClientValidationFunction="ValidarNome"></asp:CustomValidator>
                    </div>
                    <div class="container_campo">
                        <asp:TextBox runat="server" ID="txtNomeDaMae" CssClass="textbox_padrao campo_obrigatorio"></asp:TextBox>
                    </div>
                </div>
                <div class="linha">
                    <h3>Bolsa</h3>
                </div>
                <div class="linha">
                    <asp:Label ID="lblValor" runat="server" CssClass="label_quero_contratar" Text="Tipo:"></asp:Label>
                    <div class="confirmar_contratacao_texto">
                        <asp:RequiredFieldValidator Display="Dynamic" runat="server" ValidationGroup="ConfirmarContratacao" ID="rfTipoBolsa" ControlToValidate="rdlValorList" CssClass="validador"></asp:RequiredFieldValidator>
                    </div>
                    <div class="container_campo_centralizado radio_bolsa">
                        <div class="radio_inner_bolsa_esquerda">
                            <asp:Image runat="server" ID="imgTipoBolsa" ImageUrl="~/img/icone_obrigatorio.gif" Width="13px" />
                        </div>
                        <div class="radio_inner_bolsa_direita">
                            <asp:RadioButtonList runat="server" ID="rdlValorList" CssClass="radio_inner_bolsa_esquerda" RepeatColumns="2" RepeatDirection="Horizontal">
                                <asp:ListItem Value="Fixo" Text="Valor Fixo"></asp:ListItem>
                                <asp:ListItem Value="PorHora" Text="Valor por Hora"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                </div>
                <div class="linha">
                    <asp:Label ID="Label3" runat="server" CssClass="label_quero_contratar" for="txtValorBolsa" Text="Valor (R$):"></asp:Label>
                    <div class="container_campo">
                        <componente:AlfaNumerico ID="txtValorBolsa"
                            runat="server"
                            ExpressaoValidacao='<%# BNE.Web.Resources.Configuracao.RegexValor  %>'
                            Obrigatorio="True"
                            MaxLength="6"
                            ValidationGroup="ConfirmarContratacao"
                            MensagemErroFormato='<%$ Resources: MensagemAviso, _505702 %>'
                            MensagemErroIntervalo='<%$ Resources: MensagemAviso, _505702 %>'
                            MensagemErroValor='<%$ Resources: MensagemAviso, _505702 %>'
                            Width="108px" />
                    </div>
                </div>
                <div class="linha">
                    <asp:Label ID="Label2" runat="server" CssClass="label_quero_contratar" Text="Benefícios:"></asp:Label>
                    <div class="container_campo">
                        <asp:TextBox ID="txtBeneficios" CssClass="textbox_padrao contratar_estag_beneficios" runat="server"></asp:TextBox>
                        <span class="span_exemplo_beneficios">(Exemplo: VT + VR)</span>
                    </div>
                </div>

                <br />
                <div class="linha">
                    <div class="container_campo_centralizado botao_contratar_confirmar">
                        <asp:Button runat="server" ID="btnConfirmar" CssClass="botao_padrao" Text="Confirmar" OnClick="btnConfirmar_OnClick" ValidationGroup="ConfirmarContratacao" CausesValidation="True" />
                    </div>
                </div>
            </asp:Panel>

            <asp:Panel runat="server" ID="pnlEnviado" Visible="False">
                <div id="divImgConfirm" runat="server" class="imagem_confirmar_contratacao">
                    <asp:Image ID="imgConfirm" runat="server" BorderWidth="0px" ImageUrl="~/img/modal_nova/imgSucesso.png" />
                </div>
                <p class="texto_cadastro_sucesso_envio_email">
                    <asp:Label ID="lblSucesso" runat="server" Text="Os dados da contratação foram enviados com sucesso!"></asp:Label>
                </p>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:HiddenField ID="hfVariavel" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeEnvioEmail" PopupControlID="pnlPrincipal"
    TargetControlID="hfVariavel" runat="server">
</AjaxToolkit:ModalPopupExtender>