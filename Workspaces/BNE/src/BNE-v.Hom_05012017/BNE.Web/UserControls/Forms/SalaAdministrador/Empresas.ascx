<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Empresas.ascx.cs" Inherits="BNE.Web.UserControls.Forms.SalaAdministrador.Empresas" %>
<%@ Register Src="../../Modais/BloquearCandidato.ascx" TagName="BloquearCandidato" TagPrefix="uc4" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/VerDadosEmpresa.css" type="text/css" rel="stylesheet" />
<asp:UpdatePanel ID="upGvEmpresas" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="painel_padrao_sala_adm">
            <p>
                Encontre a empresa desejada utilizando: CNPJ, nome fantasia, razão social, telefone, endereço ou e-mail
            </p>
            <asp:Panel runat="server" DefaultButton="btnFiltrar">
                <telerik:RadTextBox runat="server" ID="tbxFiltroBusca" TextMode="SingleLine" ValidationGroup="Filtrar"
                    EmptyMessage="" CssClass="textbox_padrao">
                </telerik:RadTextBox>
                <asp:Button ID="btnFiltrar" runat="server" CssClass="botao_padrao filtrar" Text="Pesquisar" ToolTip="Filtrar Empresas"
                    CausesValidation="True" ValidationGroup="Filtrar" OnClick="btnFiltrar_Click" />
                <script>$(".botao_padrao.filtrar").removeAttr("disabled")</script>
            </asp:Panel>
            <telerik:RadGrid ID="gvEmpresas" AllowPaging="True" AllowCustomPaging="True" CssClass="gridview_padrao" runat="server" Skin="Office2007"
                GridLines="None" OnPageIndexChanged="gvEmpresas_PageIndexChanged" OnItemCommand="gvEmpresas_ItemCommand">
                <PagerStyle Mode="NextPrevNumericAndAdvanced" Position="TopAndBottom" />
                <AlternatingItemStyle CssClass="alt_row" />
                <MasterTableView DataKeyNames="Idf_Filial">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Nome da Empresa" ItemStyle-CssClass="nome_empresa">
                            <ItemTemplate>
                                <asp:Label ID="lblNomeEmpresa" runat="server" Text='<%# Eval("Raz_Social") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CNPJ" ItemStyle-CssClass="">
                            <ItemTemplate>
                                <asp:Label ID="lblCNPJ" runat="server" Text='<%# Eval("Num_CNPJ") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cidade/UF" ItemStyle-CssClass="">
                            <ItemTemplate>
                                <asp:Label ID="lblCidadeUF" runat="server" Text='<%# Eval("CidadeUF") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Data Última Pesquisa" ItemStyle-CssClass="">
                            <ItemTemplate>
                                <asp:Label ID="lblUltimaPesquisa" runat="server" Text='<%# Eval("Dta_Ultima_pesquisa") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Data Validade Plano" ItemStyle-CssClass="">
                            <ItemTemplate>
                                <asp:Label ID="lblDataValidadePlano" runat="server" Text='<%# Eval("Data_Validade_Plano") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ações" ItemStyle-CssClass="col_action_arq">
                            <ItemTemplate>
                                <asp:ImageButton ID="bt1Editar" runat="server" ImageUrl="../../../img/icn_editar_lapis.png" ToolTip="Editar"
                                    CommandName="EditarEmpresa" AlternateText="Editar Empresa" />
                                <asp:ImageButton ID="bt1Checado" runat="server" ImageUrl="../../../img/icn_checado2.png" ToolTip="Checar"
                                    AlternateText="Checar" Visible="false" />
                                <asp:ImageButton ID="bt1Visualizar" runat="server" ImageUrl="../../../img/icn_binoculo.png" CommandName="VisualisaCurriculos"
                                    ToolTip="Currículos Visualizados" AlternateText="Visualizar" />
                                <asp:ImageButton ID="btiEditarMonitor" runat="server" ImageUrl="../../../img/icn_editar_monitor.png"
                                    CommandName="SiteTrabalheConosco" ToolTip="Exclusivo Banco de Currículos" AlternateText="Exclusivo Banco de Currículos" />
                                <asp:ImageButton ID="btiAvaliacao" runat="server" ImageUrl="../../../img/icn_infeliz.png" ToolTip="Bronquinha"
                                    Visible='<%# Convert.ToInt32(Eval("Idf_Situacao_Filial"))!=5 %>' AlternateText="Bronquinha" CausesValidation="false"
                                    CommandName="BronquinhaBloquear" CommandArgument='<%# Eval("Idf_Filial")%>' />
                                <asp:ImageButton ID="btiAvaliacaoBloqueado" runat="server" ImageUrl="../../../img/icn_infeliz.png" ToolTip="Bronquinha"
                                    Visible='<%# Convert.ToInt32(Eval("Idf_Situacao_Filial"))==5 %>' AlternateText="Bronquinha" CausesValidation="false"
                                    CommandName="BronquinhaBloqueado" CommandArgument='<%# Eval("Idf_Filial")%>' />
                                <asp:ImageButton ID="btiVagasAnunciadas" runat="server" ImageUrl="../../../img/icn_divulgar.png" ToolTip="Vagas Anunciadas"
                                    AlternateText="Vagas Anunciadas" CommandName="VagasAnunciadas" />
                                <asp:ImageButton ID="btiCadastrarUsuario" runat="server" ImageUrl="../../../img/icone_msn.png" ToolTip="Gerenciar Usuários"
                                    AlternateText="Gerenciar Usuários" CommandName="Usuario" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
        <%-- Botões --%>
        <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
            <asp:Button ID="btnVoltar" runat="server" CssClass="botao_padrao" Text="Voltar" CausesValidation="false"
                OnClick="btnVoltar_Click" />
        </asp:Panel>
        <%-- Fim Botões --%>
    </ContentTemplate>
</asp:UpdatePanel>
<uc4:BloquearCandidato ID="ucBloquearCandidato" runat="server" />
