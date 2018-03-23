<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Presidente.ascx.cs"
    Inherits="BNE.Web.UserControls.Forms.SalaAdministrador.Presidente" %>
<%@ Register Src="~/UserControls/Forms/SalaAdministrador/PresidenteEditarAgradecimento.ascx"
    TagName="PresidenteEditarAgradecimento" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/Forms/SalaAdministrador/PresidenteNovoAgradecimento.ascx"
    TagName="PresidenteNovoAgradecimento" TagPrefix="uc3" %>

<%@ Register
    Src="~/UserControls/Modais/ModalConfirmacao.ascx"
    TagName="ModalConfirmacao"
    TagPrefix="uc1" %>

<asp:UpdatePanel ID="upPrincipal" runat="server" UpdateMode="Conditional">
<ContentTemplate>

<asp:UpdatePanel ID="upAgradecimento" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="painel_configuracao_conteudo">
            <asp:Panel ID="idPesquisaAgradecimento" runat="server" CssClass="blocodados">
                <h3 class="titulo_configuracao_content">
                    <asp:Label ID="lblAgradecimentos" Text="Agradecimentos" runat="server"></asp:Label>
                </h3>
                <p>
                    Pesquise por Nome, Email ou Descrição do agradecimento
                </p>
                <div>
                    <asp:Label ID="Label1" runat="server" CssClass="label_principal"></asp:Label>
                    <telerik:RadTextBox runat="server" ID="tbxFiltroBusca" TextMode="SingleLine" ValidationGroup="Filtrar"
                        EmptyMessage="" CssClass="textbox_padrao_pesquisa">
                    </telerik:RadTextBox>
                    <asp:Button ID="btnFiltrar" runat="server" CssClass="botao_padrao filtrar" Text="Pesquisar"
                        ToolTip="Filtrar Agradecimentos" CausesValidation="True" 
                        ValidationGroup="Filtrar" onclick="btnFiltrar_Click" />
                </div>
            </asp:Panel>
            <asp:UpdatePanel ID="upGvAgradecimentos" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <telerik:RadGrid ID="gvAgradecimentos" AllowPaging="True" AllowCustomPaging="true" CssClass="gridview_padrao"
                        runat="server" Skin="Office2007" GridLines="None" OnPageIndexChanged="gvAgradecimentos_PageIndexChanged">
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" Position="TopAndBottom" />
                        <AlternatingItemStyle CssClass="alt_row" />
                        <MasterTableView>
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Data" ItemStyle-CssClass="dt_Cadastro center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblData" runat="server" Text='<%# Eval("Dta_Cadastro") %>'></asp:Label>
                                    </ItemTemplate>
                                   <ItemStyle CssClass="dt_Cadastro center" />
                                </telerik:GridTemplateColumn>
                                 <telerik:GridTemplateColumn HeaderText="Nome" ItemStyle-CssClass="des_Mensagem">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPessoa" runat="server" Text='<%# Eval("Nme_Pessoa") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="des_Mensagem center" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Email" ItemStyle-CssClass="des_Mensagem">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Eml_Pessoa") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="des_Mensagem center" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Cidade" ItemStyle-CssClass="des_Mensagem">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCidade" runat="server" Text='<%# Eval("Nme_Cidade") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="des_Mensagem center" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Descrição" ItemStyle-CssClass="des_Mensagem">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMensagem" runat="server" Text='<%# Eval("Des_Mensagem") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="des_Mensagem center" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Publicado" ItemStyle-CssClass="flg_Auditado center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAuditado" runat="server" Text='<%# Eval("Flg_Auditado") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="flg_Auditado center"  />
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Ações" ItemStyle-CssClass="col_action agradecimento center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEditarAgradecimento" runat="server" ImageUrl="../../../img/icn_editar_lapis.png"
                                            ToolTip="Editar Agradecimento" AlternateText="Editar Agradecimento" CommandName="EditarAgradecimento"
                                            CommandArgument='<%# Eval("Idf_Agradecimento") %>' OnClick="btnEditarAgradecimento_Click" />
                                        <asp:ImageButton ID="btnExcluirAgradecimento" runat="server" ImageUrl="../../../img/icn_excluirvaga.png"
                                            ToolTip="Excluir Agradecimento" AlternateText="Excluir Agradecimento" CommandName="ExcluirAgradecimento"
                                            CommandArgument='<%# Eval("Idf_Agradecimento") %>' OnClick="btnExcluirAgradecimento_Click" />
                                    </ItemTemplate>
                                  
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </ContentTemplate>
            </asp:UpdatePanel>
            <%-- Botão Salvar --%>
            <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
                <asp:Button ID="btnNovoAgradecimento" runat="server" CssClass="botao_padrao painelinterno"
                    Text="Novo Agradecimento" CausesValidation="false" OnClick="btnNovoAgradecimento_Click" />
            </asp:Panel>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="upEditarAgradecimento" runat="server" UpdateMode="Conditional"
    Visible="false">
    <ContentTemplate>
        <uc2:PresidenteEditarAgradecimento ID="ucPresidenteEditarAgradecimento" runat="server"  />
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="UpNovoAgradecimento" runat="server" UpdateMode="Conditional"
    Visible="false">
    <ContentTemplate>
        <uc3:PresidenteNovoAgradecimento ID="ucPresidenteNovoAgradecimento" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>

</ContentTemplate></asp:UpdatePanel>

<uc1:ModalConfirmacao ID="ucModalConfirmacao" runat="server" />