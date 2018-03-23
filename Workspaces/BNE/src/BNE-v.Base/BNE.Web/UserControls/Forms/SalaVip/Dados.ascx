<%@ Control
    Language="C#"
    AutoEventWireup="True"
    CodeBehind="Dados.ascx.cs"
    Inherits="BNE.Web.UserControls.Forms.SalaVip.Dados" %>
<%@ Register Src="~/UserControls/UcValidacaoCelular.ascx" TagName="UcValidacaoCelular" TagPrefix="uc1" %>
<link href="../../../icons/stylecv.css" rel="stylesheet" />
<script src="../../../js/local/Forms/CadastroCurriculo/DadosPessoais.js"></script>
<asp:UpdatePanel
    ID="upCabecalho"
    runat="server"
    UpdateMode="Conditional">
    <ContentTemplate>
        <div class="dadosTopo">
            <div class="dadosCandidato">
                <h2 class="nome_destaque">
                    <span class="foto_usuario">
                        <asp:Image
                            ID="imgFotoUsuario"
                            runat="server" />
                    </span><span
                        class="nome_usuario_sv">
                        <asp:Label
                            ID="lblSaudacao"
                            runat="server"></asp:Label>
                        <asp:Label
                            ID="lblNomePessoa"
                            runat="server" /></span>
                </h2>
            </div>
            <asp:Panel runat="server" ID="pnAviso" Visible="false" CssClass="alert">
                <asp:Literal runat="server" ID="ltAvisoSucesso"></asp:Literal></asp:Panel>
            <asp:Panel runat="server" class="callout callout-info box.box-warning img-polaroid" ID="pnPerguntaAleatoria">
                <%--<div >--%>

                <asp:Panel runat="server" ID="pnUltimaEmpresa" Visible="false">
                    <asp:Panel runat="server" ID="pnUltimaEmpresa_Fase1" Visible="false">
                        <p class="pNomeEmpresa">Atualize as informações do seu último emprego</p>
                        <%-- Linha Empresa --%>
                        <p>
                            <div class="linha">
                                <asp:Label ID="lblEmpresa1" CssClass="label_principal" runat="server" Text="Nome da Empresa"
                                    AssociatedControlID="txtEmpresa1" />
                                <div class="container_campo">
                                    <div>
                                        <asp:RequiredFieldValidator ID="rfvEmpresa1" runat="server" ControlToValidate="txtEmpresa1"
                                            ValidationGroup="CadastroDadosPessoais" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                    <asp:TextBox ID="txtEmpresa1" runat="server" CssClass="textbox_padrao textbox_sala_Vip" Columns="60"
                                        MaxLength="60" AutoPostBack="true" OnTextChanged="txtEmpresa1_TextChanged"></asp:TextBox>
                                </div>
                            </div>
                        </p>
                        <%-- FIM: Linha Empresa --%>
                        <%-- Linha Data Admissão --%>
                        <%--<div class="linha">--%>
                        <p>
                            <asp:Label ID="lblDataAdmissao1" runat="server" Text="Data de Admissão"
                                AssociatedControlID="txtDataAdmissao1" />
                        </p>
                        <p>
                            <componente:Data ID="txtDataAdmissao1" runat="server" MensagemErroIntervalo="Data Inválida"
                                CssClassTextBox="textbox_padrao" ValidationGroup="CadastroDadosPessoais" />
                        </p>
                        <%-- Linha Data Demissão --%>
                        <p>
                            <asp:Label ID="lblDataDemissao1" runat="server" Text="Data de Demissão" AssociatedControlID="txtDataDemissao1" />

                            <componente:Data ID="txtDataDemissao1" runat="server" ValidationGroup="CadastroDadosPessoais"
                                MensagemErroIntervalo="Data Inválida" Obrigatorio="false" CssClassTextBox="textbox_padrao" />
                        </p>
                        <%-- FIM: Linha Data Demissão --%>
                        <%--</div>--%>
                        <%-- FIM: Linha Data Admissão --%>
                        <p>
                            <asp:Button runat="server" ID="btnSalvarUltimaEmpresa_Fase1" Text="Salvar e continuar" OnClick="btnSalvarUltimaEmpresa_Fase1_Click" CssClass="btn btn-primary btn-small" />
                            <asp:Button runat="server" ID="btnPularUltimaEmpresa_Fase1" Text="Atualizar depois" OnClick="btnPularExperiencia_Click" CssClass="btn btn-warning btn-small" />
                        </p>
                    </asp:Panel>

                    <asp:Panel runat="server" ID="pnUltimaEmpresa_Fase2" Visible="false">
                        <%-- Linha Função Exercida --%>
                        <div class="linha">
                            <asp:Label ID="lblFuncaoExercida1" CssClass="label_principal" runat="server" Text="Função Exercida"
                                AssociatedControlID="txtFuncaoExercida1" />
                            <div class="container_campo">
                                <div>
                                    <asp:RequiredFieldValidator ID="rfvFuncaoExercida1" runat="server" ControlToValidate="txtFuncaoExercida1"
                                        ValidationGroup="CadastroDadosPessoais"></asp:RequiredFieldValidator>
                                </div>
                                <asp:TextBox ID="txtFuncaoExercida1" runat="server" CssClass="textbox_padrao textbox_sala_Vip" Columns="60"
                                    MaxLength="60" AutoPostBack="true" OnTextChanged="txtFuncaoExercida1_TextChanged"></asp:TextBox>
                                <AjaxToolkit:AutoCompleteExtender ID="aceFuncaoExercida1" runat="server" TargetControlID="txtFuncaoExercida1"
                                    UseContextKey="True" ServiceMethod="ListarFuncoes" OnClientItemSelected="">
                                </AjaxToolkit:AutoCompleteExtender>
                            </div>
                        </div>
                        <%-- FIM: Linha Função Exercida --%>
                        <p class="label_principal">Confira as atividades que você realizava na
                            <asp:Literal runat="server" ID="ltNomeEmpresa"></asp:Literal></p>
                        <p>
                            <componente:AlfaNumerico ID="txtAtividadeExercida" runat="server" MaxLength="2000"
                                OnBlurClient="" Rows="1000" ValidationGroup="CadastroDadosPessoais"
                                TextMode="Multiline" CssClassTextBox="textbox_padrao multiline atividades_exercidas"
                                Obrigatorio="false" MensagemErroObrigatorio="Campo obrigatório" OnKeyUpClient="txtAtividadeExercida_KeyUp" />
                            <asp:Panel runat="server" ID="pnlBoxSugestaoTarefas" CssClass="BoxSugestaoTarefas">
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
                        </p>
                        <p>
                            <asp:Button runat="server" ID="btnSalvarExperiencia" Text="Atualizar" OnClick="btnSalvarExperiencia_Click" CssClass="btn btn-primary btn-small" /></p>
                        <%--<p><asp:Button runat="server" ID="btnPularExperiencia" Text="Atualizar depois" OnClick="btnPularExperiencia_Click" CssClass="btn btn-warning btn-small"/></p>--%>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnCelular" Visible="false">
                    <asp:Panel runat="server" ID="pnPerguntaCelular">
                        <p class="pNomeEmpresa">Seu número de celular ainda é?</p>
                        <p><b>
                            <asp:Label runat="server" ID="lbCelular" class="formataLabel"></asp:Label></b></p>
                        <p>
                            <asp:Button runat="server" ID="btnCelularSim" Text="Sim" OnClick="btnCelularSim_Click" CssClass="btn btn-primary btn-small" />
                            <asp:Button runat="server" ID="btnCelularNão" Text="Não" OnClick="btnCelularNao_Click" CssClass="btn btn-warning btn-small" />
                        </p>
                        <asp:HiddenField runat="server" ID="hdfTelefoneDDD" Value="" />
                        <asp:HiddenField runat="server" ID="hdfTelefoneNumero" Value="" />
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnAtualizarCelular" Visible="false">
                        <p class="pNomeEmpresa">Por favor, atualize o seu número de celular.</p>
                        <asp:UpdatePanel ID="upTxtTelefoneCelular" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <componente:Telefone ID="txtTelefoneCelular" runat="server" MensagemErroFormatoFone='<%$ Resources: MensagemAviso, _100006 %>'
                                    ValidationGroup="CadastroCurriculoMini" Tipo="Celular" OnValorAlteradoFone="txtTelefoneCelular_OnValorAlteradoFone" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="upValidacaoCelular" runat="server" UpdateMode="Conditional" style="display: none;">
                            <ContentTemplate>
                                <asp:Panel runat="server" ID="pnlValidacaoCelular" Visible="false">
                                    <p class="pCodigoVerif">
                                        <uc1:UcValidacaoCelular ID="UcValidacaoCelular" runat="server" />
                                    </p>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:Panel runat="server" CssClass="painel_botoes" ID="pnlBotoes">
                            <asp:Button runat="server" ID="btnSalvarTelefone" Text="Atualizar" OnClick="btnSalvarTelefone_Click"
                                CssClass="btn btn-warning btn-medium" ValidationGroup="CadastroCurriculoMini" />
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnEmail" Visible="false">
                    <asp:Panel runat="server" ID="pnPerguntaEmail">
                        <p class="pNomeEmpresa">Seu e-mail ainda é?</p>
                        <p><b>
                            <asp:Label runat="server" ID="lblEmail" class="formataLabel"></asp:Label></b></p>
                        <p>
                            <asp:Button runat="server" ID="btnEmailSim" Text="Sim" OnClick="btnEmailSim_Click" CssClass="btn btn-primary btn-small" />
                            <asp:Button runat="server" ID="btnEmailNao" Text="Não" OnClick="btnEmailNao_Click" CssClass="btn btn-warning btn-small" />
                        </p>
                        <asp:HiddenField runat="server" ID="hdfEmail" Value="" />
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnAtualizarEmail" Visible="false">
                        <p class="pNomeEmpresa">Informe o seu novo e-mail.</p>
                        <div class="container_campo">
                            <div>
                                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                    ValidationGroup="" ErrorMessage="Email Inválido.">
                                </asp:RegularExpressionValidator>
                            </div>
                            <asp:TextBox ID="txtEmail" runat="server" Columns="50" MaxLength="50" CssClass="textbox_padrao"></asp:TextBox>
                            <AjaxToolkit:AutoCompleteExtender ID="aceEmail" runat="server" TargetControlID="txtEmail"
                                UseContextKey="False" ServiceMethod="ListarSugestaoEmail">
                            </AjaxToolkit:AutoCompleteExtender>
                        </div>
                        <asp:Button runat="server" ID="btnSalvarEmail" Text="Atualizar" OnClick="btnSalvarEmail_Click"
                            CssClass="btn btn-warning btn-medium" ValidationGroup="" />
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnSalario" Visible="false">
                    <asp:Panel runat="server" ID="pnPerguntaSalario">
                        <p class="pNomeEmpresa">Sua pretenção salarial ainda é?</p>
                        <p><b>
                            <asp:Label runat="server" ID="lblSalario" class="formataLabel"></asp:Label></b></p>
                        <p>
                            <asp:Button runat="server" ID="btnSalarioSim" Text="Sim" OnClick="btnSalarioSim_Click" CssClass="btn btn-primary btn-small" />
                            <asp:Button runat="server" ID="btnSalarioNao" Text="Não" OnClick="btnSalarioNao_Click" CssClass="btn btn-warning btn-small" />
                        </p>
                        <asp:HiddenField runat="server" ID="hdfValorSalario" Value="" />
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnAtualizarSalario" Visible="false">
                        <p class="pNomeEmpresa">Informe sua nova pretenção salarial.</p>
                        <span id="faixa_salarial" class="faixa_informacao_destaque" runat="server" visible="false"></span>
                        <div class="container_campo">
                            <componente:ValorDecimal ID="txtPretensaoSalarial" runat="server" CasasDecimais="0"
                                ValidationGroup="CadastroCurriculoMini" ValorMaximo="999999" ValorMinimo="1" />
                            <input type="hidden" id="hdfSalarioMinimo" runat="server" />
                            <label class="decimais">
                                ,00</label>
                            <asp:Button runat="server" ID="btnSalvarSalario" Text="Atualizar" OnClick="btnSalvarSalario_Click"
                                CssClass="btn btn-warning btn-medium" ValidationGroup="CadastroCurriculoMini" />

                        </div>

                    </asp:Panel>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnEducacao" Visible="false">
                    <asp:Panel runat="server" ID="pnFormacao" Visible="false">
                        <asp:Panel runat="server" ID="pnPerguntaFormacao">
                            <asp:Literal runat="server" ID="ltDescricaoPergunta"></asp:Literal>
                            <p>
                                <asp:Button runat="server" ID="btnFormacaoSim" Text="Sim" OnClick="btnFormacaoSim_Click" CssClass="btn btn-primary btn-small" />
                                <asp:Button runat="server" ID="btnFormacaoNao" Text="Não" OnClick="btnFormacaoNao_Click" CssClass="btn btn-warning btn-small" />
                            </p>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="pnAtualizarFormacao" Visible="false">
                            <!-- Linha Nível-->
                            <div class="linha">
                                <asp:Label ID="lblNivel" runat="server" Text="Nível de Formação" AssociatedControlID="ddlNivel" CssClass="label_principal"></asp:Label>
                                <div class="container_campo">
                                    <asp:DropDownList ID="ddlNivel" runat="server" ValidationGroup="CadastroFormacao" CssClass="textbox_padrao campo_obrigatorio" OnSelectedIndexChanged="ddlNivel_SelectedIndexChanged" AutoPostBack="True">
                                    </asp:DropDownList>

                                    <asp:Panel runat="server" ID="pnDadosFormacao" Visible="false"></asp:Panel>
                                    <!-- Linha Instituicao -->
                                    <div id="divLinhaInstituicao" class="linha" runat="server">
                                        <asp:Label ID="lblInstituicao" runat="server" Text="Instituição de Ensino" CssClass="label_principal" AssociatedControlID="txtInstituicao"></asp:Label>
                                        <div class="container_campo">
                                            <div>
                                                <asp:RequiredFieldValidator ID="rfvInstituicao" runat="server" ControlToValidate="txtInstituicao" ValidationGroup="CadastroFormacao"></asp:RequiredFieldValidator>
                                            </div>
                                            <asp:TextBox ID="txtInstituicao" runat="server" CssClass="textbox_padrao campo_obrigatorio" Columns="80" MaxLength="100" AutoPostBack="True" OnTextChanged="txtInstituicao_TextChanged"></asp:TextBox>
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
                                            <AjaxToolkit:AutoCompleteExtender ID="aceCidade" runat="server" TargetControlID="txtCidade" ServiceMethod="ListarCidadesSiglaEstadoPorNomeParcialEstado">
                                            </AjaxToolkit:AutoCompleteExtender>
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
                                            <componente:AlfaNumerico CssClassTextBox="textbox_padrao" ID="txtAnoConclusao" runat="server" Tipo="Numerico" MaxLength="4" Obrigatorio="false" Columns="4" ContemIntervalo="true" ValorMinimo="1950" ValorMaximo="2500" ValidationGroup="CadastroFormacao" />
                                        </div>
                                    </div>
                                    <!-- FIM: Linha Ano Conclusao -->
                                </div>
                            </div>

                            <asp:Button runat="server" ID="btnSalvarEducacao" Text="Atualizar" OnClick="btnSalvarEducacao_Click"
                                CssClass="btnAmarelo" ValidationGroup="CadastroFormacao" />
                            <!-- FIM: Linha Nível-->

                            <asp:Panel ID="pnlFormacaoEspecializacao" runat="server" Visible="false">
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
                                            <asp:TextBox ID="txtInstituicaoEspecializacao" runat="server" CssClass="textbox_padrao campo_obrigatorio" Columns="80" MaxLength="100" AutoPostBack="True" OnTextChanged="txtInstituicaoEspecializacao_TextChanged"></asp:TextBox>
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
                                            <AjaxToolkit:AutoCompleteExtender ID="aceCidadeEspecializacao" runat="server" TargetControlID="txtCidadeEspecializacao" ServiceMethod="ListarCidadesSiglaEstadoPorNomeParcialEstado">
                                            </AjaxToolkit:AutoCompleteExtender>
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
                                    <%--<asp:Panel ID="pnlBotoesEspecializacao" runat="server" CssClass="painel_botoes">
                        <asp:Button ID="btnSalvarEspecializacao" runat="server" Text="Adicionar Curso" CausesValidation="true" CssClass="botao_padrao" ValidationGroup="CadastroEspecializacao" OnClick="btnSalvarEspecializacao_Click" />
                    </asp:Panel>--%>
                                    <!-- FIM: Painel botoes -->
                                </asp:Panel>
                            </asp:Panel>
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>
        </div>

    </ContentTemplate>
</asp:UpdatePanel>
