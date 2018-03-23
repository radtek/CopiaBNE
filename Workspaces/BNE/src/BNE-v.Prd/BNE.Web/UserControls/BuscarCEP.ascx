<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BuscarCEP.ascx.cs" Inherits="BNE.Web.UserControls.BuscarCEP" %>
<%@ Register Src="ucPaginacao.ascx" TagName="Paginacao" TagPrefix="Controles" %>
<script src="../js/local/UserControls/BuscarCEP.js" type="text/javascript"></script>
<asp:LinkButton ID="btnBuscarCEP" Text="<i class='fa fa-search'></i> Não sei o CEP" AlternateText="Não sei o CEP" OnClick="btnBuscarCEP_Click" 
    OnClientClick="trackEvent('Não sei o CEP','Click','Não sei o CEP'); return true;" CausesValidation="false" runat="server" CssClass="button_nao_sei_cep"/>
<asp:Panel ID="pnlModal" runat="server" DefaultButton="bntLocalizar" CssClass="modal_conteudo candidato modal_busca_cep"
    Style="display: none">
    <h2 class="titulo_modal">
        <span>Busca de CEP</span></h2>
    <asp:UpdatePanel ID="upBtiFechar" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:LinkButton CssClass="botao_fechar_modal" ID="btiFechar"
                runat="server" CausesValidation="false" OnClick="btiFechar_Click"><i class="fa fa-times-circle"></i>
 Fechar</asp:LinkButton>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlCampos" runat="server" CssClass="painel_campos">
                <table cellspacing="0">
                    <tr>
                        <td class="label_campo">
                            <asp:Label ID="lblUF" runat="server" Text="Estado" />
                        </td>
                        <td class="container_campo  abrigatorio">
                            <componente:ListaSugestoes ID="lsUF" ValidationGroup="LocalizarCep" Obrigatorio="true"
                                runat="server" MensagemErroFormato='<%$ Resources: MensagemAviso, _100002 %>'
                                Tipo="LetraMaiuscula" ValorAlteradoClient="BuscaCEP_lsUF_ValorAlterado" />
                        </td>
                    </tr>
                    <tr>
                        <td class="label_campo">
                            <asp:Label ID="lblMunicipio" runat="server" Text="Município"></asp:Label>
                        </td>
                        <td class="container_campo">
                            <asp:TextBox ID="txtMunicipio" runat="server" CssClass="textbox_padrao" />
                            <asp:CustomValidator ID="cvMunicipio" runat="server" Display="Dynamic" ControlToValidate="txtMunicipio"
                                CssClass="validator_cv" ErrorMessage="Cidade inválida." />
                        </td>
                    </tr>
                    <tr>
                        <td class="label_campo">
                            <asp:Label ID="lblBairro" runat="server" Text="Bairro" />
                        </td>
                        <td class="container_campo">
                            <componente:AlfaNumerico ID="txtBairro" runat="server" ContemIntervalo="False" Obrigatorio="False"
                                Tipo="AlfaNumerico" CssClassTextBox="textbox_padrao" MaxLength="50" />
                        </td>
                    </tr>
                    <tr>
                        <td class="label_campo">
                            <asp:Label ID="lblLogradouro" runat="server" Text="Endereço" />
                        </td>
                        <td class="container_campo">
                            <componente:AlfaNumerico ID="txtLogradouro" runat="server" ContemIntervalo="False"
                                Obrigatorio="False" Tipo="AlfaNumerico" CssClassTextBox="textbox_padrao" MaxLength="45" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <!-- Painel botoes -->
            <asp:Panel ID="pnlBotoesBuscaCEP" runat="server" CssClass="panel_botoes_secao">
                <asp:Button ID="bntLocalizar" runat="server" ValidationGroup="LocalizarCep" CausesValidation="true"
                    OnClick="bntLocalizar_Click" Text="Localizar" CssClass="button_padrao" />
                <asp:Button ID="bntCancelar" runat="server" CausesValidation="false" Text="Cancelar"
                    OnClick="bntCancelar_Click" CssClass="button_padrao_icone cancelar" />
            </asp:Panel>
            <!-- FIM: Painel botoes -->
            <div class="painel_gv_resultado_cep">
                <asp:GridView ID="gvwCEP" runat="server" AutoGenerateColumns="False" GridLines="None"
                    CellSpacing="2" CssClass="gridview_padrao" AllowSorting="True" OnRowEditing="gvwCEP_RowEditing"
                    DataKeyNames="sCep">
                    <Columns>
                        <asp:BoundField DataField="TipoLogradouro" HeaderText="Tipo" SortExpression="TipoLogradouro" />
                        <asp:BoundField DataField="cidade" HeaderText="Cidade" SortExpression="cidade" />
                        <asp:BoundField DataField="Bairro" HeaderText="Bairro" SortExpression="Bairro" />
                        <asp:BoundField DataField="Logradouro" HeaderText="Logradouro" SortExpression="Logradouro">
                            <ItemStyle CssClass="esquerda" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Complemento" HeaderText="Faixa de Nº" SortExpression="Des_Complemento" />
                        <asp:BoundField DataField="sCep" HeaderText="CEP" SortExpression="sCep" />
                        <asp:TemplateField HeaderStyle-CssClass="col_acao_02ico_16px">
                            <ItemTemplate>
                                <asp:ImageButton ID="btiEditar" runat="server" AlternateText="Selecionar" CommandName="Edit"
                                    CausesValidation="false" Enabled="true" ImageUrl="~/img/btn_selecionar_modal_cep.png" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <AlternatingRowStyle CssClass="alt" />
                </asp:GridView>
            </div>
            <Controles:Paginacao ID="PaginacaoGrid" runat="server" CssClass="paginacao_panel"
                Visible="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:Button runat="server" ID="btnAux" Style="display: none" Enabled="false" />
<AjaxToolkit:ModalPopupExtender BackgroundCssClass="uc_cep_fundo" ID="MPE" runat="server"
    PopupControlID="pnlModal" TargetControlID="btnAux" RepositionMode="RepositionOnWindowResizeAndScroll" />
