<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="CadastroEmpresaUsuario.aspx.cs" Inherits="BNE.Web.CadastroEmpresaUsuario" %>

<%@ Register Src="UserControls/Modais/ModalConfirmacao.ascx" TagName="ModalConfirmacao"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicScript runat="server" Src="/js/local/Forms/CadastroEmpresaUsuario.js" type="text/javascript" />
    <Employer:DynamicScript runat="server" Src="/js/local/Forms/CadastroEmpresa.js" type="text/javascript" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/CadastroEmpresaUsuario.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/ModalConfirmacaoEnvioCurriculo.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <!-- Painel: Dados Básicos do Cadastro -->
    <h1>
        <asp:Label ID="lblTitulo" runat="server" Text="Cadastro de Usuário"></asp:Label></h1>
    <asp:Panel runat="server" ID="pnlCadastroUsuario" CssClass="painel_padrao">
        <div class="painel_padrao_topo">
            &nbsp;</div>
        <p class="texto_marcadores_obrigatorio">
            Os campos marcados com um
            <img alt="*" src="img/icone_obrigatorio.gif" />
            são obrigatórios para o cadastramento do usuário.
        </p>
        <h2>
            <asp:Label ID="lblTituloDadosBasicosCadastro" runat="server" Text="Dados do Usuário"></asp:Label></h2>
        <asp:UpdatePanel ID="upDadosBasicos" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlDadosBasicosCadastro" CssClass="painel_padrao" runat="server">
                    <div class="painel_padrao_topo">
                    </div>
                    <%--Linha CPF--%>
                    <div class="linha">
                        <asp:Label runat="server" ID="lblCPF" CssClass="label_principal" Text="CPF" AssociatedControlID="txtCPF" />
                        <div class="container_campo">
                            <componente:CPF ID="txtCPF" runat="server" Obrigatorio="true" ValidationGroup="SalvarDadosEmpresaUsuario"
                                OnValorAlterado="txtCPF_ValorAlterado" />
                        </div>
                    </div>
                    <%--FIM: Linha CPF--%>
                    <%--Linha Data de Nascimento--%>
                    <div class="linha">
                        <asp:Label runat="server" ID="lblDataDeNascimento" CssClass="label_principal" Text="Data de Nascimento"
                            AssociatedControlID="txtDataNascimento" />
                        <div class="container_campo">
                            <componente:Data ID="txtDataNascimento" runat="server" Obrigatorio="true" ValidationGroup="SalvarDadosEmpresaUsuario" />
                        </div>
                    </div>
                    <%--FIM: Linha Data de Nascimento--%>
                    <%--Linha Nome--%>
                    <div class="linha">
                        <asp:Label runat="server" ID="lblNome" Text="Nome Completo" CssClass="label_principal"
                            AssociatedControlID="txtNome" />
                        <div class="container_campo">
                            <componente:AlfaNumerico ID="txtNome" runat="server" Obrigatorio="true" ValidationGroup="SalvarDadosEmpresaUsuario"
                                ExpressaoValidacao="^[A-Z,a-z,ÁáÂâÃãÉéÊêÍíÓóÔôÕõÚúÛûÇç']{1,}( ){1,}[A-Z,a-z,ÁáÂâÃãÉéÊêÍíÓóÔôÕõÚúÛûÇç']{2,}(( ){1,}[A-Z,a-z,ÁáÂâÃãÉéÊêÍíÓóÔôÕõÚúÛûÇç']{1,})*$"
                                MensagemErroFormato='<%$ Resources: MensagemAviso, _100004 %>' />
                        </div>
                    </div>
                    <%--FIM: Linha Nome--%>
                    <%--Linha Sexo--%>
                    <div class="linha">
                        <asp:Label runat="server" ID="lblSexo" Text="Sexo" CssClass="label_principal" AssociatedControlID="rblSexo" />
                        <div class="container_campo">
                            <div>
                                <asp:RequiredFieldValidator ID="rfvSexo" runat="server" ControlToValidate="rblSexo"
                                    ValidationGroup="SalvarDadosEmpresaUsuario"></asp:RequiredFieldValidator>
                            </div>
                            <asp:RadioButtonList ID="rblSexo" runat="server" SkinID="Horizontal">
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
                                    ValidationGroup="SalvarDadosEmpresaUsuario"></asp:RequiredFieldValidator>
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
                            AssociatedControlID="txtTelefoneCelular" />
                        <div class="container_campo">
                            <componente:Telefone ID="txtTelefoneCelular" runat="server" Obrigatorio="true" ValidationGroup="SalvarDadosEmpresaUsuario"
                                Tipo="Celular" MensagemErroFormatoFone='<%$ Resources: MensagemAviso, _100006 %>' />
                        </div>
                    </div>
                    <%--FIM: Linha Celular--%>
                    <%--Linha Telefone--%>
                    <div class="linha">
                        <asp:Label runat="server" ID="lblTelefone" CssClass="label_principal" Text="Telefone Comercial"
                            AssociatedControlID="txtTelefone" />
                        <div class="container_campo">
                            <componente:Telefone ID="txtTelefone" runat="server" Obrigatorio="true" ValidationGroup="SalvarDadosEmpresaUsuario" />
                            <asp:Label runat="server" ID="lblRamal" CssClass="label_principal" Text="Ramal" AssociatedControlID="txtRamal" />
                        </div>
                        <div class="container_campo">
                            <componente:AlfaNumerico ID="txtRamal" runat="server" MaxLength="4" Columns="4" ValidationGroup="SalvarDadosEmpresaUsuario"
                                Obrigatorio="False" Tipo="Numerico" CssClassTextBox="textbox_padrao" />
                        </div>
                    </div>
                    <%--FIM: Ramal--%>
                    <%-- Linha E-Mail --%>
                    <div class="linha">
                        <asp:Label ID="lblEmail" AssociatedControlID="txtEmail" CssClass="label_principal"
                            runat="server" Text="E-mail Comercial" />
                        <div class="container_campo">
                            <div>
                                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                    ValidationGroup="SalvarDadosEmpresaUsuario" ErrorMessage="Email Inválido.">
                                </asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                    ValidationGroup="SalvarDadosEmpresaUsuario"></asp:RequiredFieldValidator>
                            </div>
                            <asp:TextBox ID="txtEmail" runat="server" Columns="50" MaxLength="50"></asp:TextBox>
                        </div>
                    </div>
                    <%-- FIM: Linha E-Mail --%>
                    <%-- Tipos de Acesso --%>
                    <asp:Panel ID="pnlTiposAcesso" runat="server" GroupingText="Tipos de Acesso">
                        <div class="linha">
                            <div class="container_campo" id="divMaster">
                                <asp:CheckBox ID="ckbMaster" runat="server" ValidationGroup="SalvarDadosEmpresaUsuario"
                                    Text="Este usuário poderá realizar alterações nos Dados da Empresa e Cadastro de Usuários" />
                            </div>
                        </div>
                    </asp:Panel>
                    <%-- FIM: Tipos de Acesso --%>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <!-- FIM Painel: Dados Básicos do Cadastro -->
    <!-- Painel Botões Cadastro -->
    <asp:UpdatePanel ID="upBotoesCadastro" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" CssClass="painel_botoes" ID="pnlBotoes">
                <asp:Button ID="btnSalvar" runat="server" CssClass="botao_padrao" OnClick="btnSalvar_Click"
                    CausesValidation="true" Text="Salvar" ValidationGroup="SalvarDadosEmpresaUsuario" />
                <asp:Button ID="btnCancelar" runat="server" CssClass="botao_padrao" CausesValidation="false"
                    Visible="false" Text="Cancelar" OnClick="btnCancelar_Click" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- FIM: Painel Botões Cadastro -->
    <asp:Panel runat="server" ID="pnlUsuarios" CssClass="painel_padrao">
        <div class="painel_padrao_topo">
            &nbsp;</div>
        <h2>
            <asp:Label ID="lblUsuariosEmpresa" runat="server" Text="Usuários da Empresa"></asp:Label></h2>
        <br />
        <!--GridView Cursos consulta-->
        <asp:Label ID="lblMensagemGridUsuarios" runat="server"></asp:Label>
        <div>
            <br />
        </div>
        <asp:UpdatePanel ID="upGvUsuarios" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <telerik:RadGrid ID="gvUsuarios" runat="server" AllowPaging="True" AllowCustomPaging="true"
                    CssClass="gridview_padrao" OnPageIndexChanged="gvUsuarios_PageIndexChanged" OnItemCommand="gvUsuarios_ItemCommand">
                    <ClientSettings EnablePostBackOnRowClick="true" AllowExpandCollapse="True">
                    </ClientSettings>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat=" {4} Usuários {2} a {3} de {5}"
                        Position="TopAndBottom" />
                    <AlternatingItemStyle CssClass="alt_row" />
                    <MasterTableView OverrideDataSourceControlSorting="true" DataKeyNames="Idf_Usuario_Filial_Perfil">
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Responsável" ItemStyle-CssClass="centro">
                                <ItemTemplate>
                                    <asp:Image ID="imgResponsavel" ImageUrl="~/img/img_circulo_azul_responsavel.png"
                                        runat="server" Visible='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "PerfilMaster")) %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Nome do Usuário" ItemStyle-CssClass="centro">
                                <ItemTemplate>
                                    <asp:Label ID="lblNomeUsuarioGrid" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Nme_Pessoa") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn ItemStyle-CssClass="centro" HeaderText="Função Exercida">
                                <ItemTemplate>
                                    <asp:Label ID="lblFuncaoExercidaGrid" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Des_Funcao") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderStyle-CssClass="rgHeader centro CPF" ItemStyle-CssClass="centro"
                                HeaderText="CPF">
                                <ItemTemplate>
                                    <asp:Label ID="lblCPFGrid" runat="server" Text='<%# FormatarCPF(Eval("Num_CPF").ToString()) %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Data de Nascimento">
                                <ItemTemplate>
                                    <asp:Label ID="lblDataNascimentoGrid" CssClass="DataNascimentoGrid" runat="server"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "Dta_Nascimento") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderStyle-CssClass="rgHeader centro celular" ItemStyle-CssClass="centro"
                                HeaderText="Celular">
                                <ItemTemplate>
                                    <asp:Label ID="lblCelularGrid" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Num_Celular") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Status" UniqueName="Status">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "Flg_Inativo")) ? "Inativo" : "Ativo" %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Ações" ItemStyle-CssClass="centro" HeaderStyle-CssClass="rgHeader centro">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btiEditar" runat="server" OnClientClick="BarraRolagemModoEdicaoUsuario()"
                                        AlternateText="Editar" CausesValidation="False" CommandName="Atualizar" ToolTip="Editar"><i class="fa fa-pencil"></i></asp:LinkButton>
                                    <asp:LinkButton ID="btiExcluir" runat="server" AlternateText="Excluir" CausesValidation="False"
                                        ToolTip="Inativar" Visible='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "BotaoExcluirVisible")) %>'
                                        CommandName="Deletar" ><i class="fa fa-times"></i></asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle CssClass="rgHeader centro coluna_icones" />
                                <ItemStyle CssClass="espaco_icones centro" />
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSalvar" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <!--FIM: GridView Cursos consulta-->
    </asp:Panel>
    <!-- Painel Botões Usuários -->
    <asp:UpdatePanel ID="upBotoesUsuario" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" CssClass="painel_botoes" ID="pnlBotoesUsuario">
                <asp:Button ID="btnFinalizar" runat="server" CssClass="botao_padrao" Text="Finalizar"
                    CausesValidation="false" OnClick="btnFinalizar_Click" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
    <!-- Painel: Confirmacao Cadastro -->
    <uc1:ModalConfirmacao ID="ucModalConfirmacao" runat="server" />
    <!-- FIM Painel: Confirmacao -->
    <!-- Painel: Confirmação Exclusao -->
    <asp:UpdatePanel ID="upModalConfirmacaoExclusao" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlModalConfirmacaoExclusao" runat="server" CssClass="modal_confirmacao_acao"
                Style="display: none">
                <asp:UpdatePanel ID="upBtiFechar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton CssClass="botao_fechar_modal" ID="btiFecharteste"
                         runat="server" CausesValidation="false" OnClick="btiFechar_Click" ></asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!-- Painel Modal Confirmacao Exclusao -->
                <div>
                    <h2 class="titulo_modal">
                        <span>Confirmação<%-- <asp:Label ID="lblNomePessoaExclusao" runat="server"></asp:Label> --%></span></h2>
                </div>
                <asp:Panel ID="pnlModalConfirmacaoExclusaoInformacao" runat="server" CssClass="texto_confirmacao">
                    <asp:Label ID="lblConfirmacaoExclusao" runat="server" Text="Tem certeza que deseja desativar o usuário?"></asp:Label>
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
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- FIM Painel: Confirmacao Exclusao -->
</asp:Content>
