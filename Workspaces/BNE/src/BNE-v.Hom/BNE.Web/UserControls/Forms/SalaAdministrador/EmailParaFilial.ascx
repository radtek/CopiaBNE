<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmailParaFilial.ascx.cs" Inherits="BNE.Web.UserControls.Forms.SalaAdministrador.EmailParaFilial" %>
<%@ Register Src="../../Modais/ConfirmacaoExclusao.ascx" TagName="ConfirmacaoExclusao" TagPrefix="uc1" %>
<div class="painel_configuracao_conteudo">
    <div class="titulo_filtro">
        <h2 class="titulo_carta_content">
            <asp:Label ID="lblTitulo" Text="Email para Filial" runat="server"></asp:Label>
        </h2>
    </div>
    <asp:UpdatePanel ID="upGeral" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <%-- Painel Pesquisa --%>
            <asp:Panel ID="pnlPesquisa" runat="server">
                <h3>
                    Pesquisar
                </h3>
                <p>
                    Pesquise pelo email ou pelo nome do gerente da filial. Você também pode pesquisar pela cidade ou estado.
                </p>
                <asp:UpdatePanel ID="upPnlPesquisa" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div>
                            <componente:AlfaNumerico ID="txtPequisa" runat="server" CssClassTextBox="textbox_padrao" Obrigatorio="false" />
                            <asp:Button ID="btnPesquisaPesquisar" runat="server" CssClass="botao_padrao filtrar" Text="Pesquisar"
                                ToolTip="Pesquisar" CausesValidation="false" OnClick="btnPesquisaPesquisar_Click" />
                        </div>
                        <asp:UpdatePanel ID="upGvPesquisa" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <telerik:RadGrid ID="gvPesquisa" AllowPaging="True" AllowCustomPaging="true" CssClass="gridview_padrao"
                                    runat="server" Skin="Office2007" GridLines="None" OnItemCommand="gvPesquisa_ItemCommand" OnPageIndexChanged="gvPesquisa_PageIndexChanged">
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" Position="TopAndBottom" />
                                    <AlternatingItemStyle CssClass="alt_row" />
                                    <MasterTableView DataKeyNames="Idf_Grupo_Cidade">
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText="Email" ItemStyle-CssClass="center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmail" runat="server" Text='<%# RetornarTextoEncurtado(Eval("Email").ToString(), 40) %>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Filial" ItemStyle-CssClass="center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFilial" runat="server" Text='<%# RetornarTextoEncurtado(Eval("Nme_Filial").ToString(), 40) %>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Cidade" ItemStyle-CssClass="center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCidade" runat="server" Text='<%# RetornarTextoEncurtado(Eval("Nme_Cidade").ToString(), 40) %>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Ações" ItemStyle-CssClass="col_action" ItemStyle-Width="150px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btiExibirDetalhes" runat="server" ImageUrl="../../../img/icn_binoculo.png" ToolTip="Exibir Detalhes"
                                                        AlternateText="--" CommandName="ExibirDetalhes" CausesValidation="false" />
                                                    <asp:ImageButton ID="btiEditarEmail" runat="server" ImageUrl="../../../img/icn_editar_lapis.png" ToolTip="Editar Email"
                                                        AlternateText="--" CommandName="EditarEmail" CausesValidation="false" />
                                                    <asp:ImageButton ID="btiExcluir" runat="server" ImageUrl="../../../img/icn_excluirvaga.png" ToolTip="Excluir"
                                                        AlternateText="--" CommandName="Excluir" CausesValidation="false" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <%-- Painel Botões --%>
                <asp:Panel ID="pnlPesquisaBotoes" runat="server" CssClass="painel_botoes">
                    <asp:Button ID="btnPesquisaCadastro" runat="server" CssClass="botao_padrao painelinterno" Text="Cadastro"
                        CausesValidation="false" OnClick="btnPesquisaCadastro_Click" />
                    <asp:Button ID="btnPesquisaVoltar" runat="server" CssClass="botao_padrao painelinterno" Text="Voltar"
                        CausesValidation="false" OnClick="btnPesquisaVoltar_Click" />
                </asp:Panel>
                <%-- FIM: Painel Botões --%>
            </asp:Panel>
            <%-- FIM: Painel Pesquisa --%>
            <%-- Painel Cadastro --%>
            <asp:Panel ID="pnlCadastro" runat="server">
                <h3>
                    Cadastro das Cidades
                </h3>
                <p>
                    Cidades relacionadas
                </p>
                <asp:UpdatePanel ID="upPnlCadastro" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="linha">
                            <asp:Label ID="lblEstado" runat="server" Text="Estado" CssClass="label_principal filial" AssociatedControlID="ccbEstado" />
                            <div class="container_campo">
                                <Employer:ComboCheckbox ID="ccbEstado" runat="server" EmptyMessage="Estado" ValidationGroup="Cadastrar"
                                    AutoPostBack="true" OnClientCheckItem="ccbEstado_ClientCheckItem" OnClientDropDownClosing="ccbEstado_ClientCheckItem" />
                            </div>
                            <asp:Label ID="lblCidade" runat="server" Text="Cidade" AssociatedControlID="ccbCidade" />
                            <div class="container_campo">
                                <asp:UpdatePanel ID="upCcbCidade" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnCarregarCidades" runat="server" CssClass="botao_padrao painelinterno" Text="Carregar Cidades"
                                            CausesValidation="false" OnClick="btnCarregarCidades_Click" Style="display: none" />
                                        <Employer:ComboCheckbox ID="ccbCidade" ShowCheckAllButton="true" runat="server" EmptyMessage="Cidade"
                                            ValidationGroup="Cadastrar" AutoPostBack="false" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <p>
                            Para obter os dados do Gerente insira o nome da filial Employer
                        </p>
                        <%-- Linha Filial --%>
                        <div class="linha">
                            <asp:Label ID="lblFilial" runat="server" Text="Filial" CssClass="label_principal filial" AssociatedControlID="txtFilial" />
                            <span class="container_campo">
                                <asp:UpdatePanel ID="upTxtFilial" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div>
                                            <asp:CustomValidator ID="cvFilial" runat="server" ErrorMessage="Filial Inválida." ClientValidationFunction="cvFilial_Validate"
                                                ControlToValidate="txtFilial" ValidationGroup="Adicionar"></asp:CustomValidator>
                                        </div>
                                        <div class="container_campo filial">
                                            <asp:TextBox ID="txtFilial" runat="server" CssClass="textbox_padrao textbox" Columns="32" AutoPostBack="True"
                                                OnTextChanged="txtFilial_TextChanged"></asp:TextBox>
                                            <AjaxToolkit:AutoCompleteExtender ID="aceFilial" runat="server" TargetControlID="txtFilial" ServiceMethod="ListarFiliaisEmployer"
                                                UseContextKey="false">
                                            </AjaxToolkit:AutoCompleteExtender>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </span>
                        </div>
                        <%-- FIM: Linha Filial --%>
                        <%-- Linha Nome --%>
                        <div class="linha">
                            <asp:Label ID="lblNome" runat="server" Text="Nome" CssClass="label_principal filial" AssociatedControlID="txtNome" />
                            <span class="container_campo">
                                <asp:UpdatePanel ID="upTxtNome" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <componente:AlfaNumerico ID="txtNome" runat="server" ValidationGroup="Adicionar" Obrigatorio="true" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </span>
                            <asp:UpdatePanel ID="upCkbResponsavel" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                <ContentTemplate>
                                    <asp:CheckBox ID="ckbResponsavel" runat="server" AutoPostBack="True" OnCheckedChanged="ckbResponsavel_CheckedChanged" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:Label ID="lblResponsavel" runat="server" Text="Responsável" />
                        </div>
                        <%-- FIM: Linha Nome --%>
                        <%--Linha Telefone--%>
                        <div class="linha">
                            <asp:Label runat="server" ID="lblTelefone" CssClass="label_principal filial" Text="Telefone" AssociatedControlID="txtTelefone" />
                            <span class="container_campo">
                                <asp:UpdatePanel ID="upTxtTelefone" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <componente:Telefone ID="txtTelefone" runat="server" ValidationGroup="Adicionar" Obrigatorio="false"
                                            CssClassTextBoxDDI="textbox_padrao ddi" CssClassTextBoxDDD="textbox_padrao ddd" CssClassTextBoxFone="textbox_padrao" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                        </div>
                        </div>
                        <%--FIM: Linha Telefone--%>
                        <%-- Linha E-Mail --%>
                        <div class="linha">
                            <asp:Label ID="lblEmail" AssociatedControlID="txtEmail" CssClass="label_principal filial" runat="server"
                                Text="E-mail" />
                            <div class="container_campo">
                                <asp:UpdatePanel ID="upTxtEmail" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <componente:AlfaNumerico ID="txtEmail" runat="server" ValidationGroup="Adicionar" Obrigatorio="true" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="linha">
                            <asp:Panel ID="pnlListaBotoes" runat="server" CssClass="painel_botoes">
                                <asp:UpdatePanel ID="upBtnListaSalvar" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnListaSalvar" runat="server" CssClass="botao_padrao painelinterno" Text="Adicionar"
                                            ValidationGroup="Adicionar" CausesValidation="true" OnClick="btiAdicionar_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>
                        </div>
                        <%-- FIM: Linha E-Mail --%>
                        <asp:UpdatePanel ID="upGvCadastroEmail" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <telerik:RadGrid ID="gvCadastroEmail" AllowPaging="True" AllowCustomPaging="true" CssClass="gridview_padrao"
                                    runat="server" Skin="Office2007" GridLines="None" OnItemCommand="gvCadastroEmail_ItemCommand" OnPageIndexChanged="gvCadastroEmail_PageIndexChanged">
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" Position="TopAndBottom" />
                                    <AlternatingItemStyle CssClass="alt_row" />
                                    <MasterTableView DataKeyNames="Idf_Email_Destinatario_Cidade, Flg_Responsavel">
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText="Responsável" ItemStyle-CssClass="center">
                                                <ItemTemplate>
                                                    <asp:Image ID="btiResponsavel" runat="server" ImageUrl="../../../img/img_circulo_azul_responsavel.png"
                                                        ToolTip="Responsável" AlternateText="--" Visible='<%# Convert.ToBoolean(Eval("Flg_Responsavel")) %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="Nme_Pessoa" HeaderText="Nome" ItemStyle-CssClass="center">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Des_Email" HeaderText="E-mail" ItemStyle-CssClass="center">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridTemplateColumn HeaderText="Ações" ItemStyle-CssClass="col_action" ItemStyle-Width="150px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btiEditarEmail" runat="server" ImageUrl="../../../img/icn_editar_lapis.png" ToolTip="Editar Email"
                                                        AlternateText="--" CommandName="EditarEmail" CausesValidation="false" />
                                                    <asp:ImageButton ID="btiExcluir" runat="server" ImageUrl="../../../img/icn_excluirvaga.png" ToolTip="Excluir"
                                                        AlternateText="--" CommandName="Excluir" CausesValidation="false" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <%-- Painel Botões --%>
                <asp:Panel ID="pnlCadastroBotoes" runat="server" CssClass="painel_botoes">
                    <asp:Button ID="btnCadastroSalvar" runat="server" CssClass="botao_padrao painelinterno" Text="Salvar"
                        ValidationGroup="Cadastrar" CausesValidation="true" OnClick="btnCadastroSalvar_Click" />
                    <asp:Button ID="btnCadastroLimpar" runat="server" CssClass="botao_padrao painelinterno" Text="Limpar"
                        CausesValidation="false" OnClick="btnCadastroLimpar_Click" />
                    <asp:Button ID="btnCadastroVoltar" runat="server" CssClass="botao_padrao painelinterno" Text="Voltar"
                        CausesValidation="false" OnClick="btnCadastroVoltar_Click" />
                </asp:Panel>
                <%-- FIM: Painel Botões --%>
            </asp:Panel>
            <%-- FIM: Painel Cadastro --%>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<%-- Detalhes --%>
<asp:Panel ID="pnlExibirDetalhes" runat="server" CssClass="modal_conteudo configuracao" Style="display: none;">
    <h2 class="titulo_modal">
        <span>Detalhes</span>
    </h2>
    <asp:UpdatePanel ID="upBtiExibirDetalhesFechar" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:ImageButton CssClass="botao_fechar_modal" ID="btiExibirDetalhesFechar" ImageUrl="/img/botao_padrao_fechar.png"
                runat="server" CausesValidation="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upGvExibirDetalhesPesquisa" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <telerik:RadGrid ID="gvExibirDetalhesPesquisa" AllowPaging="false" AllowCustomPaging="false" CssClass="gridview_padrao"
                runat="server" Skin="Office2007" GridLines="None" OnItemCommand="gvExibirDetalhesPesquisa_ItemCommand">
                <AlternatingItemStyle CssClass="alt_row" />
                <MasterTableView DataKeyNames="Idf_Grupo_Cidade">
                    <Columns>
                        <telerik:GridBoundColumn DataField="Nme_Responsavel" HeaderText="Responsável" ItemStyle-CssClass="center">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Email" ItemStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:Label ID="lblEmail" runat="server" Text='<%# RetornarTextoEncurtado(Eval("Email").ToString(), 1000) %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="Raz_Social" HeaderText="Filial" ItemStyle-CssClass="center">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Cidade" ItemStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:Label ID="lblCidade" runat="server" Text='<%# RetornarTextoEncurtado(Eval("Nme_Cidade").ToString(), 1000) %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ações" ItemStyle-CssClass="col_action" ItemStyle-Width="150px">
                            <ItemTemplate>
                                <asp:ImageButton ID="btiEditarEmail" runat="server" ImageUrl="../../../img/icn_editar_lapis.png" ToolTip="Editar Email"
                                    AlternateText="--" CommandName="EditarEmail" CausesValidation="false" />
                                <asp:ImageButton ID="btiExcluir" runat="server" ImageUrl="../../../img/icn_excluirvaga.png" ToolTip="Excluir"
                                    AlternateText="--" CommandName="Excluir" CausesValidation="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="Panel2" runat="server" CssClass="painel_botoes centro">
        <asp:Button ID="btnCancelar" runat="server" Text="Voltar" CssClass="botao_padrao" CausesValidation="false"
            OnClick="btnExibirDetalhesCancelar_Click" />
    </asp:Panel>
</asp:Panel>
<asp:HiddenField ID="hfExibirDetalhes" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeExibirDetalhes" runat="server" CancelControlID="btiExibirDetalhesFechar"
    PopupControlID="pnlExibirDetalhes" TargetControlID="hfExibirDetalhes">
</AjaxToolkit:ModalPopupExtender>
<%-- FIM: Detalhes --%>
<%-- Modal de confirmação de exclusão --%>
<uc1:ConfirmacaoExclusao ID="ucConfirmacaoExclusao" runat="server" />
<%-- FIM: Modal de confirmação de exclusão --%>
