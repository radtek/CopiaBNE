<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PresidenteRespostaAutomatica.ascx.cs"
    Inherits="BNE.Web.UserControls.Forms.SalaAdministrador.PresidenteRespostaAutomatica" %>

        <%@ Register Src="~/UserControls/Forms/SalaAdministrador/PresidenteEditarRespostaAutomatica.ascx" TagName="PresidenteEditarRespostaAutomatica" TagPrefix="uc5" %>
    <%@ Register Src="~/UserControls/Forms/SalaAdministrador/PresidenteNovaRespostaAutomatica.ascx" TagName="PresidenteNovaRespostaAutomatica" TagPrefix="uc6" %>
   
    <%@ Register Src="~/UserControls/Forms/SalaAdministrador/PresidenteRespAutoVisualizarTodas.ascx" TagName="PresidenteRespAutoVisualizarTodas" TagPrefix="uc7" %>
    <%@ Register
    Src="~/UserControls/Modais/ModalConfirmacao.ascx"
    TagName="ModalConfirmacao"
    TagPrefix="uc1" %>
<asp:UpdatePanel ID="upPrincipal" runat="server" UpdateMode="Conditional">
<ContentTemplate>

<asp:UpdatePanel ID="upRespostaAutomatica" runat="server" UpdateMode="Conditional">
<ContentTemplate>

<div class="painel_configuracao_conteudo">
    <h3 class="titulo_configuracao_content">
        Respostas Automáticas</h3>
    <p>
        Pesquise pelo código ou título da resposta.</p>
    <div>
        <telerik:RadTextBox runat="server" ID="tbxFiltroBusca" TextMode="SingleLine" ValidationGroup="Filtrar"
            EmptyMessage="" CssClass="textbox_padrao_pesquisa">
        </telerik:RadTextBox>
        <asp:Button ID="btnFiltrar" runat="server" CssClass="botao_padrao filtrar" Text="Pesquisar"
            ToolTip="Filtrar Empresas" CausesValidation="True" 
            ValidationGroup="Filtrar" onclick="btnFiltrar_Click" />
    </div>

    <asp:UpdatePanel
        ID="upGvRespostasAutomaticas"
        runat="server"
        UpdateMode="Conditional">
        <ContentTemplate>
            <telerik:RadGrid
                ID="gvRespostasAutomaticas"
                AllowPaging="True"
                AllowCustomPaging="true"
                CssClass="gridview_padrao"
                runat="server"
                Skin="Office2007"
                GridLines="None"
                OnPageIndexChanged="gvRespostasAutomaticas_PageIndexChanged">
                <PagerStyle
                    Mode="NextPrevNumericAndAdvanced"
                    Position="TopAndBottom" />
                <AlternatingItemStyle
                    CssClass="alt_row" />
                <MasterTableView
                    >
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Código" ItemStyle-CssClass="cod_resp_auto center">
                            <ItemTemplate>
                                <asp:Label ID="lblCodigo" runat="server" Text='<%# Eval("Idf_Resposta_Automatica") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="cod_resp_auto center" />
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Título da Resposta" ItemStyle-CssClass="tit_resp_auto ocenter">
                            <ItemTemplate>
                                <asp:Label ID="lblTitulo" runat="server" Text='<%# Eval("Nme_Resposta_Automatica") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="tit_resp_auto ocenter" />
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Ações" ItemStyle-CssClass="col_action agradecimento">
                            <ItemTemplate>
                                     <asp:ImageButton ID="btnEditarRespostaAutomatica" runat="server" ImageUrl="../../../img/icn_editar_lapis.png"
                                    ToolTip="Editar Resposta Automática" 
                                    AlternateText="Editar Resposta Automática" CausesValidation="false" 
                                     CommandArgument='<%# Eval("Idf_Resposta_Automatica") %>' onclick="btnEditarRespostaAutomatica_Click" />
                                <asp:ImageButton ID="btnExcluirRespostaAutomatica" runat="server" ImageUrl="../../../img/icn_excluirvaga.png"
                                    ToolTip="Excluir Resposta Automática" 
                                    AlternateText="Excluir Resposta Automática" CausesValidation="false" 
                                    CommandArgument='<%# Eval("Idf_Resposta_Automatica") %>'  onclick="btnExcluirRespostaAutomatica_Click" />
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>

        </ContentTemplate>

    </asp:UpdatePanel>


    <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
        <asp:Button ID="btnNovaResposta" runat="server" 
            CssClass="botao_padrao painelinterno" Text="Nova Resposta"
            CausesValidation="false" onclick="btnNovaResposta_Click" />
        <asp:Button ID="btnVerTodas" runat="server" 
            CssClass="botao_padrao painelinterno" Text="Ver Todas"
            CausesValidation="false" onclick="btnVerTodas_Click" />
    </asp:Panel>
</div>
</ContentTemplate>
     </asp:UpdatePanel>
<asp:UpdatePanel ID="upEditarRespostaAutomatica" runat="server" UpdateMode="Conditional" Visible="false">
    <ContentTemplate>
     <uc5:PresidenteEditarRespostaAutomatica ID="ucPresidenteEditarRespostaAutomatica" runat="server" />
     </ContentTemplate>
     </asp:UpdatePanel>
     <asp:UpdatePanel ID="upNovaRespostaAutomatica" runat="server" UpdateMode="Conditional"  Visible="false" >
    <ContentTemplate>
     <uc6:PresidenteNovaRespostaAutomatica ID="ucPresidenteNovaRespostaAutomatica" runat="server" />
        </ContentTemplate>
     </asp:UpdatePanel>
    <asp:UpdatePanel ID="upTodasRespostaAutomatica" runat="server" UpdateMode="Conditional"  Visible="false">
    <ContentTemplate>
     <uc7:PresidenteRespAutoVisualizarTodas ID="ucPresidenteRespAutoVisualizarTodas" runat="server" />
        </ContentTemplate>
     </asp:UpdatePanel>

</ContentTemplate>
</asp:UpdatePanel>

<uc1:ModalConfirmacao ID="ucModalConfirmacao" runat="server" />