<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="AnunciarVaga.aspx.cs" Inherits="BNE.Web.AnunciarVaga" %>

<%@ Register Src="UserControls/Modais/ModalConfirmacao.ascx" TagName="ModalConfirmacao"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ContratoFuncao.ascx" TagName="ucContrato" TagPrefix="uc2" %>
<%@ Register Src="UserControls/Modais/EnvioEmail.ascx" TagName="ModalEnvioEmail" TagPrefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <%--JS--%>
    <Employer:DynamicScript runat="server" Src="/js/local/AnunciarVaga.js" type="text/javascript" />
    <%--CSS--%>
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/Sala_Selecionador.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/AnunciarVagas.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/BannerPromocionaisR1Vagas/banners.css" type="text/css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <asp:UpdatePanel ID="upAnunciarVaga" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlAnunciarVaga" runat="server">
                <div class="painel_padrao_sala_selecionador">
                    <p>
                        Os candidatos serão avisados sobre seu anúncio e você receberá o currículo dos interessados
                    </p>
                    <img src="img/anunciarVagaImg.png" title="Clique para fechar!" class="anunciaVagaIncentivo" id="imgAmigavelAnuncioVaga" onclick="ocultarImgAmigavel();" alt="" />
                    <asp:UpdatePanel ID="upContratoFuncao" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <uc2:ucContrato Obrigatorio="true" ImagemPara="Empresa" OnClientFuncaoChange="Cidade_OnChange()" ValidationGroup="ValidationAvancar" runat="server" ID="ucContratoFuncao" OnClientFuncaoLostFocus="Funcao_LostFocus($(this));" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="linha">
                        <asp:Label ID="lblCidade" CssClass="label_principal" AssociatedControlID="txtCidadeAnunciarVaga"
                            runat="server" Text="Cidade"></asp:Label>
                        <div class="container_campo">
                            <div>
                                <asp:RequiredFieldValidator ID="rfCidade" ValidationGroup="ValidationAvancar" runat="server"
                                    ControlToValidate="txtCidadeAnunciarVaga"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvCidade" runat="server" ErrorMessage="Cidade Inválida."
                                    ClientValidationFunction="cvCidade_Validate" ControlToValidate="txtCidadeAnunciarVaga"
                                    ValidationGroup="ValidationAvancar"></asp:CustomValidator>
                            </div>
                            <asp:TextBox ID="txtCidadeAnunciarVaga" runat="server"></asp:TextBox>
                            <AjaxToolkit:AutoCompleteExtender ID="aceCidade" runat="server" TargetControlID="txtCidadeAnunciarVaga"
                                UseContextKey="True" ServiceMethod="ListarCidadesSiglaEstadoPorNomeParcialEstado">
                            </AjaxToolkit:AutoCompleteExtender>
                        </div>
                    </div>
                    <div class="linha">
                        <asp:Label ID="lblNumeroVagas" CssClass="label_principal" Text="Número de Vagas"
                            AssociatedControlID="txtNumeroVagas" runat="server"></asp:Label>
                        <div class="container_campo">
                            <componente:AlfaNumerico ID="txtNumeroVagas" MaxLength="2" ContemIntervalo="true"
                                ValorMaximo="99" ValorMinimo="1" ValidationGroup="ValidationAvancar" MensagemErroIntervalo="O numero de vagas deve ser maior que zero"
                                Obrigatorio="true" runat="server" Tipo="Numerico" Columns="2" />
                        </div>
                    </div>
                    <div class="linha">
                        <asp:Label ID="lblEscolaridade" CssClass="label_principal" Text="Escolaridade" AssociatedControlID="rcbEscolaridade"
                            runat="server"></asp:Label>
                        <div class="container_campo">
                            <telerik:RadComboBox ID="rcbEscolaridade" runat="server" CssClass="campoEscolaridade" OnClientSelectedIndexChanged="ValidarTipoContratoComEscolaridade">
                            </telerik:RadComboBox>
                        </div>
                    </div>
                    <div class="linha">
                        <asp:Label ID="lblSalario" CssClass="label_principal" Text="Faixa Salarial" AssociatedControlID="txtSalarioDe"
                            runat="server"></asp:Label>
                        <div class="container_campo">
                            <asp:UpdatePanel ID="upFaixaSalarial" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <span>De</span>
                                    <componente:ValorDecimal ID="txtSalarioDe" Obrigatorio="false" CssClassTextBox="textbox_padrao"
                                        runat="server" CasasDecimais="2" OnValorAlterado="txtSalarioDe_ValorAlterado"
                                        ValidationGroup="ValidationAvancar" />
                                    <span>até</span>
                                    <componente:ValorDecimal ID="txtSalarioAte" Obrigatorio="false" CssClassTextBox="textbox_padrao"
                                        runat="server" CasasDecimais="2" OnValorAlterado="txtSalarioAte_ValorAlterado"
                                        ValidationGroup="ValidationAvancar" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnAnunciarVaga" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div id="faixa_salarial" runat="server" class="hint_explicativo">
                        </div>
                        <asp:HiddenField ID="hfFaixaSalarial" runat="server" />
                    </div>
                    <div class="linha">
                        <asp:Label ID="lblBeneficios" CssClass="label_principal" Text="Benefícios" AssociatedControlID="txtBeneficios"
                            runat="server"></asp:Label>
                        <div class="container_campo">
                            <componente:AlfaNumerico ID="txtBeneficios" MaxLength="2000" Obrigatorio="false"
                                CssClassTextBox="textbox_padrao" runat="server" Tipo="AlfaNumerico" />
                        </div>
                    </div>
                    <div class="linha">
                        <asp:Label ID="lblIdade" CssClass="label_principal" Text="Idade" AssociatedControlID="txtIdadeMinima"
                            runat="server"></asp:Label>
                        <div class="container_campo">
                            <componente:AlfaNumerico ID="txtIdadeMinima" MaxLength="2" Obrigatorio="false" CssClassTextBox="textbox_padrao"
                                runat="server" Tipo="Numerico" Columns="3" ContemIntervalo="True" />
                            <asp:Label ID="lblAte" Text="até" runat="server"></asp:Label>
                            <componente:AlfaNumerico ID="txtIdadeMaxima" Obrigatorio="false" CssClassTextBox="textbox_padrao"
                                MaxLength="2" runat="server" Tipo="Numerico" Columns="3" ContemIntervalo="True" />
                        </div>
                        <Componentes:BalaoSaibaMais ID="bsmIdade" runat="server" ToolTipText="Escolha a idade mínima que o candidato deverá ter para ocupar sua vaga. Essa informação não será divulgada no anúncio da vaga e servirá como filtro para o recebimento dos currículos."
                            Text="Saiba mais" ToolTipTitle="Idade:" CssClassLabel="balao_saiba_mais" ShowOnMouseover="true" />
                    </div>
                    <div class="linha">
                        <asp:Label ID="lblSexo" CssClass="label_principal" Text="Sexo" AssociatedControlID="rcbSexo"
                            runat="server"></asp:Label>
                        <div class="container_campo">
                            <telerik:RadComboBox ID="rcbSexo" runat="server">
                            </telerik:RadComboBox>
                        </div>
                        <Componentes:BalaoSaibaMais ID="bsmSexo" runat="server" ToolTipText="Escolha o sexo desejado que o candidato deva ter para ocupar sua vaga. Essa informação não será divulgada no anúncio e servirá como filtro para o recebimento dos currículos."
                            Text="Saiba mais" ToolTipTitle="Sexo:" CssClassLabel="balao_saiba_mais" ShowOnMouseover="True" />
                    </div>
                    <div class="linha">
                        <asp:Label ID="lblRequisitos" CssClass="label_principal" Text="Requisitos" AssociatedControlID="txtRequisitos"
                            runat="server"></asp:Label>
                        <div class="container_campo">
                            <asp:TextBox ID="txtRequisitos" runat="server" CssClass="textarea_padrao" Columns="99999"
                                TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                    <div class="linha">
                        <asp:Label ID="lblAtribuicoes" CssClass="label_principal" Text="Atribuições (Principais Tarefas)"
                            AssociatedControlID="txtAtribuicoes" runat="server"></asp:Label>
                        <div class="container_campo">
                            <asp:TextBox ID="txtAtribuicoes" runat="server" CssClass="textarea_padrao" Columns="99999"
                                TextMode="MultiLine"></asp:TextBox>
                        </div>
                        <asp:Panel runat="server" CssClass="BoxSugestaoTarefas">
                            <div class="seta_apontador_esq">
                            </div>
                            <div class="box_conteudo sugestao">
                                <asp:Label ID="lblSugestaoTarefasAnunciarVaga" runat="server" CssClass="label_principal"
                                    AssociatedControlID="txtSugestaoTarefasAnunciarVaga" Text="Sugestão de Tarefas"></asp:Label>
                                <div class="container_campo">
                                    <asp:UpdatePanel ID="upTxtSugestaoTarefasAnunciarVaga" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtSugestaoTarefasAnunciarVaga" CssClass="textbox_padrao sugestao_tarefas"
                                                TextMode="MultiLine" ReadOnly="true" runat="server"></asp:TextBox>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="linha">
                        <asp:Label ID="lblDisponibilidade" CssClass="label_principal" Text="Disponibilidade de Trabalho"
                            AssociatedControlID="rcbDisponibilidade" runat="server"></asp:Label>
                        <div class="container_campo">
                            <asp:UpdatePanel ID="upRcbDisponibilidade" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <Employer:ComboCheckbox ID="rcbDisponibilidade" EmptyMessage="Qualquer" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="linha">
                        <asp:Label ID="lblDeficiencia" CssClass="label_principal" runat="server" Text="Pessoa com Deficiência (PCD)"
                            AssociatedControlID="ckbDeficiencia" />
                        <div class="container_campo">
                            <asp:CheckBox runat="server" ID="ckbDeficiencia" OnCheckedChanged="ckbDeficiencia_CheckedChanged"
                                AutoPostBack="True" />
                            <asp:UpdatePanel runat="server" ID="upTipoDeficiencia" UpdateMode="Conditional" RenderMode="Inline">
                                <ContentTemplate>
                                    <asp:Panel runat="server" ID="pnlTipoDeficiencia" Visible="False">
                                        <asp:Label ID="lblTipoDeficiencia" CssClass="label_principal" runat="server" Text="Tipo de Deficiência"
                                            AssociatedControlID="ddlTipoDeficiencia" />
                                        <asp:DropDownList CssClass="textbox_padrao" ID="ddlTipoDeficiencia" runat="server">
                                        </asp:DropDownList>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <%--dados para confidencialidade ou não da empresa--%>
                    <div class="dados_confidenciais">
                        <div class="coluna_label">
                            <asp:Label ID="lblNomeFantasia" CssClass="label_principal label_confidencial" Text="Nome Fantasia"
                                AssociatedControlID="txtNomeFantasia" runat="server"></asp:Label>
                            <asp:Label ID="lblTelefone" CssClass="label_principal label_confidencial" Text="Telefone"
                                AssociatedControlID="txtTelefone" runat="server"></asp:Label>
                        </div>
                        <div class="coluna_confidencial">
                            <div class="container_campo confidencial">
                                <div>
                                    <asp:TextBox ID="txtNomeFantasia" CssClass="textbox_padrao" runat="server"></asp:TextBox>
                                    <asp:CheckBox ID="cbConfidencial" runat="server" Text="Confidencial" CssClass="chk_confidencial" />
                                </div>
                                <div>
                                    <componente:Telefone ID="txtTelefone" Obrigatorio="false" CssClassTextBoxFone="textbox_padrao"
                                        CssClassTextBoxDDD="textbox_padrao" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <%-- fim dados para confidencialidade ou não da empresa--%>
                    <div class="linha">
                        <asp:Label ID="lblEmail" CssClass="label_principal" Text="E-mail confidencial para retorno"
                            AssociatedControlID="txtEmail" runat="server"></asp:Label>
                        <div class="container_campo">
                            <div>
                                <asp:RequiredFieldValidator ID="rfEmail" runat="server" ValidationGroup="ValidationAvancar"
                                    ControlToValidate="txtEmail"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revEmail" runat="server" ValidationGroup="ValidationAvancar"
                                    ControlToValidate="txtEmail" ErrorMessage="Email Inválido"></asp:RegularExpressionValidator>
                            </div>
                            <asp:TextBox ID="txtEmail" CssClass="textbox_padrao campo_obrigatorio" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="chk_info_vagas">
                        <div class="linha">
                            <div class="container_campo">
                                <asp:CheckBox ID="cbReceberCadaCVEmail" runat="server" Text="Receber e-mail para cada pessoa candidatada." />
                            </div>
                        </div>
                        <div class="linha">
                            <div class="container_campo">
                                <asp:CheckBox ID="cbReceberTodosCvsEmail" runat="server" Text="Receber e-mail diário com todas as pessoas candidatadas."
                                    Checked="true" />
                            </div>
                        </div>
                    </div>
                    <div class="linha">
                        <asp:Label ID="Label1" CssClass="label_principal" Text="Palavras Chave" AssociatedControlID="txtPalavrasChave"
                            runat="server"></asp:Label>
                        <div class="container_campo">
                            <componente:AlfaNumerico ID="txtPalavrasChave" runat="server" CssClassTextBox="textarea_padrao"
                                TextMode="MultiLine" MaxLength="500" Columns="500"></componente:AlfaNumerico>
                            <asp:Label ID="Label3" runat="server" Text="Insira até 15 palavras chaves separadas por vírgula (,)"
                                CssClass="texto_exemplo"></asp:Label>
                        </div>
                    </div>
                </div>
                <h2>
                    <asp:Label ID="lblSubTitulo" runat="server" Text="Perguntas para Pré-Seleção"></asp:Label>
                </h2>
                <div class="painel_padrao_sala_selecionador">
                    <asp:Panel CssClass="painel_padrao" runat="server">
                        <div class="linha texto_complementar">
                            <asp:Label ID="Label2" runat="server" Text="* Para auxiliar o recebimento de currículos dentro do perfil desejado, você tem a opção de fazer 4 perguntas fechadas (respostas sim ou não) para aqueles que se candidatarem. Os candidatos que informarem a resposta igual a sua serão classificados como dentro do perfil."></asp:Label>
                        </div>
                        <div class="linha">
                            <asp:Label ID="lblPergunta" runat="server" AssociatedControlID="txtPergunta" Text="Pergunta"
                                CssClass="label_principal"></asp:Label>
                            <div class="container_campo">
                                <asp:UpdatePanel ID="upPergunta" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtPergunta" MaxLength="140" CssClass="textbox_padrao" runat="server"></asp:TextBox>
                                        <asp:Label ID="lblExemplo" runat="server" Text="Ex:Tem interesse em trabalhar no comércio?"
                                            CssClass="texto_exemplo"></asp:Label>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnAdicionarPergunta" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <asp:Panel ID="pnlRespostaObjetiva" runat="server">
                            <div class="linha texto_complementar">
                                <asp:Label ID="lblTituloSelecionarPergunta" runat="server" Text="Selecione abaixo a melhor resposta para sua pergunta"></asp:Label>
                            </div>
                            <div class="linha">
                                <asp:UpdatePanel ID="upRespostas" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <asp:Label ID="lblResposta" runat="server" AssociatedControlID="rbSim" Text="Resposta"
                                            CssClass="label_principal">
                                        </asp:Label>
                                        <div class="container_campo">
                                            <asp:RadioButton ID="rbSim" runat="server" Text="Sim" AutoPostBack="true" GroupName="Radios" />
                                            <asp:RadioButton ID="rbNao" runat="server" AutoPostBack="true" Text="Não" GroupName="Radios" />
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnAdicionarPergunta" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="pnlPanelAdicionarPergunta" runat="server" CssClass="painel_botoes add_pergunta">
                        <asp:Button ID="btnAdicionarPergunta" CssClass="botao_padrao" CausesValidation="false"
                            runat="server" Text="Adicionar Pergunta" OnClick="btnAdicionarPergunta_Click" />
                    </asp:Panel>
                    <asp:UpdatePanel ID="upGvPerguntasVaga" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <telerik:RadGrid ID="gvPerguntasVaga" runat="server" AllowSorting="False" OnItemCommand="gvPerguntasVaga_ItemCommand"
                                AllowPaging="False" AllowCustomPaging="False">
                                <MasterTableView TableLayout="Fixed" DataKeyNames="Idf_Vaga_Pergunta" OverrideDataSourceControlSorting="true">
                                    <Columns>
                                        <telerik:GridBoundColumn HeaderText="Pergunta" HeaderStyle-CssClass="rgHeader" ItemStyle-CssClass="pergunta"
                                            DataField="Des_Vaga_Pergunta" />
                                        <telerik:GridBoundColumn HeaderText="Melhor Resposta" HeaderStyle-CssClass="rgHeader melhor_resposta centro"
                                            ItemStyle-CssClass="melhor_resposta centro " DataField="Flg_Resposta" />
                                        <telerik:GridTemplateColumn HeaderText=" " HeaderStyle-CssClass="rgHeader excluir"
                                            ItemStyle-CssClass="excluir">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btiExcluir" runat="server" AlternateText="Excluir" CausesValidation="False"
                                                    CommandName="Delete" ImageUrl="~/img/icone_excluir_16x16.png" ToolTip="Excluir Pergunta" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <asp:Panel ID="pnlPublicarAnunciarVaga" runat="server">
                    <div class="">
                        <div class="chk_info_vagas">
                            <div class="linha">
                                <div class="container_campo">
                                    <asp:CheckBox ID="chkPublicarVaga" runat="server" Text="Publicar Vaga" />
                                </div>
                            </div>
                            <div class="linha">
                                <div class="container_campo">
                                    <asp:CheckBox ID="chkLiberarVaga" runat="server" Text="Liberar Vaga" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:Panel ID="pnlBotoesAdm" runat="server" CssClass="painel_botoes">
                        <asp:Button ID="btnSalvarAdm" runat="server" Text="Salvar" CausesValidation="true"
                            CssClass="botao_padrao" ValidationGroup="ValidationAvancar" OnClick="btnSalvarAdm_Click" />
                        <asp:Button ID="btnArquivarAdm" runat="server" Text="Arquivar Vaga" CausesValidation="false"
                            CssClass="botao_padrao" OnClick="btnArquivarAdm_Click" />
                        <asp:Button ID="btnVoltarAdm" runat="server" Text="Voltar" CausesValidation="false"
                            CssClass="botao_padrao" OnClick="btnVoltarAdm_Click" />
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
                    <asp:Button ID="btnAvancar" runat="server" Text="Anunciar >>" CausesValidation="true"
                        CssClass="botao_padrao verde" ValidationGroup="ValidationAvancar" OnClick="btnAvancar_Click"
                        OnClientClick="javascript: AjustarScroll();" />
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upAnunciarVagaConferir" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlAnunciarVagaConferir" runat="server">
                <div class="painel_padrao_sala_selecionador">
                    <asp:Panel CssClass="painel_padrao" runat="server">
                        <h2 class="titulo_interno">Conferir</h2>
                        <div id="divTipoContratoConfirmacao" class="linha" runat="server">
                            <asp:Label ID="lblTipoContratoConfirmacao" CssClass="label_principal negrito" Text="Tipo de Contrato"
                                AssociatedControlID="lblTipoContratoConfirmacaoValor" runat="server"></asp:Label>
                            <div class="container_campo">
                                <asp:Label ID="lblTipoContratoConfirmacaoValor" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div id="divFuncaoConfirmacao" class="linha" runat="server">
                            <asp:Label ID="lblFuncaoConfirmacao" CssClass="label_principal negrito" Text="Funcão"
                                AssociatedControlID="lblFuncaoConfirmacaoValor" runat="server"></asp:Label>
                            <div class="container_campo">

                                <asp:Label ID="lblFuncaoConfirmacaoValor" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div id="divCidadeConfirmacao" class="linha" runat="server">
                            <asp:Label ID="lblCidadeConfirmacao" CssClass="label_principal negrito" Text="Cidade"
                                AssociatedControlID="lblCidadeConfirmacaoValor" runat="server"></asp:Label>
                            <div class="container_campo">
                                <asp:Label ID="lblCidadeConfirmacaoValor" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div id="divEscolaridadeConfirmacao" class="linha" runat="server">
                            <asp:Label ID="lblEscolaridadeConfirmacao" CssClass="label_principal negrito" Text="Escolaridade"
                                AssociatedControlID="lblEscolaridadeConfirmacaoValor" runat="server"></asp:Label>
                            <div class="container_campo">
                                <asp:Label ID="lblEscolaridadeConfirmacaoValor" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div id="divIdadeConfirmacao" class="linha" runat="server">
                            <asp:Label ID="lblIdadeConfirmacao" CssClass="label_principal negrito" Text="Idade"
                                AssociatedControlID="lblIdadeConfirmacaoValor" runat="server"></asp:Label>
                            <div class="container_campo">
                                <asp:Label ID="lblIdadeConfirmacaoValor" runat="server"></asp:Label>
                            </div>
                            <asp:Label ID="lblDescricaoIdade" runat="server" CssClass="hint_explicativo" Text="Este dado não aparecerá no anúncio, servirá como filtro para o recebimento de currículos."></asp:Label>
                        </div>
                        <div id="divSexoConfirmacao" class="linha" runat="server">
                            <asp:Label ID="lblSexoConfirmacao" CssClass="label_principal negrito" Text="Sexo"
                                AssociatedControlID="lblSexoConfirmacaoValor" runat="server"></asp:Label>
                            <div class="container_campo">
                                <asp:Label ID="lblSexoConfirmacaoValor" runat="server"></asp:Label>
                            </div>
                            <asp:Label ID="lblDescricaoSexo" runat="server" CssClass="hint_explicativo" Text="Este dado não aparecerá no anúncio, servirá como filtro para o recebimento de currículos."></asp:Label>
                        </div>
                        <div id="divSalarioConfirmacao" class="linha" runat="server">
                            <asp:Label ID="lblSalarioConfirmacao" CssClass="label_principal negrito" Text="Faixa Salarial"
                                AssociatedControlID="lblSalarioConfirmacaoValor" runat="server"></asp:Label>
                            <div class="container_campo">
                                <asp:Label ID="lblSalarioConfirmacaoValor" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div id="divBeneficiosConfirmacao" class="linha" runat="server">
                            <asp:Label ID="lblBeneficiosConfirmacao" CssClass="label_principal negrito" Text="Benefícios"
                                AssociatedControlID="lblBeneficiosConfirmacaoValor" runat="server"></asp:Label>
                            <div class="container_campo">
                                <asp:Label ID="lblBeneficiosConfirmacaoValor" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div id="divNumeroVagasConfirmacao" class="linha" runat="server">
                            <asp:Label ID="lblNumeroVagasConfirmacao" CssClass="label_principal negrito" Text="Número de Vagas"
                                AssociatedControlID="lblNumeroVagasConfirmacaoValor" runat="server"></asp:Label>
                            <div class="container_campo">
                                <asp:Label ID="lblNumeroVagasConfirmacaoValor" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div id="divHorarioConfirmacao" class="linha" runat="server">
                            <asp:Label ID="lblHorarioConfirmacao" CssClass="label_principal negrito" Text="Horário de Trabalho"
                                AssociatedControlID="lblHorarioConfirmacaoValor" runat="server"></asp:Label>
                            <div class="container_campo">
                                <asp:Label ID="lblHorarioConfirmacaoValor" runat="server"></asp:Label>
                            </div>
                        </div>

                        <div id="divRequisitoConfirmacao" class="linha" runat="server">
                            <asp:Label ID="lblRequisitoConfirmacao" CssClass="label_principal negrito" Text="Requisitos"
                                AssociatedControlID="lblRequisitoConfirmacaoValor" runat="server"></asp:Label>
                            <div class="container_campo">
                                <asp:Label ID="lblRequisitoConfirmacaoValor" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div id="divAtribuicoesConfirmacao" class="linha" runat="server">
                            <asp:Label ID="lblAtribuicoesConfirmacao" CssClass="label_principal negrito" Text="Atribuição"
                                AssociatedControlID="lblAtribuicoesConfirmacaoValor" runat="server"></asp:Label>
                            <div class="container_campo">
                                <asp:Label ID="lblAtribuicoesConfirmacaoValor" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div id="divDeficienciaConfirmacao" class="linha" runat="server">
                            <asp:Label ID="lblDeficienciaConfirmacao" CssClass="label_principal negrito" Text="Deficiência"
                                AssociatedControlID="lblDeficienciaConfirmacao" runat="server"></asp:Label>
                            <div class="container_campo">
                                <asp:Label ID="lblDeficienciaConfirmacaoValor" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div id="divNomeFantasiaConfirmacao" class="linha" runat="server">
                            <asp:Label ID="lblNomeFantasiaConfirmacao" CssClass="label_principal negrito" Text="Nome Fantasia"
                                AssociatedControlID="lblNomeFantasiaConfirmacaoValor" runat="server"></asp:Label>
                            <div class="container_campo">
                                <asp:Label ID="lblNomeFantasiaConfirmacaoValor" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div id="divTelefoneConfirmacao" class="linha" runat="server">
                            <asp:Label ID="lblTelefoneConfirmacao" CssClass="label_principal negrito" Text="Telefone"
                                AssociatedControlID="lblTelefoneConfirmacaoValor" runat="server"></asp:Label>
                            <div class="container_campo">
                                <asp:Label ID="lblTelefoneConfirmacaoValor" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div id="divEmailConfirmacao" class="linha" runat="server">
                            <asp:Label ID="lblEmailConfirmacao" CssClass="label_principal negrito" Text="E-mail para retorno"
                                AssociatedControlID="lblEmailConfirmacaoValor" runat="server"></asp:Label>
                            <div class="container_campo">
                                <asp:Label ID="lblEmailConfirmacaoValor" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="chk_info_vagas">
                            <div class="linha">
                                <div class="container_campo">
                                    <asp:CheckBox ID="cbReceberCadaCVEmailConfirmacao" Enabled="false" runat="server"
                                        Text="Receber e-mail para cada pessoa candidata." />
                                </div>
                            </div>
                            <div class="linha">
                                <div class="container_campo">
                                    <asp:CheckBox ID="cbReceberTodosCvsEmailConfirmacao" Enabled="false" runat="server"
                                        Text="Receber e-mail diário com todas as pessoas candidatas." />
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
                <h2>Perguntas para Pré-Seleção</h2>
                <div class="painel_padrao_sala_selecionador">
                    <asp:UpdatePanel ID="upGvAnunciarVagasConfirmacao" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <telerik:RadGrid ID="gvAnunciarVagasConfirmacao" runat="server" AllowSorting="False"
                                AllowPaging="False" AllowCustomPaging="False">
                                <MasterTableView TableLayout="Fixed" OverrideDataSourceControlSorting="true">
                                    <Columns>
                                        <telerik:GridBoundColumn HeaderText="Pergunta" HeaderStyle-CssClass="rgHeader" DataField="Des_Vaga_Pergunta" />
                                        <telerik:GridBoundColumn HeaderText="Melhor Resposta" HeaderStyle-CssClass="rgHeader melhor_resposta"
                                            ItemStyle-CssClass="melhor_resposta" DataField="Flg_Resposta" />
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <asp:Panel ID="pnlBotoesAnunciarVagaConfirmacao" runat="server" CssClass="painel_botoes">
                    <asp:Button ID="btnAnunciarVaga" runat="server" Text="Anunciar Vaga" CssClass="botao_padrao"
                        CausesValidation="false" OnClick="btnAnunciarVaga_Click" OnClientClick="javascript: AjustarScroll();" />
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upAnunciarVagaConfirmacao" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlAnunciarVagaConfirmacao" runat="server">
                <div class="painel_padrao_sala_selecionador_customizado">
                    <div class="esquerda_sucesso">
                        <h2 class="titulo_sucesso">Vaga Cadastrada com Sucesso!</h2>
                        <i class="fa fa-check-circle icone_sucesso"></i> 
                        <p>
                            Código da Vaga <b>
                                <asp:Literal ID="litCodVaga" runat="server"></asp:Literal></b>.<br />
                            Sua vaga foi cadastrada com sucesso<br />
                            e será publicada em breve.
                        </p>
                    </div>
                    <%--Start Banners Webestágios R1 e Divulgação de Vaga--%>
                    <asp:Panel ID="pnlImgWebEstagio" runat="server" CssClass="thumbnail2" Visible="false">
                        <img src="img/bannersR1Vagas/imgWebestag.png" alt="" />
                        <div class="caption" style="padding-bottom: 20px">
                            <div class="titulo">
                                A solução completa na administração de estágios!
                            </div>
                            <blockquote>
                                <p class="metric">
                                    <i style="color:#18e14e;" class="fa fa-check"></i>Formato <strong>diferenciado</strong>.
                                </p>
                                <p class="metric">
                                    <i style="color:#18e14e;" class="fa fa-check"></i>Ferramentas <strong>exclusivas</strong>.
                                </p>
                                <p class="metric">
                                    <i style="color:#18e14e;" class="fa fa-check"></i>Conceitos <strong>modernos</strong> na gestão de estagiários.
                                </p>
                                <br />

                            </blockquote>
                            <p class="align">
                                <asp:Button ID="btQueroContratarWebEstagios" Text="Receba mais informações!" runat="server" CssClass="btn btn-primary btComprar" OnClick="btQueroContratarWebEstagios_Click" />
                            </p>
                        </div>
                    </asp:Panel>

                    <asp:Panel ID="pnlImgmVendaSMS" runat="server" CssClass="thumbnail2" Visible="false">
                        <img src="img/bannersR1Vagas/imgVagas.png" title="Divulgamos sua Vaga na Hora!" alt="Divulgamos sua Vaga na Hora!">
                        <div class="caption">
                            <div class="titulo">
                                Divulgamos sua Vaga na Hora!
                            </div>
                            <blockquote>
                                <p class="metric">
                                    <i style="color:#18e14e;" class="fa fa-check"></i>Envio imediato de <strong>50 SMS</strong> + <strong>150 e-mails</strong>, para candidatos no perfil da sua vaga.
                                </p>
                                <p class="metric">
                                    <i style="color:#18e14e;" class="fa fa-check"></i>Receba até <strong>5x mais</strong> candidatos por vaga.
                                </p>
                                <p class="beforePrice">
                                    <small>De R$</small>
                                    29,90
                                <p class="afterPrice">
                                    <small>por R$</small> 23,90
                                </p>
                                <p></p>
                                <p></p>
                            </blockquote>
                            <p class="align button">
                                <asp:Button ID="btComprar" runat="server" CssClass="btn btn2 btn-primary btComprar" Text="COMPRAR" OnClick="btComprar_Click" />
                            </p>
                        </div>
                    </asp:Panel>

                    <asp:Panel ID="pnlImgR1" runat="server" CssClass="thumbnail2" Visible="false">
                        <asp:ImageButton runat="server" ID="imgR1" OnClick="imgR1_Click" AlternateText="Serviço de Recrutamento R1" title="Serviço de Recrutamento R1" ImageUrl="img/bannersR1Vagas/imgR1.png" />
                        <%--<img src="img/bannersR1Vagas/imgR1.png" alt="Serviço de Recrutamento R1" title="Serviço de Recrutamento R1" onclick="" />--%>
                        <div class="caption">
                            <div class="titulo tituloR1">
                                Conheça o serviço de recrutamento R1 para<br>
                                clientes com plano no BNE!
                            </div>
                            <blockquote>
                                <p class="metric">
                                    <i class="fa fa-check"></i>Receba os candidatos na <strong>sua empresa</strong>.
                                </p>
                                <p class="metric">
                                    <i class="fa fa-check"></i>Valores <strong>especiais </strong>para assinantes.
                                </p>

                            </blockquote>
                            <p class="align">
                                <i class="fa fa-phone"></i><strong class="phone">0800 41 2400</strong>
                            </p>
                        </div>
                    </asp:Panel>

                    <%--End Banners R1 e Divulgação de Vaga--%>


                    <div class="rodape_divulgacao_massa" style="display: none">
                        <div class="divulgacao_massa_titulo">
                            <h3 class="titulo_sucesso">Divulgação em Massa!
                            </h3>
                        </div>
                        <div class="base_redes_btn">
                            <div class="base_euquero">
                                <asp:ImageButton ID="btiEuQuero" runat="server" CssClass="btn_euquero" ImageUrl="img/btn_euquero.png"
                                    OnClick="btiEuQuero_Click" />
                            </div>
                            <div class="bg_redes_sociais">
                                <p>
                                    Divulgue suas vagas nas principais redes sociais.
                                </p>
                            </div>
                        </div>
                    </div>
                    <asp:Panel ID="pnlAnunciarVagaAnunciadas" runat="server" CssClass="painel_botoes">
                        <asp:Button ID="btnResultadoPesquisa" runat="server" Text="Prosseguir para a pesquisa >>" CssClass="botao_padrao verde"
                            CausesValidation="false" OnClick="btnResultadoPesquisa_Click" />
                        <asp:Button ID="btnVagasAnunciadas" runat="server" Text="Minhas vagas >>" CssClass="botao_padrao verde"
                            PostBackUrl="SalaSelecionadorVagasAnunciadas.aspx" CausesValidation="false" />
                    </asp:Panel>
            </asp:Panel>
            </div>
            <uc1:ModalConfirmacao ID="ucModalConfirmacao" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="conRodape" ContentPlaceHolderID="cphRodape" runat="server">
    <asp:UpdatePanel ID="upLEmail" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlLEmail" runat="server" Visible="false">
                <uc4:ModalEnvioEmail ID="ucModalEnvioEmail" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

