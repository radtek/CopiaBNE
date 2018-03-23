<%@ Control
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="Vagas.ascx.cs"
    Inherits="BNE.Web.UserControls.Forms.SalaAdministrador.Vagas" %>
<asp:UpdatePanel
    ID="upGvVagas"
    runat="server"
    UpdateMode="Conditional">
    <ContentTemplate>
        <div class="painel_padrao_sala_adm">
            <p>
                Encontre a
                vaga desejada
                utilizando:
                CNPJ, razão
                social, função
                ou código
                da vaga</p>
            <div>
                <asp:TextBox
                    ID="tbxFiltroBusca"
                    runat="server"
                    TextMode="SingleLine"
                    ValidationGroup="Filtrar"
                    CssClass="textbox_padrao"></asp:TextBox>
                <asp:Button
                    ID="btnFiltrar"
                    runat="server"
                    CssClass="botao_padrao filtrar"
                    Text="Pesquisar"
                    ToolTip="Filtrar Vagas"
                    CausesValidation="True"
                    ValidationGroup="Filtrar"
                    OnClick="btnFiltrar_Click" />
            </div>
            <telerik:RadGrid
                ID="gvVagas"
                AllowPaging="True"
                CssClass="gridview_padrao"
                runat="server"
                Skin="Office2007"
                GridLines="None"
                OnItemCommand="gvVagas_ItemCommand"
                OnPageIndexChanged="gvVagas_PageIndexChanged">
                <ClientSettings
                    AllowExpandCollapse="True" />
                <PagerStyle
                    Mode="NextPrevNumericAndAdvanced"
                    Position="TopAndBottom" />
                <AlternatingItemStyle
                    CssClass="alt_row" />
                <MasterTableView
                    DataKeyNames="Idf_Vaga"
                    AutoGenerateColumns="false">
                    <Columns>
                        <telerik:GridTemplateColumn
                            HeaderText="Código"
                            ItemStyle-CssClass="cod_vaga">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblCodigo"
                                    runat="server"
                                    Text='<%# Eval("Cod_Vaga") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn
                            HeaderText="Função"
                            ItemStyle-CssClass="funcao">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblFuncao"
                                    runat="server"
                                    Text='<%# Eval("Des_Funcao") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn
                            HeaderText="Nome da Empresa"
                            ItemStyle-CssClass="nome_empresa">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblNomeEmpresa"
                                    runat="server"
                                    Text='<%# Eval("Raz_Social") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn
                            HeaderText="Publicado em"
                            ItemStyle-CssClass="data_abertura">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblDta_Abertura"
                                    runat="server"
                                    Text='<%# Eval("Dta_Abertura") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn
                            HeaderText="Cadastrado em"
                            ItemStyle-CssClass="data_cadastro">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblDtaCadastro"
                                    runat="server"
                                    Text='<%# Eval("Dta_Cadastro") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn
                            HeaderText="Ações"
                            ItemStyle-CssClass="col_action">
                            <ItemTemplate>
                                <asp:ImageButton
                                    ID="btiVisualizarCurriculo"
                                    Enabled='<%# Convert.ToBoolean(Eval("Flg_Auditada")) %>'
                                    ToolTip='<%# !Convert.ToBoolean(Eval("Flg_Auditada")) ? "Vaga pendente de auditoria" : "Visualizar Vaga" %>'
                                    runat="server"
                                    ImageUrl="../../../img/icn_binoculo.png"
                                    AlternateText="Visualizar Vaga"
                                    CausesValidation="false"
                                    CommandName="VisualizarVaga" />
                                <asp:ImageButton
                                    ID="btiEditarVaga"
                                    runat="server"
                                    ImageUrl="../../../img/icn_editar_lapis.png"
                                    ToolTip="Editar Vaga"
                                    AlternateText="Editar Vaga"
                                    CausesValidation="false"
                                    CommandName="EditarVaga" />
                                <asp:ImageButton
                                    ID="btiExcluirVaga"
                                    
                                    runat="server"
                                    ImageUrl="../../../img/icn_excluirvaga.png"
                                    ToolTip="Excluir Vaga"
                                    AlternateText="Excluir Vaga"
                                    CausesValidation="false"
                                    CommandName="ExcluirVaga" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NoRecordsTemplate>
                        <div class="mensagem_nodata">
                            Nenhum item
                            para mostrar.</div>
                    </NoRecordsTemplate>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
        <%-- Botões --%>
        <asp:Panel
            ID="pnlBotoes"
            runat="server"
            CssClass="painel_botoes">
            <asp:Button
                ID="btnVoltar"
                runat="server"
                CssClass="botao_padrao"
                Text="Voltar"
                CausesValidation="false"
                OnClick="btnVoltar_Click" />
        </asp:Panel>
        <%-- Fim Botões --%>
    </ContentTemplate>
</asp:UpdatePanel>
