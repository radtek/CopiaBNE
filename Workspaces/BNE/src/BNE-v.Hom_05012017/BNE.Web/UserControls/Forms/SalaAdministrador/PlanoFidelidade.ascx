<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlanoFidelidade.ascx.cs"
    Inherits="BNE.Web.UserControls.Forms.SalaAdministrador.PlanoFidelidade" %>
<%@ Register Src="../../Modais/ConfirmacaoExclusao.ascx" TagName="ConfirmacaoExclusao"
    TagPrefix="uc1" %>
<%@ Register Src="../../ucObservacaoFilial.ascx" TagName="ucObservacaoFilial" TagPrefix="uc2" %>
<Employer:DynamicScript runat="server" Src="/js/local/UserControls/PlanoFidelidade.js" type="text/javascript" />
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/PlanoFidelidade.css" type="text/css" rel="stylesheet" />
<h2>
    Plano</h2>
<asp:UpdatePanel ID="upPesquisaCliente" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="idPesquisarCliente" runat="server" CssClass="blocodados" DefaultButton="btnFiltrar">
            <h3>
                Pesquisar Cliente</h3>
            <div>
                <div class="linha">
                    <asp:Label ID="lblBuscarCNPJ" runat="server" Text="CNPJ" AssociatedControlID="tbxCNPJ"
                        CssClass="label_principal" />
                    <telerik:RadTextBox runat="server" ID="tbxCNPJ" TextMode="SingleLine" EmptyMessage=""
                        CssClass="textbox_padrao_pesquisa">
                    </telerik:RadTextBox>
                </div>
                <div class="linha">
                    <asp:Label ID="lblBuscarCPF" runat="server" Text="CPF" AssociatedControlID="tbxCPF"
                        CssClass="label_principal" />
                    <telerik:RadTextBox runat="server" ID="tbxCPF" TextMode="SingleLine" EmptyMessage=""
                        CssClass="textbox_padrao_pesquisa">
                    </telerik:RadTextBox>
                </div>
                <div class="linha">
                    <asp:Label ID="Label1" runat="server" Text="Boleto" AssociatedControlID="tbxBoleto"
                        CssClass="label_principal" />
                    <telerik:RadTextBox runat="server" ID="tbxBoleto" TextMode="SingleLine" EmptyMessage=""
                        CssClass="textbox_padrao_pesquisa">
                    </telerik:RadTextBox>
                </div>
                <div class="linha">
                    <asp:Label ID="lblTID" runat="server" Text="TID" AssociatedControlID="tbxTID" CssClass="label_principal" />
                    <telerik:RadTextBox runat="server" ID="tbxTID" TextMode="SingleLine" EmptyMessage=""
                        CssClass="textbox_padrao_pesquisa">
                    </telerik:RadTextBox>
                </div>
                <asp:Panel ID="Panel6" runat="server" CssClass="painel_botoes">
                    <asp:Button ID="btnFiltrar" runat="server" CssClass="botao_padrao filtrar" Text="Pesquisar"
                        ToolTip="Filtrar" CausesValidation="True" ValidationGroup="Filtrar" OnClick="btnFiltrar_Click" />
                </asp:Panel>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="upPnlDadosClientePJ" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="pnlDadosClientePJ" runat="server" CssClass="blocodados" Visible="false">
            <h3>
                Dados do Cliente</h3>
            <%--Linha Nome Completo--%>
            <div class="linha">
                <asp:Label ID="Label2" runat="server" Text="Razão Social" CssClass="label_principal" />
                <span class="container_campo dados_empresa">
                    <asp:Literal ID="litRazaoSocial" runat="server"></asp:Literal></span>
            </div>
            <%--FIM Razão Social--%>
            <%--Linha CPF--%>
            <div class="linha">
                <asp:Label ID="lblNomeFantasiaouApelido" runat="server" Text="Nome Fantasia ou Apelido"
                    CssClass="label_principal" />
                <span class="container_campo dados_empresa">
                    <asp:Literal ID="litNomeFantasia" runat="server"></asp:Literal></span>
            </div>
            <%--FIM CPF--%>
            <%--Linha Data de Nascimento--%>
            <div class="linha">
                <asp:Label ID="Label3" runat="server" Text="CNPJ" CssClass="label_principal" />
                <span class="container_campo dados_empresa">
                    <asp:Literal ID="litCNPJ" runat="server"></asp:Literal></span>
            </div>
            <%--FIM Data de Nascimento--%>
            <%--Linha e-mail--%>
            <div class="linha">
                <asp:Label ID="Label4" runat="server" Text="Cidade" CssClass="label_principal" />
                <span class="container_campo dados_empresa">
                    <asp:Literal ID="litCidadePJ" runat="server"></asp:Literal></span>
            </div>
            <%--FIM e-mail--%>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="upPnlDadosClientePF" runat="server" UpdateMode="Conditional"
    Visible="true">
    <ContentTemplate>
        <asp:Panel ID="pnlDadosClientePF" runat="server" CssClass="blocodados" Visible="false">
            <h3>
                Dados do Cliente</h3>
            <%--Linha Nome --%>
            <div class="linha">
                <asp:Label ID="lblNome" runat="server" Text="Nome Completo" CssClass="label_principal" />
                <span class="container_campo dados_empresa">
                    <asp:Literal ID="litNome" runat="server"></asp:Literal>
                </span>
            </div>
            <%--FIM Nome --%>
            <%--Linha CPF--%>
            <div class="linha">
                <asp:Label ID="lblCPF" runat="server" Text="CPF" CssClass="label_principal" />
                <span class="container_campo dados_empresa">
                    <asp:Literal ID="litCPF" runat="server"></asp:Literal></span>
            </div>
            <%--FIM Nome CPF--%>
            <%--Linha Data de Nascimento --%>
            <div class="linha">
                <asp:Label ID="lblDataNascimento" runat="server" Text="Data de Nascimento" CssClass="label_principal" />
                <span class="container_campo dados_empresa">
                    <asp:Literal ID="litDataNascimento" runat="server"></asp:Literal></span>
            </div>
            <%--FIM Data de Nascimento --%>
            <%--Linha Email --%>
            <div class="linha">
                <asp:Label ID="lblEmail" runat="server" Text="E-Mail" CssClass="label_principal" />
                <span class="container_campo dados_empresa">
                    <asp:Literal ID="litEmail" runat="server"></asp:Literal></span>
            </div>
            <%--FIM Data de Nascimento --%>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="upPnlPlanoFidelidade" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="pnlPlanoFidelidade" runat="server" CssClass="blocodados">
            <h3>
                Cadastro de Planos</h3>
            <%--Linha Tipo do Plano --%>
            <div class="linha">
                <asp:Label ID="lblTipoPlano" runat="server" Text="Tipo de Plano" CssClass="label_principal" />
                <div class="container_campo">
                    <telerik:RadComboBox runat="server" ID="rcbPlano" EmptyMessage=" " AllowCustomText="false"
                        CssClass="rcbCombo" OnSelectedIndexChanged="rcbPlano_SelectedIndexChanged"
                        AutoPostBack="True">
                    </telerik:RadComboBox>
                    <asp:UpdatePanel ID="upBtnCriarAdicional" runat="server" UpdateMode="Conditional"
                        RenderMode="Inline">
                        <ContentTemplate>
                            <asp:Button ID="btnCriarAdicional" runat="server" CssClass="botao_padrao" Text="Adicional"
                                CausesValidation="false" OnClick="btnCriarAdicional_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <%--FIM Tipo do Plano --%>
            <%--Linha Filial Gestora --%>
            <div class="linha">
                <asp:Label ID="lblFilialGestora" runat="server" Text="Filial Gestora" CssClass="label_principal" />
                <div class="container_campo">
                    <telerik:RadComboBox runat="server" ID="rcbFilialGestora" EmptyMessage=" " AllowCustomText="false"
                        CssClass="rcbCombo" AutoPostBack="false">
                    </telerik:RadComboBox>
                </div>
            </div>
            <%--FIM Filial Gestora --%>
            <%--Linha Data Inicio Plano --%>
            <div class="linha" id="divDataInicioPlano" runat="server">
                <asp:Label ID="lblDataInicioPlano" runat="server" Text="Data de Inicio do Plano"
                    CssClass="label_principal" />
                <div class="container_campo">
                    <componente:Data ID="txtDataInicioPlano" runat="server" ValidationGroup="Salvar" />
                </div>
            </div>
            <%--FIM Data Data Inicio Plano --%>
            <%--Linha Data Fim Plano --%>
            <div class="linha" id="divDataFimPlano" runat="server">
                <asp:Label ID="lblDataFimPlano" runat="server" Text="Data de Fim do Plano" CssClass="label_principal" />
                <div class="container_campo">
                    <componente:Data ID="txtDataFimPlano" runat="server" ValidationGroup="Salvar" />
                </div>
            </div>
            <%--FIM Data Data Fim Plano --%>
            <%--Linha Data Envio Boleto--%>
            <div class="linha" id="divDataEnvioBoleto" runat="server">
                <asp:Label ID="lblDataEnvioBoleto" runat="server" Text="Data de Envio do Boleto"
                    CssClass="label_principal" />
                <div class="container_campo">
                    <componente:Data ID="txtDataEnvioBoleto" runat="server" ValidationGroup="Salvar" />
                </div>
            </div>
            <%--FIM Data Envio Boleto --%>
            <%--Linha Data de Vencimento do Boleto--%>
            <div class="linha" id="divDataVencimentoBoleto" runat="server">
                <asp:Label ID="lblDataVencimentoBoleto" runat="server" Text="Data de Vencimento do Boleto"
                    CssClass="label_principal" />
                <div class="container_campo">
                    <componente:Data ID="txtDataVencimentoBoleto" runat="server" ValidationGroup="Salvar" />
                </div>
            </div>
            <%--FIM Data de Vencimento do Boleto --%>
            <%--Linha Registra Boleto--%>
            <div class="linha" id="divRegistraBoleto" runat="server">
                <asp:Label ID="lblRegistraBoleto" runat="server" Text="Registra Boleto" CssClass="label_principal" />
                <div class="container_campo">
                    <asp:CheckBox runat="server" ID="chkRegistraBoleto" />
                </div>
            </div>
            <%--FIM Registra Boleto --%>
            <%-- Linha Nota Fiscal Antecipada --%>
            <div id="divNotaAntecipada" runat="server" class="linha">
                <asp:Label ID="lblNotaAntecipada" runat="server" Text="Nota Fiscal Antecipada" CssClass="label_principal" />
                <div class="container_campo">
                    <asp:CheckBox runat="server" ID="chkNotaAntecipada" AutoPostBack="true" OnCheckedChanged="chkNotaAntecipada_CheckedChanged" />
                </div>   
            </div>
            <%--Linha Data de Envio De Nota Antecipada--%>
            <div class="linha" id="divDataNFAntecipada" runat="server" visible="false">
                    <asp:Label ID="lblDataNFAntecipada" runat="server" Text="Data de Envio de Nota Fiscal Antecipada"
                        CssClass="label_principal" />
                    <div class="container_campo"> 
                        <componente:Data ID="txtDataNFAntecipada" runat="server" ValidationGroup="Salvar" />
                    </div>
                </div>
            <%-- Fim Linha Nota Fiscal Antecipada --%>
            <%--Linha Pagamento Recorrente --%>
            <div id="divPlanoPagamentoRecorrente" runat="server" visible="false">
                <asp:Label ID="Label5" runat="server" Text="Pagamento" CssClass="label_principal" />
                <div class="container_campo">
                    <asp:CheckBox ID="ckbPagamentoRecorrenteMensal" runat="server" Text="Recorrente Mensal (cobrança efetuada mensalmente e automaticamente na data de vencimento do Plano)" />
                </div>
                <Componentes:BalaoSaibaMais ID="bmsPeriodoPagamento" runat="server" ToolTipText="O Plano com Pagamento Recorrente será cobrado mensalmente no vencimento do plano. Meios de pagamento disponíveis para esse tipo: Cartão de Crédito e Débito Hsbc." Text="Saiba mais" ToolTipTitle="Período de Pagamento" CssClassLabel="balao_saiba_mais" ShowOnMouseover="true" />
            </div>
            <%--FIM Pagamento Recorrente --%>
            <%--Linha Valor da parcela --%>
            <div class="linha" id="divValorParcela" runat="server">
                <asp:Label ID="lblValorParcela" runat="server" Text="Valor da Parcela" CssClass="label_principal" />
                <div class="container_campo">
                    <componente:ValorDecimal ID="txtValorParcela" runat="server" ValidationGroup="Salvar" />
                </div>
            </div>
            <%--Linha Valor da parcela --%>
            <%--Linha Enviar Para--%>
            <div class="linha" id="divEnviarPara" runat="server">
                <asp:Label ID="lblEnviarPara" runat="server" Text="Enviar Para" CssClass="label_principal" />
                <div class="container_campo">
                    <componente:AlfaNumerico ID="txtEnviarPara" runat="server" ValidationGroup="Salvar" />
                </div>
            </div>
            <%--FIM  Enviar Para --%>
            <%--Linha Enviar Nota Fiscal--%>
            <div class="linha" id="divEmitirNotaFiscal" runat="server">
                <asp:Label ID="lblEnviarNF" runat="server" Text="Enviar Nota Fiscal" CssClass="label_principal" />
                <div class="container_campo">
                    <asp:RadioButton ID="rbSim" runat="server" GroupName="EnviarNF" />
                    <span>Sim</span>
                    <asp:RadioButton ID="rbNao" runat="server" GroupName="EnviarNF" />
                    <span>Não</span>
                </div>
            </div>
            <%--FIM Enviar Nota Fiscal --%>
            <%--Linha Nome Completo--%>
            <div class="linha" id="divNomeCompleto" runat="server">
                <asp:Label ID="lblNomeCompleto" runat="server" Text="Nome Completo" CssClass="label_principal" />
                <div class="container_campo">
                    <componente:AlfaNumerico ID="txtNomeCompleto" ValidationGroup="Salvar" runat="server" />
                </div>
            </div>
            <%--FIM Nome Completo --%>
            <%--Linha Telefone Comercial--%>
            <div class="linha" id="divTelefoneComercial" runat="server">
                <asp:Label ID="lblTelefoneComercial" runat="server" Text="Telefone Comercial" CssClass="label_principal" />
                <div class="container_campo">
                    <componente:Telefone ID="txtTelefoneComercial" runat="server" ValidationGroup="Salvar" />
                </div>
            </div>
            <%--FIM Telefone Comercial --%>
            <%--Linha Observações--%>
            <div class="linha">
                <asp:Label ID="lblObservacoes" runat="server" Text="Observações" CssClass="label_principal" />
                <div class="container_campo">
                    <componente:AlfaNumerico ID="txtObservacoes" runat="server" Tipo="AlfaNumerico" TextMode="MultiLine"
                        MaxLength="2000" Columns="99999" Obrigatorio="false" CssClassTextBox="textarea_padrao"
                        ValidationGroup="Salvar" />
                </div>
            </div>
            <%--FIM Observações --%>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
<%-- Botões --%>
<asp:UpdatePanel ID="upPnlBotoes" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
            <asp:Button ID="btnBloqueioPlano" runat="server" CssClass="botao_padrao" Text="Bloquear"
                CausesValidation="false" OnClick="btnBloqueio_Click" />
            <asp:Button ID="btnDesbloqueioPlano" runat="server" CssClass="botao_padrao" Text="Desbloquear"
                CausesValidation="false" OnClick="btnBloqueio_Click" />
            <asp:Button ID="btnSalvar" runat="server" CssClass="botao_padrao" Text="Salvar" CausesValidation="true"
                ValidationGroup="Salvar" OnClick="btnSalvar_Click" />
            <asp:Button ID="btnLiberarPlano" runat="server" CssClass="botao_padrao" Text="Liberar Plano"
                CausesValidation="false" OnClick="btnLiberarPlano_Click" />
            <asp:Button ID="btnLiberarPlanoAdicional" runat="server" CssClass="botao_padrao"
                Text="Liberar Plano Adicional" CausesValidation="false" OnClick="btnLiberarPlanoAdicional_Click" />
            <asp:Button ID="btnVisualizarInformacoes" runat="server" CssClass="botao_padrao"
                Text="Visualizar Informações" CausesValidation="false" OnClick="btnVisualizarInformacoes_Click" />
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
<%-- Fim Botões --%>
<asp:Panel ID="pnlPlanos" runat="server" CssClass="blocodados ultimosdados">
    <h3>
        Planos</h3>
    <asp:UpdatePanel ID="upPnlPlanos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <telerik:RadGrid ID="gvPlanos" AllowPaging="True" AllowCustomPaging="true" CssClass="gridview_padrao"
                runat="server" Skin="Office2007" GridLines="None" OnPageIndexChanged="gvPlanos_PageIndexChanged"
                OnItemCommand="gvPlanos_ItemCommand" OnItemDataBound="gvPlano_ItemDataBound">
                <PagerStyle Mode="NextPrevNumericAndAdvanced" Position="TopAndBottom" />
                <AlternatingItemStyle CssClass="alt_row" />
                <MasterTableView DataKeyNames="Idf_Plano_Adquirido">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Tipo do Plano" ItemStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:Label ID="lblDescricaoPlano" runat="server" Text='<%# Eval("Des_Plano") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Data Início" ItemStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:Label ID="lblDtaInicioPlano" runat="server" Text='<%# Convert.ToDateTime(Eval("Dta_Inicio_Plano")).ToString("dd/MM/yyyy") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Data Fim" ItemStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:Label ID="lblDtaFimPlano" runat="server" Text='<%# Convert.ToDateTime(Eval("Dta_Fim_Plano")).ToString("dd/MM/yyyy") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Situação" ItemStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:Label ID="lblSituacaoPlano" runat="server" Text='<%# Eval("Des_Plano_Situacao") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Visualizações Utilizadas" ItemStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:Label ID="lblVisualizacoes" runat="server" Text='<%# Eval("Qtd_Visualizacao") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="SMS Utilizados" ItemStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:Label ID="lblSMS" runat="server" Text='<%# Eval("Qtd_SMS") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ações" ItemStyle-CssClass="col_action" ItemStyle-Width="150px">
                            <ItemTemplate>
                                <asp:ImageButton ID="btGerarPdfBoletos" runat="server" ImageUrl="../../../img/icn_gerar_pdf_boleto.png" 
                                    ToolTip="Gerar PDFs de Boletos" AlternateText="--" CommandName="GerarPdfBoletos" CausesValidation="false" />
                                <asp:ImageButton ID="btiEditarPlano" runat="server" ImageUrl="../../../img/icn_editar_lapis.png"
                                    ToolTip="Editar Plano" AlternateText="--" CommandName="EditarPlano" CausesValidation="false" />
                                <asp:ImageButton ID="btiEditarParcelas" runat="server" ImageUrl="../../../img/icn_editarvagas.png"
                                    ToolTip="Editar/Visualizar Parcelas" AlternateText="--" CommandName="Editar"
                                    CausesValidation="false" />
                                <asp:ImageButton ID="btiExcluir" runat="server" ImageUrl="../../../img/icn_excluirvaga.png"
                                    ToolTip="Excluir" AlternateText="--" CommandName="Excluir" Visible='<%# (Convert.ToInt32(Eval("Idf_Plano_Situacao")).Equals(0) || Convert.ToInt32(Eval("Idf_Plano_Situacao")).Equals(1) ) ? true : false %>'
                                    CausesValidation="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<%-- Modal de seleção de cliente --%>
<asp:Panel ID="pnlSelecionarCliente" runat="server" CssClass="modal_conteudo financeiro"
    Style="display: none;">
    <h2 class="titulo_modal">
        <span>Selecionar Cliente</span>
    </h2>
    <asp:UpdatePanel ID="upBtiSelecionarClienteFechar" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:ImageButton CssClass="botao_fechar_modal" ID="btiSelecionarClienteFechar" ImageUrl="/img/botao_padrao_fechar.gif"
                runat="server" CausesValidation="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upPnlSelecionarCliente" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <telerik:RadGrid ID="gvSelecionarCliente" CssClass="gridview_padrao" OnItemCommand="gvSelecionarCliente_ItemCommand"
                runat="server" AllowPaging="True" OnPageIndexChanged="gvSelecionarCliente_PageIndexChanged">
                <MasterTableView DataKeyNames="Idf_Pessoa_Fisica, Idf_Filial">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Nome/Razão Social">
                            <ItemTemplate>
                                <asp:LinkButton ID="btlNomeRazao" runat="server" CommandName="Selecionar" Text='<%# Eval("Nome") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CPF/CNPJ">
                            <ItemTemplate>
                                <asp:LinkButton ID="btlCPFCNPJ" runat="server" CommandName="Selecionar" Text='<%# Eval("Cadastro Pessoa") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Data de Nascimento">
                            <ItemTemplate>
                                <asp:LinkButton ID="btlDataNascimento" runat="server" CommandName="Selecionar" Text='<%# String.IsNullOrEmpty(Eval("Dta_Nascimento").ToString()) ? " " : Convert.ToDateTime(Eval("Dta_Nascimento")).ToString("dd/MM/yyyy") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cidade">
                            <ItemTemplate>
                                <asp:LinkButton ID="btlNomeCidade" runat="server" CommandName="Selecionar" Text='<%# Eval("Nme_Cidade") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="Panel2" runat="server" CssClass="painel_botoes centro">
        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="botao_padrao"
            CausesValidation="false" OnClick="btnCancelar_Click" />
    </asp:Panel>
</asp:Panel>
<asp:HiddenField ID="hfSelecionarCliente" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeSelecionarCliente" runat="server" CancelControlID="btiSelecionarClienteFechar"
    PopupControlID="pnlSelecionarCliente" TargetControlID="hfSelecionarCliente">
</AjaxToolkit:ModalPopupExtender>
<%-- FIM: Modal de seleção de cliente --%>
<%--Modal para Liberar Cliente--%>
<asp:Panel ID="pnlLiberarCliente" runat="server" CssClass="modal_conteudo financeiro"
    Style="display: none;">
    <h2 class="titulo_modal">
        <span>Liberar</span>
    </h2>
    <asp:UpdatePanel ID="upBtiLiberarClienteFechar" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:ImageButton CssClass="botao_fechar_modal" ID="btiLiberarClienteFechar" ImageUrl="/img/botao_padrao_fechar.gif"
                runat="server" CausesValidation="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel CssClass="coluna_esquerda" ID="pnlColunaEsquerda" runat="server" Width="150px">
        <asp:Panel CssClass="painel_imagem" ID="pnlEsquerdaImagem" runat="server">
            <img alt="" class="icone" src="../../../img/SalaSelecionadora/icn_meuplano.png" />
        </asp:Panel>
    </asp:Panel>
    <asp:Panel CssClass="coluna_direita" ID="pnlDadosPlano" runat="server">
        <asp:UpdatePanel ID="upDados" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="linha">
                    <asp:Label ID="lblNomeLiberarCliente" runat="server" CssClass="label_principal" />
                    <span class="container_campo">
                        <asp:Literal ID="litNomeLiberarCliente" runat="server"></asp:Literal></span>
                </div>
                <div class="linha">
                    <asp:Label ID="lblTipoPlanoLiberarCliente" runat="server" CssClass="label_principal"
                        Text="Tipo do plano" />
                    <span class="container_campo">
                        <asp:Literal ID="litTipoPlanoLiberarCliente" runat="server"></asp:Literal>
                        <telerik:RadComboBox runat="server" ID="rcbTipoPlano" EmptyMessage=" " ZIndex="100000"
                            AllowCustomText="false" CssClass="checkbox_large">
                        </telerik:RadComboBox>
                    </span>
                </div>
                <asp:Panel ID="pnlAtualizarPlano" runat="server">
                    <div class="linha">
                        <asp:Label ID="lblQuantidadeVisualizacao" runat="server" CssClass="label_principal"
                            Text="Quantidade de Visualização" />
                        <span class="container_campo">
                            <componente:AlfaNumerico ID="txtQuantidadeVisualizacao" runat="server" CssClassTextBox="textbox_padrao"
                                Obrigatorio="true" Columns="5" ValidationGroup="AtualizarPlano" />
                            <asp:Label ID="lblQuantidadeVisualizacaoAtual" runat="server"></asp:Label>
                        </span>
                    </div>
                    <div class="linha">
                        <asp:Label ID="lblQuantidadeSMS" runat="server" CssClass="label_principal" Text="Quantidade de SMS" />
                        <span class="container_campo">
                            <componente:AlfaNumerico ID="txtQuantidadeSMS" runat="server" CssClassTextBox="textbox_padrao"
                                Obrigatorio="true" Columns="5" ValidationGroup="AtualizarPlano" />
                            <asp:Label ID="lblQuantidadeSMSAtual" runat="server"></asp:Label>
                        </span>
                    </div>
                    <div class="linha">
                        <asp:Label ID="lblQuantidadeCampanha" runat="server" CssClass="label_principal" Text="Quantidade de Campanha" />
                        <span class="container_campo">
                            <componente:AlfaNumerico ID="txtQuantidadeCampanha" runat="server" CssClassTextBox="textbox_padrao"
                                Obrigatorio="true" Columns="5" ValidationGroup="AtualizarPlano" />
                            <asp:Label ID="lblQuantidadeCampanhaAtual" runat="server"></asp:Label>
                        </span>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlBotoesLiberarPlano" runat="server" CssClass="painel_botoes centro">
                    <asp:Button ID="btnLiberarCliente" runat="server" Text="Liberar Cliente" CssClass="botao_padrao"
                        CausesValidation="false" OnClick="btnLiberarCliente_Click" />
                    <asp:Button ID="btnAtualizarPlano" runat="server" Text="Atualizar Plano" CssClass="botao_padrao"
                        CausesValidation="true" ValidationGroup="AtualizarPlano" OnClick="btnAtualizarPlano_Click" />
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Panel>
<asp:HiddenField ID="hfLiberarCliente" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeLiberarCliente" runat="server" PopupControlID="pnlLiberarCliente"
    CancelControlID="btiLiberarClienteFechar" TargetControlID="hfLiberarCliente">
</AjaxToolkit:ModalPopupExtender>
<%--Modal para Liberar Cliente--%>
<%--Modal para Liberar Plano Adicional --%>
<asp:Panel ID="pnlLiberarPlanoAdicional" runat="server" CssClass="modal_conteudo financeiro"
    Style="display: none;">
    <h2 class="titulo_modal">
        <span>Liberar Plano Adicional</span>
    </h2>
    <asp:ImageButton CssClass="botao_fechar_modal" ID="btiLiberarPlanoAdicionalFechar"
        ImageUrl="/img/botao_padrao_fechar.gif" runat="server" CausesValidation="false" />
    <asp:Panel CssClass="coluna_esquerda" ID="pnlEsquerdaLiberarPlanoAdicional" runat="server"
        Width="150px">
        <asp:Panel CssClass="painel_imagem" ID="Panel4" runat="server">
            <img alt="" class="icone" src="../../../img/SalaSelecionadora/icn_meuplano.png" />
        </asp:Panel>
    </asp:Panel>
    <asp:Panel CssClass="coluna_direita" ID="pnlDireitaLiberarPlanoAdicional" runat="server">
        <asp:UpdatePanel ID="upDadosLiberarPlanoAdicional" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <telerik:RadGrid ID="gvLiberarPlanoAdicional" CssClass="gridview_padrao" OnItemCommand="gvLiberarPlanoAdicional_ItemCommand"
                    OnPageIndexChanged="gvLiberarPlanoAdicional_PageIndexChanged" runat="server"
                    AllowPaging="True" AllowCustomPaging="true">
                    <MasterTableView DataKeyNames="Idf_Adicional_Plano">
                        <Columns>
                            <telerik:GridBoundColumn HeaderText="Tipo" DataField="Des_Tipo_Adicional" ItemStyle-CssClass="center">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Quantidade" DataField="Qtd_Adicional" ItemStyle-CssClass="center">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Valor" DataField="Vlr_Pagamento" ItemStyle-CssClass="center">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Status" DataField="Des_Adicional_Plano_Situacao"
                                ItemStyle-CssClass="center">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="Liberar" ItemStyle-CssClass="col_action">
                                <ItemTemplate>
                                    <%-- É mostrado apenas se a parcela está em aguardando liberação --%>
                                    <asp:ImageButton ID="btiLiberarPlanoAdicional" runat="server" ImageUrl="../../../img/img_icone_check_16x16.png"
                                        ToolTip="Liberar Plano Adicional" AlternateText="Liberar Plano Adicional" Visible='<%# AjustarVisibilidadeBotaoLiberarAdicional(Convert.ToInt32(Eval("Idf_Adicional_Plano_Situacao"))) %>'
                                        CommandName="Liberar" CausesValidation="false" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Panel>
<asp:HiddenField ID="hfLiberarPlanoAdicional" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeLiberarPlanoAdicional" runat="server" PopupControlID="pnlLiberarPlanoAdicional"
    CancelControlID="btiLiberarPlanoAdicionalFechar" TargetControlID="hfLiberarPlanoAdicional">
</AjaxToolkit:ModalPopupExtender>
<%--FIM: Modal para Liberar Plano Adicional --%>
<%--Modal para Criar um Plano Adicional--%>
<asp:Panel ID="pnlCriarPlanoAdicional" runat="server" CssClass="modal_conteudo financeiro"
    Style="display: none;">
    <h2 class="titulo_modal">
        <span>Criar Plano Adicional</span>
    </h2>
    <asp:UpdatePanel ID="upBtiCriarPlanoAdicionalFechar" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:ImageButton CssClass="botao_fechar_modal" ID="btiCriarPlanoAdicionalFechar"
                ImageUrl="/img/botao_padrao_fechar.gif" runat="server" CausesValidation="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel CssClass="coluna_esquerda" ID="Panel3" runat="server" Width="150px">
        <asp:Panel CssClass="painel_imagem" ID="Panel1" runat="server">
            <img alt="" class="icone" src="../../../img/SalaSelecionadora/icn_meuplano.png" />
        </asp:Panel>
    </asp:Panel>
    <asp:Panel CssClass="coluna_direita" ID="pnlCriarPlanoAdicionalColunaDireita" runat="server">
        <asp:UpdatePanel ID="upCriarPlanoAdicionalDados" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="linha">
                    <asp:Label ID="lblCriarPlanoAdicionalTipoAdicional" runat="server" CssClass="label_principal"
                        Text="Tipo do Adicional" />
                    <span class="container_campo">
                        <telerik:RadComboBox ID="rcbCriarPlanoAdicionalTipoAdicional" runat="server" EmptyMessage=" "
                            ZIndex="100000" AllowCustomText="false">
                        </telerik:RadComboBox>
                    </span>
                </div>
                <div class="linha">
                    <asp:Label ID="lblCriarPlanoAdicionalQuantidadeSMS" runat="server" CssClass="label_principal"
                        Text="Quantidade de SMS" />
                    <span class="container_campo">
                        <componente:AlfaNumerico ID="txtCriarPlanoAdicionalQuantidadeSMS" runat="server"
                            Columns="5" ValorAlteradoClient="txtCriarPlanoAdicionalQuantidadeSMS_ValorAlteradoClient"
                            ValidationGroup="CriarPlanoAdicional" />
                    </span>
                </div>
                <div class="linha">
                    <asp:Label ID="lblCriarPlanoAdicionalValorSMS" runat="server" CssClass="label_principal"
                        Text="Valor por SMS" />
                    <span class="container_campo">
                        <componente:ValorDecimal ID="txtCriarPlanoAdicionalValorSMS" runat="server" Columns="5"
                            CasasDecimais="4" ValorAlteradoClient="txtCriarPlanoAdicionalValorSMS_ValorAlteradoClient"
                            ValidationGroup="CriarPlanoAdicional" />
                    </span>
                </div>
                <div class="linha">
                    <asp:Label ID="lblCriarPlanoAdicionalValorTotal" runat="server" CssClass="label_principal"
                        Text="Valor Total" />
                    <span class="container_campo">
                        <componente:ValorDecimal ID="txtCriarPlanoAdicionalValorTotal" runat="server" Columns="5"
                            ValorAlteradoClient="txtCriarPlanoAdicionalValorTotal_ValorAlteradoClient" ValidationGroup="CriarPlanoAdicional" />
                    </span>
                </div>
                <div class="linha">
                    <asp:Label ID="lblCriarPlanoAdicionalDataVencimento" runat="server" CssClass="label_principal"
                        Text="Vencimento Boleto" />
                    <span class="container_campo">
                        <componente:Data ID="txtCriarPlanoAdicionalDataVencimento" runat="server" ValidationGroup="CriarPlanoAdicional" />
                    </span>
                </div>
                <asp:Panel ID="Panel5" runat="server" CssClass="painel_botoes centro">
                    <asp:Button ID="btnCriarAdicionalSalvar" runat="server" Text="Salvar" CssClass="botao_padrao"
                        CausesValidation="true" ValidationGroup="CriarPlanoAdicional" OnClick="btnCriarAdicionalSalvar_Click" />
                    <asp:Button ID="btnCriarAdicionalCancelar" runat="server" Text="Cancelar" CssClass="botao_padrao"
                        CausesValidation="false" OnClick="btnCriarAdicionalCancelar_Click" />
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Panel>
<asp:HiddenField ID="hfCriarPlanoAdicional" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeCriarPlanoAdicional" runat="server" PopupControlID="pnlCriarPlanoAdicional"
    CancelControlID="btiCriarPlanoAdicionalFechar" TargetControlID="hfCriarPlanoAdicional">
</AjaxToolkit:ModalPopupExtender>
<%--Modal para Criar um Plano Adicional--%>
<%-- Modal de Visualizacao de Informação --%>
<asp:Panel ID="pnlInformacoes" runat="server" CssClass="modal_conteudo financeiro"
    Style="display: none; max-height:630px; min-height:300px; overflow:auto;">
    <h2 class="titulo_modal">
        <span>Informação</span>
    </h2>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:ImageButton CssClass="botao_fechar_modal" ID="btiInformacoesFechar" ImageUrl="/img/botao_padrao_fechar.gif"
                runat="server" CausesValidation="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <uc2:ucObservacaoFilial ID="ucObservacaoFilial" runat="server" />
    <div class="linha">
        <asp:Label ID="lblSituacao" runat="server" CssClass="label_principal" Text="Situação"
            AssociatedControlID="rcbSituacaoEmpresa" />
        <asp:UpdatePanel ID="upSituacaoEmpresa" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <telerik:RadComboBox ID="rcbSituacaoEmpresa" runat="server" ZIndex="1000050">
                </telerik:RadComboBox>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="linha">
        <asp:Label ID="lblQuantidadeUsuario" runat="server" CssClass="label_principal" Text="Quantidade de Usuários"
            AssociatedControlID="txtQuantidadeUsuario" />
        <asp:UpdatePanel ID="upQuantidadeUsuario" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <componente:AlfaNumerico ID="txtQuantidadeUsuario" runat="server" Tipo="Numerico"
                    Obrigatorio="False" CssClassTextBox="textbox_padrao" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:Panel ID="Panel7" runat="server" CssClass="painel_botoes centro">
        <asp:Button ID="btnSalvarSituacaoFilial" runat="server" Text="Salvar" CssClass="botao_padrao"
            CausesValidation="true" OnClick="btnSalvarSituacaoFilial_Click" />
    </asp:Panel>
</asp:Panel>
<asp:HiddenField ID="hfInformacoes" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeInformacoes" runat="server" PopupControlID="pnlInformacoes"
    CancelControlID="btiInformacoesFechar"  TargetControlID="hfInformacoes">
</AjaxToolkit:ModalPopupExtender>
<%-- FIM: Modal de Visualizacao de Informação --%>
<%-- Modal de confirmação de exclusão --%>
<uc1:ConfirmacaoExclusao ID="ucConfirmacaoExclusao" runat="server" />
<%-- FIM: Modal de confirmação de exclusão --%>
<asp:Panel ID="pnlVisualizacaoBoleto" runat="server" CssClass="modal_conteudo reduzida" Style="display: none;">
    <h2 class="titulo_modal">
        <span>Visualização de Boleto</span>
    </h2>
    <asp:UpdatePanel ID="upBtiFechar" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:ImageButton CssClass="botao_fechar_modal" ID="btiFechar" ImageUrl="/img/botao_padrao_fechar.gif"
                runat="server" CausesValidation="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel CssClass="coluna_esquerda" ID="Panel8" runat="server">
        <asp:Panel CssClass="painel_imagem" ID="Panel9" runat="server">
            <img alt="" class="icone" src="../../../img/SalaSelecionadora/icn_meuplano.png" />
        </asp:Panel>
    </asp:Panel>
    <asp:Panel CssClass="coluna_direita" ID="pnlColunaDireita" runat="server">
        <asp:UpdatePanel ID="upDadosBoletos" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HyperLink ID="hlPDF" runat="server" Text="PDF" Target="_blank"></asp:HyperLink>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Panel>
<asp:HiddenField ID="hfVisualizacaoBoleto" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeVisualizacaoBoleto" runat="server" PopupControlID="pnlVisualizacaoBoleto"
    CancelControlID="btiFechar" TargetControlID="hfVisualizacaoBoleto">
</AjaxToolkit:ModalPopupExtender>