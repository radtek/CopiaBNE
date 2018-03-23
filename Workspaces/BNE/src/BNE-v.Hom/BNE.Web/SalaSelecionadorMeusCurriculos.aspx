<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="SalaSelecionadorMeusCurriculos.aspx.cs" Inherits="BNE.Web.SalaSelecionadorMeusCurriculos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <!-- Deixar aqui -->
    <script type="text/javascript">
        function AbrirCurriculo(url) {
            window.open(url, '_new');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <h1>
        <asp:Label ID="lblTitulo" runat="server" Text="Meus Currículos"></asp:Label></h1>
    <div class="painel_padrao_topo">
        &nbsp;</div>
    <div class="painel_padrao">
        <div>
            <asp:Label ID="lblFuncao" runat="server" Text="Função" AssociatedControlID="ccFiltrarCurriculosFuncao"></asp:Label>
            <div class="container_campo">
                <Employer:ComboCheckbox runat="server" ID="ccFiltrarCurriculosFuncao" EmptyMessage="Filtrar currículos por função"
                    AllowCustomText="false" CssClass="checkbox_large" DropDownWidth="350">
                </Employer:ComboCheckbox>
            </div>
            <asp:Label ID="lblCurso" runat="server" Text="Técnico / Graduação" AssociatedControlID="txtTecnicoGraduacao"></asp:Label>
            <div class="container_campo">
                <asp:TextBox ID="txtTecnicoGraduacao" runat="server" CssClass="textbox_padrao" MaxLength="100"></asp:TextBox>
            </div>
            <asp:Button ID="btnFiltrar" runat="server" CssClass="botao_padrao filtrar" Text="Filtrar Currículos"
                OnClick="btnFiltrarCurriculos_Click" ToolTip="Filtrar Currículos" CausesValidation="false" />
        </div>
        <div class="legenda_resultado_pesquisa_curriculo">
            Avaliação - Nome – Sexo – Estado Civil – Idade – Escolaridade – Pretensão – Bairro
            – Cidade – Funções – Tempo Experiência – CNH - Ações
        </div>
        <asp:UpdatePanel ID="upMeusCurriculos" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <telerik:RadGrid AlternatingItemStyle-CssClass="alt_row" ID="gvResultadoPesquisa"
                    runat="server" AllowPaging="True" AllowCustomPaging="true" GroupingEnabled="false"
                    CssClass="gridview_padrao pesquisa_curriculo" OnItemDataBound="gvResultadoPesquisa_ItemDataBound"
                    OnPageIndexChanged="gvResultadoPesquisa_PageIndexChanged" OnItemCreated="gvResultadoPesquisa_ItemCreated"
                    OnColumnCreated="gvResultadoPesquisa_ColumnCreated" Skin="Office2007" GridLines="None">
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat=" {4} Currículos {2} a {3} de {5}"
                        Position="TopAndBottom" />
                    <AlternatingItemStyle CssClass="alt_row" />
                    <MasterTableView DataKeyNames="Idf_Curriculo">
                        <ExpandCollapseColumn Visible="False">
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridTemplateColumn>
                                <ItemTemplate>
                                    <div class="icone_avaliacao_teste">
                                        <Componentes:BalaoSaibaMais ID="bsmAvaliacao" runat="server" ToolTipText='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Des_Avaliacao").ToString()) ? String.Empty : DataBinder.Eval(Container.DataItem, "Des_Avaliacao").ToString().Substring(0,DataBinder.Eval(Container.DataItem, "Des_Avaliacao").ToString().Length <= 140 ? DataBinder.Eval(Container.DataItem, "Des_Avaliacao").ToString().Length - 1 : 140) + "..." %>'
                                            ToolTipTitle="Avaliação" CssClassLabel="balao_saiba_mais" TargetControlID="imgAvaliacao" ShowOnMouseover="True" />
                                        <asp:LinkButton ID="imgAvaliacao" runat="server" CausesValidation="false"
                                            CommandArgument='<%# Eval("Avaliacao") %>' Visible='<%# (!String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Avaliacao").ToString())) %>' />
                                    </div>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblNomeCurriculo" runat="server" ToolTip="Nome" CssClass="nome_descricao_curriculo_padrao"
                                        Text='<%# Eval("Nme_Pessoa").ToString() %>' CausesValidation="False" CommandName="MostrarModal">
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblSexoCurriculo" runat="server" ToolTip="Sexo" CssClass="nome_descricao_curriculo"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "Des_Genero") %>' CausesValidation="False"
                                        CommandName="MostrarModal">
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblEstadoCivilCurriculo" runat="server" ToolTip="Estado Civil"
                                        CssClass="nome_descricao_curriculo" Text='<%# DataBinder.Eval(Container.DataItem, "Des_Estado_Civil") %>'
                                        CausesValidation="False" CommandName="MostrarModal">
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblIdadeCurriculo" runat="server" ToolTip="Idade" CssClass="nome_descricao_curriculo"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "Num_Idade") %>' CausesValidation="False"
                                        CommandName="MostrarModal">
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblEscolaridadeCurriculo" runat="server" ToolTip="Escolaridade"
                                        CssClass="nome_descricao_curriculo" Text='<%# DataBinder.Eval(Container.DataItem, "Sig_Escolaridade") %>'
                                        CausesValidation="False" CommandName="MostrarModal">
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblPretensaoCurriculo" runat="server" ToolTip="Pretensão" CssClass="nome_descricao_curriculo"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "Vlr_Pretensao_Salarial") %>' CausesValidation="False"
                                        CommandName="MostrarModal">
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblBairroCurriculo" runat="server" ToolTip="Bairro" CssClass="nome_descricao_curriculo"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "Des_Bairro") %>' CausesValidation="False"
                                        CommandName="MostrarModal">
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblCidadeCurriculo" runat="server" ToolTip="Cidade" CssClass="nome_descricao_curriculo"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "Nme_Cidade") %>' CausesValidation="False"
                                        CommandName="MostrarModal">
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblFuncaoCurriculo" runat="server" CssClass="nome_descricao_curriculo"
                                        ToolTip="Função" Text='<%# RetornarFuncao(DataBinder.Eval(Container.DataItem, "Des_Funcao").ToString()) %>'
                                        CausesValidation="False" CommandName="MostrarModal">
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemTemplate>
                                    <div class="experiencia_curriculo_teste">
                                        <asp:LinkButton ID="lblExperienciaCurriculo" runat="server" ToolTip="Tempo Experiência"
                                            CssClass="nome_descricao_curriculo" Text='<%# DataBinder.Eval(Container.DataItem, "Des_Experiencia") %>'
                                            CausesValidation="False" CommandName="MostrarModal">
                                        </asp:LinkButton>
                                    </div>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblCNHCurriculo" runat="server" CssClass="nome_descricao_curriculo"
                                        ToolTip="CNH" Text='<%# DataBinder.Eval(Container.DataItem, "Des_Categoria_Habilitacao") %>'
                                        CausesValidation="False" CommandName="MostrarModal">
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemTemplate>
                                    <div id="divIcones" runat="server" class="icones_pesquisa_curriculo icones">
                                        <asp:HyperLink ID="btiCompleto" runat="server" CssClass="espacamento" AlternateText="Ver CV"
                                            CausesValidation="false" ToolTip="Ver CV" ImageUrl="/img/icone_ver_curriculo.png"
                                            NavigateUrl='<%# RetornarURL(Eval("Des_Funcao").ToString(), Eval("Nme_Cidade").ToString(), Eval("Sig_Estado").ToString(), Convert.ToInt32(Eval("Idf_Curriculo"))) %>'
                                            Target="_blank" />
                                    </div>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <NoRecordsTemplate>
                            <div class="mensagem_nodata">
                                Nenhum item para mostrar.</div>
                        </NoRecordsTemplate>
                    </MasterTableView>
                    <HeaderContextMenu EnableAutoScroll="True">
                    </HeaderContextMenu>
                </telerik:RadGrid>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnFiltrar" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel ID="upBotoes" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
                <asp:Button ID="btnVoltar" runat="server" CssClass="botao_padrao" Text="Voltar" CausesValidation="false"
                    OnClick="btnVoltar_Click" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
