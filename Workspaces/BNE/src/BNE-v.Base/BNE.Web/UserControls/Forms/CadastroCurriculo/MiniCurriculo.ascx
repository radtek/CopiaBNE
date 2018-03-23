<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MiniCurriculo.ascx.cs"
    Inherits="BNE.Web.UserControls.Forms.CadastroCurriculo.MiniCurriculo" %>
<%@ Register Src="~/UserControls/UcValidacaoCelular.ascx" TagName="UcValidacaoCelular" TagPrefix="uc1" %>
<Employer:DynamicScript runat="server" Src="/js/local/Forms/CadastroCurriculo/MiniCurriculo.js" type="text/javascript" />
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/CadastroCurriculo/MiniCurriculo.css" type="text/css" rel="stylesheet" />
<!-- Tabs -->
<asp:UpdatePanel ID="upnAbas" runat="server">
    <ContentTemplate>
        <h1>
            <asp:Literal ID="litTitulo" runat="server" Text="Cadastro de Currículo">
            </asp:Literal>
        </h1>
        <asp:Panel ID="pnlAbas" runat="server">
            <div class="linha_abas">
                <div class="abas_nova">
                    <span class="aba_esquerda_selecionada_inicio aba">
                        <asp:Label ID="Label3" runat="server" Text="Mini Currículo" CssClass="texto_abas_selecionada">
                        </asp:Label>
                    </span>
                </div>
                <div class="abas_nova">
                    <span class="aba_esquerda_nao_selecionada aba">
                        <asp:LinkButton ID="btlDadosPessoais" runat="server" CssClass="texto_abas" OnClick="btlDadosPessoais_Click"
                            ValidationGroup="CadastroCurriculoMini" CausesValidation="true" Text="Dados Pessoais e Profissionais"></asp:LinkButton>
                    </span>
                </div>
                <div class="abas_nova">
                    <span class="aba_esquerda_nao_selecionada aba">
                        <asp:LinkButton ID="btlFormacaoCursos" runat="server" OnClick="btlFormacaoCursos_Click"
                            ValidationGroup="CadastroCurriculoMini" CausesValidation="true" Text="Formação e Cursos"
                            CssClass="texto_abas"></asp:LinkButton>
                    </span>
                </div>
                <div class="abas_nova">
                    <span class="aba_esquerda_nao_selecionada aba">
                        <asp:LinkButton ID="btlDadosComplementares" runat="server" OnClick="btlDadosComplementares_Click"
                            ValidationGroup="CadastroCurriculoMini" CausesValidation="true" Text="Dados Complementares"
                            CssClass="texto_abas"></asp:LinkButton>
                    </span>
                </div>
                <div class="abas_nova">
                    <span class="aba_esquerda_nao_selecionada aba">
                        <asp:LinkButton ID="btlRevisaoDados" runat="server" OnClick="btlRevisaoDados_Click"
                            ValidationGroup="CadastroCurriculoMini" CausesValidation="true" Text="Conferir"
                            CssClass="texto_abas"></asp:LinkButton>
                    </span>
                </div>
            </div>
            <div class="abas" style="display: none">
                <span class="aba_fundo">
                    <asp:LinkButton ID="btlGestao" runat="server" OnClick="btlGestao_Click" ValidationGroup="CadastroCurriculoMini"
                        CausesValidation="true" Text="Gestão"></asp:LinkButton>
                </span>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
<!-- FIM: Tabs -->
<!-- Painel: Dados Mini Curriculo -->
<div class="interno_abas">
    <asp:Panel runat="server" ID="pnlCadastroCurriculo" CssClass="painel_padrao mini_curriculo">
        <div class="foto_mini_curriculo">
            <asp:UpdatePanel ID="upFoto" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <Employer:SelecionarFoto ID="ucFoto" runat="server" AspectRatio="3:4" InitialSelection="5;5;75;100"
                        SemFotoImagemUrl="/img/sem_foto.png" ThumbDir="~/ArquivosTemporarios/" MinAcceptedHeight="100"
                        MinAcceptedWidth="100" ResizeImageHeight="133" ResizeImageWidth="178" MaxHeight="1024"
                        MaxWidth="1280" OnError="ucFoto_Error" />
                    <asp:LinkButton runat="server" ID="btlExisteFotoWS" OnClick="btlExisteFotoWS_Click"
                        Visible="False" Text="Já existe uma foto. Clique para recuperar." ToolTip="Já existe uma foto. Clique para recuperar."
                        CssClass="link_alterar_foto" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <asp:Panel runat="server" ID="pnlAjusteFloat" CssClass="AjustePanel" Style="float: left;">
            <div class="painel_padrao_topo">
            </div>
            <p class="texto_marcadores_obrigatorio  cadastro_curriculo">
                Os campos marcados com um
                <img alt="*" src="img/icone_obrigatorio.gif" />
                são obrigatórios para o cadastro de seu currículo.
            </p>
            <%--FIM: Linha imgBtnFotoCurriculo--%>
            <asp:UpdatePanel ID="upnAbaMiniCurriculoDados" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <%--Linha CPF--%>
                    <div class="linha">
                        <asp:Label runat="server" ID="lblCPF" CssClass="label_principal" Text="CPF" AssociatedControlID="txtCPF" />
                        <div class="container_campo">
                            <asp:UpdatePanel ID="upTxtCPF" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <componente:CPF ID="txtCPF" runat="server" ValidationGroup="CadastroCurriculoMini" OnValorAlterado="txtCPF_ValorAlterado" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSalvarCurriculo" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <%--FIM: Linha CPF--%>
                    <%--Linha Data de Nascimento--%>
                    <div class="linha">
                        <asp:Label runat="server" ID="lblDataDeNascimento" CssClass="label_principal" Text="Data de Nascimento"
                            AssociatedControlID="txtDataDeNascimento" />
                        <div class="container_campo">
                            <asp:UpdatePanel ID="upDataNascimento" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <Employer:DataTextBox runat="server" ID="txtDataDeNascimento" OnValorAlterado="txtDataDeNascimento_ValorAlterado"
                                        Tamanho="10" ValidationGroup="CadastroCurriculoMini" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSalvarCurriculo" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <%--FIM: Linha Data de Nascimento--%>
                    <%--Linha Celular--%>
                    <div class="linha">
                        <asp:Label runat="server" ID="lblCelular" CssClass="label_principal" Text="Celular"
                            AssociatedControlID="txtTelefoneCelular" />
                        <div class="container_campo">
                            <asp:UpdatePanel ID="upTxtTelefoneCelular" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <componente:Telefone ID="txtTelefoneCelular" runat="server" MensagemErroFormatoFone='<%$ Resources: MensagemAviso, _100006 %>'
                                        ValidationGroup="CadastroCurriculoMini" Tipo="Celular" OnValorAlteradoFone="txtTelefoneCelular_OnValorAlteradoFone" />

                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSalvarCurriculo" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="linha" style="display: none">
                        <asp:UpdatePanel ID="upValidacaoCelular" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel runat="server" ID="pnlValidacaoCelular" Visible="false">
                                    <uc1:UcValidacaoCelular ID="UcValidacaoCelular" runat="server" />
                                </asp:Panel>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSalvarCurriculo" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <%--FIM: Linha Celular--%>
                    <%-- Linha E-Mail --%>
                    <div class="linha">
                        <asp:Label ID="lblEmail" AssociatedControlID="txtEmail" CssClass="label_principal"
                            runat="server" Text="E-mail" />
                        <div class="container_campo">
                            <div>
                                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                    ValidationGroup="CadastroCurriculoMini" ErrorMessage="Email Inválido.">
                                </asp:RegularExpressionValidator>
                            </div>
                            <asp:TextBox ID="txtEmail" runat="server" Columns="50" MaxLength="50" CssClass="textbox_padrao"></asp:TextBox>
                            <AjaxToolkit:AutoCompleteExtender ID="aceEmail" runat="server" TargetControlID="txtEmail"
                                UseContextKey="False" ServiceMethod="ListarSugestaoEmail">
                            </AjaxToolkit:AutoCompleteExtender>
                        </div>
                    </div>
                    <%-- FIM: Linha E-Mail --%>
                    <%--Linha Nome--%>
                    <div class="linha">
                        <asp:Label runat="server" ID="lblNome" Text="Nome" CssClass="label_principal" AssociatedControlID="txtNome" />
                        <div class="container_campo">
                            <asp:UpdatePanel ID="upTxtNome" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <componente:AlfaNumerico CssClassTextBox="textbox_padrao campo_obrigatorio textbox_nome"
                                        ID="txtNome" runat="server" Columns="40" MensagemErroFormato='<%$ Resources: MensagemAviso, _100107 %>'
                                        MensagemErroIntervalo='<%$ Resources: MensagemAviso, _100107 %>' MensagemErroValor='<%$ Resources: MensagemAviso, _100107 %>'
                                        ValidationGroup="CadastroCurriculoMini" ClientValidationFunction="ValidarNome" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSalvarCurriculo" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <%--FIM: Linha Nome--%>
                    <%--Linha Sexo--%>
                    <div class="linha">
                        <asp:Label runat="server" ID="lblSexo" Text="Sexo" CssClass="label_principal" AssociatedControlID="rblSexo" />
                        <div class="container_campo">
                            <asp:RequiredFieldValidator ID="rfvRblSexo" runat="server" ControlToValidate="rblSexo"
                                ValidationGroup="CadastroCurriculoMini"></asp:RequiredFieldValidator>
                            <asp:UpdatePanel ID="upRblSexo" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:RadioButtonList ID="rblSexo" runat="server" SkinID="Horizontal" ValidationGroup="sexo">
                                    </asp:RadioButtonList>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSalvarCurriculo" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <%--FIM: Linha Sexo--%>
                    <%-- Linha Cidade--%>
                    <div class="linha">
                        <asp:Label runat="server" ID="lblCidade" CssClass="label_principal" Text="Cidade"
                            AssociatedControlID="txtCidadeMini" />
                        <asp:UpdatePanel ID="upTxtCidadeMini" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div>
                                    <asp:RequiredFieldValidator ID="rfvCidadeMini" runat="server" ControlToValidate="txtCidadeMini"
                                        ValidationGroup="CadastroCurriculoMini"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cvCidadeMini" runat="server" ErrorMessage="Cidade Inválida."
                                        ClientValidationFunction="cvCidadeMini_Validate" ControlToValidate="txtCidadeMini"
                                        ValidationGroup="CadastroCurriculoMini"></asp:CustomValidator>
                                </div>
                                <div class="container_campo">
                                    <asp:TextBox ID="txtCidadeMini" runat="server" CssClass="textbox_padrao campo_obrigatorio textbox_cidade_mini_cv"
                                        Columns="40"></asp:TextBox>
                                    <AjaxToolkit:AutoCompleteExtender ID="aceCidadeMini" runat="server" TargetControlID="txtCidadeMini"
                                        ServiceMethod="ListarCidadesSiglaEstadoPorNomeParcialEstado">
                                    </AjaxToolkit:AutoCompleteExtender>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSalvarCurriculo" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <%-- FIM: Linha Cidade --%>

                    <%--   [Obsolete("Optado pela não utilização/disponibilização")]
                    <div class="linha">
                        <asp:Label ID="lblTipoContrato" CssClass="label_principal" Text="Contrato Pretendido"
                            AssociatedControlID="chblContrato" runat="server"></asp:Label>
                        <div>
                            <a:CheckBoxListValidator CssClass="validador" Display="Dynamic" ValidationGroup="CadastroCurriculoMini" EnableClientScript="True" ControlToValidate="chblContrato" MinimumNumberOfSelectedCheckBoxes="1" runat="server" ID="cvCheckBoxList" ErrorMessage="É necessário escolher pelo menos um tipo de contrato." />
                        </div>
                        <div class="container_campo">
                            <asp:CheckBoxList ID="chblContrato" CssClass="container_candidato_tipo_contrato" RepeatDirection="Horizontal" RepeatColumns="3" runat="server" />
                        </div>
                    </div>--%>

                    <%-- Linha Escolaridade --%>
                    <div class="linha">
                        <asp:Label ID="lblEscolaridade" CssClass="label_principal" Text="Escolaridade"
                            AssociatedControlID="ddlEscolaridade" runat="server"></asp:Label>
                        <div>
                            <asp:RequiredFieldValidator InitialValue="0" ID="rfvEscolaridade" Display="Dynamic"
                                ValidationGroup="CadastroCurriculoMini" runat="server" ControlToValidate="ddlEscolaridade"
                                ErrorMessage="Escolaridade Inválida."></asp:RequiredFieldValidator>
                        </div>
                        <div class="container_campo">
                            <asp:DropDownList ID="ddlEscolaridade" runat="server" CssClass="textbox_padrao campo_obrigatorio dropdown_escolaridade" AutoPostBack="False">
                            </asp:DropDownList>
                            <%--<asp:CheckBoxList ID="chblContrato" CssClass="container_candidato_tipo_contrato" RepeatDirection="Horizontal" RepeatColumns="3" runat="server" />--%>
                        </div>
                    </div>

                    <div id="divLinhaTituloCurso" class="linha" runat="server" style="display: none">
                        <asp:Label ID="Label1" runat="server" Text="Nome do Curso" CssClass="label_principal" AssociatedControlID="txtTituloCurso"></asp:Label>
                        <div class="container_campo">
                            <asp:TextBox ID="txtTituloCurso" runat="server" CssClass="textbox_padrao campo_obrigatorio" MaxLength="100" Columns="40"></asp:TextBox>
                            <AjaxToolkit:AutoCompleteExtender ID="aceTituloCurso" runat="server" TargetControlID="txtTituloCurso" ServiceMethod="ListarCursoFonte">
                            </AjaxToolkit:AutoCompleteExtender>
                        </div>
                    </div>
                    <%-- FIM: Linha Escolaridade --%>

                    <%-- Linha aceito trabalhar como Estagiário --%>
                    <div id="divAceitaEstagio" runat="server" class="linha lineAdjust" style="display: none">

                        <asp:CheckBox ID="ckbAceitaEstagio" CssClass="alignCheckBox" runat="server" SkinID="Horizontal" ValidationGroup="estagiario"></asp:CheckBox>
                        <asp:Label ID="lblAceitaEstagio" Text="Também aceito atuar como Estagiário." runat="server"></asp:Label>
                        <div class="alert fade in">
                            <button type="button" class="close" data-dismiss="alert">×</button>
                            Informe abaixo <strong>as funções</strong>  que você deseja atuar como <strong>Estagiário</strong>  ou <strong>Profissional</strong>.
                                    <p><em><strong>- Exemplo: </strong>Auxiliar Administrativo.</em></p>

                        </div>
                    </div>


                    <%-- FIM: aceito trabalhar como Estagiário --%>

                    <%-- Linha Funções Pretendidas 1 --%>
                    <div class="linha">
                        <asp:Label ID="lblFuncaoPretendida" runat="server" CssClass="label_principal" Text="Função Pretendida 1"
                            AssociatedControlID="txtFuncaoPretendida1" />
                        <div class="container_campo">
                            <asp:UpdatePanel ID="upFuncaoPretendida1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div>
                                        <asp:RequiredFieldValidator ID="rfvFuncaoPretendida1" runat="server" ControlToValidate="txtFuncaoPretendida1"
                                            ValidationGroup="CadastroCurriculoMini"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvFuncaoPretendida" runat="server" ControlToValidate="txtFuncaoPretendida1"
                                            ErrorMessage="Função Inválida" ValidationGroup="CadastroCurriculoMini"></asp:CustomValidator>
                                    </div>
                                    <asp:TextBox ID="txtFuncaoPretendida1" runat="server" Columns="40"></asp:TextBox>
                                    <AjaxToolkit:AutoCompleteExtender ID="aceFuncaoPretendida1" runat="server" TargetControlID="txtFuncaoPretendida1"
                                        UseContextKey="True" ServiceMethod="ListarFuncoes">
                                    </AjaxToolkit:AutoCompleteExtender>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSalvarCurriculo" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <asp:Label ID="lblTempoExperiencia" runat="server" Text="Experiência" AssociatedControlID="txtAnoExperiencia1"></asp:Label>
                        <div class="container_campo">
                            <asp:UpdatePanel ID="upTxtAnoExperiencia1" runat="server" UpdateMode="Conditional"
                                RenderMode="Inline">
                                <ContentTemplate>
                                    <componente:AlfaNumerico ID="txtAnoExperiencia1" runat="server" Columns="2" MaxLength="2"
                                        ContemIntervalo="True" ValorMaximo="66" ValorMinimo="0" CssClassTextBox="textbox_padrao"
                                        Obrigatorio="false" MensagemErroFormato="Tempo de experiência inválido" MensagemErroIntervalo="Tempo de experiência inválido"
                                        MensagemErroValor="Tempo de experiência inválido" Tipo="Numerico" ValidationGroup="CadastroCurriculoMini" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSalvarCurriculo" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                            <label>
                                Anos</label>
                        </div>
                        <div class="container_campo">
                            <asp:UpdatePanel ID="upTxtMesExperiencia1" runat="server" UpdateMode="Conditional"
                                RenderMode="Inline">
                                <ContentTemplate>
                                    <componente:AlfaNumerico ID="txtMesExperiencia1" runat="server" Columns="2" MaxLength="2"
                                        ContemIntervalo="True" ValorMaximo="11" ValorMinimo="0" MensagemErroFormato="Tempo de experiência inválido"
                                        MensagemErroIntervalo="Tempo de experiência inválido" MensagemErroValor="Tempo de experiência inválido"
                                        Obrigatorio="False" CssClassTextBox="textbox_padrao" Tipo="Numerico" ValidationGroup="CadastroCurriculoMini" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSalvarCurriculo" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                            <label>
                                Meses</label>
                        </div>
                    </div>
                    <%-- FIM: Linha Funções Pretendidas 1 --%>
                    <%-- Linha Funções Pretendidas 2 --%>
                    <div class="linha">
                        <asp:Label ID="Label4" runat="server" CssClass="label_principal" Text="Função Pretendida 2"
                            AssociatedControlID="txtFuncaoPretendida2" />
                        <div class="container_campo">
                            <asp:UpdatePanel ID="upTxtFuncaoPretendida2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtFuncaoPretendida2" runat="server" CssClass="textbox_padrao" Columns="42"
                                        AutoPostBack="True"></asp:TextBox>
                                    <AjaxToolkit:AutoCompleteExtender ID="aceFuncaoPretendida2" runat="server" TargetControlID="txtFuncaoPretendida2"
                                        UseContextKey="True" ServiceMethod="ListarFuncoes">
                                    </AjaxToolkit:AutoCompleteExtender>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSalvarCurriculo" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <asp:Label ID="Label5" runat="server" Text="Experiência" AssociatedControlID="txtAnoExperiencia2"></asp:Label>
                        <div class="container_campo">
                            <asp:UpdatePanel ID="upTxtAnoExperiencia2" runat="server" UpdateMode="Conditional"
                                RenderMode="Inline">
                                <ContentTemplate>
                                    <componente:AlfaNumerico ID="txtAnoExperiencia2" runat="server" Columns="2" MaxLength="2"
                                        ContemIntervalo="True" Enabled="true" ValorMaximo="66" ValorMinimo="0" CssClassTextBox="textbox_padrao"
                                        Obrigatorio="false" MensagemErroFormato="Tempo de experiência inválido" MensagemErroIntervalo="Tempo de experiência inválido"
                                        MensagemErroValor="Tempo de experiência inválido" Tipo="Numerico" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSalvarCurriculo" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                            <label>
                                Anos</label>
                        </div>
                        <div class="container_campo">
                            <asp:UpdatePanel ID="upTxtMesExperiencia2" runat="server" UpdateMode="Conditional"
                                RenderMode="Inline">
                                <ContentTemplate>
                                    <componente:AlfaNumerico ID="txtMesExperiencia2" runat="server" Columns="2" MaxLength="2"
                                        Enabled="true" ContemIntervalo="True" ValorMaximo="11" ValorMinimo="0" MensagemErroFormato="Tempo de experiência inválido"
                                        MensagemErroIntervalo="Tempo de experiência inválido" MensagemErroValor="Tempo de experiência inválido"
                                        Obrigatorio="False" CssClassTextBox="textbox_padrao" Tipo="Numerico" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSalvarCurriculo" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                            <label>
                                Meses</label>
                        </div>
                    </div>
                    <%-- FIM: Linha Funções Pretendidas 2 --%>
                    <%-- Linha Funções Pretendidas 3 --%>
                    <div class="linha">
                        <asp:Label ID="Label8" runat="server" CssClass="label_principal" Text="Função Pretendida 3"
                            AssociatedControlID="txtFuncaoPretendida3" />
                        <div class="container_campo">
                            <asp:UpdatePanel ID="upTxtFuncaoPretendida3" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtFuncaoPretendida3" runat="server" CssClass="textbox_padrao" Columns="42"></asp:TextBox>
                                    <AjaxToolkit:AutoCompleteExtender ID="aceFuncaoPretendida3" runat="server" TargetControlID="txtFuncaoPretendida3"
                                        UseContextKey="True" ServiceMethod="ListarFuncoes">
                                    </AjaxToolkit:AutoCompleteExtender>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSalvarCurriculo" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <asp:Label ID="Label9" runat="server" Text="Experiência" AssociatedControlID="txtAnoExperiencia3"></asp:Label>
                        <div class="container_campo">
                            <asp:UpdatePanel ID="upTxtAnoExperiencia3" runat="server" UpdateMode="Conditional"
                                RenderMode="Inline">
                                <ContentTemplate>
                                    <componente:AlfaNumerico ID="txtAnoExperiencia3" runat="server" Columns="2" MaxLength="2"
                                        ContemIntervalo="True" ValorMaximo="66" Enabled="true" ValorMinimo="0" CssClassTextBox="textbox_padrao"
                                        Obrigatorio="false" MensagemErroFormato="Tempo de experiência inválido" MensagemErroIntervalo="Tempo de experiência inválido"
                                        MensagemErroValor="Tempo de experiência inválido" Tipo="Numerico" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSalvarCurriculo" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                            <label>
                                Anos</label>
                        </div>
                        <div class="container_campo">
                            <asp:UpdatePanel ID="upTxtMesExperiencia3" runat="server" UpdateMode="Conditional"
                                RenderMode="Inline">
                                <ContentTemplate>
                                    <componente:AlfaNumerico ID="txtMesExperiencia3" runat="server" Columns="2" MaxLength="2"
                                        Enabled="true" ContemIntervalo="True" ValorMaximo="11" ValorMinimo="0" MensagemErroFormato="Tempo de experiência inválido"
                                        MensagemErroIntervalo="Tempo de experiência inválido" MensagemErroValor="Tempo de experiência inválido"
                                        Obrigatorio="False" CssClassTextBox="textbox_padrao" Tipo="Numerico" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSalvarCurriculo" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                            <label>
                                Meses</label>
                        </div>
                    </div>
                    <%-- FIM: Linha Funções Pretendidas 3 --%>
                    <%-- Linha Pretensão Salarial--%>
                    <div class="linha">
                        <asp:Label ID="lblPretensaoSalarial" runat="server" CssClass="label_principal" Text="Pretensão Salarial R$"
                            AssociatedControlID="txtPretensaoSalarial" />
                        <div class="container_campo container_campoAdjust_cadMini">
                            <asp:UpdatePanel ID="upTxtPretensaoSalarial" runat="server" UpdateMode="Conditional"
                                RenderMode="Inline">
                                <ContentTemplate>
                                    <componente:ValorDecimal ID="txtPretensaoSalarial" runat="server" CasasDecimais="0" 
                                        ValidationGroup="CadastroCurriculoMini" ValorMaximo="999999" CssClassTextBox="textarea_padraoAdjust_cadMini textbox_padrao campo_obrigatorio" ValorMinimo="1" />
                                 <span class="decimais_cadMini">,00</span>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSalvarCurriculo" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                            <span id="faixa_salarial" style="display: none" class="faixa_informacao_destaque"
                                runat="server"></span>
                        </div>
                    </div>
                    <%-- FIM: Linha Pretensão Salarial --%>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSalvarCurriculo" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:Panel>
    </asp:Panel>
</div>
<!-- Painel: Dados Mini Curriculo-->
<!-- Painel:Botões -->
<asp:Panel runat="server" CssClass="painel_botoes" ID="pnlBotoes">
    <asp:Button runat="server" ID="btnSalvarCurriculo" Text="Salvar e Avançar" OnClick="btnSalvarCurriculo_Click"
        CssClass="botao_padrao" ValidationGroup="CadastroCurriculoMini" />
</asp:Panel>
<!-- FIM Painel:Botões -->
