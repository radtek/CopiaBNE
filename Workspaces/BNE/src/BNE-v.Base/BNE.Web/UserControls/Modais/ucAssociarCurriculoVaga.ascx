<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ucAssociarCurriculoVaga.ascx.cs"
    Inherits="BNE.Web.UserControls.Modais.ucAssociarCurriculoVaga" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/AssociarCurriculoVaga.css" type="text/css" rel="stylesheet" />
<asp:Panel ID="pnlAVC" CssClass="modal_conteudo empresa" Style="display: none;" runat="server">
    <h2 class="titulo_modal">
        <span>Associar à uma Vaga</span>
    </h2>
    <asp:UpdatePanel ID="upBtiFechar" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:LinkButton CssClass="botao_fechar_modal" ID="btiFechar"
                runat="server" CausesValidation="false" OnClick="btiFechar_Click"><i class="fa fa-times-circle"></i> Fechar</asp:LinkButton>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="pnlConviteEntrevista" runat="server" CssClass="painel_padrao">
        <div class="linha linha_funcao_filtrar_vagas">
            <asp:Label CssClass="label_funcao_filtrar_vagas" ID="lblFuncao" runat="server" Text="Função"
                AssociatedControlID="ccFiltrarVagasFuncao"></asp:Label>
            <div class="container_campo">
                <asp:UpdatePanel ID="upStatusFuncao" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                    <ContentTemplate>
                        <Employer:ComboCheckbox runat="server" ID="ccFiltrarVagasFuncao" EmptyMessage="Filtrar vagas por função"
                            AllowCustomText="false" CssClass="checkbox_large" DropDownWidth="240" Width="240">
                        </Employer:ComboCheckbox>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:Button ID="btnFiltrar" runat="server" CssClass="botao_padrao filtrar" Text="Filtrar Vagas"
                OnClick="btnFiltrarVaga_Click" ToolTip="Filtrar Vagas" CausesValidation="false" />
        </div>
        <asp:UpdatePanel ID="upGvVagas" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <telerik:RadGrid ID="gvVagas" AllowPaging="True" AllowCustomPaging="true" CssClass="gridview_padrao associarvaga"
                    runat="server" Skin="Office2007" GridLines="None" OnItemCommand="gvVagas_ItemCommand"
                    OnPageIndexChanged="gvVagas_PageIndexChanged">
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" Position="TopAndBottom" />
                    <AlternatingItemStyle CssClass="alt_row" />
                    <ClientSettings AllowExpandCollapse="false">
                    </ClientSettings>
                    <MasterTableView DataKeyNames="Idf_Vaga">
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Código">
                                <ItemTemplate>
                                    <asp:Label ID="lblIdVaga" runat="server" Text='<%# Eval("Cod_Vaga") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Vagas" ItemStyle-CssClass="descricao_vaga2">
                                <ItemTemplate>
                                    <asp:Label ID="lblDescricaoVaga" runat="server" Text='<%# Eval("Des_Funcao") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Cidade" ItemStyle-CssClass="cidade">
                                <ItemTemplate>
                                    <asp:Label ID="lblNmeVaga" runat="server" Text='<%# Eval("Nme_Cidade") + "/" + Eval("Sig_Estado") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="CV's Recebidos" ItemStyle-CssClass="cv_recebida centro">
                                <ItemTemplate>
                                    <asp:Label ID="lblCvsRecebidos" runat="server" Text='<%# Eval("Num_Cvs_Recebidos") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Associar" ItemStyle-CssClass="col_action">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btiAssociar" runat="server"
                                        ToolTip="Associar à vaga" AlternateText="Associar à  Vaga" CommandName="Associar"
                                        Visible='<%# Convert.ToBoolean(Eval("Ja_Candidatou")).Equals(false) %>' CausesValidation="false" CssClass="fa fa-check-circle"></asp:LinkButton>
                                    <asp:Literal runat="server" Visible='<%# Convert.ToBoolean(Eval("Ja_Candidatou")).Equals(true) %>'>Já associado</asp:Literal>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle ShowPagerText="true" Position="TopAndBottom" PagerTextFormat=" {4} Vagas {2} a {3} de {5}" />
                        <NoRecordsTemplate>
                            <div class="mensagem_nodata">
                                Nenhum item para mostrar.
                            </div>
                        </NoRecordsTemplate>
                    </MasterTableView>
                </telerik:RadGrid>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnFiltrar" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Panel>
<asp:HiddenField ID="hf" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeAVC" TargetControlID="hf" PopupControlID="pnlAVC"
    runat="server">
</AjaxToolkit:ModalPopupExtender>
