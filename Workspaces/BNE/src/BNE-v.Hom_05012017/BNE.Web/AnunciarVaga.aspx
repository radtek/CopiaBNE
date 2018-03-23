<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="AnunciarVaga.aspx.cs" Inherits="BNE.Web.AnunciarVaga" %>

<%@ Register Src="UserControls/Modais/ModalDeficiencia.ascx" TagName="Deficiencia" TagPrefix="uc7" %>
<%@ Register Src="UserControls/Modais/ModalConfirmacao.ascx" TagName="ModalConfirmacao"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ContratoFuncao.ascx" TagName="ucContrato" TagPrefix="uc2" %>
<%@ Register Src="UserControls/Modais/EnvioEmail.ascx" TagName="ModalEnvioEmail" TagPrefix="uc4" %>
<%@ Register TagPrefix="uc" TagName="ModalCompartilhamentoVaga" Src="~/UserControls/Modais/CompartilhamentoVaga.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <%--JS--%>
    <Employer:DynamicScript runat="server" Src="/js/local/AnunciarVaga.js" type="text/javascript" />
    <script src="/js/bootstrap/bootstrap.js"></script>
    <link href="/css/local/Bootstrap/bootstrap.css" rel="stylesheet" />

    <%--CSS--%>
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/Sala_Selecionador.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/AnunciarVagas.css" type="text/css" rel="stylesheet" />
    <script type='text/javascript'>
        function AbrirModalVideoSMSSelecionadora(autoplay) {
            var iframe = '<iframe id="video_player" width="560" height="315" src="https://www.youtube.com/embed/bDc4uaQQHHY" frameborder="0" allowfullscreen></iframe>';
            if (autoplay) {
                var iframe = '<iframe id="video_player" width="560" height="315" src="https://www.youtube.com/embed/bDc4uaQQHHY?autoplay=1" frameborder="0" allowfullscreen></iframe>';
                trackEvent("SalaSelecionadora", "play", "video sms da selecionadora");
            }
            if ($('#modalVideoSMSSelecionadora iframe').length <= 0) {
                $('#modalVideoSMSSelecionadora .modal-body').append(iframe);
            }

            $('#modalVideoSMSSelecionadora').modal('show');
        }

        $(document).ready(function () {
            $('#modalVideoSMSSelecionadora').on('hide', function () {
                $('#modalVideoSMSSelecionadora iframe').remove();
            });
        });

        var abrirCompartilhamentoFacebook = function (url) {
            window.open("https://www.facebook.com/sharer/sharer.php?u=" + url, 'fbShareWindow', 'height=450, width=550, top=' + ($(window).height() / 2 - 275) + ', left=' + ($(window).width() / 2 - 225) + ', toolbar=0, location=0, menubar=0, directories=0, scrollbars=0');
        }

        window.mostrarSucessoFacebook = function () {
            __doPostBack('__Page', 'mostrarSucessoFacebook');
        }

        var urlApiSalarioBR = "<%=urlApiSalarioBR%>";

        function RedirecionarCompraSalarioBR() {
            console.log('URL -> ' + 'Pagamento_v2.aspx?IdPlano=' + '<%=idPlanoRelatorioSalarioBR%>' + '&idFuncao=' + VariaveisGlobais.idFuncao + '&SiglaEstado=' + VariaveisGlobais.siglaEstado);
            window.open('Pagamento_v2.aspx?IdPlano=' + '<%=idPlanoRelatorioSalarioBR%>' + '&idFuncao=' + VariaveisGlobais.idFuncao + '&SiglaEstado=' + VariaveisGlobais.siglaEstado, '_blank');
        }

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <asp:UpdatePanel ID="upAnunciarVaga" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlAnunciarVaga" runat="server">
                <div class="painel_padrao_sala_selecionador">
                    <p>
                        Os candidatos serão avisados sobre seu anúncio e você receberá o currículo dos interessados
                    </p>
                    <div class="balaodialogo" id="imgAmigavelAnuncioVaga" style="display: none;">
                        <a href="javascript:void(0)">
                            <div class="fechar_Info_Relarorio_Salarial" onclick="ocultarImgAmigavel();">| Fechar |</div>
                        </a>
                        <h6 class="tit_media_b">Este salário está <strong id="txtNivelSalario" class="neg_val_sal">ABAIXO</strong> do mercado!
                            <h6>
                                <img src="/img/salariobr/grafics.png" alt="Alternate Text" class="gra" />
                                <span class="txt_m_sal">Por <strong class="dest_relat">R$ 12,90</strong> veja o <strong class="dest_relat">relatório completo para sua região!</strong></span>

                                <button type="button" onclick="RedirecionarCompraSalarioBR();" class="rel_salariobr">Clique Aqui</button>
                    </div>
                    <asp:UpdatePanel ID="upContratoFuncao" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <uc2:ucContrato Obrigatorio="true" ImagemPara="Empresa" OnClientFuncaoChange="Cidade_OnChange()"
                                ValidationGroup="ValidationAvancar" runat="server" ID="ucContratoFuncao"
                                OnClientFuncaoLostFocus="Funcao_LostFocus($(this));" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel runat="server" ID="upBairro" UpdateMode="Always">
                        <ContentTemplate>
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
                                    <asp:TextBox ID="txtCidadeAnunciarVaga" onBlur="RecuperarBairros()" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="linha">
                                <asp:Label ID="lblBairro" CssClass="label_principal" AssociatedControlID="txtBairroAnunciarVaga"
                                    runat="server" Text="Bairro"></asp:Label>
                                <div class="container_campo">
                                    <div>
                                        <asp:TextBox ID="txtBairroAnunciarVaga" CssClass="textbox_padrao" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
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
                            <asp:UpdatePanel ID="upFaixaSalarial" runat="server">
                                <ContentTemplate>
                                    <div>
                                        <span runat="server" id="avisoSalarioInvalido" class="validador" enableviewstate="false"></span>
                                    </div>
                                    <span>De</span>
                                    <asp:TextBox runat="server" ID="txtSalarioDe" name="txtSalarioDe" MaxLength="12" CssClass="textbox_padrao campo_obrigatorio input-salario" />
                                    <span>até</span>
                                    <asp:TextBox runat="server" ID="txtSalarioAte" name="txtSalarioAte" MaxLength="12" CssClass="textbox_padrao campo_obrigatorio input-salario" />
                                    <asp:HiddenField ID="hfFaixaSalarial" runat="server" />
                                    <asp:HiddenField ID="hdfValorSalarioMinimo" runat="server" />
                                    <asp:HiddenField ID="hfEstagio" runat="server" Value="0" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="faixa_salarial" runat="server" class="hint_explicativo">
                        </div>

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

                            <%--DEFICIENCIA NOVA--%>
                            <%--<asp:Label ID="lblDeficiencia" CssClass="label_principal" runat="server" Text="Pessoa com Deficiência (PCD)"
                            AssociatedControlID="ckbDeficiencia" />
                        <div class="container_campo">
                            <asp:CheckBox runat="server" ID="ckbDeficiencia" OnCheckedChanged="ckbDeficiencia_CheckedChanged"
                                AutoPostBack="True" />
                                    <asp:LinkButton ID="lnkEditDef" runat="server" AutoPostBack="True"
                                        Visible="false" OnClick="lnkEditDef_Click" >Editar Deficiência</asp:LinkButton>
                                    <asp:Panel ID="pnlDeficiencia" runat="server" Visible="false">
                                        <uc7:Deficiencia ID="ucDeficiencia" runat="server" />
                                    </asp:Panel>--%>
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
                                <asp:CheckBox ID="cbReceberCadaCVEmail" runat="server" Text="Receber um e-mail para cada candidatura recebida." />
                            </div>
                        </div>
                        <div class="linha">
                            <div class="container_campo">
                                <asp:CheckBox ID="cbReceberTodosCvsEmail" runat="server" Text="Receber um e-mail diário com a relação de candidatos inscritos no dia."
                                    Checked="true" />
                            </div>
                        </div>
                        <div class="linha">
                            <div class="container_campo">
                                <asp:CheckBox ID="ckbCompartilharFacebook" Enabled="True" runat="server" Text="Compartilhar vaga pelo Facebook" Checked="True" />
                                <span class="fa fa-facebook-square"></span>
                            </div>
                        </div>
                    </div>
                </div>
                <h2>
                    <asp:Label ID="lblSubTitulo" runat="server" Text="Perguntas para Pré-Seleção"></asp:Label>
                </h2>
                <div class="painel_padrao_sala_selecionador">
                    <asp:Panel CssClass="painel_padrao" runat="server">
                        <div class="linha texto_complementar">
                            <asp:Label ID="Label2" runat="server" Text="* Para auxiliar o recebimento de currículos dentro do perfil desejado, você tem a opção de fazer 4 perguntas para aqueles que se candidatarem. Os candidatos que informarem a resposta igual a sua serão classificados como dentro do perfil."></asp:Label>
                        </div>
                        <div class="linha">
                            <asp:Label ID="lblPergunta" runat="server" AssociatedControlID="txtPergunta" Text="Pergunta"
                                CssClass="label_principal"></asp:Label>
                            <div class="container_campo">
                                <asp:UpdatePanel ID="upPergunta" UpdateMode="Conditional" runat="server" RenderMode="Inline">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtPergunta" MaxLength="140" CssClass="textbox_padrao" runat="server"></asp:TextBox>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnAdicionarPergunta" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <asp:Label ID="lblResposta" runat="server" AssociatedControlID="rbSim" Text="Resposta"
                                    CssClass="label_principal">
                                </asp:Label>
                                <asp:UpdatePanel ID="upRespostas" UpdateMode="Conditional" runat="server" RenderMode="Inline">
                                    <ContentTemplate>

                                        <div class="container_campo">
                                            <asp:RadioButton ID="rbSim" runat="server" Text="Sim" AutoPostBack="true" GroupName="Radios" />
                                            <asp:RadioButton ID="rbNao" runat="server" AutoPostBack="true" Text="Não" GroupName="Radios" />
                                            <asp:RadioButton ID="rbDescritiva" runat="server" AutoPostBack="true" Text="Descritiva" GroupName="Radios" />
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnAdicionarPergunta" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
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
    <asp:UpdatePanel ID="upAnunciarVagaConfirmacao" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlAnunciarVagaConfirmacao" runat="server">
                <div class="painel_padrao_sala_selecionador_customizado">
                    <asp:Panel runat="server" ID="pnlCodigoVaga" CssClass="tela-sucesso">
                        <h2 class="titulo_sucesso">Vaga Cadastrada com Sucesso!</h2>
                        <i class="fa fa-check-circle icone_sucesso"></i>
                        <p>
                            Código da Vaga <b>
                                <asp:Literal ID="litCodVaga" runat="server"></asp:Literal></b>.<br />
                            Sua vaga foi cadastrada com sucesso<br />
                            e será publicada em breve.
                        </p>
                    </asp:Panel>
                    <div id="modalVideoSMSSelecionadora" style="width: 590px; display: none;" class="modal hide fade modal-size ModalBanner" style="display: none;" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header" style="border: 0;">
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                    <h4 class="modal-title" id="myModalLabel"></h4>
                                </div>
                                <div class="modal-body">
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:Panel ID="pnlImgR1" runat="server" CssClass="esquerda_sucesso" Visible="false">
                        <h2 class="titulo_sucesso">SMS da Selecionadora</h2>
                        <a href="#" onclick="AbrirModalVideoSMSSelecionadora(true);">
                            <img src="img/SalaSelecionadora/btn-video-salaselecionadora-new-150.png" /></a>
                        <p>
                            Recrute seus candidatos com agilidade e organização, utilizando o SMS da Selecionadora! Com ele você pode enviar SMS para vários candidatos e receber os retornos na tela do seu computador, em modelo de chat.
                        </p>
                    </asp:Panel>
                    <%--End Banners R1 e Divulgação de Vaga--%>
                    <asp:Panel ID="pnlAnunciarVagaAnunciadas" runat="server" CssClass="painel_botoes">
                        <asp:Button ID="btnVagasAnunciadas" runat="server" Text="Minhas vagas >>" CssClass="botao_padrao verde"
                            PostBackUrl="~/sala-selecionador-vagas-anunciadas" CausesValidation="false" />
                    </asp:Panel>
                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlPublicacaoImediata" Visible="false">

                <%--PLANOS--%>
                <section id="destaqueVaga">
                    <h3>Destaque sua vaga e <strong>obtenha inscritos rapidamente!</strong></h3>
                    <h5>Selecione uma opção:</h5>
                    <div id="destaqueVaga-container">
                        <div class="destaqueVaga-theader">
                            <div class="destaqueVaga-topics"></div>
                            <div class="destaqueVaga-option">
                                <div class="gratis">Anúncio <strong>Grátis</strong></div>
                            </div>
                            <div class="destaqueVaga-option">
                                <div class="destaque">Anúncio em <strong>Destaque</strong></div>
                            </div>
                            <div class="destaqueVaga-option">
                                <div class="ilimitado">
                                    Anúncio em Destaque <strong>Ilimitado</strong>
                                    <div class="ribbon">
                                        <img src="img/ribbon-destaque-ilimitado.png"></div>
                                </div>
                            </div>
                        </div>
                        <div class="destaqueVaga-tbody">
                            <!-- Cabeçalho -->
                            <div class="destaqueVaga-trow">
                                <div class="destaqueVaga-topics">
                                    Divulgação da vaga
                                </div>
                                <div class="destaqueVaga-option gratis">Após 3 dias</div>
                                <div class="destaqueVaga-option">Imediata para <span class="uppercase">uma</span> vaga</div>
                                <div class="destaqueVaga-option ilimitado">Imediata para <span class="uppercase">todas</span> as vagas</div>
                            </div>
                            <!-- Vaga em destaque -->
                            <div class="destaqueVaga-trow">
                                <div class="destaqueVaga-topics">Vaga em destaque</div>
                                <div class="destaqueVaga-option gratis">
                                    <i class="fa fa-times fa-lg"></i>
                                </div>
                                <div class="destaqueVaga-option"><span class="uppercase">Uma<span> vaga</div>
                                <div class="destaqueVaga-option ilimitado"><span class="uppercase">Todas</span> as vagas</div>
                            </div>
                            <!-- Aviso da vaga por SMS e email -->
                            <div class="destaqueVaga-trow">
                                <div class="destaqueVaga-topics">Aviso da vaga por SMS e Email para candidatos</div>
                                <div class="destaqueVaga-option gratis">
                                    <i class="fa fa-times fa-lg"></i>
                                </div>
                                <div class="destaqueVaga-option">
                                    <i class="fa fa-check fa-lg"></i>
                                </div>
                                <div class="destaqueVaga-option ilimitado">
                                    <i class="fa fa-check fa-lg"></i>
                                </div>
                            </div>
                            <!-- Acesso aos CVs -->
                            <div class="destaqueVaga-trow">
                                <div class="destaqueVaga-topics">Acesso aos currículos</div>
                                <div class="destaqueVaga-option gratis">Somente assinantes inscritos na vaga</div>
                                <div class="destaqueVaga-option">20 currículos inscritos</div>
                                <div class="destaqueVaga-option ilimitado">50 currículos por mês</div>
                            </div>
                            <!-- Indexação Google -->
                            <div class="destaqueVaga-trow">
                                <div class="destaqueVaga-topics">
                                    Indexação Google
                        <Componentes:BalaoSaibaMais ID="BalaoSaibaMais2" runat="server" ToolTipText="As vagas publicadas no BNE são enviadas para o Google e ficam disponíveis para candidatos. Este processo ocorre conforme as regras de indexação do Google."
                            Text="Saiba mais" ToolTipTitle="" CssClassLabel="topic-question" ShowOnMouseover="True" />

                                </div>
                                <div class="destaqueVaga-option gratis">
                                    <i class="fa fa-check fa-lg"></i>
                                </div>
                                <div class="destaqueVaga-option">
                                    <i class="fa fa-check fa-lg"></i>
                                </div>
                                <div class="destaqueVaga-option ilimitado">
                                    <i class="fa fa-check fa-lg"></i>
                                </div>
                            </div>
                            <!-- Integração SINE -->
                            <div class="destaqueVaga-trow">
                                <div class="destaqueVaga-topics">
                                    Integração SINE
                      <Componentes:BalaoSaibaMais ID="BalaoSaibaMais4" runat="server" ToolTipText="As vagas publicadas no BNE são enviadas para o SINE em destaque e ficam disponíveis para candidatos. Este processo ocorre conforme as regras de indexação do SINE."
                          Text="Saiba mais" ToolTipTitle="" CssClassLabel="topic-question" ShowOnMouseover="True" />
                                </div>
                                <div class="destaqueVaga-option gratis">
                                    <i class="fa fa-check fa-lg"></i>
                                </div>
                                <div class="destaqueVaga-option">
                                    <i class="fa fa-check fa-lg"></i>
                                </div>
                                <div class="destaqueVaga-option ilimitado">
                                    <i class="fa fa-check fa-lg"></i>
                                </div>
                            </div>
                            <!-- Compartilhamento Facebook -->
                            <div class="destaqueVaga-trow">
                                <div class="destaqueVaga-topics">
                                    Compartilhamento Facebook
                      <Componentes:BalaoSaibaMais ID="BalaoSaibaMais3" runat="server" ToolTipText="Disponibilizamos Link para divulgação de sua vaga em grupos no Facebook."
                          Text="Saiba mais" ToolTipTitle="" CssClassLabel="topic-question" ShowOnMouseover="True" />
                                </div>
                                <div class="destaqueVaga-option gratis">
                                    <i class="fa fa-check fa-lg"></i>
                                </div>
                                <div class="destaqueVaga-option">
                                    <i class="fa fa-check fa-lg"></i>
                                </div>
                                <div class="destaqueVaga-option ilimitado">
                                    <i class="fa fa-check fa-lg"></i>
                                </div>
                            </div>
                        </div>
                        <div class="destaqueVaga-tfooter">
                            <div class="destaqueVaga-topics"></div>
                            <div class="destaqueVaga-option gratis">
                                <div class="valor"><span class="lighter">GRÁTIS</span></div>
                                <asp:Button ID="btnGratis"  CssClass="btn-seleciona" runat="server" OnClick="lnkGratis_Click"
                                    OnClientClick="trackEvent('Funil CIA', 'Plano Escolhido', 'Anuncio Grátis'); return true;"
                                 Text="Aguardar 3 dias" />
                            </div>
                            <div class="destaqueVaga-option destaque">
                                <div class="valor"><span class="lighter">R$</span><asp:Literal ID="litPrecoDestaquePara" runat="server" Visible="true"></asp:Literal></div>
                                <div class="tpvalor">por vaga</div>
                                <asp:Button ID="btnPlanoDestaque" runat="server" OnClick="lkbPlanoDestaque_Click"
                                     CssClass="btn-seleciona" OnClientClick="trackEvent('Funil CIA', 'Plano Escolhido', 'Plano Basico'); return true;"
                                    Text="Publicar imediatamente"  />
                            </div>
                            <div class="destaqueVaga-option ilimitado">
                                <div class="valor"><span class="lighter">R$</span><asp:Literal ID="litPlano50PrecoPara" runat="server" Visible="true"></asp:Literal></div>
                                <div class="tpvalor">por mês</div>
                                <asp:Button ID="btnPlano50"  CssClass="btn-seleciona" OnClick="lnkPlano50_Click" runat="server" OnClientClick="trackEvent('Funil CIA', 'Plano Escolhido', 'Plano Basico'); return true;" Text="Publicar imediatamente" />
                            </div>
                        </div>
                    </div>
                </section>
                <%--FIM PLANOS--%>
            </asp:Panel>

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
    <asp:UpdatePanel ID="upCompVaga" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlCompVaga" runat="server" Visible="false">
                <uc:ModalCompartilhamentoVaga ID="ucModalCompartilhamentoVaga" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

