<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MinhasVagas.ascx.cs"
    Inherits="BNE.Web.UserControls.Forms.SalaSelecionador.MinhasVagas" %>
<%@ Register Src="../../Modais/ModalConfirmacao.ascx" TagName="ModalConfirmacao"
    TagPrefix="uc1" %>
<Employer:DynamicScript runat="server" Src="/js/local/Forms/SalaSelecionador/MinhasVagas.js" type="text/javascript" />

<script src="../js/local/UserControls/MinhasVagas.js" type="text/javascript"></script>


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
            <asp:UpdatePanel ID="upInformacaoVagas" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                <ContentTemplate>
                    <asp:Label ID="lblInformacaoVagas" CssClass="destaque_texto" runat="server"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
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
                    <Employer:ComboCheckbox runat="server" Filter="Contains" ID="ccFiltrarVagasFuncao"  EmptyMessage=""
                         CssClass="checkbox_large" DropDownWidth="340">
                    </Employer:ComboCheckbox>
                </div>
                <asp:Panel ID="pnlMinhasVagasFiltro" runat="server">
                    <asp:Label ID="lblMinhasVagas" runat="server" Text="Somente minhas vagas?" AssociatedControlID="ccFiltrarApenasMinhasVagas"></asp:Label>
                    <div class="container_campo">
                        <asp:CheckBox runat="server" ID="ccFiltrarApenasMinhasVagas" AllowCustomText="false"></asp:CheckBox>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlAnunciantes" Visible="false" runat="server">
                    <asp:Label ID="lblAnunciante" runat="server" Text="Anunciante" AssociatedControlID="ccbAnunciante"></asp:Label>
                    <div class="container_campo">
                        <Employer:ComboCheckbox ID="ccbAnunciante" EmptyMessage="Filtrar por anunciante"
                            runat="server" CssClass="" >
                        </Employer:ComboCheckbox>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Button ID="btnFiltrar" runat="server" CssClass="botao_padrao filtrar" Text="Filtrar Vagas"
            OnClick="btnFiltrarVaga_Click" ToolTip="Filtrar Vagas" CausesValidation="false" />
    </div>

    <h2>Vagas Anunciadas</h2>
    <asp:UpdatePanel ID="upGvVagas" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <telerik:RadGrid ID="gvVagas" AllowPaging="True" AllowCustomPaging="true" CssClass="gridview_padrao"
                runat="server" Skin="Office2007" GridLines="None" OnItemCommand="gvVagas_ItemCommand"
                OnPageIndexChanged="gvVagas_PageIndexChanged" OnItemDataBound="gvVagas_ItemDataBound"
                OnColumnCreated="gvVagas_ColumnCreated" OnItemCreated="gvVagas_ItemCreated">
                <ClientSettings AllowExpandCollapse="False" AllowGroupExpandCollapse="False">
                </ClientSettings>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" Position="TopAndBottom" />
                <AlternatingItemStyle CssClass="alt_row" />
                <MasterTableView DataKeyNames="Idf_Vaga, Des_Funcao, Nme_Cidade, Sig_Estado">
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
                                <p>
                                    <a class="tooltipB balao"><span>
                                        <b>Salário: </b><%# Eval("Vlr_Salario_De") != DBNull.Value && Eval("Vlr_Salario_Para") != DBNull.Value ? "R$ "+Convert.ToDecimal(Eval("Vlr_Salario_De")).ToString("N2") +" á R$ "+ Convert.ToDecimal(Eval("Vlr_Salario_Para")).ToString("N2"): 
                                Eval("Vlr_Salario_De") != DBNull.Value ?"R$ " + Convert.ToDecimal(Eval("Vlr_Salario_De")).ToString("N2") :
                                Eval("Vlr_Salario_Para") != DBNull.Value ?"R$ " + Convert.ToDecimal(Eval("Vlr_Salario_para")).ToString("N2") :
                            " a combinar" %><br />
                                        <b>Descrição: </b><%# Eval("Des_Atribuicoes") %>
                                        <%# !String.IsNullOrEmpty(Eval("Perguntas").ToString()) ? String.Format("<br><b>Perguntas:</b> {0}  ", Eval("Perguntas")) : String.Empty %>
                                    </span>&nbsp;<%# Eval("Flg_Deficiencia").ToString().Trim() == "True" ? "<i class='fa fa-wheelchair fa-1x ico_pcd'></i>"+ Eval("Des_Funcao") : Eval("Des_Funcao") %>&nbsp; </a>
                                </p>
                                <%--<asp:Label ID="lblDescricaoVaga" runat="server"><%# Eval("Flg_Deficiencia").ToString().Trim() == "True" ? "<i class='fa fa-wheelchair fa-1x ico_pcd'></i>"+ Eval("Des_Funcao") : Eval("Des_Funcao") %></asp:Label>--%>
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
                                <asp:LinkButton ID="lnkCvsPerfil" runat="server" Text='<%# RetornarBancoCurriculos(Convert.ToInt32(Eval("Idf_Vaga"))) %>' CausesValidation="false" CommandName="VisualizarCurriculosNoPerfil"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ações" ItemStyle-CssClass="col_action">
                            <ItemTemplate>
                                <a href="<%#UrlVaga + Eval("Idf_Vaga")%>" target="_blank" title="Visualizar vaga" ><i class="fa fa-search"></i></a>
                                <asp:LinkButton ID="btiEditarVaga" runat="server"
                                    ToolTip="Editar Vaga" CausesValidation="false" CommandName="EditarVaga"><i class="fa fa-pencil-square-o"></i></asp:LinkButton>
                                <asp:LinkButton ID="btiPublicarImediato" runat="server"
                                    ToolTip="Publicação Imediata de Vaga" AlternateText="Ativar Vaga" CommandName="PublicacaoImediataVaga" Visible='<%# Convert.ToDateTime(Eval("Dta_Abertura")) > DateTime.Today.AddDays(1) %>'><i class="fa fa-money"></i></asp:LinkButton>
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
    <h2>Vagas de Campanhas</h2>
    <asp:UpdatePanel ID="upGvVagasCampanha" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <telerik:RadGrid ID="gvVagasCampanha" AllowPaging="True" AllowCustomPaging="true" CssClass="gridview_padrao"
                runat="server" Skin="Office2007" GridLines="None" OnItemCommand="gvVagasCampanha_ItemCommand"
                OnPageIndexChanged="gvVagasCampanha_PageIndexChanged" OnItemDataBound="gvVagasCampanha_ItemDataBound"
                OnColumnCreated="gvVagasCampanha_ColumnCreated" OnItemCreated="gvVagasCampanha_ItemCreated">
                <ClientSettings AllowExpandCollapse="False" AllowGroupExpandCollapse="False">
                </ClientSettings>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" Position="TopAndBottom" />
                <AlternatingItemStyle CssClass="alt_row" />
                <MasterTableView DataKeyNames="Idf_Vaga, Des_Funcao, Nme_Cidade, Sig_Estado">
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
                                <p>
                                    <a class="tooltipB balao"><span>
                                        <b>Salário: </b><%# Eval("Vlr_Salario_De") != DBNull.Value && Eval("Vlr_Salario_Para") != DBNull.Value ? "R$ "+Convert.ToDecimal(Eval("Vlr_Salario_De")).ToString("N2") +" á R$ "+ Convert.ToDecimal(Eval("Vlr_Salario_Para")).ToString("N2"): 
                                Eval("Vlr_Salario_De") != DBNull.Value ?"R$ " + Convert.ToDecimal(Eval("Vlr_Salario_De")).ToString("N2") :
                                Eval("Vlr_Salario_Para") != DBNull.Value ?"R$ " + Convert.ToDecimal(Eval("Vlr_Salario_para")).ToString("N2") :
                            " a combinar" %><br />
                                        <b>Descrição: </b><%# Eval("Des_Atribuicoes") %>
                                        <%# !String.IsNullOrEmpty(Eval("Perguntas").ToString()) ? String.Format("<br><b>Pergunas:</b> {0}  ", Eval("Perguntas")) : String.Empty %>
                                    </span>&nbsp;<%# Eval("Flg_Deficiencia").ToString().Trim() == "True" ? "<i class='fa fa-wheelchair fa-1x ico_pcd'></i>"+ Eval("Des_Funcao") : Eval("Des_Funcao") %>&nbsp; </a>
                                </p>
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
                                <asp:LinkButton ID="btiVisualizarVaga" runat="server"
                                    ToolTip="Visualização completa da vaga" CausesValidation="false" CommandName="VisualizarVaga"><i class="fa fa-search"></i></asp:LinkButton>
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

<uc1:ModalConfirmacao ID="ucModalConfirmacao" runat="server" />
