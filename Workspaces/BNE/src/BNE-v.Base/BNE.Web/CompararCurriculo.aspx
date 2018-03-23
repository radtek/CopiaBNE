<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="CompararCurriculo.aspx.cs" Inherits="BNE.Web.CompararCurriculo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/CompararCurriculo.css" type="text/css" rel="stylesheet" />

        <%--Correção de BUG no Telerik com IE 10--%>
        <script>
            $(function () {
                if (typeof (window.$telerik.getLocation) == 'function' && Sys.Browser.agent == Sys.Browser.InternetExplorer && Sys.Browser.version == 10) {
                    window.$telerik.getLocation = function (a) {
                        if (a === document.documentElement) {
                            return new Sys.UI.Point(0, 0);
                        }
                        var offset = $(a).offset();
                        return new Sys.UI.Point(offset.left, offset.top);
                    }
                }
            });
        </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <h1>
        Comparar Currículos
    </h1>
    <div class="painel_padrao">
        <div class="painel_gerenciar_colunas">
            <Componentes:BalaoSaibaMais ID="bsmColunas" runat="server" ToolTipText="Utilize essa opção para inserção ou remoção de colunas na compraração dos currículos selecionados."
                CssClassLabel="balao_saiba_mais" TargetControlID="ccbColunas" ShowOnMouseover="True" />
            <Employer:ComboCheckbox ID="ccbColunas" AllowCustomText="false" ShowCheckAllButton="false"
                runat="server" EmptyMessage="Gerenciar Colunas" OnClientCheckItem="OnClientCheckItem"
                AutoPostBack="false" />
        </div>
        <asp:UpdatePanel ID="upGvCompararCurriculo" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="divImpressaoCompararCurriculo">
                    <telerik:RadGrid ID="gvCompararCurriculo" AlternatingItemStyle-CssClass="alt_row"
                        runat="server" CssClass="gridview_padrao" Skin="Office2007" GridLines="None"
                        AutoGenerateColumns="false" AllowSorting="true" OnItemCommand="gvCompararCurriculo_ItemCommand"
                        OnSortCommand="gvCompararCurriculo_SortCommand">
                        <AlternatingItemStyle CssClass="alt_row" />
                        <SortingSettings SortToolTip="Clique aqui para ordenar" SortedAscToolTip="Crescente"
                            EnableSkinSortStyles="true" SortedDescToolTip="Decrescente" />
                        <MasterTableView DataKeyNames="Idf_Curriculo">
                            <Columns>
                                <telerik:GridTemplateColumn HeaderStyle-CssClass="rgHeader coluna_nome_foto" ItemStyle-CssClass="centro"
                                    SortExpression="Nme_Pessoa" UniqueName="Coluna_nome" HeaderText="Nome">
                                    <ItemTemplate>
                                        <div class="foto_pesquisa_curriculo">
                                            <div class="borda_branca">
                                                <div class="container_foto">
                                                    <asp:HyperLink ID="btiFoto" runat="server" CssClass="btiFoto foto_dentro_borda_branca"
                                                        ImageUrl='<%# RetornarUrlFoto(DataBinder.Eval(Container.DataItem, "Num_CPF").ToString()) %>'
                                                        NavigateUrl='<%# RetornarURL(Eval("Des_Funcao").ToString(), Eval("Nme_Cidade").ToString(), Eval("Sig_Estado").ToString(), Convert.ToInt32(Eval("Idf_Curriculo"))) %>' />
                                                </div>
                                            </div>
                                            <asp:Label ID="lblNome" runat="server" CssClass="nome_descricao_curriculo" Text='<%# (DataBinder.Eval(Container.DataItem, "Nme_Pessoa").ToString()).Substring(0,(DataBinder.Eval(Container.DataItem, "Nme_Pessoa").ToString()).IndexOf(" ")) %>'>
                                            </asp:Label>
                                        </div>
                                        <%--Text='<%# (DataBinder.Eval(Container.DataItem, "Nme_Pessoa").ToString()).Substring(0,(DataBinder.Eval(Container.DataItem, "Nme_Pessoa").ToString()).IndexOf(" ")) %>'--%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn ItemStyle-CssClass="centro celula_idade" UniqueName="Coluna_idade"
                                    SortExpression="Num_Idade" HeaderText="Idade">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIdade" runat="server" ToolTip="Idade" CssClass="nome_descricao_curriculo"
                                            Text='<%# DataBinder.Eval(Container.DataItem, "Num_Idade") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Coluna_estadocivil" HeaderText="Estado Civil"
                                    SortExpression="Des_Estado_Civil" ItemStyle-CssClass="celula_estado_civil">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEstadoCivil" runat="server" ToolTip="Estado Civil" CssClass="nome_descricao_curriculo"
                                            Text='<%# DataBinder.Eval(Container.DataItem, "Des_Estado_Civil") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Coluna_escolaridade" SortExpression="Des_Escolaridade"
                                    HeaderText="Escolaridade">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEscolaridade" runat="server" ToolTip="Escolaridade" CssClass="nome_descricao_curriculo"
                                            Text='<%# DataBinder.Eval(Container.DataItem, "Des_Escolaridade") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Coluna_cidade" SortExpression="Nme_Cidade"
                                    HeaderText="Cidade">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCidade" runat="server" ToolTip="Cidade" CssClass="nome_descricao_curriculo"
                                            Text='<%# Eval("Nme_Cidade") + "/" + Eval("Sig_Estado") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Coluna_bairro" SortExpression="Des_Bairro"
                                    HeaderText="Bairro">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBairro" runat="server" ToolTip="Bairro" CssClass="nome_descricao_curriculo"
                                            Text='<%# DataBinder.Eval(Container.DataItem, "Des_Bairro") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn ItemStyle-CssClass="centro" UniqueName="Coluna_cnh" SortExpression="Des_Categoria_Habilitacao"
                                    HeaderText="CNH">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCNH" runat="server" ToolTip="CNH" CssClass="nome_descricao_curriculo"
                                            Text='<%# DataBinder.Eval(Container.DataItem, "Des_Categoria_Habilitacao") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Coluna_veiculo" HeaderText="Tipo de Veículo"
                                    SortExpression="Des_Tipo_Veiculo" ItemStyle-CssClass="celula_tipo_veiculo">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVeiculo" runat="server" ToolTip="Tipo de Veículo" CssClass="nome_descricao_curriculo"
                                            Text='<%# DataBinder.Eval(Container.DataItem, "Des_Tipo_Veiculo") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Coluna_filho" HeaderText="Filho" SortExpression="Flg_Filhos"
                                    ItemStyle-CssClass="celula_filhos">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFilho" runat="server" ToolTip="Filho" CssClass="nome_descricao_curriculo"
                                            Text='<%# DataBinder.Eval(Container.DataItem, "Flg_Filhos") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Coluna_deficiencia" HeaderText="Deficiência"
                                    SortExpression="Des_Deficiencia" ItemStyle-CssClass="celula_deficiencia">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDeficiencia" runat="server" ToolTip="Deficiência" CssClass="nome_descricao_curriculo"
                                            Text='<%# DataBinder.Eval(Container.DataItem, "Des_Deficiencia") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-CssClass="rgHeader coluna_pretensao" ItemStyle-CssClass="direita"
                                    SortExpression="Vlr_Pretensao_Salarial" UniqueName="Coluna_pretensao_salarial"
                                    HeaderText="Pretensão">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPretensaoSalarial" runat="server" ToolTip="Pretensão Salarial"
                                            CssClass="nome_descricao_curriculo" Text='<%# DataBinder.Eval(Container.DataItem, "Vlr_Pretensao_Salarial", "{0:C}") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-CssClass="rgHeader coluna_ultimo_salario"
                                    ItemStyle-CssClass="direita" UniqueName="Coluna_ultimo_salario" SortExpression="Vlr_Ultimo_Salario"
                                    HeaderText="Último Salário">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUltimoSalario" runat="server" ToolTip="Último Salário" CssClass="nome_descricao_curriculo"
                                            Text='<%# DataBinder.Eval(Container.DataItem, "Vlr_Ultimo_Salario", "{0:C}") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Coluna_funcao_pretendida" SortExpression="Des_Funcao"
                                    HeaderText="Função Pretendida">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFuncaoPretendida" runat="server" ToolTip="Função Pretendida" CssClass="nome_descricao_curriculo"
                                            Text='<%# FormatarFuncaoPretendida(Eval("Des_Funcao")) %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Coluna_ultima_experiencia" SortExpression="Des_Ultima_Experiencia"
                                    HeaderText="Última Empresa">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUltimaEmpresa" runat="server" ToolTip="Última Empresa" CssClass="nome_descricao_curriculo"
                                            Text='<%# DataBinder.Eval(Container.DataItem, "Des_Ultima_Experiencia") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Coluna_penultima_experiencia" SortExpression="Des_Penultima_Experiencia"
                                    HeaderText="Penúltima Empresa">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPenultimaEmpresa" runat="server" ToolTip="Penúltima Empresa" CssClass="nome_descricao_curriculo"
                                            Text='<%# DataBinder.Eval(Container.DataItem, "Des_Penultima_Experiencia") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Coluna_antepenultima_experiencia" SortExpression="Des_Antepenultima_Experiencia"
                                    HeaderText="Antepenúltima Empresa">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAntepenúltimaEmpresa" runat="server" ToolTip="Antepenúltima Empresa"
                                            CssClass="nome_descricao_curriculo" Text='<%# DataBinder.Eval(Container.DataItem, "Des_Antepenultima_Experiencia") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Ações" ItemStyle-CssClass="centro" HeaderStyle-CssClass="rgHeader centro">
                                    <ItemTemplate>
                                     
                                        <asp:LinkButton ID="btiExcluir" runat="server" AlternateText="Excluir" CausesValidation="False"
                                            ToolTip="Excluir" CommandName="Deletar"><i class="fa fa-trash"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
                <asp:Button ID="btnAplicarColunas" runat="server" Text="Aplicar alteração nas colunas"
                    OnClick="btnAplicarColunas_Click" Style="display: none" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel ID="upBotoes" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
                <asp:Button ID="btnImprimir" runat="server" CssClass="botao_padrao" OnClientClick="ImprimirCompararCv();"
                    Text="Imprimir" PostBackUrl="javascript:;" CausesValidation="false" Visible="false" />
                <asp:Button ID="btnVoltar" runat="server" CssClass="botao_padrao" Text="Voltar" CausesValidation="false"
                    OnClick="btnVoltar_Click" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
    <Employer:DynamicScript runat="server" Src="/js/local/Forms/CompararCurriculo.js" type="text/javascript" />
</asp:Content>
