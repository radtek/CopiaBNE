<%@ Control
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="RastreioCV.ascx.cs"
    Inherits="BNE.Web.UserControls.Forms.SalaSelecionador.RastreioCV" %>
<%@ Register
    Src="../../Modais/ConfirmacaoExclusao.ascx"
    TagName="ConfirmacaoExclusao"
    TagPrefix="uc2" %>
<div class="painel_padrao_sala_selecionador">
    <p>
        Informe um
        novo rastreador
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
        e-mail.</p>
    <%-- Grid de resultado--%>
    <div class="uctabs_sala_selecionador">
        <!-- Painel: Confirmacao Cadastro -->
        <asp:Label
            ID="lblTitulo"
            CssClass="titulo_uctabs"
            runat="server"
            Text="Rastreador de Talentos"></asp:Label>
        <asp:UpdatePanel
            ID="upGvRastreador"
            runat="server"
            UpdateMode="Conditional">
            <ContentTemplate>
                <telerik:RadGrid
                    ID="gvRastreador"
                    AllowPaging="True"
                    AllowCustomPaging="true"
                    CssClass="gridview_padrao"
                    runat="server"
                    Skin="Office2007"
                    GridLines="None"
                    OnItemCommand="gvRastreador_ItemCommand"
                    OnPageIndexChanged="gvRastreador_PageIndexChanged">
                    <PagerStyle
                        Mode="NextPrevNumericAndAdvanced"
                        Position="TopAndBottom" />
                    <AlternatingItemStyle
                        CssClass="alt_row" />
                    <MasterTableView
                        DataKeyNames="Idf_Rastreador">
                        <Columns>
                            <telerik:GridTemplateColumn
                                HeaderText="Código"
                                ItemStyle-CssClass="codigo centro">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblIdVaga"
                                        runat="server"
                                        Text='<%# Eval("Idf_Rastreador") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn
                                HeaderText="Função"
                                ItemStyle-CssClass="descricao_vaga2">
                                <ItemTemplate>
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
                                HeaderText="CV's Recebidos"
                                ItemStyle-CssClass="cv_recebida centro">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblCvsRecebidos"
                                        runat="server"
                                        Text='<%# Eval("Qtd_Curriculo") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn
                                HeaderText="Ações"
                                ItemStyle-CssClass="col_action">
                                <ItemTemplate>
                                    <asp:ImageButton
                                        ID="btiVisualizarCurriculo"
                                        runat="server"
                                        ImageUrl="../../../img/icn_binoculo.png"
                                        ToolTip="Visualizar Currículo"
                                        AlternateText="Visualizar Currículo"
                                        CommandName="VisualizarCurriculo" />
                                    <asp:ImageButton
                                        ID="btiEditar"
                                        runat="server"
                                        ImageUrl="../../../img/icn_editar_lapis.png"
                                        ToolTip="Editar Rastreador"
                                        AlternateText="Editar Rastreador"
                                        CommandName="EditarRastreador" />
                                    <asp:ImageButton
                                        ID="btiExcluir"
                                        runat="server"
                                        ImageUrl="../../../img/icn_excluirvaga.png"
                                        ToolTip="Excluir"
                                        AlternateText="Excluir Rastreador"
                                        CommandName="ExcluirRastreador" />
                                    <%--<AjaxToolkit:ConfirmButtonExtender
                                    ID="btiExcluir_ConfirmButtonExtender"
                                    runat="server"
                                    ConfirmText="Confirma a exclusão deste item?"
                                    Enabled="True"
                                    TargetControlID="btiExcluir">
                                </AjaxToolkit:ConfirmButtonExtender>--%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
<%-- Fim Grid de resultado--%>
<%-- Botões --%>
<asp:Panel
    ID="pnlBotoes"
    runat="server"
    CssClass="painel_botoes">
    <asp:Button
        ID="btnInserirPerfil"
        runat="server"
        CssClass="botao_padrao"
        Text="Incluir Novo Perfil"
        CausesValidation="false"
        PostBackUrl="/SalaSelecionadorCadastroRastreadorCV.aspx" />
    <asp:Button
        ID="btnVoltar"
        runat="server"
        CssClass="botao_padrao"
        Text="Voltar"
        CausesValidation="false"
        OnClick="btnVoltar_Click" />
</asp:Panel>
<%-- Fim Botões --%>
<uc2:ConfirmacaoExclusao
    ID="ucConfirmacaoExclusao"
    runat="server" />
