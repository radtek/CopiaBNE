<%@ Control
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="MensagensRecebidas.ascx.cs"
    Inherits="BNE.Web.UserControls.Forms.Mensagens.MensagensRecebidas" %>
<asp:Panel
    ID="pnlMensagensRecebidas"
    runat="server">
    <asp:UpdatePanel
        ID="upMensagensRecebidas"
        runat="server"
        UpdateMode="Conditional">
        <ContentTemplate>
            <div class="titulo_filtro">
                <h2 class="titulo_msg_content">
                    <asp:Label
                        ID="lblTitulo"
                        Text="Mensagens Recebidas"
                        runat="server"></asp:Label>
                </h2>
                <div class="filtro_pesquisa">
                    <asp:TextBox
                        ID="txtPesquisar"
                        runat="server"></asp:TextBox>
                    <AjaxToolkit:TextBoxWatermarkExtender
                        ID="txtWePesquisar"
                        runat="server"
                        TargetControlID="txtPesquisar"
                        WatermarkText="Pesquisar">
                    </AjaxToolkit:TextBoxWatermarkExtender>
                    <asp:ImageButton
                        ID="btiPesquisar"
                        runat="server"
                        ImageUrl="img"
                        src="../../../img/icn_filtro_pesquisa_msg.png"
                        CssClass="btn_pesquisa"
                        OnClick="btiPesquisar_Click" />
                </div>
            </div>
            <telerik:RadGrid
                ID="gvMensagensRecebidas"
                AlternatingItemStyle-CssClass="alt_row"
                runat="server"
                AllowPaging="True"
                AllowCustomPaging="true"
                CssClass="gridview_padrao mensagens"
                Skin="Office2007"
                GridLines="None"
                OnItemCommand="gvMensagensRecebidas_ItemCommand"
                OnPageIndexChanged="gvMensagensRecebidas_PageIndexChanged">
                <ClientSettings
                    EnablePostBackOnRowClick="true"
                    AllowExpandCollapse="True">
                </ClientSettings>
                <PagerStyle
                    Mode="NextPrevNumericAndAdvanced"
                    PagerTextFormat=" {4} Mensagens {2} a {3} de {5}"
                    Position="TopAndBottom" />
                <AlternatingItemStyle />
                <MasterTableView
                    DataKeyNames="Idf_Mensagem_CS">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Mensagem" ItemStyle-CssClass="dadoscol1 esquerda" HeaderStyle-CssClass="rgHeader titulo esquerda">
                            <ItemTemplate>
                                <div style="float: left" class="rgBarraMsg">
                                    <asp:Label ID="lblBarraNovaMsg" runat="server" CssClass="rgBarraMsgNova" Visible='<%# Eval("Flg_Lido").Equals(true) ? false : true %>'>
                                    </asp:Label>
                                    <asp:Label ID="lblBarraMsgLida" runat="server" CssClass="rgBarraMsgLida" Visible='<%# Eval("Flg_Lido").Equals(true) ? true : false %>'>
                                    </asp:Label>
                                </div>
                                <div style="float: left" class="rgIconeMsg">
                                   <i ID="imgIconeMensagem" runat="server" class="fa fa-envelope"></i> </div>
                                <div style="float: left" class="rgLblTituloConteudo">
                                    <div>
                                        <asp:Label ID="lblTituloDe" runat="server" Text="De:" CssClass="rgLblTitulo">
                                        </asp:Label>
                                        <asp:Label ID="lblRemetente" runat="server" Text='<%# Eval("Remetente")%>'
                                            Font-Bold='<%# Eval("Flg_Lido").Equals(true) ? false : true %>'>
                                        </asp:Label>
                                    </div>
                                    <div style="float: left">
                                        <asp:Label ID="lblTituloAssunto" runat="server" Text="Assunto:" CssClass="rgLblTitulo">
                                        </asp:Label>
                                        <asp:Label ID="lblAssunto" runat="server" Text='<%# Eval("Des_Email_Assunto")%>' CssClass="rgLblTituloConteudoAssunto"
                                            Font-Bold='<%# Eval("Flg_Lido").Equals(true) ? false : true %>'>
                                        </asp:Label>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Data/Hora" ItemStyle-CssClass="dadoscol1 direita rgLblDataHora" HeaderStyle-CssClass="rgHeader  titulo direita">
                            <ItemTemplate>
                                <div>
                                    <asp:Label ID="lblDataHora" runat="server" Text='<%# Eval("Dta_Cadastro","{0:dd/MM/yyyy | HH:mm}")%>'
                                        Font-Bold='<%# Eval("Flg_Lido").Equals(true) ? false : true %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NoRecordsTemplate>
                        <div class="mensagem_nodata">Nenhum item para mostrar.</div>
                    </NoRecordsTemplate>
                </MasterTableView>
            </telerik:RadGrid>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
