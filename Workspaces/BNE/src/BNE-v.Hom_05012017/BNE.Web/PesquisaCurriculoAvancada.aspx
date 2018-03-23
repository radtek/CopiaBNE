<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true" CodeBehind="PesquisaCurriculoAvancada.aspx.cs" Inherits="BNE.Web.PesquisaCurriculoAvancada" %>

<%@ Register TagPrefix="uc2" Src="~/UserControls/FuncaoEmPesquisaCurriculo.ascx" TagName="PesquisaCurriculoCustomizacao" %>
<%@ Register TagPrefix="ucIdioma" Src="~/UserControls/IdiomaEmPesquisaCurriculo.ascx" TagName="PesquisaCurriculoIdioma" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <link href="css/local/Bootstrap/bootstrap.css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/PesquisaCurriculoAvancada.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicScript runat="server" Src="/js/local/PesquisaCurriculoAvancada.js" type="text/javascript" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <h1>
        <asp:Label ID="lblTituloPesquisaAvancada" runat="server" Text="Pesquisa Avançada" /></h1>
    <br />
    <asp:HiddenField
        ID="hfInstituicaoGraduacao"
        runat="server" />
    <asp:HiddenField
        ID="hfInstituicaoOutrosCursos"
        runat="server" />
    <asp:UpdatePanel
        ID="upPnlCamposPesquisa"
        runat="server"
        UpdateMode="Conditional"
        ChildrenAsTriggers="False">
        <ContentTemplate>
            <asp:Panel
                ID="pnlCamposPesquisa"
                runat="server"
                CssClass="painel_padrao pesquisa_avancada" DefaultButton="btnBuscar">
                <%-- Início da fonte de dados de currículos--%>
                <asp:Panel ID="pnl_stc_fonte" runat="server" CssClass="row">
                    <div class="span10">
                        <div class="accordion" id="accordion_stc_fonte">
                            <div class="accordion-group-bne">
                                <div class="accordion-heading-bne accordion-heading-bne-bg-green">
                                    <a class="accordion-toggle-bne" data-toggle="collapse" data-parent="#accordion_stc_fonte" href="#accordion_stc_fonte_content">
                                        <h3 class="colorheader">Fonte de dados<i class="fa fa-sort-desc pull-right"></i></h3>
                                    </a>
                                </div>
                                <div id="accordion_stc_fonte_content" class="accordion-body accordion-body-set collapse in">
                                    <div class="accordion-inner-bne">
                                        <div class="span10">
                                            <div style="width: 100%; height: 30px;">
                                                <label><strong>Desejo buscar por currículo </strong></label>
                                            </div>
                                            <asp:UpdatePanel ID="FonteSTCUpdatePanel" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:CheckBox ID="chk_stc_fonte_bne" runat="server" Checked="true" Text="No BNE"
                                                        CssClass="stc_fonte_checkbox" OnCheckedChanged="chk_stc_fonte_bne_CheckedChanged" AutoPostBack="true" />
                                                    <asp:CheckBox ID="chk_stc_fonte_meus_cv" runat="server" Checked="true" Text="Nos meus CVs"
                                                        CssClass="stc_fonte_checkbox" OnCheckedChanged="chk_stc_fonte_meus_cv_CheckedChanged" AutoPostBack="true" />

                                                    <br />
                                                    <asp:Panel ID="pnl_stc_fonte_alert" Visible="false" CssClass="alert alert-warning stc_fonte_alert" runat="server" role="alert">
                                                        Marque pelo menos uma opção.
                                                    </asp:Panel>

                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <%--Fim da fonte de dados de currículos--%>
                <%--Start Accordion Definir Pretensão do Candidato--%>
                <div class="row">
                    <div class="span10">
                        <div class="accordion" id="accordionPretensao">
                            <div class="accordion-group-bne">
                                <div class="accordion-heading-bne accordion-heading-bne-bg-green">
                                    <a id="pnPretensao" class="accordion-toggle-bne" data-toggle="collapse" data-parent="#accordionPretensao" href="#collapsePretenCand">
                                        <h3 class="colorheader">Pretensão do Candidato<i class="fa fa-sort-desc pull-right"></i></h3>
                                    </a>
                                </div>
                                <div id="collapsePretenCand" class="accordion-body accordion-body-set collapse in">
                                    <div class="accordion-inner-bne">
                                        <div class="span10">
                                            <span class="span5">
                                                <!-- Linha Função e Estagiário -->
                                                <asp:UpdatePanel ID="upEstagiarioFuncaoArea" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <uc2:PesquisaCurriculoCustomizacao Obrigatorio="false" ID="ucEstagiarioFuncao" ValidationGroup="PesquisaAvancada" runat="server" />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnLimpar" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                                <!-- FIM: Função e Estagiário -->
                                            </span>
                                        </div>
                                        <div class="span10">
                                            <span class="span5">
                                                <!-- Linha Disponibilidade de Trabalho -->
                                                <asp:Label ID="lblDisponibilidadeTrabalho" runat="server" CssClass="label_principal-set" AssociatedControlID="rcbDisponibilidade"><strong>Disponibilidade de Trabalho</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <Employer:ComboCheckbox ID="rcbDisponibilidade" EmptyMessage="Qualquer" runat="server" />
                                                <Componentes:BalaoSaibaMais ID="bsmChkDisponibilidade" runat="server" CssClassLabel="balao_saiba_mais" ToolTipText="Se você procura candidatos com disponibilidade para trabalho específico. Selecione nesse campo uma ou mais opções e tenha apenas os candidatos que atendem aos seus requisitos." ToolTipTitle="Disponibilidade para Trabalhar:" Text="Saiba Mais" ShowOnMouseover="true" />
                                                <!-- FIM: Linha Disponibilidade de Trabalho -->
                                            </span>

                                            <%--<span class="span4">
                                                <asp:Label ID="lblTempoExperiencia" runat="server" CssClass="label_principal-set" AssociatedControlID="txtTempoExperiencia"><strong>Tempo de Experiência</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <componente:AlfaNumerico ID="txtTempoExperiencia" runat="server" Columns="2" MaxLength="3" CssClassTextBox="textbox_padrao" Obrigatorio="false" Tipo="Numerico" ValidationGroup="PesquisaAvancada" />
                                                <span runat="server" associatedcontrolid="txtTempoExperiencia" class="set-label">meses</span>
                                            </span>--%>
                                        </div>

                                        <%--Mostrar qdo for marcado Quero contratar estagiário--%>
                                        <div class="escolaridade-estagiario" id="escolaridade-estagiario">
                                            <div class="span10">
                                                <span class="span5">
                                                    <!-- Linha Nível-->
                                                    <asp:Label ID="Label2" runat="server" AssociatedControlID="rcbNivelEscolaridadeEstagiario" CssClass="label_principal-set"><strong>Escolaridade Mínima</strong></asp:Label>
                                                    <div class="clearfix"></div>
                                                    <telerik:RadComboBox ID="rcbNivelEscolaridadeEstagiario" CssClass="campoEscolaridade" runat="server"></telerik:RadComboBox>
                                                    <!-- FIM: Linha Nível -->
                                                </span>
                                                <span class="span4"></span>
                                            </div>
                                            <div class="span10">
                                                <span class="span5">
                                                    <!-- Linha Técnico/Graduação -->
                                                    <asp:Label ID="lblTituloTecnicoGraduacaoEstag" runat="server" CssClass="label_principal-set" AssociatedControlID="txtTituloTecnicoGraduacaoEstag"><strong>Técnico / Graduação em</strong></asp:Label>
                                                    <div class="clearfix"></div>
                                                    <asp:TextBox ID="txtTituloTecnicoGraduacaoEstag" runat="server" CssClass="textbox_padrao" MaxLength="100"></asp:TextBox>
                                                    <AjaxToolkit:AutoCompleteExtender ID="aceTituloTecnicoGraduacaoEstag" runat="server" TargetControlID="txtTituloTecnicoGraduacaoEstag" ServiceMethod="ListarCursoNivel3"></AjaxToolkit:AutoCompleteExtender>
                                                    <!-- FIM: Técnico/Graduação -->
                                                </span>
                                                <span class="span4">
                                                    <!--Linha Instituicao Graduação-->
                                                    <asp:Label ID="lblInstituicaoTecnicoGraduacaoEstag" runat="server" AssociatedControlID="txtInstituicaoTecnicoGraduacaoEstag"><strong>Instituição</strong></asp:Label>
                                                    <div class="clearfix"></div>
                                                    <asp:TextBox ID="txtInstituicaoTecnicoGraduacaoEstag" runat="server" CssClass="textbox_padrao" Columns="30" MaxLength="100"></asp:TextBox>
                                                    <AjaxToolkit:AutoCompleteExtender ID="aceInstituicaoTecnicoGraduacaoestag" runat="server" TargetControlID="txtInstituicaoTecnicoGraduacaoEstag" UseContextKey="false" ServiceMethod="ListarSiglaNomeFonte" OnClientItemSelected="InstituicaoGraduacao"></AjaxToolkit:AutoCompleteExtender>
                                                </span>
                                            </div>
                                        </div>
                                        <%--FIM: Mostrar qdo for marcado Quero contratar estagiário--%>
                                        <div class="span10">
                                            <span class="span5">
                                                <!-- Linha Salário -->
                                                <asp:Label ID="lblSalariob" runat="server" CssClass="label_principal-set" AssociatedControlID="txtSalario"><strong>Salário</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <asp:Label ID="lblSalario" AssociatedControlID="txtSalario" runat="server" CssClass="label_principal-set"><strong>Mínimo</strong></asp:Label>
                                                <componente:ValorDecimal ID="txtSalario" runat="server" CasasDecimais="0" ValorMaximo="20000" Obrigatorio="false" CssClassTextBox="textbox_padrao textbox_padrao-set-salario" ValorAlteradoClient="txtSalarioDe_ValorAlterado" />

                                                <asp:Label ID="lblSalarioAte" AssociatedControlID="txtSalarioAte" runat="server" CssClass="label_principal-set"><strong>Máximo</strong></asp:Label>
                                                <componente:ValorDecimal ID="txtSalarioAte" runat="server" CasasDecimais="0" ValorMaximo="20000" CssClassTextBox="textbox_padrao textbox_padrao-set-salario" Obrigatorio="false" />
                                                <span runat="server" associatedcontrolid="txtSalarioAte" class="set-label">reais</span>

                                                <!-- FIM: Linha Salário -->
                                            </span>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <%--End Accordion Definir Pretensão do Candidato--%>
                <%--Start Accordion Definir Região e Localidade--%>
                <div class="row">
                    <div class="span10">
                        <div class="accordion" id="accordionRegiaoLocalidade">
                            <div class="accordion-group-bne">
                                <div class="accordion-heading-bne accordion-heading-bne-bg-green">
                                    <a class="accordion-toggle-bne" data-toggle="collapse" data-parent="#accordionRegiaoLocalidade" href="#collapseRegLoc">
                                        <h3 class="colorheader">Região e Localidade<i class="fa fa-sort-desc pull-right"></i></h3>
                                    </a>
                                </div>
                                <div id="collapseRegLoc" class="accordion-body accordion-body-set collapse in">
                                    <div class="accordion-inner-bne">
                                        <div class="span10">
                                            <span class="span3 linha">
                                                <!-- Linha Cidade-->
                                                <asp:Label runat="server" ID="lblCidade" CssClass="label_principa-set" AssociatedControlID="txtCidadePesquisa"><strong>Cidade</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <asp:CustomValidator ID="cvCidadePesquisa" runat="server" ErrorMessage="Cidade Inválida." ClientValidationFunction="cvCidade_Validate" ControlToValidate="txtCidadePesquisa" ValidationGroup="PesquisaAvancada"></asp:CustomValidator>
                                                <asp:TextBox ID="txtCidadePesquisa" runat="server" CssClass="textbox_padrao" Columns="25" OnTextChanged="txtCidadePesquisa_TextChanged" AutoPostBack="True"></asp:TextBox>
                                                <!--FIM: Linha Cidade-->
                                            </span>
                                            <span class="span3">
                                                <!--Linha Estado-->
                                                <asp:Label ID="lblUF" runat="server" CssClass="label_principal-set" AssociatedControlID="rcbEstado"><strong>Estado</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <telerik:RadComboBox ID="rcbEstado" CssClass="campoEstado" runat="server"></telerik:RadComboBox>
                                                <!--FIM: Linha Estado-->
                                            </span>
                                            <asp:UpdatePanel runat="server" ID="upBairroZona" UpdateMode="Conditional" ChildrenAsTriggers="False">
                                                <ContentTemplate>

                                                    <asp:Panel runat="server" CssClass="span3" Visible="True" ID="pnlBairroZona">
                                                        <%--mostrar bairro por região qdo for São Paulo--%>
                                                        <asp:Label ID="Label5" AssociatedControlID="txtBairro" runat="server" CssClass="label_principal-set"><strong>Bairro</strong></asp:Label>
                                                        <div class="clearfix"></div>
                                                        <div style="position: inherit;">

                                                            <span class="span4 bairro">
                                                                <label class="label_principal-set">Centro</label>
                                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <Employer:ComboCheckbox runat="server" ID="rcbBairroZonaCentral" CssClass="padding-set-bairro" DropDownWidth="350" AllowCustomText="False" EmptyMessage="Selecione..." OnClientDropDownClosing="ClientDropDownClosing">
                                                                        </Employer:ComboCheckbox>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>

                                                            </span>
                                                            <span class="span4 bairro">
                                                                <label class="label_principal-set">Zona Norte</label>
                                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <Employer:ComboCheckbox runat="server" ID="rcbBairroZonaNorte" CssClass="padding-set-bairro" DropDownWidth="350" AllowCustomText="False" EmptyMessage="Selecione..." OnClientDropDownClosing="ClientDropDownClosing">
                                                                        </Employer:ComboCheckbox>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </span>
                                                            <span class="span4 bairro">
                                                                <label class="label_principal-set">Zona Sul</label>
                                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <Employer:ComboCheckbox runat="server" ID="rcbBairroZonaSul" CssClass="padding-set-bairro" DropDownWidth="350" AllowCustomText="False" EmptyMessage="Selecione..." OnClientDropDownClosing="ClientDropDownClosing">
                                                                        </Employer:ComboCheckbox>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </span>
                                                            <span class="span4 bairro">
                                                                <label class="label_principal-set">Zona Leste</label>
                                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <Employer:ComboCheckbox runat="server" ID="rcbBairroZonaLeste" CssClass="padding-set-bairro" DropDownWidth="350" AllowCustomText="False" EmptyMessage="Selecione..." OnClientDropDownClosing="ClientDropDownClosing">
                                                                        </Employer:ComboCheckbox>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </span>
                                                            <span class="span4 bairro">
                                                                <label class="label_principal-set">Zona Oeste</label>
                                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <Employer:ComboCheckbox runat="server" ID="rcbBairroZonaOeste" CssClass="padding-set-bairro" DropDownWidth="350" AllowCustomText="False" EmptyMessage="Selecione..." OnClientDropDownClosing="ClientDropDownClosing">
                                                                        </Employer:ComboCheckbox>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </span>
                                                        </div>
                                                        <%--FIM:mostrar bairro por região qdo for São Paulo--%>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:UpdatePanel runat="server" ID="upBairroTexto" UpdateMode="Conditional" ChildrenAsTriggers="False">
                                                <ContentTemplate>
                                                    <asp:Panel runat="server" CssClass="span3" Visible="True" ID="pnlBairroTexto">
                                                        <!-- Linha Bairro Default qdo não for São Paulo  -->
                                                        <asp:Label ID="lblBairro" AssociatedControlID="txtBairro" runat="server" Text="Bairro" CssClass="label_principal-set" />
                                                        <div class="clearfix"></div>
                                                        <asp:UpdatePanel ID="upTxtBairro" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                            <ContentTemplate>
                                                                <componente:AlfaNumerico ID="txtBairro" runat="server" MaxLength="100" CssClassTextBox="textbox_padrao textbox_padrao-set-endereco" Obrigatorio="false" ValidationGroup="PesquisaAvancada" ExpressaoValidacao="^[A-Z,a-z,0-9,ÁáÂâÃãÉéÊêÍíÓóÔôÕõÚúÛûÇç\,']{0,}( ){0,}[A-Z,a-z,0-9,ÁáÂâÃãÉéÊêÍíÓóÔôÕõÚúÛûÇç\,']{0,}(( ){0,}[A-Z,a-z,0-9,ÁáÂâÃãÉéÊêÍíÓóÔôÕõÚúÛûÇç\,']{0,})*$" Enabled="True" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        <asp:UpdatePanel ID="upBtiMapaBairro" runat="server" UpdateMode="Conditional" RenderMode="Inline"></asp:UpdatePanel>
                                                        <!-- FIM: Linha Bairro Default qdo não for São Paulo -->
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="span10" style="position: relative;" id="bairro-endereco">
                                            <span class="span3">
                                                <!-- Linha Endereço -->
                                                <asp:Label ID="lblLogradouro" AssociatedControlID="txtLogradouro" runat="server" CssClass="label_principal-set"><strong>Endereço</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <componente:AlfaNumerico ID="txtLogradouro" CssClassTextBox="textbox_padrao textbox_padrao-set-endereco" runat="server" Obrigatorio="false" MaxLength="50" ValidationGroup="PesquisaAvancada" ExpressaoValidacao="^[A-Z,a-z,0-9,ÁáÂâÃãÉéÊêÍíÓóÔôÕõÚúÛûÇç\,']{0,}( ){0,}[A-Z,a-z,0-9,ÁáÂâÃãÉéÊêÍíÓóÔôÕõÚúÛûÇç\,']{0,}(( ){0,}[A-Z,a-z,0-9,ÁáÂâÃãÉéÊêÍíÓóÔôÕõÚúÛûÇç\,']{0,})*$" Enabled="True" />
                                                <span style="display: none">
                                                    <asp:Label ID="lblAvisoCidadeEndereco" runat="server"><strong>Selecione uma cidade</strong></asp:Label>
                                                </span>
                                                <!-- FIM: Linha Endereço -->
                                            </span>
                                            <span class="span3">
                                                <!-- Linha Faixa CEP -->
                                                <asp:Label ID="lblFaixaCep" runat="server" AssociatedControlID="txtFaixaCep" CssClass="label_principal-set"><strong>Faixa de CEP</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <componente:CEP ID="txtFaixaCep" runat="server" Columns="8" MaxLength="8" Obrigatorio="False" ValorAlteradoClient="txtFaixaCep_ValorAlteradoClient" CssClassTextBox="textbox_padrao textbox_padrao-set-cep textbox_padrao-set" />
                                                <asp:Label ID="lblFaixaCepAte" AssociatedControlID="txtFaixaCepAte" runat="server" Text="Até" CssClass="label_principal-set" />
                                                <asp:CustomValidator ID="cvFuncaoFaixaCepAte" runat="server" ValidationGroup="PesquisaAvancada" ControlToValidate="txtTeste" ValidateEmptyText="true" ClientValidationFunction="cvFuncaoFaixaCepAte_Validate"></asp:CustomValidator>
                                                <asp:TextBox ID="txtTeste" Style="display: none" runat="server"></asp:TextBox>
                                                <componente:CEP ID="txtFaixaCepAte" runat="server" Columns="8" MaxLength="8" Obrigatorio="False" ValorAlteradoClient="txtFaixaCepAte_ValorAlteradoClient" CssClassTextBox="textbox_padrao textbox_padrao-set-cep textbox_padrao-set" />
                                                <!-- FIM: Linha Faixa CEP-->
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <%--End Accordion Definir Região e Localidade--%>
                <%--Start Accordion Definir Palavra-chave--%>
                <div class="row">
                    <div class="span10">
                        <div class="accordion" id="accordionPalavraChave">
                            <div class="accordion-group-bne">
                                <div class="accordion-heading-bne accordion-heading-bne-bg-green">
                                    <a class="accordion-toggle-bne" data-toggle="collapse" data-parent="#accordionPalavraChave" href="#collapseOne">
                                        <h3 class="colorheader">Definir Palavra-Chave <i class="fa fa-sort-desc pull-right"></i></h3>
                                    </a>
                                </div>
                                <div id="collapseOne" class="accordion-body accordion-body-set collapse in">
                                    <div class="accordion-inner-bne">
                                        <div class="span10">
                                            <span class="span3">
                                                <!-- Linha Incluir Palavra-Chave -->
                                                <asp:Label ID="lblPalavraChave" runat="server" CssClass="label_principal-set" AssociatedControlID="txtPalavraChave"><strong>Incluir Palavra-Chave</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <componente:AlfaNumerico ID="txtPalavraChave" runat="server" CssClassTextBox="textbox_padrao" Obrigatorio="false" Columns="25" ValidationGroup="PesquisaAvancada" />
                                                <Componentes:BalaoSaibaMais ID="bsmPalavraChave" runat="server" ToolTipText='Informe palavras para INCLUIR candidatos no resultado da pesquisa. Utilize buscas com "aspas" para termos exatos e separado por vírgula para várias palavras. Ex.: "power point", Excel, word' Text="Saiba mais" ToolTipTitle="Palavra-Chave:" CssClassLabel="balao_saiba_mais" ShowOnMouseover="true" />
                                                <!-- FIM: Linha Incluir Palavra-Chave -->
                                            </span>
                                            <span class="span3">
                                                <asp:Label ID="lbBuscaPalavraChave" runat="server" CssClass="label_principal-set" AssociatedControlID="rcbBuscaPalavraChave"><strong>Busca em</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <Employer:ComboCheckbox ID="rcbBuscaPalavraChave" EmptyMessage="Qualquer" runat="server" ShowCheckAllButton="false"  />
                                                <Componentes:BalaoSaibaMais ID="BalaoSaibaMaiPalavraChave" runat="server" CssClassLabel="balao_saiba_mais" ToolTipText="Selecione nesse campo uma ou mais opções e tenha apenas os candidatos que atendem aos seus requisitos." ToolTipTitle="Buscar em:" Text="Saiba Mais" ShowOnMouseover="true" />
                                            </span>
                                            <span class="span3">
                                                <!-- Linha Excluir Palavra-Chave -->
                                                <asp:Label ID="lblExcluirPalavraChave" runat="server" CssClass="label_principal-set" AssociatedControlID="txtPalavraChave"><strong>Excluir Palavra-Chave</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <componente:AlfaNumerico ID="txtExcluirPalavraChave" runat="server" CssClassTextBox="textbox_padrao" Obrigatorio="false" Columns="25" ValidationGroup="PesquisaAvancada" autocomplete="on" />
                                                <Componentes:BalaoSaibaMais ID="bsmExcluirPalavraChave" runat="server" ToolTipText='Informe palavras para EXCLUIR candidatos no resultado da pesquisa. Utilize buscas com "aspas" para termos exatos e separado por vírgula para várias palavras. Ex.: "power point", Excel, word' Text="Saiba mais" ToolTipTitle="Excluir Palavra-Chave:" CssClassLabel="balao_saiba_mais" ShowOnMouseover="True" />
                                                <!-- FIM: Linha Excluir Palavra-Chave -->
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>

                </div>
                <%--End Accordion Definir Palavra-chave--%>
                <%--Start Accordion Definir Formação e Escolaridade--%>
                <div class="row">
                    <div class="span10">
                        <div class="accordion" id="accordionFormacaoEscolaridade">
                            <div class="accordion-group-bne">
                                <div class="accordion-heading-bne accordion-heading-bne-bg-green">
                                    <a class="accordion-toggle-bne" data-toggle="collapse" data-parent="#accordionFormacaoEscolaridade" href="#collapseFormEsc">
                                        <h3 class="colorheader">Formação e Escolaridade <i class="fa fa-sort-desc pull-right"></i></h3>
                                    </a>
                                </div>
                                <div id="collapseFormEsc" class="accordion-body accordion-body-set collapse in">
                                    <div class="accordion-inner-bne">
                                        <div class="span10">
                                            <span class="span3">
                                                <!-- Linha Nível-->
                                                <asp:Label ID="lblNivel" runat="server" AssociatedControlID="rcbNivelEscolaridade" CssClass="label_principal-set"><strong>Escolaridade Mínima</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <telerik:RadComboBox ID="rcbNivelEscolaridade" CssClass="campoEscolaridade" runat="server"></telerik:RadComboBox>
                                                <!-- FIM: Linha Nível -->
                                            </span>
                                               <span class="span1"></span>
                                            <span class="span5">
                                                <%--Linha Idioma--%>
                                               
                                                <!--FIM: Linha Idioma-->
                                                 <ucIdioma:PesquisaCurriculoIdioma Obrigatorio="false" ID="ucPesquisaIdioma" ValidationGroup="PesquisaAvancada" runat="server" />
                                            </span>
                                        </div>
                                        <div class="span10">
                                            <span class="span5">
                                                <!-- Linha Técnico/Graduação -->
                                                <asp:Label ID="lblTituloTecnicoGraduacao" runat="server" CssClass="label_principal-set" AssociatedControlID="txtTituloTecnicoGraduacao"><strong>Técnico / Graduação em</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <asp:TextBox ID="txtTituloTecnicoGraduacao" runat="server" CssClass="textbox_padrao" MaxLength="100"></asp:TextBox>
                                                <AjaxToolkit:AutoCompleteExtender ID="aceTituloTecnicoGraduacao" runat="server" TargetControlID="txtTituloTecnicoGraduacao" ServiceMethod="ListarCursoNivel3"></AjaxToolkit:AutoCompleteExtender>
                                                <!-- FIM: Técnico/Graduação -->
                                            </span>
                                            <span class="span4 span4-set-formEsc">
                                                <!--Linha Instituicao Graduação-->
                                                <asp:Label ID="lblInstituicaoTecnicoGraduacao" runat="server" AssociatedControlID="txtInstituicaoTecnicoGraduacao"><strong>Instituição</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <asp:TextBox ID="txtInstituicaoTecnicoGraduacao" runat="server" CssClass="textbox_padrao" Columns="30" MaxLength="100"></asp:TextBox>
                                                <AjaxToolkit:AutoCompleteExtender ID="aceInstituicaoTecnicoGraduacao" runat="server" TargetControlID="txtInstituicaoTecnicoGraduacao" UseContextKey="false" ServiceMethod="ListarSiglaNomeFonte" OnClientItemSelected="InstituicaoGraduacao"></AjaxToolkit:AutoCompleteExtender>
                                                <!--FIM: Linha Instituicao Graduação-->
                                                <Componentes:BalaoSaibaMais ID="bsmTxtInstituicaoTecnicoGraduacao" runat="server" CssClassLabel="balao_saiba_mais" Text="Saiba mais" ToolTipTitle="Técnico/Graduação em:" ToolTipText="Se você procura por candidatos com Técnico, Tecnólogo ou Graduado. Informe neste campo o nome da formação desejada. Exemplo: Psicologia. <BR />Se desejar candidatos de uma determinada instituição, informe no campo ao lado o nome da Instituição de ensino. Exemplo: Universidade Federal do Paraná." ShowOnMouseover="true" />
                                            </span>
                                        </div>
                                        <div class="span10">
                                            <span class="span5">
                                                <!-- Linha Outros Cursos -->
                                                <asp:Label ID="lblCurso" runat="server" CssClass="label_principal-set" AssociatedControlID="txtTituloCurso"><strong>Outros Cursos</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <asp:TextBox ID="txtTituloCurso" runat="server" CssClass="textbox_padrao" MaxLength="100"></asp:TextBox>
                                                <AjaxToolkit:AutoCompleteExtender ID="aceTituloOutrosCursos" runat="server" TargetControlID="txtTituloCurso" ServiceMethod="ListarCursoNivel4"></AjaxToolkit:AutoCompleteExtender>
                                                <!-- FIM: Outros Cursos -->
                                            </span>
                                            <span class="span4 span4-set-formEsc {width: 350px;}">
                                                <!--Linha Instituicao Outros Cursos-->
                                                <asp:Label ID="lblInstituicao" runat="server" AssociatedControlID="txtInstituicao"><strong>Instituição</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <asp:TextBox ID="txtInstituicao" runat="server" CssClass="textbox_padrao" Columns="30" MaxLength="100"></asp:TextBox>
                                                <AjaxToolkit:AutoCompleteExtender ID="aceInstituicaoOutrosCursos" runat="server" TargetControlID="txtInstituicao" UseContextKey="false" ServiceMethod="ListarSiglaNomeFonte" OnClientItemSelected="InstituicaoOutrosCursos"></AjaxToolkit:AutoCompleteExtender>
                                                <Componentes:BalaoSaibaMais ID="bsmTxtInstituicaoOutrosCursos" runat="server" CssClassLabel="balao_saiba_mais" ToolTipText="Se você procura por candidatos com Especialização, Pós graduação ou Cursos distintos. Informe neste campo o nome do curso desejado. Exemplo: Gestão Estratégica de Pessoas. <BR />Se  desejar candidatos de uma determinada instituição, informe no campo ao lado o nome da Instituição de ensino. Exemplo: Pontifícia Universidade Católica do Paraná." ToolTipTitle="Outros Cursos:" Text="Saiba mais" ShowOnMouseover="true" />
                                                <!--FIM: Linha Instituicao Outros Cursos-->
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <%--End Accordion Definir Formação e Escolaridade--%>
                <%--Start Accordion Definir Experiência do Candidato--%>
                <div class="row">
                    <div class="span10">
                        <div class="accordion" id="accordionExperiencia">
                            <div class="accordion-group-bne">
                                <div class="accordion-heading-bne accordion-heading-bne-bg-green">
                                    <a class="accordion-toggle-bne" data-toggle="collapse" data-parent="#accordionExperiencia" href="#collapseExpCand">
                                        <h3 class="colorheader">Experiência do Candidato<i class="fa fa-sort-desc pull-right"></i></h3>
                                    </a>
                                </div>
                                <div id="collapseExpCand" class="accordion-body accordion-body-set collapse in">
                                    <div class="accordion-inner-bne">
                                        <div class="span10">
                                            <span class="span5">
                                                <!-- Linha Empresa -->
                                                <asp:Label ID="lblEmpresa" CssClass="label_principal-set" runat="server" AssociatedControlID="txtEmpresa"><strong>Nome Empresa</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <asp:UpdatePanel ID="upTxtEmpresa" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                    <ContentTemplate>
                                                        <componente:AlfaNumerico ID="txtEmpresa" runat="server" ValidationGroup="PesquisaAvancada" Obrigatorio="false" CssClassTextBox="textbox_padrao" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <!--FIM: Linha Empresa -->
                                            </span>
                                            <span class="span4">
                                                <!-- Linha Atividade Empresa -->
                                                <asp:Label ID="lblAtividadeEmpresa" CssClass="label_principal-set" runat="server" AssociatedControlID="rcbAtividadeEmpresa"><strong>Atividade da Empresa</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <telerik:RadComboBox ID="rcbAtividadeEmpresa" runat="server"></telerik:RadComboBox>
                                                <!-- FIM: Linha Atividade Empresa -->
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <%--End Accordion Definir Experiência do Candidato--%>
                <%--Start Accordion Definir Características--%>
                <div class="row">
                    <div class="span10">
                        <div class="accordion" id="accordionCaracteristicas">
                            <div class="accordion-group-bne">
                                <div class="accordion-heading-bne accordion-heading-bne-bg-green">
                                    <a class="accordion-toggle-bne" data-toggle="collapse" data-parent="#accordionCaracteristicas" href="#collapseCaractPessoais">
                                        <h3 class="colorheader">Características<i class="fa fa-sort-desc pull-right"></i></h3>
                                    </a>
                                </div>
                                <div id="collapseCaractPessoais"  class="accordion-body accordion-body-set collapse in">
                                    <div id="divFiltrosPessoais" class="accordion-inner-bne" runat="server">
                                        <div class="span10">
                                            <span class="span3">
                                                <!-- Linha Idade -->
                                                <asp:Label ID="Label1" runat="server" AssociatedControlID="txtIdadeDe"><strong>Idade</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <asp:Label ID="lblIdade" runat="server" AssociatedControlID="txtIdadeDe" CssClass="label_principal-set"><strong>Mínima</strong></asp:Label>
                                                <componente:AlfaNumerico ID="txtIdadeDe" runat="server" Obrigatorio="False" CssClassTextBox="textbox_padrao textbox_padrao-set-idade" Columns="2" MaxLength="2" Tipo="Numerico" ValorMinimo="0" ValorMaximo="100" ContemIntervalo="true" ValidationGroup="PesquisaAvancada" />

                                                <asp:Label ID="lblIdadeAte" AssociatedControlID="txtIdadeAte" runat="server" CssClass="label_principal-set"><strong>Máxima</strong></asp:Label>
                                                <componente:AlfaNumerico ID="txtIdadeAte" runat="server" Columns="2" MaxLength="2" CssClass="alinhar_container_campo" Obrigatorio="False" CssClassTextBox="textbox_padrao textbox_padrao-set-idade" ValorMinimo="0" ValorMaximo="100" ContemIntervalo="true" Tipo="Numerico" ValidationGroup="PesquisaAvancada" ClientValidationFunction="cvIdade_ClientValidationFunction" />
                                                <span runat="server" associatedcontrolid="txtIdadeAte" class="set-label">anos</span>
                                                <!-- FIM: Linha Idade-->
                                            </span>
                                            <span class="span3">
                                                <!-- Linha Sexo -->
                                                <asp:Label runat="server" ID="lblSexo" CssClass="label_principal-set" AssociatedControlID="rcbSexo"><strong>Sexo</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <telerik:RadComboBox ID="rcbSexo" runat="server"></telerik:RadComboBox>
                                                <!-- FIM: Linha Sexo -->
                                            </span>
                                            <span class="span3">
                                                <!-- Linha Estado Civil -->
                                                <asp:Label ID="lblEstadoCivil" CssClass="label_principal-set" runat="server" AssociatedControlID="rcbEstadoCivil"><strong>Estado Civil</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <telerik:RadComboBox ID="rcbEstadoCivil" runat="server"></telerik:RadComboBox>
                                                <!-- FIM: Linha Estado Civil -->
                                            </span>
                                        </div>
                                        <div class="span10">
                                            <span class="span3">
                                                <!-- Linha Raça -->
                                                <asp:Label ID="lblRaca" runat="server" CssClass="label_principal-set" AssociatedControlID="rcbRaca"><strong>Raça</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <telerik:RadComboBox ID="rcbRaca" runat="server"></telerik:RadComboBox>
                                                <!-- FIM: Linha Raça -->
                                            </span>
                                            <span class="span3">
                                                <!-- Linha Filhos -->
                                                <asp:Label ID="lblFilhos" runat="server" CssClass="label_principal-set" AssociatedControlID="rcbFilhos"><strong>Filhos</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <telerik:RadComboBox ID="rcbFilhos" runat="server"></telerik:RadComboBox>
                                                <!-- FIM: Linha Filhos -->
                                            </span>
                                            <span class="span3">
                                                <!-- Linha PCD -->
                                                <asp:Label ID="lblPCD" runat="server" CssClass="label_principal-set" AssociatedControlID="rcbPCD"><strong>Pessoa com Deficiência (PCD - <i class="fa fa-wheelchair fa-1x"></i>
)</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <telerik:RadComboBox ID="rcbPCD" runat="server"></telerik:RadComboBox>
                                                <!-- FIM: Linha PCD -->
                                            </span>
                                        </div>
                                        <div class="span10">
                                            <span class="span3">
                                                <!-- Linha Habilitação-->
                                                <asp:Label ID="lblHabilitacao" CssClass="label_principal-set" runat="server" AssociatedControlID="rcbHabilitacao"><strong>Habilitação</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <telerik:RadComboBox ID="rcbHabilitacao" runat="server"></telerik:RadComboBox>
                                            </span>
                                            <span class="span3">
                                                <asp:Label ID="lblVeiculo" CssClass="label_principal-set" runat="server" AssociatedControlID="rcbVeiculo"><strong>Veículo</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <telerik:RadComboBox ID="rcbVeiculo" runat="server"></telerik:RadComboBox>
                                                <!-- FIM: Linha Habilitação -->
                                            </span>
                                        </div>
                                    </div>
                                    <div id="divFiltrosPessoaisAviso" visible="false" runat="server" class="accordion-body accordion-body-set collapse in">
                                        <p>Filtros disponíveis somente para empresas logadas.</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <%--End Accordion Definir Características--%>
                <%--Start Accordion Definir Candidato Específico--%>
                <div class="row">
                    <div class="span10">
                        <div class="accordion" id="accordionCandidatoEspecifico">
                            <div class="accordion-group-bne-blue">
                                <div class="accordion-heading-bne accordion-heading-bne-bg-green">
                                    <a class="accordion-toggle-bne" data-toggle="collapse" data-parent="#accordionCandidatoEspecifico" href="#collapseCandidato">
                                        <h3 class="colorheader">Candidato Específico <i class="fa fa-sort-desc pull-right"></i></h3>
                                    </a>
                                </div>
                                <div id="collapseCandidato" class="accordion-body accordion-body-set collapse in">
                                    <div id="divFiltrosCandidato" class="accordion-inner-bne" runat="server">
                                        <div class="span10">
                                            <span class="span4 span4-set-nome">
                                                <!--Nome, CPF ou Código-->
                                                <asp:Label ID="lblNomeCpfCodigo" AssociatedControlID="txtNomeCpfCodigo" runat="server" CssClass="label_principal-set"><strong>Nome, CPF ou Código CV</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <componente:AlfaNumerico ID="txtNomeCpfCodigo" runat="server" Columns="30" MaxLength="100" Obrigatorio="false" CssClassTextBox="textbox_padrao textbox_padrao-set-nome" />
                                                <Componentes:BalaoSaibaMais ID="bsmNomeCpfCodigo" runat="server" ToolTipText="Pesquise mais de um CV adicionando ; entre os códigos de CV." Text="Saiba mais" ToolTipTitle="Código:" CssClassLabel="balao_saiba_mais" ShowOnMouseover="True" />
                                                <!-- FIM: Nome, CPF ou Código-->
                                            </span>
                                            <span class="span3">
                                                <!-- Linha E-Mail -->
                                                <asp:Label ID="lblEmail" AssociatedControlID="txtEmail" CssClass="label_principal-set" runat="server"><strong>E-mail</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <asp:TextBox ID="txtEmail" runat="server" Columns="30" MaxLength="50" CssClass="textbox_padrao textbox_padrao-set-email"></asp:TextBox>
                                                <!-- FIM: Linha E-Mail -->
                                            </span>
                                            <span class="span3 span3-set-telefone">
                                                <!-- Linha Telefone -->
                                                <asp:Label ID="lblTelefone" AssociatedControlID="txtTelefone" CssClass="label_principal-set" runat="server"><strong>Telefone</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <componente:Telefone ID="txtTelefone" CssClassTextBoxFone="textbox_padrao textbox_padrao-set-fone" CssClassTextBoxDDD="textbox_padrao textbox_padrao-set-ddd" Obrigatorio="false" runat="server" ValidationGroup="PesquisaAvancada" Tipo="FixoCelular" />
                                                <!-- FIM: Linha Telefone -->
                                            </span>
                                        </div>
                                    </div>
                                    <div id="divFiltrosCandidatoAviso" visible="false" runat="server" class="accordion-body accordion-body-set collapse in">
                                        <p>Filtros disponíveis somente para empresas logadas.</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <%--End Accordion Definir Candidato Específico--%>



                <%-- Início - Pesquisa pela avaliação do candidato --%>
                <div class="row">
	                <div class="span10">
		                <div class="accordion" id="pnl_avaliacao_candidato">
			                <div class="accordion-group-bne-blue">
				                <div class="accordion-heading-bne accordion-heading-bne-bg-green">
					                <a class="accordion-toggle-bne" data-toggle="collapse" data-parent="#pnl_avaliacao_candidato" href="#pnl_avaliacao_candidato_inner">
						                <h3 class="colorheader">Avaliação do Candidato <i class="fa fa-sort-desc pull-right"></i></h3>
					                </a>
				                </div>
				                <div id="pnl_avaliacao_candidato_inner" class="accordion-body accordion-body-set collapse in">
					                <div id="divAvaliacao" class="accordion-inner-bne" runat="server">

                                        <div class="span10">

                                            <span class="span4 span4-set-nome">
                                                <label class="label_principal-set"><strong>Ponto de Vista</strong></label>
                                                <span>
                                                    <Componentes:BalaoSaibaMais ID="BalaoSaibaMaisAvaliacao" runat="server" ToolTipText="Pesquisa pelo último ponto de vista cadastrado para o candidato." Text="" ToolTipTitle="" CssClassLabel="balao_saiba_mais" ShowOnMouseover="True" />
                                                </span>
                                                <div class="clearfix"></div>
                                                <ul id="carinhas">
                                                    <li>
                                                        <asp:CheckBox  ID="CheckAvaliacaoPositiva"  runat="server" Text="" />
                                                         <i class="fa fa-smile-o fa-2x"></i></span>
                                                    </li>
                                                    <li>
                                                        <asp:CheckBox  ID="CheckSemAvaliacao"  runat="server" Text="" />
                                                         <i class="fa fa-meh-o fa-2x"></i></span>
                                                    </li>
                                                    <li>
                                                        <asp:CheckBox  ID="CheckAvaliacaoNegativa"  runat="server" Text="" />
                                                        <i class="fa fa-frown-o fa-2x"></i></span>
                                                    </li>
                                                </ul>
                                           </span>



                                            <span class="span4 span4-set-nome">
                                                <label class="label_principal-set"><strong>Palavras chaves no comentário da avaliação</strong></label>
                                                <div class="clearfix"></div>
                                                <asp:TextBox runat="server" ID="TxtComentario" CssClass="TxtComentario"></asp:TextBox>
                                           </span>


                                       </div>

					                </div>
                                    <div id="divAvaliacaoAviso" visible="false" runat="server" class="accordion-body accordion-body-set collapse in">
                                        <p>Filtros disponíveis somente para empresas logadas.</p>
                                    </div>
				                </div>
			                </div>
		                </div>
	                </div>
                </div>
                <%-- Fim - Pesquisa pela avaliação do candidato --%>
                <div id="divSlowDown">
                    <asp:UpdatePanel
                        ID="upPnlBotoesFlutuantes"
                        runat="server"
                        UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel ID="pnlBotoes" runat="server">
                                <asp:LinkButton CssClass="mini_botao_pesquisa_avancada"
                                    ID="btiBuscarFlutuante"
                                    runat="server"
                                    OnClick="btnBuscar_Click"
                                    CausesValidation="True"
                                    ValidationGroup="PesquisaAvancada"><i class="fa fa-search"></i> Buscar</asp:LinkButton>
                                <asp:LinkButton CssClass="mini_botao_pesquisa_avancada"
                                    ID="btiLimparFlutuante"
                                    runat="server"
                                    OnClick="btiLimparFlutuante_Click"
                                    CausesValidation="false"><i class="fa fa-eraser"></i> Limpar</asp:LinkButton>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <asp:UpdatePanel
            ID="upPnlBotoesFixo"
            runat="server"
            UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel
                    ID="pnlBotoesFixo"
                    runat="server"
                    CssClass="painel_botoes">
                    <asp:Button
                        ID="btnBuscar"
                        CssClass="botao_padrao btnBuscar"
                        Text="Buscar"
                        runat="server"
                        OnClick="btnBuscar_Click"
                        CausesValidation="True"
                        ValidationGroup="PesquisaAvancada" />
                    <asp:Button
                        ID="btnLimpar"
                        CssClass="botao_padrao"
                        runat="server"
                        Text="Limpar"
                        OnClick="btnLimpar_Click" />
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="modal-anuncio-vaga" class="modal fade in hide" tabindex="-1" role="dialog" aria-labelledby="lblModalAnuncioVaga" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true" aria-label="Close" style="display: none;">×</button>
            <h4 class="modal-title" id="lblModalAnuncioVaga">ANUNCIAR VAGA</h4>
        </div>
        <div class="modal-body">
            <p>Deseja anunciar uma vaga com os filtros informados?</p>
        </div>
        <div class="modal-footer">
            <asp:CheckBox runat="server" ID="ckbAnunciarVagaNaoPerguntarNovamente" Checked="False" Text="Não mostrar novamente" />
            <asp:LinkButton runat="server" ID="btlAnunciarVagaNao" CssClass="btn btn-danger btn-large"
                CausesValidation="False" OnClick="btlAnunciarVagaNao_Click">Não
            </asp:LinkButton>
            <asp:LinkButton runat="server" ID="btlAnunciarVagaSim" CssClass="btn btn-success btn-large"
                CausesValidation="False" OnClick="btlAnunciarVagaSim_Click">Sim
            </asp:LinkButton>
        </div>
    </div>
    <script src="js/bootstrap/bootstrap.js"></script>
</asp:Content>
<asp:Content
    ID="Content3"
    ContentPlaceHolderID="cphRodape"
    runat="server">
</asp:Content>
