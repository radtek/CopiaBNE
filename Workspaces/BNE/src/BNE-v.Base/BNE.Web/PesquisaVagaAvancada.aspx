<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master"
    AutoEventWireup="true" CodeBehind="PesquisaVagaAvancada.aspx.cs"
    Inherits="BNE.Web.PesquisaVagaAvancada" %>

<%@ Register Src="~/UserControls/ContratoFuncao.ascx" TagName="TipoContratoFuncao" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/PesquisaVagaAvancada.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicScript runat="server" Src="/js/local/Forms/PesquisaVagaAvancada.js" type="text/javascript" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <h1>
        <asp:Label ID="lblTituloPesquisaAvancada" runat="server" Text="Pesquisa de Vaga" />
    </h1>
    <asp:Panel ID="pnlCamposPesquisa" runat="server" CssClass="painel_padrao pesquisa_avancada" DefaultButton="btnBuscar">
        <div class="painel_padrao_topo">
            &nbsp;
        </div>

        <asp:UpdatePanel ID="upTipoContratoFuncaoArea" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:TipoContratoFuncao ID="ucTipoContratoFuncao" runat="server" ValidationGroup="PesquisaAvancada" OnFuncaoReset="ucTipoContratoFuncao_OnFuncaoReset"
                    Obrigatorio="false" OnFuncaoValida="ucTipoContratoFuncao_OnFuncaoValida" OnFuncaoInvalida="ucTipoContratoFuncao_OnFuncaoInvalida" />
                <div class="linha">
                    <asp:Label ID="lblAreaBNE" CssClass="label_principal" runat="server" Text="Área de Atuação"
                        AssociatedControlID="rcbAreaBNE" />
                    <div class="container_campo">
                        <telerik:RadComboBox ID="rcbAreaBNE" runat="server">
                        </telerik:RadComboBox>
                    </div>
                </div>

                <!-- Linha Palavra-Chave -->
                <div class="linha">
                    <asp:Label ID="lblPalavraChave" runat="server" Text="Palavra-Chave"
                        CssClass="label_principal" AssociatedControlID="txtPalavraChave"></asp:Label>
                    <div class="container_campo">
                        <componente:AlfaNumerico ID="txtPalavraChave" runat="server" CssClassTextBox="textbox_padrao"
                            Obrigatorio="false" Columns="25" ValidationGroup="PesquisaAvancada" />
                    </div>
                    <Componentes:BalaoSaibaMais ID="bsmPalavraChave" runat="server" ToolTipText="Nesse campo você pode informar atribuições e conhecimentos que deseja que a vaga possua. Exemplo: vendas, Excel"
                        Text="Saiba mais" ToolTipTitle="Palavra-Chave:" CssClassLabel="balao_saiba_mais" ShowOnMouseover="True" />
                </div>
                <!-- FIM: Linha Palavra-Chave -->
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnLimpar" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>

        <!--Linha Estado-->
        <div class="linha">
            <asp:Label ID="lblUF" runat="server" Text="Estado" CssClass="label_principal"
                AssociatedControlID="rcbEstado" />
            <div class="container_campo">
                <asp:UpdatePanel ID="upEstado" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <telerik:RadComboBox ID="rcbEstado" CssClass="campoEstado" runat="server"
                            AutoPostBack="true" OnSelectedIndexChanged="rcbEstado_SelectedIndexChanged">
                        </telerik:RadComboBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnLimpar" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <!-- Linha Cidade-->
        <div class="linha">
            <asp:Label runat="server" ID="lblCidade" Text="Cidade" AssociatedControlID="txtCidadePesquisa"
                CssClass="label_principal" />
            <div class="container_campo">
                <asp:UpdatePanel ID="upCidade" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div>
                            <asp:CustomValidator ID="cvCidade" runat="server" ErrorMessage="Cidade Inválida."
                                ClientValidationFunction="cvCidade_Validate" ControlToValidate="txtCidadePesquisa"
                                ValidationGroup="PesquisaAvancada"></asp:CustomValidator>
                        </div>
                        <asp:TextBox ID="txtCidadePesquisa" runat="server" OnTextChanged="txtCidadePesquisa_TextChanged"
                            AutoPostBack="true" CssClass="textbox_padrao" Columns="25"></asp:TextBox>
                        <AjaxToolkit:AutoCompleteExtender ID="aceCidade" runat="server" TargetControlID="txtCidadePesquisa"
                            ServiceMethod="ListarCidadesSiglaEstadoPorNomeParcialEstado">
                        </AjaxToolkit:AutoCompleteExtender>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnLimpar" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <!-- FIM: Linha Cidade -->
        </div>
        <!-- Linha Escolaridade-->
        <div class="linha">
            <asp:Label ID="lblEscolaridade" runat="server" Text="Escolaridade"
                AssociatedControlID="rcbEscolaridade" CssClass="label_principal"></asp:Label>
            <div class="container_campo">
                <asp:UpdatePanel ID="upEscolaridade" runat="server" UpdateMode="Conditional"
                    RenderMode="Inline">
                    <ContentTemplate>
                        <telerik:RadComboBox ID="rcbEscolaridade" CssClass="campoEscolaridade"
                            runat="server">
                        </telerik:RadComboBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnLimpar" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <!-- FIM: Linha Escolaridade -->
        <!-- Linha Salário -->
        <div class="linha">
            <asp:Label ID="lblSalario" AssociatedControlID="txtSalario" runat="server"
                CssClass="label_principal" Text="Salário Mínimo" />
            <div class="container_campo">
                <asp:UpdatePanel ID="upSalario" runat="server" UpdateMode="Conditional"
                    RenderMode="Inline">
                    <ContentTemplate>
                        <componente:ValorDecimal ID="txtSalario" runat="server" CasasDecimais="0"
                            ValorMaximo="100000" Obrigatorio="false" CssClassTextBox="textbox_padrao"
                            ValorAlteradoClient="txtSalarioDe_ValorAlterado" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnLimpar" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <asp:Label ID="lblSalarioAte" AssociatedControlID="txtSalarioAte"
                runat="server" Text="Máximo" />
            <div class="container_campo">
                <asp:UpdatePanel ID="upSalarioAte" runat="server" UpdateMode="Conditional"
                    RenderMode="Inline">
                    <ContentTemplate>
                        <componente:ValorDecimal ID="txtSalarioAte" runat="server" CasasDecimais="0"
                            CssClass="alinhar_container_campo" ValorMaximo="100000" CssClassTextBox="textbox_padrao"
                            Obrigatorio="false" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnLimpar" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <asp:Label ID="lblReais" AssociatedControlID="txtSalarioAte" runat="server"
                Text="reais" />
        </div>
        <!-- FIM: Linha Salário -->
        <!-- Linha Disponibilidade de Trabalho -->
        <div class="linha">
            <asp:Label ID="lblDisponibilidadeTrabalho" runat="server" Text="Disponibilidade de Trabalho"
                CssClass="label_principal" AssociatedControlID="rcbDisponibilidade"></asp:Label>
            <div class="container_campo">
                <asp:UpdatePanel ID="upDisponibilidade" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <Employer:ComboCheckbox ID="rcbDisponibilidade" EmptyMessage="Qualquer"
                            runat="server" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnLimpar" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <!-- FIM: Linha Disponibilidade de Trabalho -->
        <!-- Linha Empresa -->
        <div class="linha">
            <asp:Label ID="lblEmpresa" CssClass="label_principal" runat="server"
                Text="Nome Empresa" AssociatedControlID="txtEmpresa" />
            <div class="container_campo">
                <asp:UpdatePanel ID="upEmpresa" runat="server" UpdateMode="Conditional"
                    RenderMode="Inline">
                    <ContentTemplate>
                        <componente:AlfaNumerico ID="txtEmpresa" runat="server" ValidationGroup="PesquisaAvancada"
                            Obrigatorio="false" CssClassTextBox="textbox_padrao" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnLimpar" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <!-- FIM: Linha Empresa -->
        <!-- Código da Vaga -->
        <div class="linha">
            <asp:Label ID="lblCodigoVaga" AssociatedControlID="txtCodigoVaga"
                runat="server" CssClass="label_principal" Text="Código da Vaga" />
            <div class="container_campo">
                <asp:UpdatePanel ID="upCodigoVaga" runat="server" UpdateMode="Conditional"
                    RenderMode="Inline">
                    <ContentTemplate>
                        <componente:AlfaNumerico ID="txtCodigoVaga" runat="server" Columns="10"
                            MaxLength="9" Obrigatorio="false" CssClassTextBox="textbox_padrao" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnLimpar" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <!-- FIM: Código da Vaga -->
        <!-- Linha Tipo de Deficiencia -->
        <div class="linha">
            <asp:Label ID="lblTipoDeficiencia" runat="server" Text="Tipo de Deficiência"
                CssClass="label_principal" AssociatedControlID="rcbTipoDeficiencia"></asp:Label>
            <div class="container_campo">
                <asp:UpdatePanel ID="upTipoDeficiencia" runat="server" UpdateMode="Conditional"
                    RenderMode="Inline">
                    <ContentTemplate>
                        <telerik:RadComboBox ID="rcbTipoDeficiencia" runat="server">
                        </telerik:RadComboBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnLimpar" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <!-- FIM: Linha Tipo de Deficiencia -->
    </asp:Panel>
    <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
        <asp:Button ID="btnBuscar" CssClass="botao_padrao" runat="server"
            Text="Buscar" OnClick="btnBuscar_Click" ValidationGroup="PesquisaAvancada" />
        <asp:Button ID="btnLimpar" CssClass="botao_padrao" runat="server"
            Text="Limpar" OnClick="btnLimpar_Click" />
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
