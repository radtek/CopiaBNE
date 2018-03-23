<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BuscarCEP.ascx.cs" Inherits="BNE.Web.UserControls.BuscarCEP" %>
<%@ Register Src="ucPaginacao.ascx" TagName="Paginacao" TagPrefix="Controles" %>
<script src="../js/local/UserControls/BuscarCEP.js" type="text/javascript"></script>
<asp:LinkButton ID="btnBuscarCEP" Text="<i class='fa fa-search'></i> Não sei o CEP" AlternateText="Não sei o CEP" OnClick="btnBuscarCEP_Click" CausesValidation="false" runat="server" CssClass="button_nao_sei_cep"/>
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
                            <AjaxToolkit:AutoCompleteExtender runat="server" ID="acMunicipio" TargetControlID="txtMunicipio"
                                EnableCaching="false" FirstRowSelected="true" ServiceMethod="ListarCidadesPorNomeParcialEstado"
                                CompletionListCssClass="ajax_ace cep" CompletionListHighlightedItemCssClass="selecionado"
                                CompletionListItemCssClass="item" ServicePath="~/WebServices/wsAutoComplete.asmx"
                                UseContextKey="True" OnClientItemSelected="employer.form.util.autoCompleteClientSelected" />
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
                    DataKeyNames="Num_Cep">
                    <Columns>
                        <asp:BoundField DataField="Tip_Logradouro" HeaderText="Tipo" SortExpression="Tip_Logradouro" />
                        <asp:BoundField DataField="Des_cidade" HeaderText="Cidade" SortExpression="Des_cidade" />
                        <asp:BoundField DataField="Des_Bairro" HeaderText="Bairro" SortExpression="Des_Bairro" />
                        <asp:BoundField DataField="Des_Endereco" HeaderText="Logradouro" SortExpression="Des_Endereco">
                            <ItemStyle CssClass="esquerda" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Des_Complemento" HeaderText="Faixa de Nº" SortExpression="Des_Complemento" />
                        <asp:BoundField DataField="Num_Cep" HeaderText="CEP" SortExpression="Num_Cep" />
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
