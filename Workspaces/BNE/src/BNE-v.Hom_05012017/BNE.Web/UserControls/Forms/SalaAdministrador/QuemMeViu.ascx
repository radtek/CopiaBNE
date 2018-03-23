<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuemMeViu.ascx.cs" Inherits="BNE.Web.UserControls.Forms.SalaAdministrador.QuemMeViu" %>
<asp:UpdatePanel ID="upQuemMeViu" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="painel_padrao_sala_adm">
            <p>
                Verifique aqui data, hora e nome das empresas que visualizaram o currículo.</p>
            <asp:UpdatePanel ID="upGvQuemMeViu" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <telerik:RadGrid ID="gvQuemMeViu" AllowPaging="True" AllowCustomPaging="true" CssClass="gridview_padrao"
                        runat="server" Skin="Office2007" GridLines="None" OnPageIndexChanged="gvQuemMeViu_PageIndexChanged">
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
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Cidade/UF" ItemStyle-CssClass="cidade_uf">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNmeCidade" runat="server" Text='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Cidade_Estado").ToString()) ? "<i>Não Informado</i>" : Eval("Cidade_Estado")%>'></asp:Label>
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
