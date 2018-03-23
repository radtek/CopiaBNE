<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuemMeViu.ascx.cs" Inherits="BNE.Web.UserControls.Forms.SalaVip.QuemMeViu" %>
<asp:UpdatePanel ID="upQuemMeViu" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="painel_padrao_sala_vip">
            <p>
                Verifique aqui data, hora e nome das empresas que visualizaram o seu currículo.</p>
            <asp:UpdatePanel ID="upGvQuemMeViu" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <telerik:RadGrid ID="gvQuemMeViu" AllowPaging="True" AllowCustomPaging="true" CssClass="gridview_padrao"
                        runat="server" Skin="Office2007" GridLines="None" OnPageIndexChanged="gvQuemMeViu_PageIndexChanged"
                        OnItemCommand="gvQuemMeViu_ItemCommand">
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" Position="TopAndBottom" />
                        <AlternatingItemStyle CssClass="alt_row" />
                        <MasterTableView DataKeyNames="Idf_Filial,Idf_Curriculo_Quem_Me_Viu">
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Data" ItemStyle-CssClass="data">
                                    <ItemTemplate>
                                        <asp:Label ID="lblData" runat="server" Text='<%# Eval("Dta_Quem_Me_Viu","{0:dd/MM/yyyy}") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Hora" ItemStyle-CssClass="hora">
                                    <ItemTemplate>
                                        <asp:Label ID="lblHora" runat="server" Text='<%# Eval("Dta_Quem_Me_Viu","{0:HH:mm}") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Nome da Empresa" ItemStyle-CssClass="nome_empresa">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmpresa" runat="server" Text='<%# Eval("Raz_Social") %>'></asp:Label>
                                        <asp:Image ID="imgVip" runat="server" Visible='<%# Convert.ToBoolean(Eval("Img_VIP_Visible")) %>' ImageUrl="~/img/icone_vip.png" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Cidade/UF" ItemStyle-CssClass="cidade_uf">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNmeCidade" runat="server" Text='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Cidade_Estado").ToString()) ? "<i>Não Informado</i>" : Eval("Cidade_Estado")%>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Ações" ItemStyle-CssClass="col_action_arq">
                                    <ItemTemplate>
                                        <asp:LinkButton CssClass="fa fa-binoculars space-left" ID="btiVerDadosEmpresa" runat="server" CommandName="VerDadosEmpresa" ToolTip="Ver Dados da Empresa">
                                            Dados da empresa
                                        </asp:LinkButton>
                                        <asp:LinkButton CssClass="fa fa-bullhorn space-left" ID="btiVerVagasEmpresa" runat="server" CommandName="VerVagasEmpresa" ToolTip="Vagas da Empresa">
                                            Vagas
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
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
    </ContentTemplate>
</asp:UpdatePanel>
