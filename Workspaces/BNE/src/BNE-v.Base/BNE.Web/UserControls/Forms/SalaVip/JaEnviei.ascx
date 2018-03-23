<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JaEnviei.ascx.cs" Inherits="BNE.Web.UserControls.Forms.SalaVip.JaEnviei" %>
<div class="painel_padrao_sala_vip">
    <p>
        Confira as vagas que você já se candidatou.</p>
    <asp:UpdatePanel ID="upGvJaEnviei" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <telerik:RadGrid ID="gvJaEnviei" AllowPaging="True" AllowCustomPaging="true" CssClass="gridview_padrao"
                runat="server" Skin="Office2007" GridLines="None" OnPageIndexChanged="gvJaEnviei_PageIndexChanged"
                OnItemCommand="gvJaEnviei_ItemCommand">
                <PagerStyle Mode="NextPrevNumericAndAdvanced" Position="TopAndBottom" />
                <AlternatingItemStyle CssClass="alt_row" />
                <MasterTableView DataKeyNames="Idf_Filial,Idf_Vaga,Img_Empresa_Confidencial_Visible">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Data" ItemStyle-CssClass="data">
                            <ItemTemplate>
                                <asp:Label ID="lblData" runat="server" Text='<%# Eval("Dta_Cadastro","{0:dd/MM/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Hora" ItemStyle-CssClass="hora">
                            <ItemTemplate>
                                <asp:Label ID="lblHora" runat="server" Text='<%# Eval("Dta_Cadastro","{0:HH:mm}") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Função" ItemStyle-CssClass="funcao">
                            <ItemTemplate>
                                <asp:Label ID="lblFuncao" runat="server" Text='<%# Eval("Des_Funcao") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Salário" ItemStyle-CssClass="salario">
                            <ItemTemplate>
                                <asp:Label ID="lblSalario" runat="server" Text='<%# Eval("Vlr_Salario")%>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Nome da Empresa" ItemStyle-CssClass="nome_empresa">
                            <ItemTemplate>
                                <asp:Label ID="lblEmpresa" runat="server" Text='<%# Eval("Raz_Social") %>' Visible='<%# Convert.ToBoolean(Eval("Flg_VIP")) %>'></asp:Label>
                                <asp:Image ID="imgEmpresaBloqueadaAcessoVIP" Visible='<%# !Convert.ToBoolean(Eval("Flg_VIP")) %>'
                                    runat="server" ImageUrl="~/img/img_nome_empresa_borrado.png" />
                                <div runat="server" Visible='<%# Convert.ToBoolean(Eval("Img_Empresa_Confidencial_Visible")) && Convert.ToBoolean(Eval("Flg_VIP")) %>' class="fa fa-lock fa3"></div>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ações" ItemStyle-CssClass="col_action_arq">
                            <ItemTemplate>
                                <asp:LinkButton Style="padding:5px;" class="fa fa-eye espacamento" ID="btiVerVaga" runat="server" CommandName="VerVaga" ToolTip="Ver Vaga" AlternateText="Ver Vaga" />
                                <asp:LinkButton Style="padding:5px;" class="fa fa-building-o espacamento" ID="btiVerEmpresa" runat="server" ToolTip="Ver Empresa" CommandName="VerEmpresa" AlternateText="Ver Empresa" />
                                <asp:LinkButton Style="padding:5px;" class="fa fa-print espacamento" ID="btiImprimir" runat="server" ToolTip="Imprimir" CommandName="Imprimir" AlternateText="Imprimir" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle ShowPagerText="true" Position="TopAndBottom" PagerTextFormat=" {4} Vagas {2} a {3} de {5}" />
                </MasterTableView>
            </telerik:RadGrid>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<%-- Botões --%>
<asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
    <asp:Button ID="btnVoltar" runat="server" CssClass="botao_padrao" Text="Voltar" CausesValidation="false"
        OnClick="btnVoltar_Click" />
</asp:Panel>
<%-- Fim Botões --%>
