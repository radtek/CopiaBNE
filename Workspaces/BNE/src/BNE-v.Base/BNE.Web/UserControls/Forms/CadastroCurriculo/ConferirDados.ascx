<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConferirDados.ascx.cs"
    Inherits="BNE.Web.UserControls.Forms.CadastroCurriculo.ConferirDados" %>

<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/CadastroCurriculo/ConferirDados.css" type="text/css" rel="stylesheet" />
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/EnvioCurriculo.css" type="text/css" rel="stylesheet" />
<Employer:DynamicScript runat="server" Src="/js/local/Forms/CadastroCurriculo/ConferirDados.js" type="text/javascript" />

<%@ Register Src="~/UserControls/BuscarCEP.ascx" TagName="BuscarCEP" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/Modais/EnvioCurriculo.ascx" TagName="EnvioCurriculo"
    TagPrefix="uc3" %>
<%@ Import Namespace="BNE.BLL.Custom" %>
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
                    <span class="aba_esquerda_nao_selecionada_inicio aba">
                        <asp:LinkButton ID="btlMiniCurriculo" OnClick="btlMiniCurriculo_Click" Text="Mini Currículo"
                            ValidationGroup="CadastroRevisao" CssClass="texto_abas" runat="server"></asp:LinkButton>
                    </span>
                </div>
                <div class="abas_nova">
                    <span class="aba_esquerda_nao_selecionada aba">
                        <asp:LinkButton ID="btlDadosPessoais" runat="server" OnClick="btlDadosPessoais_Click"
                            ValidationGroup="CadastroRevisao" Text="Dados Pessoais e Profissionais" CssClass="texto_abas"></asp:LinkButton>
                    </span>
                </div>
                <div class="abas_nova">
                    <span class="aba_esquerda_nao_selecionada aba">
                        <asp:LinkButton ID="btlFormacaoCursos" runat="server" OnClick="btlFormacaoCursos_Click"
                            ValidationGroup="CadastroRevisao" Text="Formação e Cursos" CssClass="texto_abas"></asp:LinkButton>
                    </span>
                </div>
                <div class="abas_nova">
                    <span class="aba_esquerda_nao_selecionada aba">
                        <asp:LinkButton ID="btlDadosComplementares" runat="server" OnClick="btlDadosComplementares_Click"
                            ValidationGroup="CadastroDadosPessoais" Text="Dados Complementares" CssClass="texto_abas"></asp:LinkButton>
                    </span>
                </div>
                <div class="abas_nova">
                    <span class="aba_esquerda_selecionada aba">
                        <asp:Label ID="Label4" runat="server" Text="Conferir" CssClass="texto_abas_selecionada">
                        </asp:Label>
                    </span>
                </div>
            </div>
            <div class="abas" style="display: none">
                <span class="aba_fundo">
                    <asp:LinkButton ID="btlGestao" runat="server" OnClick="btlGestao_Click" ValidationGroup="CadastroRevisao"
                        CausesValidation="true" Text="Gestão"></asp:LinkButton>
                </span>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
<!-- FIM: Tabs -->
<!-- Painel: Dados Pessoais -->
<div class="interno_abas">
    <h2 class="titulo_painel_padrao" style="display: none">
        <asp:Label ID="lblTituloConferirDados" runat="server" Text="Conferir" />
    </h2>
    <asp:Panel ID="pnlConferirDados" runat="server" CssClass="painel_padrao div_curriculo_conteudo">
        <div class="painel_padrao_topo ">
            &nbsp;
        </div>
        <p class="texto_marcadores_obrigatorio">
            <%--Os campos marcados com um
                    <img alt="*" src="img/icone_obrigatorio.gif" />
                    são obrigatórios para o cadastro de seu currículo.--%>
            Confira atentamente todos os dados informados!
            <br />
            Clique sobre os dados para efetuar a alteração.
        </p>
        <div class="painel_cabecalho_curriculo">
            <div class="painel_nome_completo_codigo_cv">
                <div id="divNomeCandidato" runat="server" class="nome_curriculo">
                    <font size="2">
                        <h1>
                            <div id="divNomeLabel" runat="server" class="container_label">
                                <asp:Label ID="lblNome" runat="server" />
                            </div>
                            <div id="divNomeTextBox" runat="server" style="display: none" class="container_textbox">
                                <componente:AlfaNumerico CssClassTextBox="textbox_padrao campo_obrigatorio textbox_nome"
                                    ID="txtNome" runat="server" Columns="40" MensagemErroFormato='<%$ Resources: MensagemAviso, _100107 %>'
                                    MensagemErroIntervalo='<%$ Resources: MensagemAviso, _100107 %>' MensagemErroValor='<%$ Resources: MensagemAviso, _100107 %>'
                                    ValidationGroup="CadastroRevisao" ClientValidationFunction="ValidarNome" />
                            </div>
                            <h1></h1>
                        </h1>
                    </font>
                </div>
            </div>
        </div>
        <asp:Panel ID="pnlDadosPessoais" runat="server">
            <font size="2">
                <div class="titulo">
                    Dados Pessoais</div>
            </font>
            <table cellspacing="0">
                <tbody>
                    <tr>
                        <th width="220" align="left" valign="top">Sexo:
                        </th>
                        <td valign="top" class="celula_valor_esquerda">
                            <div id="divSexoLabel" runat="server" class="container_label">
                                <asp:Label ID="lblSexo" runat="server" />
                            </div>
                            <div id="divSexoTextBox" runat="server" style="display: none" class="container_campo container_textbox">
                                <asp:RadioButtonList ID="rblSexo" runat="server" SkinID="Horizontal" ValidationGroup="CadastroRevisao">
                                </asp:RadioButtonList>
                            </div>
                        </td>
                        <th width="100" align="left" valign="top" class="celula_label_direita">CPF:
                        </th>
                        <td valign="top">
                            <div id="divCPFLabel" runat="server" class="container_label">
                                <asp:Label ID="lblCPF" runat="server" />
                            </div>
                        </td>
                        <td valign="top" class="celula_foto" rowspan="9">
                            <div id="divFotoTextBox" runat="server">
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
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <th width="220" align="left" valign="top" id="thEstadoCivil" runat="server">Estado Civil:
                        </th>
                        <td valign="top">
                            <div id="divEstadoCivilLabel" runat="server" class="container_label">
                                <asp:Label ID="lblEstadoCivil" runat="server" />
                            </div>
                            <div id="divEstadoCivilTextBox" runat="server" class="container_textbox" style="display: none">
                                <div>
                                    <asp:CustomValidator ID="cvEstadoCivil" runat="server" ControlToValidate="ddlEstadoCivil"
                                        ValidationGroup="CadastroRevisao" ErrorMessage="Estado Civil Inválido. Selecione!"
                                        ClientValidationFunction="cvEstadoCivil_Validate">
                                    </asp:CustomValidator>
                                </div>
                                <asp:DropDownList ID="ddlEstadoCivil" runat="server" CssClass="textbox_padrao campo_obrigatorio">
                                </asp:DropDownList>
                            </div>
                        </td>
                        <th width="100" align="left" valign="top" class="celula_label_direita">
                            <asp:Literal ID="litFilhos" runat="server" Text="Filhos:" />
                        </th>
                        <td valign="top">
                            <div id="divFilhosLabel" runat="server" class="container_label">
                                <asp:Label ID="lblFilhos" runat="server" />
                            </div>
                            <div id="divFilhosTextBox" runat="server" style="display: none" class="container_textbox">
                                <asp:DropDownList ID="ddlFilhos" CssClass="textbox_padrao" runat="server">
                                </asp:DropDownList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <th width="220" align="left" valign="top">Data de Nascimento:
                        </th>
                        <td valign="top">
                            <div id="divDataNascimentoLabel" runat="server" class="container_label">
                                <asp:Label ID="lblDataNascimento" runat="server" />
                                <asp:Label ID="lblIdade" runat="server" />
                            </div>
                            <div id="divDataNascimentoTextBox" runat="server" style="display: none" class="container_textbox">
                                <componente:Data ID="txtDataDeNascimento" runat="server" ValidationGroup="CadastroRevisao" />
                            </div>
                        </td>
                        <th width="100" align="left" valign="top" class="celula_label_direita">
                            <asp:Literal ID="litHabilitacao" runat="server" Text="Habilitação:" />
                        </th>
                        <td valign="top">
                            <div id="divHabilitacaoLabel" runat="server" class="container_label">
                                <asp:Label ID="lblHabilitacao" runat="server" />
                            </div>
                            <div id="divHabilitacaoTextBox" runat="server" style="display: none" class="container_textbox">
                                <asp:DropDownList ID="ddlHabilitacao" CssClass="textbox_padrao" runat="server">
                                </asp:DropDownList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <th width="220" align="left" valign="top">
                            <asp:Literal ID="litTelefoneCelular" runat="server" Text="Telefone Celular:" />
                        </th>
                        <td valign="top">
                            <div id="divTelefoneCelularLabel" runat="server" class="container_label">
                                <asp:Label ID="lblTelefoneCelular" runat="server" />
                            </div>
                            <div id="divTelefoneCelularTextBox" runat="server" style="display: none" class="container_textbox">
                                <componente:Telefone ID="txtTelefoneCelular" runat="server" MensagemErroFormatoFone='<%$ Resources: MensagemAviso, _100006 %>'
                                    ValidationGroup="CadastroRevisao" Tipo="Celular" />
                            </div>
                        </td>
                        <th width="100" align="left" valign="top" class="celula_label_direita">
                            <asp:Literal ID="litEmail" runat="server" Text="E-mail:" />
                        </th>
                        <td valign="top">
                            <div id="divEmailLabel" runat="server" class="container_label">
                                <asp:Label ID="lblEmail" runat="server" />
                            </div>
                            <div id="divEmailTextBox" runat="server" style="display: none" class="container_textbox">
                                <div>
                                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                        ValidationGroup="CadastroRevisao" ErrorMessage="Email Inválido." ValidationExpression="^([0-9a-zA-Z]+([_.-]?[0-9a-zA-Z]+)*@[0-9a-zA-Z]+[0-9,a-z,A-Z,.,-]*(.){1}[a-zA-Z]{2,4})+$">
                                    </asp:RegularExpressionValidator>
                                </div>
                                <asp:TextBox ID="txtEmail" runat="server" Columns="30" MaxLength="50" CssClass="textbox_padrao">
                                </asp:TextBox>
                                <AjaxToolkit:AutoCompleteExtender ID="aceEmail" runat="server" TargetControlID="txtEmail"
                                    UseContextKey="False" ServiceMethod="ListarSugestaoEmail">
                                </AjaxToolkit:AutoCompleteExtender>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <th width="220" align="left" valign="top">
                            <asp:Literal ID="litTelefoneResidencial" runat="server" Text="Telefone Fixo Residencial:" />
                        </th>
                        <td valign="top" colspan="3">
                            <div id="divTelefoneResidencialLabel" runat="server" class="container_label">
                                <asp:Label ID="lblTelefoneResidencial" runat="server" />
                            </div>
                            <div id="divTelefoneResidencialTextBox" runat="server" style="display: none" class="container_textbox">
                                <componente:Telefone ID="txtTelefoneResidencial" runat="server" Obrigatorio="False"
                                    CssClassTextBoxDDI="textbox_padrao ddi" CssClassTextBoxDDD="textbox_padrao ddd"
                                    CssClassTextBoxFone="textbox_padrao" ValidationGroup="CadastroRevisao" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <th width="220" align="left" valign="top">
                            <asp:Literal ID="litTelefoneRecado" runat="server" Text="Telefone Fixo Recado:" />
                        </th>
                        <td valign="top">
                            <div id="divTelefoneRecadoLabel" runat="server" class="container_label">
                                <asp:Label ID="lblTelefoneRecado" runat="server" />
                            </div>
                            <div id="divTelefoneRecadoTextBox" runat="server" style="display: none" class="container_textbox">
                                <componente:Telefone ID="txtTelefoneRecado" runat="server" Obrigatorio="False" CssClassTextBoxDDD="textbox_padrao ddd"
                                    CssClassTextBoxDDI="textbox_padrao ddi" CssClassTextBoxFone="textbox_padrao"
                                    ValidationGroup="CadastroRevisao" />
                            </div>
                        </td>
                        <th width="100" align="left" valign="top" class="celula_label_direita">
                            <asp:Literal ID="litTelefoneRecadoFalarCom" runat="server" Text="Falar com:" />
                        </th>
                        <td valign="top">
                            <div id="divTelefoneRecadoFalarComLabel" runat="server" class="container_label">
                                <asp:Label ID="lblTelefoneRecadoFalarCom" runat="server" />
                            </div>
                            <div id="divTelefoneRecadoFalarComTextBox" runat="server" style="display: none" class="container_textbox">
                                <asp:TextBox ID="txtTelefoneRecadoFalarCom" runat="server" Columns="32" MaxLength="50"
                                    CssClass="textbox_padrao">
                                </asp:TextBox>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <th width="220" align="left" valign="top">
                            <asp:Literal ID="litCelularRecado" runat="server" Text="Celular Recado:" />
                        </th>
                        <td valign="top">
                            <div id="divCelularRecadoLabel" runat="server" class="container_label">
                                <asp:Label ID="lblCelularRecado" runat="server" />
                            </div>
                            <div id="divCelularRecadoTextBox" runat="server" style="display: none" class="container_textbox">
                                <componente:Telefone ID="txtCelularRecado" runat="server" Obrigatorio="False" CssClassTextBoxDDD="textbox_padrao ddd"
                                    CssClassTextBoxDDI="textbox_padrao ddi" CssClassTextBoxFone="textbox_padrao celular"
                                    ValidationGroup="CadastroRevisao" Tipo="Celular" />
                            </div>
                        </td>
                        <th width="100" align="left" valign="top" class="celula_label_direita">
                            <asp:Literal ID="litCelularRecadoFalarCom" runat="server" Text="Falar com:" />
                        </th>
                        <td valign="top">
                            <div id="divCelularRecadoFalarComLabel" runat="server" class="container_label">
                                <asp:Label ID="lblCelularRecadoFalarCom" runat="server" />
                            </div>
                            <div id="divCelularRecadoFalarComTextBox" runat="server" style="display: none" class="container_textbox">
                                <asp:TextBox ID="txtCelularRecadoFalarCom" runat="server" Columns="32" MaxLength="50"
                                    CssClass="textbox_padrao">
                                </asp:TextBox>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <th width="220" align="left" valign="top">
                            <asp:Literal ID="litCep" runat="server" Text="CEP:" />
                        </th>
                        <td valign="top">
                            <div id="divCEPLabel" runat="server" class="container_label" style="float: left;">
                                <asp:Label ID="lblCep" runat="server" />
                            </div>
                            <div id="divCEPTextBox" runat="server" style="display: none" class="container_textbox">
                                <asp:UpdatePanel ID="upCEP" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <componente:CEP ID="txtCEP" runat="server" OnValorAlterado="txtCEP_ValorAlterado"
                                            ValidationGroup="CadastroRevisao" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div style="display: block; float: left;">
                                <!-- UserControl Pesquisa CEP -->
                                <uc2:BuscarCEP ID="ucBuscarCEP" runat="server" OnVoltarFoco="ucBuscarCEP_VoltarFoco" />
                                <!-- FIM: UserControl Pesquisa CEP -->
                            </div>
                        </td>
                        <th width="100" align="left" valign="top" class="celula_label_direita">Cidade:
                        </th>
                        <td valign="top">
                            <div id="divCidadeLabel" runat="server" class="container_label">
                                <asp:UpdatePanel ID="upCidadeLabel" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <asp:Label ID="lblCidade" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div id="divCidadeTextBox" runat="server" style="display: none" class="container_textbox">
                                <asp:UpdatePanel ID="upCidadeTextbox" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <asp:CustomValidator ID="cvCidade" runat="server" ControlToValidate="txtCidade" ValidationGroup="CadastroRevisao"
                                            ErrorMessage="Valor Incorreto." ClientValidationFunction="cvCidade_Validate">
                                        </asp:CustomValidator>
                                        <asp:RequiredFieldValidator ID="rfvCidade" runat="server" ErrorMessage="Campo Obrigatório."
                                            ControlToValidate="txtCidade" ValidationGroup="CadastroRevisao">
                                        </asp:RequiredFieldValidator>
                                        <asp:TextBox ID="txtCidade" runat="server" MaxLength="50">
                                        </asp:TextBox>
                                        <AjaxToolkit:AutoCompleteExtender runat="server" ID="aceCidade" TargetControlID="txtCidade"
                                            ServiceMethod="ListarCidadesSiglaEstadoPorNomeParcialEstado" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <th width="220" align="left" valign="top">
                            <asp:Literal ID="litEndereco" runat="server" Text="Endereço:" />
                        </th>
                        <td valign="top" colspan="3">
                            <asp:UpdatePanel ID="upEndereco" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <div id="divEnderecoLabel" runat="server" class="container_label">
                                        <asp:Label ID="lblLogradouro" runat="server" />
                                        ,
                                        <asp:Label ID="lblNumero" runat="server" />
                                        ,
                                        <asp:Label ID="lblComplemento" runat="server" />
                                        ,
                                        <asp:Label ID="lblBairro" runat="server" />
                                    </div>
                                    <div id="divEnderecoTextBox" runat="server" style="display: none" class="container_textbox">
                                        <div class="linha">
                                            <asp:Label ID="lblLogradouroTextbox" CssClass="label_principal" runat="server" Text="Endereço"
                                                AssociatedControlID="txtLogradouro" />
                                            <div class="container_campo">
                                                <div>
                                                    <asp:RequiredFieldValidator ID="rfvLogradouro" runat="server" ErrorMessage="Campo Obrigatório."
                                                        ControlToValidate="txtLogradouro" ValidationGroup="CadastroRevisao"></asp:RequiredFieldValidator>
                                                </div>
                                                <asp:TextBox Columns="40" ID="txtLogradouro" runat="server" Enabled="False"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="linha">
                                            <asp:Label CssClass="label_numero" ID="lblNumeroTextbox" runat="server" Text="Número"
                                                AssociatedControlID="txtNumero" />
                                            <div class="container_campo">
                                                <div>
                                                    <asp:RequiredFieldValidator ID="rfvNumero" runat="server" ErrorMessage="Campo Obrigatório."
                                                        ControlToValidate="txtNumero" ValidationGroup="CadastroRevisao"></asp:RequiredFieldValidator>
                                                </div>
                                                <asp:TextBox ID="txtNumero" Columns="5" runat="server" MaxLength="15" Enabled="False"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="linha">
                                            <asp:Label ID="lblComplementoTextbox" CssClass="label_principal" runat="server" Text="Complemento"
                                                AssociatedControlID="txtComplemento" />
                                            <div class="container_campo">
                                                <asp:TextBox Columns="25" ID="txtComplemento" runat="server" CssClass="textbox_padrao textbox_complemento"
                                                    Enabled="False"></asp:TextBox>
                                            </div>
                                        </div>
                                        <%-- Linha Bairro --%>
                                        <div class="linha">
                                            <asp:Label CssClass="label_bairro" ID="lblBairroTextbox" AssociatedControlID="txtBairro"
                                                runat="server" Text="Bairro" />
                                            <div class="container_campo">
                                                <div>
                                                    <asp:RequiredFieldValidator ID="rfvBairro" runat="server" ErrorMessage="Campo Obrigatório."
                                                        ControlToValidate="txtBairro" ValidationGroup="CadastroRevisao"></asp:RequiredFieldValidator>
                                                </div>
                                                <asp:TextBox ID="txtBairro" runat="server" MaxLength="50" Enabled="False"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>

                </tbody>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlPretensoes" runat="server">
            <font size="2">
                <div class="titulo">
                    Pretensões</div>
            </font>
            <table cellspacing="0">
                <tbody>
                    <%--     //[Obsolete("Obtado por não utilização/disponibilização.")]
                    <tr>
                        <th width="220" align="left" valign="top">
                            <asp:Literal ID="litTipoContrato" runat="server" Text="Contrato Pretendido:" />
                        </th>
                        <td valign="top" class="celula_valor_esquerda" colspan="3">
                            <div id="divTipoContratoLabel" runat="server" class="container_label">
                                <asp:Label ID="lblTipoContrato" runat="server" />
                            </div>
                            <div id="divTipoContratoTextBox" runat="server" style="display: none" class="container_textbox">
                                <div>
                                    <a:CheckBoxListValidator runat="server" ID="cvChblTipoContrato" CssClass="validador_sem_asterisco" Display="Dynamic" ValidationGroup="CadastroRevisao"
                                         EnableClientScript="True" ControlToValidate="chblTipoContrato" MinimumNumberOfSelectedCheckBoxes="1" ErrorMessage="É necessário escolher pelo menos um tipo de contrato." />
                                </div>
                                <asp:CheckBoxList RepeatLayout="Table" TextAlign="Right" CssClass="container_box_tipo_contrato_revisao" runat="server" ID="chblTipoContrato" RepeatDirection="Horizontal" RepeatColumns="3"  />
                            </div>
                        </td>
                    </tr>--%>

                    <tr>
                        <th width="220" align="left" valign="top">
                            <asp:Literal ID="litFuncaoPretendida" runat="server" Text="Funções Pretendidas:" />
                        </th>
                        <td valign="top" class="celula_valor_esquerda">
                            <div id="divFuncaoPretendida1Label" runat="server" class="container_label">
                                <asp:Label ID="lblFuncaoPretendida1" runat="server" />
                            </div>
                            <div id="divFuncaoPretendida1TextBox" runat="server" style="display: none" class="container_textbox">
                                <div>
                                    <asp:RequiredFieldValidator ID="rfvFuncaoPretendida1" runat="server" ControlToValidate="txtFuncaoPretendida1"
                                        ValidationGroup="CadastroRevisao">
                                    </asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cvFuncaoPretendida1" runat="server" ClientValidationFunction="cvFuncaoPretendida1_Validate"
                                        ControlToValidate="txtFuncaoPretendida1" EnableClientScript="True" ValidationGroup="CadastroRevisao">
                                    </asp:CustomValidator>
                                </div>
                                <asp:TextBox ID="txtFuncaoPretendida1" runat="server" Columns="40">
                                </asp:TextBox>
                                <AjaxToolkit:AutoCompleteExtender ID="aceFuncaoPretendida1" runat="server" TargetControlID="txtFuncaoPretendida1"
                                    UseContextKey="True" ServiceMethod="ListarFuncoes">
                                </AjaxToolkit:AutoCompleteExtender>
                            </div>
                            <div id="divFuncaoPretendida2Label" runat="server" class="container_label">
                                <asp:Label ID="lblFuncaoPretendida2" runat="server" />
                            </div>
                            <div id="divFuncaoPretendida2TextBox" runat="server" style="display: none" class="container_textbox">
                                <asp:TextBox ID="txtFuncaoPretendida2" runat="server" CssClass="textbox_padrao" Columns="42">
                                </asp:TextBox>
                                <AjaxToolkit:AutoCompleteExtender ID="aceFuncaoPretendida2" runat="server" TargetControlID="txtFuncaoPretendida2"
                                    UseContextKey="True" ServiceMethod="ListarFuncoes">
                                </AjaxToolkit:AutoCompleteExtender>
                            </div>
                            <div id="divFuncaoPretendida3Label" runat="server" class="container_label">
                                <asp:Label ID="lblFuncaoPretendida3" runat="server" />
                            </div>
                            <div id="divFuncaoPretendida3TextBox" runat="server" style="display: none" class="container_textbox">
                                <asp:TextBox ID="txtFuncaoPretendida3" runat="server" CssClass="textbox_padrao" Columns="42">
                                </asp:TextBox>
                                <AjaxToolkit:AutoCompleteExtender ID="aceFuncaoPretendida3" runat="server" TargetControlID="txtFuncaoPretendida3"
                                    UseContextKey="True" ServiceMethod="ListarFuncoes">
                                </AjaxToolkit:AutoCompleteExtender>
                            </div>
                        </td>
                        <th width="100" align="left" valign="top" class="celula_label_direita">
                            <asp:Literal ID="litPretensaoSalarial" runat="server" Text="Salário:" />
                        </th>
                        <td valign="top">
                            <div id="divPretensaoSalarialLabel" runat="server" class="container_label">
                                <asp:Label ID="lblPretensaoSalarial" runat="server" />
                            </div>
                            <div id="divPretensaoSalarialTextBox" runat="server" style="display: none" class="container_textbox">
                                <componente:ValorDecimal ID="txtPretensaoSalarial" runat="server" CasasDecimais="0"
                                    ValidationGroup="CadastroRevisao" ValorMaximo="999999" ValorMinimo="1" CssClassTextBox="textbox_padrao campo_obrigatorio esquerda" />
                                <label class="decimais">
                                    ,00</label>
                                <div style="clear: both;">
                                    <span id="faixa_salarial" style="display: none" class="faixa_informacao_destaque" />
                                </div>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlDadosEscolaridade" runat="server">
            <%-- HTML Email --%>
            <font size="2">
                <div class="titulo">
                    Escolaridade</div>
            </font>
            <table cellspacing="0">
                <tbody>
                    <tr>
                        <th width="220" align="left" valign="top">
                            <asp:Literal ID="litEscolaridadeNivel" runat="server" Text="Nível:" />
                        </th>
                        <td valign="top">
                            <asp:UpdatePanel ID="upFormacao" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div id="divFormacaoLabel" runat="server" class="container_label">
                                        <asp:Label ID="lblCursos" runat="server" />
                                        <asp:Repeater ID="rptCursos" runat="server" OnItemCommand="rptCursos_ItemCommand">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lblNivelValor" runat="server" Visible='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Des_Bne").ToString()) ? false : true %>'
                                                    Text='<%# Eval("Des_BNE") %>' CommandName="Editar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Idf_Formacao")%>' />
                                                <asp:LinkButton ID="lblTituloCursoValor" runat="server" Text='<%# " - " +  Eval("Des_Curso")%>'
                                                    Visible='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Des_Curso").ToString()) ? false : true %>'
                                                    CommandName="Editar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Idf_Formacao")%>' />
                                                <asp:LinkButton ID="lblSiglaInstituicaoValor" runat="server" Text='<%# " - " +  Eval("Sig_Fonte") %>'
                                                    Visible='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Sig_Fonte").ToString()) ? false : true %>'
                                                    CommandName="Editar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Idf_Formacao")%>' />
                                                <asp:LinkButton ID="lblInsituicaoValor" runat="server" Text='<%# " - " +  Eval("Nme_Fonte") %>'
                                                    Visible='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Nme_Fonte").ToString()) ? false : true %>'
                                                    CommandName="Editar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Idf_Formacao")%>' />
                                                <asp:LinkButton ID="lblAnoConclusaoValor" runat="server" Text='<%# " - " + Eval("Ano_Conclusao") %>'
                                                    Visible='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Ano_Conclusao").ToString()) ? false : true %>'
                                                    CommandName="Editar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Idf_Formacao")%>' />
                                                <br />
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                    <div id="divFormacaoTextBox" runat="server" style="display: none;" class="container_textbox">
                                        <asp:Panel ID="pnlFormacao" runat="server" CssClass="painel_padrao">
                                            <!-- Linha Nível-->
                                            <div class="linha">
                                                <asp:Label ID="lblNivel" runat="server" Text="Nível de Formação" AssociatedControlID="ddlNivel"
                                                    CssClass="label_principal"></asp:Label>
                                                <div class="container_campo">
                                                    <asp:UpdatePanel ID="upNivel" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlNivel" runat="server" ValidationGroup="CadastroFormacao"
                                                                CssClass="textbox_padrao campo_obrigatorio">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <!-- FIM: Linha Nível -->
                                            <!-- Linha Instituicao -->
                                            <div id="divLinhaInstituicao" class="linha">
                                                <asp:Label ID="lblInstituicao" runat="server" Text="Instituição de Ensino" CssClass="label_principal"
                                                    AssociatedControlID="txtInstituicao"></asp:Label>
                                                <div class="container_campo">
                                                    <div>
                                                        <asp:RequiredFieldValidator ID="rfvInstituicao" runat="server" ControlToValidate="txtInstituicao"
                                                            ValidationGroup="CadastroFormacao"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <asp:TextBox ID="txtInstituicao" runat="server" CssClass="textbox_padrao campo_obrigatorio"
                                                        Columns="50" MaxLength="100"></asp:TextBox>
                                                    <AjaxToolkit:AutoCompleteExtender ID="aceInstituicao" runat="server" TargetControlID="txtInstituicao"
                                                        ServiceMethod="ListarSiglaNomeFonteNivelCurso">
                                                    </AjaxToolkit:AutoCompleteExtender>
                                                </div>
                                            </div>
                                            <!-- FIM: Linha Instituicao -->
                                            <!-- Linha Titulo Curso -->
                                            <div id="divLinhaTituloCurso" class="linha">
                                                <asp:Label ID="Label3" runat="server" Text="Nome do Curso" CssClass="label_principal"
                                                    AssociatedControlID="txtTituloCurso"></asp:Label>
                                                <div class="container_campo">
                                                    <div>
                                                        <asp:RequiredFieldValidator ID="rfvTituloCurso" runat="server" ControlToValidate="txtTituloCurso"
                                                            ValidationGroup="CadastroFormacao"></asp:RequiredFieldValidator>
                                                        <asp:CustomValidator ID="cvTituloCurso" runat="server" ErrorMessage="Curso Inválida."
                                                            ClientValidationFunction="cvTituloCurso_Validate" ControlToValidate="txtTituloCurso"
                                                            ValidationGroup="CadastroFormacao"></asp:CustomValidator>
                                                    </div>
                                                    <asp:TextBox ID="txtTituloCurso" runat="server" CssClass="textbox_padrao campo_obrigatorio"
                                                        MaxLength="100"></asp:TextBox>
                                                    <AjaxToolkit:AutoCompleteExtender ID="aceTituloCurso" runat="server" TargetControlID="txtTituloCurso"
                                                        ServiceMethod="ListarCursoFonte">
                                                    </AjaxToolkit:AutoCompleteExtender>
                                                </div>
                                            </div>
                                            <!-- FIM: Linha Titulo Curso -->
                                            <!-- Linha Cidade -->
                                            <div id="divCidade" class="linha">
                                                <asp:Label runat="server" ID="lblCidadeFormacao" Text="Cidade" CssClass="label_principal"
                                                    AssociatedControlID="txtCidadeFormacao" />
                                                <div class="container_campo">
                                                    <div>
                                                        <asp:RequiredFieldValidator ID="rfvCidadeFormacao" runat="server" ControlToValidate="txtCidadeFormacao"
                                                            ValidationGroup="CadastroFormacao"></asp:RequiredFieldValidator>
                                                        <asp:CustomValidator ID="cvCidadeFormacao" runat="server" ErrorMessage="Cidade Inválida."
                                                            ClientValidationFunction="cvCidadeFormacao_Validate" ControlToValidate="txtCidadeFormacao"
                                                            ValidationGroup="CadastroFormacao"></asp:CustomValidator>
                                                    </div>
                                                    <asp:TextBox ID="txtCidadeFormacao" runat="server" Columns="50" CssClass="textbox_padrao"
                                                        MaxLength="50"></asp:TextBox>
                                                    <AjaxToolkit:AutoCompleteExtender ID="aceCidadeFormacao" runat="server" TargetControlID="txtCidadeFormacao"
                                                        ServiceMethod="ListarCidadesSiglaEstadoPorNomeParcialEstado">
                                                    </AjaxToolkit:AutoCompleteExtender>
                                                </div>
                                            </div>
                                            <!-- FIM: Linha Cidade -->
                                            <!-- Linha Situacao -->
                                            <div id="divLinhaSituacao" class="linha">
                                                <asp:Label ID="lblSituacao" runat="server" Text="Situação" CssClass="label_principal"
                                                    AssociatedControlID="ddlSituacao"></asp:Label>
                                                <div class="container_campo">
                                                    <div>
                                                        <asp:CustomValidator ID="cvSituacao" runat="server" ValidationGroup="CadastroFormacao"
                                                            ControlToValidate="ddlSituacao" ErrorMessage="Campo Obrigatório" ClientValidationFunction="cvSituacao_Validate">
                                                        </asp:CustomValidator>
                                                    </div>
                                                    <asp:DropDownList ID="ddlSituacao" CssClass="textbox_padrao campo_obrigatorio" runat="server"
                                                        ValidationGroup="CadastroFormacao">
                                                    </asp:DropDownList>
                                                </div>
                                                <asp:Label ID="lblPeriodo" runat="server" Text="Período" AssociatedControlID="txtPeriodo"></asp:Label>
                                                <componente:AlfaNumerico ID="txtPeriodo" runat="server" ValidationGroup="CadastroFormacao"
                                                    ContemIntervalo="true" ValorMinimo="1" ValorMaximo="12" Columns="2" MaxLength="2"
                                                    Tipo="Numerico" />
                                            </div>
                                            <!-- FIM: Linha Situacao -->
                                            <!-- Linha Ano Conclusao -->
                                            <div id="divLinhaConclusao" class="linha">
                                                <asp:Label ID="lblAnoConclusao" runat="server" Text="Ano de Conclusão" CssClass="label_principal"
                                                    AssociatedControlID="txtAnoConclusao"></asp:Label>
                                                <div class="container_campo">
                                                    <componente:AlfaNumerico CssClassTextBox="textbox_padrao" ID="txtAnoConclusao" runat="server"
                                                        Tipo="Numerico" MaxLength="4" Obrigatorio="false" Columns="4" ContemIntervalo="true"
                                                        ValorMinimo="1950" ValorMaximo="2500" ValidationGroup="CadastroFormacao" />
                                                </div>
                                            </div>
                                            <!-- FIM: Linha Ano Conclusao -->
                                            <!-- Painel botoes -->
                                            <asp:Panel ID="pnlBotoesFormacao" runat="server" CssClass="painel_botoes">
                                                <asp:Button ID="btnSalvarFormacao" runat="server" Text="Salvar Curso" CausesValidation="true"
                                                    CssClass="botao_padrao" ValidationGroup="CadastroFormacao" OnClick="btnSalvarFormacao_Click" />
                                            </asp:Panel>
                                            <!-- FIM: Painel botoes -->
                                            <!--GridView Cursos consulta-->
                                            <div>
                                                <telerik:RadGrid ID="gvFormacao" runat="server" AllowSorting="False" OnItemCommand="gvFormacao_ItemCommand"
                                                    AllowPaging="False" AllowCustomPaging="False" ShowHeader="True" AlternatingRowStyle-CssClass="alt_row">
                                                    <ClientSettings EnablePostBackOnRowClick="true">
                                                    </ClientSettings>
                                                    <MasterTableView TableLayout="Fixed" DataKeyNames="Idf_Formacao">
                                                        <Columns>
                                                            <telerik:GridBoundColumn HeaderText="Nível de formação" DataField="Des_BNE" HeaderStyle-CssClass="rgHeader centro" />
                                                            <telerik:GridBoundColumn HeaderText="Nome do Curso" DataField="Des_Curso" HeaderStyle-CssClass="rgHeader centro" />
                                                            <telerik:GridBoundColumn HeaderText="Instituição de Ensino" DataField="Nme_Fonte"
                                                                HeaderStyle-CssClass="rgHeader centro" />
                                                            <telerik:GridBoundColumn HeaderText="Data de Conclusão" DataField="Ano_Conclusao"
                                                                HeaderStyle-CssClass="rgHeader centro" />
                                                            <telerik:GridTemplateColumn HeaderText="Ações" HeaderStyle-CssClass="rgHeader centro">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btiEditar" runat="server" ToolTip="Editar" AlternateText="Editar"
                                                                        CausesValidation="False" CommandArgument='<%# Eval("Idf_Formacao") %>' ImageUrl="~/img/icone_editar_16x16.gif"
                                                                        CommandName="Editar" />
                                                                    <asp:ImageButton ID="btiExcluir" runat="server" AlternateText="Excluir" CausesValidation="False"
                                                                        ToolTip="Excluir" CommandName="Delete" CommandArgument='<%# Eval("Idf_Formacao") %>'
                                                                        ImageUrl="~/img/icone_excluir_16x16.png" />
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="rgHeader centro coluna_icones" />
                                                                <ItemStyle CssClass="espaco_icones centro" />
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </div>
                                            <!--FIM: GridView Cursos consulta-->
                                        </asp:Panel>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <!-- FIM Painel: Formacao -->
                        </td>
                    </tr>
                    <tr id="trEspecializacao" runat="server">
                        <th width="220" align="left" valign="top">
                            <asp:UpdatePanel ID="upEspecializacaoValor" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlEspecializacaoNivel" runat="server">
                                        <asp:Literal ID="litEspecializacaoNivel" runat="server" Text="Especializacao:" />
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </th>
                        <td valign="top">
                            <asp:UpdatePanel ID="upEspecializacao" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlEspecializacao" runat="server">
                                        <div id="divEspecializacaoLabel" runat="server" class="container_label">
                                            <asp:Label ID="lblEspecializacao" runat="server" />
                                            <asp:Repeater ID="rptEspecializacao" runat="server" OnItemCommand="rptEspecializacao_ItemCommand">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lblNivelValor" runat="server" Visible='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Des_Bne").ToString()) ? false : true %>'
                                                        Text='<%# Eval("Des_BNE") %>' CommandName="Editar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Idf_Formacao")%>' />
                                                    <asp:LinkButton ID="lblTituloCursoValor" runat="server" Text='<%# " - " +  Eval("Des_Curso")%>'
                                                        Visible='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Des_Curso").ToString()) ? false : true %>'
                                                        CommandName="Editar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Idf_Formacao")%>' />
                                                    <asp:LinkButton ID="lblSiglaInstituicaoValor" runat="server" Text='<%# " - " +  Eval("Sig_Fonte") %>'
                                                        Visible='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Sig_Fonte").ToString()) ? false : true %>'
                                                        CommandName="Editar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Idf_Formacao")%>' />
                                                    <asp:LinkButton ID="lblInsituicaoValor" runat="server" Text='<%# " - " +  Eval("Nme_Fonte") %>'
                                                        Visible='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Nme_Fonte").ToString()) ? false : true %>'
                                                        CommandName="Editar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Idf_Formacao")%>' />
                                                    <asp:LinkButton ID="lblAnoConclusaoValor" runat="server" Text='<%# " - " + Eval("Ano_Conclusao") %>'
                                                        Visible='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Ano_Conclusao").ToString()) ? false : true %>'
                                                        CommandName="Editar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Idf_Formacao")%>' />
                                                    <br />
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                        <div id="divEspecializacaoTextBox" runat="server" style="display: none;" class="container_textbox">
                                            <div class="linha">
                                                <asp:Label ID="lblNivelEspecializacao" runat="server" Text="Nível de Especialização"
                                                    AssociatedControlID="ddlNivelEspecializacao" CssClass="label_principal"></asp:Label>
                                                <div class="container_campo">
                                                    <asp:UpdatePanel ID="upNivelEspecializacao" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlNivelEspecializacao" runat="server" ValidationGroup="CadastroEspecializacao"
                                                                CssClass="textbox_padrao campo_obrigatorio_radcombobox">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <!-- FIM: Linha Nível -->
                                            <!-- Linha Instituicao -->
                                            <div id="divLinhaInstituicaoEspecializacao" class="linha">
                                                <asp:Label ID="lblInstituicaoEspecializacao" runat="server" Text="Instituição de Ensino"
                                                    CssClass="label_principal" AssociatedControlID="txtInstituicaoEspecializacao"></asp:Label>
                                                <div class="container_campo">
                                                    <div>
                                                        <asp:RequiredFieldValidator ID="rfvInstituicaoEspecializacao" runat="server" ControlToValidate="txtInstituicaoEspecializacao"
                                                            ValidationGroup="CadastroEspecializacao"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <asp:TextBox ID="txtInstituicaoEspecializacao" runat="server" CssClass="textbox_padrao campo_obrigatorio"
                                                        Columns="50" MaxLength="100"></asp:TextBox>
                                                    <AjaxToolkit:AutoCompleteExtender ID="aceInstituicaoEspecializacao" runat="server"
                                                        TargetControlID="txtInstituicaoEspecializacao" ServiceMethod="ListarSiglaNomeFonteNivelCurso">
                                                    </AjaxToolkit:AutoCompleteExtender>
                                                </div>
                                            </div>
                                            <!-- FIM: Linha Instituicao -->
                                            <!-- Linha Titulo Curso -->
                                            <div id="divLinhaTituloCursoEspecializacao" class="linha">
                                                <asp:Label ID="Label6" runat="server" Text="Nome do Curso" CssClass="label_principal"
                                                    AssociatedControlID="txtTituloCursoEspecializacao"></asp:Label>
                                                <div class="container_campo">
                                                    <div>
                                                        <asp:RequiredFieldValidator ID="rfvTituloCursoEspecializacao" runat="server" ControlToValidate="txtTituloCursoEspecializacao"
                                                            ValidationGroup="CadastroEspecializacao"></asp:RequiredFieldValidator>
                                                        <asp:CustomValidator ID="cvTituloCursoEspecializacao" runat="server" ErrorMessage="Curso Inválido."
                                                            ClientValidationFunction="cvTituloCurso_Validate" ControlToValidate="txtTituloCursoEspecializacao"
                                                            ValidationGroup="CadastroEspecializacao"></asp:CustomValidator>
                                                    </div>
                                                    <asp:TextBox ID="txtTituloCursoEspecializacao" runat="server" CssClass="textbox_padrao campo_obrigatorio"
                                                        MaxLength="100"></asp:TextBox>
                                                    <AjaxToolkit:AutoCompleteExtender ID="aceTituloCursoEspecializacao" runat="server"
                                                        TargetControlID="txtTituloCursoEspecializacao" ServiceMethod="ListarCursoFonte">
                                                    </AjaxToolkit:AutoCompleteExtender>
                                                </div>
                                            </div>
                                            <!-- FIM: Linha Titulo Curso -->
                                            <!-- Linha Cidade -->
                                            <div id="divCidadeEspecializacao" class="linha">
                                                <asp:Label runat="server" ID="Label9" Text="Cidade" CssClass="label_principal" AssociatedControlID="txtCidadeEspecializacao" />
                                                <div class="container_campo">
                                                    <div>
                                                        <asp:RequiredFieldValidator ID="rfvCidadeEspecializacao" runat="server" ControlToValidate="txtCidadeEspecializacao"
                                                            ValidationGroup="CadastroEspecializacao"></asp:RequiredFieldValidator>
                                                        <asp:CustomValidator ID="cvCidadeEspecializacao" runat="server" ErrorMessage="Cidade Inválida."
                                                            ClientValidationFunction="cvCidade_Validate" ValidationGroup="CadastroEspecializacao"
                                                            ControlToValidate="txtCidadeEspecializacao"></asp:CustomValidator>
                                                    </div>
                                                    <asp:TextBox ID="txtCidadeEspecializacao" runat="server" Columns="50" CssClass="textbox_padrao"
                                                        MaxLength="50"></asp:TextBox>
                                                    <AjaxToolkit:AutoCompleteExtender ID="aceCidadeEspecializacao" runat="server" TargetControlID="txtCidadeEspecializacao"
                                                        ServiceMethod="ListarCidadesSiglaEstadoPorNomeParcialEstado">
                                                    </AjaxToolkit:AutoCompleteExtender>
                                                </div>
                                            </div>
                                            <!-- FIM: Linha Cidade -->
                                            <!-- Linha Situacao -->
                                            <div id="divLinhaSituacaoEspecializacao" class="linha">
                                                <asp:Label ID="lblSituacaoEspecializacao" runat="server" Text="Situação" CssClass="label_principal"
                                                    AssociatedControlID="ddlSituacaoEspecializacao"></asp:Label>
                                                <div class="container_campo">
                                                    <div>
                                                        <asp:CustomValidator ID="cvSituacaoEspecializacao" runat="server" ValidationGroup="CadastroEspecializacao"
                                                            ControlToValidate="ddlSituacaoEspecializacao" ErrorMessage="Campo Obrigatório"
                                                            ClientValidationFunction="cvSituacao_Validate">
                                                        </asp:CustomValidator>
                                                    </div>
                                                    <asp:DropDownList ID="ddlSituacaoEspecializacao" runat="server" ValidationGroup="CadastroEspecializacao">
                                                    </asp:DropDownList>
                                                </div>
                                                <asp:Label ID="lblPeriodoEspecializacao" runat="server" Text="Período" AssociatedControlID="txtPeriodoEspecializacao"></asp:Label>
                                                <componente:AlfaNumerico ID="txtPeriodoEspecializacao" runat="server" ValidationGroup="CadastroEspecializacao"
                                                    ContemIntervalo="true" ValorMinimo="1" ValorMaximo="12" Columns="2" MaxLength="2"
                                                    Tipo="Numerico" />
                                            </div>
                                            <!-- FIM: Linha Situacao -->
                                            <!-- Linha Ano Conclusao -->
                                            <div id="divLinhaConclusaoEspecializacao" class="linha">
                                                <asp:Label ID="lblAnoConclusaoEspecializacao" runat="server" Text="Ano de Conclusão"
                                                    CssClass="label_principal" AssociatedControlID="txtAnoConclusaoEspecializacao"></asp:Label>
                                                <div class="container_campo">
                                                    <componente:AlfaNumerico CssClassTextBox="textbox_padrao" ID="txtAnoConclusaoEspecializacao"
                                                        runat="server" Tipo="Numerico" MaxLength="4" Obrigatorio="false" Columns="4"
                                                        ContemIntervalo="true" ValorMinimo="1950" ValorMaximo="2500" ValidationGroup="CadastroEspecializacao" />
                                                </div>
                                            </div>
                                            <!-- FIM: Linha Ano Conclusao -->
                                            <!-- Painel botoes -->
                                            <asp:Panel ID="pnlBotoesEspecializacao" runat="server" CssClass="painel_botoes">
                                                <asp:Button ID="btnSalvarEspecializacao" runat="server" Text="Adicionar Curso" CausesValidation="true"
                                                    CssClass="botao_padrao" ValidationGroup="CadastroEspecializacao" OnClick="btnSalvarEspecializacao_Click" />
                                            </asp:Panel>
                                            <!-- FIM: Painel botoes -->
                                            <!-- GridView Especialização consulta-->
                                            <div>
                                                <telerik:RadGrid ID="gvEspecializacao" runat="server" AllowSorting="False" OnItemCommand="gvEspecializacao_ItemCommand"
                                                    AllowPaging="False" AllowCustomPaging="False" ShowHeader="True" AlternatingRowStyle-CssClass="alt_row">
                                                    <ClientSettings EnablePostBackOnRowClick="true">
                                                    </ClientSettings>
                                                    <MasterTableView TableLayout="Fixed" DataKeyNames="Idf_Formacao">
                                                        <Columns>
                                                            <telerik:GridBoundColumn HeaderText="Nível de formação" DataField="Des_BNE" HeaderStyle-CssClass="rgHeader centro" />
                                                            <telerik:GridBoundColumn HeaderText="Nome do Curso" DataField="Des_Curso" HeaderStyle-CssClass="rgHeader centro" />
                                                            <telerik:GridBoundColumn HeaderText="Instituição de Ensino" DataField="Nme_Fonte"
                                                                HeaderStyle-CssClass="rgHeader centro" />
                                                            <telerik:GridBoundColumn HeaderText="Data de Conclusão" DataField="Ano_Conclusao"
                                                                HeaderStyle-CssClass="rgHeader centro" />
                                                            <telerik:GridTemplateColumn HeaderText="Ações" HeaderStyle-CssClass="rgHeader centro">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btiEditar" runat="server" ToolTip="Editar" AlternateText="Editar"
                                                                        CausesValidation="False" CommandArgument='<%# Eval("Idf_Formacao") %>' ImageUrl="~/img/icone_editar_16x16.gif"
                                                                        CommandName="Editar" />
                                                                    <asp:ImageButton ID="btiExcluir" runat="server" AlternateText="Excluir" CausesValidation="False"
                                                                        ToolTip="Excluir" CommandName="Delete" CommandArgument='<%# Eval("Idf_Formacao") %>'
                                                                        ImageUrl="~/img/icone_excluir_16x16.png" />
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="rgHeader centro coluna_icones" />
                                                                <ItemStyle CssClass="espaco_icones centro" />
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </div>
                                            <!-- FIM: GridView Especialização consulta-->
                                        </div>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <th width="220" align="left" valign="top">
                            <asp:Literal ID="litCursosComplementares" runat="server" Text="Cursos Complementares:" />
                        </th>
                        <td valign="top">
                            <asp:UpdatePanel ID="upComplementares" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div id="divComplementaresLabel" runat="server" class="container_label">
                                        <asp:Label ID="lblComplementares" runat="server" />
                                        <asp:Repeater ID="rptComplementar" runat="server" OnItemCommand="rptComplementar_ItemCommand">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lblNivelValor" runat="server" Visible='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Des_Bne").ToString()) ? false : true %>'
                                                    Text='<%# Eval("Des_BNE") %>' CommandName="Editar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Idf_Formacao")%>' />
                                                <asp:LinkButton ID="lblTituloCursoValor" runat="server" Text='<%# " - " +  Eval("Des_Curso")%>'
                                                    Visible='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Des_Curso").ToString()) ? false : true %>'
                                                    CommandName="Editar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Idf_Formacao")%>' />
                                                <asp:LinkButton ID="lblSiglaInstituicaoValor" runat="server" Text='<%# " - " +  Eval("Sig_Fonte") %>'
                                                    Visible='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Sig_Fonte").ToString()) ? false : true %>'
                                                    CommandName="Editar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Idf_Formacao")%>' />
                                                <asp:LinkButton ID="lblInsituicaoValor" runat="server" Text='<%# " - " +  Eval("Nme_Fonte") %>'
                                                    Visible='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Nme_Fonte").ToString()) ? false : true %>'
                                                    CommandName="Editar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Idf_Formacao")%>' />
                                                <asp:LinkButton ID="lblAnoConclusaoValor" runat="server" Text='<%# " - " + Eval("Ano_Conclusao") %>'
                                                    Visible='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Ano_Conclusao").ToString()) ? false : true %>'
                                                    CommandName="Editar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Idf_Formacao")%>' />
                                                <br />
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                    <div id="divComplementaresTextBox" runat="server" style="display: none" class="container_textbox">
                                        <!-- Linha Instituicao  -->
                                        <div class="linha">
                                            <asp:Label ID="lblInstituicaoComplementar" runat="server" Text="Instituição de Ensino"
                                                CssClass="label_principal" AssociatedControlID="txtInstituicaoComplementar"></asp:Label>
                                            <div class="container_campo">
                                                <div>
                                                    <asp:RequiredFieldValidator ID="rfvInstituicaoComplementar" runat="server" ControlToValidate="txtInstituicaoComplementar"
                                                        ValidationGroup="CadastroComplementar"></asp:RequiredFieldValidator>
                                                </div>
                                                <asp:TextBox ID="txtInstituicaoComplementar" runat="server" CssClass="textbox_padrao"
                                                    Columns="50" MaxLength="100"></asp:TextBox>
                                                <AjaxToolkit:AutoCompleteExtender ID="aceInstituicaoComplementar" runat="server"
                                                    TargetControlID="txtInstituicaoComplementar" ServiceMethod="ListarSiglaNomeFonteNivelCurso">
                                                </AjaxToolkit:AutoCompleteExtender>
                                            </div>
                                        </div>
                                        <!-- FIM: Linha Instituicao -->
                                        <!-- Linha Titulo Curso -->
                                        <div class="linha">
                                            <asp:Label ID="lblTituloCursoComplementar" runat="server" Text="Nome do Curso" CssClass="label_principal"
                                                AssociatedControlID="txtTituloCursoComplementar"></asp:Label>
                                            <div class="container_campo">
                                                <div>
                                                    <asp:RequiredFieldValidator ID="rfvTituloCursoComplementar" runat="server" ControlToValidate="txtTituloCursoComplementar"
                                                        ValidationGroup="CadastroComplementar"></asp:RequiredFieldValidator>
                                                    <asp:CustomValidator ID="cvTituloCursoComplementar" runat="server" ErrorMessage="Cidade Inválida."
                                                        ClientValidationFunction="cvTituloCursoComplementar_Validate" ControlToValidate="txtTituloCursoComplementar"
                                                        ValidationGroup="CadastroComplementar"></asp:CustomValidator>
                                                </div>
                                                <asp:TextBox ID="txtTituloCursoComplementar" runat="server" CssClass="textbox_padrao"
                                                    MaxLength="100"></asp:TextBox>
                                                <%--Auto Complete estará disponivel quando for criado uma tabela de nome de cursos complementares--%>
                                                <AjaxToolkit:AutoCompleteExtender ID="aceTituloCursoComplementar" runat="server"
                                                    TargetControlID="txtTituloCursoComplementar" ServiceMethod="ListarCursoFonte">
                                                </AjaxToolkit:AutoCompleteExtender>
                                            </div>
                                        </div>
                                        <!-- FIM: Linha Titulo Curso -->
                                        <!-- Linha Cidade -->
                                        <div class="linha">
                                            <asp:Label runat="server" ID="lblCidadeComplementar" Text="Cidade" CssClass="label_principal"
                                                AssociatedControlID="txtCidadeComplementar" />
                                            <div class="container_campo">
                                                <div>
                                                    <asp:CustomValidator ID="cvCidadeComplementar" runat="server" ErrorMessage="Cidade Inválida."
                                                        ClientValidationFunction="cvCidadeComplementar_Validate" ControlToValidate="txtCidadeComplementar"
                                                        ValidationGroup="CadastroComplementar"></asp:CustomValidator>
                                                </div>
                                                <asp:TextBox ID="txtCidadeComplementar" runat="server" CssClass="textbox_padrao"
                                                    Columns="50" MaxLength="50"></asp:TextBox>
                                                <AjaxToolkit:AutoCompleteExtender ID="aceCidadeComplementar" runat="server" TargetControlID="txtCidadeComplementar"
                                                    ServiceMethod="ListarCidadesSiglaEstadoPorNomeParcialEstado">
                                                </AjaxToolkit:AutoCompleteExtender>
                                            </div>
                                        </div>
                                        <!-- FIM: Linha Cidade  -->
                                        <!-- Linha Ano Conclusao -->
                                        <div class="linha">
                                            <asp:Label ID="lblAnoConclusaoComplementar" runat="server" CssClass="label_principal"
                                                Text="Ano de Conclusão" AssociatedControlID="txtAnoConclusaoComplementar"></asp:Label>
                                            <div class="container_campo">
                                                <componente:AlfaNumerico ID="txtAnoConclusaoComplementar" runat="server" Tipo="Numerico"
                                                    Obrigatorio="false" Columns="6" MaxLength="4" ContemIntervalo="true" CssClassTextBox="textbox_padrao"
                                                    ValorMinimo="1950" ValorMaximo="2500" ValidationGroup="Complementar" />
                                            </div>
                                            <!-- Linha Carga Horária -->
                                            <div class="linha">
                                                <asp:Label ID="lblCargaHorariaComplementar" runat="server" Text="Carga Horária" CssClass="label_principal"
                                                    AssociatedControlID="txtCargaHorariaComplementar"></asp:Label>
                                                <div class="container_campo">
                                                    <componente:AlfaNumerico ID="txtCargaHorariaComplementar" runat="server" Tipo="Numerico"
                                                        MaxLength="6" Obrigatorio="false" CssClassTextBox="textbox_padrao" Columns="6"
                                                        ContemIntervalo="true" ValorMinimo="1" ValorMaximo="10000" ValidationGroup="Complementar" />
                                                </div>
                                            </div>
                                            <!-- FIM: Linha Carga Horária -->
                                        </div>
                                        <!-- FIM: Linha Ano Conclusao -->
                                        <!-- Painel botoes -->
                                        <asp:Panel ID="pnlBotoesCursosComplementares" runat="server" CssClass="painel_botoes">
                                            <asp:Button ID="btnSalvarCursoComplementar" runat="server" Text="Adicionar Curso"
                                                CausesValidation="true" CssClass="botao_padrao" ValidationGroup="CadastroComplementar"
                                                OnClick="btnSalvarCursoComplementar_Click" />
                                        </asp:Panel>
                                        <!-- FIM: Painel botoes -->
                                        <!--GridView Cursos consulta-->
                                        <div>
                                            <asp:UpdatePanel ID="upGvComplementar" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <telerik:RadGrid ID="gvComplementar" runat="server" AllowSorting="False" OnItemCommand="gvComplementar_ItemCommand"
                                                        AllowPaging="False" AllowCustomPaging="False" ShowHeader="True" AlternatingRowStyle-CssClass="alt_row">
                                                        <ClientSettings EnablePostBackOnRowClick="true">
                                                        </ClientSettings>
                                                        <MasterTableView TableLayout="Fixed" DataKeyNames="Idf_Formacao">
                                                            <Columns>
                                                                <telerik:GridBoundColumn HeaderText="Nível de formação" DataField="Des_BNE" HeaderStyle-CssClass="rgHeader centro" />
                                                                <telerik:GridBoundColumn HeaderText="Nome do Curso" DataField="Des_Curso" HeaderStyle-CssClass="rgHeader centro" />
                                                                <telerik:GridBoundColumn HeaderText="Instituição de Ensino" DataField="Nme_Fonte"
                                                                    HeaderStyle-CssClass="rgHeader centro" />
                                                                <telerik:GridBoundColumn HeaderText="Data de Conclusão" DataField="Ano_Conclusao"
                                                                    HeaderStyle-CssClass="rgHeader centro" />
                                                                <telerik:GridTemplateColumn HeaderText="Ações" HeaderStyle-CssClass="rgHeader centro">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btiEditar" runat="server" ToolTip="Editar" AlternateText="Editar"
                                                                            CausesValidation="False" CommandArgument='<%# Eval("Idf_Formacao") %>' ImageUrl="~/img/icone_editar_16x16.gif"
                                                                            CommandName="Editar" />
                                                                        <asp:ImageButton ID="btiExcluir" runat="server" AlternateText="Excluir" CausesValidation="False"
                                                                            ToolTip="Excluir" CommandName="Delete" CommandArgument='<%# Eval("Idf_Formacao") %>'
                                                                            ImageUrl="~/img/icone_excluir_16x16.png" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle CssClass="rgHeader centro coluna_icones" />
                                                                    <ItemStyle CssClass="espaco_icones centro" />
                                                                </telerik:GridTemplateColumn>
                                                            </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnSalvarCursoComplementar" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                        <!--FIM: GridView Cursos consulta-->
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr class="linha_idiomas">
                        <th width="220" align="left" valign="top">
                            <asp:Literal ID="litEscolaridadeIdiomas" runat="server" Text="Idiomas:" />
                        </th>
                        <td valign="top">
                            <asp:UpdatePanel ID="upIdiomas" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <div id="divIdiomasLabel" runat="server" class="container_label">
                                        <asp:Label ID="lblIdiomas" runat="server" />
                                        <asp:Repeater ID="rptIdiomas" runat="server" OnItemCommand="rptIdiomas_ItemCommand">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btlIdioma" runat="server" Text='<%# Eval("Des_Idioma") + " - " + Eval("Des_Nivel_Idioma") %>'
                                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Idf_Pessoa_Fisica_Idioma")%>'
                                                    CommandName="Editar" />
                                                <br />
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                    <div id="divIdiomasTextBox" runat="server" style="display: none" class="container_textbox">
                                        <!-- Linha Idioma -->
                                        <div class="linha">
                                            <asp:Label ID="lblIdioma" runat="server" Text="Idioma" CssClass="label_principal"
                                                AssociatedControlID="ddlIdioma"></asp:Label>
                                            <div class="container_campo">
                                                <asp:DropDownList ID="ddlIdioma" runat="server" CssClass="textbox_padrao" ValidationGroup="CadastroIdioma">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <!-- FIM: Linha Idioma -->
                                        <!-- Linha Nivel -->
                                        <div class="linha">
                                            <asp:Label ID="lblNivelidioma" runat="server" Text="Nível" CssClass="label_principal"
                                                AssociatedControlID="rblNivelIdioma"></asp:Label>
                                            <div class="container_campo">
                                                <div>
                                                    <asp:CustomValidator ID="cvNivelIdioma" runat="server" ValidationGroup="CadastroIdioma"
                                                        ErrorMessage="Selecione um nível de idioma" OnServerValidate="cvNivelIdioma_ServerValidate"
                                                        Display="Dynamic">
                                                    </asp:CustomValidator>
                                                </div>
                                                <asp:RadioButtonList ID="rblNivelIdioma" CssClass="radiobutton_padrao" runat="server">
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                        <!-- FIM: Linha Nivel -->
                                        <!-- Painel botoes -->
                                        <asp:Panel ID="Panel2" runat="server" CssClass="painel_botoes">
                                            <asp:Button ID="btnSalvarIdioma" runat="server" Text="Salvar Idioma" CausesValidation="True"
                                                CssClass="botao_padrao" ValidationGroup="CadastroIdioma" OnClick="btnSalvarIdioma_Click" />
                                        </asp:Panel>
                                        <!-- FIM: Painel botoes -->
                                        <!--GridView Idiomas consulta-->
                                        <div>
                                            <telerik:RadGrid ID="gvIdioma" runat="server" AllowSorting="False" OnItemCommand="gvIdioma_ItemCommand"
                                                AllowPaging="False" AllowCustomPaging="False" ShowHeader="True" AlternatingRowStyle-CssClass="alt_row">
                                                <ClientSettings EnablePostBackOnRowClick="true">
                                                </ClientSettings>
                                                <MasterTableView TableLayout="Fixed" DataKeyNames="Idf_Pessoa_Fisica_Idioma">
                                                    <Columns>
                                                        <telerik:GridBoundColumn HeaderText="Idioma" DataField="Des_Idioma" HeaderStyle-CssClass="rgHeader centro" />
                                                        <telerik:GridBoundColumn HeaderText="Nível" DataField="Des_Nivel_Idioma" HeaderStyle-CssClass="rgHeader centro" />
                                                        <telerik:GridTemplateColumn HeaderText="Ações" HeaderStyle-CssClass="rgHeader centro">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btiExcluir" runat="server" AlternateText="Excluir" CausesValidation="False"
                                                                    CommandName="Delete" ImageUrl="~/img/icone_excluir_16x16.png" />
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="rgHeader centro coluna_icones" />
                                                            <ItemStyle CssClass="espaco_icones centro" />
                                                        </telerik:GridTemplateColumn>
                                                    </Columns>
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </div>
                                        <!--FIM: GridView Idiomas consulta-->
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </tbody>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlDadosExperienciaProfissional" runat="server">
            <div class="titulo">
                Experiência Profissional
            </div>
            <asp:UpdatePanel ID="upExperienciaProfissional" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="divExperienciaProfissionalLabel" runat="server" class="container_label">
                        <asp:Repeater ID="rptExperienciaProfissional" OnItemCreated="rptExperienciaProfissional_ItemCreated"
                            runat="server" OnItemCommand="rptExperienciaProfissional_ItemCommand" OnItemDataBound="rptExperienciaProfissional_ItemDataBound">
                            <ItemTemplate>
                                <asp:LinkButton ID="litNomeEmpresaValor" runat="server" CommandName="Editar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Idf_Experiencia_Profissional")%>'
                                    Visible='<%# Eval("Idf_Experiencia_Profissional") == DBNull.Value %>' Text='<%# Eval("Raz_Social") %>'
                                    CssClass="label_nao_informado" />
                                <%--CssClass='<%$ Resources: Configuracao, CssLabelNaoInformado %>'--%>
                                <h3>
                                    <asp:LinkButton ID="lnkAdmissaoDemissaoValor" runat="server" CommandName="Editar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Idf_Experiencia_Profissional")%>' />
                                    <%--<asp:LinkButton ID="litAdmissaoDemissaoValor" runat="server" CommandName="Editar"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Idf_Experiencia_Profissional")%>'
                                        Visible='<%# Eval("Idf_Experiencia_Profissional") != DBNull.Value %>' Text='<%# Eval("Raz_Social") + " - de " + Eval("Dta_Admissao") + " até " + Eval("Dta_Demissao") + " (" + string.Format("{0} ano(s)", new DateTime(Convert.ToDateTime(Eval("Dta_Demissao")).Subtract(Convert.ToDateTime(Eval("Dta_Admissao"))).Ticks).Year -1) + ")" %>' />--%>
                                </h3>
                                <asp:Panel ID="pnlDadosExperienciaProfissional" runat="server" Visible='<%# Eval("Idf_Experiencia_Profissional") != DBNull.Value %>'>
                                    <table cellspacing="0">
                                        <tbody>
                                            <tr>
                                                <th width="220" align="left" valign="top">A Empresa Trabalha com:
                                                </th>
                                                <td valign="top">
                                                    <asp:LinkButton ID="litAtividadeEmpresaValor" runat="server" Text='<%# Eval("Des_Area_BNE") %>'
                                                        CommandName="Editar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Idf_Experiencia_Profissional")%>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <th width="220" align="left" valign="top">Função Exercida:
                                                </th>
                                                <td valign="top">
                                                    <asp:LinkButton ID="litFuncaoExercidaValor" runat="server" Text='<%# Eval("Des_Funcao") %>'
                                                        CommandName="Editar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Idf_Experiencia_Profissional")%>' />
                                                </td>
                                            </tr>
                                            <tr id="trLinhaUltimoSalario" runat="server">
                                                <th width="220" align="left" valign="top">
                                                    <asp:LinkButton ID="Literal1" runat="server" Visible='<%#  !String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Vlr_Salario").ToString()) ? true : false %>'
                                                        Text="Último Salário:" CommandName="Editar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Idf_Experiencia_Profissional")%>' />
                                                </th>
                                                <td valign="top">
                                                    <asp:LinkButton ID="litUltimoSalarioValor" runat="server" Text='<%# !String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Vlr_Salario").ToString()) ? "R$ " + Convert.ToDecimal(Eval("Vlr_Salario")).ToString("N2") : " " %>'
                                                        CommandName="Editar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Idf_Experiencia_Profissional")%>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <th width="220" align="left" valign="top">Atribuições:
                                                </th>
                                                <td valign="top">
                                                    <asp:LinkButton ID="litAtividadesExercidasValor" runat="server" CommandName="Editar"
                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Idf_Experiencia_Profissional")%>'
                                                        Text='<%# Eval("Des_Atividade").ToString().ReplaceBreaks() %>' />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </asp:Panel>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <div id="divExperienciaProfissionalTextBox" runat="server" style="display: none"
                        class="container_textbox">
                        <asp:Panel ID="pnlExperienciaProfissional" runat="server" CssClass="painel_padrao">
                            <div class="painel_padrao_topo">
                                &nbsp;
                            </div>
                            <%-- Linha Empresa --%>
                            <div class="linha">
                                <asp:Label ID="lblEmpresa1" CssClass="label_principal" runat="server" Text="Nome da Empresa"
                                    AssociatedControlID="txtEmpresa" />
                                <div class="container_campo">
                                    <div>
                                        <asp:RequiredFieldValidator ID="rfvEmpresa" runat="server" ControlToValidate="txtEmpresa"
                                            ValidationGroup="CadastroExperiencia" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                    <asp:TextBox ID="txtEmpresa" runat="server" CssClass="textbox_padrao" Columns="60"
                                        MaxLength="60"></asp:TextBox>
                                </div>
                            </div>
                            <%-- FIM: Linha Empresa --%>
                            <%-- Linha Atividade Empresa --%>
                            <div class="linha">
                                <asp:Label ID="lblAtividadeEmpresa" CssClass="label_principal" runat="server" Text="A Empresa Trabalha com"
                                    AssociatedControlID="ddlAtividadeEmpresa" />
                                <div class="container_campo">
                                    <div>
                                        <asp:CustomValidator ID="cvAtividadeEmpresa" ValidationGroup="CadastroExperiencia"
                                            runat="server" ControlToValidate="ddlAtividadeEmpresa" ErrorMessage="Campo Obrigatório"
                                            ClientValidationFunction="cvAtividadeExercida_Validate">
                                        </asp:CustomValidator>
                                    </div>
                                    <asp:DropDownList ID="ddlAtividadeEmpresa" CssClass="textbox_padrao" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <%-- FIM: Linha Atividade Empresa --%>
                            <%-- Linha Data Admissão --%>
                            <div class="linha">
                                <asp:Label ID="lblDataAdmissao" CssClass="label_principal" runat="server" Text="Data de Admissão"
                                    AssociatedControlID="txtDataAdmissao" />
                                <div class="container_campo">
                                    <componente:Data ID="txtDataAdmissao" runat="server" MensagemErroIntervalo="Data Inválida"
                                        CssClassTextBox="textbox_padrao" ValidationGroup="CadastroExperiencia" />
                                </div>
                                <%-- Linha Data Demissão --%>
                                <asp:Label ID="lblDataDemissao" runat="server" Text="Data de Demissão" AssociatedControlID="txtDataDemissao" />
                                <div class="container_campo">
                                    <componente:Data ID="txtDataDemissao" runat="server" ValidationGroup="CadastroExperiencia"
                                        MensagemErroIntervalo="Data Inválida" Obrigatorio="false" CssClassTextBox="textbox_padrao" />
                                    <div runat="server" id="divDtaDemissaoAviso" class="tooltips_aviso"><span>A data de demissão não pode ser menor que a data de admissão</span></div>

                                </div>
                                <%-- FIM: Linha Data Demissão --%>
                            </div>
                            <%-- FIM: Linha Data Admissão --%>
                            <%-- Linha Função Exercida --%>
                            <div class="linha">
                                <asp:Label ID="lblFuncaoExercida" CssClass="label_principal" runat="server" Text="Função Exercida"
                                    AssociatedControlID="txtFuncaoExercida" />
                                <div class="container_campo">
                                    <div>
                                        <%--<asp:CustomValidator
                                                ID="cvFuncaoExercida1"
                                                runat="server"
                                                ControlToValidate="txtFuncaoExercida1"
                                                ErrorMessage="Função Inválida."
                                                ClientValidationFunction="cvFuncaoExercida1_Validate"
                                                ValidationGroup="CadastroDadosPessoais"></asp:CustomValidator>--%>
                                        <asp:RequiredFieldValidator ID="rfvFuncaoExercida" runat="server" ControlToValidate="txtFuncaoExercida"
                                            ValidationGroup="CadastroExperiencia"></asp:RequiredFieldValidator>
                                    </div>
                                    <asp:TextBox ID="txtFuncaoExercida" runat="server" CssClass="textbox_padrao" Columns="60"
                                        MaxLength="60" AutoPostBack="true" OnTextChanged="txtFuncaoExercida_TextChanged"></asp:TextBox>
                                    <AjaxToolkit:AutoCompleteExtender ID="aceFuncaoExercida" runat="server" TargetControlID="txtFuncaoExercida"
                                        UseContextKey="True" ServiceMethod="ListarFuncoes" OnClientItemSelected="">
                                    </AjaxToolkit:AutoCompleteExtender>
                                </div>
                            </div>
                            <%-- FIM: Linha Função Exercida --%>
                            <%-- Linha Atividades Exercidas --%>
                            <div class="linha">
                                <asp:Label ID="lblAtividadeExercida1" CssClass="label_principal" runat="server" AssociatedControlID="txtAtividadeExercida">Atribuições<br />(Escreva no mínimo 3 tarefas)</asp:Label>
                                <div class="container_campo">
                                    <componente:AlfaNumerico ID="txtAtividadeExercida" runat="server" MaxLength="2000"
                                        OnBlurClient="txtAtividadeExercida_OnBlur" Rows="1000" ValidationGroup="CadastroExperiencia"
                                        TextMode="Multiline" CssClassTextBox="textbox_padrao multiline atividades_exercidas"
                                        Obrigatorio="false" MensagemErroObrigatorio="Campo obrigatório" OnKeyUpClient="txtAtividadeExercida_KeyUp" />
                                </div>
                                <asp:Panel runat="server" ID="pnlBoxSugestaoTarefas" CssClass="BoxSugestaoTarefas">
                                    <span id="GraficoQualidade">
                                        <asp:Literal runat="server" ID="ltGraficoQualidade"></asp:Literal></span>
                                    <div class="seta_apontador_esq">
                                    </div>
                                    <div class="box_conteudo sugestao ">
                                        <asp:Label ID="lblSugestaoTarefas" runat="server" CssClass="label_principal" AssociatedControlID="txtSugestaoTarefas"
                                            Text="Sugestão de Tarefas"></asp:Label>
                                        <div class="container_campo">
                                            <asp:TextBox ID="txtSugestaoTarefas" CssClass="textbox_padrao sugestao_tarefas" TextMode="MultiLine"
                                                ReadOnly="true" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                            <%-- Linha Ultimo Salario --%>
                            <div class="linha">
                                <asp:Label ID="Label8" runat="server" Text="Último Salário" CssClass="label_principal"
                                    AssociatedControlID="txtUltimoSalario" />
                                <div class="container_campo">
                                    <componente:ValorDecimal ID="txtUltimoSalario" runat="server" CasasDecimais="0" ValidationGroup="CadastroExperiencia"
                                        ValorMinimo="1" Obrigatorio="false" CssClassTextBox="textbox_padrao" MensagemErroObrigatorio="Campo obrigatório" />
                                    <label class="decimais">
                                        ,00</label>
                                </div>
                            </div>
                            <%-- FIM: Linha Ultimo Salario --%>
                            <%-- FIM: Atividades Exercidas --%>
                            <!-- Painel botoes -->
                            <asp:Panel ID="Panel3" runat="server" CssClass="painel_botoes">
                                <asp:Button ID="btnSalvarExperiencia" runat="server" Text="Salvar Experiencia" CausesValidation="True"
                                    CssClass="botao_padrao" ValidationGroup="CadastroExperiencia" OnClick="btnSalvarExperiencia_Click" />
                            </asp:Panel>
                            <!-- FIM: Painel botoes -->
                        </asp:Panel>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
        <asp:Panel ID="pnlObservacoes" runat="server">
            <div class="titulo">
                Observações
            </div>
            <table cellspacing="0">
                <tbody>
                    <tr id="trLinhaObservacoes" runat="server">
                        <th width="220" align="left" valign="top">Observações:
                        </th>
                        <td valign="top">
                            <div id="divObservacoesLabel" runat="server" class="container_label">
                                <asp:Label ID="lblObservacoes" runat="server" />
                            </div>
                            <div id="divObservacoesTextBox" runat="server" style="display: none" class="container_textbox">
                                <componente:AlfaNumerico ID="txtObservacoes" runat="server" Obrigatorio="False" CssClassTextBox="textbox_padrao multiline atividades_exercidas"
                                    TextMode="MultiLine" MaxLength="500" Rows="500" />
                            </div>
                        </td>
                    </tr>
                    <tr id="trLinhaOutrosConhecimentos" runat="server">
                        <th width="220" align="left" valign="top">Outros Conhecimentos:
                        </th>
                        <td valign="top">
                            <div id="divOutrosConhecimentosLabel" runat="server" class="container_label">
                                <asp:Label ID="lblOutrosConhecimentos" runat="server" />
                            </div>
                            <div id="divOutrosConhecimentosTextBox" runat="server" style="display: none" class="container_textbox">
                                <componente:AlfaNumerico ID="txtOutrosConhecimentos" runat="server" Obrigatorio="False"
                                    CssClassTextBox="textbox_padrao multiline atividades_exercidas" TextMode="MultiLine"
                                    MaxLength="500" Rows="500" />
                            </div>
                        </td>
                    </tr>
                    <tr id="trLinhaCaracteristicasPessoais" runat="server">
                        <th width="220" align="left" valign="top">Características Pessoais:
                        </th>
                        <td valign="top">
                            <div id="divCaracteristicasPessoaisLabel" runat="server" class="container_label">
                                <asp:Label ID="lblCaracteristicasPessoaisRaca" runat="server" />
                                <asp:Label ID="lblCaracteristicasPessoaisAltura" runat="server" />
                                <asp:Label ID="lblCaracteristicasPessoaisPeso" runat="server" />
                            </div>
                            <div id="divCaracteristicasPessoaisTextBox" runat="server" style="display: none; float: left;"
                                class="container_textbox">
                                <label>
                                    Raça:</label>
                                <asp:DropDownList ID="ddlRaca" CssClass="textbox_padrao" runat="server" Enabled="False">
                                </asp:DropDownList>
                                <br />
                                <label>
                                    Altura</label>
                                <componente:ValorDecimal ID="txtAltura" runat="server" CssClassTextBox="textbox_padrao"
                                    Obrigatorio="False" ValorMaximo="2.50" ValidationGroup="CadastroRevisao" CasasDecimais="2"
                                    Enabled="False" />
                                <br />
                                <label>
                                    Peso</label>
                                <componente:ValorDecimal ID="txtPeso" runat="server" Obrigatorio="false" CssClassTextBox="textbox_padrao"
                                    ValorMaximo="500" ValidationGroup="CadastroRevisao" CasasDecimais="2" Enabled="False" />
                            </div>
                        </td>
                    </tr>
                    <tr id="trLinhaHorarioDisponivel" runat="server">
                        <th width="220" align="left" valign="top">Disponibilidade para Trabalho:
                        </th>
                        <td valign="top">
                            <div id="divDisponibilidadeLabel" runat="server" class="container_label">
                                <asp:Label ID="lblDisponibilidade" runat="server" />
                            </div>
                            <div id="divDisponibilidadeTextBox" runat="server" style="display: none" class="container_textbox">
                                <asp:CustomValidator runat="server" ID="cvDisponibilidade" ValidationGroup="CadastroRevisao"
                                    ClientValidationFunction="cvDisponibilidade_Validate" ErrorMessage="Selecione uma ou mais disponibilidade de trabalho.">
                                </asp:CustomValidator>
                                <asp:CheckBoxList ID="ckblDisponibilidade" CausesValidation="True" ValidationGroup="CadastroRevisao"
                                    RepeatDirection="Horizontal" runat="server">
                                    <asp:ListItem Text="Manhã" Value="1">
                                    </asp:ListItem>
                                    <asp:ListItem Text="Tarde" Value="2">
                                    </asp:ListItem>
                                    <asp:ListItem Text="Noite" Value="3">
                                    </asp:ListItem>
                                    <asp:ListItem Text="Sábado" Value="4">
                                    </asp:ListItem>
                                    <asp:ListItem Text="Domingo" Value="5">
                                    </asp:ListItem>
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>
                    <tr id="trLinhaDisponibilidadeViagens" runat="server">
                        <th width="220" align="left" valign="top">Disponibilidade para Viagens:
                        </th>
                        <td valign="top">
                            <div id="divDisponibilidadeViagemLabel" runat="server" class="container_label">
                                <asp:Label ID="lblDisponibilidadeViagem" runat="server" />
                            </div>
                            <div id="divDisponibilidadeViagemTextBox" runat="server" style="display: none" class="container_textbox">
                                <asp:CheckBox ID="ckbDisponibilidadeViagem" runat="server" Text="Disponibilidade para Viagem" />
                            </div>
                        </td>
                    </tr>
                    <tr id="trLinhaDisponibilidadeMoradia" runat="server">
                        <th width="220" align="left" valign="top">Disponibilidade para Trabalhar em Outra Cidade:
                        </th>
                        <td valign="top">
                            <asp:UpdatePanel ID="upCidadeDisponivel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div id="divDisponibilidadeMorarEmLabel" runat="server" class="container_label">
                                        <asp:Label ID="lblDisponibilidadeMorarEm" runat="server" />
                                    </div>
                                    <div id="divDisponibilidadeMorarEmTextBox" runat="server" style="display: none" class="container_textbox">
                                        <div class="linha">
                                            <asp:Label ID="lblCidadeDisponivel" runat="server" Text="Cidade" CssClass="label_principal"
                                                AssociatedControlID="txtCidadeDisponivel"></asp:Label>
                                            <div>
                                                <asp:RequiredFieldValidator ID="rfvCidadeDisponivel" runat="server" ControlToValidate="txtCidadeDisponivel"
                                                    ValidationGroup="SalvarCidade"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cvCidadeDisponivel" runat="server" ErrorMessage="Cidade Inválida."
                                                    ClientValidationFunction="cvCidadeDisponivel_Validate" ControlToValidate="txtCidadeDisponivel"
                                                    ValidationGroup="SalvarCidade"></asp:CustomValidator>
                                            </div>
                                            <div class="container_campo">
                                                <asp:TextBox ID="txtCidadeDisponivel" runat="server" CssClass="textbox_padrao" Columns="40">
                                                </asp:TextBox>
                                                <AjaxToolkit:AutoCompleteExtender ID="aceCidadeDisponivel" runat="server" TargetControlID="txtCidadeDisponivel"
                                                    ServiceMethod="ListarCidadesSiglaEstadoPorNomeParcialEstado">
                                                </AjaxToolkit:AutoCompleteExtender>
                                            </div>
                                            <asp:Button ID="btnAdicionarCidade" runat="server" Text="Adicionar Cidade" CausesValidation="True"
                                                CssClass="botao_padrao" ValidationGroup="SalvarCidade" OnClick="btnAdicionarCidade_Click" />
                                        </div>
                                        <div class="linha">
                                            <asp:GridView ID="gvCidade" runat="server" AllowSorting="false" AlternatingRowStyle-CssClass="alt_row"
                                                AllowPaging="false" CssClass="gridview_padrao" AutoGenerateColumns="false" DataKeyNames="Idf_Curriculo_Disponibilidade_Cidade"
                                                OnRowDeleted="gvCidadeRowDeleting">
                                                <Columns>
                                                    <asp:BoundField HeaderText="Cidade" DataField="Nme_Cidade" HeaderStyle-CssClass="rgHeader centro" />
                                                    <asp:TemplateField HeaderText="Ações">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btiExcluir" runat="server" AlternateText="Excluir" ToolTrip="Excluir"
                                                                OnClick="gvCidadeRowDeleting" ausesvalidation="false" CausesValidation="False"
                                                                CommandName="Delete" ImageUrl="~/img/icone_excluir_16x16.png" />
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="rgHeader centro coluna_icones" />
                                                        <ItemStyle CssClass="espaco_icones centro" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr id="trLinhaTipoVeiculo" runat="server">
                        <th width="220" align="left" valign="top">Tipo de Veículo:
                        </th>
                        <td valign="top">
                            <asp:UpdatePanel ID="upTipoVeiculo" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div id="divTipoVeiculoLabel" runat="server" class="container_label">
                                        <asp:Label ID="lblModeloAno" runat="server" />
                                        <asp:Repeater ID="rptModeloAno" runat="server" OnItemCommand="rptModeloAno_ItemCommand">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lblNivelValor" runat="server" Text='<%# String.Format("{0} - {1} - {2}", Eval("Des_Tipo_Veiculo"), Eval("Des_Modelo"), Eval("Ano_Veiculo") ) %>'
                                                    CommandName="Editar" />
                                                <br />
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                    <div id="divTipoVeiculoTextBox" runat="server" class="container_textbox">
                                        <asp:Label ID="lblTipoVeiculo" AssociatedControlID="ddlTipoVeiculo" runat="server"
                                            Text="Tipo de Veículo" />
                                        <div class="container_campo">
                                            <div>
                                                <asp:CustomValidator ID="cvTipoVeiculo" runat="server" ValidationGroup="AdicionarVeiculo"
                                                    ControlToValidate="ddlTipoVeiculo" ErrorMessage="Campo Obrigatório" ClientValidationFunction="cvTipoVeiculo_Validate">
                                                </asp:CustomValidator>
                                            </div>
                                            <asp:DropDownList ID="ddlTipoVeiculo" runat="server" CssClass="textbox_padrao" OnClientSelectedIndexChanged="ddlTipoVeiculo_SelectedIndexChanged"
                                                CausesValidation="false">
                                            </asp:DropDownList>
                                        </div>
                                        <%-- FIM: Linha Tipo Veículo--%>
                                        <%-- Linha Modelo --%>
                                        <asp:Label ID="lblModelo" AssociatedControlID="txtModelo" runat="server" Text="Modelo (ex: Fusca)" />
                                        <div class="container_campo">
                                            <componente:AlfaNumerico ID="txtModelo" runat="server" Columns="30" MaxLength="50"
                                                ValidationGroup="AdicionarVeiculo" Obrigatorio="true" CssClassTextBox="textbox_padrao"
                                                WidthTextBox="80px" />
                                        </div>
                                        <%-- FIM: Linha Modelo --%>
                                        <%--Linha Ano--%>
                                        <asp:Label ID="lblAnoVeiculo" runat="server" AssociatedControlID="txtAnoVeiculo"
                                            Text="Ano"></asp:Label>
                                        <div class="container_campo">
                                            <componente:AlfaNumerico ID="txtAnoVeiculo" runat="server" Columns="4" MaxLength="4"
                                                ContemIntervalo="True" ValorMinimo="1800" ValorMaximo="2500" CssClassTextBox="textbox_padrao"
                                                Obrigatorio="true" Tipo="Numerico" ValidationGroup="AdicionarVeiculo" />
                                        </div>
                                        <asp:Panel ID="pnlBotoesCarros" runat="server" CssClass="painel_botoes">
                                            <asp:Button ID="bntAdicionarVeiculo" runat="server" CssClass="botao_padrao" ValidationGroup="AdicionarVeiculo"
                                                CausesValidation="true" Text="Adicionar Veículo" OnClick="bntAdicionarVeiculo_Click" />
                                        </asp:Panel>
                                        <telerik:RadGrid ID="gvModeloAno" runat="server" AllowSorting="False" OnItemCommand="gvModeloAno_ItemCommand"
                                            AllowPaging="False" AllowCustomPaging="False" ShowHeader="True" AlternatingRowStyle-CssClass="alt_row">
                                            <ClientSettings EnablePostBackOnRowClick="true">
                                            </ClientSettings>
                                            <MasterTableView TableLayout="Fixed" DataKeyNames="Idf_Veiculo">
                                                <Columns>
                                                    <telerik:GridBoundColumn HeaderText="Veículo" DataField="Des_Tipo_Veiculo" HeaderStyle-CssClass="rgHeader centro" />
                                                    <telerik:GridBoundColumn HeaderText="Modelo" DataField="Des_Modelo" HeaderStyle-CssClass="rgHeader centro" />
                                                    <telerik:GridBoundColumn HeaderStyle-CssClass="rgHeader centro" HeaderText="Ano"
                                                        ItemStyle-CssClass="centro" DataField="Ano_Veiculo" />
                                                    <telerik:GridTemplateColumn HeaderText="Ações" HeaderStyle-CssClass="rgHeader centro">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btiExcluir" runat="server" AlternateText="Excluir" CausesValidation="False"
                                                                CommandName="Delete" ImageUrl="~/img/icone_excluir_16x16.png" />
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="rgHeader centro coluna_icones" />
                                                        <ItemStyle CssClass="espaco_icones centro" />
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr id="trTipoDeficiencia" runat="server">
                        <th width="220" align="left" valign="top">Tipo de Deficiência:
                        </th>
                        <td valign="top">
                            <div id="divDeficienciaLabel" runat="server" class="container_label">
                                <asp:Label ID="lblDeficiencia" runat="server" />
                            </div>
                            <div id="divDeficienciaTextBox" runat="server" style="display: none" class="container_textbox">
                                <asp:DropDownList CssClass="textbox_padrao" ID="ddlDeficiencia" runat="server">
                                </asp:DropDownList>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </asp:Panel>
    </asp:Panel>
</div>
<!-- Painel botoes -->
<asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
    <asp:UpdatePanel ID="upPnlBotoes" runat="server">
        <ContentTemplate>
            <asp:Button ID="btnSalvar" runat="server" Text="Confirmar" OnClick="btnSalvar_Click"
                ValidationGroup="CadastroRevisao" CssClass="botao_padrao" />
            <asp:Button ID="btnImprimirCurriculo" runat="server" OnClick="btnImprimirCurriculo_Click"
                CssClass="botao_padrao" CausesValidation="true" ValidationGroup="CadastroRevisao"
                Text="Imprimir" />
            <asp:Button ID="btnEnviarCurriculo" runat="server" CssClass="botao_padrao" Text="Enviar CV"
                CausesValidation="true" ValidationGroup="CadastroRevisao" OnClick="btnEnviarCurriculo_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<!-- FIM: Painel botoes -->
<uc3:EnvioCurriculo ID="ucEnvioCurriculo" runat="server" />
