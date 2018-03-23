<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="SalaSelecionadorRastreadorCurriculos.aspx.cs" Inherits="BNE.Web.SalaSelecionadorRastreadorCurriculos" %>

<%@ Register TagPrefix="ucIdioma" Src="~/UserControls/IdiomaEmPesquisaCurriculo.ascx" TagName="PesquisaCurriculoIdioma" %>
<%@ Register TagPrefix="usercontrol" TagName="Confirmacao" Src="UserControls/Modais/ModalConfirmacao.ascx" %>
<%@ Register TagPrefix="usercontrol" TagName="ConfirmacaoExclusao" Src="UserControls/Modais/ConfirmacaoExclusao.ascx" %>
<%@ Register TagPrefix="usercontrol" TagName="pesquisacurriculocustomizacao" Src="~/UserControls/FuncaoEmPesquisaCurriculo.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/SalaSelecionadorRastreadorCurriculos.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <h1>Alerta de Currículos</h1>
    <div class="painel_padrao">
        <%-- painel_padrao_sala_selecionador --%>
        <p>
            Informe um
                novo alerta
                com o perfil
                dos currículos
                que você deseja
                receber. Assim,
                sempre que
                for cadastrado
                um novo currículo
                com o perfil
                solicitado,
                você receberá
                um aviso por
                e-mail.
        </p>
        <asp:Panel ID="pnlCadastro" runat="server">
            <asp:HiddenField
                ID="hfInstituicaoGraduacao"
                runat="server" />
            <asp:HiddenField
                ID="hfInstituicaoOutrosCursos"
                runat="server" />
            <asp:Panel
                ID="pnlCamposPesquisa"
                runat="server"
                CssClass="painel_padrao pesquisa_avancada"
                DefaultButton="btnSalvar">
                <%-- Início da fonte de dados de currículos--%>
                <asp:Panel ID="pnl_stc_fonte" runat="server" CssClass="row">
                    <div class="span11">
                        <div class="accordion" id="accordion_stc_fonte">
                            <div class="accordion-group-bne">
                                <div class="accordion-heading-bne accordion-heading-bne-bg-green">
                                    <a class="accordion-toggle-bne" data-toggle="collapse" data-parent="#accordion_stc_fonte" href="#accordion_stc_fonte_content">
                                        <h3 class="colorheader">Fonte de dados<i class="fa fa-sort-desc pull-right"></i></h3>
                                    </a>
                                </div>
                                <div id="accordion_stc_fonte_content" class="accordion-body accordion-body-set collapse in">
                                    <div class="accordion-inner-bne">
                                        <div class="span11">
                                            <div style="width: 100%; height: 30px;">
                                                <label><strong>Desejo buscar por currículo </strong></label>
                                            </div>
                                            <asp:UpdatePanel ID="FonteSTCUpdatePanel" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:CheckBox ID="chk_stc_fonte_bne" runat="server" Checked="true" Text="No BNE"
                                                        CssClass="stc_fonte_checkbox" OnCheckedChanged="chk_stc_fonte_bne_CheckedChanged" AutoPostBack="true" />
                                                    <asp:CheckBox ID="chk_stc_fonte_meus_cv" runat="server" Checked="true" Text="Nos meus CVs"
                                                        CssClass="stc_fonte_checkbox" OnCheckedChanged="chk_stc_fonte_meus_cv_CheckedChanged" AutoPostBack="true" />
                                                    <br />
                                                    <asp:Panel ID="pnl_stc_fonte_alert" Visible="false" CssClass="alert alert-warning stc_fonte_alert" runat="server" role="alert">
                                                        Marque pelo menos uma opção.
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <%--Fim da fonte de dados de currículos--%>
                <%--Start Accordion Definir Pretensão do Candidato--%>
                <div class="row">
                    <div class="span11">
                        <div class="accordion" id="accordionPretensao">
                            <div class="accordion-group-bne">
                                <div class="accordion-heading-bne accordion-heading-bne-bg-green">
                                    <a class="accordion-toggle-bne" data-toggle="collapse" data-parent="#accordionPretensao" href="#collapsePretenCand">
                                        <h3 class="colorheader">Pretensão do Candidato<i class="fa fa-sort-desc pull-right"></i></h3>
                                    </a>
                                </div>
                                <div id="collapsePretenCand" class="accordion-body accordion-body-set collapse in">
                                    <div class="accordion-inner-bne">
                                        <div class="span11">
                                            <span class="span5">
                                                <asp:UpdatePanel ID="upEstagiarioFuncao" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                    <ContentTemplate>
                                                        <usercontrol:pesquisacurriculocustomizacao Obrigatorio="false" ID="ucEstagiarioFuncao" ValidationGroup="PesquisaAvancada" runat="server" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </span>
                                        </div>
                                        <div class="span11">
                                            <span class="span5">
                                                <!-- Linha Disponibilidade de Trabalho -->
                                                <asp:Label ID="lblDisponibilidadeTrabalho" runat="server" CssClass="label_principal-set" AssociatedControlID="rcbDisponibilidade"><strong>Disponibilidade de Trabalho</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <asp:UpdatePanel ID="upDisponibilidade" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                    <ContentTemplate>
                                                        <Employer:ComboCheckbox ID="rcbDisponibilidade" EmptyMessage="Qualquer" runat="server" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <Componentes:BalaoSaibaMais ID="bsmChkDisponibilidade" runat="server" CssClassLabel="balao_saiba_mais" ToolTipText="Se você procura candidatos com disponibilidade para trabalho específico. Selecione nesse campo uma ou mais opções e tenha apenas os candidatos que atendem aos seus requisitos." ToolTipTitle="Disponibilidade para Trabalhar:" Text="Saiba Mais" ShowOnMouseover="true" />
                                                <!-- FIM: Linha Disponibilidade de Trabalho -->
                                            </span>
                                        </div>

                                        <%--Mostrar qdo for marcado Quero contratar estagiário--%>
                                        <div class="escolaridade-estagiario" id="escolaridade-estagiario">
                                            <div class="span11">
                                                <span class="span5">
                                                    <!-- Linha Nível-->
                                                    <asp:Label ID="Label2" runat="server" AssociatedControlID="rcbNivelEscolaridadeEstagiario" CssClass="label_principal-set"><strong>Escolaridade Mínima</strong></asp:Label>
                                                    <div class="clearfix"></div>
                                                    <asp:UpdatePanel ID="upNivelEscolaridadeEstagiario" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                        <ContentTemplate>
                                                            <telerik:RadComboBox ID="rcbNivelEscolaridadeEstagiario" CssClass="campoEscolaridade" runat="server"></telerik:RadComboBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                    <!-- FIM: Linha Nível -->
                                                </span>
                                                <span class="span4"></span>
                                            </div>
                                            <div class="span11">
                                                <span class="span5">
                                                    <!-- Linha Técnico/Graduação -->
                                                    <asp:Label ID="lblTituloTecnicoGraduacaoEstag" runat="server" CssClass="label_principal-set" AssociatedControlID="txtTituloTecnicoGraduacaoEstag"><strong>Técnico / Graduação em</strong></asp:Label>
                                                    <div class="clearfix"></div>
                                                    <asp:UpdatePanel ID="upTituloTecnicoGraduacaoEstag" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtTituloTecnicoGraduacaoEstag" runat="server" CssClass="textbox_padrao" MaxLength="100"></asp:TextBox>
                                                            <AjaxToolkit:AutoCompleteExtender ID="aceTituloTecnicoGraduacaoEstag" runat="server" TargetControlID="txtTituloTecnicoGraduacaoEstag" ServiceMethod="ListarCursoNivel3"></AjaxToolkit:AutoCompleteExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                    <!-- FIM: Técnico/Graduação -->
                                                </span>
                                                <span class="span4">
                                                    <!--Linha Instituicao Graduação-->
                                                    <asp:Label ID="lblInstituicaoTecnicoGraduacaoEstag" runat="server" AssociatedControlID="txtInstituicaoTecnicoGraduacaoEstag"><strong>Instituição</strong></asp:Label>
                                                    <div class="clearfix"></div>
                                                    <asp:UpdatePanel ID="upInstituicaoTecnicoGraduacaoEstag" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtInstituicaoTecnicoGraduacaoEstag" runat="server" CssClass="textbox_padrao" Columns="30" MaxLength="100"></asp:TextBox>
                                                            <AjaxToolkit:AutoCompleteExtender ID="aceInstituicaoTecnicoGraduacaoestag" runat="server" TargetControlID="txtInstituicaoTecnicoGraduacaoEstag" UseContextKey="false" ServiceMethod="ListarSiglaNomeFonte" OnClientItemSelected="InstituicaoGraduacao"></AjaxToolkit:AutoCompleteExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </span>
                                            </div>
                                        </div>
                                        <%--FIM: Mostrar qdo for marcado Quero contratar estagiário--%>
                                        <div class="span11">
                                            <span class="span5">
                                                <!-- Linha Salário -->
                                                <asp:Label ID="lblSalariob" runat="server" CssClass="label_principal-set" AssociatedControlID="txtSalario"><strong>Salário</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <asp:Label ID="lblSalario" AssociatedControlID="txtSalario" runat="server" CssClass="label_principal-set"><strong>Mínimo</strong></asp:Label>
                                                <asp:UpdatePanel ID="upSalarioDe" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                    <ContentTemplate>
                                                        <componente:ValorDecimal ID="txtSalario" runat="server" CasasDecimais="0" ValorMaximo="20000" Obrigatorio="false" CssClassTextBox="textbox_padrao textbox_padrao-set-salario" ValorAlteradoClient="txtSalarioDe_ValorAlterado" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <asp:Label ID="lblSalarioAte" AssociatedControlID="txtSalarioAte" runat="server" CssClass="label_principal-set"><strong>Máximo</strong></asp:Label>
                                                <asp:UpdatePanel ID="upSalarioAte" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                    <ContentTemplate>
                                                        <componente:ValorDecimal ID="txtSalarioAte" runat="server" CasasDecimais="0" ValorMaximo="20000" CssClassTextBox="textbox_padrao textbox_padrao-set-salario" Obrigatorio="false" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <span runat="server" associatedcontrolid="txtSalarioAte" class="set-label">reais</span>
                                                <!-- FIM: Linha Salário -->
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <%--End Accordion Definir Pretensão do Candidato--%>
                <%--Start Accordion Definir Região e Localidade--%>
                <div class="row">
                    <div class="span11">
                        <div class="accordion" id="accordionRegiaoLocalidade">
                            <div class="accordion-group-bne">
                                <div class="accordion-heading-bne accordion-heading-bne-bg-green">
                                    <a class="accordion-toggle-bne" data-toggle="collapse" data-parent="#accordionRegiaoLocalidade" href="#collapseRegLoc">
                                        <h3 class="colorheader">Região e Localidade<i class="fa fa-sort-desc pull-right"></i></h3>
                                    </a>
                                </div>
                                <div id="collapseRegLoc" class="accordion-body accordion-body-set collapse in">
                                    <div class="accordion-inner-bne">
                                        <div class="span11">
                                            <span class="span3 linha">
                                                <!-- Linha Cidade-->
                                                <asp:Label runat="server" ID="lblCidade" CssClass="label_principa-set" AssociatedControlID="txtCidadePesquisa"><strong>Cidade</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <asp:UpdatePanel ID="upCidade" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                    <ContentTemplate>
                                                        <asp:CustomValidator ID="cvCidadePesquisa" runat="server" ErrorMessage="Cidade Inválida." ClientValidationFunction="cvCidade_Validate" ControlToValidate="txtCidadePesquisa" ValidationGroup="PesquisaAvancada"></asp:CustomValidator>
                                                        <asp:TextBox ID="txtCidadePesquisa" runat="server" CssClass="textbox_padrao" Columns="25" OnTextChanged="txtCidadePesquisa_TextChanged" AutoPostBack="True"></asp:TextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <!--FIM: Linha Cidade-->
                                            </span>
                                            <span class="span3">
                                                <!--Linha Estado-->
                                                <asp:Label ID="lblUF" runat="server" CssClass="label_principal-set" AssociatedControlID="rcbEstado"><strong>Estado</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <asp:UpdatePanel ID="upEstado" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                    <ContentTemplate>
                                                        <telerik:RadComboBox ID="rcbEstado" CssClass="campoEstado" runat="server"></telerik:RadComboBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <!--FIM: Linha Estado-->
                                            </span>
                                            <asp:UpdatePanel runat="server" ID="upBairroZona" UpdateMode="Conditional" ChildrenAsTriggers="False">
                                                <ContentTemplate>
                                                    <asp:Panel runat="server" CssClass="span3" Visible="True" ID="pnlBairroZona">
                                                        <%--mostrar bairro por região qdo for São Paulo--%>
                                                        <asp:Label ID="Label5" AssociatedControlID="txtBairro" runat="server" CssClass="label_principal-set"><strong>Bairro</strong></asp:Label>
                                                        <div class="clearfix"></div>
                                                        <div style="position: inherit;">
                                                            <span class="span4 bairro">
                                                                <label class="label_principal-set">Centro</label>
                                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upBairroZonaCentral">
                                                                    <ContentTemplate>
                                                                        <Employer:ComboCheckbox runat="server" ID="rcbBairroZonaCentral" CssClass="padding-set-bairro" DropDownWidth="350" AllowCustomText="False" EmptyMessage="Selecione..." OnClientDropDownClosing="ClientDropDownClosing">
                                                                        </Employer:ComboCheckbox>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </span>
                                                            <span class="span4 bairro">
                                                                <label class="label_principal-set">Zona Norte</label>
                                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upBairroZonaNorte">
                                                                    <ContentTemplate>
                                                                        <Employer:ComboCheckbox runat="server" ID="rcbBairroZonaNorte" CssClass="padding-set-bairro" DropDownWidth="350" AllowCustomText="False" EmptyMessage="Selecione..." OnClientDropDownClosing="ClientDropDownClosing">
                                                                        </Employer:ComboCheckbox>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </span>
                                                            <span class="span4 bairro">
                                                                <label class="label_principal-set">Zona Sul</label>
                                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upBairroZonaSul">
                                                                    <ContentTemplate>
                                                                        <Employer:ComboCheckbox runat="server" ID="rcbBairroZonaSul" CssClass="padding-set-bairro" DropDownWidth="350" AllowCustomText="False" EmptyMessage="Selecione..." OnClientDropDownClosing="ClientDropDownClosing">
                                                                        </Employer:ComboCheckbox>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </span>
                                                            <span class="span4 bairro">
                                                                <label class="label_principal-set">Zona Leste</label>
                                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upBairroZonaLeste">
                                                                    <ContentTemplate>
                                                                        <Employer:ComboCheckbox runat="server" ID="rcbBairroZonaLeste" CssClass="padding-set-bairro" DropDownWidth="350" AllowCustomText="False" EmptyMessage="Selecione..." OnClientDropDownClosing="ClientDropDownClosing">
                                                                        </Employer:ComboCheckbox>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </span>
                                                            <span class="span4 bairro">
                                                                <label class="label_principal-set">Zona Oeste</label>
                                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upBairroZonaOeste">
                                                                    <ContentTemplate>
                                                                        <Employer:ComboCheckbox runat="server" ID="rcbBairroZonaOeste" CssClass="padding-set-bairro" DropDownWidth="350" AllowCustomText="False" EmptyMessage="Selecione..." OnClientDropDownClosing="ClientDropDownClosing">
                                                                        </Employer:ComboCheckbox>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </span>
                                                        </div>
                                                        <%--FIM:mostrar bairro por região qdo for São Paulo--%>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:UpdatePanel runat="server" ID="upBairroTexto" UpdateMode="Conditional" ChildrenAsTriggers="False">
                                                <ContentTemplate>
                                                    <asp:Panel runat="server" CssClass="span3" Visible="True" ID="pnlBairroTexto">
                                                        <!-- Linha Bairro Default qdo não for São Paulo  -->
                                                        <asp:Label ID="lblBairro" AssociatedControlID="txtBairro" runat="server" Text="Bairro" CssClass="label_principal-set" />
                                                        <div class="clearfix"></div>
                                                        <asp:UpdatePanel ID="upTxtBairro" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                            <ContentTemplate>
                                                                <componente:AlfaNumerico ID="txtBairro" runat="server" MaxLength="100" CssClassTextBox="textbox_padrao textbox_padrao-set-endereco" Obrigatorio="false" ValidationGroup="PesquisaAvancada" ExpressaoValidacao="^[A-Z,a-z,0-9,ÁáÂâÃãÉéÊêÍíÓóÔôÕõÚúÛûÇç\,']{0,}( ){0,}[A-Z,a-z,0-9,ÁáÂâÃãÉéÊêÍíÓóÔôÕõÚúÛûÇç\,']{0,}(( ){0,}[A-Z,a-z,0-9,ÁáÂâÃãÉéÊêÍíÓóÔôÕõÚúÛûÇç\,']{0,})*$" Enabled="True" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        <asp:UpdatePanel ID="upBtiMapaBairro" runat="server" UpdateMode="Conditional" RenderMode="Inline"></asp:UpdatePanel>
                                                        <!-- FIM: Linha Bairro Default qdo não for São Paulo -->
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="span11" style="position: relative;" id="bairro-endereco">
                                            <span class="span3">
                                                <!-- Linha Endereço -->
                                                <asp:Label ID="lblLogradouro" AssociatedControlID="txtLogradouro" runat="server" CssClass="label_principal-set"><strong>Endereço</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <asp:UpdatePanel ID="upLogradouro" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                    <ContentTemplate>
                                                        <componente:AlfaNumerico ID="txtLogradouro" CssClassTextBox="textbox_padrao textbox_padrao-set-endereco" runat="server" Obrigatorio="false" MaxLength="50" ValidationGroup="PesquisaAvancada" ExpressaoValidacao="^[A-Z,a-z,0-9,ÁáÂâÃãÉéÊêÍíÓóÔôÕõÚúÛûÇç\,']{0,}( ){0,}[A-Z,a-z,0-9,ÁáÂâÃãÉéÊêÍíÓóÔôÕõÚúÛûÇç\,']{0,}(( ){0,}[A-Z,a-z,0-9,ÁáÂâÃãÉéÊêÍíÓóÔôÕõÚúÛûÇç\,']{0,})*$" Enabled="True" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <span style="display: none">
                                                    <asp:Label ID="lblAvisoCidadeEndereco" runat="server"><strong>Selecione uma cidade</strong></asp:Label>
                                                </span>
                                                <!-- FIM: Linha Endereço -->
                                            </span>
                                            <span class="span3">
                                                <!-- Linha Faixa CEP -->
                                                <asp:Label ID="lblFaixaCep" runat="server" AssociatedControlID="txtFaixaCep" CssClass="label_principal-set"><strong>Faixa de CEP</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <asp:UpdatePanel ID="upFaixaCepDe" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                    <ContentTemplate>
                                                        <componente:CEP ID="txtFaixaCep" runat="server" Columns="8" MaxLength="8" Obrigatorio="False" ValorAlteradoClient="txtFaixaCep_ValorAlteradoClient" CssClassTextBox="textbox_padrao textbox_padrao-set-cep textbox_padrao-set" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <asp:Label ID="lblFaixaCepAte" AssociatedControlID="txtFaixaCepAte" runat="server" Text="Até" CssClass="label_principal-set" />
                                                <asp:CustomValidator ID="cvFuncaoFaixaCepAte" runat="server" ValidationGroup="PesquisaAvancada" ControlToValidate="txtTeste" ValidateEmptyText="true" ClientValidationFunction="cvFuncaoFaixaCepAte_Validate"></asp:CustomValidator>
                                                <asp:TextBox ID="txtTeste" Style="display: none" runat="server"></asp:TextBox>
                                                <asp:UpdatePanel ID="upFaixaCepAte" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                    <ContentTemplate>
                                                        <componente:CEP ID="txtFaixaCepAte" runat="server" Columns="8" MaxLength="8" Obrigatorio="False" ValorAlteradoClient="txtFaixaCepAte_ValorAlteradoClient" CssClassTextBox="textbox_padrao textbox_padrao-set-cep textbox_padrao-set" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <!-- FIM: Linha Faixa CEP-->
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <%--End Accordion Definir Região e Localidade--%>
                <%--Start Accordion Definir Palavra-chave--%>
                <div class="row">
                    <div class="span11">
                        <div class="accordion" id="accordionPalavraChave">
                            <div class="accordion-group-bne">
                                <div class="accordion-heading-bne accordion-heading-bne-bg-green">
                                    <a class="accordion-toggle-bne" data-toggle="collapse" data-parent="#accordionPalavraChave" href="#collapseOne">
                                        <h3 class="colorheader">Definir Palavra-Chave <i class="fa fa-sort-desc pull-right"></i></h3>
                                    </a>
                                </div>
                                <div id="collapseOne" class="accordion-body accordion-body-set collapse in">
                                    <div class="accordion-inner-bne">
                                        <div class="span11">
                                            <span class="span3">
                                                <!-- Linha Incluir Palavra-Chave -->
                                                <asp:Label ID="lblPalavraChave" runat="server" CssClass="label_principal-set" AssociatedControlID="txtPalavraChave"><strong>Incluir Palavra-Chave</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <asp:UpdatePanel ID="upPalavraChave" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                    <ContentTemplate>
                                                        <componente:AlfaNumerico ID="txtPalavraChave" runat="server" CssClassTextBox="textbox_padrao" Obrigatorio="false" Columns="25" ValidationGroup="PesquisaAvancada" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <Componentes:BalaoSaibaMais ID="bsmPalavraChave" runat="server" ToolTipText="Nesse campo você pode informar atribuições e conhecimentos que deseja que o candidato possua. Exemplo: vendas, Excel" Text="Saiba mais" ToolTipTitle="Palavra-Chave:" CssClassLabel="balao_saiba_mais" ShowOnMouseover="true" />
                                                <!-- FIM: Linha Incluir Palavra-Chave -->
                                            </span>
                                            <span class="span3">
                                                <asp:Label ID="lbBuscaPalavraChave" runat="server" CssClass="label_principal-set" AssociatedControlID="rcbBuscaPalavraChave"><strong>Busca em</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <asp:UpdatePanel ID="upBuscaPalavraChave" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                    <ContentTemplate>
                                                        <Employer:ComboCheckbox ID="rcbBuscaPalavraChave" EmptyMessage="Qualquer" runat="server">
                                                        </Employer:ComboCheckbox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <Componentes:BalaoSaibaMais ID="BalaoSaibaMaiPalavraChave" runat="server" CssClassLabel="balao_saiba_mais" ToolTipText="Selecione nesse campo uma ou mais opções e tenha apenas os candidatos que atendem aos seus requisitos." ToolTipTitle="Buscar em:" Text="Saiba Mais" ShowOnMouseover="true" />
                                            </span>
                                            <span class="span3">
                                                <!-- Linha Excluir Palavra-Chave -->
                                                <asp:Label ID="lblExcluirPalavraChave" runat="server" CssClass="label_principal-set" AssociatedControlID="txtPalavraChave"><strong>Excluir Palavra-Chave</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <asp:UpdatePanel ID="upExcluirPalavraChave" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                    <ContentTemplate>
                                                        <componente:AlfaNumerico ID="txtExcluirPalavraChave" runat="server" CssClassTextBox="textbox_padrao" Obrigatorio="false" Columns="25" ValidationGroup="PesquisaAvancada" autocomplete="on" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <Componentes:BalaoSaibaMais ID="bsmExcluirPalavraChave" runat="server" ToolTipText="Informe palavras para remover candidatos no resultado da pesquisa" Text="Saiba mais" ToolTipTitle="Excluir Palavra-Chave:" CssClassLabel="balao_saiba_mais" ShowOnMouseover="True" />
                                                <!-- FIM: Linha Excluir Palavra-Chave -->
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <%--End Accordion Definir Palavra-chave--%>
                <%--Start Accordion Definir Formação e Escolaridade--%>
                <div class="row">
                    <div class="span11">
                        <div class="accordion" id="accordionFormacaoEscolaridade">
                            <div class="accordion-group-bne">
                                <div class="accordion-heading-bne accordion-heading-bne-bg-green">
                                    <a class="accordion-toggle-bne" data-toggle="collapse" data-parent="#accordionFormacaoEscolaridade" href="#collapseFormEsc">
                                        <h3 class="colorheader">Formação e Escolaridade <i class="fa fa-sort-desc pull-right"></i></h3>
                                    </a>
                                </div>
                                <div id="collapseFormEsc" class="accordion-body accordion-body-set collapse in">
                                    <div class="accordion-inner-bne">
                                        <div class="span10">
                                            <span class="span3">
                                                <!-- Linha Nível-->
                                                <asp:Label ID="lblNivel" runat="server" AssociatedControlID="rcbNivelEscolaridade" CssClass="label_principal-set"><strong>Escolaridade Mínima</strong></asp:Label>
                                                <div class="clearfix"></div>

                                                <asp:UpdatePanel ID="upNivelEscolaridade" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                    <ContentTemplate>
                                                        <telerik:RadComboBox ID="rcbNivelEscolaridade" CssClass="campoEscolaridade" runat="server"></telerik:RadComboBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <!-- FIM: Linha Nível -->
                                            </span>
                                            <span class="span1"></span>
                                            <span class="span5">
                                                <%--Linha Idioma--%>

                                                <asp:UpdatePanel ID="upIdioma" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                    <ContentTemplate>
                                                        <ucIdioma:PesquisaCurriculoIdioma Obrigatorio="false" ID="ucPesquisaIdioma" ValidationGroup="PesquisaAvancada" runat="server" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </span>
                                        </div>
                                        <div class="span10">
                                            <span class="span5">
                                                <!-- Linha Técnico/Graduação -->
                                                <asp:Label ID="lblTituloTecnicoGraduacao" runat="server" CssClass="label_principal-set" AssociatedControlID="txtTituloTecnicoGraduacao"><strong>Técnico / Graduação em</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <asp:UpdatePanel ID="upTituloTecnicoGraduacao" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtTituloTecnicoGraduacao" runat="server" CssClass="textbox_padrao" MaxLength="100"></asp:TextBox>
                                                        <AjaxToolkit:AutoCompleteExtender ID="aceTituloTecnicoGraduacao" runat="server" TargetControlID="txtTituloTecnicoGraduacao" ServiceMethod="ListarCursoNivel3"></AjaxToolkit:AutoCompleteExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <!-- FIM: Técnico/Graduação -->
                                            </span>
                                            <span class="span4 span4-set-formEsc">
                                                <!--Linha Instituicao Graduação-->
                                                <asp:Label ID="lblInstituicaoTecnicoGraduacao" runat="server" AssociatedControlID="txtInstituicaoTecnicoGraduacao"><strong>Instituição</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <asp:UpdatePanel ID="upInstituicaoTecnicoGraduacao" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtInstituicaoTecnicoGraduacao" runat="server" CssClass="textbox_padrao" Columns="30" MaxLength="100"></asp:TextBox>
                                                        <AjaxToolkit:AutoCompleteExtender ID="aceInstituicaoTecnicoGraduacao" runat="server" TargetControlID="txtInstituicaoTecnicoGraduacao" UseContextKey="false" ServiceMethod="ListarSiglaNomeFonte" OnClientItemSelected="InstituicaoGraduacao"></AjaxToolkit:AutoCompleteExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <!--FIM: Linha Instituicao Graduação-->
                                                <Componentes:BalaoSaibaMais ID="bsmTxtInstituicaoTecnicoGraduacao" runat="server" CssClassLabel="balao_saiba_mais" Text="Saiba mais" ToolTipTitle="Técnico/Graduação em:" ToolTipText="Se você procura por candidatos com Técnico, Tecnólogo ou Graduado. Informe neste campo o nome da formação desejada. Exemplo: Psicologia. <BR />Se desejar candidatos de uma determinada instituição, informe no campo ao lado o nome da Instituição de ensino. Exemplo: Universidade Federal do Paraná." ShowOnMouseover="true" />
                                            </span>
                                        </div>
                                        <div class="span11">
                                            <span class="span5">
                                                <!-- Linha Outros Cursos -->
                                                <asp:Label ID="lblCurso" runat="server" CssClass="label_principal-set" AssociatedControlID="txtTituloCurso"><strong>Outros Cursos</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <asp:UpdatePanel ID="upTituloCurso" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtTituloCurso" runat="server" CssClass="textbox_padrao" MaxLength="100"></asp:TextBox>
                                                        <AjaxToolkit:AutoCompleteExtender ID="aceTituloOutrosCursos" runat="server" TargetControlID="txtTituloCurso" ServiceMethod="ListarCursoNivel4"></AjaxToolkit:AutoCompleteExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <!-- FIM: Outros Cursos -->
                                            </span>
                                            <span class="span4 span4-set-formEsc {width: 350px;}">
                                                <!--Linha Instituicao Outros Cursos-->
                                                <asp:Label ID="lblInstituicao" runat="server" AssociatedControlID="txtInstituicao"><strong>Instituição</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <asp:UpdatePanel ID="upInstituicao" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtInstituicao" runat="server" CssClass="textbox_padrao" Columns="30" MaxLength="100"></asp:TextBox>
                                                        <AjaxToolkit:AutoCompleteExtender ID="aceInstituicaoOutrosCursos" runat="server" TargetControlID="txtInstituicao" UseContextKey="false" ServiceMethod="ListarSiglaNomeFonte" OnClientItemSelected="InstituicaoOutrosCursos"></AjaxToolkit:AutoCompleteExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <Componentes:BalaoSaibaMais ID="bsmTxtInstituicaoOutrosCursos" runat="server" CssClassLabel="balao_saiba_mais" ToolTipText="Se você procura por candidatos com Especialização, Pós graduação ou Cursos distintos. Informe neste campo o nome do curso desejado. Exemplo: Gestão Estratégica de Pessoas. <BR />Se  desejar candidatos de uma determinada instituição, informe no campo ao lado o nome da Instituição de ensino. Exemplo: Pontifícia Universidade Católica do Paraná." ToolTipTitle="Outros Cursos:" Text="Saiba mais" ShowOnMouseover="true" />
                                                <!--FIM: Linha Instituicao Outros Cursos-->
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <%--End Accordion Definir Formação e Escolaridade--%>
                <%--Start Accordion Definir Experiência do Candidato--%>
                <div class="row">
                    <div class="span11">
                        <div class="accordion" id="accordionExperiencia">
                            <div class="accordion-group-bne">
                                <div class="accordion-heading-bne accordion-heading-bne-bg-green">
                                    <a class="accordion-toggle-bne" data-toggle="collapse" data-parent="#accordionExperiencia" href="#collapseExpCand">
                                        <h3 class="colorheader">Experiência do Candidato<i class="fa fa-sort-desc pull-right"></i></h3>
                                    </a>
                                </div>
                                <div id="collapseExpCand" class="accordion-body accordion-body-set collapse in">
                                    <div class="accordion-inner-bne">
                                        <div class="span11">
                                            <span class="span4">
                                                <!-- Linha Atividade Empresa -->
                                                <asp:Label ID="lblAtividadeEmpresa" CssClass="label_principal-set" runat="server" AssociatedControlID="rcbAtividadeEmpresa"><strong>Atividade da Empresa</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <asp:UpdatePanel ID="upAtividadeEmpresa" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                    <ContentTemplate>
                                                        <telerik:RadComboBox ID="rcbAtividadeEmpresa" runat="server"></telerik:RadComboBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <!-- FIM: Linha Atividade Empresa -->
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <%--End Accordion Definir Experiência do Candidato--%>
                <%--Start Accordion Definir Características--%>
                <div class="row">
                    <div class="span11">
                        <div class="accordion" id="accordionCaracteristicas">
                            <div class="accordion-group-bne">
                                <div class="accordion-heading-bne accordion-heading-bne-bg-green">
                                    <a class="accordion-toggle-bne" data-toggle="collapse" data-parent="#accordionCaracteristicas" href="#collapseCaractPessoais">
                                        <h3 class="colorheader">Características<i class="fa fa-sort-desc pull-right"></i></h3>
                                    </a>
                                </div>
                                <div id="collapseCaractPessoais" class="accordion-body accordion-body-set collapse in">
                                    <div class="accordion-inner-bne">
                                        <div class="span11">
                                            <span class="span3">
                                                <!-- Linha Idade -->
                                                <asp:Label ID="Label1" runat="server" AssociatedControlID="txtIdadeDe"><strong>Idade</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <asp:Label ID="lblIdade" runat="server" AssociatedControlID="txtIdadeDe" CssClass="label_principal-set"><strong>Mínima</strong></asp:Label>
                                                <asp:UpdatePanel ID="upIdadeDe" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                    <ContentTemplate>
                                                        <componente:AlfaNumerico ID="txtIdadeDe" runat="server" Obrigatorio="False" CssClassTextBox="textbox_padrao textbox_padrao-set-idade" Columns="2" MaxLength="2" Tipo="Numerico" ValorMinimo="0" ValorMaximo="100" ContemIntervalo="true" ValidationGroup="PesquisaAvancada" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <asp:Label ID="lblIdadeAte" AssociatedControlID="txtIdadeAte" runat="server" CssClass="label_principal-set"><strong>Máxima</strong></asp:Label>
                                                <asp:UpdatePanel ID="upIdadeAte" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                    <ContentTemplate>
                                                        <componente:AlfaNumerico ID="txtIdadeAte" runat="server" Columns="2" MaxLength="2" CssClass="alinhar_container_campo" Obrigatorio="False" CssClassTextBox="textbox_padrao textbox_padrao-set-idade" ValorMinimo="0" ValorMaximo="100" ContemIntervalo="true" Tipo="Numerico" ValidationGroup="PesquisaAvancada" ClientValidationFunction="cvIdade_ClientValidationFunction" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <span runat="server" associatedcontrolid="txtIdadeAte" class="set-label">anos</span>
                                                <!-- FIM: Linha Idade-->
                                            </span>
                                            <span class="span3">
                                                <!-- Linha Sexo -->
                                                <asp:Label runat="server" ID="lblSexo" CssClass="label_principal-set" AssociatedControlID="rcbSexo"><strong>Sexo</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <asp:UpdatePanel ID="upSexo" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                    <ContentTemplate>
                                                        <telerik:RadComboBox ID="rcbSexo" runat="server"></telerik:RadComboBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <!-- FIM: Linha Sexo -->
                                            </span>
                                            <span class="span3">
                                                <!-- Linha Estado Civil -->
                                                <asp:Label ID="lblEstadoCivil" CssClass="label_principal-set" runat="server" AssociatedControlID="rcbEstadoCivil"><strong>Estado Civil</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <asp:UpdatePanel ID="upEstadoCivil" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                    <ContentTemplate>
                                                        <telerik:RadComboBox ID="rcbEstadoCivil" runat="server"></telerik:RadComboBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <!-- FIM: Linha Estado Civil -->
                                            </span>
                                        </div>
                                        <div class="span11">
                                            <span class="span3">
                                                <!-- Linha Raça -->
                                                <asp:Label ID="lblRaca" runat="server" CssClass="label_principal-set" AssociatedControlID="rcbRaca"><strong>Raça</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <asp:UpdatePanel ID="upRaca" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                    <ContentTemplate>
                                                        <telerik:RadComboBox ID="rcbRaca" runat="server"></telerik:RadComboBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <!-- FIM: Linha Raça -->
                                            </span>
                                            <span class="span3">
                                                <!-- Linha Filhos -->
                                                <asp:Label ID="lblFilhos" runat="server" CssClass="label_principal-set" AssociatedControlID="rcbFilhos"><strong>Filhos</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <asp:UpdatePanel ID="upFilhos" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                    <ContentTemplate>
                                                        <telerik:RadComboBox ID="rcbFilhos" runat="server"></telerik:RadComboBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <!-- FIM: Linha Filhos -->
                                            </span>
                                            <span class="span3">
                                                <!-- Linha PCD -->
                                                <asp:Label ID="lblPCD" runat="server" CssClass="label_principal-set" AssociatedControlID="rcbPCD"><strong>Pessoa com Deficiência (PCD - <i class="fa fa-wheelchair fa-1x"></i>)</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <asp:UpdatePanel ID="upPCD" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                    <ContentTemplate>
                                                        <telerik:RadComboBox ID="rcbPCD" runat="server"></telerik:RadComboBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <!-- FIM: Linha PCD -->
                                            </span>
                                        </div>
                                        <div class="span11">
                                            <span class="span3">
                                                <!-- Linha Habilitação-->
                                                <asp:Label ID="lblHabilitacao" CssClass="label_principal-set" runat="server" AssociatedControlID="rcbHabilitacao"><strong>Habilitação</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <asp:UpdatePanel ID="upHabilitacao" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                    <ContentTemplate>
                                                        <telerik:RadComboBox ID="rcbHabilitacao" runat="server"></telerik:RadComboBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </span>
                                            <span class="span3">
                                                <asp:Label ID="lblVeiculo" CssClass="label_principal-set" runat="server" AssociatedControlID="rcbVeiculo"><strong>Veículo</strong></asp:Label>
                                                <div class="clearfix"></div>
                                                <asp:UpdatePanel ID="upVeiculo" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                    <ContentTemplate>
                                                        <telerik:RadComboBox ID="rcbVeiculo" runat="server"></telerik:RadComboBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <!-- FIM: Linha Habilitação -->
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <%--End Accordion Definir Características--%>
                <%--Start Accordion Definir Notificacao--%>
                <div class="row">
                    <div class="span11">
                        <div class="accordion" id="accordionNotificacao">
                            <div class="accordion-group-bne">
                                <div class="accordion-heading-bne accordion-heading-bne-bg-green">
                                    <a class="accordion-toggle-bne" data-toggle="collapse" data-parent="#accordionNotificacao" href="#collapseExpCand">
                                        <h3 class="colorheader">Forma de Notificação<i class="fa fa-sort-desc pull-right"></i></h3>
                                    </a>
                                </div>
                                <div class="accordion-body accordion-body-set collapse in">
                                    <div class="accordion-inner-bne">
                                        <div class="span11">
                                            <span class="span4">
                                                <asp:Label ID="lblNotificarUmaVezPorDia" CssClass="label_principal-set" runat="server" AssociatedControlID="ckbNotificarUmaVezPorDia"><strong>Notificar uma vez ao dia</strong></asp:Label>
                                                <asp:UpdatePanel ID="upNotificarUmaVezPorDia" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                    <ContentTemplate>
                                                        <asp:CheckBox runat="server" ID="ckbNotificarUmaVezPorDia" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </span>
                                            <br /><br />
                                            <span class="span4">
                                                <asp:Label ID="lblNotificarUmaVezPorHora" CssClass="label_principal-set" runat="server" AssociatedControlID="ckbNotificarUmaVezPorHora"><strong>Notificar na hora</strong></asp:Label>
                                                <asp:UpdatePanel ID="upNotificarUmaVezPorHora" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                    <ContentTemplate>
                                                        <asp:CheckBox runat="server" ID="ckbNotificarUmaVezPorHora" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <%--End Accordion Definir Notificacao--%>
            </asp:Panel>
            <div class="painel_botoes">
                <asp:Button
                    ID="btnSalvar"
                    runat="server"
                    CssClass="botao_padrao"
                    Text="Salvar"
                    ValidationGroup="SalvarRastreador"
                    CausesValidation="true"
                    OnClick="btnSalvar_Click" />
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlGrid" runat="server" CssClass="uctabs_sala_selecionador">
            <asp:UpdatePanel
                ID="upGvRastreadorCurriculos"
                runat="server"
                UpdateMode="Conditional">
                <ContentTemplate>
                    <telerik:RadGrid
                        ID="gvRastreadorCurriculos"
                        AllowPaging="True"
                        AllowCustomPaging="true"
                        CssClass="gridview_padrao"
                        runat="server"
                        Skin="Office2007"
                        GridLines="None"
                        OnItemCommand="gvRastreadorCurriculos_ItemCommand"
                        OnPageIndexChanged="gvRastreadorCurriculos_PageIndexChanged">
                        <PagerStyle
                            Mode="NextPrevNumericAndAdvanced"
                            Position="TopAndBottom" />
                        <AlternatingItemStyle
                            CssClass="alt_row" />
                        <MasterTableView
                            DataKeyNames="Idf_Rastreador_Curriculo">
                            <Columns>
                                <telerik:GridTemplateColumn
                                    HeaderText="Função"
                                    ItemStyle-CssClass="descricao_vaga2">
                                    <ItemTemplate>
                                        <%--<asp:Literal ID="litAntesDoTituloVaga" runat="server" Text="Estagiário para " Visible='<%# DataBinder.Eval(Container.DataItem, "Des_Funcao").ToString() != "Estagiário" && !string.IsNullOrWhiteSpace(DataBinder.Eval(Container.DataItem, "Idf_Escolaridade_WebStagio").ToString())%>'></asp:Literal>--%>
                                        <asp:Label
                                            ID="lblDescricaoVaga"
                                            runat="server"
                                            Text='<%#  String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Des_Funcao").ToString()) ? "<i>Não Informado</i>" : Eval("Des_Funcao") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn
                                    HeaderText="Cidade"
                                    ItemStyle-CssClass="cidade">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblNmeCidade"
                                            runat="server"
                                            Text='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Nme_Cidade").ToString()) ? "<i>Não Informado</i>" : Eval("Nme_Cidade") + "/" + Eval("Sig_Estado") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn
                                    HeaderText="Currículos Rastreados"
                                    ItemStyle-CssClass="cv_recebida centro">
                                    <ItemTemplate>
                                        <asp:LinkButton
                                            ID="btiVisualizarCurriculo"
                                            runat="server"
                                            ToolTip="Visualizar Currículo"
                                            AlternateText="Visualizar Currículo"
                                            CommandName="VisualizarCurriculo"
                                            Text='<%# Eval("Qtd_Curriculo") %>'
                                            CausesValidation="false" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn
                                    HeaderText="Novos Currículos"
                                    ItemStyle-CssClass="cv_recebida centro">
                                    <ItemTemplate>
                                        <asp:LinkButton
                                            ID="btiVisualizarCurriculoNaoVisualizado"
                                            runat="server"
                                            ToolTip="Visualizar Currículos Não Visulizados"
                                            AlternateText="Visualizar Currículos Não Visulizados"
                                            CommandName="VisualizarCurriculoNaoVisualizado"
                                            Text='<%# RecuperarQuantidadeNaoVisualizado(Convert.ToInt32(Eval("Idf_Rastreador_Curriculo"))) %>'
                                            CausesValidation="false" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn
                                    HeaderText="Ações"
                                    ItemStyle-CssClass="col_action">
                                    <ItemTemplate>
                                        <asp:ImageButton
                                            ID="btiEditar"
                                            runat="server"
                                            ImageUrl="/img/icn_editar_lapis.png"
                                            ToolTip="Editar Rastreador"
                                            AlternateText="Editar Rastreador"
                                            CommandName="EditarRastreador"
                                            CausesValidation="false" />
                                        <asp:ImageButton
                                            ID="btiExcluir"
                                            runat="server"
                                            ImageUrl="/img/icn_excluirvaga.png"
                                            ToolTip="Excluir"
                                            AlternateText="Excluir Rastreador"
                                            CommandName="ExcluirRastreador"
                                            CausesValidation="false" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
        <div class="painel_botoes">
            <asp:Button
                ID="btnVoltar"
                runat="server"
                CssClass="botao_padrao"
                Text="Voltar"
                CausesValidation="false"
                OnClick="btnVoltar_Click" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
    <usercontrol:Confirmacao ID="ucConfirmacao" runat="server" />
    <usercontrol:ConfirmacaoExclusao ID="ucConfirmacaoExclusao" runat="server" />
    <Employer:DynamicScript runat="server" Src="/js/local/SalaSelecionadorRastreadorCurriculos.js" type="text/javascript" />
</asp:Content>
