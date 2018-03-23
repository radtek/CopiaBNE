<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VisualizacaoCurriculo.ascx.cs"
    Inherits="BNE.Web.UserControls.VisualizacaoCurriculo" %>
<%@ Import Namespace="BNE.BLL.Common" %>

<%@ Register Src="~/UserControls/Modais/ucModalLogin.ascx" TagName="ucModalLogin"
    TagPrefix="uc7" %>

<%@ Register Src="~/UserControls/Modais/EmpresaAguardandoPublicacao.ascx" TagName="ucEmpresaAuditoria"
    TagPrefix="uc8" %>
<%@ Register Src="~/UserControls/Modais/ModalVendaChupaVIP.ascx" TagName="ModalVendaChupaVIP"
    TagPrefix="uc6" %>
<%@ Register Src="~/UserControls/Modais/ModalVendaCIA.ascx" TagName="ModalVendaCIA"
    TagPrefix="uc9" %>
<%@ Register Src="~/UserControls/Modais/QueroContratarWebEstagios.ascx" TagName="QueroContratarEstagiario" TagPrefix="uc10" %>
<%@ Register Src="~/UserControls/Modais/EmpresaBloqueada.ascx" TagPrefix="uc1" TagName="EmpresaBloqueada" %>
<%@ Register Src="~/UserControls/UCLogoFilial.ascx" TagName="UCLogoFilial" TagPrefix="uc1" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/VisualizacaoCurriculo.css" type="text/css" rel="stylesheet" />
<Employer:DynamicScript runat="server" Src="/js/local/VisualizacaoCurriculo.js" type="text/javascript" />
<!--[if IE]>
	<style>
       		 @media print {body{zoom:1.46!important;}}
   	 </style>
<![endif]-->

<div style="width: 100%;">
    <div style="width: 700px; margin: 0 auto; margin-left: 200px;">
        <div style="width: 600px; margin-left: 0px; position: relative">
            <div style="display: block; font-weight: bold; font-size: 24px; margin-left: 20px; position: relative; width: 350px; float: left;">
                <asp:UpdatePanel ID="upNomeCandidato" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <div id="divNomeCandidato" runat="server" class="nome_curriculo">
                            <asp:Literal ID="litNomeValor" runat="server" />
                            <asp:Panel runat="server" ID="pnlPCD" CssClass="ico_pcd" Visible="False">
                                <i class="fa fa-wheelchair"></i>
                            </asp:Panel>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div style="display: block; font-family: Arial Unicode MS; font-size: 12px; margin-left: 20px; position: relative; float: left;">
                Código CV:<asp:Label ID="lblCodigoCurriculo" class="cod_curriculo" runat="server" />
                | Último Acesso em:
                <asp:Label CssClass="texto_data_atualizado" ID="lblAtualizacaoCV"
                    runat="server" />
                <asp:LinkButton runat="server" ID="btiDownload" CssClass="espacamento completo btn-defaut btiDownloadCls" CausesValidation="false" Visible="false" OnClick="btiDownload_Click">
     <i class="fa fa-paperclip" aria-hidden="true"></i> Ver Anexo</asp:LinkButton>
            </div>
            <div class="resumo" style="display: block; font-weight: bold; font-size: 15px; margin-left: 20px; position: relative; width: 300px; float: left; clear: both;">
                CV Resumo
            </div>
            <div class="resumo" id="container_resumo_info" style="display: block; font-size: 12px; margin-left: 20px; position: relative; float: left;">
                <asp:Literal runat="server" ID="litCVResumo"></asp:Literal>
            </div>
        </div>
        <div style="display: block; font-size: 12px; margin-left: 560px; position: relative; width: 100px; margin-top: 10px;">
            <asp:UpdatePanel ID="upLogoCliente" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <uc1:UCLogoFilial ID="UCLogoFilial" runat="server" CssClass="img_logo_filial" ImageUrl="/img/cv_email/logo_bne_cv_tr.png" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <asp:UpdatePanel ID="upBotaoVerDados" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel runat="server" ID="pnlVerDados" CssClass="btn_ver_curriculo">
                    <asp:Button CausesValidation="false" CssClass="ver_curriculo_completo" ID="btnVerDados" OnClick="btnVerDados_Click"
                        runat="server" Text="Ver Currículo Completo" ToolTip="Ver Currículo Completo" />
                    <asp:HyperLink ID="hlVerDados" runat="server" Visible="false" Target="_blank" ToolTip="Ver Dados Pessoais"
                        Text="Ver Dados Pessoais"> 
                    </asp:HyperLink>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="upQueroContratar" runat="server" UpdateMode="Conditional" Visible="False">
            <ContentTemplate>
                <asp:Panel runat="server" ID="pnlQueroContratarWebEstagios" CssClass="btn_ver_curriculo">
                    <asp:HiddenField runat="server" ID="hfEstagiario" Value="false" />
                    <asp:Button runat="server" Text="Quero Contratar para Estágio!" CausesValidation="false" CssClass="botao_padrao" ID="btnQueroContratarWebEstagios" ToolTip="Quero Contratar este Estagiário" OnClick="btnQueroContratarWebEstagios_OnClick" />
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="upVerDados" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlDadosPessoais" runat="server">
                    <div style="border-bottom: 1px solid #999; display: block; font-family: Arial Unicode MS; font-size: 15px; font-weight: bold; margin-left: 20px; width: 655px; float: left;">
                        Dados Pessoais
                    </div>
                    <table style="margin-left: 20px; width: 560px; float: left;">
                        <tr>
                            <td valign="top" style="text-align: left; font-weight: bold; width: 175px;">Sexo:
                            </td>
                            <td style="text-align: left;">
                                <asp:Literal ID="litSexoValor" runat="server" />
                            </td>
                            <%--  <td valign="top" style="text-align: left; font-weight: bold;">CPF:
                            </td>--%>
                            <td valign="top" style="text-align: left;">
                                <asp:Literal ID="litCpfValor" Visible="false" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" style="text-align: left; font-weight: bold; width: 175px;">
                                <asp:Literal ID="litEstadoCivil" runat="server" Text="Estado Civil:" />
                            </td>
                            <td valign="top" style="text-align: left;">
                                <asp:Literal ID="litEstadoCivilValor" runat="server" />
                            </td>
                            <td valign="top" style="text-align: left; font-weight: bold;">
                                <asp:Literal ID="litFilhos" runat="server" Text="Filhos:" />
                            </td>
                            <td valign="top" style="text-align: left;">
                                <asp:Literal ID="litFilhosValor" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" id="litDataNascimento" runat="server" style="text-align: left; font-weight: bold; width: 175px;">Data de Nascimento:
                            </td>
                            <td valign="top" id="tdIdade" runat="server" style="text-align: left;">
                                <asp:Literal ID="litDataNascimentoValor" runat="server" />
                                <asp:Literal ID="litIdadeValor" runat="server" />
                            </td>
                            <td valign="top" style="text-align: left; font-weight: bold; width: 85px;">
                                <asp:Literal ID="litHabilitacao" runat="server" Text="Habilitação:" />
                            </td>
                            <td valign="top" style="text-align: left;">
                                <asp:Literal ID="litHabilitacaoValor" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" style="text-align: left; font-weight: bold; width: 175px;">
                                <asp:Literal ID="litTelefoneCelular" runat="server" Text="Telefone Celular:" />
                            </td>
                            <td valign="top" colspan="2" style="text-align: left;">
                                <div style="display: flex; align-items: center;">
                                    <asp:Literal ID="litTelefoneCelularValor" runat="server" />
                                    <div runat="server" id="divWhats" visible="false" style="display: flex; align-items: center; margin-left: 8px;">
                                        <i class="fa fa-whatsapp fa-2x" aria-hidden="true" style="color: #43d854;"></i>
                                    </div>
                                    <asp:Image runat="server" Style="margin-left: 8px;" ID="imgOperadoraCelular" Visible="False" />

                                </div>
                            </td>

                        </tr>
                        <tr id="trLinhaTelefoneResidencial" runat="server">
                            <td valign="top" style="text-align: left; font-weight: bold; width: 175px;">
                                <asp:Literal ID="litTelefoneResidencial" runat="server" Text="Telefone Fixo Residencial:" />
                            </td>
                            <td valign="top" style="text-align: left;">
                                <asp:Literal ID="litTelefoneResidencialValor" runat="server" />
                            </td>
                            <td valign="top" style="text-align: left; font-weight: bold; width: 50px;"></td>
                            <td valign="top" style="text-align: left;"></td>
                        </tr>
                        <tr id="trLinhaTelefoneRecado" runat="server">
                            <td valign="top" style="text-align: left; font-weight: bold; width: 175px;">
                                <asp:Literal ID="litTelefoneRecado" runat="server" Text="Telefone Fixo Recado:" />
                            </td>
                            <td valign="top" style="text-align: left;">
                                <asp:Literal ID="litTelefoneRecadoValor" runat="server" />
                            </td>
                            <td valign="top" style="text-align: left; font-weight: bold; width: 50px;">
                                <asp:Literal ID="litTelefoneRecadoFalarCom" runat="server" Text="Falar com:" />
                            </td>
                            <td valign="top" style="text-align: left;">
                                <asp:Literal ID="litTelefoneRecadoFalarComValor" runat="server" />
                            </td>
                        </tr>
                        <tr id="trLinhaCelularRecado" runat="server">
                            <td valign="top" style="text-align: left; font-weight: bold; width: 175px;">
                                <asp:Literal ID="litCelularRecado" runat="server" Text="Celular Recado:" />
                            </td>
                            <td valign="top" style="text-align: left;">
                                <asp:Literal ID="litCelularRecadoValor" runat="server" />
                            </td>
                            <td valign="top" style="text-align: left; font-weight: bold; width: 50px;">
                                <asp:Literal ID="litCelularRecadoFalarCom" runat="server" Text="Falar com:" />
                            </td>
                            <td valign="top" style="text-align: left;">
                                <asp:Literal ID="litCelularRecadoFalarComValor" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" style="text-align: left; font-weight: bold; width: 175px;">
                                <asp:Literal ID="litCep" runat="server" Text="CEP:" />
                            </td>
                            <td valign="top" style="text-align: left;">
                                <asp:Literal ID="litCepValor" runat="server" />
                            </td>
                            <td valign="top" style="text-align: left; font-weight: bold; width: 50px;">Cidade:
                            </td>
                            <td valign="top" style="text-align: left;">
                                <asp:Literal ID="litCidadeValor" runat="server" />
                            </td>
                        </tr>
                        <tr id="trLinhaEndereco" runat="server">
                            <td valign="top" style="text-align: left; font-weight: bold; width: 175px;">
                                <asp:Literal ID="litEndereco" runat="server" Text="Endereço:" />
                            </td>
                            <td valign="top" style="text-align: left;" colspan="2">
                                <asp:Literal ID="litEnderecoValor" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <table style="width: 100px; float: left;">
                        <tr>
                            <td valign="top" style="text-align: left;">
                                <asp:UpdatePanel ID="upFotoVisualizacaoCurriculo" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <telerik:RadBinaryImage runat="server" CssClass="btiFoto" ID="rbiThumbnailVisualizacao"
                                            ResizeMode="None" Width="94" Height="120" AutoAdjustImageControlSize="False" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div style="border-bottom: 1px solid #999; display: block; font-family: Arial Unicode MS; font-size: 15px; font-weight: bold; margin-left: 20px; width: 655px; clear: both; margin-bottom: 8px;">
            Pretensões
        </div>
        <asp:Table runat="server" CssClass="pretensoes" Style="margin-left: 20px;" CellPadding="0" CellSpacing="0">
            <%--  //[Obsolete("Optado pela não utilização/disponibilização")]
            <asp:TableRow ID="trFormaDeContrato" runat="server">
                <asp:TableCell CssClass="funcao_pretendida" valign="top" style="text-align: right; font-weight: bold;">
                     <asp:Literal ID="litFormaDeContrato" runat="server" Text="Formas de Contrato:" /></asp:TableCell>
                <asp:TableCell width="0" valign="top" style="text-align: left; width: 200px">
                   <asp:Literal ID="litTipoDeContratoValor" runat="server" /></asp:TableCell>
            </asp:TableRow>--%>
            <asp:TableRow>
                <asp:TableCell CssClass="funcao_pretendida" valign="top" Style="text-align: left; font-weight: bold; width: 175px;">
                    <asp:Literal ID="litFuncaoPretendida" runat="server" Text="Funções Pretendidas:" />
                </asp:TableCell><asp:TableCell Width="0" valign="top" Style="text-align: left; width: 280px">
                    <asp:Literal ID="litFuncaoPretendidaValor" runat="server" />
                </asp:TableCell><asp:TableCell valign="top" Style="text-align: left; font-weight: bold; width: 50px;">
                    <asp:Literal ID="litPretensaoSalarial" runat="server" Text="Salário:" runat="server" />
                </asp:TableCell><asp:TableCell valign="top" Style="text-align: left;">
                    <asp:Literal ID="litPretensaoSalarialValor" runat="server" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Panel ID="pnlDadosEscolaridade" runat="server">
            <div style="border-bottom: 1px solid #999; display: block; font-family: Arial Unicode MS; font-size: 15px; font-weight: bold; margin-left: 20px; width: 655px; margin-bottom: 8px;">
                Escolaridade
            </div>
            <table style="margin-left: 20px; width: 655px;" cellspacing="0" cellpadding="0">
                <tr id="trLinhaEscolaridadeNivel" runat="server">
                    <td style="text-align: left; font-weight: bold; width: 175px; vertical-align: top;">
                        <asp:Literal ID="litEscolaridadeNivel" runat="server" Text="Nível:" />
                    </td>
                    <td valign="top" style="text-align: left;">
                        <asp:Repeater ID="rptNivel" runat="server">
                            <ItemTemplate>
                                <asp:Label ID="lblNivelValor" runat="server" Visible='<%# !String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Des_Bne").ToString()) %>'
                                    Text='<%# Eval("Des_BNE") %>' />
                                <asp:Label ID="lblTituloCursoValor" runat="server" Text='<%# " - " + BNE.BLL.Custom.Helper.AjustarString(TratarTexto(Eval("Des_Curso").ToString()))%>'
                                    Visible='<%# !String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Des_Curso").ToString()) %>'>
                                </asp:Label><asp:Label ID="lblSiglaInstituicaoValor" runat="server" Text='<%# " - " + TratarTexto(Eval("Sig_Fonte").ToString()) %>'
                                    Visible='<%# !String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Sig_Fonte").ToString()) %>' />
                                <asp:Label ID="lblInsituicaoValor" runat="server" Text='<%# " - " +  BNE.BLL.Custom.Helper.AjustarString(TratarTexto(Eval("Nme_Fonte").ToString())) %>'
                                    Visible='<%# !String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Nme_Fonte").ToString()) %>' />
                                <asp:Label ID="lblAnoConclusaoValor" runat="server" Text='<%# " - " + Eval("Ano_Conclusao") %>'
                                    Visible='<%# !String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Ano_Conclusao").ToString()) %>' />
                                <asp:Label ID="lblSituacaoFormacaoValor" runat="server" Text='<%# " - " + Eval("Des_Situacao_Formacao") %>'
                                    Visible='<%# !String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Des_Situacao_Formacao").ToString()) %>' />
                                <asp:Label ID="lblPeriodoValor" runat="server" Text='<%# " - " + Eval("Num_Periodo") + "º Período" %>'
                                    Visible='<%# !String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Num_Periodo").ToString()) %>' />
                                <br />
                            </ItemTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
                <tr id="trLinhaEscolaridadeCursos" runat="server">
                    <td style="text-align: left; font-weight: bold; width: 175px; vertical-align: top;">
                        <asp:Literal ID="litEscolaridadeCursos" runat="server" Text="Outros Cursos:" />
                    </td>
                    <td style="text-align: left;">
                        <asp:Repeater ID="rptCursos" runat="server">
                            <ItemTemplate>
                                <asp:Label ID="lblTituloCursoValor" runat="server" Text='<%# TratarTexto(Eval("Des_Curso").ToString()) %>'
                                    Visible='<%# !String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Des_Curso").ToString()) %>'>
                                </asp:Label><asp:Label ID="lblSiglaInstituicaoValor" runat="server" Text='<%# " - " +  TratarTexto(Eval("Sig_Fonte").ToString()) %>'
                                    Visible='<%# !String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Sig_Fonte").ToString()) %>' />
                                <asp:Label ID="lblInsituicaoValor" runat="server" Text='<%# " - " +  TratarTexto(Eval("Nme_Fonte").ToString()) %>'
                                    Visible='<%# !String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Nme_Fonte").ToString()) %>' />
                                <asp:Label ID="lblAnoConclusaoValor" runat="server" Text='<%# " - " + Eval("Ano_Conclusao") %>'
                                    Visible='<%# !String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Ano_Conclusao").ToString()) %>' />
                                <asp:Label ID="lblSituacaoFormacaoValor" runat="server" Text='<%# " - " + Eval("Des_Situacao_Formacao") %>'
                                    Visible='<%# !String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Des_Situacao_Formacao").ToString()) %>' />
                                <asp:Label ID="lblPeriodoValor" runat="server" Text='<%# " - " + Eval("Num_Periodo") + "º Período" %>'
                                    Visible='<%# !String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Num_Periodo").ToString()) %>' />
                                <br />
                            </ItemTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
                <tr class="linha_idiomas" id="trLinhaIdiomas" runat="server">
                    <td style="text-align: left; font-weight: bold; width: 150px; vertical-align: top;">
                        <asp:Literal ID="litEscolaridadeIdiomas" runat="server" Text="Idiomas:" />
                    </td>
                    <td style="text-align: left;">
                        <asp:Repeater ID="rptIdiomas" runat="server">
                            <ItemTemplate>
                                <asp:Label ID="lblIdioma" runat="server" Text='<%# TratarTexto(Eval("Des_Idioma").ToString()) %>' />
                                <asp:Label ID="lblIdiomaValor" runat="server" Visible='<%# !String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Des_Nivel_Idioma").ToString()) %>'
                                    Text='<%# " - " +  Eval("Des_Nivel_Idioma") %>' />
                                <br />
                            </ItemTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:UpdatePanel ID="upExperienciaProfissional" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlDadosExperienciaProfissional" runat="server">
                    <div style="border-bottom: 1px solid #999; display: block; font-family: Arial Unicode MS; font-size: 15px; font-weight: bold; margin-left: 20px; width: 655px; margin-top: 10px;">
                        Experiência Profissional
                    </div>
                    <table style="margin-left: 20px; width: 655px;">
                        <asp:Repeater ID="rptExperienciaProfissional" OnItemCreated="rptExperienciaProfissional_ItemCreated"
                            runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td colspan="4" valign="top" style="text-align: left; width: 550px; font-style: italic; font-size: 14px; padding-top: 5px; font-weight: bold;">
                                        <asp:Literal runat="server" ID="ltExperiencia" />
                                        <%--<%# Eval("Raz_Social") %>
                                - de
                                <%# Eval("Dta_Admissao") %>
                                até
                                <%# Eval("Dta_Demissao") %><%# " (" + Eval("Des_Tempo") + ")" %>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" style="text-align: left; font-weight: bold; width: 175px">Atividade da Empresa: </td>
                                    <td style="text-align: left;">
                                        <%# TratarTexto(Eval("Des_Area_BNE").ToString()) %>
                                        <asp:Label runat="server" ID="lblTempoExperiencia" Style="font-weight: bold;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" style="text-align: left; font-weight: bold; width: 175px;">Função Exercida: </td>
                                    <td valign="top" style="text-align: left;">
                                        <%# Eval("Des_Funcao") %>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-weight: bold; width: 150px; vertical-align: top;">Atribuições: </td>
                                    <td valign="top" style="text-align: justify; text-justify: inter-word;">
                                        <%# TratarTexto(Eval("Des_Atividade").ToString().ReplaceBreaks()) %>
                                    </td>
                                </tr>
                                <tr id="trLinhaUltimoSalario" runat="server">
                                    <td valign="top" style="text-align: left; font-weight: bold; width: 175px;">
                                        <asp:Literal ID="Literal2" runat="server" Visible='<%# !String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Vlr_Ultimo_Salario").ToString()) ? true : false %>'
                                            Text="Último Salário:"></asp:Literal></td>
                                    <td valign="top" style="text-align: left;">
                                        <%# !String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Vlr_Ultimo_Salario").ToString()) ? "R$ " + Convert.ToDecimal(Eval("Vlr_Ultimo_Salario")).ToString("N2") : " " %>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="upObservacoes" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <div id="teste" style="border-bottom: 1px solid #999; display: block; font-family: Arial Unicode MS; font-size: 15px; font-weight: bold; margin-left: 20px; width: 655px">
                    Observações
                </div>
                <asp:Panel ID="pnlObservacoes" runat="server">
                    <table class="observacoes" style="margin-left: 20px; width: 655px;">
                        <tr id="trLinhaObservacoes" runat="server">
                            <td style="text-align: left; font-weight: bold; width: 175px; vertical-align: top;">Observações: </td>
                            <td valign="top" style="text-align: justify; text-justify: inter-word; width: 470px">
                                <asp:Literal ID="litObservacoes" runat="server" />
                            </td>
                        </tr>
                        <tr id="trLinhaOutrosConhecimentos" runat="server">
                            <td style="text-align: left; font-weight: bold; width: 175px; vertical-align: top;">Outros Conhecimentos: </td>
                            <td valign="top" style="text-align: justify; text-justify: inter-word;">
                                <asp:Literal ID="litOutrosConhecimentosValor" runat="server" />
                            </td>
                        </tr>
                        <tr id="trLinhaCaracteristicasPessoais" runat="server">
                            <td valign="top" style="text-align: left; font-weight: bold; width: 175px">Características Pessoais: </td>
                            <td style="text-align: left;">
                                <asp:Literal ID="litCaracteristicasPessoaisValor" runat="server" />
                            </td>
                        </tr>
                        <tr id="trLinhaHorarioDisponivel" runat="server">
                            <td valign="top" style="text-align: left; font-weight: bold; width: 175px">Disponibilidade para Trabalho: </td>
                            <td style="text-align: left;">
                                <asp:Literal ID="litHorarioDisponivelValor" runat="server" />
                            </td>
                        </tr>
                        <tr id="trLinhaDisponibilidadeViagens" runat="server">
                            <td valign="top" style="text-align: left; font-weight: bold; width: 175px">Disponibilidade para Viagens: </td>
                            <td style="text-align: left;">
                                <asp:Literal ID="litDisponibilidadeViagensValor" runat="server" />
                            </td>
                        </tr>
                        <tr id="trLinhaDisponibilidadeMoradia" runat="server">
                            <td valign="top" style="text-align: left; font-weight: bold; width: 175px">Disponibilidade para Morar em: </td>
                            <td style="text-align: left;">
                                <asp:Literal ID="litDisponibilidadeMoradiaValor" runat="server" />
                            </td>
                        </tr>
                        <tr id="trLinhaTipoVeiculo" runat="server">
                            <td valign="top" style="text-align: left; font-weight: bold; width: 175px">Tipo de Veículo: </td>
                            <td style="text-align: left;">
                                <asp:Literal ID="litTipoVeiculoValor" runat="server" />
                            </td>
                        </tr>
                        <tr id="trTipoDeficiencia" runat="server">
                            <td valign="top" style="text-align: left; font-weight: bold; width: 175px">Tipo de Deficiência: </td>
                            <td style="text-align: left;">
                                <asp:Literal ID="litTipoDeficiencia" runat="server" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlObservacoesApenasLogado" runat="server">
                    <table class="observacoes" style="margin-left: 20px; min-height: 10px;">
                        <tr>
                            <td style="text-align: left; font-weight: bold; width: 175px; vertical-align: top;"></td>
                            <td valign="top" style="text-align: justify; text-justify: inter-word; width: 470px">Realize seu login para visualizar as observações do currículo.
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>

        <br />
        <asp:UpdatePanel ID="upResposta" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <asp:Panel runat="server" ID="pnlRespostas" Visible="false">
                <div id="divPerguntasVaga" style="border-bottom: 1px solid #999; display: block; font-family: Arial Unicode MS; font-size: 15px; font-weight: bold; margin-left: 20px; width: 655px">
                   Perguntas Específicas
                </div>
               
                    <table class="observacoes" style="margin-left: 20px; width: 655px; margin-top: 16px;">
                        <asp:Repeater ID="rpRespostas" runat="server">
                            <ItemTemplate>
                                <tr id="trLinhaObservacoes">
                                    <td style="text-align: left; font-weight: bold; width: 655px; vertical-align: top;">
                                        <asp:Literal runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Pergunta") %>'></asp:Literal> </td>
                                   
                                </tr>
                                <tr>
                                     <td valign="top" style="text-align: justify; text-justify: inter-word; width: 655px">
                                        <asp:Literal  runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Resposta") %>' />
                                    </td>
                                </tr>

                            </ItemTemplate>
                        </asp:Repeater>
                    </table>

                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
<%-- Container do conteúdo imprimível --%>
<%-- FIM: Container do conteúdo imprimível --%>
<uc8:ucEmpresaAuditoria ID="ucEmpresaBloqueadaAguardandoPub" runat="server" />
<uc7:ucModalLogin ID="ucModalLogin" runat="server" />
<uc6:ModalVendaChupaVIP ID="ModalVendaChupaVIP" runat="server" />
<uc9:ModalVendaCIA ID="ucModalVendaCIA" runat="server" />
<uc10:QueroContratarEstagiario runat="server" ID="ucModalQueroContratarEstagiario" />
<uc1:EmpresaBloqueada runat="server" ID="ucEmpresaBloqueada" />
