<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VisualizadoCV.ascx.cs"
    Inherits="BNE.Web.UserControls.Forms.SalaAdministrador.VisualizadoCV" %>
<%@ Import Namespace="BNE.Web.Code" %>
<asp:UpdatePanel ID="upGvVisualizadoCV" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="painel_padrao_sala_adm">
            <p>
                Encontre o Curriculo desejado utilizando: nome, função ou cidade
            </p>
            <div>
                <telerik:RadTextBox runat="server" ID="tbxFiltroBusca" TextMode="SingleLine" ValidationGroup="Filtrar"
                    EmptyMessage="" CssClass="textbox_padrao">
                </telerik:RadTextBox>
                <asp:Button ID="btnFiltrar" runat="server" CssClass="botao_padrao filtrar" Text="Pesquisar"
                    ToolTip="Filtrar Currículos" CausesValidation="True" ValidationGroup="Filtrar"
                    OnClick="btnFiltrar_Click" />
            </div>
            <telerik:RadGrid ID="gvVisualizadoCV" AllowPaging="True" CssClass="gridview_padrao"
                runat="server" Skin="Office2007" GridLines="None" OnPageIndexChanged="gvVisualizadoCV_PageIndexChanged"
                AllowCustomPaging="True">
                <PagerStyle Mode="NextPrevNumericAndAdvanced" Position="TopAndBottom" />
                <AlternatingItemStyle CssClass="alt_row" />
                <MasterTableView>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Nome" ItemStyle-CssClass="nome_candidato">
                            <ItemTemplate>
                                <asp:Label ID="lblNome" runat="server" Text='<%# Eval("Nme_Pessoa") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Função" ItemStyle-CssClass="funcao">
                            <ItemTemplate>
                                <asp:Label ID="lblFuncao" runat="server" Text='<%# RetornarFuncao(Eval("Des_Funcao").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cidade" ItemStyle-CssClass="cidade">
                            <ItemTemplate>
                                <asp:Label ID="lblNmeVaga" runat="server" Text='<%# UIHelper.FormatarCidade(Eval("Nme_Cidade").ToString(), Eval("Sig_Estado").ToString()) %>' CssClass="nome_vaga"></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Data" ItemStyle-CssClass="dt_visualizacao">
                            <ItemTemplate>
                                <asp:Label ID="lblDataVisualizacao" runat="server" Text='<%# Eval("Dta_Visualizacao") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="IP" ItemStyle-CssClass="ip_usuario">
                            <ItemTemplate>
                                <asp:Label ID="lblIP" runat="server" Text='<%# Eval("Num_IP") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
        <%-- Botões --%>
        <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
            <asp:Button ID="btnVoltar" runat="server" CssClass="botao_padrao" Text="Voltar" CausesValidation="false"
                PostBackUrl="~/SalaAdministradorEmpresas.aspx" />
        </asp:Panel>
        <%-- Fim Botões --%>
    </ContentTemplate>
</asp:UpdatePanel>
