<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EscolherEmpresa.ascx.cs"
    Inherits="BNE.Web.UserControls.Forms.SalaVip.EscolherEmpresa" %>
<%@ Register Src="../../Modais/VerDadosEmpresa.ascx" TagName="VerDadosEmpresa" TagPrefix="uc1" %>
<%@ Register Src="../../Modais/ModalConfirmacao.ascx" TagName="ModalConfirmacao"
    TagPrefix="uc2" %>
<%@ Register Src="../../Modais/IndicarEmpresa.ascx" TagName="IndicarEmpresa" TagPrefix="uc3" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/EscolherEmpresa.css" type="text/css" rel="stylesheet" />
<div class="painel_padrao_sala_vip">
    <p>
        Escolha onde você deseja trabalhar, as empresas serão avisadas pelo BNE do seu interesse.</p>
    <asp:UpdatePanel ID="upGvEscolherEmpresa" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div>
                <telerik:RadTextBox runat="server" ID="tbxFiltroBusca" Columns="72" TextMode="SingleLine"
                    ValidationGroup="Filtrar" EmptyMessage="Você pode pesquisar por nome, ramo de atividade ou cidade"
                    CssClass="textbox_filtro_busca">
                </telerik:RadTextBox>
                <asp:Button ID="btnFiltrar" runat="server" CssClass="botao_padrao filtrar" Text="Filtrar Empresas"
                    ToolTip="Filtrar Empresas" CausesValidation="True" ValidationGroup="Filtrar"
                    OnClick="btnFiltrar_Click" />
            </div>
            <telerik:RadGrid ID="gvEscolherEmpresa" AllowPaging="True" AllowCustomPaging="true"
                CssClass="gridview_padrao" runat="server" Skin="Office2007" GridLines="None"
                OnPageIndexChanged="gvEscolherEmpresa_PageIndexChanged" OnItemCommand="gvEscolherEmpresa_ItemCommand">
                <PagerStyle Mode="NextPrevNumericAndAdvanced" Position="TopAndBottom" />
                <AlternatingItemStyle CssClass="alt_row" />
                <MasterTableView DataKeyNames="Idf_Filial">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Nome da Empresa" ItemStyle-CssClass="nome_empresa">
                            <ItemTemplate>
                                <asp:Label ID="lblNomeEmpresa" runat="server" Text='<%# Eval("Nme_Empresa") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Nº Funcionários" ItemStyle-CssClass="nr_funcionarios">
                            <ItemTemplate>
                                <asp:Label ID="lblNumFuncionario" runat="server" Text='<%# Eval("Num_Funcionario") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ramo de Atividade" ItemStyle-CssClass="ramo_atividade">
                            <ItemTemplate>
                                <asp:Label ID="lblRamoDeAtividade" runat="server" Text='<%#  Eval("Des_Area_BNE")%>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cidade/UF" ItemStyle-CssClass="cidade_uf">
                            <ItemTemplate>
                                <asp:Label ID="lblNmeCidade" runat="server" Text='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Nme_Cidade").ToString()) ? "<i>Não Informado</i>" : Eval("Nme_Cidade") + "/" + Eval("Sig_Estado") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ação" ItemStyle-CssClass="col_action_arq">
                            <ItemTemplate>
                                <asp:LinkButton ID="btiCandidatar" runat="server" 
                                    ToolTip="Candidatar-se" CommandName="Candidatar"
                                    Visible='<%# !Convert.ToBoolean(Eval("Flg_Candidatou")) %>'>
                                    <i class="fa fa-file-text icon-"></i> 
                                    Candidatar
                                </asp:LinkButton>                                
                                <span runat="server" id="spanJaEnviei" visible='<%# Convert.ToBoolean(Eval("Flg_Candidatou")) %>'>
                                    <i class="fa fa-check-circle"></i>
                                    Já Enviei!
                                </span>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle ShowPagerText="true" Position="TopAndBottom" PagerTextFormat=" {4} Empresas {2} a {3} de {5}" />
                </MasterTableView>
            </telerik:RadGrid>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<%-- Botões --%>
<asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
    <asp:UpdatePanel ID="upNaoAcheiEmpresa" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <asp:Button ID="btnNaoAcheiEmpresa" runat="server" CssClass="botao_padrao" Text="Não Achei Empresa"
                CausesValidation="false" OnClick="btnNaoAcheiEmpresa_Click" />
            <asp:Button ID="btnVoltar" runat="server" CssClass="botao_padrao" Text="Voltar" CausesValidation="false"
                PostBackUrl="/SalaVip.aspx" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<%-- Fim Botões --%>
<uc1:VerDadosEmpresa ID="ucVerDadosEmpresa" runat="server" />
<uc2:ModalConfirmacao ID="ucModalConfirmacao" runat="server" />
<uc3:IndicarEmpresa ID="ucIndicarEmpresa" runat="server" />
