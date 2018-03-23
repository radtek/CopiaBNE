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
        var callbackMethod;
        function CallbackPaging() {
            if (callbackMethod != null) {
                callbackMethod();
            }
        }

        function GoPrevious(callback) {
            $('#cphConteudo_btnPrevious').trigger('click');
            callbackMethod = callback;
        }
        function GoNext(callback) {
            $('#cphConteudo_btnNext').trigger('click');
            callbackMethod = callback;
        }

        function ExibirDownloadSineCV(id) {
            var prv = $("#btn_link_sine_" + id).prev();
            if (!prv.hasClass("btiDownloadCls")) {
                $("#btn_link_sine_" + id).show();
            }
        }


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

            $('#upTopo').css('width', 'auto');
            $('#upTopo').css('height', '230px');
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <!-- Painel: Resultado Pesquisa -->
    <asp:UpdatePanel ID="upPesquisa" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlPesquisa" runat="server">
                <asp:UpdatePanel ID="upTitulo" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <h1>
                            <asp:Label ID="lblTituloResultadoPesquisa" runat="server" Text="Resultado da Pesquisa de Currículos" />
                        </h1>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <%--INICIO BOTÃO FILTROS--%>
                <asp:UpdatePanel runat="server" ID="upFiltroCvVaga" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel runat="server" ID="pnlFiltroUsuarioVaga" Visible="false">
                            <section id="filtrar_busca_cv">
                                <div>
                                    <a class="btn btn-success" id="filtrar_busca_cv-open_modal" onclick="AbrirModalFiltros();">filtrar lista<span class="span_novo">novo!</span></a>
                                </div>
                                <div id="filtrar_busca_cv-filtros_utilizados">

                                    <asp:Panel runat="server" ID="pnlFiltrosCurriculosVaga" Visible="false">
                                        <div id="filtrar_busca_cv__subtitle">Filtros aplicados, clique para remover:</div>
                                        <ul>
                                            <asp:Repeater ID="rpFiltrosCurriculos" runat="server">
                                                <ItemTemplate>
                                                    <li>
                                                        <asp:LinkButton runat="server" ID="lnkRemoverFiltroCurriculoVaga"
                                                            OnCommand="lnkRemoverFiltroCurriculoVaga_Command" OnClientClick="MostrarCarregando()" CausesValidation="false" CommandName='<%# DataBinder.Eval(Container.DataItem, "ValorFiltro")%>' CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Filtro") %>'>
                                                        <strong>		<i class="fa fa-times" aria-hidden="true"></i> <%# DataBinder.Eval(Container.DataItem, "Campo") %></strong> <%# DataBinder.Eval(Container.DataItem, "Des_Filtro") %>
                                                        </asp:LinkButton>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:Repeater>

                                        </ul>
                                    </asp:Panel>

                                </div>
                            </section>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <%--FIM BOTÃO FILTROS--%>
                <asp:UpdatePanel ID="upBase" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                    <ContentTemplate>
                        <asp:Panel runat="server" ID="pnlBasePaga" CssClass="base-curriculo base-paga" Visible="False">
                            <span>Acesse os currículos exclusivos para assinantes!</span>
                            <asp:LinkButton runat="server" ID="btlBasePaga" OnClick="btlBasePaga_Click" Text="Currículos Exclusivos" CausesValidation="False"></asp:LinkButton>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="pnlBaseGratis" CssClass="base-curriculo base-gratis" Visible="False">
                            <span>O BNE disponibiliza para você uma base gratuita de currículos!</span>
                            <asp:LinkButton runat="server" ID="btlBaseGratis" OnClick="btlBaseGratis_Click" Text="Currículos Grátis" CausesValidation="False"></asp:LinkButton>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="upInformacoesPesquisa" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:HiddenField ID="IdPF" runat="server" />
                        <asp:HiddenField ID="IdFi" runat="server" />
                        <asp:Panel ID="pnlBannerMedia" runat="server" Visible="false">
                            <a runat="server" id="linkSalarioBrMedia" href="http://www.salariobr.com.br/PesquisaSalarialPorPorte?funcao=&idadeDe=16&idadeAte=80&utm_source=SalarioBR&utm_medium=banner&utm_campaign=BneBannerMedia" target="_blank">
                                <div class="painel_resumo_curriculo">
                                    <asp:Panel ID="pnlInformacoesPesquisa" runat="server">
                                        <div class="BannerMedia">
                                            Quer Saber a <b>média salarial</b> de
                                        </div>
                                        <div class="txtLinhaFuncao">
                                            <b>
                                                <asp:Label ID="lblFuncao" runat="server"></asp:Label></b>
                                            em
                                <b>
                                    <asp:Label ID="lblCidade" runat="server"></asp:Label></b><br />
                                        </div>

                                    </asp:Panel>
                                </div>
                                <img onclick="trackEvent('PesquisaCurriculo', 'SalarioBR', 'BannerMedia');SalarioBR(); return true;" id="ImgBannerMedia" src="/img/salariobr/banner_media-final.png" />
                            </a>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="pnlBannerAtribuicoes" Visible="false">
                            <a runat="server" id="linkSalarioBrAtribuicoes" href="http://www.salariobr.com.br/?utm_source=SalarioBR&utm_medium=banner&utm_campaign=BneBannerAtribuicoes" target="_blank">
                                <div class="painel_resumo_curriculo">
                                    <asp:Panel ID="Panel1" runat="server">
                                        <div class="linhaFunao">
                                            <b>
                                                <asp:Label ID="lblFuncao2" runat="server"></asp:Label></b>
                                            em
                                <b>
                                    <asp:Label ID="lblCidade2" runat="server"></asp:Label></b><br />
                                        </div>
                                        <div id="txtAtribuicoes" class="linhaAtribuicao">
                                            <b>Atribuições:</b>
                                            <asp:Label ID="lblDescricaoFuncao" runat="server"></asp:Label>
                                        </div>
                                    </asp:Panel>
                                </div>
                                <img onclick="trackEvent('PesquisaCurriculo', 'SalarioBR', 'BannerAtribuicoes');SalarioBR(); return true;" id="ImgBannerAtribuicoes" src="/img/salariobr/banner_atribuicoes-final.png" />
                            </a>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <asp:Panel ID="pnlResultadoPesquisa" runat="server" CssClass="painel_padrao">
                    <%-- <div class="painel_padrao_topo">
                        &nbsp;
                    </div>--%>
                    <%--<div class="legenda_resultado_pesquisa_curriculo">
                        VIP - Avaliação - Nome – Sexo – Estado Civil – Idade – Escolaridade – Pretensão
                        – Bairro – Cidade – Funções – Tempo Experiência – CNH - Ações
                    </div>--%>
                    <asp:UpdatePanel ID="upCurriculos" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
                        <ContentTemplate>
                            <telerik:RadGrid AlternatingItemStyle-CssClass="alt_row" ID="gvResultadoPesquisa"
                                runat="server" AllowPaging="True" AllowCustomPaging="True"
                                CssClass="gridview_padrao pesquisa_curriculo" OnItemDataBound="gvResultadoPesquisa_ItemDataBound"
                                OnItemCommand="gvResultadoPesquisa_ItemCommand" OnPageIndexChanged="gvResultadoPesquisa_PageIndexChanged"
                                OnItemCreated="gvResultadoPesquisa_ItemCreated" OnColumnCreated="gvResultadoPesquisa_ColumnCreated"
                                Skin="Office2007" GridLines="None" PageSize="20" OnDataBound="gvResultadoPesquisa_DataBound">
                                <ClientSettings EnablePostBackOnRowClick="true" AllowExpandCollapse="true" AllowGroupExpandCollapse="false">
                                </ClientSettings>
                                <PagerStyle Mode="NextPrevAndNumeric" PagerTextFormat=" {4} Currículos {2} a {3} de {5}"
                                    Position="TopAndBottom" EnableSEOPaging="False" PageSizeLabelText="Quantidade de cvs" />
                                <AlternatingItemStyle CssClass="alt_row" />
                                <MasterTableView DataKeyNames="Idf_Curriculo">
                                    <GroupByExpressions>
                                        <telerik:GridGroupByExpression>
                                            <SelectFields>
                                                <telerik:GridGroupByField FieldAlias="DentroPerfil" FieldName="Dentro_Perfil"></telerik:GridGroupByField>
                                            </SelectFields>
                                            <GroupByFields>
                                                <telerik:GridGroupByField FieldName="Dentro_Perfil" SortOrder="Descending"></telerik:GridGroupByField>
                                            </GroupByFields>
                                        </telerik:GridGroupByExpression>
                                    </GroupByExpressions>
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
                                                                ImageUrl='<%# RetornarUrlFoto(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "Num_CPF"))) %>'
                                                                ID="rbiThumbnail" ResizeMode="None" AutoAdjustImageControlSize="False" />
                                                        </div>
                                                        <br />
                                                        <div class="codigo_curriculo">
                                                            Código CV:
                                                            <%# Eval("Idf_Curriculo")%>
                                                        </div>
                                                        <div>
                                                            Último Acesso em:
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
                                                        <div id="divUltimoSalario" runat="server" class="ultimo_salario" visible="false">
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
                                                        <br />
                                                        <asp:Panel runat="server" ID="pnlMinhasExperiencias" Visible="False">
                                                            Minhas experiências:
                                                            <br />
                                                            <asp:Repeater ID="rptFuncaoPretendida" runat="server" OnItemDataBound="rptFuncaoPretendida_ItemDataBound">
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltFuncaoPretendida" />
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </asp:Panel>
                                                    </div>
                                                    <div class="botoes_resultado_pesquisa icones">
                                                        <asp:LinkButton ID="lnkAtualizarCurriculo" runat="server" CssClass="espacamento completo btn-defaut" CausesValidation="false" CommandName="AtualizarCurriculo">
                                                             <i class="fa fa-exclamation-triangle"></i>Solicitar Atualização
                                                        </asp:LinkButton>

                                                        <asp:LinkButton ID="lbtiChamarAgora" runat="server" CssClass="espacamento completo btn-defaut" CausesValidation="false" CommandName="ChamarAgora">
                                                            <i class="fa fa-bell"></i> Chamar Agora
                                                        </asp:LinkButton>
                                                        <asp:HyperLink ID="btiCvCompleto" runat="server" CssClass="espacamento completo btn-defaut" Target="_blank"
                                                            CausesValidation="false" NavigateUrl='<%#RetornarURL(Eval("Des_Funcao").ToString(), Eval("Nme_Cidade").ToString(), Eval("Sig_Estado").ToString(), Convert.ToInt32(Eval("Idf_Curriculo"))) %>'>
                                                            <i class="fa fa-eye"></i> Ver CV
                                                        </asp:HyperLink>
                                                        <asp:LinkButton runat="server" ID="btiDownload" CssClass="espacamento completo btn-defaut btiDownloadCls" CommandName="DownloadAnexo" CausesValidation="false" Visible="false">
                                                            <%--<i class="fa fa-download"></i>--%> <i class="fa fa-paperclip" aria-hidden="true"></i> Ver Anexo
                                                        </asp:LinkButton>

                                                        <a style="display: none;" class="espacamento completo btn-defaut" id="btn_link_sine_<%#Eval("Idf_Curriculo")%>" onclick="javascript:__doPostBack('ver_cv_sine','<%#Eval("Idf_Curriculo")%>')">
                                                            <i class="fa fa-download"></i>Ver Anexo 
                                                        </a>

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
                                        <telerik:GridTemplateColumn HeaderText="Avaliação " HeaderTooltip="Avaliação/SMS">
                                            <ItemTemplate>
                                                <div class="icone_avaliacao_sms">
                                                    <span class="icone_avaliacao_teste">
                                                        <%--<Componentes:BalaoSaibaMais ID="bsmAvaliacao" runat="server" ToolTipText='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Des_Avaliacao").ToString()) ? String.Empty : (DataBinder.Eval(Container.DataItem, "Des_Avaliacao").ToString().Length <= 140 ?  DataBinder.Eval(Container.DataItem, "Des_Avaliacao").ToString() : String.Format("{0}...", DataBinder.Eval(Container.DataItem, "Des_Avaliacao").ToString().Substring(0,140)))  %>'
                                                            ToolTipTitle="Avaliação" CssClassLabel="balao_saiba_mais" TargetControlID="imgAvaliacao" Visible='<%# !String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Des_Avaliacao").ToString()) %>' />--%>
                                                        <div class="tooltipC balao">
                                                            <div id="Div1" runat="server">
                                                                Avaliação
                                                                <br />
                                                                <%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Des_Avaliacao").ToString()) ? String.Empty : (DataBinder.Eval(Container.DataItem, "Des_Avaliacao").ToString().Length <= 140 ?  DataBinder.Eval(Container.DataItem, "Des_Avaliacao").ToString() : String.Format("{0}...", DataBinder.Eval(Container.DataItem, "Des_Avaliacao").ToString().Substring(0,140)))  %>
                                                            </div>
                                                         
                                                           <asp:LinkButton ID="imgAvaliacao" runat="server" AlternateText="Avaliação" CausesValidation="false"
                                                            CommandArgument='<%# Eval("Avaliacao") %>' Visible='<%# (!String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Des_Avaliacao").ToString())) %>' />
                                                         
                                                        </div>
                                                       
                                                    </span>
                                                    <span class="icone_sms_teste" style="position: relative;">
                                                        <%--   <Componentes:BalaoSaibaMais ID="bsmSMS" runat="server" ToolTipText='<%# Check("Des_SMS") ? (!string.IsNullOrWhiteSpace(Eval("Des_SMS").ToString()) ? Eval("Des_SMS").ToString() : string.Empty) : string.Empty %>'
                                                            ToolTipTitle="SMS" CssClassLabel="balao_saiba_mais" TargetControlID="lblSMS" />--%>
                                                        <div class="tooltipC balao">
                                                            <div id="litSMSTooltip2" runat="server">
                                                                SMS
                                                                <br />
                                                                <%# Check("Des_SMS") ? (!string.IsNullOrWhiteSpace(Eval("Des_SMS").ToString()) ? Eval("Des_SMS").ToString() : string.Empty) : string.Empty %>
                                                            </div>
                                                            <asp:Label runat="server" ID="lblSMS2" class="fa fa-paper-plane fa-2x label_sms" Visible='<%# Check("Des_SMS") && !string.IsNullOrWhiteSpace(Eval("Des_SMS").ToString()) %>'>
                                                          
                                                            </asp:Label>
                                                        </div>
                                                    </span>
                                                    <span>
                                                        <asp:Literal ID="litCVIndicado" Visible='<%# Convert.ToBoolean(Eval("Flg_Auto_Candidatura")) %>' runat="server"><i class="fa fa-thumbs-up" data-toggle="tooltip" title="CV indicado."></i></asp:Literal>
                                                    </span>
                                                </div>

                                                <asp:Panel runat="server" ID="pnlSTC" Visible='<%# Check("Pertence_origem") && Convert.ToBoolean(Eval("Pertence_origem")) %>'>
                                                    <i class="fa fa-check fa-2x" data-toggle="tooltip" title="CV cadastrado no site da empresa."></i>
                                                </asp:Panel>

                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Nome">
                                            <ItemTemplate>
                                                <div class="nome_curriculo">
                                                    <asp:LinkButton ID="lblNomeCurriculo" runat="server" CssClass=" nome_descricao_curriculo_padrao"
                                                        CausesValidation="False" CommandName="MostrarModal">
                                                        <asp:Label ID="lblQuemVisualiza" runat="server" Visible="false"></asp:Label>
                                                        <%# RetornarNome( Eval("Nme_Pessoa").ToString(), Convert.ToBoolean(Eval("VIP")))  + (!String.IsNullOrEmpty(Eval("Nme_Anexo").ToString()) ? "    <i class=\"fa fa-paperclip fa-1x\" aria-hidden=\"true\"></i>" : "") %>
                                                    </asp:LinkButton>
                                                    <asp:Panel runat="server" ID="pnlPCD" CssClass="ico_pcd" Visible='<%# Check("Idf_Deficiencia") && (Eval("Idf_Deficiencia") != DBNull.Value && Convert.ToInt32(Eval("Idf_Deficiencia")) != 0) %>'>
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
                                                    Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "Vlr_Pretensao_Salarial")).ToString("N2") %>' CausesValidation="False"
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
                                        <telerik:GridTemplateColumn HeaderText="TE" HeaderTooltip="Tempo de Experiência" Visible="False">
                                            <ItemTemplate>
                                                <div class="experiencia_curriculo_teste">
                                                    <asp:LinkButton ID="lblExperienciaCurriculo" runat="server" ToolTip="Tempo Experiência"
                                                        CssClass="nome_descricao_curriculo" Text='<%# DataBinder.Eval(Container.DataItem, "Des_Experiencia") %>'
                                                        CausesValidation="False" CommandName="MostrarModal">
                                                    </asp:LinkButton>
                                                </div>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Ações">
                                            <ItemTemplate>
                                                <div id="divIcones" runat="server" class="icones_pesquisa_curriculo icones">
                                                    <asp:LinkButton ID="lbtnChamarAgora" runat="server" CommandName="ChamarAgora" CssClass="fa fa-bell espacamento" CausesValidation="false" ToolTip="Chamar Agora" OnClientClick="trackEvent('ResultadoPesquisaCurriculoCurriculoss', 'clickAcao', 'ChamarAgora');"></asp:LinkButton>
                                                    <asp:LinkButton ID="imgMensagem" class="fa fa-envelope espacamento" runat="server" CommandName="EnviarMensagem" CausesValidation="false" ToolTip="Enviar Mensagem" OnClientClick="trackEvent('ResultadoPesquisaCurriculoCurriculoss', 'clickAcao', 'EnviarMensagem');"></asp:LinkButton>
                                                    <asp:LinkButton ID="imgCurriculo" class="fa fa-reply-all espacamento" runat="server" CausesValidation="false" CommandName="EnviarCurriculo" ToolTip="Enviar CV" OnClientClick="trackEvent('ResultadoPesquisaCurriculoCurriculoss', 'clickAcao', 'EnviarCV');"></asp:LinkButton>
                                                    <asp:LinkButton ID="btiCompleto" class="fa fa-eye espacamento" runat="server" CausesValidation="false" ToolTip="Ver CV" href='<%# RetornarURL(Eval("Des_Funcao").ToString(), Eval("Nme_Cidade").ToString(), Eval("Sig_Estado").ToString(), Convert.ToInt32(Eval("Idf_Curriculo"))) %>' Target="_blank" OnClientClick="trackEvent('ResultadoPesquisaCurriculoCurriculoss', 'clickAcao', 'VerCV');"></asp:LinkButton>
                                                    <asp:ImageButton ID="btiInformacoes" runat="server" class="espacamento" AlternateText="Informações" CausesValidation="false" CommandName="Informacoes" ToolTip="" Visible="false" ImageUrl="/img/icone_informacoes.png" OnClientClick="trackEvent('ResultadoPesquisaCurriculoCurriculoss', 'clickAcao', 'Informacoes');" />

                                                    <asp:LinkButton ID="btiExcluirCurriculo" class="fa fa-times espacamento" runat="server" CausesValidation="false" CommandName="RemoverCurriculo" ToolTip="Excluir" Visible="false" OnClientClick="trackEvent('ResultadoPesquisaCurriculoCurriculoss', 'clickAcao', 'ExcluirCV');"></asp:LinkButton>
                                                    <asp:LinkButton ID="btiAssociar" class="fa fa-folder-open-o espacamento" runat="server" ToolTip="Associar à uma Vaga" CommandName="Associar" Visible="False" CausesValidation="false" OnClientClick="trackEvent('ResultadoPesquisaCurriculoCurriculoss', 'clickAcao', 'AssociarAVaga');"></asp:LinkButton>
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

                            <asp:Panel runat="server" ID="pnlFiltros">
                                <div id="filtros_pesq_avanc">
                                    <div class="filtros_pesq_avanc__title">Faça uma nova pesquisa</div>
                                    <div class="filtros_pesq_avanc__subtitle">Remova um dos filtros utilizados na busca atual:</div>
                                    <ul>
                                        <asp:Repeater ID="rpFiltrosBusca" runat="server">
                                            <ItemTemplate>
                                                <li>
                                                    <asp:LinkButton runat="server" ID="lnkRemoverFiltro" OnCommand="lnkRemoverFiltro_Command" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Coluna") %>'>
                                                <i class="fa fa-times" aria-hidden="true"></i>
                                                <strong> <%# DataBinder.Eval(Container.DataItem, "Filtro") %></strong> <%# DataBinder.Eval(Container.DataItem, "FiltroValor") %><span class="filtros_pesq_avanc__value">+ <%# DataBinder.Eval(Container.DataItem, "qtdCvs") %> CVS</span>

                                                    </asp:LinkButton>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                </div>
                            </asp:Panel>

                            <telerik:RadGrid AlternatingItemStyle-CssClass="alt_row" Visible="false" ID="gvResultadoPesquisaCampanha"
                                runat="server" AllowPaging="True" AllowCustomPaging="True"
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
                                    <GroupByExpressions>
                                        <telerik:GridGroupByExpression>
                                            <SelectFields>
                                                <telerik:GridGroupByField FieldAlias="DentroPerfil" FieldName="Dentro_Perfil"></telerik:GridGroupByField>
                                            </SelectFields>
                                            <GroupByFields>
                                                <telerik:GridGroupByField FieldName="Dentro_Perfil" SortOrder="Descending"></telerik:GridGroupByField>
                                            </GroupByFields>
                                        </telerik:GridGroupByExpression>
                                    </GroupByExpressions>
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
                                                            <%# RetornarNome( Eval("Nme_Pessoa").ToString(), Convert.ToBoolean(Eval("VIP")))  %>
                                                        </div>
                                                        <div class="borda_branca">
                                                            <telerik:RadBinaryImage runat="server" CssClass="btiFoto foto_dentro_borda_branca"
                                                                ImageUrl='<%# RetornarUrlFoto(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "Num_CPF"))) %>'
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
                                                        <div id="divUltimoSalario" runat="server" class="ultimo_salario" visible="false">
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
                                                        <br />
                                                        <asp:Panel runat="server" ID="pnlMinhasExperiencias" Visible="False">
                                                            Minhas experiências:
                                                            <br />
                                                            <asp:Repeater ID="rptFuncaoPretendida" runat="server" OnItemDataBound="rptFuncaoPretendida_ItemDataBound">
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltFuncaoPretendida" />
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </asp:Panel>
                                                    </div>
                                                    <div class="botoes_resultado_pesquisa icones">
                                                        <asp:LinkButton ID="lbtiChamarAgora" runat="server" CssClass="espacamento completo btn-defaut" CausesValidation="false" CommandName="ChamarAgora">
                                                            <i class="fa fa-bell"></i> Chamar Agora
                                                        </asp:LinkButton>
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
                                                <asp:CheckBox ID="ckbHeaderItem" runat="server" AutoPostBack="True" OnCheckedChanged="ckbHeaderItem_CheckedChanged1" />
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
                                        <telerik:GridTemplateColumn HeaderText="Campanha" DataField="Campanha">
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemTemplate>
                                                <asp:ImageButton CssClass="espacamento" ID="imgCampanha" runat="server" AlternateText="Campanha"
                                                    ToolTip="Campanha" Title="Campanha" ImageUrl="/img/img_icone_check_16x16.png" Visible='<%# Convert.ToBoolean(Eval("Flg_Campanha")) %>'
                                                    Enabled="false" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Avaliação" HeaderTooltip="Avaliação/SMS">
                                            <ItemTemplate>
                                                <div class="icone_avaliacao_sms">
                                                    <span class="icone_avaliacao_teste">
                                                        <Componentes:BalaoSaibaMais ID="bsmAvaliacao" runat="server" ToolTipText='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Des_Avaliacao").ToString()) ? String.Empty : (DataBinder.Eval(Container.DataItem, "Des_Avaliacao").ToString().Length <= 140 ?  DataBinder.Eval(Container.DataItem, "Des_Avaliacao").ToString() : String.Format("{0}...", DataBinder.Eval(Container.DataItem, "Des_Avaliacao").ToString().Substring(0,140))) %>'
                                                            ToolTipTitle="Avaliação" CssClassLabel="balao_saiba_mais" TargetControlID="imgAvaliacao" Visible='<%# !String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Des_Avaliacao").ToString()) %>' />
                                                        <asp:LinkButton ID="imgAvaliacao" runat="server" AlternateText="Avaliação" CausesValidation="false"
                                                            CommandArgument='<%# Eval("Avaliacao") %>' Visible='<%# (!String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Avaliacao").ToString())) %>' />
                                                    </span>
                                                    <span class="icone_sms_teste" style="position: relative;">
                                                        <%--   <Componentes:BalaoSaibaMais ID="bsmSMS" runat="server" ToolTipText='<%# Check("Des_SMS") ? (!string.IsNullOrWhiteSpace(Eval("Des_SMS").ToString()) ? Eval("Des_SMS").ToString() : string.Empty) : string.Empty %>'
                                                            ToolTipTitle="SMS" CssClassLabel="balao_saiba_mais" TargetControlID="lblSMS" />--%>
                                                        <div class="tooltipC balao">
                                                            <div id="litSMSTooltip1" runat="server">
                                                                SMS
                                                                <br />
                                                                <%# Check("Des_SMS") ? (!string.IsNullOrWhiteSpace(Eval("Des_SMS").ToString()) ? Eval("Des_SMS").ToString() : string.Empty) : string.Empty %>
                                                            </div>
                                                            <asp:Label runat="server" ID="lblSMS1" class="fa fa-paper-plane fa-2x label_sms" Visible='<%# Check("Des_SMS") && !string.IsNullOrWhiteSpace(Eval("Des_SMS").ToString()) %>'>
                                                          
                                                            </asp:Label>
                                                        </div>
                                                    </span>
                                                </div>

                                                <asp:Panel runat="server" ID="pnlSTC" Visible='<%# Check("Pertence_origem") && Convert.ToBoolean(Eval("Pertence_origem")) %>'>
                                                    <i class="fa fa-check fa-2x" data-toggle="tooltip" title="CV cadastrado no site da empresa."></i>
                                                </asp:Panel>

                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Nome">
                                            <ItemTemplate>
                                                <div class="nome_curriculo">
                                                    <asp:LinkButton ID="lblNomeCurriculo" runat="server" CssClass=" nome_descricao_curriculo_padrao"
                                                        CausesValidation="False" CommandName="MostrarModal">
                                                        <asp:Label ID="lblQuemVisualiza" runat="server" Visible="false"></asp:Label>
                                                        <%# RetornarNome( Eval("Nme_Pessoa").ToString(), Convert.ToBoolean(Eval("VIP")))  + (!String.IsNullOrEmpty(Eval("Nme_Anexo").ToString()) ? "    <i class=\"fa fa-paperclip fa-1x\" aria-hidden=\"true\"></i>" : "") %>
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
                                                    Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "Vlr_Pretensao_Salarial")).ToString("N2") %>' CausesValidation="False"
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
                                        <telerik:GridTemplateColumn HeaderText="TE" HeaderTooltip="Tempo de Experiência" Visible="False">
                                            <ItemTemplate>
                                                <div class="experiencia_curriculo_teste">
                                                    <asp:LinkButton ID="lblExperienciaCurriculo" runat="server" ToolTip="Tempo Experiência"
                                                        CssClass="nome_descricao_curriculo" Text='<%# DataBinder.Eval(Container.DataItem, "Des_Experiencia") %>'
                                                        CausesValidation="False" CommandName="MostrarModal">
                                                    </asp:LinkButton>
                                                </div>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Ações">
                                            <ItemTemplate>
                                                <div id="divIcones" runat="server" class="icones_pesquisa_curriculo icones">
                                                    <asp:LinkButton ID="lbtnChamarAgora" runat="server" CommandName="ChamarAgora" CssClass="espacamento linkChamarAgoraGrid" CausesValidation="false" ToolTip="Chamar Agora" OnClientClick="trackEvent('ResultadoPesquisaCurriculoCurriculoss', 'clickAcao', 'ChamarAgora');">Chamar Agora</asp:LinkButton>
                                                    <asp:LinkButton ID="imgMensagem" class="fa fa-envelope espacamento" runat="server" CommandName="EnviarMensagem" CausesValidation="false" ToolTip="Enviar Mensagem" OnClientClick="trackEvent('ResultadoPesquisaCurriculoCurriculoss', 'clickAcao', 'EnviarMensagem');"></asp:LinkButton>
                                                    <asp:LinkButton ID="imgCurriculo" class="fa fa-reply-all espacamento" runat="server" CausesValidation="false" CommandName="EnviarCurriculo" ToolTip="Enviar CV" OnClientClick="trackEvent('ResultadoPesquisaCurriculoCurriculoss', 'clickAcao', 'EnviarCV');"></asp:LinkButton>
                                                    <asp:LinkButton ID="btiCompleto" class="fa fa-eye espacamento" runat="server" CausesValidation="false" ToolTip="Ver CV" href='<%# RetornarURL(Eval("Des_Funcao").ToString(), Eval("Nme_Cidade").ToString(), Eval("Sig_Estado").ToString(), Convert.ToInt32(Eval("Idf_Curriculo"))) %>' Target="_blank" OnClientClick="trackEvent('ResultadoPesquisaCurriculoCurriculoss', 'clickAcao', 'VerCV');"></asp:LinkButton>
                                                    <asp:ImageButton ID="btiInformacoes" runat="server" class="espacamento" AlternateText="Informações" CausesValidation="false" CommandName="Informacoes" ToolTip="" Visible="false" ImageUrl="/img/icone_informacoes.png" OnClientClick="trackEvent('ResultadoPesquisaCurriculoCurriculoss', 'clickAcao', 'Informacoes');" />
                                                    <asp:LinkButton ID="btiExcluirCurriculo" class="fa fa-times espacamento" runat="server" CausesValidation="false" CommandName="RemoverCurriculo" ToolTip="" Visible='true' OnClientClick="trackEvent('ResultadoPesquisaCurriculoCurriculoss', 'clickAcao', 'ExcluirCV');"></asp:LinkButton>
                                                    <asp:LinkButton ID="btiAssociar" class="fa fa-folder-open-o espacamento" runat="server" ToolTip="Associar à uma Vaga" CommandName="Associar" Visible="False" CausesValidation="false" OnClientClick="trackEvent('ResultadoPesquisaCurriculoCurriculoss', 'clickAcao', 'AssociarAVaga');"></asp:LinkButton>
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

                            <%--</div>--%>

                            <%-- INICIO -- RESULTADO CURRICULOS COM DIPONIBILIDADE DE TRABALHAR NA CIDADE PESQUISADA--%>
                            <asp:Panel runat="server" ID="pnlDisponibilidadeCidade" Visible="false">
                                <h1>
                                    <asp:Label runat="server" ID="lblDisponibilidadeCidade" CssClass=""></asp:Label>
                                </h1>

                                <telerik:RadGrid AlternatingItemStyle-CssClass="alt_row" ID="gvDisponibilidadeCidade"
                                    runat="server" AllowPaging="True" AllowCustomPaging="True"
                                    CssClass="gridview_padrao pesquisa_curriculo" OnItemDataBound="gvResultadoPesquisa_ItemDataBound"
                                    OnItemCommand="gvResultadoPesquisa_ItemCommand" OnPageIndexChanged="gvResultadoPesquisa_PageIndexChanged"
                                    OnItemCreated="gvResultadoPesquisa_ItemCreated" OnColumnCreated="gvResultadoPesquisa_ColumnCreated"
                                    Skin="Office2007" GridLines="None" PageSize="20" OnDataBound="gvResultadoPesquisa_DataBound">
                                    <ClientSettings EnablePostBackOnRowClick="true" AllowExpandCollapse="true" AllowGroupExpandCollapse="false">
                                    </ClientSettings>
                                    <PagerStyle Mode="NextPrevAndNumeric" PagerTextFormat=" {4} Currículos {2} a {3} de {5}"
                                        Position="TopAndBottom" EnableSEOPaging="False" PageSizeLabelText="Quantidade de cvs" />
                                    <AlternatingItemStyle CssClass="alt_row" />
                                    <MasterTableView DataKeyNames="Idf_Curriculo">
                                        <GroupByExpressions>
                                            <telerik:GridGroupByExpression>
                                                <SelectFields>
                                                    <telerik:GridGroupByField FieldAlias="DentroPerfil" FieldName="Dentro_Perfil"></telerik:GridGroupByField>
                                                </SelectFields>
                                                <GroupByFields>
                                                    <telerik:GridGroupByField FieldName="Dentro_Perfil" SortOrder="Descending"></telerik:GridGroupByField>
                                                </GroupByFields>
                                            </telerik:GridGroupByExpression>
                                        </GroupByExpressions>
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
                                                                    ImageUrl='<%# RetornarUrlFoto(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "Num_CPF"))) %>'
                                                                    ID="rbiThumbnail" ResizeMode="None" AutoAdjustImageControlSize="False" />
                                                            </div>
                                                            <br />
                                                            <div class="codigo_curriculo">
                                                                Código CV:
                                                            <%# Eval("Idf_Curriculo")%>
                                                            </div>
                                                            <div>
                                                                Último Acesso em:
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
                                                            <div id="divUltimoSalario" runat="server" class="ultimo_salario" visible="false">
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
                                                            <br />
                                                            <asp:Panel runat="server" ID="pnlMinhasExperiencias" Visible="False">
                                                                Minhas experiências:
                                                            <br />
                                                                <asp:Repeater ID="rptFuncaoPretendida" runat="server" OnItemDataBound="rptFuncaoPretendida_ItemDataBound">
                                                                    <ItemTemplate>
                                                                        <asp:Literal runat="server" ID="ltFuncaoPretendida" />
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                            </asp:Panel>
                                                        </div>
                                                        <div class="botoes_resultado_pesquisa icones">
                                                            <asp:LinkButton ID="lnkAtualizarCurriculo" runat="server" CssClass="espacamento completo btn-defaut" CausesValidation="false" CommandName="AtualizarCurriculo">
                                                             <i class="fa fa-exclamation-triangle"></i>Solicitar Atualização
                                                            </asp:LinkButton>

                                                            <asp:LinkButton ID="lbtiChamarAgora" runat="server" CssClass="espacamento completo btn-defaut" CausesValidation="false" CommandName="ChamarAgora">
                                                            <i class="fa fa-bell"></i> Chamar Agora
                                                            </asp:LinkButton>
                                                            <asp:HyperLink ID="btiCvCompleto" runat="server" CssClass="espacamento completo btn-defaut" Target="_blank"
                                                                CausesValidation="false" NavigateUrl='<%#RetornarURL(Eval("Des_Funcao").ToString(), Eval("Nme_Cidade").ToString(), Eval("Sig_Estado").ToString(), Convert.ToInt32(Eval("Idf_Curriculo"))) %>'>
                                                            <i class="fa fa-eye"></i> Ver CV
                                                            </asp:HyperLink>
                                                            <asp:LinkButton runat="server" ID="btiDownload" CssClass="espacamento completo btn-defaut btiDownloadCls" CommandName="DownloadAnexo" CausesValidation="false" Visible="false">
                                                            <%--<i class="fa fa-download"></i>--%> <i class="fa fa-paperclip" aria-hidden="true"></i> Ver Anexo
                                                            </asp:LinkButton>

                                                            <a style="display: none;" class="espacamento completo btn-defaut" id="btn_link_sine_<%#Eval("Idf_Curriculo")%>" onclick="javascript:__doPostBack('ver_cv_sine','<%#Eval("Idf_Curriculo")%>')">
                                                                <i class="fa fa-download"></i>Ver Anexo 
                                                            </a>

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
                                                    <asp:CheckBox ID="ckbHeaderItem" runat="server" AutoPostBack="True" OnCheckedChanged="ckbHeaderItem_CheckedChanged1" />
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
                                            <telerik:GridTemplateColumn HeaderText="Avaliação " HeaderTooltip="Avaliação/SMS">
                                                <ItemTemplate>
                                                    <div class="icone_avaliacao_sms">
                                                        <span class="icone_avaliacao_teste">
                                                            <Componentes:BalaoSaibaMais ID="bsmAvaliacao" runat="server" ToolTipText='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Des_Avaliacao").ToString()) ? String.Empty : (DataBinder.Eval(Container.DataItem, "Des_Avaliacao").ToString().Length <= 140 ?  DataBinder.Eval(Container.DataItem, "Des_Avaliacao").ToString() : String.Format("{0}...", DataBinder.Eval(Container.DataItem, "Des_Avaliacao").ToString().Substring(0,140)))  %>'
                                                                ToolTipTitle="Avaliação" CssClassLabel="balao_saiba_mais" TargetControlID="imgAvaliacao" Visible='<%# !String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Des_Avaliacao").ToString()) %>' />
                                                            <asp:LinkButton ID="imgAvaliacao" runat="server" AlternateText="Avaliação" CausesValidation="false"
                                                                CommandArgument='<%# Eval("Avaliacao") %>' Visible='<%# (!String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Avaliacao").ToString())) %>' />
                                                        </span>
                                                        <span class="icone_sms_teste" style="position: relative;">
                                                            <%--   <Componentes:BalaoSaibaMais ID="bsmSMS" runat="server" ToolTipText='<%# Check("Des_SMS") ? (!string.IsNullOrWhiteSpace(Eval("Des_SMS").ToString()) ? Eval("Des_SMS").ToString() : string.Empty) : string.Empty %>'
                                                            ToolTipTitle="SMS" CssClassLabel="balao_saiba_mais" TargetControlID="lblSMS" />--%>
                                                            <div class="tooltipC balao">
                                                                <div id="litSMSTooltip3" runat="server">
                                                                    SMS
                                                                    <br />
                                                                    <%# Check("Des_SMS") ? (!string.IsNullOrWhiteSpace(Eval("Des_SMS").ToString()) ? Eval("Des_SMS").ToString() : string.Empty) : string.Empty %>
                                                                </div>
                                                                <asp:Label runat="server" ID="lblSMS3" class="fa fa-paper-plane fa-2x label_sms" Visible='<%# Check("Des_SMS") && !string.IsNullOrWhiteSpace(Eval("Des_SMS").ToString()) %>'>
                                                          
                                                                </asp:Label>
                                                            </div>
                                                        </span>
                                                    </div>

                                                    <asp:Panel runat="server" ID="pnlSTC" Visible='<%# Check("Pertence_origem") && Convert.ToBoolean(Eval("Pertence_origem")) %>'>
                                                        <i class="fa fa-check fa-2x" data-toggle="tooltip" title="CV cadastrado no site da empresa."></i>
                                                    </asp:Panel>

                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Nome">
                                                <ItemTemplate>
                                                    <div class="nome_curriculo">
                                                        <asp:LinkButton ID="lblNomeCurriculo" runat="server" CssClass=" nome_descricao_curriculo_padrao"
                                                            CausesValidation="False" CommandName="MostrarModal">
                                                            <asp:Label ID="lblQuemVisualiza" runat="server" Visible="false"></asp:Label>
                                                            <%# RetornarNome( Eval("Nme_Pessoa").ToString(), Convert.ToBoolean(Eval("VIP")))  + (!String.IsNullOrEmpty(Eval("Nme_Anexo").ToString()) ? "    <i class=\"fa fa-paperclip fa-1x\" aria-hidden=\"true\"></i>" : "") %>
                                                        </asp:LinkButton>
                                                        <asp:Panel runat="server" ID="pnlPCD" CssClass="ico_pcd" Visible='<%# Check("Idf_Deficiencia") && (Eval("Idf_Deficiencia") != DBNull.Value && Convert.ToInt32(Eval("Idf_Deficiencia")) != 0) %>'>
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
                                                        Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "Vlr_Pretensao_Salarial")).ToString("N2") %>' CausesValidation="False"
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
                                            <telerik:GridTemplateColumn HeaderText="TE" HeaderTooltip="Tempo de Experiência" Visible="False">
                                                <ItemTemplate>
                                                    <div class="experiencia_curriculo_teste">
                                                        <asp:LinkButton ID="lblExperienciaCurriculo" runat="server" ToolTip="Tempo Experiência"
                                                            CssClass="nome_descricao_curriculo" Text='<%# DataBinder.Eval(Container.DataItem, "Des_Experiencia") %>'
                                                            CausesValidation="False" CommandName="MostrarModal">
                                                        </asp:LinkButton>
                                                    </div>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Ações">
                                                <ItemTemplate>
                                                    <div id="divIcones" runat="server" class="icones_pesquisa_curriculo icones">
                                                        <asp:LinkButton ID="lbtnChamarAgora" runat="server" CommandName="ChamarAgora" CssClass="fa fa-bell espacamento" CausesValidation="false" ToolTip="Chamar Agora" OnClientClick="trackEvent('ResultadoPesquisaCurriculoCurriculoss', 'clickAcao', 'ChamarAgora');"></asp:LinkButton>
                                                        <asp:LinkButton ID="imgMensagem" class="fa fa-envelope espacamento" runat="server" CommandName="EnviarMensagem" CausesValidation="false" ToolTip="Enviar Mensagem" OnClientClick="trackEvent('ResultadoPesquisaCurriculoCurriculoss', 'clickAcao', 'EnviarMensagem');"></asp:LinkButton>
                                                        <asp:LinkButton ID="imgCurriculo" class="fa fa-reply-all espacamento" runat="server" CausesValidation="false" CommandName="EnviarCurriculo" ToolTip="Enviar CV" OnClientClick="trackEvent('ResultadoPesquisaCurriculoCurriculoss', 'clickAcao', 'EnviarCV');"></asp:LinkButton>
                                                        <asp:LinkButton ID="btiCompleto" class="fa fa-eye espacamento" runat="server" CausesValidation="false" ToolTip="Ver CV" href='<%# RetornarURL(Eval("Des_Funcao").ToString(), Eval("Nme_Cidade").ToString(), Eval("Sig_Estado").ToString(), Convert.ToInt32(Eval("Idf_Curriculo"))) %>' Target="_blank" OnClientClick="trackEvent('ResultadoPesquisaCurriculoCurriculoss', 'clickAcao', 'VerCV');"></asp:LinkButton>
                                                        <asp:ImageButton ID="btiInformacoes" runat="server" class="espacamento" AlternateText="Informações" CausesValidation="false" CommandName="Informacoes" ToolTip="" Visible="false" ImageUrl="/img/icone_informacoes.png" OnClientClick="trackEvent('ResultadoPesquisaCurriculoCurriculoss', 'clickAcao', 'Informacoes');" />

                                                        <asp:LinkButton ID="btiExcluirCurriculo" class="fa fa-times espacamento" runat="server" CausesValidation="false" CommandName="RemoverCurriculo" ToolTip="Excluir" Visible="false" OnClientClick="trackEvent('ResultadoPesquisaCurriculoCurriculoss', 'clickAcao', 'ExcluirCV');"></asp:LinkButton>
                                                        <asp:LinkButton ID="btiAssociar" class="fa fa-folder-open-o espacamento" runat="server" ToolTip="Associar à uma Vaga" CommandName="Associar" Visible="False" CausesValidation="false" OnClientClick="trackEvent('ResultadoPesquisaCurriculoCurriculoss', 'clickAcao', 'AssociarAVaga');"></asp:LinkButton>
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
                                <telerik:RadGrid AlternatingItemStyle-CssClass="alt_row" Visible="false" ID="RadGrid2"
                                    runat="server" AllowPaging="True" AllowCustomPaging="True"
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
                                        <GroupByExpressions>
                                            <telerik:GridGroupByExpression>
                                                <SelectFields>
                                                    <telerik:GridGroupByField FieldAlias="DentroPerfil" FieldName="Dentro_Perfil"></telerik:GridGroupByField>
                                                </SelectFields>
                                                <GroupByFields>
                                                    <telerik:GridGroupByField FieldName="Dentro_Perfil" SortOrder="Descending"></telerik:GridGroupByField>
                                                </GroupByFields>
                                            </telerik:GridGroupByExpression>
                                        </GroupByExpressions>
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
                                                                <%# RetornarNome( Eval("Nme_Pessoa").ToString(), Convert.ToBoolean(Eval("VIP")))  %>
                                                            </div>
                                                            <div class="borda_branca">
                                                                <telerik:RadBinaryImage runat="server" CssClass="btiFoto foto_dentro_borda_branca"
                                                                    ImageUrl='<%# RetornarUrlFoto(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "Num_CPF"))) %>'
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
                                                            <div id="divUltimoSalario" runat="server" class="ultimo_salario" visible="false">
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
                                                            <br />
                                                            <asp:Panel runat="server" ID="pnlMinhasExperiencias" Visible="False">
                                                                Minhas experiências:
                                                            <br />
                                                                <asp:Repeater ID="rptFuncaoPretendida" runat="server" OnItemDataBound="rptFuncaoPretendida_ItemDataBound">
                                                                    <ItemTemplate>
                                                                        <asp:Literal runat="server" ID="ltFuncaoPretendida" />
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                            </asp:Panel>
                                                        </div>
                                                        <div class="botoes_resultado_pesquisa icones">
                                                            <asp:LinkButton ID="lbtiChamarAgora" runat="server" CssClass="espacamento completo btn-defaut" CausesValidation="false" CommandName="ChamarAgora">
                                                            <i class="fa fa-bell"></i> Chamar Agora
                                                            </asp:LinkButton>
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
                                            <telerik:GridTemplateColumn HeaderText="Campanha" DataField="Campanha">
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <ItemTemplate>
                                                    <asp:ImageButton CssClass="espacamento" ID="imgCampanha" runat="server" AlternateText="Campanha"
                                                        ToolTip="Campanha" Title="Campanha" ImageUrl="/img/img_icone_check_16x16.png" Visible='<%# Convert.ToBoolean(Eval("Flg_Campanha")) %>'
                                                        Enabled="false" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Avaliação" HeaderTooltip="Avaliação/SMS">
                                                <ItemTemplate>
                                                    <div class="icone_avaliacao_sms">
                                                        <span class="icone_avaliacao_teste">
                                                            <Componentes:BalaoSaibaMais ID="bsmAvaliacao" runat="server" ToolTipText='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Des_Avaliacao").ToString()) ? String.Empty : (DataBinder.Eval(Container.DataItem, "Des_Avaliacao").ToString().Length <= 140 ?  DataBinder.Eval(Container.DataItem, "Des_Avaliacao").ToString() : String.Format("{0}...", DataBinder.Eval(Container.DataItem, "Des_Avaliacao").ToString().Substring(0,140))) %>'
                                                                ToolTipTitle="Avaliação" CssClassLabel="balao_saiba_mais" TargetControlID="imgAvaliacao" Visible='<%# !String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Des_Avaliacao").ToString()) %>' />
                                                            <asp:LinkButton ID="imgAvaliacao" runat="server" AlternateText="Avaliação" CausesValidation="false"
                                                                CommandArgument='<%# Eval("Avaliacao") %>' Visible='<%# (!String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Avaliacao").ToString())) %>' />
                                                        </span>
                                                        <span class="icone_sms_teste" style="position: relative;">
                                                            <%--   <Componentes:BalaoSaibaMais ID="bsmSMS" runat="server" ToolTipText='<%# Check("Des_SMS") ? (!string.IsNullOrWhiteSpace(Eval("Des_SMS").ToString()) ? Eval("Des_SMS").ToString() : string.Empty) : string.Empty %>'
                                                            ToolTipTitle="SMS" CssClassLabel="balao_saiba_mais" TargetControlID="lblSMS" />--%>
                                                            <div class="tooltipC balao">
                                                                <div id="litSMSTooltip4" runat="server">
                                                                    SMS
                                                                    <br />
                                                                    <%# Check("Des_SMS") ? (!string.IsNullOrWhiteSpace(Eval("Des_SMS").ToString()) ? Eval("Des_SMS").ToString() : string.Empty) : string.Empty %>
                                                                </div>
                                                                <asp:Label runat="server" ID="lblSMS4" class="fa fa-paper-plane fa-2x label_sms" Visible='<%# Check("Des_SMS") && !string.IsNullOrWhiteSpace(Eval("Des_SMS").ToString()) %>'>
                                                          
                                                                </asp:Label>
                                                            </div>
                                                        </span>
                                                    </div>

                                                    <asp:Panel runat="server" ID="pnlSTC" Visible='<%# Check("Pertence_origem") && Convert.ToBoolean(Eval("Pertence_origem")) %>'>
                                                        <i class="fa fa-check fa-2x" data-toggle="tooltip" title="CV cadastrado no site da empresa."></i>
                                                    </asp:Panel>

                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Nome">
                                                <ItemTemplate>
                                                    <div class="nome_curriculo">
                                                        <asp:LinkButton ID="lblNomeCurriculo" runat="server" CssClass=" nome_descricao_curriculo_padrao"
                                                            CausesValidation="False" CommandName="MostrarModal">
                                                            <asp:Label ID="lblQuemVisualiza" runat="server" Visible="false"></asp:Label>
                                                            <%# RetornarNome( Eval("Nme_Pessoa").ToString(), Convert.ToBoolean(Eval("VIP")))  + (!String.IsNullOrEmpty(Eval("Nme_Anexo").ToString()) ? "    <i class=\"fa fa-paperclip fa-1x\" aria-hidden=\"true\"></i>" : "") %>
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
                                                        Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "Vlr_Pretensao_Salarial")).ToString("N2") %>' CausesValidation="False"
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
                                            <telerik:GridTemplateColumn HeaderText="TE" HeaderTooltip="Tempo de Experiência" Visible="False">
                                                <ItemTemplate>
                                                    <div class="experiencia_curriculo_teste">
                                                        <asp:LinkButton ID="lblExperienciaCurriculo" runat="server" ToolTip="Tempo Experiência"
                                                            CssClass="nome_descricao_curriculo" Text='<%# DataBinder.Eval(Container.DataItem, "Des_Experiencia") %>'
                                                            CausesValidation="False" CommandName="MostrarModal">
                                                        </asp:LinkButton>
                                                    </div>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Ações">
                                                <ItemTemplate>
                                                    <div id="divIcones" runat="server" class="icones_pesquisa_curriculo icones">
                                                        <asp:LinkButton ID="lbtnChamarAgora" runat="server" CommandName="ChamarAgora" CssClass="espacamento linkChamarAgoraGrid" CausesValidation="false" ToolTip="Chamar Agora" OnClientClick="trackEvent('ResultadoPesquisaCurriculoCurriculoss', 'clickAcao', 'ChamarAgora');">Chamar Agora</asp:LinkButton>
                                                        <asp:LinkButton ID="imgMensagem" class="fa fa-envelope espacamento" runat="server" CommandName="EnviarMensagem" CausesValidation="false" ToolTip="Enviar Mensagem" OnClientClick="trackEvent('ResultadoPesquisaCurriculoCurriculoss', 'clickAcao', 'EnviarMensagem');"></asp:LinkButton>
                                                        <asp:LinkButton ID="imgCurriculo" class="fa fa-reply-all espacamento" runat="server" CausesValidation="false" CommandName="EnviarCurriculo" ToolTip="Enviar CV" OnClientClick="trackEvent('ResultadoPesquisaCurriculoCurriculoss', 'clickAcao', 'EnviarCV');"></asp:LinkButton>
                                                        <asp:LinkButton ID="btiCompleto" class="fa fa-eye espacamento" runat="server" CausesValidation="false" ToolTip="Ver CV" href='<%# RetornarURL(Eval("Des_Funcao").ToString(), Eval("Nme_Cidade").ToString(), Eval("Sig_Estado").ToString(), Convert.ToInt32(Eval("Idf_Curriculo"))) %>' Target="_blank" OnClientClick="trackEvent('ResultadoPesquisaCurriculoCurriculoss', 'clickAcao', 'VerCV');"></asp:LinkButton>
                                                        <asp:ImageButton ID="btiInformacoes" runat="server" class="espacamento" AlternateText="Informações" CausesValidation="false" CommandName="Informacoes" ToolTip="" Visible="false" ImageUrl="/img/icone_informacoes.png" OnClientClick="trackEvent('ResultadoPesquisaCurriculoCurriculoss', 'clickAcao', 'Informacoes');" />
                                                        <asp:LinkButton ID="btiExcluirCurriculo" class="fa fa-times espacamento" runat="server" CausesValidation="false" CommandName="RemoverCurriculo" ToolTip="" Visible='true' OnClientClick="trackEvent('ResultadoPesquisaCurriculoCurriculoss', 'clickAcao', 'ExcluirCV');"></asp:LinkButton>
                                                        <asp:LinkButton ID="btiAssociar" class="fa fa-folder-open-o espacamento" runat="server" ToolTip="Associar à uma Vaga" CommandName="Associar" Visible="False" CausesValidation="false" OnClientClick="trackEvent('ResultadoPesquisaCurriculoCurriculoss', 'clickAcao', 'AssociarAVaga');"></asp:LinkButton>
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
                            </asp:Panel>
                            <%--FIM - RESULTADO CURRICULOS COM DIPONIBILIDADE DE TRABALHAR NA CIDADE PESQUISADA--%>
                        </ContentTemplate>
                    </asp:UpdatePanel>
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
                <asp:Button ID="btnCampanhaRecrutamento" runat="server" CssClass="botao_padrao" Text="Campanha de Recrutamento"
                    CausesValidation="false" OnClick="btnCampanhaRecrutamento_Click" />
                <asp:Button ID="btnAnunciarVaga" runat="server" CssClass="botao_padrao" Text="Anunciar Vaga"
                    CausesValidation="false" OnClick="btnAnunciarVaga_Click" />
                <asp:Button ID="btnBuscaAvancada" runat="server" CssClass="botao_padrao" Text="Busca Avançada"
                    CausesValidation="false" OnClick="btnBuscaAvancada_Click" />
                <asp:Button ID="btnExcluirCurriculos" runat="server" CssClass="botao_padrao" Text="Excluir Currículos"
                    CausesValidation="false" OnClick="btnExcluirCurriculos_Click" Visible="false" />
                <asp:Button ID="btnNext" runat="server" Style="display: none;" OnClick="btnNext_OnClick" />
                <asp:Button ID="btnPrevious" runat="server" Style="display: none;" OnClick="btnPrevious_OnClick" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- FIM: Painel botoes -->
    <asp:Panel ID="pnlConteudo" runat="server">
        <uc:ConfirmacaoExclusao ID="ucConfirmacaoExclusao" runat="server" />
    </asp:Panel>

    <asp:UpdatePanel ID="upBotoesLaterais" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="DivBotoesLateraisFechado">
                <a onclick="AbrirBoxLateral()" class="linksAcoes">
                    <i class="fa fa-arrow-left"></i>
                </a>
            </div>

            <div style="display: none;" id="DivBotoesLateraisAberto">
                <div id="DivTopoBotoesLaterais">
                    <a onclick="FecharBoxLateral()" class="linksAcoes">
                        <i class="fa fa-arrow-right"></i>Recolher Aba 
                    </a>
                </div>
                <p>
                    <asp:LinkButton runat="server" ID="lbtnEnviarMensagemLateral" OnClick="btnEnviarMensagem_Click" CssClass="btn-defaut">
                        <i class="fa fa-envelope"></i> Enviar Mensagem
                    </asp:LinkButton>
                </p>
                <p>
                    <asp:LinkButton runat="server" ID="lbtnEnviarCVLateral" OnClick="btnEnviarCurriculo_Click" CssClass="btn-defaut">
                        <i class="fa fa-paper-plane"></i> Enviar CV
                    </asp:LinkButton>
                </p>
                <p>
                    <asp:LinkButton runat="server" ID="lbtnVoltar" OnClick="btnVoltar_Click" CssClass="btn-defaut">
                        <i class="fa fa-arrow-left">Voltar</i>
                    </asp:LinkButton>
                </p>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

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

    <span id="spModal" style="display: none;">
        <div class="modal-backdrop fade in"></div>
    </span>

    <%--Inicio modal de filtros--%>
    <div id="filtrar_busca_cv-modal" class="modal fade in" tabindex="-1" role="dialog" aria-labelledby="filtrar_busca_cv-modal_label" style="overflow: scroll;" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" onclick="FecharModalFiltros();" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="filtrar_busca_cv-modal_label">Filtrar <strong>lista de currículos</strong></h4>
                    <img src="img/modal_filtro_candidato/filter-circle.png" id="filtrar_busca_cv-modal_img" />
                </div>
                <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="upModalFiltro">
                    <ContentTemplate>
                        <div class="modal-body bodyFiltros">

                            <div id="filtrar_busca_cv-modal_form" class="row Modal_Filtro_Candidatos">
                                <div class="form-group col-md-10 col-md-offset-1">
                                    <label for="exampleInputEmail1">Função</label>
                                    <Employer:ComboCheckbox runat="server" Filter="Contains" DropDownWidth="400" CssClass="checkbox_100" ID="ccFuncoes" EmptyMessage=""
                                        AutoPostBack="false">
                                    </Employer:ComboCheckbox>
                                </div>
                                <div class="form-group col-md-10 col-md-offset-1">
                                    <label for="exampleInputEmail1">Cidade</label>
                                    <Employer:ComboCheckbox runat="server" Filter="Contains" CssClass="checkbox_100" ID="ccCidade" AutoPostBack="false" EmptyMessage="">
                                    </Employer:ComboCheckbox>
                                </div>
                                <div class="form-group col-md-10 col-md-offset-1">
                                    <label for="filtrar_busca_cv-campo_idade_de">Idade</label><br />
                                    <div class="linha">
                                        <div class="filtroMargem">
                                            De
                                           <componente:AlfaNumerico ID="txtIdadeMinima" runat="server" Columns="2" MaxLength="2"
                                               CssClass="alinhar_container_campo" Obrigatorio="False" CssClassTextBox="textbox_padrao textbox_padrao-set-idade"
                                               ValorMinimo="0" ValorMaximo="100" ContemIntervalo="true" Tipo="Numerico"
                                               ClientValidationFunction="cvIdade_ClientValidationFunction" />

                                            Até
                                           <componente:AlfaNumerico ID="txtIdadeMaxima" runat="server" Columns="2" MaxLength="2"
                                               CssClass="alinhar_container_campo" Obrigatorio="False" CssClassTextBox="textbox_padrao textbox_padrao-set-idade"
                                               ValorMinimo="0" ValorMaximo="100" ContemIntervalo="true" Tipo="Numerico"
                                               ClientValidationFunction="cvIdade_ClientValidationFunction" />

                                            Anos
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group col-md-10 col-md-offset-1">
                                    <label for="exampleInputEmail1">Salario</label><br />
                                    <div class="linha">
                                        <div class="filtroMargem">
                                            De
                                            <componente:ValorDecimal ID="txtSalarioMinimo" runat="server" CasasDecimais="0"
                                                Obrigatorio="false" CssClassTextBox="textbox_padrao textbox_padrao-set-salario alinhar_container_campo" Columns="4" Placeholder="R$" />
                                            Até
                                            <componente:ValorDecimal ID="txtSalarioMaximo" runat="server" CasasDecimais="0"
                                                Obrigatorio="false" CssClassTextBox="textbox_padrao textbox_padrao-set-salario alinhar_container_campo" Columns="4" Placeholder="R$" />
                                            Reais
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group col-md-10 col-md-offset-1">
                                    <label for="filtrar_busca_cv-campo_escolaridade">Escolaridade</label>
                                    <Employer:ComboCheckbox ID="ccEscolaridade" runat="server" AutoPostBack="false" CssClass="dropdownFiltro"></Employer:ComboCheckbox>
                                </div>

                                <div class="col-md-3 col-md-offset-1">
                                    <label for="filtrar_busca_cv-campo_sexo">Sexo</label>
                                    <asp:DropDownList runat="server" ID="ddlSexo" CssClass="form-control dropdownFiltro"></asp:DropDownList>
                                </div>
                                <div class="col-md-3 col-md-offset-1">
                                    <label for="filtrar_busca_cv-campo_PCD" style="width:100%;" ><i class="fa fa-wheelchair"></i> PCD</label>
                                    <br />
                                    <asp:CheckBox runat="server" ID="chkPCDFiltro"   />
                                </div>

                                 <div class="col-md-3">
                                    <label for="filtrar_busca_cv-campo_PCD" style="width:100%;" >Respostas Corretas</label>
                                    <br />
                                    <asp:CheckBox runat="server" ID="chkRespostaCorreta"   />
                                </div>

                                <div class="form-group col-md-10 col-md-offset-1">
                                    <label for="filtrar_busca_cv-campo-palavra_chave" style="width: 100%;">
                                        Palavra-chave  
                                        <Componentes:BalaoSaibaMais ID="bsmPalavraChave" runat="server"
                                            ToolTipText='Informe palavras para INCLUIR candidatos no resultado da pesquisa. Utilize buscas com "aspas" para termos exatos e separado por vírgula para várias palavras. Ex.: "power point", Excel, word'
                                            Text="Saiba mais" ToolTipTitle="Palavra-Chave:" Style="margin-left: 6px;" CssClassLabel="balao_saiba_mais" ShowOnMouseover="true" />
                                    </label>

                                    <textarea class="form-control" runat="server" id="txtPalavraChave" rows="3"></textarea>

                                </div>
                            </div>
                        </div>
                        <div class="modal-footer rodapeFiltro">
                            <!-- <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button> -->
                            <asp:Button runat="server" ID="bntAplicarFiltro" CssClass="btn btn-success" Text="Aplicar Filtros" OnClick="bntAplicarFiltro_Click" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </div>
    </div>
    <%--Fim modal de filtros--%>
</asp:Content>

