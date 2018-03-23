<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormacaoCursos.ascx.cs" Inherits="BNE.Web.UserControls.Forms.CadastroCurriculo.FormacaoCursos" %>
<Employer:DynamicScript runat="server" Src="/js/local/Forms/CadastroCurriculo/FormacaoCursos.js" type="text/javascript" />
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/CadastroCurriculo/FormacaoCursos.css" type="text/css" rel="stylesheet" />
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
                        <asp:LinkButton ID="btlMiniCurriculo" runat="server" OnClick="btlMiniCurriculo_Click" CssClass="texto_abas" ValidationGroup="salvar" Text="Mini Currículo"></asp:LinkButton>
                    </span>
                </div>
                <div class="abas_nova">
                    <span class="aba_esquerda_nao_selecionada aba">
                        <asp:LinkButton ID="btlDadosPessoais" runat="server" OnClick="btlDadosPessoais_Click" ValidationGroup="salvar" Text="Dados Pessoais e Profissionais" CssClass="texto_abas"></asp:LinkButton>
                    </span>
                </div>
                <div class="abas_nova">
                    <span class="aba_esquerda_selecionada aba">
                        <asp:Label ID="lblFormacaoCursos" runat="server" Text="Formação e Cursos" CssClass="texto_abas_selecionada">
                        </asp:Label>
                    </span>
                </div>
                <div class="abas_nova">
                    <span class="aba_esquerda_nao_selecionada aba">
                        <asp:LinkButton ID="btlDadosComplementares" runat="server" OnClick="btlDadosComplementares_Click" ValidationGroup="salvar" Text="Dados Complementares" CssClass="texto_abas"></asp:LinkButton>
                    </span>
                </div>
                <div class="abas_nova">
                    <span class="aba_esquerda_nao_selecionada aba">
                        <asp:LinkButton ID="btlRevisaoDados" runat="server" OnClick="btlRevisaoDados_Click" ValidationGroup="salvar" Text="Conferir" CssClass="texto_abas"></asp:LinkButton>
                    </span>
                </div>
            </div>
            <div class="abas" style="display: none">
                <span class="aba_fundo">
                    <asp:LinkButton ID="btlGestao" runat="server" OnClick="btlGestao_Click" ValidationGroup="salvar" CausesValidation="true" Text="Gestão"></asp:LinkButton>
                </span>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
<%--<asp:UpdatePanel ID="upFormacaoCursos" runat="server" UpdateMode="Conditional">
    <ContentTemplate>--%>
<!-- FIM: Tabs -->
<div class="interno_abas">
    <!-- Painel: Formacao -->
    <h2 class="titulo_painel_padrao" style="display: none">
        <asp:Label ID="lblTituloFormacao" runat="server" Text="Formação" />
    </h2>
    <asp:UpdatePanel ID="upFormacao" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlFormacao" runat="server" CssClass="painel_padrao">
                <div class="painel_padrao_topo">
                </div>
                <p class="texto_marcadores_obrigatorio">
                    Os campos marcados com um
                    <img alt="*" src="img/icone_obrigatorio.gif" />
                    são obrigatórios para o cadastro de seu currículo.
                </p>
                <!-- Linha Nível-->
                <div class="linha">
                    <asp:Label ID="lblNivel" runat="server" Text="Nível de Formação" AssociatedControlID="ddlNivel" CssClass="label_principal"></asp:Label>
                    <div class="container_campo">
                        <asp:UpdatePanel ID="upNivel" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlNivel" runat="server" ValidationGroup="CadastroFormacao" CssClass="textbox_padrao campo_obrigatorio" OnSelectedIndexChanged="ddlNivel_SelectedIndexChanged" AutoPostBack="True">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <!-- FIM: Linha Nível -->
                <!-- Linha Instituicao -->
                <div id="divLinhaInstituicao" class="linha" runat="server">
                    <asp:Label ID="lblInstituicao" runat="server" Text="Instituição de Ensino" CssClass="label_principal" AssociatedControlID="txtInstituicaoComplementar"></asp:Label>
                    <div class="container_campo">
                        <div>
                            <asp:RequiredFieldValidator ID="rfvInstituicao" runat="server" ControlToValidate="txtInstituicao" ValidationGroup="CadastroFormacao"></asp:RequiredFieldValidator>
                        </div>
                        <asp:TextBox ID="txtInstituicao" runat="server" CssClass="textbox_padrao campo_obrigatorio" Columns="80" MaxLength="100" AutoPostBack="True" ontextchanged="txtInstituicao_TextChanged"></asp:TextBox>
                        <AjaxToolkit:AutoCompleteExtender ID="aceInstituicao" runat="server" TargetControlID="txtInstituicao" ServiceMethod="ListarSiglaNomeFonteNivelCurso">
                        </AjaxToolkit:AutoCompleteExtender>
                    </div>
                </div>
                <!-- FIM: Linha Instituicao -->
                <!-- Linha Titulo Curso -->
                <div id="divLinhaTituloCurso" class="linha" runat="server">
                    <asp:Label ID="Label3" runat="server" Text="Nome do Curso" CssClass="label_principal" AssociatedControlID="txtTituloCurso"></asp:Label>
                    <div class="container_campo">
                        <div>
                            <asp:RequiredFieldValidator ID="rfvTituloCurso" runat="server" ControlToValidate="txtTituloCurso" ValidationGroup="CadastroFormacao"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cvTituloCurso" runat="server" ErrorMessage="Curso Inválida." ClientValidationFunction="cvTituloCurso_Validate" ControlToValidate="txtTituloCurso" ValidationGroup="CadastroFormacao"></asp:CustomValidator>
                        </div>
                        <asp:TextBox ID="txtTituloCurso" runat="server" CssClass="textbox_padrao campo_obrigatorio" MaxLength="100"></asp:TextBox>
                        <AjaxToolkit:AutoCompleteExtender ID="aceTituloCurso" runat="server" TargetControlID="txtTituloCurso" ServiceMethod="ListarCursoFonte">
                        </AjaxToolkit:AutoCompleteExtender>
                    </div>
                </div>
                <!-- FIM: Linha Titulo Curso -->
                <!-- Linha Cidade -->
                <div id="divCidade" class="linha" runat="server">
                    <asp:Label runat="server" ID="lblCidade" Text="Cidade" CssClass="label_principal" AssociatedControlID="txtCidade" />
                    <div class="container_campo">
                        <div>
                            <asp:RequiredFieldValidator ID="rfvCidade" runat="server" ControlToValidate="txtCidade" ValidationGroup="CadastroFormacao"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cvCidade" runat="server" ErrorMessage="Cidade Inválida." ClientValidationFunction="cvCidade_Validate" ControlToValidate="txtCidade" ValidationGroup="CadastroFormacao"></asp:CustomValidator>
                        </div>
                        <asp:TextBox ID="txtCidade" runat="server" Columns="50" CssClass="textbox_padrao" MaxLength="50"></asp:TextBox>
                    </div>
                </div>
                <!-- FIM: Linha Cidade -->
                <!-- Linha Situacao -->
                <div id="divLinhaSituacao" class="linha" runat="server">
                    <asp:Label ID="lblSituacao" runat="server" Text="Situação" CssClass="label_principal" AssociatedControlID="ddlSituacao"></asp:Label>
                    <div class="container_campo">
                        <div>
                            <asp:CustomValidator ID="cvSituacao" runat="server" ValidationGroup="CadastroFormacao" ControlToValidate="ddlSituacao" ErrorMessage="Campo Obrigatório" ClientValidationFunction="cvSituacao_Validate">
                            </asp:CustomValidator>
                        </div>
                        <asp:DropDownList ID="ddlSituacao" CssClass="textbox_padrao campo_obrigatorio" runat="server" ValidationGroup="CadastroFormacao">
                        </asp:DropDownList>
                    </div>
                    <asp:Label ID="lblPeriodo" runat="server" Text="Período" AssociatedControlID="txtPeriodo"></asp:Label>
                    <componente:AlfaNumerico ID="txtPeriodo" runat="server" ValidationGroup="CadastroFormacao" ContemIntervalo="true" ValorMinimo="1" ValorMaximo="12" Columns="2" MaxLength="2" Tipo="Numerico" />
                </div>
                <!-- FIM: Linha Situacao -->
                <!-- Linha Ano Conclusao -->
                <div id="divLinhaConclusao" class="linha" runat="server">
                    <asp:Label ID="lblAnoConclusao" runat="server" Text="Ano de Conclusão" CssClass="label_principal" AssociatedControlID="txtAnoConclusao"></asp:Label>
                    <div class="container_campo">
                        <componente:AlfaNumerico CssClassTextBox="textbox_padrao campo_obrigatorio" ID="txtAnoConclusao" runat="server" Tipo="Numerico" MaxLength="4" Obrigatorio="false" Columns="4" ContemIntervalo="true" ValorMinimo="1950" ValorMaximo="2500" ValidationGroup="CadastroFormacao" />
                    </div>
                </div>
                <!-- FIM: Linha Ano Conclusao -->
                <!-- Painel botoes -->
                <%--<asp:UpdatePanel ID="upBotoesFormacao" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                        <asp:Panel ID="pnlBotoesFormacao" runat="server" CssClass="painel_botoes">
                            <asp:Button ID="btnSalvarFormacao" runat="server" Text="Adicionar Curso" CausesValidation="true" CssClass="botao_padrao" ValidationGroup="CadastroFormacao" OnClick="btnSalvarFormacao_Click" />
                        </asp:Panel>
                   <%-- </ContentTemplate>
                </asp:UpdatePanel>--%>
                <!-- FIM: Painel botoes -->
                <!--GridView Cursos consulta-->
                <%--<div>--%>
                    <asp:UpdatePanel ID="upGvFormacao" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="gvFormacao" runat="server" AllowSorting="False" AllowPaging="False" AllowCustomPaging="False" AlternatingRowStyle-CssClass="alt_row" AutoGenerateColumns="false" OnRowDeleting="gvFormacao_RowDeleting" DataKeyNames="Idf_Formacao" OnRowUpdating="gvFormacao_RowUpdating">
                                <Columns>
                                    <asp:BoundField HeaderText="Nível de formação" HeaderStyle-CssClass="rgHeader centro" DataField="Des_BNE" />
                                    <asp:BoundField HeaderText="Nome do Curso" HeaderStyle-CssClass="rgHeader centro" DataField="Des_Curso" />
                                    <asp:BoundField HeaderText="Instituição de Ensino" HeaderStyle-CssClass="rgHeader centro" DataField="Nme_Fonte" />
                                    <asp:BoundField HeaderText="Data de Conclusão" HeaderStyle-CssClass="rgHeader centro" ItemStyle-CssClass="centro" DataField="Ano_Conclusao" />
                                    <asp:TemplateField HeaderText="Ações" HeaderStyle-CssClass="rgHeader centro">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btiEditar" CssClass="btnPadding" runat="server" ToolTip="Editar" AlternateText="Editar" CausesValidation="False" CommandName="Update" CommandArgument='<%# Eval("Idf_Formacao") %>'><i class="fa fa-pencil"></i></asp:LinkButton>
                                            <asp:LinkButton ID="btiExcluir" runat="server" AlternateText="Excluir" CausesValidation="False" ToolTip="Excluir" CommandName="Delete" CommandArgument='<%# Eval("Idf_Formacao") %>'><i class="fa fa-times"></i></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="rgHeader centro coluna_icones" />
                                        <ItemStyle CssClass="espaco_icones centro" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSalvarFormacao" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                <%--</div>--%>
                <!--FIM: GridView Cursos consulta-->
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- FIM Painel: Formacao -->
    <!-- Painel: Formacao Especialização -->
    <asp:UpdatePanel ID="upFormacaoEspecializacao" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlFormacaoEspecializacao" runat="server">
                <h2 class="titulo_painel_padrao">
                    <asp:Label ID="lblTituloFormacaoEspecializacao" runat="server" Text="Especialização" />
                </h2>
                <asp:Panel ID="pnlEspecializacao" runat="server" CssClass="painel_padrao">
                    <div class="painel_padrao_topo">
                    </div>
                    <div class="linha">
                        <asp:Label ID="lblNivelEspecializacao" runat="server" Text="Nível de Especialização" AssociatedControlID="ddlNivelEspecializacao" CssClass="label_principal"></asp:Label>
                        <div class="container_campo">
                            <asp:UpdatePanel ID="upNivelEspecializacao" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlNivelEspecializacao" runat="server" ValidationGroup="CadastroEspecializacao" CssClass="textbox_padrao campo_obrigatorio_radcombobox" OnSelectedIndexChanged="ddlNivelEspecializacao_SelectedIndexChanged" AutoPostBack="True">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <!-- FIM: Linha Nível -->
                    <!-- Linha Instituicao -->
                    <div id="divLinhaInstituicaoEspecializacao" class="linha" runat="server">
                        <asp:Label ID="lblInstituicaoEspecializacao" runat="server" Text="Instituição de Ensino" CssClass="label_principal" AssociatedControlID="txtInstituicaoEspecializacao"></asp:Label>
                        <div class="container_campo">
                            <div>
                                <asp:RequiredFieldValidator ID="rfvInstituicaoEspecializacao" runat="server" ControlToValidate="txtInstituicaoEspecializacao" ValidationGroup="CadastroEspecializacao"></asp:RequiredFieldValidator>
                            </div>
                            <asp:TextBox ID="txtInstituicaoEspecializacao" runat="server" CssClass="textbox_padrao campo_obrigatorio" Columns="80" MaxLength="100" AutoPostBack="True" ontextchanged="txtInstituicaoEspecializacao_TextChanged"></asp:TextBox>
                            <AjaxToolkit:AutoCompleteExtender ID="aceInstituicaoEspecializacao" runat="server" TargetControlID="txtInstituicaoEspecializacao" ServiceMethod="ListarSiglaNomeFonteNivelCurso">
                            </AjaxToolkit:AutoCompleteExtender>
                        </div>
                    </div>
                    <!-- FIM: Linha Instituicao -->
                    <!-- Linha Titulo Curso -->
                    <div id="divLinhaTituloCursoEspecializacao" class="linha" runat="server">
                        <asp:Label ID="Label6" runat="server" Text="Nome do Curso" CssClass="label_principal" AssociatedControlID="txtTituloCursoEspecializacao"></asp:Label>
                        <div class="container_campo">
                            <div>
                                <asp:RequiredFieldValidator ID="rfvTituloCursoEspecializacao" runat="server" ControlToValidate="txtTituloCursoEspecializacao" ValidationGroup="CadastroEspecializacao"></asp:RequiredFieldValidator>
                            </div>
                            <asp:TextBox ID="txtTituloCursoEspecializacao" runat="server" CssClass="textbox_padrao campo_obrigatorio" MaxLength="100"></asp:TextBox>
                            <AjaxToolkit:AutoCompleteExtender ID="aceTituloCursoEspecializacao" runat="server" TargetControlID="txtTituloCursoEspecializacao" ServiceMethod="ListarCursoFonte">
                            </AjaxToolkit:AutoCompleteExtender>
                        </div>
                    </div>
                    <!-- FIM: Linha Titulo Curso -->
                    <!-- Linha Cidade -->
                    <div id="divCidadeEspecializacao" class="linha" runat="server">
                        <asp:Label runat="server" ID="Label9" Text="Cidade" CssClass="label_principal" AssociatedControlID="txtCidadeEspecializacao" />
                        <div class="container_campo">
                            <div>
                                <asp:RequiredFieldValidator ID="rfvCidadeEspecializacao" runat="server" ControlToValidate="txtCidadeEspecializacao" ValidationGroup="CadastroEspecializacao"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvCidadeEspecializacao" runat="server" ErrorMessage="Cidade Inválida." ClientValidationFunction="cvCidade_Validate" ValidationGroup="CadastroEspecializacao" ControlToValidate="txtCidadeEspecializacao"></asp:CustomValidator>
                            </div>
                            <asp:TextBox ID="txtCidadeEspecializacao" runat="server" Columns="50" CssClass="textbox_padrao" MaxLength="50"></asp:TextBox>
                        </div>
                    </div>
                    <!-- FIM: Linha Cidade -->
                    <!-- Linha Situacao -->
                    <div id="divLinhaSituacaoEspecializacao" class="linha" runat="server">
                        <asp:Label ID="lblSituacaoEspecializacao" runat="server" Text="Situação" CssClass="label_principal" AssociatedControlID="ddlSituacaoEspecializacao"></asp:Label>
                        <div class="container_campo">
                            <div>
                                <asp:CustomValidator ID="cvSituacaoEspecializacao" runat="server" ValidationGroup="CadastroEspecializacao" ControlToValidate="ddlSituacaoEspecializacao" ErrorMessage="Campo Obrigatório" ClientValidationFunction="cvSituacao_Validate">
                                </asp:CustomValidator>
                            </div>
                            <asp:DropDownList ID="ddlSituacaoEspecializacao" runat="server" ValidationGroup="CadastroEspecializacao">
                            </asp:DropDownList>
                        </div>
                        <asp:Label ID="lblPeriodoEspecializacao" runat="server" Text="Período" AssociatedControlID="txtPeriodoEspecializacao"></asp:Label>
                        <componente:AlfaNumerico ID="txtPeriodoEspecializacao" runat="server" ValidationGroup="CadastroEspecializacao" ContemIntervalo="true" ValorMinimo="1" ValorMaximo="12" Columns="2" MaxLength="2" Tipo="Numerico" />
                    </div>
                    <!-- FIM: Linha Situacao -->
                    <!-- Linha Ano Conclusao -->
                    <div id="divLinhaConclusaoEspecializacao" class="linha" runat="server">
                        <asp:Label ID="lblAnoConclusaoEspecializacao" runat="server" Text="Ano de Conclusão" CssClass="label_principal" AssociatedControlID="txtAnoConclusaoEspecializacao"></asp:Label>
                        <div class="container_campo">
                            <componente:AlfaNumerico CssClassTextBox="textbox_padrao" ID="txtAnoConclusaoEspecializacao" runat="server" Tipo="Numerico" MaxLength="4" Obrigatorio="false" Columns="4" ContemIntervalo="true" ValorMinimo="1950" ValorMaximo="2500" ValidationGroup="CadastroEspecializacao" />
                        </div>
                    </div>
                    <!-- FIM: Linha Ano Conclusao -->
                    <!-- Painel botoes -->
                    <asp:Panel ID="pnlBotoesEspecializacao" runat="server" CssClass="painel_botoes">
                        <asp:Button ID="btnSalvarEspecializacao" runat="server" Text="Adicionar Curso" CausesValidation="true" CssClass="botao_padrao" ValidationGroup="CadastroEspecializacao" OnClick="btnSalvarEspecializacao_Click" />
                    </asp:Panel>
                    <!-- FIM: Painel botoes -->
                    <!-- GridView Especialização consulta-->
                   <%-- <div>--%>
                        <asp:UpdatePanel ID="upGvEspecializacao" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="gvEspecializacao" runat="server" AllowSorting="False" AllowPaging="False" AllowCustomPaging="False" AlternatingRowStyle-CssClass="alt_row" AutoGenerateColumns="false" OnRowUpdating="gvEspecializacao_RowUpdating" DataKeyNames="Idf_Formacao" OnRowDeleting="gvEspecializacao_RowDeleting">
                                    <Columns>
                                        <asp:BoundField HeaderText="Nível formação" HeaderStyle-CssClass="rgHeader centro" DataField="Des_BNE" />
                                        <asp:BoundField HeaderText="Nome do Curso" HeaderStyle-CssClass="rgHeader centro" DataField="Des_Curso" />
                                        <asp:BoundField HeaderText="Instituição de Ensino" HeaderStyle-CssClass="rgHeader centro" DataField="Nme_Fonte" />
                                        <asp:BoundField HeaderText="Data de Conclusão" HeaderStyle-CssClass="rgHeader centro" ItemStyle-CssClass="centro" DataField="Ano_Conclusao" />
                                        <asp:TemplateField HeaderText="Ações" HeaderStyle-CssClass="rgHeader centro">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btiEditar" CssClass="btnPadding" runat="server" AlternateText="Editar" ToolTip="Editar" CausesValidation="False" CommandName="Update" CommandArgument='<%# Eval("Idf_Formacao") %>' ><i class="fa fa-pencil"></i></asp:LinkButton>
                                                <asp:LinkButton ID="btiExcluir" runat="server" AlternateText="Excluir" CausesValidation="False" ToolTip="Excluir" CommandName="Delete" CommandArgument='<%# Eval("Idf_Formacao") %>'><i class="fa fa-times"></i></asp:LinkButton>
                                           </ItemTemplate>
                                            <HeaderStyle CssClass="rgHeader centro coluna_icones" />
                                            <ItemStyle CssClass="espaco_icones centro" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSalvarEspecializacao" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    <%--</div>--%>
                    <!-- FIM: GridView Especialização consulta-->
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- FIM Painel: Formacao Especialização -->
    <!-- Painel: Cursos Complementares -->
    <h2 class="titulo_painel_padrao">
        <asp:Label ID="lblTituloCursosComplementares" runat="server" Text="Cursos Complementares" />
    </h2>
    <asp:UpdatePanel ID="upComplementar" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlCursosComplementares" runat="server" CssClass="painel_padrao">
                <div class="painel_padrao_topo">
                </div>
                <p>
                    Informe aqui participação em cursos extracurriculares. Exemplo: Curso de folha de pagamento, excel e informática
                </p>
                <!-- Linha Instituicao  -->
                <div class="linha">
                    <asp:Label ID="lblInstituicaoComplementar" runat="server" Text="Instituição de Ensino" CssClass="label_principal" AssociatedControlID="txtInstituicaoComplementar"></asp:Label>
                    <div class="container_campo">
                        <div>
                            <asp:RequiredFieldValidator ID="rfvInstituicaoComplementar" runat="server" ControlToValidate="txtInstituicaoComplementar" ValidationGroup="Complementar"></asp:RequiredFieldValidator>
                        </div>
                        <asp:TextBox ID="txtInstituicaoComplementar" runat="server" CssClass="textbox_padrao campo_obrigatorio" Columns="80" MaxLength="100"></asp:TextBox>
                        <AjaxToolkit:AutoCompleteExtender ID="aceInstituicaoComplementar" runat="server" TargetControlID="txtInstituicaoComplementar" ServiceMethod="ListarSiglaNomeFonteNivelCurso">
                        </AjaxToolkit:AutoCompleteExtender>
                    </div>
                </div>
                <!-- FIM: Linha Instituicao -->
                <!-- Linha Titulo Curso -->
                <div class="linha">
                    <asp:Label ID="lblTituloCursoComplementar" runat="server" Text="Nome do Curso" CssClass="label_principal" AssociatedControlID="txtTituloCursoComplementar"></asp:Label>
                    <div class="container_campo">
                        <div>
                            <asp:RequiredFieldValidator ID="rfvTituloCursoComplementar" runat="server" ControlToValidate="txtTituloCursoComplementar" ValidationGroup="Complementar"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cvTituloCursoComplementar" runat="server" ErrorMessage="Cidade Inválida." ClientValidationFunction="cvTituloCursoComplementar_Validate" ControlToValidate="txtTituloCursoComplementar" ValidationGroup="Complementar"></asp:CustomValidator>
                        </div>
                        <asp:TextBox ID="txtTituloCursoComplementar" runat="server" CssClass="textbox_padrao campo_obrigatorio" MaxLength="100"></asp:TextBox>
                        <%--Auto Complete estará disponivel quando for criado uma tabela de nome de cursos complementares--%>
                        <AjaxToolkit:AutoCompleteExtender ID="aceTituloCursoComplementar" runat="server" TargetControlID="txtTituloCursoComplementar" ServiceMethod="ListarCursoFonte">
                        </AjaxToolkit:AutoCompleteExtender>
                    </div>
                </div>
                <!-- FIM: Linha Titulo Curso -->
                <!-- Linha Cidade -->
                <div class="linha">
                    <asp:Label runat="server" ID="lblCidadeComplementar" Text="Cidade" CssClass="label_principal" AssociatedControlID="txtCidadeComplementar" />
                    <div class="container_campo">
                        <div>
                            <asp:CustomValidator ID="cvCidadeComplementar" runat="server" ErrorMessage="Cidade Inválida." ClientValidationFunction="cvCidadeComplementar_Validate" ControlToValidate="txtCidadeComplementar" ValidationGroup="CadastroComplementar"></asp:CustomValidator>
                        </div>
                        <asp:TextBox ID="txtCidadeComplementar" runat="server" CssClass="textbox_padrao" Columns="50" MaxLength="50"></asp:TextBox>
                    </div>
                </div>
                <!-- FIM: Linha Cidade  -->
                <!-- Linha Ano Conclusao -->
                <div class="linha">
                    <asp:Label ID="lblAnoConclusaoComplementar" runat="server" CssClass="label_principal" Text="Ano de Conclusão" AssociatedControlID="txtAnoConclusaoComplementar"></asp:Label>
                    <div class="container_campo">
                        <componente:AlfaNumerico ID="txtAnoConclusaoComplementar" runat="server" Tipo="Numerico" Obrigatorio="false" Columns="6" MaxLength="4" ContemIntervalo="true" CssClassTextBox="textbox_padrao" ValorMinimo="1950" ValorMaximo="2500" ValidationGroup="Complementar" />
                    </div>
                    <!-- Linha Carga Horária -->
                    <div class="linha">
                        <asp:Label ID="lblCargaHorariaComplementar" runat="server" Text="Carga Horária" CssClass="label_principal" AssociatedControlID="txtCargaHorariaComplementar"></asp:Label>
                        <div class="container_campo">
                            <componente:AlfaNumerico ID="txtCargaHorariaComplementar" runat="server" Tipo="Numerico" MaxLength="6" Obrigatorio="false" CssClassTextBox="textbox_padrao" Columns="6" ContemIntervalo="true" ValorMinimo="1" ValorMaximo="10000" ValidationGroup="Complementar" />
                        </div>
                    </div>
                    <!-- FIM: Linha Carga Horária -->
                </div>
                <!-- FIM: Linha Ano Conclusao -->
                <!-- Painel botoes -->
                <asp:Panel ID="pnlBotoesCursosComplementares" runat="server" CssClass="painel_botoes">
                    <asp:Button ID="btnSalvarCursoComplementar" runat="server" Text="Adicionar Curso" CausesValidation="true" CssClass="botao_padrao" ValidationGroup="CadastroComplementar" OnClick="btnSalvarCursoComplementar_Click" />
                </asp:Panel>
                <!-- FIM: Painel botoes -->
                <!--GridView Cursos consulta-->
                <div>
                    <asp:UpdatePanel ID="upGvComplementar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="gvComplementar" runat="server" AllowSorting="False" AllowPaging="False" AlternatingRowStyle-CssClass="alt_row" AutoGenerateColumns="False" AllowCustomPaging="False" DataKeyNames="Idf_Formacao" OnRowDeleting="gvComplementar_RowDeleting" OnRowUpdating="gvComplementar_RowUpdating">
                                <Columns>
                                    <asp:BoundField HeaderText="Nível formação" HeaderStyle-CssClass="rgHeader centro" DataField="Des_BNE" />
                                    <asp:BoundField HeaderText="Nome do Curso" HeaderStyle-CssClass="rgHeader centro" DataField="Des_Curso" />
                                    <asp:BoundField HeaderText="Instituição de Ensino" HeaderStyle-CssClass="rgHeader centro" DataField="Nme_Fonte" />
                                    <asp:BoundField HeaderText="Data de Conclusão" ItemStyle-CssClass="centro" HeaderStyle-CssClass="rgHeader centro" DataField="Ano_Conclusao" />
                                    <asp:TemplateField HeaderText="Ações">
                                        <ItemTemplate>
                                         <asp:Linkbutton CommandName="Update" CssClass="btnPadding" ID="btiEditar" runat="server" AlternateText="Editar" CausesValidation="False" CommandArgument='<%# Eval("Idf_Formacao") %>'><i class="fa fa-pencil"></i></asp:Linkbutton>
                                         <asp:Linkbutton CommandName="Delete" ID="btiExcluir" runat="server" AlternateText="Excluir" CausesValidation="False"  ToolTip="Excluir" CommandArgument='<%# Eval("Idf_Formacao") %>'><i class="fa fa-times"></i></asp:Linkbutton>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="rgHeader centro coluna_icones" />
                                        <ItemStyle CssClass="espaco_icones centro" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSalvarCursoComplementar" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <!--FIM: GridView Cursos consulta-->
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- FIM Painel: Cursos Complementares -->
    <!-- Painel: Idiomas -->
    <h2 class="titulo_painel_padrao">
        <asp:Label ID="lblIdiomas" runat="server" Text="Idiomas" />
    </h2>
    <asp:UpdatePanel ID="upIdiomas" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlIdiomas" runat="server" CssClass="painel_padrao">
                <div class="painel_padrao_topo">
                </div>
                <!-- Linha Idioma -->
                <div class="linha">
                    <asp:Label ID="lblIdioma" runat="server" Text="Idioma" CssClass="label_principal" AssociatedControlID="ddlIdioma"></asp:Label>
                    <div class="container_campo">
                        <asp:DropDownList ID="ddlIdioma" runat="server" CssClass="textbox_padrao" ValidationGroup="CadastroIdioma">
                        </asp:DropDownList>
                    </div>
                </div>
                <!-- FIM: Linha Idioma -->
                <!-- Linha Nivel -->
                <div class="linha">
                    <asp:Label ID="lblNivelidioma" runat="server" Text="Nível" CssClass="label_principal" AssociatedControlID="rblNivelIdioma"></asp:Label>
                    <div class="container_campo">
                        <div>
                            <asp:CustomValidator ID="cvNivelIdioma" runat="server" ValidationGroup="CadastroIdioma" ErrorMessage="Selecione um nível de idioma" OnServerValidate="cvNivelIdioma_ServerValidate" Display="Dynamic">
                            </asp:CustomValidator>
                        </div>
                        <asp:RadioButtonList ID="rblNivelIdioma" runat="server">
                        </asp:RadioButtonList>
                    </div>
                </div>
                <!-- FIM: Linha Nivel -->
                <!-- Painel botoes -->
                <asp:Panel ID="Panel2" runat="server" CssClass="painel_botoes">
                    <asp:Button ID="btnSalvarIdioma" runat="server" Text="Adicionar Idioma" CausesValidation="True" CssClass="botao_padrao" ValidationGroup="CadastroIdioma" OnClick="btnSalvarIdioma_Click" />
                </asp:Panel>
                <!-- FIM: Painel botoes -->
                <!--GridView Idiomas consulta-->
                <div>
                    <asp:GridView ID="gvIdioma" runat="server" AllowSorting="false" AlternatingRowStyle-CssClass="alt_row" AllowPaging="false" CssClass="gridview_padrao" AutoGenerateColumns="False" DataKeyNames="Idf_Pessoa_Fisica_Idioma" OnRowDeleting="gvIdioma_RowDeleting">
                        <Columns>
                            <asp:BoundField HeaderText="Idioma" DataField="Des_Idioma" HeaderStyle-CssClass="rgHeader centro" />
                            <asp:BoundField HeaderText="Nível" HeaderStyle-CssClass="rgHeader centro" DataField="Des_Nivel_Idioma" />
                            <asp:TemplateField HeaderText="Ações">
                                <ItemTemplate>
                                    <asp:Linkbutton CssClass="btnPadding" ID="btiExcluir" runat="server" AlternateText="Excluir" ToolTip="Excluir" CausesValidation="False" CommandName="Delete" ><i class="fa fa-times"></i></asp:Linkbutton>
                                </ItemTemplate>
                                <HeaderStyle CssClass="rgHeader centro coluna_icones" />
                                <ItemStyle CssClass="espaco_icones centro" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <!--FIM: GridView Idiomas consulta-->
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- FIM Painel: Idiomas -->
</div>
<!-- Painel botoes -->
<asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
    <asp:Button ID="btnSalvar" runat="server" Text="Salvar e Avançar" ValidationGroup="salvar" CausesValidation="true" OnClick="btnSalvar_Click" CssClass="botao_padrao" />
</asp:Panel>
<!-- FIM:Painel botoes-->
<%--    </ContentTemplate>
</asp:UpdatePanel>--%>
