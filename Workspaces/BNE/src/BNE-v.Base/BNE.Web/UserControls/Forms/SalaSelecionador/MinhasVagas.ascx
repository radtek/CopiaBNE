<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MinhasVagas.ascx.cs"
    Inherits="BNE.Web.UserControls.Forms.SalaSelecionador.MinhasVagas" %>
<%@ Register Src="../../Modais/ModalConfirmacao.ascx" TagName="ModalConfirmacao"
    TagPrefix="uc1" %>
<Employer:DynamicScript runat="server" Src="/js/local/Forms/SalaSelecionador/MinhasVagas.js" type="text/javascript" />
<asp:UpdatePanel ID="upGvVagas" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="painel_padrao_sala_selecionador">

            <div class="jumbotron" contenteditable="false">
                <asp:Button ID="Button2" runat="server" CssClass="botao_padrao verde" Text="Nova Vaga >>"
                    CausesValidation="false" OnClick="btnNovaVaga_Click" />
                Anuncie e receba candidato no
                <asp:Label ID="Label1" CssClass="destaque_texto" runat="server">perfil</asp:Label>
                e
                <asp:Label ID="Label2" CssClass="destaque_texto" runat="server">interessados</asp:Label>
                na sua oportunidade.
            </div>
            <p>

                <span class="formataParagrafo"></span>


                Você possui
                <asp:Label ID="lblInformacaoVagas" CssClass="destaque_texto" runat="server"></asp:Label>
                no momento.
                
            </p>
            <div>
                <asp:UpdatePanel ID="upStatusFuncao" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblStatus" runat="server" Text="Status" AssociatedControlID="rcbStatus"></asp:Label>
                        <div class="container_campo">
                            <telerik:RadComboBox ID="rcbStatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rcbStatus_SelectedIndexChanged">
                            </telerik:RadComboBox>
                        </div>
                        <asp:Label ID="lblFuncao" runat="server" Text="Função" AssociatedControlID="ccFiltrarVagasFuncao"></asp:Label>
                        <div class="container_campo">
                            <Employer:ComboCheckbox runat="server" ID="ccFiltrarVagasFuncao" EmptyMessage="Filtrar vagas por função"
                                AllowCustomText="false" CssClass="checkbox_large" DropDownWidth="350">
                            </Employer:ComboCheckbox>
                        </div>
                        <asp:Label ID="lblMinhasVagas" runat="server" Text="Somente minhas vagas?" AssociatedControlID="ccFiltrarApenasMinhasVagas"></asp:Label>
                        <div class="container_campo">
                            <asp:CheckBox runat="server" ID="ccFiltrarApenasMinhasVagas" AllowCustomText="false"></asp:CheckBox>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Button ID="btnFiltrar" runat="server" CssClass="botao_padrao filtrar" Text="Filtrar Vagas"
                    OnClick="btnFiltrarVaga_Click" ToolTip="Filtrar Vagas" CausesValidation="false" />
            </div>
            <asp:UpdatePanel ID="upMinhasVagas" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <telerik:RadGrid ID="gvVagas" AllowPaging="True" AllowCustomPaging="true" CssClass="gridview_padrao"
                        runat="server" Skin="Office2007" GridLines="None" OnItemCommand="gvVagas_ItemCommand"
                        OnPageIndexChanged="gvVagas_PageIndexChanged" OnItemDataBound="gvVagas_ItemDataBound"
                        OnColumnCreated="gvVagas_ColumnCreated" OnItemCreated="gvVagas_ItemCreated">
                        <ClientSettings AllowExpandCollapse="False" AllowGroupExpandCollapse="False">
                        </ClientSettings>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" Position="TopAndBottom" />
                        <AlternatingItemStyle CssClass="alt_row" />
                        <MasterTableView DataKeyNames="Idf_Vaga">
                            <GroupByExpressions>
                                <telerik:GridGroupByExpression>
                                    <SelectFields>
                                        <telerik:GridGroupByField FieldAlias="FlgVagaArquivada" FieldName="Flg_Vaga_Arquivada"></telerik:GridGroupByField>
                                    </SelectFields>
                                    <GroupByFields>
                                        <telerik:GridGroupByField FieldName="Flg_Vaga_Arquivada" SortOrder="Ascending"></telerik:GridGroupByField>
                                    </GroupByFields>
                                </telerik:GridGroupByExpression>
                            </GroupByExpressions>
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Código" ItemStyle-CssClass="codigo">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIdVaga" runat="server" Text='<%# Eval("Cod_Vaga") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Vagas" ItemStyle-CssClass="descricao_vaga2">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescricaoVaga" runat="server" Text='<%# Eval("Des_Funcao") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Cidade" ItemStyle-CssClass="cidade">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNmeVaga" runat="server" Text='<%# Eval("Nme_Cidade") + "/" + Eval("Sig_Estado") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Candidatos Inscritos" ItemStyle-CssClass="cv_recebida centro">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkCvsRecebidos" runat="server" Text='<%# Eval("Num_Cvs_Recebidos") %>' CausesValidation="false"
                                            CommandName="VisualizarCurriculo"></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Inscritos não Lidos" ItemStyle-CssClass="cv_recebida centro">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkCvsNaoLidos" runat="server" Text='<%# Eval("Num_Cvs_Recebidos_Nao_Lidos") %>' CausesValidation="false" CommandName="VisualizarCurriculosNaoLidos"></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Banco de Currículos" ItemStyle-CssClass="cv_recebida centro">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkCvsPerfil" runat="server" Text='<%# Eval("Qtd_Curriculos_Perfil") %>' CausesValidation="false" CommandName="VisualizarCurriculosNoPerfil"></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Ações" ItemStyle-CssClass="col_action">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btiEditarVaga" runat="server"
                                            ToolTip="Editar Vaga" CausesValidation="false" CommandName="EditarVaga"><i class="fa fa-pencil-square-o"></i></asp:LinkButton>
                                        <asp:LinkButton ID="btlClonarVaga" runat="server" ToolTip="Clonar Vaga" CausesValidation="false" CommandName="ClonarVaga"><i class="fa fa-files-o"></i></asp:LinkButton>
                                        <asp:LinkButton ID="btiArquivarVaga" runat="server"
                                            ToolTip="Inativar Vaga" CausesValidation="false"
                                            CommandName="ArquivarVaga" Visible='<%# Convert.ToBoolean(Eval("Flg_Vaga_Arquivada")).Equals(false) %>'><i class="fa fa-times-circle"></i></asp:LinkButton>
                                        <asp:ImageButton ID="btiAnuncioMassa" runat="server" ImageUrl="../../../img/icn_anunciomassa.png"
                                            ToolTip="Anúncio em Massa" AlternateText="Anúncio em Massa" CausesValidation="false"
                                            CommandName="AnuncioMassa" Visible='<%# Convert.ToBoolean(Eval("Flg_Vaga_Arquivada")).Equals(false) %>' Style="display: none;" />
                                        <asp:LinkButton ID="btiAtivarVaga" runat="server"
                                            ToolTip="Ativar Vaga" AlternateText="Ativar Vaga" CommandName="AtivarVaga" Visible='<%# Convert.ToBoolean(Eval("Flg_Vaga_Arquivada")).Equals(true) %>'><i class="fa fa-check-square"></i></asp:LinkButton>
                                        <asp:LinkButton ID="btiExcluirVaga" runat="server"
                                            ToolTip="Excluir Vaga" CausesValidation="false"
                                            CommandName="ExcluirVaga"><i class="fa fa-trash"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle ShowPagerText="true" Position="TopAndBottom" PagerTextFormat=" {4} Vagas {2} a {3} de {5}" />
                            <NoRecordsTemplate>
                                <div class="mensagem_nodata">
                                    Nenhum item para mostrar.
                                </div>
                            </NoRecordsTemplate>
                        </MasterTableView>
                    </telerik:RadGrid>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<uc1:ModalConfirmacao ID="ucModalConfirmacao" runat="server" />
