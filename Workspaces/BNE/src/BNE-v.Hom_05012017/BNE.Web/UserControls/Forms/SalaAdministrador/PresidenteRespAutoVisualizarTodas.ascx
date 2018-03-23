<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PresidenteRespAutoVisualizarTodas.ascx.cs" Inherits="BNE.Web.UserControls.Forms.SalaAdministrador.PresidenteRespAutoVisualizarTodas" %>
<%@ Register
    Src="~/UserControls/Modais/ModalConfirmacao.ascx"
    TagName="ModalConfirmacao"
    TagPrefix="uc1" %>
      <%@ Register Src="~/UserControls/Forms/SalaAdministrador/PresidenteEditarRespostaAutomatica.ascx" TagName="PresidenteEditarRespostaAutomatica" TagPrefix="uc5" %>
<asp:UpdatePanel ID="upPrincipal" runat="server" UpdateMode="Conditional">
<ContentTemplate>

<asp:UpdatePanel ID="upRespostaAutomatica" runat="server" UpdateMode="Conditional">
<ContentTemplate>

<div class="painel_configuracao_conteudo">
    <h3 class="titulo_configuracao_content">
        Visualizar Todas</h3>
  
            <telerik:RadGrid
                ID="gvVisualizarTodas"
                AllowPaging="false"
                CssClass="gridview_padrao"
                runat="server"
                Skin="Office2007"
                GridLines="None">
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

                        <telerik:GridTemplateColumn HeaderText="Ações" ItemStyle-CssClass="col_action agradecimento ">
                            <ItemTemplate>
                                     <asp:ImageButton ID="btnEditarRespostaAutomatica" runat="server" ImageUrl="../../../img/icn_editar_lapis.png"
                                    ToolTip="Editar Resposta Automática" 
                                    AlternateText="Editar Resposta Automática" CausesValidation="false" 
                                      CommandArgument='<%# Eval("Idf_Resposta_Automatica") %>' onclick="btnEditarRespostaAutomatica_Click" />
                                <asp:ImageButton ID="btnExcluirRespostaAutomatica" runat="server" ImageUrl="../../../img/icn_excluirvaga.png"
                                    ToolTip="Excluir Resposta Automática" 
                                    AlternateText="Excluir Resposta Automática" CausesValidation="false" 
                                     CommandArgument='<%# Eval("Idf_Resposta_Automatica") %>' onclick="btnExcluirRespostaAutomatica_Click" />
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>

       
  
</div>
</ContentTemplate>
     </asp:UpdatePanel>

<asp:UpdatePanel ID="upEditarRespostaAutomatica" runat="server" UpdateMode="Conditional" Visible="false">
    <ContentTemplate>
     <uc5:PresidenteEditarRespostaAutomatica ID="ucPresidenteEditarRespostaAutomatica" runat="server" />
     </ContentTemplate>
     </asp:UpdatePanel>
</ContentTemplate>
</asp:UpdatePanel>

<uc1:ModalConfirmacao ID="ucModalConfirmacao" runat="server" />