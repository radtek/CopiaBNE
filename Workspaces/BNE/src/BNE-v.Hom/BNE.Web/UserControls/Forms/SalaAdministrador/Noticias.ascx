<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Noticias.ascx.cs" Inherits="BNE.Web.UserControls.Forms.SalaAdministrador.Noticias" %>
<%@ Register Src="NoticiasEditar.ascx" TagName="NoticiasEditar" TagPrefix="uc1" %>
<%@ Register Src="../../Modais/ConfirmacaoExclusao.ascx" TagName="ConfirmacaoExclusao"
    TagPrefix="uc2" %>
<asp:UpdatePanel ID="upPnlNoticias" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="pnlNoticias" runat="server">
            <div class="painel_configuracao_conteudo">
                <h3>
                    Notícias</h3>
                <p>
                    Pesquise por data de publicação ou titulo da notícia.</p>
                <div>
                    <telerik:RadTextBox runat="server" ID="tbxFiltroBusca" TextMode="SingleLine" ValidationGroup="Filtrar"
                        EmptyMessage="" CssClass="textbox_padrao_pesquisa">
                    </telerik:RadTextBox>
                    <asp:Button ID="btnFiltrar" runat="server" CssClass="botao_padrao filtrar" Text="Pesquisar"
                        ToolTip="Filtrar" CausesValidation="True" ValidationGroup="Filtrar" 
                        onclick="btnFiltrar_Click" />
                </div>
                <telerik:RadGrid ID="gvNoticias" AllowPaging="True" AllowCustomPaging="true" CssClass="gridview_padrao"
                    runat="server" Skin="Office2007" GridLines="None" OnPageIndexChanged="gvNoticias_PageIndexChanged"
                    OnItemCommand="gvNoticias_ItemCommand">
                        <PagerStyle
        PagerTextFormat=" {4} Notícias {2} a {3} de {5}"
        FirstPageToolTip="Primeira página"
        LastPageToolTip="Ultima página"
        NextPageToolTip="Próxima página"
        PrevPageToolTip="Página anterior" Position="TopAndBottom"/>
        
        
                       <AlternatingItemStyle CssClass="alt_row" />
                    <MasterTableView DataKeyNames="Idf_Noticia">
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Data de Publicação" ItemStyle-CssClass="dt_publicacao center">
                                <ItemTemplate>
                                    <asp:Label ID="lblDataPublicacao" runat="server" Text='<%# Eval("Dta_Publicacao") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Título de Notícias" ItemStyle-CssClass="tit_noticia">
                                <ItemTemplate>
                                    <asp:Label ID="lblTitulo" runat="server" Text='<%# Eval("Nme_Titulo_Noticia") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Exibição no Site" ItemStyle-CssClass="exibir_site center">
                                <ItemTemplate>
                                    <asp:Label ID="lblExibicao" runat="server" Text='<%# Eval("Des_Exibicao") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Ações" ItemStyle-CssClass="col_action">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnEditarNotícia" runat="server" ImageUrl="../../../img/icn_editar_lapis.png"
                                        ToolTip="Editar Notícia" AlternateText="Editar Notícia" CausesValidation="false"
                                        CommandName="Editar" />
                                    <asp:ImageButton ID="btnExcluirNoticia" runat="server" ImageUrl="../../../img/icn_excluirvaga.png"
                                        ToolTip="Excluir Notícia" AlternateText="Excluir Notícia" CausesValidation="false"
                                        CommandName="Excluir" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
             <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
                <asp:Button ID="btnNovaNoticia" runat="server" CssClass="botao_padrao painelinterno"
                    Text="Nova Notícia" CausesValidation="false" OnClick="btnNovaNoticia_Click" />
            </asp:Panel>
            
             </div>


        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="upPnlNoticiasEditar" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="pnlNoticiasEditar" runat="server">
            <uc1:NoticiasEditar ID="ucNoticiasEditar" runat="server" />
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
<uc2:ConfirmacaoExclusao ID="ucConfirmacaoExclusao" runat="server" />
