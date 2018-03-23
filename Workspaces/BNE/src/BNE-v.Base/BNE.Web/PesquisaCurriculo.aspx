<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    Async="true" CodeBehind="PesquisaCurriculo.aspx.cs" Inherits="BNE.Web.PesquisaCurriculo" %>

<%@ Register Src="UserControls/Modais/EnvioDeMensagem.ascx" TagName="EnvioDeMensagem"
    TagPrefix="uc5" %>
<%@ Register Src="UserControls/Modais/EnvioCurriculo.ascx" TagName="EnvioCurriculo"
    TagPrefix="uc6" %>
<%@ Register Src="~/UserControls/Modais/ModalConfirmacao.ascx" TagName="ModalConfirmacao"
    TagPrefix="uc" %>
<%@ Register Src="UserControls/Modais/ConfirmacaoExclusao.ascx" TagName="ConfirmacaoExclusao"
    TagPrefix="uc" %>
<%@ Register Src="UserControls/Modais/ucModalLogin.ascx" TagName="ucModalLogin" TagPrefix="uc4" %>
<%@ Register Src="UserControls/Modais/ucAssociarCurriculoVaga.ascx" TagName="ucAssociarCurriculoVaga"
    TagPrefix="uc1" %>
<%@ Register Src="UserControls/Modais/ModalVendaChupaVIP.ascx" TagName="ModalVendaChupaVIP"
    TagPrefix="uc7" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/pesquisa_curriculo2.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicScript runat="server" Src="/js/local/Forms/PesquisaCurriculo.js" type="text/javascript" />
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
    <!-- Painel: Resultado Pesquisa -->
    <asp:UpdatePanel ID="upPesquisa" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlPesquisa" runat="server">
                <h1>
                    <asp:Label ID="lblTituloResultadoPesquisa" runat="server" Text="Resultado da Pesquisa de Currículos" />
                </h1>
                <asp:UpdatePanel ID="upInformacoesPesquisa" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="painel_resumo_curriculo">
                            <asp:Panel ID="pnlInformacoesPesquisa" runat="server">
                                <asp:Label ID="lblInformacoesPesquisa" runat="server"></asp:Label>
                            </asp:Panel>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="upDescricaoFuncao" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="pnlDescricaoFuncao" runat="server">
                            <div class="painel_resumo_atribuicoes">
                                <span class="atribuicoes">Atribuições do Cargo:</span>
                                <asp:Label ID="lblDescricaoFuncao" runat="server"></asp:Label>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Panel ID="pnlResultadoPesquisa" runat="server" CssClass="painel_padrao">
                    <div class="painel_padrao_topo">
                        &nbsp;
                    </div>
                    <%--<div class="legenda_resultado_pesquisa_curriculo">
                        VIP - Avaliação - Nome – Sexo – Estado Civil – Idade – Escolaridade – Pretensão
                        – Bairro – Cidade – Funções – Tempo Experiência – CNH - Ações
                    </div>--%>
                    <asp:UpdatePanel ID="upCurriculos" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
                        <ContentTemplate>
                            <telerik:RadGrid AlternatingItemStyle-CssClass="alt_row" ID="gvResultadoPesquisa"
                                runat="server" AllowPaging="True" AllowCustomPaging="true"
                                CssClass="gridview_padrao pesquisa_curriculo" OnItemDataBound="gvResultadoPesquisa_ItemDataBound"
                                OnItemCommand="gvResultadoPesquisa_ItemCommand" OnPageIndexChanged="gvResultadoPesquisa_PageIndexChanged"
                                OnItemCreated="gvResultadoPesquisa_ItemCreated" OnColumnCreated="gvResultadoPesquisa_ColumnCreated"
                                Skin="Office2007" GridLines="None" PageSize="20">
                                <ClientSettings EnablePostBackOnRowClick="true" AllowExpandCollapse="true" AllowGroupExpandCollapse="false">
                                </ClientSettings>
                                <PagerStyle Mode="NextPrevAndNumeric" PagerTextFormat=" {4} Currículos {2} a {3} de {5}"
                                    Position="TopAndBottom" EnableSEOPaging="False" PageSizeLabelText="Quantidade de cvs" />
                                <AlternatingItemStyle CssClass="alt_row" />
                                <MasterTableView DataKeyNames="Idf_Curriculo">
                                    <ExpandCollapseColumn Visible="False">
                                    </ExpandCollapseColumn>
                                    <NestedViewTemplate>
                                        <asp:Panel ID="ContainerCurriculo" runat="server" Visible="false">
                                            <telerik:RadListView ID="rlvCurriculos" runat="server" AllowPaging="false" DataKeyNames="Idf_Curriculo"
                                                ItemPlaceholderID="CurriculoPlaceHolder" OnItemCommand="rlvCurriculos_ItemCommand"
                                                OnItemDataBound="rlvCurriculos_ItemDataBound">
                                                <LayoutTemplate>
                                                    <table class="grid_resultado_curriculo">
                                                        <asp:PlaceHolder ID="CurriculoPlaceHolder" runat="server"></asp:PlaceHolder>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <div class="foto_pesquisa_curriculo">
                                                        <div class="nome_pessoa">
                                                            <%# RetornarNome( Eval("Nme_Pessoa").ToString(), Convert.ToBoolean(Eval("VIP"))) %>
                                                        </div>
                                                        <div class="borda_branca">
                                                            <telerik:RadBinaryImage runat="server" CssClass="btiFoto foto_dentro_borda_branca"
                                                                ImageUrl='<%# RetornarUrlFoto(DataBinder.Eval(Container.DataItem, "Num_CPF").ToString()) %>'
                                                                ID="rbiThumbnail" ResizeMode="None" AutoAdjustImageControlSize="False" />
                                                        </div>
                                                        <br />
                                                        <div class="codigo_curriculo">
                                                            Código CV:
                                                            <%# Eval("Idf_Curriculo")%>
                                                        </div>
                                                        <div>
                                                            Atualizado em:
                                                            <%# Convert.ToDateTime(Eval("Dta_Atualizacao")).ToShortDateString() %>
                                                        </div>
                                                    </div>
                                                    <div class="botao_padrao_fechar_escuro">

                                                        <asp:LinkButton ID="btiFechar" runat="server" CssClass="espacamento"
                                                            ToolTip="Fechar" CommandName="RowClick" CausesValidation="false"><i class="fa fa-times-circle"></i> Fechar</asp:LinkButton>
                                                    </div>
                                                    <div class="resultado_pesquisa_informacoes">
                                                        <asp:Repeater ID="rptEscolaridade" runat="server">
                                                            <ItemTemplate>
                                                                <div>
                                                                    <span class="descricao_bold">
                                                                        <%# Eval("Des_Grau_Escolaridade")%>
                                                                    </span><span class='<%# DataBinder.Eval(Container.DataItem, "Des_Grau_Escolaridade").ToString().Equals(String.Empty) ? "descricao_bold" : "texte_cor" %>'>
                                                                        <%# Eval("Des_Escolaridade")%>
                                                                    </span><span class="texte_cor">
                                                                        <%# DataBinder.Eval(Container.DataItem,"Des_Grau_Escolaridade").ToString().Equals(String.Empty) ? string.Empty :
                                                                                                                                                        !String.IsNullOrEmpty(Eval("Des_Escolaridade_Fonte").ToString()) ? " - " + Eval("Des_Escolaridade_Fonte") : string.Empty%></span>
                                                                    <span class="texte_cor">
                                                                        <%# DataBinder.Eval(Container.DataItem,"Dta_Conclusao").ToString().Equals(String.Empty) ? string.Empty : " - " + Eval("Dta_Conclusao") %>
                                                                    </span>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                        <div id="divUltimoSalario" class="ultimo_salario">
                                                            <span class="descricao_bold">
                                                                <%# DataBinder.Eval(Container.DataItem, "Vlr_Ultimo_Salario").ToString().Equals(String.Empty) ? String.Empty : "Último Salário: "%>
                                                            </span><span class="texte_cor">
                                                                <%# DataBinder.Eval(Container.DataItem, "Vlr_Ultimo_Salario").ToString().Equals(String.Empty) ? String.Empty : Convert.ToDecimal(Eval("Vlr_Ultimo_Salario")).ToString("N2")%></span>
                                                        </div>
                                                        <br />
                                                        <asp:Repeater ID="rptExperiencia" runat="server" OnItemDataBound="rptExperiencia_ItemDataBound">
                                                            <ItemTemplate>
                                                                <asp:Literal runat="server" ID="ltExperiencia" />
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </div>
                                                    <div class="botoes_resultado_pesquisa icones">
                                                        <asp:HyperLink ID="btiCvCompleto" runat="server" CssClass="espacamento completo btn-defaut" Target="_blank"
                                                            CausesValidation="false" NavigateUrl='<%# RetornarURL(Eval("Des_Funcao").ToString(), Eval("Nme_Cidade").ToString(), Eval("Sig_Estado").ToString(), Convert.ToInt32(Eval("Idf_Curriculo"))) %>'>
                                                            <i class="fa fa-eye"></i> Ver CV
                                                        </asp:HyperLink>
                                                        <asp:LinkButton runat="server" ID="btiDownload" CssClass="espacamento completo btn-defaut" CommandName="DownloadAnexo" CausesValidation="false" Visible="false">
                                                            <i class="fa fa-download"></i> Ver Anexo
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" ID="btiQuestionario" CssClass="espacamento completo btn-defaut" CommandName="Questionario" CausesValidation="false" Visible="false">
                                                            <i style="color:#0026ff" class="fa fa-question-circle"></i> Questionário
                                                        </asp:LinkButton>
                                                    </div>
                                                </ItemTemplate>
                                            </telerik:RadListView>
                                        </asp:Panel>
                                    </NestedViewTemplate>
                                    <Columns>
                                        <telerik:GridTemplateColumn>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="ckbHeaderItem" runat="server" AutoPostBack="True" OnCheckedChanged="ckbHeaderItem_CheckedChanged" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ckbDataItem" runat="server" AutoPostBack="True" OnCheckedChanged="ckbDataItem_CheckedChanged" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="VIP">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgClienteVIP" runat="server" CssClass="espacamento" AlternateText="VIP"
                                                    ToolTip="VIP" Title="VIP" ImageUrl="/img/icone_vip.png" Visible='<%# Convert.ToBoolean(Eval("VIP")) %>'
                                                    Enabled="false" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Avaliação" HeaderTooltip="Avaliação/SMS">
                                            <ItemTemplate>
                                                <div class="icone_avaliacao_sms">
                                                    <span class="icone_avaliacao_teste">
                                                        <Employer:BalaoSaibaMais ID="bsmAvaliacao" runat="server" ToolTipText='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Des_Avaliacao").ToString()) ? String.Empty : DataBinder.Eval(Container.DataItem, "Des_Avaliacao").ToString().Substring(0,DataBinder.Eval(Container.DataItem, "Des_Avaliacao").ToString().Length <= 140 ? DataBinder.Eval(Container.DataItem, "Des_Avaliacao").ToString().Length - 1 : 140) + "..." %>'
                                                            ToolTipTitle="Avaliação" CssClassLabel="balao_saiba_mais" TargetControlID="imgAvaliacao" />
                                                        <asp:LinkButton ID="imgAvaliacao" runat="server" AlternateText="Avaliação" CausesValidation="false"
                                                            CommandArgument='<%# Eval("Avaliacao") %>' Visible='<%# (!String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Avaliacao").ToString())) %>' />
                                                    </span>
                                                    <span class="icone_sms_teste">
                                                        <Employer:BalaoSaibaMais ID="bsmSMS" runat="server" ToolTipText='<%# Check("Des_SMS") ? (!string.IsNullOrWhiteSpace(Eval("Des_SMS").ToString()) ? Eval("Des_SMS").ToString() : string.Empty) : string.Empty %>'
                                                            ToolTipTitle="SMS" CssClassLabel="balao_saiba_mais" TargetControlID="lblSMS" />
                                                        <asp:Label runat="server" ID="lblSMS" class="fa fa-paper-plane fa-2x label_sms" Visible='<%# Check("Des_SMS") && !string.IsNullOrWhiteSpace(Eval("Des_SMS").ToString()) %>'></asp:Label>
                                                    </span>
                                                </div>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Nome">
                                            <ItemTemplate>
                                                <div class="nome_curriculo">
                                                    <asp:LinkButton ID="lblNomeCurriculo" runat="server" ToolTip="Nome" CssClass="nome_descricao_curriculo_padrao"
                                                        Text='<%# RetornarNome( Eval("Nme_Pessoa").ToString(), Convert.ToBoolean(Eval("VIP"))) %>'
                                                        CausesValidation="False" CommandName="MostrarModal">
                                                    </asp:LinkButton>
                                                    <asp:Panel runat="server" ID="pnlPCD" CssClass="ico_pcd" Visible='<%# Check("Idf_Deficiencia") && (Eval("Idf_Deficiencia") != null && Convert.ToInt32(Eval("Idf_Deficiencia")) != 0) %>'>
                                                        <i class="fa fa-wheelchair"></i>
                                                    </asp:Panel>
                                                </div>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Sexo">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lblSexoCurriculo" runat="server" ToolTip="Sexo" CssClass="nome_descricao_curriculo"
                                                    Text='<%# DataBinder.Eval(Container.DataItem, "Des_Genero") %>' CausesValidation="False"
                                                    CommandName="MostrarModal">
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="EC" HeaderTooltip="Estado Civil">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lblEstadoCivilCurriculo" runat="server" ToolTip="Estado Civil"
                                                    CssClass="nome_descricao_curriculo" Text='<%# DataBinder.Eval(Container.DataItem, "Des_Estado_Civil") %>'
                                                    CausesValidation="False" CommandName="MostrarModal">
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Idade">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lblIdadeCurriculo" runat="server" ToolTip="Idade" CssClass="nome_descricao_curriculo"
                                                    Text='<%# DataBinder.Eval(Container.DataItem, "Num_Idade") %>' CausesValidation="False"
                                                    CommandName="MostrarModal">
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="ESC" HeaderTooltip="Escolaridade">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lblEscolaridadeCurriculo" runat="server" ToolTip="Escolaridade"
                                                    CssClass="nome_descricao_curriculo" Text='<%# DataBinder.Eval(Container.DataItem, "Sig_Escolaridade") %>'
                                                    CausesValidation="False" CommandName="MostrarModal">
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="R$" HeaderTooltip="Pretensão Salarial">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lblPretensaoCurriculo" runat="server" ToolTip="Pretensão" CssClass="nome_descricao_curriculo"
                                                    Text='<%# DataBinder.Eval(Container.DataItem, "Vlr_Pretensao_Salarial") %>' CausesValidation="False"
                                                    CommandName="MostrarModal">
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Bairro">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lblBairroCurriculo" runat="server" ToolTip="Bairro" CssClass="nome_descricao_curriculo"
                                                    Text='<%# DataBinder.Eval(Container.DataItem, "Des_Bairro") %>' CausesValidation="False"
                                                    CommandName="MostrarModal">
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Cidade">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lblCidadeCurriculo" runat="server" ToolTip="Cidade" CssClass="nome_descricao_curriculo"
                                                    Text='<%# DataBinder.Eval(Container.DataItem, "Nme_Cidade") %>' CausesValidation="False"
                                                    CommandName="MostrarModal">
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Funções Pretendidas">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lblFuncaoCurriculo" runat="server" CssClass="nome_descricao_curriculo"
                                                    ToolTip="Função" Text='<%# RetornarFuncao(DataBinder.Eval(Container.DataItem, "Des_Funcao").ToString()) %>'
                                                    CausesValidation="False" CommandName="MostrarModal">
                                                    <%--ToolTip="Função" Text='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Des_Funcao").ToString()) ? String.Empty : DataBinder.Eval(Container.DataItem, "Des_Funcao").ToString().Substring(0, DataBinder.Eval(Container.DataItem, "Des_Funcao").ToString().Length - 1) %>'--%>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="TE" HeaderTooltip="Tempo de Experiência">
                                            <ItemTemplate>
                                                <div class="experiencia_curriculo_teste">
                                                    <asp:LinkButton ID="lblExperienciaCurriculo" runat="server" ToolTip="Tempo Experiência"
                                                        CssClass="nome_descricao_curriculo" Text='<%# DataBinder.Eval(Container.DataItem, "Des_Experiencia") %>'
                                                        CausesValidation="False" CommandName="MostrarModal">
                                                    </asp:LinkButton>
                                                </div>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="CNH">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lblCNHCurriculo" runat="server" CssClass="nome_descricao_curriculo"
                                                    ToolTip="CNH" Text='<%# DataBinder.Eval(Container.DataItem, "Des_Categoria_Habilitacao") %>'
                                                    CausesValidation="False" CommandName="MostrarModal">
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Ações">
                                            <ItemTemplate>
                                                <div id="divIcones" runat="server" class="icones_pesquisa_curriculo icones">
                                                    <asp:LinkButton ID="imgMensagem" class="fa fa-envelope espacamento" runat="server" CommandName="EnviarMensagem" CausesValidation="false" ToolTip="Enviar Mensagem"></asp:LinkButton>
                                                    <asp:LinkButton ID="imgCurriculo" class="fa fa-reply-all espacamento" runat="server" CausesValidation="false" CommandName="EnviarCurriculo" ToolTip="Enviar CV"></asp:LinkButton>
                                                    <asp:LinkButton ID="btiCompleto" class="fa fa-eye espacamento" runat="server" CausesValidation="false" ToolTip="Ver CV" href='<%# RetornarURL(Eval("Des_Funcao").ToString(), Eval("Nme_Cidade").ToString(), Eval("Sig_Estado").ToString(), Convert.ToInt32(Eval("Idf_Curriculo"))) %>' Target="_blank"></asp:LinkButton>
                                                    <asp:ImageButton ID="btiInformacoes" runat="server" class="espacamento" AlternateText="Informações" CausesValidation="false" CommandName="Informacoes" ToolTip="" Visible="false" ImageUrl="/img/icone_informacoes.png" />
                                                    <asp:LinkButton ID="btiExcluirCurriculo" class="fa fa-times espacamento" runat="server" CausesValidation="false" CommandName="RemoverCurriculo" ToolTip="" Visible='true'></asp:LinkButton>
                                                    <asp:LinkButton ID="btiAssociar" class="fa fa-folder-open-o espacamento" runat="server" ToolTip="Associar à uma Vaga" CommandName="Associar" Visible="False" CausesValidation="false"></asp:LinkButton>
                                                </div>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                    <NoRecordsTemplate>
                                        <div class="mensagem_nodata">
                                            Nenhum item para mostrar.
                                        </div>
                                    </NoRecordsTemplate>
                                </MasterTableView>
                                <HeaderContextMenu EnableAutoScroll="True">
                                </HeaderContextMenu>
                            </telerik:RadGrid>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <%--</div>--%>
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- FIM Painel: Resultado Pesquisa -->
    <!-- Painel Botões -->
    <asp:UpdatePanel ID="upBotoes" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes painel_pesquisa_curriculo">
                <asp:Button ID="btnVoltar" runat="server" Text="Voltar" CausesValidation="false"
                    CssClass="botao_padrao" OnClick="btnVoltar_Click" />
                <asp:Button ID="btnEnviarMensagem" runat="server" CssClass="botao_padrao" Text="Enviar Mensagem"
                    CausesValidation="false" OnClick="btnEnviarMensagem_Click" />
                <asp:Button ID="btnEnviarCurriculo" runat="server" CssClass="botao_padrao" Text="Enviar CV"
                    CausesValidation="false" OnClick="btnEnviarCurriculo_Click" />
                <asp:Button ID="btnComparar" runat="server" CssClass="botao_padrao" Text="Comparar CV"
                    CausesValidation="false" OnClick="btnComparar_Click" />
                <asp:Button ID="btnR1" runat="server" CssClass="botao_padrao" Text="Não Achei CV"
                    CausesValidation="false" OnClick="btnR1_Click" />
                <asp:Button ID="btnAnunciarVaga" runat="server" CssClass="botao_padrao" Text="Anunciar Vaga"
                    CausesValidation="false" OnClick="btnAnunciarVaga_Click" />
                <asp:Button ID="btnBuscaAvancada" runat="server" CssClass="botao_padrao" Text="Busca Avançada"
                    CausesValidation="false" OnClick="btnBuscaAvancada_Click" />
                <asp:Button ID="btnExcluirCurriculos" runat="server" CssClass="botao_padrao" Text="Excluir Currículos"
                    CausesValidation="false" OnClick="btnExcluirCurriculos_Click" Visible="false" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- FIM: Painel botoes -->
    <asp:Panel ID="pnlConteudo" runat="server">
        <uc:ConfirmacaoExclusao ID="ucConfirmacaoExclusao" runat="server" />
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
    <uc5:EnvioDeMensagem ID="ucEnvioMensagem" runat="server" />
    <uc6:EnvioCurriculo ID="ucEnvioCurriculo" runat="server" />
    <!-- Modal Confirmacao Envio -->
    <asp:UpdatePanel ID="upConfirmacaoEnvio" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlConfirmacaoEnvio" runat="server">
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- FIM: Modal Confirmacao Envio -->
    <asp:UpdatePanel ID="upConcessaoVip" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlConcessaoVip" runat="server">
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upM" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlM" runat="server" Visible="false">
                <uc:ModalConfirmacao ID="ucModalConfirmacao" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- Modal Login -->
    <asp:UpdatePanel ID="upL" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlL" runat="server" Visible="false">
                <uc4:ucModalLogin ID="ucModalLogin" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- FIM: Modal Login -->
    <asp:UpdatePanel ID="upCphModais" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:PlaceHolder ID="cphModais" runat="server"></asp:PlaceHolder>
        </ContentTemplate>
    </asp:UpdatePanel>
    <uc1:ucAssociarCurriculoVaga ID="ucAssociarCurriculoVaga" runat="server" />
    <uc7:ModalVendaChupaVIP ID="ModalVendaChupaVIP" runat="server" />
</asp:Content>
