<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConsultaNotas.ascx.cs" Inherits="BNE.Web.UserControls.Forms.SalaAdministrador.ConsultaNotas" %>
<Employer:DynamicScript runat="server" Src="/js/local/UserControls/PlanoFidelidade.js" type="text/javascript" />
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/PlanoFidelidade.css" type="text/css" rel="stylesheet" />

<h2>Consulta de Notas</h2>

<asp:UpdatePanel runat="server" ID="updBuscarNotas"  UpdateMode="Conditional" >
    <ContentTemplate>
        <asp:Panel runat="server" ID="plnBuscaNota"   CssClass="blocodados" DefaultButton="btnFiltrar">
            <h3>
                Consultar
            </h3>
            <div>
                <div class="linha">
                    <asp:Label runat="server" ID="lblIdentificador"  CssClass="label_principal"  Text="Identificador de Pagamento" AssociatedControlID="txtIdentificador">
                    </asp:Label>
                     <telerik:RadTextBox runat="server" ID="txtIdentificador" TextMode="SingleLine" EmptyMessage=""
                        CssClass="textbox_padrao_pesquisa">
                    </telerik:RadTextBox>
                      <Componentes:BalaoSaibaMais ID="bsmIdade" runat="server" ToolTipText="Para consulta de mais de uma nota separe por vírgula. Ex: 10108032446EN942SCJB, 101089888, 10108032446EN942SDDD"
                            Text="Saiba mais" ToolTipTitle="Idade:" CssClassLabel="balao_saiba_mais" ShowOnMouseover="true" />
                </div>
               
                 <asp:Panel ID="Panel6" runat="server" CssClass="painel_botoes">
                    <asp:Button ID="btnFiltrar" runat="server" CssClass="botao_padrao filtrar" Text="Pesquisar"
                        ToolTip="Filtrar" CausesValidation="True" ValidationGroup="Filtrar" OnClick="btnFiltrar_Click" />
                </asp:Panel>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel runat="server" ID="plnNotasConsultadas">
    <ContentTemplate>
          <telerik:RadGrid ID="gvNotas" AllowPaging="True" AllowCustomPaging="true" CssClass="gridview_padrao"
                runat="server" Skin="Office2007" GridLines="None">
                <PagerStyle Mode="NextPrevNumericAndAdvanced" Position="TopAndBottom" />
                <AlternatingItemStyle CssClass="alt_row" />
                <MasterTableView >
                    <Columns>
                        <telerik:GridBoundColumn DataField="NomeSistema" HeaderText="Sistema" ReadOnly="true">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Data_Cadastro" HeaderText="Data Geração" ItemStyle-CssClass="center">
                            <%--<ItemTemplate>
                                <asp:Label ID="lblDtaGeracao" runat="server" Text='<%# Convert.ToDateTime(Eval("Data_Cadastro")).ToString("dd/MM/yyyy") %>'></asp:Label>
                            </ItemTemplate>--%>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Identificador" DataField="Transacao" ItemStyle-CssClass="center">
                           <%-- <ItemTemplate>
                                <asp:Label ID="lblIdentificador" runat="server" Text='<%# Eval("Transacao") %>'></asp:Label>
                            </ItemTemplate>--%>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Nota" DataField="NumeroNotaFiscal" ItemStyle-CssClass="center">
                            <%--<ItemTemplate>
                                <asp:Label ID="lblNota" runat="server" Text='<%# Eval("NumeroNotaFiscal") %>'></asp:Label>
                            </ItemTemplate>--%>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Valor" DataField="ValorNota" ItemStyle-CssClass="center">
                            <%--<ItemTemplate>
                                <asp:Label ID="lblValorTotal" runat="server" Text='<%# Eval("ValorNota") %>'></asp:Label>
                            </ItemTemplate>--%>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Cód. Verificacao" DataField="CodigoVerificacao" ItemStyle-CssClass="center">
                            <%--<ItemTemplate>
                                <asp:Label ID="lblVerificacao" runat="server" Text='<%# Eval("CodigoVerificacao") %>'></asp:Label>
                            </ItemTemplate>--%>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Erro" DataField="Erro" ItemStyle-CssClass="center">
                            <%--<ItemTemplate>
                                <asp:Label ID="lblErro" runat="server" Text='<%# Eval("Erro") %>'></asp:Label>
                            </ItemTemplate>--%>
                        </telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
    </ContentTemplate>
</asp:UpdatePanel>
