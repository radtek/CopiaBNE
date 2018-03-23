<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrocarEmpresa.ascx.cs" Inherits="BNE.Web.UserControls.Modais.TrocarEmpresa" %>
<%@ Register Src="../UCLogoFilial.ascx" TagName="UCLogoFilial" TagPrefix="uc1" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/ModalConfirmacaoEnvioCurriculo.css" type="text/css" rel="stylesheet" />
<style>
    .logo_empresa
    {
        width: 50px;
        height: 50px;
    }
</style>
<asp:Panel ID="pnlTrocarEmpresa" runat="server" CssClass="modal_trocar_empresa" Style="display: none;">
    <asp:UpdatePanel ID="upTrocarEmpresa" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="head-modal">
                 <h2 class="titulo_modal">
                Trocar Empresa</h2>
            <asp:UpdatePanel ID="upBtiFechar" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:LinkButton  CssClass="botao_fechar_modal" ID="btiFechar" runat="server" CausesValidation="false" OnClick="btiFechar_Click" ><i class="fa fa-times-circle"></i> Fechar</asp:LinkButton> 
                </ContentTemplate>
            </asp:UpdatePanel>

            </div>
           
            <asp:Panel runat="server" ID="pnlTrocarEmpresaConteudo" CssClass="TrocarEmpresaConteudo">
                <div>
                    <h3 class="titulo_item_modal">
                        Pesquise por CNPJ ou Nome da Empresa</h3>
                    <div>
                        <telerik:RadTextBox runat="server" ID="tbxFiltroBusca" TextMode="SingleLine" ValidationGroup="Filtrar" EmptyMessage="" CssClass="textbox_padrao">
                        </telerik:RadTextBox>
                        <asp:Button ID="btnPesquisar" runat="server" CssClass="botao_padrao filtrar" Text="Pesquisar" ToolTip="Filtrar Empresas" CausesValidation="True" ValidationGroup="Filtrar" OnClick="btnPesquisar_Click" />
                    </div>
                </div>
                <div>
                    <h3 class="titulo_item_modal">
                        Pesquise por CNPJ ou Nome da Empresa</h3>
                    <telerik:RadGrid ID="gvEmpresas" CssClass="gridview_padrao trocar_empresa" OnItemCommand="gvEmpresas_ItemCommand" AllowPaging="True" AlternatingItemStyle-CssClass="alt_row" AllowCustomPaging="true" OnPageIndexChanged="gvEmpresas_PageIndexChanged" runat="server">
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" Position="TopAndBottom" />
                        <MasterTableView DataKeyNames="Idf_Filial,Idf_Usuario_Filial_Perfil">
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Logo" ItemStyle-CssClass="logo_empresa_conteudo">
                                    <ItemTemplate>
                                        <uc1:UCLogoFilial ID="imgLogo" runat="server" Visible='<%# VerificaLogoEmpresa(Eval("Num_CNPJ")) %>' Cnpj='<%# Eval("Num_CNPJ") %>' CssClass="logo_empresa_trocar" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Nome de Empresa" ItemStyle-CssClass="nome_empresa_conteudo">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRazaoSocial" Font-Bold='<%# VerificaFilialLogado(Eval("Idf_Filial")) %>' Text='<%# Eval("Raz_Social")%>' ToolTip='<%# Eval("Num_CNPJ") %>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Selecionar" ItemStyle-CssClass="selecionar_empresa">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btiSelecionar" CommandName="SelecionarEmpresa" runat="server"><i class="fa fa-check-circle"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:HiddenField ID="hfTrocarEmpresa" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeTrocarEmpresa" runat="server" PopupControlID="pnlTrocarEmpresa" TargetControlID="hfTrocarEmpresa">
</AjaxToolkit:ModalPopupExtender>
