<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="CadastroEmpresaDados.aspx.cs" Inherits="BNE.Web.CadastroEmpresaDados" %>

<%@ Register Src="~/UserControls/ucEndereco.ascx" TagName="ucEndereco" TagPrefix="uc1" %>
<%@ Register Src="UserControls/Modais/ModalConfirmacao.ascx" TagName="ModalConfirmacao"
    TagPrefix="uc2" %>
<%@ Register Src="UserControls/Modais/ConfirmacaoCadastroEmpresa.ascx" TagName="ConfirmacaoCadastroEmpresa"
    TagPrefix="uc5" %>
<%@ Register Src="UserControls/Forms/SalaAdministrador/DadosRepetidosEmpresa.ascx"
    TagName="DadosRepetidosEmpresa" TagPrefix="uc3" %>
<%@ Register Src="UserControls/ucObservacaoFilial.ascx" TagName="ucObservacaoFilial"
    TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/CadastroEmpresa.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink ID="dhlReceitaFederal" runat="server" href="/css/componentes/receita_federal.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <%-- Painel: Dados Básicos do Cadastro --%>
    <h1>Seja bem vindo! Crie sua conta grátis para acessar os serviços do BNE</h1>
    <h2 class="titulo_painel_padrao">
        <asp:Label ID="lblCNPJEmpresa" runat="server" Text="CNPJ da Empresa" />
    </h2>
    <asp:UpdatePanel ID="upDadosBasicos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlDadosBasicosCadastro" runat="server" CssClass="painel_padrao">
                <div class="painel_padrao_topo">
                    &nbsp;
                </div>
                <%--Linha CNPJ--%>
                <div class="linha">
                    <asp:Label runat="server" ID="lblCNPJ" CssClass="label_principal" Text="CNPJ" AssociatedControlID="txtCNPJ" />
                    <div class="container_campo">
                        <Componentes:ControlCNPJReceitaFederal runat="server" ID="txtCNPJ" ValidarReceitaFederal="False"
                            ExibirValidadorReceitaFederal="True" OnCnpjEncontrado="txtCNPJ_CnpjEncontrado"
                            Obrigatorio="true" ValidationGroup="SalvarDadosEmpresa" OnProblemaComunicacao="txtCNPJ_ProblemaComunicacao" />
                    </div>
                </div>
                <%--FIM: Linha CNPJ --%>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSalvar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>

    <h2>
        <asp:Label ID="lblTituloCartaoCNPJ" runat="server" Text="Dados do Cartão do CNPJ"></asp:Label></h2>
    <asp:UpdatePanel ID="upCartaoCNPJ" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlCartaoCNPJ" CssClass="painel_padrao" runat="server">
                <div class="painel_padrao_topo">
                </div>
                <!--Linha Razao Social-->
                <div class="linha">
                    <asp:Label runat="server" ID="lblRazaoSocial" CssClass="label_principal" Text="Razão Social"
                        AssociatedControlID="txtRazaoSocial" />
                    <div class="container_campo">
                        <componente:AlfaNumerico ID="txtRazaoSocial" runat="server" Columns="60" MaxLength="100"
                            ValidationGroup="SalvarDadosEmpresa" />
                    </div>
                </div>
                <!--FIM: Linha Razao Social-->
                <!--Linha Nome Fantasia -->
                <div class="linha">
                    <asp:Label runat="server" ID="lblNomeFantasia" CssClass="label_principal" Text="Nome Fantasia ou Apelido"
                        AssociatedControlID="txtNomeFantasia" />
                    <div class="container_campo">
                        <componente:AlfaNumerico ID="txtNomeFantasia" runat="server" Columns="60" MaxLength="100"
                            ValidationGroup="SalvarDadosEmpresa" />
                    </div>
                </div>
                <!--FIM: Linha Nome Fantasia-->
                <!--Linha CNAE E Natureza Juridica-->
                <div class="linha">
                    <div class="dados_cnae_pessoa_juridica">
                        <div class="linha">
                            <asp:Label runat="server" ID="lblCNAE" CssClass="label_principal" Text="CNAE" AssociatedControlID="txtCNAE" />
                        </div>
                        <div class="linha">
                            <asp:Label runat="server" ID="lblNaturezaJuridica" CssClass="label_principal" Text="Natureza Jurídica"
                                AssociatedControlID="txtNaturezaJuridica" />
                        </div>
                    </div>
                    <div class="caixa_amarela">
                        <div class="input_CNAE_natureza_empresa">
                            <div class="container_campo CNAE">
                                <componente:AlfaNumerico ID="txtCNAE" runat="server" Columns="7" MaxLength="7" ValidationGroup="SalvarDadosEmpresa"
                                    Tipo="Numerico" ClientValidationFunction="cvCNAE_ServerValidate" OnFocusClient="txtCNAE_OnFocus"
                                    OnBlurClient="txtCNAE_OnBlur" />
                                <asp:Label ID="lblInfoCNAE" runat="server" CssClass="label"></asp:Label>
                            </div>
                            <div class="container_campo natureza_juridica">
                                <componente:AlfaNumerico ID="txtNaturezaJuridica" runat="server" Columns="4" MaxLength="4"
                                    ValidationGroup="SalvarDadosEmpresa" Tipo="Numerico" ClientValidationFunction="cvNaturezaJuridica_ServerValidate"
                                    OnFocusClient="txtNaturezaJuridica_OnFocus" OnBlurClient="txtNaturezaJuridica_OnBlur" />
                                <asp:Label ID="lblInfoNaturezaJuridica" runat="server" CssClass="label"></asp:Label>
                            </div>
                        </div>
                        <!--FIM:  CNAE E Natureza Juridica-->
                        <div id="divDuvidas" class="painel_consulte_cnae">
                            <div style="display: none;">
                                Consulte aqui o CNAE e a Natureza Jurídica
                            </div>
                            <asp:UpdatePanel ID="upReceita" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:HyperLink ID="hlReceita" Target="_blank" runat="server" CssClass="btn btn-defaut btn_consultar"><i class="fa fa-search"></i> Consultar CNAE e <br> Natureza Jurídica</asp:HyperLink>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
                <uc1:ucEndereco ID="ucEndereco" runat="server" />
                <div class="linha autorizacao">
                    <div class="container_campo ">
                        <asp:CheckBox ID="ckbAutorizacao" runat="server" ValidationGroup="SalvarDadosEmpresa" />
                        <asp:Label ID="lblAutorizacao" runat="server" CssClass="label_principal" Text="Autorizo o BNE a divulgar as minhas vagas."
                            AssociatedControlID="ckbAutorizacao" />
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="txtDataNascimento" EventName="ValorAlterado" />
            <asp:AsyncPostBackTrigger ControlID="btnSalvar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>

    

    <h2>
        <asp:Label ID="lblTituloUsuarioMasterEmpresa" runat="server" Text="Responsável pelo Cadastro no BNE"></asp:Label></h2>
    <asp:UpdatePanel ID="upUsuarioMasterEmpresa" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlUsuarioMaster" runat="server" CssClass="painel_padrao">
                <div class="painel_padrao_topo">
                    &nbsp;
                </div>
                <%--Linha CPF--%>
                <div class="linha">
                    <asp:Label runat="server" ID="lblCPF" CssClass="label_principal" Text="CPF" AssociatedControlID="txtCPF" />
                    <div class="container_campo">
                        <componente:CPF ID="txtCPF" runat="server" ValidationGroup="SalvarDadosEmpresa" OnValorAlterado="txtCPF_ValorAlterado" />
                    </div>
                </div>
                <%--FIM: Linha CPF--%>
                <%--Linha Data de Nascimento--%>
                <div class="linha">
                    <asp:Label runat="server" ID="lblDataDeNascimento" CssClass="label_principal" Text="Data de Nascimento"
                        AssociatedControlID="txtDataNascimento" />
                    <div class="container_campo">
                        <componente:Data ID="txtDataNascimento" runat="server" ValidationGroup="SalvarDadosEmpresa"
                            OnValorAlterado="txtDataNascimento_ValorAlterado" />
                    </div>
                </div>
                <%--FIM: Linha Data de Nascimento--%>
                <%--Linha Nome--%>
                <div class="linha">
                    <asp:Label runat="server" ID="lblNome" Text="Nome Completo" CssClass="label_principal"
                        AssociatedControlID="txtNome" />
                    <div class="container_campo">
                        <componente:AlfaNumerico ID="txtNome" runat="server" MensagemErroFormato='<%$ Resources: MensagemAviso, _100004 %>'
                            ValidationGroup="SalvarDadosEmpresa" ClientValidationFunction="ValidarNome" />
                    </div>
                </div>
                <%--FIM: Linha Nome--%>
                <%--Linha Sexo--%>
                <div class="linha">
                    <asp:Label runat="server" ID="lblSexo" Text="Sexo" CssClass="label_principal" AssociatedControlID="rblSexo" />
                    <div class="container_campo">
                        <div>
                            <asp:RequiredFieldValidator ID="rfvRblSexo" runat="server" ControlToValidate="rblSexo"
                                ValidationGroup="SalvarDadosEmpresa"></asp:RequiredFieldValidator>
                        </div>
                        <asp:RadioButtonList ID="rblSexo" runat="server" SkinID="Horizontal" ValidationGroup="SalvarDadosEmpresa">
                        </asp:RadioButtonList>
                    </div>
                </div>
                <%--FIM: Linha Sexo--%>
                <%-- Linha Função Exercida --%>
                <div class="linha">
                    <asp:Label ID="lblFuncaoExercida" runat="server" CssClass="label_principal" Text="Função Exercida"
                        AssociatedControlID="txtFuncaoExercida" />
                    <div class="container_campo">
                        <div>
                            <asp:RequiredFieldValidator ID="rfvFuncaoExercida" runat="server" ControlToValidate="txtFuncaoExercida"
                                ValidationGroup="SalvarDadosEmpresa"></asp:RequiredFieldValidator>
                        </div>
                        <asp:TextBox ID="txtFuncaoExercida" runat="server" CssClass="textbox_padrao campo_obrigatorio"
                            Columns="50" MaxLength="50"></asp:TextBox>
                        <AjaxToolkit:AutoCompleteExtender ID="aceFuncaoExercida" runat="server" TargetControlID="txtFuncaoExercida"
                            ServiceMethod="ListarFuncoes" UseContextKey="True">
                        </AjaxToolkit:AutoCompleteExtender>
                    </div>
                </div>
                <%-- FIM: Linha Função Exercida --%>
                <%--Linha Celular--%>
                <div class="linha">
                    <asp:Label runat="server" ID="lblCelular" CssClass="label_principal" Text="Celular"
                        AssociatedControlID="txtTelefoneCelularMaster" />
                    <div class="container_campo">
                        <componente:Telefone ID="txtTelefoneCelularMaster" runat="server" ValidationGroup="SalvarDadosEmpresa"
                            Tipo="Celular" MensagemErroFormatoFone='<%$ Resources: MensagemAviso, _100006 %>' />
                    </div>
                </div>
                <%--FIM: Linha Celular--%>
                <%--Linha Telefone--%>
                <div class="linha">
                    <asp:Label runat="server" ID="lblTelefoneMaster" CssClass="label_principal" Text="Telefone Comercial"
                        AssociatedControlID="txtTelefoneMaster" />
                    <div class="container_campo">
                        <componente:Telefone ID="txtTelefoneMaster" runat="server" ValidationGroup="SalvarDadosEmpresa" Tipo="FixoCelular" />
                    </div>
                </div>
                <%--FIM: Linha Telefone--%>
                <%-- Linha E-Mail --%>
                <div class="linha">
                    <asp:Label ID="lblEmail" AssociatedControlID="txtEmail" CssClass="label_principal"
                        runat="server" Text="E-mail Comercial" />
                    <div class="container_campo">
                        <div>
                            <asp:RegularExpressionValidator ID="regexEmail"
                                runat="server"
                                ErrorMessage="E-mail Inválido."
                                ControlToValidate="txtEmail"
                                CssClass="validador"
                                ValidationGroup="SalvarDadosEmpresa"
                                ValidationExpression="(\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)">
                            </asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="rfvEmail"
                                runat="server"
                                ControlToValidate="txtEmail"
                                ValidationGroup="SalvarDadosEmpresa" />
                        </div>
                        <asp:TextBox ID="txtEmail" runat="server" ValidationGroup="SalvarDadosEmpresa"
                            CssClass="textbox_padrao campo_obrigatorio" Style="float: left;" />
                    </div>
                </div>
                <%-- FIM: Linha E-Mail --%>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="txtCNPJ" EventName="ValorAlterado" />
            <asp:AsyncPostBackTrigger ControlID="btnSalvar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <h2>
        <asp:Label ID="lblTituloDadosGeraisEmpresa" runat="server" Text="Dados da Empresa"></asp:Label></h2>
    <asp:Panel ID="pnlFotoDadosEmpresaGeral" runat="server" CssClass="painel_padrao conteudo_ajustado">
        <%--Início da div da Foto--%>
        <div class="inserir_logo_empresa">
            <asp:UpdatePanel ID="upFoto" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <Componentes:SelecionarFoto ID="ucFoto" runat="server" InitialSelection="1;1;370;244"
                        PainelFileUploadCssClass="painel_upload_foto" PainelImagemFotoCssClass="painel_imagem_foto_logo"
                        SemFotoImagemUrl="/img/btn_coloque_sua_logo.png" ThumbDir="~/ArquivosTemporarios/" BotaoFecharImageUrl="/img/botao_padrao_fechar.gif"
                        MinAcceptedHeight="100" MinAcceptedWidth="100" ResizeImageHeight="1530" ResizeImageWidth="2048"
                        MaxHeight="1024" MaxWidth="1280" OnError="ucFoto_Error" />
                    <asp:LinkButton runat="server" ID="btlExisteLogoWS" OnClick="btlExisteLogoWS_Click"
                        Visible="False" Text="Já existe uma logo. Clique para recuperar." ToolTip="Já existe uma foto. Clique para recuperar."
                        CssClass="link_alterar_foto" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <%--Final da div da foto--%>
        <asp:UpdatePanel ID="upDadosGeraisEmpresa" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlDadosGeraisEmpresa" runat="server">
                    <div class="painel_padrao_topo">
                    </div>
                    <div class="dados_da_empresa">
                        <%-- Linha Site --%>
                        <div class="linha">
                            <asp:Label ID="lblSite" AssociatedControlID="txtSite" CssClass="label_principal"
                                runat="server" Text="Site" />
                            <div class="container_campo">
                                <div>
                                    <asp:RegularExpressionValidator ID="regexSite"
                                        runat="server"
                                        ErrorMessage="Site Inválido."
                                        ControlToValidate="txtSite"
                                        CssClass="validador"
                                        ValidationGroup="SalvarDadosEmpresa"
                                        ValidationExpression="((http|https|ftp)\://|(www))(((25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])\.){3}(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])|([a-zA-Z0-9_\-\.])+\.(com|net|org|edu|int|mil|gov|arpa|biz|aero|name|coop|info|pro|museum|uk|me|ind))((:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*)">
                                    </asp:RegularExpressionValidator>
                                </div>

                                <asp:TextBox ID="txtSite" runat="server" ValidationGroup="SalvarDadosEmpresa"
                                    CssClass="textbox_padrao" Style="float: left;" />
                            </div>
                        </div>
                        <%-- FIM: Linha Site --%>
                        <%-- Linha Site --%>
                        <div class="linha">
                            <asp:Label ID="lblFacebook" CssClass="label_principal" runat="server" Text="Sua empresa no Facebook" />
                            <asp:Label ID="lblFacebookWWW" AssociatedControlID="txtFacebook" runat="server" Text="http://www.facebook.com/" />
                            <div class="container_campo">
                                <componente:AlfaNumerico ID="txtFacebook" runat="server" ValidationGroup="SalvarDadosEmpresa"
                                    CssClassTextBox="textbox_padrao" Obrigatorio="False" />
                            </div>
                        </div>
                        <%-- FIM: Linha Site --%>
                        <%-- Linha Numero Funcionarios --%>
                        <div class="linha">
                            <asp:Label ID="lblNumeroFuncionarios" AssociatedControlID="txtNumeroFuncionarios"
                                CssClass="label_principal" runat="server" Text="Número de Funcionários" />
                            <div class="container_campo">
                                <componente:AlfaNumerico ID="txtNumeroFuncionarios" runat="server" MaxLength="5"
                                    Columns="5" ContemIntervalo="true" ValorMinimo="1" ValorMaximo="1000000" ValidationGroup="SalvarDadosEmpresa"
                                    Tipo="Numerico" />
                            </div>
                        </div>
                        <%-- FIM: Linha Numero Funcionarios --%>
                        <%--Linha Telefone--%>
                        <div class="linha">
                            <asp:Label runat="server" ID="lblTelefoneComercialEmpresa" CssClass="label_principal"
                                Text="Telefone Comercial" AssociatedControlID="txtTelefoneComercialEmpresa" />
                            <div class="container_campo">
                                <componente:Telefone ID="txtTelefoneComercialEmpresa" runat="server" ValidationGroup="SalvarDadosEmpresa" />
                            </div>
                        </div>
                        <%--FIM: Linha Telefone--%>
                        <%--Linha Empresa Oferece Cursos--%>
                        <div class="linha">
                            <asp:Label runat="server" ID="Label3" CssClass="label_principal" AssociatedControlID="lblMinhaEmpresaCursos" />
                            <div class="container_campo">
                                <asp:CheckBox ID="ckbCursos" runat="server" />
                                <asp:Label runat="server" ID="lblMinhaEmpresaCursos" CssClass="label_principal" Text="Minha empresa oferece cursos"
                                    AssociatedControlID="ckbCursos" />
                            </div>
                        </div>
                        <div class="linha">
                            <asp:Label runat="server" ID="lblEstagioEsquerda" CssClass="label_principal" AssociatedControlID="ckbEstagiarios" />
                            <div class="container_campo">
                                <asp:CheckBox ID="ckbEstagiarios" runat="server" />
                                <asp:Label runat="server" ID="lblEstagioDireita" CssClass="label_principal" Text="Minha empresa contrata estagiários"
                                    AssociatedControlID="ckbEstagiarios" />
                            </div>
                        </div>
                        <%--FIM: Linha Empresa Oferece Cursos--%>
                    </div>
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="txtDataNascimento" EventName="ValorAlterado" />
                <asp:AsyncPostBackTrigger ControlID="btnSalvar" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </asp:Panel>
    
    <asp:UpdatePanel ID="upPnlAdministrador" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlPerfilAdministrador" runat="server">
                <h2>
                    <asp:Label ID="lblTituloDadosFinanceiro" runat="server" Text="Dados Financeiro"></asp:Label>
                </h2>
                    <asp:Panel ID="pnlDadosFinanceiro" runat="server" CssClass="painel_padrao">
                        <asp:UpdatePanel ID="upDadosFinanceiro" runat="server" UpdateMode="Conditional">
                        <ContentTemplate> 
                            <div class="linha">
                                <asp:Label ID="lblIss" Text="ISS" runat="server" CssClass="label_principal" />
                                <asp:CheckBox ID="cbISS" runat="server" />
                            </div>
                            <div class="linha">
                                <asp:Label ID="lblTextoNF" runat="server" Text="Texto Personalizado Nota Fiscal" CssClass="label_principal"></asp:Label><br />
                                <asp:TextBox ID="txtTextoNF" runat="server" Columns="50" Rows="5" TextMode="MultiLine" CssClass="textbox_nota_fiscal"></asp:TextBox>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    </asp:Panel>
                    <br />
                <h2>Administrador</h2>
                <div class="painel_padrao">
                    <div class="painel_padrao_topo">
                        &nbsp;
                    </div>
                    <uc4:ucObservacaoFilial ID="ucObservacaoFilial" runat="server" />
                    <div class="linha">
                        <asp:Label ID="lblSituacao" runat="server" CssClass="label_principal" Text="Situação"
                            AssociatedControlID="rcbEditarEmpresa" />
                        <telerik:RadComboBox ID="rcbEditarEmpresa" runat="server">
                        </telerik:RadComboBox>
                    </div>
                    <div class="linha">
                        <asp:Label ID="lblBancoDados" runat="server" CssClass="label_principal" Text="Banco de dados"
                            AssociatedControlID="rcbBancoDados" />
                        <telerik:RadComboBox ID="rcbBancoDados" runat="server">
                        </telerik:RadComboBox>
                    </div>
                    <div class="linha">
                        <asp:Label ID="lblQuantidadeUsuario" runat="server" CssClass="label_principal" Text="Quantidade de Usuários"
                            AssociatedControlID="txtQuantidadeUsuario" />
                        <componente:AlfaNumerico ID="txtQuantidadeUsuario" runat="server" Tipo="Numerico"
                            Obrigatorio="False" CssClassTextBox="textbox_padrao" />
                    </div>
                    <div class="linha">
                        <asp:Label ID="lblTipoParceiro" runat="server" CssClass="label_principal" Text="Tipo de Parceiro"
                            AssociatedControlID="rcbTipoParceiro" />
                        <telerik:RadComboBox ID="rcbTipoParceiro" runat="server">
                        </telerik:RadComboBox>
                    </div>
                    <div class="linha">
                        <asp:Label ID="lblClienteWebEstagios" runat="server" CssClass="label_principal" Text="Cliente WebEstágios"
                            AssociatedControlID="rcbTipoParceiro" />
                        <asp:CheckBox runat="server" ID="ckbClienteWebEstagios" />
                    </div>
                    <div class="linha">
                        <p>
                            Personalize o e-mail que será enviado automaticamente para o candidato que se candidatou à sua vaga.
                        </p>
                        <telerik:RadEditor CssClass="radeditor_pagina_inicial" Height="180px" ID="reAgradecimentoCandidatura"
                            runat="server" SkinID="RadEditorControlesBasicos" Width="850px">
                        </telerik:RadEditor>
                    </div>
                    <div class="linha">
                        <p>
                            Personalize o texto que será exibido como a descrição de sua empresa.
                        </p>
                        <telerik:RadEditor CssClass="radeditor_pagina_inicial" Height="180px" ID="reCartaApresentacao"
                            runat="server" SkinID="RadEditorControlesBasicos" Width="850px">
                        </telerik:RadEditor>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="txtDataNascimento" EventName="ValorAlterado" />
            <asp:AsyncPostBackTrigger ControlID="btnSalvar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <%-- Painel botoes --%>
    <asp:UpdatePanel ID="upBotoes" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" CssClass="painel_botoes" ID="pnlBotoes">
                <asp:Button ID="btnVisualizarCNPJ" runat="server" CssClass="botao_padrao" CausesValidation="false"
                    Text="Visualizar CNPJ" Visible="false" />
                <asp:Button ID="btnDadosRepetidos" runat="server" CssClass="botao_padrao" OnClick="btnDadosRepetidos_Click"
                    CausesValidation="false" Text="Dados Repetidos" Visible="false" />
                <asp:Button ID="btnSalvar" runat="server" CssClass="botao_padrao" OnClick="btnSalvar_Click"
                    CausesValidation="true" Text="Salvar" ValidationGroup="SalvarDadosEmpresa" />
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="txtDataNascimento" EventName="ValorAlterado" />
        </Triggers>
    </asp:UpdatePanel>
    <%-- FIM: Painel botoes --%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
    <uc2:ModalConfirmacao ID="ucModalConfirmacao" runat="server" />
    <uc5:ConfirmacaoCadastroEmpresa ID="ucConfirmacaoCadastroEmpresa" runat="server" />
    <uc3:DadosRepetidosEmpresa ID="ucDadosRepetidosEmpresa" runat="server" />
    <Employer:DynamicScript runat="server" Src="/js/local/Forms/CadastroEmpresa.js" type="text/javascript" />
</asp:Content>
