<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DadosComplementares.ascx.cs" Inherits="BNE.Web.UserControls.Forms.CadastroCurriculo.DadosComplementares" %>
<Employer:DynamicScript runat="server" Src="/js/local/Forms/CadastroCurriculo/DadosComplementares.js" type="text/javascript" />
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/CadastroCurriculo/DadosComplementares.css" type="text/css" rel="stylesheet" />
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
                        <asp:LinkButton ID="btlMiniCurriculo" OnClick="btlMiniCurriculo_Click" Text="Mini Currículo" ValidationGroup="Salvar"
                            CssClass="texto_abas" runat="server"></asp:LinkButton>
                    </span>
                </div>
                <div class="abas_nova">
                    <span class="aba_esquerda_nao_selecionada aba">
                        <asp:LinkButton ID="btlDadosPessoais" runat="server" OnClick="btlDadosPessoais_Click" ValidationGroup="Salvar"
                            Text="Dados Pessoais e Profissionais" CssClass="texto_abas"></asp:LinkButton>
                    </span>
                </div>
                <div class="abas_nova">
                    <span class="aba_esquerda_nao_selecionada aba">
                        <asp:LinkButton ID="btlFormacaoCursos" runat="server" OnClick="btlFormacaoCursos_Click" ValidationGroup="Salvar"
                            Text="Formação e Cursos" CssClass="texto_abas"></asp:LinkButton>
                    </span>
                </div>
                <div class="abas_nova">
                    <span class="aba_esquerda_selecionada aba">
                        <asp:Label ID="Label4" runat="server" Text="Dados Complementares" CssClass="texto_abas_selecionada">
                        </asp:Label>
                    </span>
                </div>
                <div class="abas_nova">
                    <span class="aba_esquerda_nao_selecionada aba">
                        <asp:LinkButton ID="btlRevisaoDados" runat="server" OnClick="btlRevisaoDados_Click" ValidationGroup="Salvar"
                            Text="Conferir" CssClass="texto_abas"></asp:LinkButton>
                    </span>
                </div>
            </div>
            <div class="abas" style="display: none">
                <span class="aba_fundo">
                    <asp:LinkButton ID="btlGestao" runat="server" OnClick="btlGestao_Click" ValidationGroup="salvar" CausesValidation="true"
                        Text="Gestão"></asp:LinkButton>
                </span>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
<!-- FIM: Tabs -->
<!-- Painel: Dados Complementares -->
<div class="interno_abas">
    <asp:UpdatePanel ID="upAbaDadosComplementares" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlDadosComplementares" runat="server" CssClass="painel_padrao">
                <div class="painel_padrao_topo">
                </div>
                <%-- Linha Tipo Veículo --%>
                <div class="linha">
                    <asp:Label ID="lblTipoVeiculo" CssClass="label_principal" runat="server" Text="Possui Veículo" AssociatedControlID="ddlTipoVeiculo" />
                    <div class="container_campo">
                        <div>
                            <asp:CustomValidator ID="cvTipoVeiculo" runat="server" ValidationGroup="SalvarVeiculo" ControlToValidate="ddlTipoVeiculo"
                                ErrorMessage="Campo Obrigatório" ClientValidationFunction="cvTipoVeiculo_Validate">
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
                        <componente:AlfaNumerico ID="txtModelo" runat="server" Columns="44" MaxLength="50" ValidationGroup="SalvarVeiculo"
                            Obrigatorio="true" CssClassTextBox="textbox_padrao" WidthTextBox="100px" />
                    </div>
                    <%-- FIM: Linha Modelo --%>
                    <%--Linha Ano--%>
                    <asp:Label ID="lblAnoVeiculo" runat="server" AssociatedControlID="txtAnoVeiculo" Text="Ano"></asp:Label>
                    <div class="container_campo">
                        <componente:AlfaNumerico ID="txtAnoVeiculo" runat="server" Columns="4" MaxLength="4" ContemIntervalo="True"
                            ValorMinimo="1800" ValorMaximo="2500" CssClassTextBox="textbox_padrao" Obrigatorio="true" Tipo="Numerico"
                            ValidationGroup="SalvarVeiculo" />
                    </div>
                </div>
                <asp:Panel ID="pnlBotoesCarros" runat="server" CssClass="painel_botoes">
                    <asp:Button ID="bntAdicionarVeiculo" runat="server" CssClass="botao_padrao" ValidationGroup="SalvarVeiculo"
                        CausesValidation="true" Text="Adicionar Veículo" OnClick="bntAdicionarVeiculo_Click" />
                </asp:Panel>
                <%--FIM: Linha Modelo--%>
                <div class="linha">
                    <asp:GridView ID="gvModeloAno" runat="server" AllowSorting="False" AllowPaging="False" AllowCustomPaging="False"
                        AlternatingRowStyle-CssClass="alt_row" OnRowDeleting="gvModeloAno_RowDeleting" DataKeyNames="Idf_Veiculo">
                        <Columns>
                            <asp:BoundField HeaderText="Veículo" DataField="Des_Tipo_Veiculo" HeaderStyle-CssClass="rgHeader centro" />
                            <asp:BoundField HeaderText="Modelo" DataField="Des_Modelo" HeaderStyle-CssClass="rgHeader centro" />
                            <asp:BoundField HeaderStyle-CssClass="rgHeader centro" HeaderText="Ano" ItemStyle-CssClass="centro" DataField="Ano_Veiculo" />
                            <asp:TemplateField HeaderText="Ações" HeaderStyle-CssClass="rgHeader centro">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btiExcluir" runat="server" AlternateText="Excluir" CausesValidation="False" CommandName="Delete"><i class="fa fa-times"></i></asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle CssClass="rgHeader centro coluna_icones" />
                                <ItemStyle CssClass="espaco_icones centro" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <%-- Linha Habilitação--%>
                <div class="linha linha_cnh">
                    <asp:Label ID="lblHabilitacao" CssClass="label_principal" runat="server" Text="Habilitação (CNH)" AssociatedControlID="ddlHabilitacao" />
                    <div class="container_campo">
                        <asp:DropDownList ID="ddlHabilitacao" CssClass="textbox_padrao" runat="server">
                        </asp:DropDownList>
                    </div>
                    <asp:Label ID="Label5" runat="server" AssociatedControlID="txtHabilitacao" CssClass="label_principal label_n_registro_cnh"
                        Text="Número do Registro da CNH"></asp:Label>
                    <componente:AlfaNumerico ID="txtHabilitacao" runat="server" Columns="15" MaxLength="15" CssClassTextBox="textbox_padrao"
                        Obrigatorio="false" />
                </div>
                <%-- FIM: Linha Habilitação --%>
                <%--Linha OutrosConhecimentos--%>
                <div class="linha">
                    <asp:Label ID="lblOutrosConhecimentosInfoAdd" CssClass="label_principal" runat="server" Text="Outros Conhecimentos"
                        AssociatedControlID="txtOutrosConhecimentos" />
                    <div class="container_campo">
                        <componente:AlfaNumerico ID="txtOutrosConhecimentos" runat="server" Obrigatorio="False" CssClassTextBox="textbox_padrao multiline atividades_exercidas"
                            TextMode="MultiLine" MaxLength="500" Rows="500" />
                    </div>
                    <asp:Panel runat="server" ID="pnlLegenda" CssClass="BoxSugestaoTarefas">
                        <div class="seta_apontador_esq">
                        </div>
                        <div class="box_conteudo sugestao">
                            <asp:Label ID="lblLegenda" CssClass="textbox_padrao sugestao_tarefas" runat="server" Text="Informe aqui suas habilidades e conhecimentos importantes para a função. Ex: (Vendedor) Facilidade em negociações comerciais, prospecção e reativação de carteira de clientes." />
                        </div>
                    </asp:Panel>
                </div>
                <%--Linha Observações--%>
                <div class="linha">
                    <asp:Label ID="lblObservacaoInfoAdd" CssClass="label_principal" runat="server" Text="Observações" AssociatedControlID="txtObservacoes" />
                    <div class="container_campo">
                        <componente:AlfaNumerico ID="txtObservacoes" runat="server" Obrigatorio="False" CssClassTextBox="textbox_padrao multiline atividades_exercidas"
                            TextMode="MultiLine" MaxLength="500" Rows="500" />
                    </div>
                    <div>
                        <asp:Panel runat="server" ID="Panel4" CssClass="BoxSugestaoTarefas">
                            <div class="seta_apontador_esq">
                            </div>
                            <div class="box_conteudo sugestao">
                                <asp:Label ID="Label1" CssClass="textbox_padrao sugestao_tarefas" runat="server" Text="Utilize este campo para descrever dados como, religião, trabalhos temporários/ free lancer, outras experiências profissionais, trabalho voluntário hobbies etc." />
                            </div>
                        </asp:Panel>
                    </div>
                </div>
                <%--FIM: Linha Observações--%>
            </asp:Panel>
            <%-- Painel Anexar Arquivo 1 --%>
            <h2 class="titulo_painel_padrao">
                <asp:Label ID="lblAnexarArquivos" runat="server" Text="Anexar Arquivos" />
            </h2>
            <asp:Panel ID="pnlAnexarArquivos" runat="server" CssClass="painel_padrao">
                <div class="painel_padrao_topo">
                    &nbsp;
                </div>
                <div class="linha">
                    <div class="container_campo">
                        <asp:Panel ID="pnlAsyncUpload" runat="server">
                            <telerik:RadAsyncUpload ID="auUpload" runat="server" MaxFileInputsCount="1" InitialFileInputsCount="1"
                                OnFileUploaded="auUpload_FileUploaded" ReadOnlyFileInputs="True"
                                ControlObjectsVisibility="None" OverwriteExistingFiles="True" Skin="Office2007" CssClass="campo_anexar_arquivo" TemporaryFileExpiration="00:10:00" AllowedFileExtensions=".doc, .pdf, .txt, .rtf, .docx , .pub, .odt, .dotx">
                                <Localization Select="Procurar Arquivo" Remove="Remover" />
                            </telerik:RadAsyncUpload>
                        </asp:Panel>
                        <asp:Panel ID="pnlUploadedFile" runat="server" CssClass="col_action_arq">
                            <asp:HyperLink ID="hlDownload" Target="_blank" CssClass="espacamento" runat="server" />
                            <asp:LinkButton ID="btiExcluirAnexo" runat="server" Text="Excluir Anexo" ToolTip="Excluir Anexo" AlternateText="Excluir Anexo" CausesValidation="false" OnClick="btiExcluirAnexo_Click"><i class="fa fa-times"></i></asp:LinkButton>
                        </asp:Panel>
                    </div>
                    <asp:Panel runat="server" ID="Panel6" CssClass="BoxSugestaoTarefas">
                        <div class="seta_apontador_esq largura_media">
                        </div>
                        <div class="box_conteudo sugestao">
                            <asp:Label ID="Label9" CssClass="textbox_padrao sugestao_tarefas" runat="server" Text="Anexe outro arquivo que pode lhe ajudar nos processos seletivos como: currículo já criado, artigos, portfólios, projetos, etc." />
                        </div>
                    </asp:Panel>
                </div>
            </asp:Panel>
            <%-- Fim:Painel Anexar Arquivo 1 --%>
            <!-- Painel: Horario Disponivel -->
            <h2 class="titulo_painel_padrao">
                <asp:Label ID="lblTituloHorarioDisponivel" runat="server" Text="Horário Disponível para Trabalho" />
            </h2>
            <asp:Panel ID="pnlHorarioDisponivel" runat="server" CssClass="painel_padrao">
                <div class="painel_padrao_topo">
                    &nbsp;
                </div>
                <%-- Linha Horario --%>
                <div class="linha">
                    <div class="container_campo" id="divHorarios">
                        <asp:CustomValidator runat="server" ID="cvDisponibilidade" ValidationGroup="Salvar" ClientValidationFunction="cvDisponibilidade_Validate"
                            ErrorMessage="Selecione uma ou mais disponibilidade de trabalho."></asp:CustomValidator>
                        <asp:CheckBoxList ID="ckblDisponibilidade" CausesValidation="True" ValidationGroup="Salvar" RepeatDirection="Horizontal"
                            runat="server">
                            <asp:ListItem Text="Manhã" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Tarde" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Noite" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Sábado" Value="4"></asp:ListItem>
                            <asp:ListItem Text="Domingo" Value="5"></asp:ListItem>
                        </asp:CheckBoxList>
                        <asp:CheckBox ID="ckbDisponibilidadeViagem" runat="server" Text="Disponibilidade para Viagem" />
                    </div>
                </div>
            </asp:Panel>
            <!-- FIM Painel: Horario Disponivel -->
            <!-- Painel: Disponibilidade Trabalhar Outra Cidade -->
            <h2 class="titulo_painel_padrao">
                <asp:Label ID="Label6" runat="server" Text="Disponibilidade para Trabalhar em Outra Cidade" />
            </h2>
            <asp:Panel ID="pnlDisponibilidadeTrabalharOutraCidade" runat="server" CssClass="painel_padrao">
                <div class="linha">
                    <asp:Label ID="lblCidadeDisponivel" runat="server" Text="Cidade" CssClass="label_principal" AssociatedControlID="txtCidadeDisponivel"></asp:Label>
                    <div>
                        <asp:RequiredFieldValidator ID="rfvCidadeDisponivel" runat="server" ControlToValidate="txtCidadeDisponivel"
                            ValidationGroup="SalvarCidade"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cvCidadeDisponivel" runat="server" ErrorMessage="Cidade Inválida." ClientValidationFunction="cvCidadeDisponivel_Validate"
                            ControlToValidate="txtCidadeDisponivel" ValidationGroup="SalvarCidade"></asp:CustomValidator>
                    </div>
                    <div class="container_campo">
                        <asp:TextBox ID="txtCidadeDisponivel" runat="server" CssClass="textbox_padrao" Columns="40"></asp:TextBox>
                    </div>
                    <asp:Button ID="btnAdicionarCidade" runat="server" Text="Adicionar Cidade" CausesValidation="True" CssClass="botao_padrao"
                        ValidationGroup="SalvarCidade" OnClick="btnAdicionarCidade_Click" />
                </div>
                <div class="linha">
                    <asp:GridView ID="gvCidade" runat="server" AllowSorting="false" AlternatingRowStyle-CssClass="alt_row"
                        AllowPaging="false" CssClass="gridview_padrao" AutoGenerateColumns="false" DataKeyNames="Idf_Curriculo_Disponibilidade_Cidade"
                        OnRowDeleting="gvCidadeRowDeleting">
                        <Columns>
                            <asp:BoundField HeaderText="Cidade" DataField="Nme_Cidade" HeaderStyle-CssClass="rgHeader centro" />
                            <asp:TemplateField HeaderText="Ações">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btiExcluir" runat="server" AlternateText="Excluir" ToolTrip="Excluir" ausesvalidation="false" CausesValidation="False" CommandName="Delete" ><i class="fa fa-times"></i></asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle CssClass="rgHeader centro coluna_icones" />
                                <ItemStyle CssClass="espaco_icones centro" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>
            <!-- FIM Painel: Disponibilidade Trabalhar Outra Cidade -->
            <%--Linha Raça--%>
            <h2 class="titulo_painel_padrao">
                <asp:Label ID="Label3" runat="server" Text="Características Pessoais" />
            </h2>
            <asp:Panel ID="Panel1" CssClass="painel_padrao" runat="server">
                <div class="painel_padrao_topo">
                </div>
                <div class="linha">
                    <asp:Label ID="lblRaca" CssClass="label_principal" runat="server" Text="Escolha sua raça de acordo com a tabela do governo"
                        AssociatedControlID="ddlRaca" />
                    <div class="container_campo">
                        <asp:DropDownList ID="ddlRaca" CssClass="textbox_padrao" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <%--FIM:Linha Raça--%>
                <%--Linha Altura Peso--%>
                <div class="linha">
                    <asp:Label ID="lblAltura" CssClass="label_principal" runat="server" Text="Altura" AssociatedControlID="txtAltura" />
                    <div class="container_campo">
                        <componente:ValorDecimal ID="txtAltura" runat="server" CssClassTextBox="textbox_padrao" Obrigatorio="False"
                            ValorMaximo="2.50" ValidationGroup="Salvar" />
                    </div>
                    <asp:Label ID="lblPeso" CssClass="label_principal" runat="server" Text="Peso" AssociatedControlID="txtPeso" />
                    <div class="container_campo">
                        <componente:ValorDecimal ID="txtPeso" runat="server" Obrigatorio="false" CssClassTextBox="textbox_padrao"
                            ValorMaximo="500" ValidationGroup="Salvar" />
                        <label>
                            Kg</label>
                    </div>
                </div>
                <%--FIM: Linha Altura Peso--%>
                <%--Linha --%>
                <div class="linha">
                    <asp:Label ID="Label10" runat="server" CssClass="label_principal" AssociatedControlID="ddlFilhos" Text="Filhos?"></asp:Label>
                    <div class="container_campo">
                        <asp:DropDownList ID="ddlFilhos" CssClass="textbox_padrao" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
            </asp:Panel>
            <h2 class="titulo_painel_padrao">
                <asp:Label ID="Label2" runat="server" Text="Profissional com Deficiência?" />
            </h2>
            <asp:Panel ID="Panel2" CssClass="painel_padrao" runat="server">
                <div class="painel_padrao_topo">
                </div>
                <div class="linha">
                    <asp:Label ID="lblDeficiencia" CssClass="label_principal" runat="server" Text="Tipo de Deficiência" AssociatedControlID="ddlDeficiencia" />
                    <div class="container_campo">
                        <asp:DropDownList CssClass="textbox_padrao" ID="ddlDeficiencia" runat="server">
                        </asp:DropDownList>
                    </div>
                    <asp:Label ID="Label11" runat="server" CssClass="label_principal" Text="Complemento" AssociatedControlID="txtComplementoDeficiencia"></asp:Label>
                    <div class="container_campo">
                        <asp:TextBox ID="txtComplementoDeficiencia" CssClass="textbox_padrao" runat="server"></asp:TextBox>
                    </div>
                </div>
                <%--FIM: Linha Peso--%>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnFinalizarCadastro" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<!-- FIM Painel: Dados Complementares -->
<!-- Painel botoes -->
<asp:Panel runat="server" CssClass="painel_botoes" ID="pnlBotoes">
    <asp:Button ID="btnFinalizarCadastro" runat="server" CssClass="botao_padrao" OnClick="btnFinalizarCadastro_Click"
        Text="Salvar e Avançar" ValidationGroup="Salvar" />
</asp:Panel>
<!-- FIM: Painel botoes -->
<asp:Panel ID="pnlModalUpload" runat="server" CssClass="modal_conteudo" Style="display: none">
    <div class="panel_padrao_topo">
        &nbsp;
    </div>
    <asp:Button ID="bntCancelar" runat="server" Text="Voltar" CssClass="botao_padrao voltar" />
</asp:Panel>
<asp:HiddenField ID="hfUpload" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeUpload" runat="server" PopupControlID="pnlModalUpload" CancelControlID="bntCancelar"
    TargetControlID="hfUpload" />
