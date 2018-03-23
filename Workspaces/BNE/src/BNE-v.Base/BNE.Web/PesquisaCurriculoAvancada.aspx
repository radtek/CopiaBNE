<%@ Page Title=""
    Language="C#"
    MasterPageFile="~/Master/Principal.Master"
    AutoEventWireup="true"
    CodeBehind="PesquisaCurriculoAvancada.aspx.cs"
    Inherits="BNE.Web.PesquisaCurriculoAvancada" %>

<%@ Register TagPrefix="uc2" Src="~/UserControls/FuncaoEmPesquisaCurriculo.ascx" TagName="PesquisaCurriculoCustomizacao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/PesquisaCurriculoAvancada.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicScript runat="server" Src="/js/local/PesquisaCurriculoAvancada.js" type="text/javascript" />
</asp:Content>
<asp:Content
    ID="Content2"
    ContentPlaceHolderID="cphConteudo"
    runat="server">
    <h1>
        <asp:Label
            ID="lblTituloPesquisaAvancada"
            runat="server"
            Text="Pesquisa Avançada" />
    </h1>
    <asp:HiddenField
        ID="hfInstituicaoGraduacao"
        runat="server" />
    <asp:HiddenField
        ID="hfInstituicaoOutrosCursos"
        runat="server" />
    <asp:UpdatePanel
        ID="upPnlCamposPesquisa"
        runat="server"
        UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel
                ID="pnlCamposPesquisa"
                runat="server"
                CssClass="painel_padrao pesquisa_avancada" DefaultButton="btnBuscar">
                <div class="painel_padrao_topo">
                    &nbsp;
                </div>

                <asp:UpdatePanel ID="upEstagiarioFuncaoArea" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <uc2:PesquisaCurriculoCustomizacao Obrigatorio="false" ID="ucEstagiarioFuncao" ValidationGroup="PesquisaAvancada" runat="server" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnLimpar" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>

                <div class="linha">
                    <asp:Label ID="lblUF" runat="server" Text="Estado" CssClass="label_principal" AssociatedControlID="rcbEstado" />
                    <div class="container_campo">
                        <telerik:RadComboBox ID="rcbEstado" CssClass="campoEstado" runat="server">
                        </telerik:RadComboBox>
                    </div>
                </div>

                <!-- Linha Cidade-->
                <div class="linha">
                    <asp:Label runat="server" ID="lblCidade" Text="Cidade" CssClass="label_principal" AssociatedControlID="txtCidadePesquisa" />
                    <div class="container_campo">
                        <div>
                            <asp:CustomValidator
                                ID="cvCidade"
                                runat="server"
                                ErrorMessage="Cidade Inválida."
                                ClientValidationFunction="cvCidade_Validate"
                                ControlToValidate="txtCidadePesquisa"
                                ValidationGroup="PesquisaAvancada"></asp:CustomValidator>
                        </div>
                        <asp:TextBox
                            ID="txtCidadePesquisa"
                            runat="server"
                            OnTextChanged="txtCidadePesquisa_TextChanged"
                            CssClass="textbox_padrao"
                            Columns="25"></asp:TextBox>
                        <AjaxToolkit:AutoCompleteExtender
                            ID="aceCidade"
                            runat="server"
                            TargetControlID="txtCidadePesquisa"
                            ServiceMethod="ListarCidadesSiglaEstadoPorNomeParcialEstado">
                        </AjaxToolkit:AutoCompleteExtender>
                        <!-- FIM: Linha Cidade -->
                    </div>
                </div>

                <!-- Linha Incluir Palavra-Chave -->
                <div class="linha">
                    <asp:Label
                        ID="lblPalavraChave"
                        runat="server"
                        Text="Incluir Palavra-Chave"
                        CssClass="label_principal"
                        AssociatedControlID="txtPalavraChave"></asp:Label>
                    <div class="container_campo">
                        <componente:AlfaNumerico
                            ID="txtPalavraChave"
                            runat="server"
                            CssClassTextBox="textbox_padrao"
                            Obrigatorio="false"
                            Columns="25"
                            ValidationGroup="PesquisaAvancada" />
                    </div>
                    <Componentes:BalaoSaibaMais
                        ID="bsmPalavraChave"
                        runat="server"
                        ToolTipText="Nesse campo você pode informar atribuições e conhecimentos que deseja que o candidato possua. Exemplo: vendas, Excel"
                        Text="Saiba mais"
                        ToolTipTitle="Palavra-Chave:"
                        CssClassLabel="balao_saiba_mais"
                        ShowOnMouseover="true" />
                </div>
                <!-- FIM: Linha Incluir Palavra-Chave -->
                <!-- Linha Excluir Palavra-Chave -->
                <div class="linha">
                    <asp:Label
                        ID="lblExcluirPalavraChave"
                        runat="server"
                        Text="Excluir Palavra-Chave"
                        CssClass="label_principal"
                        AssociatedControlID="txtPalavraChave"></asp:Label>
                    <div class="container_campo">
                        <componente:AlfaNumerico
                            ID="txtExcluirPalavraChave"
                            runat="server"
                            CssClassTextBox="textbox_padrao"
                            Obrigatorio="false"
                            Columns="25"
                            ValidationGroup="PesquisaAvancada"
                            autocomplete="on" />
                    </div>
                    <Componentes:BalaoSaibaMais
                        ID="bsmExcluirPalavraChave"
                        runat="server"
                        ToolTipText="Informe palavras para remover candidatos no resultado da pesquisa"
                        Text="Saiba mais"
                        ToolTipTitle="Excluir Palavra-Chave:"
                        CssClassLabel="balao_saiba_mais" ShowOnMouseover="True" />
                </div>
                <!-- FIM: Linha Excluir Palavra-Chave -->
                <!-- Linha Nível-->
                <div class="linha">
                    <asp:Label
                        ID="lblNivel"
                        runat="server"
                        Text="Escolaridade Mínima"
                        AssociatedControlID="rcbNivel"
                        CssClass="label_principal"></asp:Label>
                    <div class="container_campo">
                        <telerik:RadComboBox
                            ID="rcbNivel"
                            CssClass="campoEscolaridade"
                            runat="server">
                        </telerik:RadComboBox>
                    </div>
                </div>
                <!-- FIM: Linha Nível -->
                <!-- Linha Sexo -->
                <div class="linha">
                    <asp:Label
                        runat="server"
                        ID="lblSexo"
                        Text="Sexo"
                        CssClass="label_principal"
                        AssociatedControlID="rcbSexo" />
                    <div class="container_campo">
                        <telerik:RadComboBox
                            ID="rcbSexo"
                            runat="server">
                        </telerik:RadComboBox>
                    </div>
                </div>
                <!-- FIM: Linha Sexo -->
                <!-- Linha Idade -->
                <div class="linha">
                    <asp:Label
                        ID="lblIdade"
                        runat="server"
                        Text="Idade Mínima"
                        AssociatedControlID="txtIdadeDe"
                        CssClass="label_principal">
                    </asp:Label>
                    <div class="container_campo">
                        <componente:AlfaNumerico
                            ID="txtIdadeDe"
                            runat="server"
                            Obrigatorio="False"
                            CssClassTextBox="textbox_padrao"
                            Columns="2"
                            MaxLength="2"
                            Tipo="Numerico"
                            ValorMinimo="0"
                            ValorMaximo="100"
                            ContemIntervalo="true"
                            ValidationGroup="PesquisaAvancada" />
                    </div>
                    <asp:Label
                        ID="lblIdadeAte"
                        AssociatedControlID="txtIdadeAte"
                        runat="server"
                        Text="Máxima" />
                    <div class="container_campo">
                        <componente:AlfaNumerico
                            ID="txtIdadeAte"
                            runat="server"
                            Columns="2"
                            MaxLength="2"
                            CssClass="alinhar_container_campo"
                            Obrigatorio="False"
                            CssClassTextBox="textbox_padrao"
                            ValorMinimo="0"
                            ValorMaximo="100"
                            ContemIntervalo="true"
                            Tipo="Numerico"
                            ValidationGroup="PesquisaAvancada"
                            ClientValidationFunction="cvIdade_ClientValidationFunction" />
                    </div>
                    <asp:Label
                        ID="Label1"
                        runat="server"
                        Text="anos"
                        AssociatedControlID="txtIdadeAte"></asp:Label>
                </div>
                <!-- FIM: Linha Idade-->
                <!-- Linha Salário -->
                <div class="linha">
                    <asp:Label
                        ID="lblSalario"
                        AssociatedControlID="txtSalario"
                        runat="server"
                        CssClass="label_principal"
                        Text="Salário Mínimo" />
                    <div class="container_campo">
                        <componente:ValorDecimal
                            ID="txtSalario"
                            runat="server"
                            CasasDecimais="0"
                            ValorMaximo="20000"
                            Obrigatorio="false"
                            CssClassTextBox="textbox_padrao"
                            ValorAlteradoClient="txtSalarioDe_ValorAlterado" />
                    </div>
                    <asp:Label
                        ID="lblSalarioAte"
                        AssociatedControlID="txtSalarioAte"
                        runat="server"
                        Text="Máximo" />
                    <div class="container_campo">
                        <componente:ValorDecimal
                            ID="txtSalarioAte"
                            runat="server"
                            CasasDecimais="0"
                            CssClass="alinhar_container_campo"
                            ValorMaximo="20000"
                            CssClassTextBox="textbox_padrao"
                            Obrigatorio="false" />
                    </div>
                    <asp:Label
                        ID="lblReais"
                        AssociatedControlID="txtSalarioAte"
                        runat="server"
                        Text="reais" />
                </div>
                <!-- FIM: Linha Salário -->
                <!--Linha Experiência-->
                <div class="linha">
                    <asp:Label
                        ID="lblTempoExperiencia"
                        runat="server"
                        Text="Tempo de Experiência"
                        CssClass="label_principal"
                        AssociatedControlID="txtTempoExperiencia"></asp:Label>
                    <div class="container_campo">
                        <componente:AlfaNumerico
                            ID="txtTempoExperiencia"
                            runat="server"
                            Columns="2"
                            MaxLength="3"
                            CssClassTextBox="textbox_padrao"
                            Obrigatorio="false"
                            Tipo="Numerico"
                            ValidationGroup="PesquisaAvancada" />
                        <asp:Label
                            ID="Label2"
                            AssociatedControlID="txtTempoExperiencia"
                            runat="server"
                            Text="meses">
                        </asp:Label>
                    </div>
                </div>
                <!--FIM: Linha Experiência-->
                <!-- Linha Idioma -->
                <div class="linha">
                    <asp:Label
                        runat="server"
                        ID="lblIdioma"
                        Text="Idioma"
                        CssClass="label_principal"
                        AssociatedControlID="rcbIdioma" />
                    <div class="container_campo">
                        <Employer:ComboCheckbox
                            ID="rcbIdioma"
                            EmptyMessage="Qualquer"
                            runat="server" />
                    </div>
                    <Componentes:BalaoSaibaMais
                        ID="bsmChkIdioma"
                        runat="server"
                        CssClassLabel="balao_saiba_mais"
                        ToolTipTitle="Idioma:"
                        Text="Saiba mais"
                        ToolTipText="Se você procura candidatos com idiomas. Selecione nesse campo uma ou mais opções e tenha apenas os candidatos com os idiomas escolhidos."
                        ShowOnMouseover="true" />
                </div>
                <!-- FIM: Linha Idioma -->
                <!--Nome, CPF ou Código-->
                <div class="linha">
                    <asp:Label
                        ID="lblNomeCpfCodigo"
                        AssociatedControlID="txtNomeCpfCodigo"
                        runat="server"
                        CssClass="label_principal"
                        Text="Nome, CPF ou Código CV" />
                    <div class="container_campo">
                        <componente:AlfaNumerico
                            ID="txtNomeCpfCodigo"
                            runat="server"
                            Columns="30"
                            MaxLength="100"
                            Obrigatorio="false"
                            CssClassTextBox="textbox_padrao" />
                        <Componentes:BalaoSaibaMais
                            ID="bsmNomeCpfCodigo"
                            runat="server"
                            ToolTipText="Pesquise mais de um CV adicionando ; entre os códigos de CV."
                            Text="Saiba mais"
                            ToolTipTitle="Código:"
                            CssClassLabel="balao_saiba_mais" ShowOnMouseover="True" />
                    </div>
                </div>
                <!-- FIM: Nome, CPF ou Código-->

                <!-- Linha Telefone -->
                <div class="linha">
                    <asp:Label
                        ID="lblTelefone"
                        AssociatedControlID="txtTelefone"
                        CssClass="label_principal"
                        runat="server"
                        Text="Telefone" />
                    <div class="container_campo">
                        <componente:Telefone
                            ID="txtTelefone"
                            CssClassTextBoxFone="textbox_padrao"
                            CssClassTextBoxDDD="textbox_padrao"
                            Obrigatorio="false"
                            runat="server"
                            ValidationGroup="PesquisaAvancada"
                            Tipo="FixoCelular" />
                    </div>
                </div>
                <!-- FIM: Linha Telefone -->
                <!-- Linha E-Mail -->
                <div class="linha">
                    <asp:Label
                        ID="lblEmail"
                        AssociatedControlID="txtEmail"
                        CssClass="label_principal"
                        runat="server"
                        Text="E-mail" />
                    <div class="container_campo">
                        <asp:TextBox
                            ID="txtEmail"
                            runat="server"
                            Columns="30"
                            MaxLength="50"
                            CssClass="textbox_padrao"></asp:TextBox>
                    </div>
                </div>
                <!-- FIM: Linha E-Mail -->
                <!-- Linha Estado Civil -->
                <div class="linha">
                    <asp:Label
                        ID="lblEstadoCivil"
                        CssClass="label_principal"
                        runat="server"
                        Text="Estado Civil"
                        AssociatedControlID="rcbEstadoCivil" />
                    <div class="container_campo">
                        <telerik:RadComboBox
                            ID="rcbEstadoCivil"
                            runat="server">
                        </telerik:RadComboBox>
                    </div>
                </div>
                <!-- FIM: Linha Estado Civil -->
                <!-- Linha Bairro -->
                <div class="linha">
                    <asp:Label
                        ID="lblBairro"
                        AssociatedControlID="txtBairro"
                        runat="server"
                        Text="Bairro"
                        CssClass="label_principal" />
                    <div class="container_campo">
                        <asp:UpdatePanel
                            ID="upTxtBairro"
                            runat="server"
                            UpdateMode="Conditional"
                            RenderMode="Inline">
                            <ContentTemplate>
                                <componente:AlfaNumerico
                                    ID="txtBairro"
                                    runat="server"
                                    MaxLength="50"
                                    CssClassTextBox="textbox_padrao"
                                    Obrigatorio="false"
                                    ValidationGroup="PesquisaAvancada"
                                    ExpressaoValidacao="^[A-Z,a-z,0-9,ÁáÂâÃãÉéÊêÍíÓóÔôÕõÚúÛûÇç\,']{0,}( ){0,}[A-Z,a-z,0-9,ÁáÂâÃãÉéÊêÍíÓóÔôÕõÚúÛûÇç\,']{0,}(( ){0,}[A-Z,a-z,0-9,ÁáÂâÃãÉéÊêÍíÓóÔôÕõÚúÛûÇç\,']{0,})*$"
                                    Enabled="True" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel
                            ID="upBtiMapaBairro"
                            runat="server"
                            UpdateMode="Conditional"
                            RenderMode="Inline">
                            <ContentTemplate>

                                <asp:LinkButton ID="btiMapaBairro"
                                    runat="server"
                                    OnClick="btiMapaBairro_Click" CssClass="btn btn-defaut btn-no-margin"><i class="fa fa-map-marker"></i>
 Ver Mapa</asp:LinkButton>

                                <Componentes:BalaoSaibaMais
                                    ID="bsmBtiMapaBairro"
                                    runat="server"
                                    CssClassLabel="balao_saiba_mais"
                                    ToolTipText="Se você procura candidatos que residem em determinados bairros, informe nesse campo um ou mais bairros e tenha apenas os candidatos dessa localidade. Exemplo: Centro, Vila Mariana. <BR />Utilize ainda o botão “Ver Mapa” e visualize o mapa dos bairros de sua cidade, clicando sobre eles automaticamente serão adicionados em sua pesquisa."
                                    ToolTipTitle="Bairro:"
                                    Text="Saiba mais"
                                    ShowOnMouseover="true" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <!-- FIM: Linha Bairro -->
                <!-- Linha Logradouro -->
                <div class="linha">
                    <asp:Label
                        ID="lblLogradouro"
                        AssociatedControlID="txtLogradouro"
                        runat="server"
                        Text="Endereço"
                        CssClass="label_principal" />
                    <div class="container_campo">
                        <componente:AlfaNumerico
                            ID="txtLogradouro"
                            CssClassTextBox="textbox_padrao"
                            runat="server"
                            Obrigatorio="false"
                            MaxLength="50"
                            ValidationGroup="PesquisaAvancada"
                            ExpressaoValidacao="^[A-Z,a-z,0-9,ÁáÂâÃãÉéÊêÍíÓóÔôÕõÚúÛûÇç\,']{0,}( ){0,}[A-Z,a-z,0-9,ÁáÂâÃãÉéÊêÍíÓóÔôÕõÚúÛûÇç\,']{0,}(( ){0,}[A-Z,a-z,0-9,ÁáÂâÃãÉéÊêÍíÓóÔôÕõÚúÛûÇç\,']{0,})*$"
                            Enabled="True" />
                        <span style="display: none">
                            <asp:Label
                                ID="lblAvisoCidadeEndereco"
                                runat="server"
                                Text="Selecione uma cidade"></asp:Label>
                        </span>
                    </div>
                </div>
                <!-- FIM: Linha Logradouro -->
                <!-- Linha Faixa CEP -->
                <div class="linha">
                    <asp:Label
                        ID="lblFaixaCep"
                        runat="server"
                        Text="Faixa de CEP"
                        AssociatedControlID="txtFaixaCep"
                        CssClass="label_principal">
                    </asp:Label>
                    <div class="container_campo">
                        <componente:CEP
                            ID="txtFaixaCep"
                            runat="server"
                            Columns="8"
                            MaxLength="8"
                            Obrigatorio="False"
                            ValorAlteradoClient="txtFaixaCep_ValorAlteradoClient"
                            CssClassTextBox="textbox_padrao" />
                    </div>
                    <asp:Label
                        ID="lblFaixaCepAte"
                        AssociatedControlID="txtFaixaCepAte"
                        runat="server"
                        Text="Até" />
                    <div class="container_campo">
                        <asp:CustomValidator
                            ID="cvFuncaoFaixaCepAte"
                            runat="server"
                            ValidationGroup="PesquisaAvancada"
                            ControlToValidate="txtTeste"
                            ValidateEmptyText="true"
                            ClientValidationFunction="cvFuncaoFaixaCepAte_Validate">
                        </asp:CustomValidator>
                        <asp:TextBox
                            ID="txtTeste"
                            Style="display: none"
                            runat="server"></asp:TextBox>
                        <componente:CEP
                            ID="txtFaixaCepAte"
                            runat="server"
                            Columns="8"
                            MaxLength="8"
                            Obrigatorio="False"
                            ValorAlteradoClient="txtFaixaCepAte_ValorAlteradoClient"
                            CssClassTextBox="textbox_padrao" />
                    </div>
                </div>
                <!-- FIM: Linha Faixa CEP-->
                <!-- Linha Técnico/Graduação -->
                <div class="linha">
                    <asp:Label
                        ID="lblTituloTecnicoGraduacao"
                        runat="server"
                        Text="Técnico / Graduação em"
                        CssClass="label_principal"
                        AssociatedControlID="txtTituloTecnicoGraduacao"></asp:Label>
                    <div class="container_campo">
                        <asp:TextBox
                            ID="txtTituloTecnicoGraduacao"
                            runat="server"
                            CssClass="textbox_padrao"
                            MaxLength="100"></asp:TextBox>
                        <AjaxToolkit:AutoCompleteExtender
                            ID="aceTituloTecnicoGraduacao"
                            runat="server"
                            TargetControlID="txtTituloTecnicoGraduacao"
                            ServiceMethod="ListarCursoNivel3">
                        </AjaxToolkit:AutoCompleteExtender>
                    </div>
                    <!-- Linha Instituicao -->
                    <asp:Label
                        ID="lblInstituicaoTecnicoGraduacao"
                        runat="server"
                        Text="Instituição"
                        AssociatedControlID="txtInstituicaoTecnicoGraduacao"></asp:Label>
                    <div class="container_campo">
                        <asp:TextBox
                            ID="txtInstituicaoTecnicoGraduacao"
                            runat="server"
                            CssClass="textbox_padrao"
                            Columns="30"
                            MaxLength="100"></asp:TextBox>
                        <AjaxToolkit:AutoCompleteExtender
                            ID="aceInstituicaoTecnicoGraduacao"
                            runat="server"
                            TargetControlID="txtInstituicaoTecnicoGraduacao"
                            UseContextKey="false"
                            ServiceMethod="ListarSiglaNomeFonte"
                            OnClientItemSelected="InstituicaoGraduacao">
                        </AjaxToolkit:AutoCompleteExtender>
                        <!-- FIM: Linha Instituicao -->
                    </div>
                    <Componentes:BalaoSaibaMais
                        ID="bsmTxtInstituicaoTecnicoGraduacao"
                        runat="server"
                        CssClassLabel="balao_saiba_mais"
                        Text="Saiba mais"
                        ToolTipTitle="Técnico/Graduação em:"
                        ToolTipText="Se você procura por candidatos com Técnico, Tecnólogo ou Graduado. Informe neste campo o nome da formação desejada. Exemplo: Psicologia. <BR />Se desejar candidatos de uma determinada instituição, informe no campo ao lado o nome da Instituição de ensino. Exemplo: Universidade Federal do Paraná."
                        ShowOnMouseover="true" />
                </div>
                <!-- FIM: Linha Técnico/Graduação -->
                <!-- Linha Outros Cursos -->
                <div class="linha">
                    <asp:Label
                        ID="lblCurso"
                        runat="server"
                        Text="Outros Cursos"
                        CssClass="label_principal"
                        AssociatedControlID="txtTituloCurso"></asp:Label>
                    <div class="container_campo">
                        <asp:TextBox
                            ID="txtTituloCurso"
                            runat="server"
                            CssClass="textbox_padrao"
                            MaxLength="100"></asp:TextBox>
                        <AjaxToolkit:AutoCompleteExtender
                            ID="aceTituloOutrosCursos"
                            runat="server"
                            TargetControlID="txtTituloCurso"
                            ServiceMethod="ListarCursoNivel4">
                        </AjaxToolkit:AutoCompleteExtender>
                    </div>
                    <!-- Linha Instituicao -->
                    <asp:Label
                        ID="lblInstituicao"
                        runat="server"
                        Text="Instituição"
                        AssociatedControlID="txtInstituicao"></asp:Label>
                    <div class="container_campo">
                        <asp:TextBox
                            ID="txtInstituicao"
                            runat="server"
                            CssClass="textbox_padrao"
                            Columns="30"
                            MaxLength="100"></asp:TextBox>
                        <AjaxToolkit:AutoCompleteExtender
                            ID="aceInstituicaoOutrosCursos"
                            runat="server"
                            TargetControlID="txtInstituicao"
                            UseContextKey="false"
                            ServiceMethod="ListarSiglaNomeFonte"
                            OnClientItemSelected="InstituicaoOutrosCursos">
                        </AjaxToolkit:AutoCompleteExtender>
                        <!-- FIM: Linha Instituicao -->
                    </div>
                    <Componentes:BalaoSaibaMais
                        ID="bsmTxtInstituicaoOutrosCursos"
                        runat="server"
                        CssClassLabel="balao_saiba_mais"
                        ToolTipText="Se você procura por candidatos com Especialização, Pós graduação ou Cursos distintos. Informe neste campo o nome do curso desejado. Exemplo: Gestão Estratégica de Pessoas. <BR />Se  desejar candidatos de uma determinada instituição, informe no campo ao lado o nome da Instituição de ensino. Exemplo: Pontifícia Universidade Católica do Paraná."
                        ToolTipTitle="Outros Cursos:"
                        Text="Saiba mais"
                        ShowOnMouseover="true" />
                </div>
                <!-- FIM: Linha Outros Cursos -->
                <!-- Linha Disponibilidade de Trabalho -->
                <div class="linha">
                    <asp:Label
                        ID="lblDisponibilidadeTrabalho"
                        runat="server"
                        Text="Disponibilidade de Trabalho"
                        CssClass="label_principal"
                        AssociatedControlID="rcbDisponibilidade"></asp:Label>
                    <div class="container_campo">
                        <Employer:ComboCheckbox
                            ID="rcbDisponibilidade"
                            EmptyMessage="Qualquer"
                            runat="server" />
                    </div>
                    <Componentes:BalaoSaibaMais
                        ID="bsmChkDisponibilidade"
                        runat="server"
                        CssClassLabel="balao_saiba_mais"
                        ToolTipText="Se você procura candidatos com disponibilidade para trabalho específico. Selecione nesse campo uma ou mais opções e tenha apenas os candidatos que atendem aos seus requisitos."
                        ToolTipTitle="Disponibilidade para Trabalhar:"
                        Text="Saiba Mais"
                        ShowOnMouseover="true" />
                </div>
                <!-- FIM: Linha Disponibilidade de Trabalho -->
                <!-- Linha Empresa -->
                <div class="linha">
                    <asp:Label
                        ID="lblEmpresa"
                        CssClass="label_principal"
                        runat="server"
                        Text="Nome Empresa"
                        AssociatedControlID="txtEmpresa" />
                    <div class="container_campo">
                        <componente:AlfaNumerico
                            ID="txtEmpresa"
                            runat="server"
                            ValidationGroup="PesquisaAvancada"
                            Obrigatorio="false"
                            CssClassTextBox="textbox_padrao" />
                        <!-- Linha Atividade Empresa -->
                        <asp:Label
                            ID="lblAtividadeEmpresa"
                            CssClass="label_principal"
                            runat="server"
                            Text="Atividade da Empresa"
                            AssociatedControlID="rcbAtividadeEmpresa" />
                        <telerik:RadComboBox
                            ID="rcbAtividadeEmpresa"
                            runat="server">
                        </telerik:RadComboBox>
                        <!-- FIM: Linha Atividade Empresa -->
                    </div>
                </div>
                <!-- FIM: Linha Empresa -->
                <!-- Linha Habilitação-->
                <div class="linha">
                    <asp:Label
                        ID="lblHabilitacao"
                        CssClass="label_principal"
                        runat="server"
                        Text="Habilitação"
                        AssociatedControlID="rcbHabilitacao" />
                    <div class="container_campo">
                        <telerik:RadComboBox
                            ID="rcbHabilitacao"
                            runat="server">
                        </telerik:RadComboBox>
                        <asp:Label
                            ID="lblVeiculo"
                            CssClass="label_principal"
                            runat="server"
                            Text="Veículo"
                            AssociatedControlID="rcbVeiculo" />
                        <telerik:RadComboBox
                            ID="rcbVeiculo"
                            runat="server">
                        </telerik:RadComboBox>
                    </div>
                </div>
                <!-- FIM: Linha Habilitação -->
                <!-- Linha PCD -->
                <div class="linha">
                    <asp:Label
                        ID="lblPCD"
                        runat="server"
                        Text="Pessoa com Deficiência (PCD)"
                        CssClass="label_principal"
                        AssociatedControlID="rcbPCD"></asp:Label>
                    <div class="container_campo">
                        <telerik:RadComboBox
                            ID="rcbPCD"
                            runat="server">
                        </telerik:RadComboBox>
                    </div>
                </div>
                <!-- FIM: Linha PCD -->
                <!-- Linha Raça -->
                <div class="linha">
                    <asp:Label
                        ID="lblRaca"
                        runat="server"
                        Text="Raça"
                        CssClass="label_principal"
                        AssociatedControlID="rcbRaca"></asp:Label>
                    <div class="container_campo">
                        <telerik:RadComboBox
                            ID="rcbRaca"
                            runat="server">
                        </telerik:RadComboBox>
                    </div>
                </div>
                <!-- FIM: Linha Raça -->
                <!-- Linha Filhos -->
                <div class="linha">
                    <asp:Label
                        ID="lblFilhos"
                        runat="server"
                        Text="Filhos"
                        CssClass="label_principal"
                        AssociatedControlID="rcbFilhos"></asp:Label>
                    <div class="container_campo">
                        <telerik:RadComboBox
                            ID="rcbFilhos"
                            runat="server">
                        </telerik:RadComboBox>
                    </div>
                </div>
                <!-- FIM: Linha Filhos -->
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
                                    OnClick="btiBuscarFlutuante_Click"
                                    CausesValidation="True"
                                    ValidationGroup="PesquisaAvancada"><i class="fa fa-search"></i> Buscar</asp:LinkButton>
                                <asp:LinkButton CssClass="mini_botao_pesquisa_avancada"
                                    ID="btiLimparFlutuante"
                                    runat="server"
                                    OnClick="btiLimparFlutuante_Click"
                                    CausesValidation="false"><i class="fa fa-eraser"></i> Limpar</asp:LinkButton>
                                <asp:LinkButton CssClass="mini_botao_pesquisa_avancada"
                                    ID="btiAnunciarVagas"
                                    runat="server"
                                    OnClick="btiAnunciarVagas_Click"
                                    CausesValidation="false"><i class="fa fa-bullhorn"></i> Anunciar Vaga</asp:LinkButton>
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
                        runat="server"
                        Text="Buscar"
                        OnClick="btnBuscar_Click" />
                    <asp:Button
                        ID="btnLimpar"
                        CssClass="botao_padrao"
                        runat="server"
                        Text="Limpar"
                        OnClick="btnLimpar_Click" />
                    <asp:Button
                        ID="btnAnunciarVagas"
                        runat="server"
                        Text="Anunciar Vaga"
                        CssClass="botao_padrao"
                        CausesValidation="false"
                        OnClick="btnAnunciarVagas_Click" />
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
<asp:Content
    ID="Content3"
    ContentPlaceHolderID="cphRodape"
    runat="server">
</asp:Content>
