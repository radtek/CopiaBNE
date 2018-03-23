<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MiniCurriculo.ascx.cs"
    Inherits="BNE.Web.UserControls.Forms.CadastroCurriculo.MiniCurriculo" %>
<%@ Register Src="~/UserControls/Modais/ucPoliticaPrivacidade.ascx" TagPrefix="uc1" TagName="ucPoliticaPrivacidade" %>

<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/CadastroCurriculo/MiniCurriculo.css" type="text/css" rel="stylesheet" />
<!-- Tabs -->
<script>
    $(document).ready(function () {
        blockAt();
        $('[data-toggle="tooltip"]').tooltip();
    });
</script>
<style>
    .CelularCheckedStyle { float: right !important; margin-top: 2px !important; }
</style>
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
                    <Componentes:SelecionarFoto ID="ucFoto" runat="server" AspectRatio="3:4" InitialSelection="5;5;75;100"
                        SemFotoImagemUrl="/img/sem_foto.png" ThumbDir="~/ArquivosTemporarios/" MinAcceptedHeight="100"
                        MinAcceptedWidth="100" ResizeImageHeight="133" ResizeImageWidth="178" MaxHeight="1024"
                        MaxWidth="1280" OnError="ucFoto_Error" PainelRemoverImagemCssClass="" ModalCssClass="modal_conteudo foto" />
                    <asp:LinkButton runat="server" ID="btlExisteFotoWS" OnClick="btlExisteFotoWS_Click"
                        Visible="False" Text="Encontramos uma foto em um sistema parceiro. Clique aqui para recuperar." ToolTip="Encontramos uma foto em um sistema parceiro. Clique aqui para recuperar."
                        CssClass="link_alterar_foto" CausesValidation="false" />
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
                    <%-- Linha E-Mail --%>
                    <div class="linha">
                        <div class="alert" id="dvEmail" style="display: none; margin-left: 164px;"></div>
                    </div>
                    <div class="linha">
                        <asp:UpdatePanel ID="upEmail" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblEmail" AssociatedControlID="txtEmail" CssClass="label_principal"
                                    runat="server" Text="E-mail" />
                                <div class="container_campo">
                                    <div>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvEmail" ControlToValidate="txtEmail" Enabled="false"
                                            ErrorMessage="Campo Obrigatório" ValidationGroup="CadastroCurriculoMini">
                                        </asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvEmailAuto"
                                            ErrorMessage="Email Incorreto" runat="server" ClientValidationFunction="ValEmail"
                                            ValidationGroup="CadastroCurriculoMini" ControlToValidate="txtAt">
                                        </asp:CustomValidator>
                                        <asp:RegularExpressionValidator ID="revEmailvalidacao" runat="server" ValidationGroup="CadastroCurriculoMini"
                                            ErrorMessage="Formato incorreto" ControlToValidate="txtEmail"
                                            ValidationExpression="^\w+([-+.']\w+)*$">
                                        </asp:RegularExpressionValidator>
                                    </div>
                                    <asp:TextBox ID="txtEmail" runat="server" Columns="16" MaxLength="50" CssClass="textbox_padrao" AutoPostBack="true" OnTextChanged="txtEmail_OnTextChanged"></asp:TextBox>
                                    <asp:Label ID="lblAt" runat="server" Text="@"></asp:Label>
                                    &nbsp
                <%--    <AjaxToolkit:AutoCompleteExtender ID="aceEmail" runat="server" TargetControlID="txtEmail"
                                UseContextKey="False" ServiceMethod="ListarSugestaoEmail">
                            </AjaxToolkit:AutoCompleteExtender>--%>
                                </div>
                                <div class="container_campo">
                                    <div>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvAt" ControlToValidate="txtAt"
                                            ErrorMessage="Campo Obrigatório" ValidationGroup="CadastroCurriculoMini" Enabled="false">
                                        </asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator InitialValue="0" runat="server" ID="rfvDdlAt" Enabled="false"
                                            ErrorMessage="Campo Obrigatório" ControlToValidate="ddlAt" ValidationGroup="CadastroCurriculoMini">
                                        </asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revTxtAt" runat="server" ControlToValidate="TxtAt" Enabled="false"
                                            ErrorMessage="Formato incorreto" ValidationGroup="CadastroCurriculoMini"
                                            ValidationExpression="^((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,3}(?:\.[a-z]{2})?)$">
                                        </asp:RegularExpressionValidator>
                                    </div>
                                    <asp:TextBox ID="txtAT" runat="server" OnTextChanged="txtAT_TextChanged" AutoPostBack="true" CssClass="textbox_padrao" Visible="false" Columns="12"></asp:TextBox>
                                    <asp:DropDownList CssClass="textbox_padrao" ID="ddlAt" AutoPostBack="true" OnSelectedIndexChanged="ddlAt_SelectedIndexChanged" runat="server"></asp:DropDownList>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <%-- FIM: Linha E-Mail --%>
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
                                    <Componentes:DataTextBox runat="server" ID="txtDataDeNascimento" OnValorAlterado="txtDataDeNascimento_ValorAlterado"
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
                                    <componente:Telefone ID="txtTelefoneCelular" runat="server" MensagemErroFormatoFone="<%$ Resources: MensagemAviso, _100006 %>"
                                        ValidationGroup="CadastroCurriculoMini" Tipo="Celular" OnValorAlteradoFone="txtTelefoneCelular_OnValorAlteradoFone" />
                                    <asp:Image ID="CelularChecked" runat="server" ImageUrl="~/img/img_icone_check_16x16.png" CssClass="CelularCheckedStyle"
                                        data-toggle="tooltip" data-placement="top" title="Número de celular confirmado." />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSalvarCurriculo" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <asp:CheckBox runat="server" ID="cbWhats"  />
                        <asp:Label runat="server" ID="lblWhats" Text="Tenho WhatsApp neste celular"></asp:Label>
                    </div>
                    <%--FIM: Linha Celular--%>
                    <%--Linha Nome--%>
                    <div class="linha">
                        <asp:Label runat="server" ID="lblNome" Text="Nome" CssClass="label_principal" AssociatedControlID="txtNome" />
                        <div class="container_campo">
                            <asp:UpdatePanel ID="upTxtNome" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <componente:AlfaNumerico CssClassTextBox="textbox_padrao campo_obrigatorio textbox_nome"
                                        ID="txtNome" runat="server" Columns="40" MensagemErroFormato="<%$ Resources: MensagemAviso, _100107 %>"
                                        MensagemErroIntervalo="<%$ Resources: MensagemAviso, _100107 %>" MensagemErroValor="<%$ Resources: MensagemAviso, _100107 %>"
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
                                ValidationGroup="CadastroCurriculoMini">
                            </asp:RequiredFieldValidator>
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
                                        ValidationGroup="CadastroCurriculoMini">
                                    </asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cvCidadeMini" runat="server" ErrorMessage="Cidade Inválida."
                                        ClientValidationFunction="cvCidadeMini_Validate" ControlToValidate="txtCidadeMini"
                                        ValidationGroup="CadastroCurriculoMini">
                                    </asp:CustomValidator>
                                </div>
                                <div class="container_campo">
                                    <asp:TextBox ID="txtCidadeMini" runat="server" CssClass="textbox_padrao campo_obrigatorio textbox_cidade_mini_cv"
                                        Columns="40">
                                    </asp:TextBox>
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
                            AssociatedControlID="ddlEscolaridade" runat="server">
                        </asp:Label>
                        <div>
                            <asp:RequiredFieldValidator InitialValue="0" ID="rfvEscolaridade" Display="Dynamic"
                                ValidationGroup="CadastroCurriculoMini" runat="server" ControlToValidate="ddlEscolaridade"
                                ErrorMessage="Escolaridade Inválida.">
                            </asp:RequiredFieldValidator>
                        </div>
                        <div class="container_campo">
                            <asp:DropDownList ID="ddlEscolaridade" runat="server" CssClass="textbox_padrao campo_obrigatorio dropdown_escolaridade" AutoPostBack="False">
                            </asp:DropDownList>
                            <%--<asp:CheckBoxList ID="chblContrato" CssClass="container_candidato_tipo_contrato" RepeatDirection="Horizontal" RepeatColumns="3" runat="server" />--%>
                        </div>
                    </div>

                    <div id="divLinhaTituloCurso" runat="server" style="display: none">
                        <div class="linha">
                            <asp:Label ID="Label1" runat="server" Text="Nome do Curso" CssClass="label_principal" AssociatedControlID="txtTituloCurso"></asp:Label>
                            <div class="container_campo">
                                <asp:TextBox ID="txtTituloCurso" runat="server" CssClass="textbox_padrao campo_obrigatorio" MaxLength="100" Columns="36"></asp:TextBox>
                                <AjaxToolkit:AutoCompleteExtender ID="aceTituloCurso" runat="server" TargetControlID="txtTituloCurso" ServiceMethod="ListarCursoFonte">
                                </AjaxToolkit:AutoCompleteExtender>
                            </div>
                        </div>
                        <div id="divLinhaSituacao" runat="server" style="display: none">
                            <div class="linha">
                                <asp:Label ID="lblSituacao" runat="server" Text="Situação" CssClass="label_principal" AssociatedControlID="ddlSituacao"></asp:Label>
                                <div class="container_campo">
                                    <asp:CustomValidator ID="cvSituacao" runat="server" ValidationGroup="CadastroFormacao" ControlToValidate="ddlSituacao" ErrorMessage="Campo Obrigatório" ClientValidationFunction="cvSituacao_Validate">
                                    </asp:CustomValidator>
                                    <asp:DropDownList ID="ddlSituacao" CssClass="textbox_padrao campo_obrigatorio" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="linha">
                                <asp:Label ID="lblPeriodo" runat="server" CssClass="label_principal" Text="Período ou Semestre" AssociatedControlID="txtPeriodo"></asp:Label>
                                <div class="container_campo">
                                    <componente:AlfaNumerico ID="txtPeriodo" runat="server" ContemIntervalo="true" ValorMinimo="1" ValorMaximo="12" Columns="2" MaxLength="2" Tipo="Numerico" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <%-- FIM: Linha Escolaridade --%>

                    <%-- Linha aceito trabalhar como Estagiário --%>
                    <div id="divAceitaEstagio" runat="server" class="linha lineAdjust" style="display: none">

                        <asp:CheckBox ID="ckbAceitaEstagio" CssClass="alignCheckBox" runat="server" SkinID="Horizontal" ValidationGroup="estagiario"></asp:CheckBox>
                        <asp:Label ID="lblAceitaEstagio" Text="Também aceito atuar como Estagiário." runat="server"></asp:Label>
                        <div class="alert fade in">
                            <button type="button" class="close" data-dismiss="alert">×</button>
                            Informe abaixo <strong>as funções</strong> que você deseja atuar como <strong>Estagiário</strong> ou <strong>Profissional</strong>.
                                <p>
                                    <em><strong>- Exemplo: </strong>Auxiliar Administrativo.</em>
                                </p>
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
                                            ValidationGroup="CadastroCurriculoMini">
                                        </asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvFuncaoPretendida" runat="server" ControlToValidate="txtFuncaoPretendida1"
                                            ErrorMessage="Função Inválida" ValidationGroup="CadastroCurriculoMini">
                                        </asp:CustomValidator>
                                    </div>
                                    <asp:TextBox ID="txtFuncaoPretendida1" runat="server" Columns="40"></asp:TextBox>
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
                                Anos
                            </label>
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
                                Meses
                            </label>
                        </div>

                    </div>
                    <%-- iNICIO Sugest de funnção sinonomo 1 --%>
                    <div class="linha" id="divSugestFuncaoSinonimo1" style="display: none;">
                        <asp:Label ID="Label2" runat="server" CssClass="label_principal" Text="." Style="color: white;" />
                        <div class="container_campo">

                            <div class="sugestao_funcao-sinonimo">
                                <div class="sugestao_funcao-sinonimo_title">
                                    <div>Sugestão BNE</div>
                                    <a onclick="CloseSugest(1);" class="close"><i class="fa fa-times" aria-hidden="true"></i></a>
                                </div>
                                <div class="sugestao_funcao-sinonimo_txt">
                                    Sugerimos que troque a função de <strong><span id="spanFuncaoSininimoPesquisada1"></span></strong>para <strong><span class="sugestao_funcao-sinonimo_functions" id="spanFuncaoSugestao1"></span></strong>pois esta função é <strong><span id="spanPorcentagem1"></span>%</strong> mais utilizada pelos recrutadores
                                    para buscar currículos.                                    
                                </div>
                                <div class="sugestao_funcao-sinonimo_functions">
                                </div>
                            </div>
                        </div>
                    </div>
                    <%-- FIM Sugest de funnção sinonomo 1 --%>

                    <%-- FIM: Linha Funções Pretendidas 1 --%>
                    <%-- Linha Funções Pretendidas 2 --%>
                    <div class="linha">
                        <asp:Label ID="Label4" runat="server" CssClass="label_principal" Text="Função Pretendida 2"
                            AssociatedControlID="txtFuncaoPretendida2" />
                        <div class="container_campo">
                            <asp:UpdatePanel ID="upTxtFuncaoPretendida2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtFuncaoPretendida2" onchange="SugestaoFuncao(this);" runat="server" CssClass="textbox_padrao" Columns="42"
                                        AutoPostBack="True">
                                    </asp:TextBox>
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
                                Anos
                            </label>
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
                                Meses
                            </label>
                        </div>
                    </div>
                    <%-- iNICIO Sugest de funnção sinonomo 2 --%>
                    <div class="linha" id="divSugestFuncaoSinonimo2" style="display: none;">
                        <asp:Label ID="Label6" runat="server" CssClass="label_principal" Text="." Style="color: white;" />
                        <div class="container_campo">

                            <div class="sugestao_funcao-sinonimo">
                                <div class="sugestao_funcao-sinonimo_title">
                                    <div>Sugestão BNE</div>
                                    <a onclick="CloseSugest(2);" class="close"><i class="fa fa-times" aria-hidden="true"></i></a>
                                </div>
                                <div class="sugestao_funcao-sinonimo_txt">
                                    Sugerimos que troque a função de <strong><span id="spanFuncaoSininimoPesquisada2"></span></strong>para <strong><span class="sugestao_funcao-sinonimo_functions" id="spanFuncaoSugestao2"></span></strong>pois esta função é 
                                      <strong><span id="spanPorcentagem2"></span>%</strong> mais utilizada pelos recrutadores
                                    para buscar currículos.                                    
                                </div>
                                <div class="sugestao_funcao-sinonimo_functions">
                                </div>
                            </div>
                        </div>
                    </div>
                    <%-- FIM Sugest de funnção sinonomo 2 --%>
                    <%-- FIM: Linha Funções Pretendidas 2 --%>
                    <%-- Linha Funções Pretendidas 3 --%>
                    <div class="linha">
                        <asp:Label ID="Label8" runat="server" CssClass="label_principal" Text="Função Pretendida 3"
                            AssociatedControlID="txtFuncaoPretendida3" />
                        <div class="container_campo">
                            <asp:UpdatePanel ID="upTxtFuncaoPretendida3" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtFuncaoPretendida3" onchange="SugestaoFuncao(this);" runat="server" CssClass="textbox_padrao" Columns="42"></asp:TextBox>
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
                                Anos
                            </label>
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
                                Meses
                            </label>
                        </div>
                    </div>
                    <%-- iNICIO Sugest de funnção sinonomo 3--%>
                    <div class="linha" id="divSugestFuncaoSinonimo3" style="display: none;">
                        <asp:Label ID="Label7" runat="server" CssClass="label_principal" Text="." Style="color: white;" />
                        <div class="container_campo">

                            <div class="sugestao_funcao-sinonimo">
                                <div class="sugestao_funcao-sinonimo_title">
                                    <div>Sugestão BNE</div>
                                    <a onclick="CloseSugest(3);" class="close"><i class="fa fa-times" aria-hidden="true"></i></a>
                                </div>
                                <div class="sugestao_funcao-sinonimo_txt">
                                    Sugerimos que troque a função de <strong><span id="spanFuncaoSininimoPesquisada3"></span></strong>para <strong><span class="sugestao_funcao-sinonimo_functions" id="spanFuncaoSugestao3"></span></strong>pois esta função é <strong><span id="spanPorcentagem3"></span>%</strong> mais utilizada pelos recrutadores
                                    para buscar currículos.                                    
                                </div>
                                <div class="sugestao_funcao-sinonimo_functions">
                                </div>
                            </div>
                        </div>
                    </div>
                    <%-- FIM Sugest de funnção sinonomo 3 --%>
                    <%-- FIM: Linha Funções Pretendidas 3 --%>
                    <%-- Linha Pretensão Salarial--%>
                    <div class="linha">
                        <asp:Label ID="lblPretensaoSalarial" runat="server" CssClass="label_principal" Text="Pretensão Salarial R$"
                            AssociatedControlID="txtPretensaoSalarial" />
                        <div class="container_campo container_campoAdjust_cadMini">
                            <asp:UpdatePanel ID="upTxtPretensaoSalarial" runat="server" UpdateMode="Conditional"
                                RenderMode="Inline">
                                <ContentTemplate>
                                    <div>
                                        <span runat="server" id="avisoSalarionivalido" class="validador" enableviewstate="false"></span>
                                    </div>
                                    <asp:TextBox runat="server" ID="txtPretensaoSalarial" name="txtPretensaoSalarial" MaxLength="12" CssClass="textbox_padrao campo_obrigatorio inputPretensaoSalarial" />
                                    <asp:HiddenField ID="hdfValorSalarioMinimo" runat="server" />
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
                    <asp:Panel runat="server" ID="pnlPoliticaPrivacidade" CssClass="linha politica-privacidade">
                        <div>
                            <asp:CustomValidator runat="server" ValidationGroup="CadastroCurriculoMini"
                                ClientValidationFunction="checarPoliticaPrivacidade" CssClass=""></asp:CustomValidator>
                        </div>
                        <input type="checkbox" id="cbPoliticaPrivacidade" runat="server" ClientIDMode="Static" />
                        <%--<asp:CheckBox runat="server" ID="cbPoliticaPrivacidade" Checked="False" ClientIDMode="Static" />--%>
                        <a onclick="javascript:AbrirModalPoliticaPrivacidade();" id="politica-privacidade">Concordo com a política de privacidade</a>
                    </asp:Panel>
                    <div class="linha">
                        <label class="setlabel-margin-left">Ao clicar em "Salvar e Avançar" aceito receber propostas de emprego por celular ou e-mail.</label>
                    </div>
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
<Employer:DynamicScript runat="server" Src="/js/local/Forms/CadastroCurriculo/MiniCurriculo.js" type="text/javascript" />
<Employer:DynamicScript runat="server" Src="/js/jquery.simulate.js" type="text/javascript" />
<uc1:ucPoliticaPrivacidade runat="server" ID="ucPoliticaPrivacidade" />
