<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PagamentosCielo.ascx.cs" Inherits="BNE.Web.UserControls.Forms.SalaAdministrador.PagamentosCielo" %>
<link href="../../../css/local/UserControls/PlanoFidelidade.css" rel="stylesheet" type="text/css" />
<h2>Pagamentos CIELO</h2>
<asp:UpdatePanel ID="upPagamentoCielo" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="idPagamentoCielo" runat="server" CssClass="coluna_direita" style="margin: 10px;">
            <asp:FileUpload ID="fCsvCielo" runat="server" />
            <asp:Button ID="btEnviar" runat="server" Text="Enviar" OnClick="btEnviar_Click" />
        </asp:Panel>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btEnviar" />
    </Triggers>
</asp:UpdatePanel>


<asp:UpdatePanel ID="upPagamentos" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <telerik:RadGrid ID="gvPagamentosCielo" CssClas="gridview_padrao" runat="server"
            Skin="Office2007" GridLines="None">
            <PagerStyle Mode="NextPrevNumericAndAdvanced" Position="TopAndBottom" />
            <AlternatingItemStyle CssClass="alt_row" />
            <MasterTableView DataKeyNames="Des_Codigo_Autorizacao">
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Identificador" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblIdentiicador" runat="server" Text='<%# Eval("Des_Codigo_Autorizacao") %>'></asp:Label>
                        </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Sistema" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblSistema" runat="server" Text='<%# Eval("Sistema") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Plano" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblDescricaoPlano" runat="server" Text='<%# Eval("Descrição_Plano") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Nome/Razão Social" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblNomePlano" runat="server" Text='<%# Eval("Nome_Razão_Social") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="CPF/CNPJ" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblCPF_CNPJ" runat="server" Text='<%# Eval("CPF_CNPJ") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Cartão" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblCartao" runat="server" Text='<%# Eval("Cartão") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Valor" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblValorPlano" runat="server" Text='<%# Eval("Valor_Plano") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Nota Fiscal" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <a href="<%# Eval("Url_Nota_Fiscal") %>" target="_blank">
                                <asp:Label ID="lblNotaFiscal" runat="server" Text='<%# Eval("Nota_Fiscal") %>'></asp:Label>
                            </a>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Stauts" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("StatusNota") %>'></asp:Label>
                        </ItemTemplate>

                    </telerik:GridTemplateColumn>
                    
                    <telerik:GridTemplateColumn HeaderText="Autorizacao">
                        <ItemTemplate>
                             <asp:Label runat="server" Text='<%# Eval("codAut") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                </Columns>
            </MasterTableView>
            <HeaderContextMenu EnableAutoScroll="true"></HeaderContextMenu>
        </telerik:RadGrid>
    </ContentTemplate>
</asp:UpdatePanel>
