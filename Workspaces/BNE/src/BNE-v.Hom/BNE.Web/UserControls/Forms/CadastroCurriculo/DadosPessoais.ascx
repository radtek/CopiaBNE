<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DadosPessoais.ascx.cs"
    Inherits="BNE.Web.UserControls.Forms.CadastroCurriculo.DadosPessoais" %>
<%@ Register Src="~/UserControls/ucEndereco.ascx" TagName="ucEndereco" TagPrefix="uc1" %>
<%@ Register Src="../../Modais/ModalDegustacaoCandidatura.ascx" TagName="ModalDegustacaoCandidatura" TagPrefix="uc2" %>

<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/CadastroCurriculo/DadosPessoais.css" type="text/css" rel="stylesheet" />
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/ModalConfirmacaoEnvioCurriculo.css" type="text/css" rel="stylesheet" />
<link href="../../../icons/stylecv.css" rel="stylesheet" />

<Employer:DynamicScript runat="server" Src="/js/local/Forms/CadastroCurriculo/DadosPessoais.js" type="text/javascript" />

<script>
    $(document).ready(function () {
        $('[data-toggle="tooltip"]').tooltip();
    });
</script>
<style>
    .CelularCheckedStyle { float: right !important; margin-top: 2px !important; }
</style>

<!-- Tabs -->
<asp:UpdatePanel ID="upnAbas" runat="server">
    <ContentTemplate>
        <h1>
            <asp:Literal ID="litTitulo" runat="server" Text="Cadastro de Currículo">
            </asp:Literal>
        </h1>
        <asp:Panel ID="pnlAbas" runat="server">
            <div class="linha_abas">
                <div class="abas_nova ">
                    <span class="aba_esquerda_nao_selecionada_inicio aba">
                        <asp:LinkButton ID="btlMiniCurriculo" runat="server" OnClick="btlMiniCurriculo_Click"
                            CssClass="texto_abas" ValidationGroup="CadastroDadosPessoais" Text="Mini Currículo"></asp:LinkButton>
                    </span>
                </div>
                <div class="abas_nova">
                    <span class="aba_esquerda_selecionada aba">
                        <asp:Label ID="Label3" runat="server" Text="Dados Pessoais e Profissionais" CssClass="texto_abas_selecionada"></asp:Label>
                    </span>
                </div>
                <div class="abas_nova">
                    <span class="aba_esquerda_nao_selecionada aba">
                        <asp:LinkButton ID="btlFormacaoCursos" runat="server" OnClick="btlFormacaoCursos_Click"
                            ValidationGroup="CadastroDadosPessoais" Text="Formação e Cursos" CssClass="texto_abas"></asp:LinkButton>
                    </span>
                </div>
                <div class="abas_nova">
                    <span class="aba_esquerda_nao_selecionada aba">
                        <asp:LinkButton ID="btlDadosComplementares" runat="server" OnClick="btlDadosComplementares_Click"
                            ValidationGroup="CadastroDadosPessoais" Text="Dados Complementares" CssClass="texto_abas"></asp:LinkButton>
                    </span>
                </div>
                <div class="abas_nova">
                    <span class="aba_esquerda_nao_selecionada aba">
                        <asp:LinkButton ID="btlRevisaoDados" runat="server" OnClick="btlRevisaoDados_Click"
                            ValidationGroup="CadastroDadosPessoais" Text="Conferir" CssClass="texto_abas"></asp:LinkButton>
                    </span>
                </div>
            </div>
            <div class="abas" style="display: none">
                <span class="aba_fundo">
                    <asp:LinkButton ID="btlGestao" runat="server" OnClick="btlGestao_Click" ValidationGroup="CadastroDadosPessoais"
                        CausesValidation="true" Text="Gestão"></asp:LinkButton>
                </span>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
<!-- FIM: Tabs -->
<!-- Painel: Dados Pessoais -->
 <body onkeydown="return (event.keyCode!=13)">
<div class="interno_abas">
    <h2 class="titulo_painel_padrao" style="display: none">
        <asp:Label ID="lblTituloDadosPessoais" runat="server" Text="Dados Pessoais" />
    </h2>
    <asp:Panel ID="pnlDadosPessoais" runat="server" CssClass="painel_padrao">
        <div class="painel_padrao_topo">
            &nbsp;
        </div>
        <p class="texto_marcadores_obrigatorio  cadastro_curriculo">
            Os campos marcados com um
            <img alt="*" src="img/icone_obrigatorio.gif" />
            são obrigatórios para o cadastro de seu currículo.
        </p>
        <%-- RG --%>
        <div class="linha">
            <asp:Label ID="Label4" CssClass="label_principal" AssociatedControlID="txtNumeroRG"
                runat="server" Text="RG"></asp:Label>
            <div class="container_campo">
                <componente:AlfaNumerico ID="txtNumeroRG" runat="server" ValidationGroup="CadastroDadosPessoais"
                    Obrigatorio="false" ExpressaoValidacao="^\w{1,}(-){0,1}\w{1,}$" MaxLength="20"
                    CssClassTextBox="textbox_padrao" Columns="20" OnValorAlterado="txtNumeroRG_ValorAlterado" />
            </div>
            <asp:Label ID="Label5" runat="server" AssociatedControlID="txtOrgaoEmissorRG" Text="Órgão Emissor"></asp:Label>
            <div class="container_campo">
                <componente:AlfaNumerico ID="txtOrgaoEmissorRG" runat="server" Columns="4" MaxLength="4"
                    ValidationGroup="CadastroDadosPessoais" Obrigatorio="False" Enabled="false" Tipo="LetraMaiuscula"
                    CssClassTextBox="textbox_padrao" ExpressaoValidacao="([A-Z,a-z,ÁáÂâÃãÉéÊêÍíÓóÔôÕõÚúÛûÇç]){1,4}" />
            </div>
        </div>
        <%-- Fim: RG --%>
        <%-- Linha Estado Civil --%>
        <div class="linha">
            <asp:Label ID="lblEstadoCivil" CssClass="label_principal" runat="server" Text="Estado Civil"
                AssociatedControlID="ddlEstadoCivil" />
            <div class="container_campo">
                <div>
                    <asp:CustomValidator ID="cvEstadoCivil" runat="server" ControlToValidate="ddlEstadoCivil"
                        ValidationGroup="CadastroDadosPessoais" ErrorMessage="Estado Civil Inválido. Selecione!"
                        ClientValidationFunction="cvEstadoCivil_Validate"></asp:CustomValidator>
                </div>
                <asp:DropDownList ID="ddlEstadoCivil" runat="server" CssClass="textbox_padrao campo_obrigatorio">
                </asp:DropDownList>
            </div>
        </div>
        <%-- FIM: Linha Estado Civil --%>
        <%-- Painel Endereco --%>
                  <uc1:ucEndereco ID="ucEndereco" runat="server" />
        <%-- FIM: Painel Endereco --%>
        <%-- Linha Celular --%>
        <div class="linha">
            <asp:Label ID="lblCelular" AssociatedControlID="txtCelular" CssClass="label_principal"
                runat="server" Text="Telefone Celular" />
            <div class="container_campo">
                <componente:Telefone ID="txtCelular" runat="server" ValidationGroup="CadastroDadosPessoais"
                    CssClassTextBoxFone="textbox_padrao nr_telefone" Tipo="Celular" />
                <asp:Image ID="CelularChecked" runat="server" ImageUrl="~/img/img_icone_check_16x16.png" CssClass="CelularCheckedStyle"
                    data-toggle="tooltip" data-placement="top" title="Número de celular confirmado." />
            </div>
        </div>
        <%-- FIM: Linha Celular --%>
        <%-- Linha Residencial --%>
        <div class="linha">
            <asp:Label ID="lblTelefoneResidencial" AssociatedControlID="txtTelefoneResidencial"
                CssClass="label_principal" runat="server" Text="Telefone Fixo Residencial" />
            <div class="container_campo">
                <componente:Telefone ID="txtTelefoneResidencial" runat="server" Obrigatorio="False"
                    CssClassTextBoxDDI="textbox_padrao ddi" CssClassTextBoxDDD="textbox_padrao ddd"
                    CssClassTextBoxFone="textbox_padrao" ValidationGroup="CadastroDadosPessoais" />
            </div>
        </div>
        <%-- FIM: Linha Residencial --%>
        <%-- Linha Recado --%>
        <div class="linha">
            <asp:Label ID="lblTelefoneRecado" AssociatedControlID="txtTelefoneRecado" CssClass="label_principal"
                runat="server" Text="Telefone Fixo Recado" />
            <div class="container_campo">
                <componente:Telefone ID="txtTelefoneRecado" runat="server" Obrigatorio="False" CssClassTextBoxDDD="textbox_padrao ddd"
                    CssClassTextBoxDDI="textbox_padrao ddi" CssClassTextBoxFone="textbox_padrao"
                    ValidationGroup="CadastroDadosPessoais" />
            </div>
            <asp:Label ID="lblRecadoFalarCom" AssociatedControlID="txtTelefoneRecadoFalarCom"
                runat="server" Text="Falar com" />
            <div class="container_campo">
                <asp:TextBox ID="txtTelefoneRecadoFalarCom" runat="server" Columns="32" MaxLength="50"
                    CssClass="textbox_padrao"></asp:TextBox>
            </div>
        </div>
        <%-- FIM: Linha Recado --%>
        <%-- Linha Celular Recado --%>
        <div class="linha">
            <asp:Label ID="lblCelularRecado" AssociatedControlID="txtCelularRecado" CssClass="label_principal"
                runat="server" Text="Celular Recado" />
            <div class="container_campo">
                <componente:Telefone ID="txtCelularRecado" runat="server" Obrigatorio="False" CssClassTextBoxDDD="textbox_padrao ddd"
                    CssClassTextBoxDDI="textbox_padrao ddi" CssClassTextBoxFone="textbox_padrao celular"
                    ValidationGroup="CadastroDadosPessoais" Tipo="Celular" />
            </div>
            <asp:Label ID="lblCelularRecadoFalarCom" AssociatedControlID="txtCelularRecadoFalarCom"
                runat="server" Text="Falar com" />
            <div class="container_campo">
                <asp:TextBox ID="txtCelularRecadoFalarCom" runat="server" Columns="32" MaxLength="50"
                    CssClass="textbox_padrao"></asp:TextBox>
            </div>
        </div>
        <%-- FIM: Linha Celular Recado --%>
    </asp:Panel>
    <!-- FIM Painel: Dados Pessoais -->

    <!-- Painel: Experiência Profissional 1 -->
    <h2 class="titulo_painel_padrao">
        <asp:Label ID="lblTituloExperienciaProfissional1" runat="server" Text="Experiência Profissional - Última Empresa (a mais recente)" />
    </h2>
    <asp:UpdatePanel ID="upExperienciaProfissional1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlExperienciaProfissional" runat="server" CssClass="painel_padrao">
                <script type="text/javascript">
                    function validarUltimoSalario() {
                        $('#cphConteudo_ucDadosPessoais_avisoSI').hide();
                        if ($('#cphConteudo_ucDadosPessoais_txtEmpresa1').val() != "") {
                            if (parseFloat($('#cphConteudo_ucDadosPessoais_txtUltimoSalario').val()) > 0) {
                                $('#cphConteudo_ucDadosPessoais_avisoSI').hide();
                                $('#cphConteudo_ucDadosPessoais_btnSalvar').show();
                                $('#cphConteudo_ucDadosPessoais_btnSalvar').removeClass('hideAviso');
                            }
                            else {
                                $('#cphConteudo_ucDadosPessoais_avisoSI').show();
                                $('#cphConteudo_ucDadosPessoais_btnSalvar').hide();
                            }
                        }
                    }
                </script>
                <div class="painel_padrao_topo">
                    &nbsp;
                </div>
                <%-- Linha Empresa --%>
                <div class="linha">
                    <asp:Label ID="lblEmpresa1" CssClass="label_principal" runat="server" Text="Nome da Empresa"
                        AssociatedControlID="txtEmpresa1" />
                    <div class="container_campo">
                        <div>
                            <asp:RequiredFieldValidator ID="rfvEmpresa1" runat="server" ControlToValidate="txtEmpresa1"
                                ValidationGroup="CadastroDadosPessoais" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        <asp:TextBox ID="txtEmpresa1" runat="server" CssClass="textbox_padrao" Columns="60"
                            MaxLength="60" AutoPostBack="true" OnTextChanged="txtEmpresa1_TextChanged"></asp:TextBox>
                    </div>
                </div>
                <%-- FIM: Linha Empresa --%>
                <%-- Linha Atividade Empresa --%>
                <div class="linha">
                    <asp:Label ID="lblAtividadeEmpresa1" CssClass="label_principal" runat="server" Text="A Empresa Trabalha com"
                        AssociatedControlID="ddlAtividadeEmpresa1" />
                    <div class="container_campo">
                        <div>
                            <asp:CustomValidator ID="cvAtividadeEmpresa1" ValidationGroup="CadastroDadosPessoais"
                                runat="server" ControlToValidate="ddlAtividadeEmpresa1" ErrorMessage="Campo Obrigatório"
                                ClientValidationFunction="cvAtividadeExercida1_Validate">
                            </asp:CustomValidator>
                        </div>
                        <asp:DropDownList ID="ddlAtividadeEmpresa1" CssClass="textbox_padrao" runat="server"
                            OnSelectedIndexChanged="ddlAtividadeEmpresa1_SelectedIndexChanged" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                </div>
                <%-- FIM: Linha Atividade Empresa --%>
                <%-- Linha Data Admissão --%>
                <div class="linha">
                    <asp:Label ID="lblDataAdmissao1" CssClass="label_principal" runat="server" Text="Data de Admissão"
                        AssociatedControlID="txtDataAdmissao1" />
                    <div class="container_campo">
                        <componente:Data ID="txtDataAdmissao1" runat="server" MensagemErroIntervalo="Data Inválida"
                            CssClassTextBox="textbox_padrao" ValidationGroup="CadastroDadosPessoais" />
                    </div>
                    <%-- Linha Data Demissão --%>
                    <asp:Label ID="lblDataDemissao1" runat="server" Text="Data de Demissão" AssociatedControlID="txtDataDemissao1" />
                    <div class="container_campo">
                        <componente:Data ID="txtDataDemissao1" runat="server" ValidationGroup="CadastroDadosPessoais"
                            MensagemErroIntervalo="Data Inválida" Obrigatorio="false" CssClassTextBox="textbox_padrao" />
                        <div runat="server" id="divDtaDemissaoAviso1" class="tooltips_aviso"><span>A data de Admissão deve ser menor que a data de Demissão.</span></div>
                    </div>
                    <%-- FIM: Linha Data Demissão --%>
                </div>
                <%-- FIM: Linha Data Admissão --%>
                <%-- Linha Função Exercida --%>
                <div class="linha">
                    <asp:Label ID="lblFuncaoExercida1" CssClass="label_principal" runat="server" Text="Função Exercida"
                        AssociatedControlID="txtFuncaoExercida1" />
                    <div class="container_campo">
                        <div>
                            <asp:RequiredFieldValidator ID="rfvFuncaoExercida1" runat="server" ControlToValidate="txtFuncaoExercida1"
                                ValidationGroup="CadastroDadosPessoais"></asp:RequiredFieldValidator>
                        </div>
                        <asp:TextBox ID="txtFuncaoExercida1" runat="server" CssClass="textbox_padrao" Columns="60" MaxLength="60"></asp:TextBox>
                    </div>
                </div>
                <%-- FIM: Linha Função Exercida --%>
                <%-- Linha Atividades Exercidas --%>
                <div class="linha">
                    <asp:Label ID="lblAtividadeExercida1" CssClass="label_principal" runat="server" AssociatedControlID="txtAtividadeExercida1">Atribuições<br />(Escreva no mínimo 3 tarefas)</asp:Label>
                    <div class="container_campo">
                        <componente:AlfaNumerico ID="txtAtividadeExercida1" runat="server" MaxLength="2000"
                            OnBlurClient="txtAtividadeExercida1_OnBlur" Rows="1000" ValidationGroup="CadastroDadosPessoais"
                            TextMode="Multiline" CssClassTextBox="textbox_padrao multiline atividades_exercidas"
                            Obrigatorio="false" MensagemErroObrigatorio="Campo obrigatório" OnKeyUpClient="txtAtividadeExercida1_KeyUp" />
                    </div>
                    <asp:Panel runat="server" ID="pnlBoxSugestaoTarefas" CssClass="BoxSugestaoTarefas">
                        <%--<span id="contadorcatacter1"></span>
                        <span id="GraficoQualidade1"></span>--%>
                        <%--<asp:Image id="GraficoQualidade1" runat="server" />--%>
                        <span id="GraficoQualidade1">
                            <asp:Literal runat="server" ID="ltGraficoQualidade1"></asp:Literal></span>
                        <div class="seta_apontador_esq">
                        </div>
                        <div class="box_conteudo sugestao ">
                            <asp:Label ID="lblSugestaoTarefas1" runat="server" CssClass="label_principal" AssociatedControlID="txtSugestaoTarefas1"
                                Text="Sugestão de Tarefas"></asp:Label>
                            <div class="container_campo">
                                <asp:TextBox ID="txtSugestaoTarefas1" CssClass="textbox_padrao sugestao_tarefas"
                                    TextMode="MultiLine" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
                <%-- Linha Ultimo Salario --%>
                <div class="linha">
                    <asp:Label ID="Label8" runat="server" Text="Último Salário" CssClass="label_principal"
                        AssociatedControlID="txtUltimoSalario" />
                    <div class="container_campo container_campoAdjust_dpessoais">
                        <div><span id="avisoSI" runat="server" class="validador">O valor do salário deve ser maior que zero.</span></div>
                        <asp:TextBox runat="server" onBlur="validarUltimoSalario();" ID="txtUltimoSalario" name="txtUltimoSalario" MaxLength="12" CssClass="textbox_padrao campo_obrigatorio inputSalario" />
                    </div>
                </div>
                <%-- FIM: Linha Ultimo Salario --%>
                <%-- FIM: Atividades Exercidas --%>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSalvar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <!-- FIM Painel: Experiência Profissional 1 -->
    <!-- Painel: Experiência Profissional 2 -->
    <h2 class="titulo_painel_padrao">
        <asp:Label ID="lblTituloExperienciaProfissional2" runat="server" Text="Experiência Profissional - Penúltima Empresa" />
    </h2>
    <asp:UpdatePanel ID="upExperienciaProfissional2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="Panel2" runat="server" CssClass="painel_padrao">
                <div class="painel_padrao_topo">
                    &nbsp;
                </div>
                <%-- Linha Empresa --%>
                <div class="linha">
                    <asp:Label ID="lblEmpresa2" CssClass="label_principal" runat="server" Text="Nome da Empresa"
                        AssociatedControlID="txtEmpresa2" />
                    <div class="container_campo">
                        <div>
                            <asp:RequiredFieldValidator ID="rfvEmpresa2" runat="server" ControlToValidate="txtEmpresa2"
                                ValidationGroup="CadastroDadosPessoais"></asp:RequiredFieldValidator>
                        </div>
                        <asp:TextBox ID="txtEmpresa2" runat="server" CssClass="textbox_padrao" Columns="60"
                            MaxLength="60" AutoPostBack="true" OnTextChanged="txtEmpresa2_TextChanged"></asp:TextBox>
                    </div>
                </div>
                <%-- FIM: Linha Empresa --%>
                <%-- Linha Atividade Empresa --%>
                <div class="linha">
                    <asp:Label ID="lblAtividadeEmpresa2" CssClass="label_principal" runat="server" Text="A Empresa Trabalha com"
                        AssociatedControlID="ddlAtividadeEmpresa2" />
                    <div class="container_campo">
                        <div>
                            <asp:CustomValidator ID="cvAtividadeEmpresa2" runat="server" ValidationGroup="CadastroDadosPessoais"
                                ControlToValidate="ddlAtividadeEmpresa2" ErrorMessage="Campo Obrigatório" ClientValidationFunction="cvAtividadeExercida2_Validate">
                            </asp:CustomValidator>
                        </div>
                        <asp:DropDownList ID="ddlAtividadeEmpresa2" CssClass="textbox_padrao" runat="server"
                            OnSelectedIndexChanged="ddlAtividadeEmpresa2_SelectedIndexChanged" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                </div>
                <%-- FIM: Linha Atividade Empresa --%>
                <%-- Linha Data Admissão --%>
                <div class="linha">
                    <asp:Label ID="lblDataAdmissao2" CssClass="label_principal" runat="server" Text="Data de Admissão"
                        AssociatedControlID="txtDataAdmissao2" />
                    <div class="container_campo">
                        <componente:Data ID="txtDataAdmissao2" runat="server" CssClassTextBox="textbox_padrao"
                            MensagemErroIntervalo="Data Inválida" ValidationGroup="CadastroDadosPessoais" />
                    </div>
                    <%-- Linha Data Demissão --%>
                    <asp:Label ID="lblDataDemissao2" runat="server" Text="Data de Demissão" AssociatedControlID="txtDataDemissao2" />
                    <div class="container_campo">
                        <componente:Data ID="txtDataDemissao2" runat="server" CssClassTextBox="textbox_padrao"
                            MensagemErroIntervalo="Data Inválida" Obrigatorio="false" ValidationGroup="CadastroDadosPessoais" />
                        <div runat="server" id="divDtaDemissaoAviso2" class="tooltips_aviso"><span>A data de Admissão deve ser menor que a data de Demissão.</span></div>

                    </div>
                    <%-- FIM: Linha Data Demissão --%>
                </div>
                <%-- FIM: Linha Data Admissão --%>
                <%-- Linha Função Exercida --%>
                <div class="linha">
                    <asp:Label ID="lblFuncaoExercida2" CssClass="label_principal" runat="server" Text="Função Exercida"
                        AssociatedControlID="txtFuncaoExercida2" />
                    <div class="container_campo">
                        <div>
                            <asp:RequiredFieldValidator ID="rfvFuncaoExercida2" runat="server" ControlToValidate="txtFuncaoExercida2"
                                ValidationGroup="CadastroDadosPessoais"></asp:RequiredFieldValidator>
                        </div>
                        <asp:TextBox ID="txtFuncaoExercida2" runat="server" CssClass="textbox_padrao" Columns="60"
                            MaxLength="60"></asp:TextBox>
                    </div>
                </div>
                <%-- FIM: Linha Função Exercida --%>
                <%-- Linha Atividades Exercidas --%>
                <div class="linha">
                    <asp:Label ID="lblAtividadeExercida2" CssClass="label_principal" runat="server" AssociatedControlID="txtAtividadeExercida2">Atribuições<br />(Escreva no mínimo 3 tarefas)</asp:Label>
                    <div class="container_campo">
                        <componente:AlfaNumerico ID="txtAtividadeExercida2" runat="server" Rows="1000" MaxLength="2000"
                            ValidationGroup="CadastroDadosPessoais" OnBlurClient="txtAtividadeExercida2_OnBlur"
                            CssClassTextBox="textbox_padrao multiline atividades_exercidas" TextMode="Multiline"
                            Obrigatorio="false" MensagemErroObrigatorio="Campo obrigatório" OnKeyUpClient="txtAtividadeExercida2_KeyUp" />
                    </div>
                    <asp:Panel runat="server" ID="Panel1" CssClass="BoxSugestaoTarefas">
                        <span id="GraficoQualidade2">
                            <asp:Literal runat="server" ID="ltGraficoQualidade2"></asp:Literal></span>
                        <div class="seta_apontador_esq">
                        </div>
                        <div class="box_conteudo sugestao">
                            <asp:Label ID="lblSugestaoTarefas2" runat="server" CssClass="label_principal" AssociatedControlID="txtSugestaoTarefas2"
                                Text="Sugestão de Tarefas"></asp:Label>
                            <div class="container_campo">
                                <asp:TextBox ID="txtSugestaoTarefas2" CssClass="textbox_padrao sugestao_tarefas"
                                    TextMode="MultiLine" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
                <%-- Linha Ultimo Salario --%>
                <div class="linha">
                    <asp:Label ID="Label1" runat="server" Text="Último Salário" CssClass="label_principal"
                        AssociatedControlID="txtUltimoSalario2" />
                    <div class="container_campo container_campoAdjust_dpessoais">
                        <asp:TextBox runat="server" ID="txtUltimoSalario2" name="txtUltimoSalario2" MaxLength="12" CssClass="textbox_padrao inputSalario" />
                        <%--<componente:ValorDecimal ID="txtUltimoSalario2" runat="server" CasasDecimais="0" ValidationGroup="CadastroDadosPessoais"
                            ValorMinimo="1" ValorMaximo="999999" Obrigatorio="false" CssClassTextBox="textbox_padrao textarea_padraoAdjust_dpessoais" MensagemErroObrigatorio="Campo obrigatório" />
                        <span class="decimais_dpessoais">,00</span>--%>
                    </div>
                </div>
                <%-- FIM: Linha Ultimo Salario --%>
                <%-- FIM: Atividades Exercidas --%>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSalvar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <!-- FIM Painel: Experiência Profissional 2 -->
    <!-- Painel: Experiência Profissional 3 -->
    <h2 class="titulo_painel_padrao">
        <asp:Label ID="lblTituloExperienciaProfissional3" runat="server" Text="Experiência Profissional - Antepenúltima Empresa" />
    </h2>
    <asp:UpdatePanel ID="upExperienciaProfissional3" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="Panel4" runat="server" CssClass="painel_padrao">
                <div class="painel_padrao_topo">
                    &nbsp;
                </div>
                <%-- Linha Empresa --%>
                <div class="linha">
                    <asp:Label ID="lblEmpresa3" CssClass="label_principal" runat="server" Text="Nome da Empresa"
                        AssociatedControlID="txtEmpresa3" />
                    <div class="container_campo">
                        <div>
                            <asp:RequiredFieldValidator ID="rfvEmpresa3" runat="server" ControlToValidate="txtEmpresa3"
                                ValidationGroup="CadastroDadosPessoais"></asp:RequiredFieldValidator>
                        </div>
                        <asp:TextBox ID="txtEmpresa3" runat="server" CssClass="textbox_padrao" Columns="60"
                            MaxLength="60" AutoPostBack="true" OnTextChanged="txtEmpresa3_TextChanged"></asp:TextBox>
                    </div>
                </div>
                <%-- FIM: Linha Empresa --%>
                <%-- Linha Atividade Empresa --%>
                <div class="linha">
                    <asp:Label ID="lblAtividadeEmpresa3" CssClass="label_principal" runat="server" Text="A Empresa Trabalha com"
                        AssociatedControlID="ddlAtividadeEmpresa3" />
                    <div class="container_campo">
                        <div>
                            <asp:CustomValidator ID="cvAtividadeEmpresa3" runat="server" ControlToValidate="ddlAtividadeEmpresa3"
                                ValidationGroup="CadastroDadosPessoais" ErrorMessage="Campo Obrigatório" ClientValidationFunction="cvAtividadeExercida3_Validate">
                            </asp:CustomValidator>
                        </div>
                        <asp:DropDownList ID="ddlAtividadeEmpresa3" CssClass="textbox_padrao" runat="server"
                            OnSelectedIndexChanged="ddlAtividadeEmpresa3_SelectedIndexChanged" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                </div>
                <%-- FIM: Linha Atividade Empresa --%>
                <%-- Linha Data Admissão --%>
                <div class="linha">
                    <asp:Label ID="lblDataAdmissao3" CssClass="label_principal" runat="server" Text="Data de Admissão"
                        AssociatedControlID="txtDataAdmissao3" />
                    <div class="container_campo">
                        <componente:Data ID="txtDataAdmissao3" runat="server" CssClassTextBox="textbox_padrao"
                            MensagemErroIntervalo="Data Inválida" ValidationGroup="CadastroDadosPessoais" />
                    </div>
                    <%-- Linha Data Demissão --%>
                    <asp:Label ID="lblDataDemissao3" runat="server" Text="Data de Demissão" AssociatedControlID="txtDataDemissao3" />
                    <div class="container_campo">
                        <componente:Data ID="txtDataDemissao3" runat="server" CssClassTextBox="textbox_padrao"
                            MensagemErroIntervalo="Data Inválida" Obrigatorio="false" ValidationGroup="CadastroDadosPessoais" />
                        <div runat="server" id="divDtaDemissaoAviso3" class="tooltips_aviso"><span>A data de Admissão deve ser menor que a data de Demissão.</span></div>

                    </div>
                    <%-- FIM: Linha Data Demissão --%>
                </div>
                <%-- FIM: Linha Data Admissão --%>
                <%-- Linha Função Exercida --%>
                <div class="linha">
                    <asp:Label ID="lblFuncaoExercida3" CssClass="label_principal" runat="server" Text="Função Exercida"
                        AssociatedControlID="txtFuncaoExercida3" />
                    <div class="container_campo">
                        <div>
                            <asp:RequiredFieldValidator ID="rfvFuncaoExercida3" runat="server" ControlToValidate="txtFuncaoExercida3"
                                ValidationGroup="CadastroDadosPessoais"></asp:RequiredFieldValidator>
                        </div>
                        <asp:TextBox ID="txtFuncaoExercida3" runat="server" CssClass="textbox_padrao" Columns="60"
                            MaxLength="60"></asp:TextBox>
                    </div>
                </div>
                <%-- FIM: Linha Função Exercida --%>
                <%-- Linha Atividades Exercidas --%>
                <div class="linha">
                    <asp:Label ID="lblAtividadeExercida3" CssClass="label_principal" runat="server" AssociatedControlID="txtAtividadeExercida3">Atribuições<br />(Escreva no mínimo 3 tarefas)</asp:Label>
                    <div class="container_campo">
                        <componente:AlfaNumerico ID="txtAtividadeExercida3" runat="server" Rows="1000" MaxLength="2000"
                            OnBlurClient="txtAtividadeExercida3_OnBlur" ValidationGroup="CadastroDadosPessoais"
                            CssClassTextBox="textbox_padrao multiline atividades_exercidas" TextMode="Multiline"
                            Obrigatorio="false" MensagemErroObrigatorio="Campo obrigatório" OnKeyUpClient="txtAtividadeExercida3_KeyUp" />
                    </div>
                    <asp:Panel runat="server" ID="Panel3" CssClass="BoxSugestaoTarefas">
                        <span id="GraficoQualidade3">
                            <asp:Literal runat="server" ID="ltGraficoQualidade3"></asp:Literal></span>
                        <div class="seta_apontador_esq">
                        </div>
                        <div class="box_conteudo sugestao">
                            <asp:Label ID="lblSugestaoTarefas3" runat="server" CssClass="label_principal" AssociatedControlID="txtSugestaoTarefas3"
                                Text="Sugestão de Tarefas"></asp:Label>
                            <div class="container_campo">
                                <asp:TextBox ID="txtSugestaoTarefas3" CssClass="textbox_padrao sugestao_tarefas"
                                    TextMode="MultiLine" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
                <%-- Linha Ultimo Salario --%>
                <div class="linha">
                    <asp:Label ID="Label2" runat="server" Text="Último Salário" CssClass="label_principal"
                        AssociatedControlID="txtUltimoSalario3" />
                    <div class="container_campo container_campoAdjust_dpessoais">
                        <asp:TextBox runat="server" ID="txtUltimoSalario3" name="txtUltimoSalario3" MaxLength="12" CssClass="textbox_padrao inputSalario" />

                        <%--<componente:ValorDecimal ID="txtUltimoSalario3" runat="server" CasasDecimais="0" ValidationGroup="CadastroDadosPessoais"
                            ValorMinimo="1" ValorMaximo="999999" Obrigatorio="false" CssClassTextBox="textbox_padrao textarea_padraoAdjust_dpessoais" MensagemErroObrigatorio="Campo obrigatório" />
                        <span class="decimais_dpessoais">,00</span>--%>
                    </div>
                </div>
                <%-- FIM: Linha Ultimo Salario --%>
                <%-- FIM: Atividades Exercidas --%>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSalvar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <!-- FIM Painel: Experiência Profissional 3 -->

    <asp:UpdatePanel runat="server" ID="upExperienciaProfissionais" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="tooltips" id="divSlowDown">
                <%--<asp:UpdatePanel
                        ID="upPnlBotoesFlutuantes"
                        runat="server"
                        UpdateMode="Conditional">
                        <ContentTemplate>--%>
                <%--<asp:Panel
                                ID="pnlBotoesFlutuantes"
                                runat="server">--%>
                <span>
                    <asp:LinkButton runat="server" ID="lnkAddExperiencia" OnClick="btnAddExperiencia_Click" Text="Adicionar mais experiências"></asp:LinkButton>
                    <%--<asp:Button ID="btnAddExperiencia" runat="server" Text="Adicionar mais experiências" OnClick="btnAddExperiencia_Click" CssClass="btn btn-warning btn-small"/>--%>
                </span>
                <%--</asp:Panel>--%>
                <%--</ContentTemplate>
                    </asp:UpdatePanel>--%>
            </div>
            <!-- Experiência profisisonal 04 -->
            <asp:Panel ID="pnExperienciaProfissional4" runat="server" CssClass="painel_padrao" Visible="false">
                <h2 class="titulo_painel_padrao">
                    <asp:Label ID="lblTituloExperienciaProfissionalAdicional4" runat="server" Text="Experiência Profissional" />
                    <asp:Button ID="btnExcluirExp4" runat="server" Text="Excluir experiência" OnClick="btnExcluirExp4_Click" />
                    <%--<asp:ImageButton ID="imgBtnExcluirExperiencia4" runat="server" ImageUrl="~/img/icone_excluir_16x16.png"  />--%>
                </h2>
                <asp:Panel ID="Panel5" runat="server">
                    <div class="painel_padrao_topo">
                        &nbsp;
                    </div>
                    <%-- Linha Empresa --%>
                    <div class="linha">
                        <asp:Label ID="lblEmpresa4" CssClass="label_principal" runat="server" Text="Nome da Empresa"
                            AssociatedControlID="txtEmpresa4" />
                        <div class="container_campo">
                            <div>
                                <asp:RequiredFieldValidator ID="rfvEmpresa4" runat="server" ControlToValidate="txtEmpresa4"
                                    ValidationGroup="CadastroDadosPessoais"></asp:RequiredFieldValidator>
                            </div>
                            <asp:TextBox ID="txtEmpresa4" runat="server" CssClass="textbox_padrao" Columns="60"
                                MaxLength="60" AutoPostBack="true" OnTextChanged="txtEmpresa4_TextChanged"></asp:TextBox>
                        </div>
                    </div>
                    <%-- FIM: Linha Empresa --%>
                    <%-- Linha Atividade Empresa --%>
                    <div class="linha">
                        <asp:Label ID="lblAtividadeEmpresa4" CssClass="label_principal" runat="server" Text="A Empresa Trabalha com"
                            AssociatedControlID="ddlAtividadeEmpresa4" />
                        <div class="container_campo">
                            <div>
                                <asp:CustomValidator ID="cvAtividadeEmpresa4" runat="server" ControlToValidate="ddlAtividadeEmpresa4"
                                    ValidationGroup="CadastroDadosPessoais" ErrorMessage="Campo Obrigatório" ClientValidationFunction="cvAtividadeExercida4_Validate">
                                </asp:CustomValidator>
                            </div>
                            <asp:DropDownList ID="ddlAtividadeEmpresa4" CssClass="textbox_padrao" runat="server"
                                OnSelectedIndexChanged="ddlAtividadeEmpresa4_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <%-- FIM: Linha Atividade Empresa --%>
                    <%-- Linha Data Admissão --%>
                    <div class="linha">
                        <asp:Label ID="lblDataAdmissao4" CssClass="label_principal" runat="server" Text="Data de Admissão"
                            AssociatedControlID="txtDataAdmissao4" />
                        <div class="container_campo">
                            <componente:Data ID="txtDataAdmissao4" runat="server" CssClassTextBox="textbox_padrao"
                                MensagemErroIntervalo="Data Inválida" ValidationGroup="CadastroDadosPessoais" />
                        </div>
                        <%-- Linha Data Demissão --%>
                        <asp:Label ID="lbltDataDemissao4" runat="server" Text="Data de Demissão" AssociatedControlID="txtDataDemissao4" />
                        <div class="container_campo">
                            <componente:Data ID="txtDataDemissao4" runat="server" CssClassTextBox="textbox_padrao"
                                MensagemErroIntervalo="Data Inválida" Obrigatorio="false" ValidationGroup="CadastroDadosPessoais" />
                            <div runat="server" id="divDtaDemissaoAviso4" class="tooltips_aviso"><span>A data de Admissão deve ser menor que a data de Demissão.</span></div>
                        </div>
                        <%-- FIM: Linha Data Demissão --%>
                    </div>
                    <%-- FIM: Linha Data Admissão --%>
                    <%-- Linha Função Exercida --%>
                    <div class="linha">
                        <asp:Label ID="lblFuncaoExercida4" CssClass="label_principal" runat="server" Text="Função Exercida"
                            AssociatedControlID="txtFuncaoExercida4" />
                        <div class="container_campo">
                            <div>
                                <asp:RequiredFieldValidator ID="rfvFuncaoExercida4" runat="server" ControlToValidate="txtFuncaoExercida4"
                                    ValidationGroup="CadastroDadosPessoais"></asp:RequiredFieldValidator>
                            </div>
                            <asp:TextBox ID="txtFuncaoExercida4" runat="server" CssClass="textbox_padrao" Columns="60"
                                MaxLength="60"></asp:TextBox>
                        </div>
                    </div>
                    <%-- FIM: Linha Função Exercida --%>
                    <%-- Linha Atividades Exercidas --%>
                    <div class="linha">
                        <asp:Label ID="lblAtividadeExercida4" CssClass="label_principal" runat="server" AssociatedControlID="txtAtividadeExercida4">Atribuições<br />(Escreva no mínimo 3 tarefas)</asp:Label>
                        <div class="container_campo">
                            <componente:AlfaNumerico ID="txtAtividadeExercida4" runat="server" Rows="1000" MaxLength="2000"
                                OnBlurClient="txtAtividadeExercida4_OnBlur" ValidationGroup="CadastroDadosPessoais"
                                CssClassTextBox="textbox_padrao multiline atividades_exercidas" TextMode="Multiline"
                                Obrigatorio="false" MensagemErroObrigatorio="Campo obrigatório" OnKeyUpClient="txtAtividadeExercida4_KeyUp" />
                        </div>
                        <asp:Panel runat="server" ID="Panel6" CssClass="BoxSugestaoTarefas">
                            <span id="GraficoQualidade4">
                                <asp:Literal runat="server" ID="ltGraficoQualidade4"></asp:Literal></span>
                            <div class="seta_apontador_esq">
                            </div>
                            <div class="box_conteudo sugestao">
                                <asp:Label ID="lblSugestaoTarefas4" runat="server" CssClass="label_principal" AssociatedControlID="txtSugestaoTarefas4"
                                    Text="Sugestão de Tarefas"></asp:Label>
                                <div class="container_campo">
                                    <asp:TextBox ID="txtSugestaoTarefas4" CssClass="textbox_padrao sugestao_tarefas"
                                        TextMode="MultiLine" ReadOnly="true" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                    <%-- Linha Ultimo Salario --%>
                    <div class="linha">
                        <asp:Label ID="Label6" runat="server" Text="Último Salário" CssClass="label_principal"
                            AssociatedControlID="txtUltimoSalario4" />
                        <div class="container_campo container_campoAdjust_dpessoais">
                            <asp:TextBox runat="server" ID="txtUltimoSalario4" name="txtUltimoSalario4" MaxLength="12" CssClass="textbox_padrao inputSalario" />

                            <%--<componente:ValorDecimal ID="txtUltimoSalario4" runat="server" CasasDecimais="0" ValidationGroup="CadastroDadosPessoais"
                            ValorMinimo="1" ValorMaximo="999999" Obrigatorio="false" CssClassTextBox="textbox_padrao textarea_padraoAdjust_dpessoais" MensagemErroObrigatorio="Campo obrigatório" />
                        <span class="decimais_dpessoais">,00</span>--%>
                        </div>
                    </div>
                    <%-- FIM: Linha Ultimo Salario --%>
                    <%-- FIM: Atividades Exercidas --%>
                </asp:Panel>
            </asp:Panel>
            <!-- FIM Painel: Experiência Profissional 4 -->
            <!-- Experiência profisisonal 05 -->
            <asp:Panel ID="pnExperienciaProfissional5" runat="server" CssClass="painel_padrao" Visible="false">
                <h2 class="titulo_painel_padrao">
                    <asp:Label ID="lblTituloExperienciaProfissionalAdicional5" runat="server" Text="Experiência Profissional" />
                    <asp:Button ID="btnExcluirExp5" runat="server" Text="Excluir experiência" OnClick="btnExcluirExp5_Click" />
                </h2>
                <asp:Panel ID="Panel7" runat="server">
                    <div class="painel_padrao_topo">
                        &nbsp;
                    </div>
                    <%-- Linha Empresa --%>
                    <div class="linha">
                        <asp:Label ID="lblEmpresa5" CssClass="label_principal" runat="server" Text="Nome da Empresa"
                            AssociatedControlID="txtEmpresa5" />
                        <div class="container_campo">
                            <div>
                                <asp:RequiredFieldValidator ID="rfvEmpresa5" runat="server" ControlToValidate="txtEmpresa5"
                                    ValidationGroup="CadastroDadosPessoais"></asp:RequiredFieldValidator>
                            </div>
                            <asp:TextBox ID="txtEmpresa5" runat="server" CssClass="textbox_padrao" Columns="60"
                                MaxLength="60" AutoPostBack="true" OnTextChanged="txtEmpresa5_TextChanged"></asp:TextBox>
                        </div>
                    </div>
                    <%-- FIM: Linha Empresa --%>
                    <%-- Linha Atividade Empresa --%>
                    <div class="linha">
                        <asp:Label ID="lblAtividadeEmpresa5" CssClass="label_principal" runat="server" Text="A Empresa Trabalha com"
                            AssociatedControlID="ddlAtividadeEmpresa5" />
                        <div class="container_campo">
                            <div>
                                <asp:CustomValidator ID="cvAtividadeEmpresa5" runat="server" ControlToValidate="ddlAtividadeEmpresa5"
                                    ValidationGroup="CadastroDadosPessoais" ErrorMessage="Campo Obrigatório" ClientValidationFunction="cvAtividadeExercida5_Validate">
                                </asp:CustomValidator>
                            </div>
                            <asp:DropDownList ID="ddlAtividadeEmpresa5" CssClass="textbox_padrao" runat="server"
                                OnSelectedIndexChanged="ddlAtividadeEmpresa5_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <%-- FIM: Linha Atividade Empresa --%>
                    <%-- Linha Data Admissão --%>
                    <div class="linha">
                        <asp:Label ID="lblDataAdmissao5" CssClass="label_principal" runat="server" Text="Data de Admissão"
                            AssociatedControlID="txtDataAdmissao5" />
                        <div class="container_campo">
                            <componente:Data ID="txtDataAdmissao5" runat="server" CssClassTextBox="textbox_padrao"
                                MensagemErroIntervalo="Data Inválida" ValidationGroup="CadastroDadosPessoais" />
                        </div>
                        <%-- Linha Data Demissão --%>
                        <asp:Label ID="lblDataDemissao5" runat="server" Text="Data de Demissão" AssociatedControlID="txtDataDemissao5" />
                        <div class="container_campo">
                            <componente:Data ID="txtDataDemissao5" runat="server" CssClassTextBox="textbox_padrao"
                                MensagemErroIntervalo="Data Inválida" Obrigatorio="false" ValidationGroup="CadastroDadosPessoais" />
                            <div runat="server" id="divDtaDemissaoAviso5" class="tooltips_aviso"><span>A data de Admissão deve ser menor que a data de Demissão.</span></div>

                        </div>
                        <%-- FIM: Linha Data Demissão --%>
                    </div>
                    <%-- FIM: Linha Data Admissão --%>
                    <%-- Linha Função Exercida --%>
                    <div class="linha">
                        <asp:Label ID="lblFuncaoExercida5" CssClass="label_principal" runat="server" Text="Função Exercida"
                            AssociatedControlID="txtFuncaoExercida5" />
                        <div class="container_campo">
                            <div>
                                <asp:RequiredFieldValidator ID="rfvFuncaoExercida5" runat="server" ControlToValidate="txtFuncaoExercida5"
                                    ValidationGroup="CadastroDadosPessoais"></asp:RequiredFieldValidator>
                            </div>
                            <asp:TextBox ID="txtFuncaoExercida5" runat="server" CssClass="textbox_padrao" Columns="60"
                                MaxLength="60"></asp:TextBox>
                        </div>
                    </div>
                    <%-- FIM: Linha Função Exercida --%>
                    <%-- Linha Atividades Exercidas --%>
                    <div class="linha">
                        <asp:Label ID="lblAtividadeExercida5" CssClass="label_principal" runat="server" AssociatedControlID="txtAtividadeExercida5">Atribuições<br />(Escreva no mínimo 3 tarefas)</asp:Label>
                        <div class="container_campo">
                            <componente:AlfaNumerico ID="txtAtividadeExercida5" runat="server" Rows="1000" MaxLength="2000"
                                OnBlurClient="txtAtividadeExercida5_OnBlur" ValidationGroup="CadastroDadosPessoais"
                                CssClassTextBox="textbox_padrao multiline atividades_exercidas" TextMode="Multiline"
                                Obrigatorio="false" MensagemErroObrigatorio="Campo obrigatório" OnKeyUpClient="txtAtividadeExercida5_KeyUp" />
                        </div>
                        <asp:Panel runat="server" ID="Panel8" CssClass="BoxSugestaoTarefas">
                            <span id="GraficoQualidade5">
                                <asp:Literal runat="server" ID="ltGraficoQualidade5"></asp:Literal></span>
                            <div class="seta_apontador_esq">
                            </div>
                            <div class="box_conteudo sugestao">
                                <asp:Label ID="lblSugestaoTarefas5" runat="server" CssClass="label_principal" AssociatedControlID="txtSugestaoTarefas5"
                                    Text="Sugestão de Tarefas"></asp:Label>
                                <div class="container_campo">
                                    <asp:TextBox ID="txtSugestaoTarefas5" CssClass="textbox_padrao sugestao_tarefas"
                                        TextMode="MultiLine" ReadOnly="true" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                    <%-- Linha Ultimo Salario --%>
                    <div class="linha">
                        <asp:Label ID="Label7" runat="server" Text="Último Salário" CssClass="label_principal"
                            AssociatedControlID="txtUltimoSalario5" />
                        <div class="container_campo container_campoAdjust_dpessoais">
                            <asp:TextBox runat="server" ID="txtUltimoSalario5" name="txtUltimoSalario5" MaxLength="12" CssClass="textbox_padrao inputSalario" />

                            <%--<componente:ValorDecimal ID="txtUltimoSalario5" runat="server" CasasDecimais="0" ValidationGroup="CadastroDadosPessoais"
                            ValorMinimo="1" ValorMaximo="999999" Obrigatorio="false" CssClassTextBox="textbox_padrao textarea_padraoAdjust_dpessoais" MensagemErroObrigatorio="Campo obrigatório" />
                        <span class="decimais_dpessoais">,00</span>--%>
                        </div>
                    </div>
                    <%-- FIM: Linha Ultimo Salario --%>
                    <%-- FIM: Atividades Exercidas --%>
                </asp:Panel>
            </asp:Panel>
            <!-- FIM Painel: Experiência Profissional 5 -->

            <!-- Experiência profisisonal 06 -->
            <asp:Panel ID="pnExperienciaProfissional6" runat="server" CssClass="painel_padrao" Visible="false">
                <h2 class="titulo_painel_padrao">
                    <asp:Label ID="lblTituloExperienciaProfissionalAdicional6" runat="server" Text="Experiência Profissional" />
                    <asp:Button ID="btnExcluirExp6" runat="server" Text="Excluir experiência" OnClick="btnExcluirExp6_Click" />
                </h2>
                <asp:Panel ID="Panel10" runat="server">
                    <div class="painel_padrao_topo">
                        &nbsp;
                    </div>
                    <%-- Linha Empresa --%>
                    <div class="linha">
                        <asp:Label ID="lblEmpresa6" CssClass="label_principal" runat="server" Text="Nome da Empresa"
                            AssociatedControlID="txtEmpresa6" />
                        <div class="container_campo">
                            <div>
                                <asp:RequiredFieldValidator ID="rfvEmpresa6" runat="server" ControlToValidate="txtEmpresa6"
                                    ValidationGroup="CadastroDadosPessoais"></asp:RequiredFieldValidator>
                            </div>
                            <asp:TextBox ID="txtEmpresa6" runat="server" CssClass="textbox_padrao" Columns="60"
                                MaxLength="60" AutoPostBack="true" OnTextChanged="txtEmpresa6_TextChanged"></asp:TextBox>
                        </div>
                    </div>
                    <%-- FIM: Linha Empresa --%>
                    <%-- Linha Atividade Empresa --%>
                    <div class="linha">
                        <asp:Label ID="Label11" CssClass="label_principal" runat="server" Text="A Empresa Trabalha com"
                            AssociatedControlID="ddlAtividadeEmpresa6" />
                        <div class="container_campo">
                            <div>
                                <asp:CustomValidator ID="cvAtividadeEmpresa6" runat="server" ControlToValidate="ddlAtividadeEmpresa6"
                                    ValidationGroup="CadastroDadosPessoais" ErrorMessage="Campo Obrigatório" ClientValidationFunction="cvAtividadeExercida6_Validate">
                                </asp:CustomValidator>
                            </div>
                            <asp:DropDownList ID="ddlAtividadeEmpresa6" CssClass="textbox_padrao" runat="server"
                                OnSelectedIndexChanged="ddlAtividadeEmpresa6_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <%-- FIM: Linha Atividade Empresa --%>
                    <%-- Linha Data Admissão --%>
                    <div class="linha">
                        <asp:Label ID="lblDataAdmissao6" CssClass="label_principal" runat="server" Text="Data de Admissão"
                            AssociatedControlID="txtDataAdmissao6" />
                        <div class="container_campo">
                            <componente:Data ID="txtDataAdmissao6" runat="server" CssClassTextBox="textbox_padrao"
                                MensagemErroIntervalo="Data Inválida" ValidationGroup="CadastroDadosPessoais" />
                        </div>
                        <%-- Linha Data Demissão --%>
                        <asp:Label ID="lblDataDemissao6" runat="server" Text="Data de Demissão" AssociatedControlID="txtDataDemissao6" />
                        <div class="container_campo">
                            <componente:Data ID="txtDataDemissao6" runat="server" CssClassTextBox="textbox_padrao"
                                MensagemErroIntervalo="Data Inválida" Obrigatorio="false" ValidationGroup="CadastroDadosPessoais" />
                            <div runat="server" id="divDtaDemissaoAviso6" class="tooltips_aviso"><span>A data de Admissão deve ser menor que a data de Demissão.</span></div>

                        </div>
                        <%-- FIM: Linha Data Demissão --%>
                    </div>
                    <%-- FIM: Linha Data Admissão --%>
                    <%-- Linha Função Exercida --%>
                    <div class="linha">
                        <asp:Label ID="lblFuncaoExercida6" CssClass="label_principal" runat="server" Text="Função Exercida"
                            AssociatedControlID="txtFuncaoExercida6" />
                        <div class="container_campo">
                            <div>
                                <asp:RequiredFieldValidator ID="rfvFuncaoExercida6" runat="server" ControlToValidate="txtFuncaoExercida6"
                                    ValidationGroup="CadastroDadosPessoais"></asp:RequiredFieldValidator>
                            </div>
                            <asp:TextBox ID="txtFuncaoExercida6" runat="server" CssClass="textbox_padrao" Columns="60"
                                MaxLength="60"></asp:TextBox>
                        </div>
                    </div>
                    <%-- FIM: Linha Função Exercida --%>
                    <%-- Linha Atividades Exercidas --%>
                    <div class="linha">
                        <asp:Label ID="Label15" CssClass="label_principal" runat="server" AssociatedControlID="txtAtividadeExercida6">Atribuições<br />(Escreva no mínimo 3 tarefas)</asp:Label>
                        <div class="container_campo">
                            <componente:AlfaNumerico ID="txtAtividadeExercida6" runat="server" Rows="1000" MaxLength="2000"
                                OnBlurClient="txtAtividadeExercida6_OnBlur" ValidationGroup="CadastroDadosPessoais"
                                CssClassTextBox="textbox_padrao multiline atividades_exercidas" TextMode="Multiline"
                                Obrigatorio="false" MensagemErroObrigatorio="Campo obrigatório" OnKeyUpClient="txtAtividadeExercida6_KeyUp" />
                        </div>
                        <asp:Panel runat="server" ID="Panel11" CssClass="BoxSugestaoTarefas">
                            <span id="GraficoQualidade6">
                                <asp:Literal runat="server" ID="ltGraficoQualidade6"></asp:Literal></span>
                            <div class="seta_apontador_esq">
                            </div>
                            <div class="box_conteudo sugestao">
                                <asp:Label ID="lblSugestaoTarefas6" runat="server" CssClass="label_principal" AssociatedControlID="txtSugestaoTarefas6"
                                    Text="Sugestão de Tarefas"></asp:Label>
                                <div class="container_campo">
                                    <asp:TextBox ID="txtSugestaoTarefas6" CssClass="textbox_padrao sugestao_tarefas"
                                        TextMode="MultiLine" ReadOnly="true" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                    <%-- Linha Ultimo Salario --%>
                    <div class="linha">
                        <asp:Label ID="Label17" runat="server" Text="Último Salário" CssClass="label_principal"
                            AssociatedControlID="txtUltimoSalario6" />
                        <div class="container_campo container_campoAdjust_dpessoais">
                            <asp:TextBox runat="server" ID="txtUltimoSalario6" name="txtUltimoSalario6" MaxLength="12" CssClass="textbox_padrao inputSalario" />

                            <%--<componente:ValorDecimal ID="txtUltimoSalario6" runat="server" CasasDecimais="0" ValidationGroup="CadastroDadosPessoais"
                            ValorMinimo="1" ValorMaximo="999999" Obrigatorio="false" CssClassTextBox="textbox_padrao textarea_padraoAdjust_dpessoais" MensagemErroObrigatorio="Campo obrigatório" />
                        <span class="decimais_dpessoais">,00</span>--%>
                        </div>
                    </div>
                    <%-- FIM: Linha Ultimo Salario --%>
                    <%-- FIM: Atividades Exercidas --%>
                </asp:Panel>
            </asp:Panel>
            <!-- FIM Painel: Experiência Profissional 6 -->

            <!-- Experiência profisisonal 07 -->
            <asp:Panel ID="pnExperienciaProfissional7" runat="server" CssClass="painel_padrao" Visible="false">
                <h2 class="titulo_painel_padrao">
                    <asp:Label ID="lblTituloExperienciaProfissionalAdicional7" runat="server" Text="Experiência Profissional" />
                    <asp:Button ID="btnExcluirExp7" runat="server" Text="Excluir experiência" OnClick="btnExcluirExp7_Click" />
                </h2>
                <asp:Panel ID="Panel13" runat="server">
                    <div class="painel_padrao_topo">
                        &nbsp;
                    </div>
                    <%-- Linha Empresa --%>
                    <div class="linha">
                        <asp:Label ID="Label10" CssClass="label_principal" runat="server" Text="Nome da Empresa"
                            AssociatedControlID="txtEmpresa7" />
                        <div class="container_campo">
                            <div>
                                <asp:RequiredFieldValidator ID="rfvEmpresa7" runat="server" ControlToValidate="txtEmpresa7"
                                    ValidationGroup="CadastroDadosPessoais"></asp:RequiredFieldValidator>
                            </div>
                            <asp:TextBox ID="txtEmpresa7" runat="server" CssClass="textbox_padrao" Columns="60"
                                MaxLength="60" AutoPostBack="true" OnTextChanged="txtEmpresa7_TextChanged"></asp:TextBox>
                        </div>
                    </div>
                    <%-- FIM: Linha Empresa --%>
                    <%-- Linha Atividade Empresa --%>
                    <div class="linha">
                        <asp:Label ID="Label12" CssClass="label_principal" runat="server" Text="A Empresa Trabalha com"
                            AssociatedControlID="ddlAtividadeEmpresa7" />
                        <div class="container_campo">
                            <div>
                                <asp:CustomValidator ID="cvAtividadeEmpresa7" runat="server" ControlToValidate="ddlAtividadeEmpresa7"
                                    ValidationGroup="CadastroDadosPessoais" ErrorMessage="Campo Obrigatório" ClientValidationFunction="cvAtividadeExercida7_Validate">
                                </asp:CustomValidator>
                            </div>
                            <asp:DropDownList ID="ddlAtividadeEmpresa7" CssClass="textbox_padrao" runat="server"
                                OnSelectedIndexChanged="ddlAtividadeEmpresa7_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <%-- FIM: Linha Atividade Empresa --%>
                    <%-- Linha Data Admissão --%>
                    <div class="linha">
                        <asp:Label ID="lblDataAdmissao7" CssClass="label_principal" runat="server" Text="Data de Admissão"
                            AssociatedControlID="txtDataAdmissao7" />
                        <div class="container_campo">
                            <componente:Data ID="txtDataAdmissao7" runat="server" CssClassTextBox="textbox_padrao"
                                MensagemErroIntervalo="Data Inválida" ValidationGroup="CadastroDadosPessoais" />
                        </div>
                        <%-- Linha Data Demissão --%>
                        <asp:Label ID="lblDataDemissao7" runat="server" Text="Data de Demissão" AssociatedControlID="txtDataDemissao7" />
                        <div class="container_campo">
                            <componente:Data ID="txtDataDemissao7" runat="server" CssClassTextBox="textbox_padrao"
                                MensagemErroIntervalo="Data Inválida" Obrigatorio="false" ValidationGroup="CadastroDadosPessoais" />
                            <div runat="server" id="divDtaDemissaoAviso7" class="tooltips_aviso"><span>A data de Admissão deve ser menor que a data de Demissão.</span></div>

                        </div>
                        <%-- FIM: Linha Data Demissão --%>
                    </div>
                    <%-- FIM: Linha Data Admissão --%>
                    <%-- Linha Função Exercida --%>
                    <div class="linha">
                        <asp:Label ID="lblFuncaoExercida7" CssClass="label_principal" runat="server" Text="Função Exercida"
                            AssociatedControlID="txtFuncaoExercida7" />
                        <div class="container_campo">
                            <div>
                                <asp:RequiredFieldValidator ID="rfvFuncaoExercida7" runat="server" ControlToValidate="txtFuncaoExercida7"
                                    ValidationGroup="CadastroDadosPessoais"></asp:RequiredFieldValidator>
                            </div>
                            <asp:TextBox ID="txtFuncaoExercida7" runat="server" CssClass="textbox_padrao" Columns="60"
                                MaxLength="60"></asp:TextBox>
                        </div>
                    </div>
                    <%-- FIM: Linha Função Exercida --%>
                    <%-- Linha Atividades Exercidas --%>
                    <div class="linha">
                        <asp:Label ID="Label18" CssClass="label_principal" runat="server" AssociatedControlID="txtAtividadeExercida7">Atribuições<br />(Escreva no mínimo 3 tarefas)</asp:Label>
                        <div class="container_campo">
                            <componente:AlfaNumerico ID="txtAtividadeExercida7" runat="server" Rows="1000" MaxLength="2000"
                                OnBlurClient="txtAtividadeExercida7_OnBlur" ValidationGroup="CadastroDadosPessoais"
                                CssClassTextBox="textbox_padrao multiline atividades_exercidas" TextMode="Multiline"
                                Obrigatorio="false" MensagemErroObrigatorio="Campo obrigatório" OnKeyUpClient="txtAtividadeExercida7_KeyUp" />
                        </div>
                        <asp:Panel runat="server" ID="Panel14" CssClass="BoxSugestaoTarefas">
                            <span id="GraficoQualidade7">
                                <asp:Literal runat="server" ID="ltGraficoQualidade7"></asp:Literal></span>
                            <div class="seta_apontador_esq">
                            </div>
                            <div class="box_conteudo sugestao">
                                <asp:Label ID="Label19" runat="server" CssClass="label_principal" AssociatedControlID="txtSugestaoTarefas7"
                                    Text="Sugestão de Tarefas"></asp:Label>
                                <div class="container_campo">
                                    <asp:TextBox ID="txtSugestaoTarefas7" CssClass="textbox_padrao sugestao_tarefas"
                                        TextMode="MultiLine" ReadOnly="true" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                    <%-- Linha Ultimo Salario --%>
                    <div class="linha">
                        <asp:Label ID="Label20" runat="server" Text="Último Salário" CssClass="label_principal"
                            AssociatedControlID="txtUltimoSalario7" />
                        <div class="container_campo container_campoAdjust_dpessoais">
                            <asp:TextBox runat="server" ID="txtUltimoSalario7" name="txtUltimoSalario7" MaxLength="12" CssClass="textbox_padrao inputSalario" />

                            <%--<componente:ValorDecimal ID="txtUltimoSalario7" runat="server" CasasDecimais="0" ValidationGroup="CadastroDadosPessoais"
                            ValorMinimo="1" ValorMaximo="999999" Obrigatorio="false" CssClassTextBox="textbox_padrao textarea_padraoAdjust_dpessoais" MensagemErroObrigatorio="Campo obrigatório" />
                        <span class="decimais_dpessoais">,00</span>--%>
                        </div>
                    </div>
                    <%-- FIM: Linha Ultimo Salario --%>
                    <%-- FIM: Atividades Exercidas --%>
                </asp:Panel>
            </asp:Panel>
            <!-- FIM Painel: Experiência Profissional 7 -->

            <!-- Experiência profisisonal 8 -->
            <asp:Panel ID="pnExperienciaProfissional8" runat="server" CssClass="painel_padrao" Visible="false">
                <h2 class="titulo_painel_padrao">
                    <asp:Label ID="lblTituloExperienciaProfissionalAdicional8" runat="server" Text="Experiência Profissional" />
                    <asp:Button ID="btnExcluirExp8" runat="server" Text="Excluir experiência" OnClick="btnExcluirExp8_Click" />
                </h2>
                <asp:Panel ID="Panel15" runat="server">
                    <div class="painel_padrao_topo">
                        &nbsp;
                    </div>
                    <%-- Linha Empresa --%>
                    <div class="linha">
                        <asp:Label ID="Label13" CssClass="label_principal" runat="server" Text="Nome da Empresa"
                            AssociatedControlID="txtEmpresa8" />
                        <div class="container_campo">
                            <div>
                                <asp:RequiredFieldValidator ID="rfvEmpresa8" runat="server" ControlToValidate="txtEmpresa8"
                                    ValidationGroup="CadastroDadosPessoais"></asp:RequiredFieldValidator>
                            </div>
                            <asp:TextBox ID="txtEmpresa8" runat="server" CssClass="textbox_padrao" Columns="60"
                                MaxLength="60" AutoPostBack="true" OnTextChanged="txtEmpresa8_TextChanged"></asp:TextBox>
                        </div>
                    </div>
                    <%-- FIM: Linha Empresa --%>
                    <%-- Linha Atividade Empresa --%>
                    <div class="linha">
                        <asp:Label ID="Label14" CssClass="label_principal" runat="server" Text="A Empresa Trabalha com"
                            AssociatedControlID="ddlAtividadeEmpresa8" />
                        <div class="container_campo">
                            <div>
                                <asp:CustomValidator ID="cvAtividadeEmpresa8" runat="server" ControlToValidate="ddlAtividadeEmpresa8"
                                    ValidationGroup="CadastroDadosPessoais" ErrorMessage="Campo Obrigatório" ClientValidationFunction="cvAtividadeExercida8_Validate">
                                </asp:CustomValidator>
                            </div>
                            <asp:DropDownList ID="ddlAtividadeEmpresa8" CssClass="textbox_padrao" runat="server"
                                OnSelectedIndexChanged="ddlAtividadeEmpresa8_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <%-- FIM: Linha Atividade Empresa --%>
                    <%-- Linha Data Admissão --%>
                    <div class="linha">
                        <asp:Label ID="Label16" CssClass="label_principal" runat="server" Text="Data de Admissão"
                            AssociatedControlID="txtDataAdmissao8" />
                        <div class="container_campo">
                            <componente:Data ID="txtDataAdmissao8" runat="server" CssClassTextBox="textbox_padrao"
                                MensagemErroIntervalo="Data Inválida" ValidationGroup="CadastroDadosPessoais" />
                        </div>
                        <%-- Linha Data Demissão --%>
                        <asp:Label ID="Label21" runat="server" Text="Data de Demissão" AssociatedControlID="txtDataDemissao8" />
                        <div class="container_campo">
                            <componente:Data ID="txtDataDemissao8" runat="server" CssClassTextBox="textbox_padrao"
                                MensagemErroIntervalo="Data Inválida" Obrigatorio="false" ValidationGroup="CadastroDadosPessoais" />
                            <div runat="server" id="divDtaDemissaoAviso8" class="tooltips_aviso"><span>A data de Admissão deve ser menor que a data de Demissão.</span></div>
                        </div>
                        <%-- FIM: Linha Data Demissão --%>
                    </div>
                    <%-- FIM: Linha Data Admissão --%>
                    <%-- Linha Função Exercida --%>
                    <div class="linha">
                        <asp:Label ID="Label22" CssClass="label_principal" runat="server" Text="Função Exercida"
                            AssociatedControlID="txtFuncaoExercida8" />
                        <div class="container_campo">
                            <div>
                                <asp:RequiredFieldValidator ID="rfvFuncaoExercida8" runat="server" ControlToValidate="txtFuncaoExercida8"
                                    ValidationGroup="CadastroDadosPessoais"></asp:RequiredFieldValidator>
                            </div>
                            <asp:TextBox ID="txtFuncaoExercida8" runat="server" CssClass="textbox_padrao" Columns="60"
                                MaxLength="60"></asp:TextBox>
                        </div>
                    </div>
                    <%-- FIM: Linha Função Exercida --%>
                    <%-- Linha Atividades Exercidas --%>
                    <div class="linha">
                        <asp:Label ID="Label23" CssClass="label_principal" runat="server" AssociatedControlID="txtAtividadeExercida8">Atribuições<br />(Escreva no mínimo 3 tarefas)</asp:Label>
                        <div class="container_campo">
                            <componente:AlfaNumerico ID="txtAtividadeExercida8" runat="server" Rows="1000" MaxLength="2000"
                                OnBlurClient="txtAtividadeExercida8_OnBlur" ValidationGroup="CadastroDadosPessoais"
                                CssClassTextBox="textbox_padrao multiline atividades_exercidas" TextMode="Multiline"
                                Obrigatorio="false" MensagemErroObrigatorio="Campo obrigatório" OnKeyUpClient="txtAtividadeExercida8_KeyUp" />
                        </div>
                        <asp:Panel runat="server" ID="Panel16" CssClass="BoxSugestaoTarefas">
                            <span id="GraficoQualidade8">
                                <asp:Literal runat="server" ID="ltGraficoQualidade8"></asp:Literal></span>
                            <div class="seta_apontador_esq">
                            </div>
                            <div class="box_conteudo sugestao">
                                <asp:Label ID="Label24" runat="server" CssClass="label_principal" AssociatedControlID="txtSugestaoTarefas8"
                                    Text="Sugestão de Tarefas"></asp:Label>
                                <div class="container_campo">
                                    <asp:TextBox ID="txtSugestaoTarefas8" CssClass="textbox_padrao sugestao_tarefas"
                                        TextMode="MultiLine" ReadOnly="true" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                    <%-- Linha Ultimo Salario --%>
                    <div class="linha">
                        <asp:Label ID="Label25" runat="server" Text="Último Salário" CssClass="label_principal"
                            AssociatedControlID="txtUltimoSalario8" />
                        <div class="container_campo container_campoAdjust_dpessoais">
                            <asp:TextBox runat="server" ID="txtUltimoSalario8" name="txtUltimoSalario8" MaxLength="12" CssClass="textbox_padrao inputSalario" />

                            <%--<componente:ValorDecimal ID="txtUltimoSalario8" runat="server" CasasDecimais="0" ValidationGroup="CadastroDadosPessoais"
                            ValorMinimo="1" ValorMaximo="999999" Obrigatorio="false" CssClassTextBox="textbox_padrao textarea_padraoAdjust_dpessoais" MensagemErroObrigatorio="Campo obrigatório" />
                        <span class="decimais_dpessoais">,00</span>--%>
                        </div>
                    </div>
                    <%-- FIM: Linha Ultimo Salario --%>
                    <%-- FIM: Atividades Exercidas --%>
                </asp:Panel>
            </asp:Panel>
            <!-- FIM Painel: Experiência Profissional 8 -->

            <!-- Experiência profisisonal 9 -->
            <asp:Panel ID="pnExperienciaProfissional9" runat="server" CssClass="painel_padrao" Visible="false">
                <h2 class="titulo_painel_padrao">
                    <asp:Label ID="lblTituloExperienciaProfissionalAdicional9" runat="server" Text="Experiência Profissional" />
                    <asp:Button ID="btnExcluirExp9" runat="server" Text="Excluir experiência" OnClick="btnExcluirExp9_Click" />
                </h2>
                <asp:Panel ID="Panel17" runat="server">
                    <div class="painel_padrao_topo">
                        &nbsp;
                    </div>
                    <%-- Linha Empresa --%>
                    <div class="linha">
                        <asp:Label ID="Label26" CssClass="label_principal" runat="server" Text="Nome da Empresa"
                            AssociatedControlID="txtEmpresa9" />
                        <div class="container_campo">
                            <div>
                                <asp:RequiredFieldValidator ID="rfvEmpresa9" runat="server" ControlToValidate="txtEmpresa9"
                                    ValidationGroup="CadastroDadosPessoais"></asp:RequiredFieldValidator>
                            </div>
                            <asp:TextBox ID="txtEmpresa9" runat="server" CssClass="textbox_padrao" Columns="60"
                                MaxLength="60" AutoPostBack="true" OnTextChanged="txtEmpresa9_TextChanged"></asp:TextBox>
                        </div>
                    </div>
                    <%-- FIM: Linha Empresa --%>
                    <%-- Linha Atividade Empresa --%>
                    <div class="linha">
                        <asp:Label ID="Label27" CssClass="label_principal" runat="server" Text="A Empresa Trabalha com"
                            AssociatedControlID="ddlAtividadeEmpresa9" />
                        <div class="container_campo">
                            <div>
                                <asp:CustomValidator ID="cvAtividadeEmpresa9" runat="server" ControlToValidate="ddlAtividadeEmpresa9"
                                    ValidationGroup="CadastroDadosPessoais" ErrorMessage="Campo Obrigatório" ClientValidationFunction="cvAtividadeExercida9_Validate">
                                </asp:CustomValidator>
                            </div>
                            <asp:DropDownList ID="ddlAtividadeEmpresa9" CssClass="textbox_padrao" runat="server"
                                OnSelectedIndexChanged="ddlAtividadeEmpresa9_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <%-- FIM: Linha Atividade Empresa --%>
                    <%-- Linha Data Admissão --%>
                    <div class="linha">
                        <asp:Label ID="Label28" CssClass="label_principal" runat="server" Text="Data de Admissão"
                            AssociatedControlID="txtDataAdmissao9" />
                        <div class="container_campo">
                            <componente:Data ID="txtDataAdmissao9" runat="server" CssClassTextBox="textbox_padrao"
                                MensagemErroIntervalo="Data Inválida" ValidationGroup="CadastroDadosPessoais" />
                        </div>
                        <%-- Linha Data Demissão --%>
                        <asp:Label ID="Label29" runat="server" Text="Data de Demissão" AssociatedControlID="txtDataDemissao9" />
                        <div class="container_campo">
                            <componente:Data ID="txtDataDemissao9" runat="server" CssClassTextBox="textbox_padrao"
                                MensagemErroIntervalo="Data Inválida" Obrigatorio="false" ValidationGroup="CadastroDadosPessoais" />
                            <div runat="server" id="divDtaDemissaoAviso9" class="tooltips_aviso"><span>A data de Admissão deve ser menor que a data de Demissão.</span></div>
                        </div>
                        <%-- FIM: Linha Data Demissão --%>
                    </div>
                    <%-- FIM: Linha Data Admissão --%>
                    <%-- Linha Função Exercida --%>
                    <div class="linha">
                        <asp:Label ID="Label30" CssClass="label_principal" runat="server" Text="Função Exercida"
                            AssociatedControlID="txtFuncaoExercida9" />
                        <div class="container_campo">
                            <div>
                                <asp:RequiredFieldValidator ID="rfvFuncaoExercida9" runat="server" ControlToValidate="txtFuncaoExercida9"
                                    ValidationGroup="CadastroDadosPessoais"></asp:RequiredFieldValidator>
                            </div>
                            <asp:TextBox ID="txtFuncaoExercida9" runat="server" CssClass="textbox_padrao" Columns="60"
                                MaxLength="60"></asp:TextBox>
                        </div>
                    </div>
                    <%-- FIM: Linha Função Exercida --%>
                    <%-- Linha Atividades Exercidas --%>
                    <div class="linha">
                        <asp:Label ID="Label31" CssClass="label_principal" runat="server" AssociatedControlID="txtAtividadeExercida9">Atribuições<br />(Escreva no mínimo 3 tarefas)</asp:Label>
                        <div class="container_campo">
                            <componente:AlfaNumerico ID="txtAtividadeExercida9" runat="server" Rows="1000" MaxLength="2000"
                                OnBlurClient="txtAtividadeExercida9_OnBlur" ValidationGroup="CadastroDadosPessoais"
                                CssClassTextBox="textbox_padrao multiline atividades_exercidas" TextMode="Multiline"
                                Obrigatorio="false" MensagemErroObrigatorio="Campo obrigatório" OnKeyUpClient="txtAtividadeExercida9_KeyUp" />
                        </div>
                        <asp:Panel runat="server" ID="Panel18" CssClass="BoxSugestaoTarefas">
                            <span id="GraficoQualidade9">
                                <asp:Literal runat="server" ID="ltGraficoQualidade9"></asp:Literal></span>
                            <div class="seta_apontador_esq">
                            </div>
                            <div class="box_conteudo sugestao">
                                <asp:Label ID="Label32" runat="server" CssClass="label_principal" AssociatedControlID="txtSugestaoTarefas9"
                                    Text="Sugestão de Tarefas"></asp:Label>
                                <div class="container_campo">
                                    <asp:TextBox ID="txtSugestaoTarefas9" CssClass="textbox_padrao sugestao_tarefas"
                                        TextMode="MultiLine" ReadOnly="true" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                    <%-- Linha Ultimo Salario --%>
                    <div class="linha">
                        <asp:Label ID="Label33" runat="server" Text="Último Salário" CssClass="label_principal"
                            AssociatedControlID="txtUltimoSalario9" />
                        <div class="container_campo container_campoAdjust_dpessoais">
                            <asp:TextBox runat="server" ID="txtUltimoSalario9" name="txtUltimoSalario9" MaxLength="12" CssClass="textbox_padrao inputSalario" />

                            <%--<componente:ValorDecimal ID="txtUltimoSalario9" runat="server" CasasDecimais="0" ValidationGroup="CadastroDadosPessoais"
                            ValorMinimo="1" ValorMaximo="999999" Obrigatorio="false" CssClassTextBox="textbox_padrao textarea_padraoAdjust_dpessoais" MensagemErroObrigatorio="Campo obrigatório" />
                        <span class="decimais_dpessoais">,00</span>--%>
                        </div>
                    </div>
                    <%-- FIM: Linha Ultimo Salario --%>
                    <%-- FIM: Atividades Exercidas --%>
                </asp:Panel>
            </asp:Panel>
            <!-- FIM Painel: Experiência Profissional 9 -->

            <!-- Experiência profisisonal 10 -->
            <asp:Panel ID="pnExperienciaProfissional10" runat="server" CssClass="painel_padrao" Visible="false">
                <h2 class="titulo_painel_padrao">
                    <asp:Label ID="lblTituloExperienciaProfissionalAdicional10" runat="server" Text="Experiência Profissional" />
                    <asp:Button ID="btnExcluirExp10" runat="server" Text="Excluir experiência" OnClick="btnExcluirExp10_Click" />
                </h2>
                <asp:Panel ID="Panel19" runat="server">
                    <div class="painel_padrao_topo">
                        &nbsp;
                    </div>
                    <%-- Linha Empresa --%>
                    <div class="linha">
                        <asp:Label ID="Label34" CssClass="label_principal" runat="server" Text="Nome da Empresa"
                            AssociatedControlID="txtEmpresa10" />
                        <div class="container_campo">
                            <div>
                                <asp:RequiredFieldValidator ID="rfvEmpresa10" runat="server" ControlToValidate="txtEmpresa10"
                                    ValidationGroup="CadastroDadosPessoais"></asp:RequiredFieldValidator>
                            </div>
                            <asp:TextBox ID="txtEmpresa10" runat="server" CssClass="textbox_padrao" Columns="60"
                                MaxLength="60" AutoPostBack="true" OnTextChanged="txtEmpresa10_TextChanged"></asp:TextBox>
                        </div>
                    </div>
                    <%-- FIM: Linha Empresa --%>
                    <%-- Linha Atividade Empresa --%>
                    <div class="linha">
                        <asp:Label ID="Label35" CssClass="label_principal" runat="server" Text="A Empresa Trabalha com"
                            AssociatedControlID="ddlAtividadeEmpresa10" />
                        <div class="container_campo">
                            <div>
                                <asp:CustomValidator ID="cvAtividadeEmpresa10" runat="server" ControlToValidate="ddlAtividadeEmpresa10"
                                    ValidationGroup="CadastroDadosPessoais" ErrorMessage="Campo Obrigatório" ClientValidationFunction="cvAtividadeExercida10_Validate">
                                </asp:CustomValidator>
                            </div>
                            <asp:DropDownList ID="ddlAtividadeEmpresa10" CssClass="textbox_padrao" runat="server"
                                OnSelectedIndexChanged="ddlAtividadeEmpresa10_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <%-- FIM: Linha Atividade Empresa --%>
                    <%-- Linha Data Admissão --%>
                    <div class="linha">
                        <asp:Label ID="Label36" CssClass="label_principal" runat="server" Text="Data de Admissão"
                            AssociatedControlID="txtDataAdmissao10" />
                        <div class="container_campo">
                            <componente:Data ID="txtDataAdmissao10" runat="server" CssClassTextBox="textbox_padrao"
                                MensagemErroIntervalo="Data Inválida" ValidationGroup="CadastroDadosPessoais" />
                        </div>
                        <%-- Linha Data Demissão --%>
                        <asp:Label ID="Label37" runat="server" Text="Data de Demissão" AssociatedControlID="txtDataDemissao10" />
                        <div class="container_campo">
                            <componente:Data ID="txtDataDemissao10" runat="server" CssClassTextBox="textbox_padrao"
                                MensagemErroIntervalo="Data Inválida" Obrigatorio="false" ValidationGroup="CadastroDadosPessoais" />
                            <div runat="server" id="divDtaDemissaoAviso10" class="tooltips_aviso"><span>A data de Admissão deve ser menor que a data de Demissão.</span></div>
                        </div>
                        <%-- FIM: Linha Data Demissão --%>
                    </div>
                    <%-- FIM: Linha Data Admissão --%>
                    <%-- Linha Função Exercida --%>
                    <div class="linha">
                        <asp:Label ID="Label38" CssClass="label_principal" runat="server" Text="Função Exercida"
                            AssociatedControlID="txtFuncaoExercida10" />
                        <div class="container_campo">
                            <div>
                                <asp:RequiredFieldValidator ID="rfvFuncaoExercida10" runat="server" ControlToValidate="txtFuncaoExercida10"
                                    ValidationGroup="CadastroDadosPessoais"></asp:RequiredFieldValidator>
                            </div>
                            <asp:TextBox ID="txtFuncaoExercida10" runat="server" CssClass="textbox_padrao" Columns="60"
                                MaxLength="60"></asp:TextBox>
                        </div>
                    </div>
                    <%-- FIM: Linha Função Exercida --%>
                    <%-- Linha Atividades Exercidas --%>
                    <div class="linha">
                        <asp:Label ID="Label39" CssClass="label_principal" runat="server" AssociatedControlID="txtAtividadeExercida10">Atribuições<br />(Escreva no mínimo 3 tarefas)</asp:Label>
                        <div class="container_campo">
                            <componente:AlfaNumerico ID="txtAtividadeExercida10" runat="server" Rows="1000" MaxLength="2000"
                                OnBlurClient="txtAtividadeExercida10_OnBlur" ValidationGroup="CadastroDadosPessoais"
                                CssClassTextBox="textbox_padrao multiline atividades_exercidas" TextMode="Multiline"
                                Obrigatorio="false" MensagemErroObrigatorio="Campo obrigatório" OnKeyUpClient="txtAtividadeExercida10_KeyUp" />
                        </div>
                        <asp:Panel runat="server" ID="Panel20" CssClass="BoxSugestaoTarefas">
                            <span id="GraficoQualidade10">
                                <asp:Literal runat="server" ID="ltGraficoQualidade10"></asp:Literal></span>
                            <div class="seta_apontador_esq">
                            </div>
                            <div class="box_conteudo sugestao">
                                <asp:Label ID="Label40" runat="server" CssClass="label_principal" AssociatedControlID="txtSugestaoTarefas10"
                                    Text="Sugestão de Tarefas"></asp:Label>
                                <div class="container_campo">
                                    <asp:TextBox ID="txtSugestaoTarefas10" CssClass="textbox_padrao sugestao_tarefas"
                                        TextMode="MultiLine" ReadOnly="true" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                    <%-- Linha Ultimo Salario --%>
                    <div class="linha">
                        <asp:Label ID="Label41" runat="server" Text="Último Salário" CssClass="label_principal"
                            AssociatedControlID="txtUltimoSalario10" />
                        <div class="container_campo container_campoAdjust_dpessoais">
                            <asp:TextBox runat="server" ID="txtUltimoSalario10" name="txtUltimoSalario10" MaxLength="12" CssClass="textbox_padrao inputSalario" />

                            <%--<componente:ValorDecimal ID="txtUltimoSalario10" runat="server" CasasDecimais="0" ValidationGroup="CadastroDadosPessoais"
                            ValorMinimo="1" ValorMaximo="999999" Obrigatorio="false" CssClassTextBox="textbox_padrao textarea_padraoAdjust_dpessoais" MensagemErroObrigatorio="Campo obrigatório" />
                        <span class="decimais_dpessoais">,00</span>--%>
                        </div>
                    </div>
                    <%-- FIM: Linha Ultimo Salario --%>
                    <%-- FIM: Atividades Exercidas --%>
                </asp:Panel>
            </asp:Panel>
            <!-- FIM Painel: Experiência Profissional 10 -->

            <!-- MODAL de Exclusão da Experiência profisisonal -->
            <asp:Panel ID="pnlModalConfirmacaoExclusao" runat="server" CssClass="modal_confirmacao_acao"
                Style="display: none">
                <asp:UpdatePanel ID="upBtiFechar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:ImageButton CssClass="botao_fechar_modal" ID="btiFechar" ImageUrl="/img/botao_padrao_fechar.gif"
                            runat="server" CausesValidation="false" OnClick="btiFechar_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!-- Painel Modal Confirmacao Exclusao -->
                <div>
                    <h2 class="titulo_modal">
                        <span>Confirmação<%-- <asp:Label ID="lblNomePessoaExclusao" runat="server"></asp:Label> --%></span></h2>
                </div>
                <asp:Panel ID="pnlModalConfirmacaoExclusaoInformacao" runat="server" CssClass="texto_confirmacao">
                    <asp:Label ID="lblConfirmacaoExclusao" runat="server" Text=""></asp:Label>
                </asp:Panel>
                <!-- FIM: Painel Modal Confirmacao Exclusao -->
                <!-- Painel botoes -->
                <asp:Panel ID="pnlBotoesModalEclusao" runat="server" CssClass="painel_botoes">
                    <asp:Button ID="btnExcluirModalExclusao" runat="server" Text="Sim" OnClick="btnExcluirModalExclusao_Click"
                        CssClass="botao_padrao" CausesValidation="false" />
                    <asp:Button ID="btnCancelarModalExclusao" runat="server" Text="Não" OnClick="btnCancelarModalExclusao_Click"
                        CssClass="botao_padrao" CausesValidation="false" />
                </asp:Panel>
                <!-- FIM: Painel botoes -->
            </asp:Panel>
            <asp:HiddenField ID="hfModalConfirmacaoExclusao" runat="server" />
            <AjaxToolkit:ModalPopupExtender ID="mpeConfirmacaoExclusao" BackgroundCssClass="modal_fundo"
                runat="server" PopupControlID="pnlModalConfirmacaoExclusao" TargetControlID="hfModalConfirmacaoExclusao"
                RepositionMode="RepositionOnWindowResizeAndScroll" />
            <!-- FIM MODAL de Exclusão da Experiência profisisonal -->

            <!-- MODAL de Aviso após 6 Experiência profisisonal -->
            <asp:Panel ID="pnlAvisoApos6Experiencia" runat="server" CssClass="modal_confirmacao_acao"
                Style="display: none">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:ImageButton CssClass="botao_fechar_modal" ID="ImageButton1" ImageUrl="/img/botao_padrao_fechar.gif"
                            runat="server" CausesValidation="false" OnClick="btiFechar_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!-- Painel Modal Confirmacao Exclusao -->
                <div>
                    <h2 class="titulo_modal">
                        <span>Confirmação<%-- <asp:Label ID="lblNomePessoaExclusao" runat="server"></asp:Label> --%></span></h2>
                </div>
                <asp:Panel ID="Panel12" runat="server" CssClass="texto_confirmacao">
                    <asp:Label ID="lblAvisoApos6Experiencia" runat="server" Text=""></asp:Label>
                </asp:Panel>
                <!-- FIM: Painel Modal Confirmacao Exclusao -->
                <%--<!-- Painel botoes -->
                <asp:Panel ID="Panel13" runat="server" CssClass="painel_botoes">
                    <asp:Button ID="Button2" runat="server" Text="Sim" OnClick="btnExcluirModalExclusao_Click"
                        CssClass="botao_padrao" CausesValidation="false" />
                    <asp:Button ID="Button3" runat="server" Text="Não" OnClick="btnCancelarModalExclusao_Click"
                        CssClass="botao_padrao" CausesValidation="false" />
                </asp:Panel>
                <!-- FIM: Painel botoes -->--%>
            </asp:Panel>
            <asp:HiddenField ID="hfModalAvisoApos6Experiencia" runat="server" />
            <AjaxToolkit:ModalPopupExtender ID="mpeAvisoApos6Experiencia" BackgroundCssClass="modal_fundo"
                runat="server" PopupControlID="pnlAvisoApos6Experiencia" TargetControlID="hfModalAvisoApos6Experiencia"
                RepositionMode="RepositionOnWindowResizeAndScroll" />
            <!-- FIM MODAL de Exclusão da Experiência profisisonal -->
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
        </body>
<!-- Painel botoes -->
<asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
    <asp:UpdatePanel ID="upPnlBotoes" runat="server">
        <ContentTemplate>
            <asp:Button ID="btnSalvar" runat="server" Text="Salvar e Avançar" OnClick="btnSalvar_Click"
                ValidationGroup="CadastroDadosPessoais" CssClass="botao_padrao" CausesValidation="True" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<!-- FIM: Painel botoes -->
<uc2:ModalDegustacaoCandidatura ID="ucModalDegustacaoCandidatura" runat="server" />
<script type="text/javascript">
    $(document).ready(function () {

        var options = {
            reverse: true
        }

        validarUltimoSalario();

        $('#cphConteudo_ucDadosPessoais_txtUltimoSalario').mask('0.000.000,00', { reverse: true });
        $('#cphConteudo_ucDadosPessoais_txtUltimoSalario2').mask('0.000.000,00', options);
        $('#cphConteudo_ucDadosPessoais_txtUltimoSalario3').mask('0.000.000,00', options);
        $('#cphConteudo_ucDadosPessoais_txtUltimoSalario4').mask('0.000.000,00', options);
        $('#cphConteudo_ucDadosPessoais_txtUltimoSalario5').mask('0.000.000,00', options);
        $('#cphConteudo_ucDadosPessoais_txtUltimoSalario6').mask('0.000.000,00', options);
        $('#cphConteudo_ucDadosPessoais_txtUltimoSalario7').mask('0.000.000,00', options);
        $('#cphConteudo_ucDadosPessoais_txtUltimoSalario8').mask('0.000.000,00', options);
        $('#cphConteudo_ucDadosPessoais_txtUltimoSalario9').mask('0.000.000,00', options);
        $('#cphConteudo_ucDadosPessoais_txtUltimoSalario10').mask('0.000.000,00', options);

    });


</script>
