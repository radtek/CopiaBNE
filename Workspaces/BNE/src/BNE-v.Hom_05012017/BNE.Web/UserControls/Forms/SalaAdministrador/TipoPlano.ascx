<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TipoPlano.ascx.cs" Inherits="BNE.Web.UserControls.Forms.SalaAdministrador.TipoPlano" %>
<%@ Register Src="../../Modais/ConfirmacaoExclusao.ascx" TagName="ConfirmacaoExclusao" TagPrefix="uc1" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/TipoPlano.css" type="text/css" rel="stylesheet" />
<h2>Tipo de Plano</h2>
<asp:UpdatePanel ID="upCadastroPlano" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="pnlCadastroPlano" runat="server" CssClass="blocodados">
            <h3>Incluir Tipo de Plano</h3>
            <%--Linha Tipo de Cliente --%>
            <div class="linha">
                <asp:Label ID="lblTipoCliente" runat="server" Text="Tipo de Cliente" CssClass="label_principal" />
                <div class="container_campo">
                    <telerik:RadComboBox runat="server" ID="rcbFiltrarTipoCliente" EmptyMessage=" " AllowCustomText="false" CssClass="checkbox_large" AutoPostBack="True" OnSelectedIndexChanged="rcbFiltrarTipoCliente_SelectedIndexChanged">
                    </telerik:RadComboBox>
                </div>
            </div>
            <%--FIM Tipo de Cliente --%>
            <%--Linha Nome do Plano --%>
            <div class="linha">
                <asp:Label ID="lblNomePlano" runat="server" Text="Nome do Plano" CssClass="label_principal" AssociatedControlID="txtNomePlano" />
                <div class="container_campo">
                    <componente:AlfaNumerico ID="txtNomePlano" runat="server" ValidationGroup="Salvar" MaxLength="100" Columns="20" />
                </div>
            </div>
            <%--FIM Nome do Plano --%>
            <%--Linha Duração do PLano--%>
            <div class="linha">
                <asp:Label ID="lblDuracaoPlano" runat="server" Text="Duração do Plano" CssClass="label_principal" AssociatedControlID="txtTempoAcesso" />
                <div class="container_campo">
                    <componente:AlfaNumerico ID="txtTempoAcesso" runat="server" ValidationGroup="Salvar" MaxLength="4" Tipo="Numerico" />
                </div>
            </div>
            <%--FIM Duração do PLano--%>
            <%--Linha Quantidade de Visualizações--%>
            <div class="linha">
                <asp:Label ID="lblQuantidadeVisualizacoes" runat="server" Text="Quantidade de Visualizações" CssClass="label_principal" AssociatedControlID="txtQuantidadeVisualizacoes" />
                <div class="container_campo">
                    <componente:AlfaNumerico ID="txtQuantidadeVisualizacoes" runat="server" ValidationGroup="Salvar" MaxLength="4" Tipo="Numerico" />
                </div>
            </div>
            <%--FIM Quantidade de Visualizações --%>
            <%--Linha Quantidade de SMS--%>
            <div class="linha">
                <asp:Label ID="lblQuantidadeSMS" runat="server" Text="Quantidade de SMS" CssClass="label_principal" AssociatedControlID="txtQuantidadeSMS" />
                <div class="container_campo">
                    <componente:AlfaNumerico ID="txtQuantidadeSMS" runat="server" ValidationGroup="Salvar" MaxLength="4" Tipo="Numerico" />
                </div>
            </div>
            <%--FIM Quantidade de SMS --%>
            <%--Linha Quantidade de Campanha--%>
            <div class="linha">
                <asp:Label ID="lblQuantidadeCampanha" runat="server" Text="Quantidade de Campanha" CssClass="label_principal" AssociatedControlID="txtQuantidadeCampanha" />
                <div class="container_campo">
                    <componente:AlfaNumerico ID="txtQuantidadeCampanha" runat="server" ValidationGroup="Salvar" MaxLength="4" Tipo="Numerico" />
                </div>
            </div>
            <%--FIM Quantidade de Campanha --%>
            <%--Linha Valor--%>
            <div class="linha">
                <asp:Label ID="lblValor" runat="server" Text="Valor" CssClass="label_principal" AssociatedControlID="txtValor" />
                <div class="container_campo">
                    <componente:ValorDecimal ID="txtValor" runat="server" ValidationGroup="Salvar" CasasDecimais="2" ValorMaximo="99999" />
                </div>
            </div>
            <%--FIM Valor--%>
            <%--Linha Desconto Máximo --%>
            <div class="linha">
                <asp:Label ID="lblDescontoMaximo" runat="server" Text="Desconto Máximo " CssClass="label_principal" />
                <div class="container_campo small">
                    <componente:AlfaNumerico ID="txtDesconto" runat="server" ValidationGroup="Salvar" MaxLength="4" Tipo="Numerico" />
                    <span class="observacao">%</span>
                </div>
            </div>
            <%--FIM Desconto Máximo --%>
            <%--Linha Número de Parcelas --%>
            <div class="linha">
                <asp:Label ID="lblNumeroParcelas" runat="server" Text="Número de Parcelas " CssClass="label_principal" AssociatedControlID="txtNumeroParcelas" />
                <div class="container_campo small">
                    <componente:AlfaNumerico ID="txtNumeroParcelas" runat="server" ValidationGroup="Salvar" MaxLength="2" Tipo="Numerico" />
                </div>
            </div>
            <%--FIM Número de Parcelas --%>
            <%--Linha Envia Contrato --%>
            <div class="linha">
                <asp:Label ID="lblEnviaContrato" runat="server" Text="Envia Contrato" CssClass="label_principal" />
                <div class="container_campo">
                    <asp:CheckBox ID="chkEnviaContrato" runat="server" Text="Envia Contrato" />
                </div>
            </div>
            <%--FIM TEnvia Contrato --%>
            <%--Linha Tipo de Cliente --%>
            <div class="linha">
                <asp:Label ID="lblTipoContrato" runat="server" Text="Tipo de Contrato" CssClass="label_principal" />
                <div class="container_campo">
                    <telerik:RadComboBox runat="server" ID="rcbTipoContrato" EmptyMessage=" " AllowCustomText="false" CssClass="checkbox_large">
                    </telerik:RadComboBox>
                </div>
            </div>
            <%--FIM Tipo de Cliente --%>
            <%--Linha Envia Contrato --%>
            <div class="linha">
                <asp:Label ID="lblHabilitaVendaPersonalizada" runat="server" Text="Habilita Venda Personalizada" CssClass="label_principal" />
                <div class="container_campo">
                    <asp:CheckBox ID="chkHabilitaVendaPersonalizada" runat="server" Text="Habilita Venda Personalizada" />
                </div>
            </div>
            <%--FIM TEnvia Contrato --%>
            <%--Linha Libera no tanque --%>
            <div class="linha">
                <asp:Label ID="lblLiberaUsuarioTanque" runat="server" Text="Habilita Uso do Tanque" CssClass="label_principal" />
                <div class="container_campo">
                    <asp:CheckBox ID="ckbLiberaUsuarioTanque" runat="server" Text="Habilita Uso do Tanque" />
                </div>
            </div>
            <%--FIM TEnvia Contrato --%>
            <%--Linha Pagamento Recorrente --%>
            <div class="linha">
                <asp:Label ID="Label1" runat="server" Text="Pagamento" CssClass="label_principal" />
                <div class="container_campo">
                    <asp:CheckBox ID="ckbPagamentoRecorrenteMensal" runat="server" Text="Recorrente Mensal (cobrança efetuada mensalmente e automaticamente na data de vencimento do Plano)" />
                </div>
                <Componentes:BalaoSaibaMais ID="bmsPeriodoPagamento" runat="server" ToolTipText="O Plano com Pagamento Recorrente será cobrado mensalmente no vencimento do plano. Meios de pagamento disponíveis para esse tipo: Cartão de Crédito e Débito Hsbc." Text="Saiba mais" ToolTipTitle="Período de Pagamento" CssClassLabel="balao_saiba_mais" ShowOnMouseover="true" />
            </div>
            <%--FIM Pagamento Recorrente --%>
        </asp:Panel>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnSalvar" EventName="Click" />
    </Triggers>
</asp:UpdatePanel>
<%-- Botões --%>
<asp:Panel ID="Panel1" runat="server" CssClass="painel_botoes">
    <asp:Button ID="btnSalvar" runat="server" CssClass="botao_padrao" Text="Salvar" ValidationGroup="Salvar" CausesValidation="True" OnClick="btnSalvar_Click" />
    <asp:Button ID="btnCancelar" runat="server" CssClass="botao_padrao" Text="Limpar" CausesValidation="false" OnClick="btnCancelar_Click" />
</asp:Panel>
<%-- Fim Botões --%>
<h3>Planos Cadastrados</h3>
<%--Linha Status Planos --%>
<asp:UpdatePanel runat="server" ID="upRdStatusPlanos" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel runat="server" ID="pnlStatusPlanos">
            <div class="linha">
                <asp:Label ID="lblStatusPlanos" runat="server" Text="Status" CssClass="label_principal label_principalStatusPlano" />
                <div class="container_campo">
                    <asp:RadioButtonList runat="server" ID="rdLstStatusPlanos" RepeatDirection="Horizontal">
                        <asp:ListItem Value="0" Selected="True">Ativos</asp:ListItem>
                        <asp:ListItem Value="1">Inativos</asp:ListItem>
                        <asp:ListItem Value="2">Todos</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
                <div class="container_campo">
                    <div class="container_campo">
                        <componente:AlfaNumerico ID="filtroDePlanos" runat="server" Height="10px" MaxLength="100" Columns="20" MinLength="2" MensagemErroValor="Campo deve conter no mínimo 2 caracteres" Obrigatorio="false"/>
                    </div>
                    <asp:Button ID="btnFiltrar" runat="server" CssClass="botao_padrao filtrar" Text="Pesquisar" ToolTip="Filtrar" OnClick="btnFiltrarStatus_Click" />
                </div>

            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
<%--FIM Status Planos --%>
<asp:UpdatePanel ID="upGvPlanos" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <telerik:RadGrid ID="gvPlanos" AllowPaging="True" AllowCustomPaging="true" CssClass="gridview_padrao" runat="server" Skin="Office2007" GridLines="None" OnPageIndexChanged="gvPlanos_PageIndexChanged" OnItemCommand="gvPlanos_ItemCommand">
            <PagerStyle Mode="NextPrevNumericAndAdvanced" Position="TopAndBottom" />
            <AlternatingItemStyle CssClass="alt_row" />
            <MasterTableView DataKeyNames="Idf_Plano">
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Nome do Plano" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblNomePlano" runat="server" Text='<%# Eval("Des_Plano") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Validade" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblDtaValidade" runat="server" Text='<%# Eval("Qtd_Dias_Validade") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Valor" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblValor" runat="server" Text='<%# Eval("Vlr_Base") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Ações" ItemStyle-CssClass="col_action tab_tipoplano">
                        <ItemTemplate>
                            <asp:ImageButton ID="btiEditar" runat="server" ImageUrl="../../../img/icn_editar_lapis.png" ToolTip="Editar" AlternateText="Editar" CommandName="Editar" Visible='<%# !Convert.ToBoolean(Eval("Flg_Inativo")) %>' CausesValidation="false" />
                            <asp:ImageButton ID="btiExcluir" runat="server" ImageUrl="../../../img/icn_excluirvaga.png" ToolTip="Excluir" AlternateText="Excluir" CommandName="Excluir" Visible='<%# !Convert.ToBoolean(Eval("Flg_Inativo")) %>' CausesValidation="false" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
    </ContentTemplate>
</asp:UpdatePanel>
<%--Modal de confirmação de exclusão--%>
<uc1:ConfirmacaoExclusao ID="ucConfirmacaoExclusao" runat="server" />
