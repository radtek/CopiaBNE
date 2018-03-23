<%@ Control
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="MensagensEnviadas.ascx.cs"
    Inherits="BNE.Web.UserControls.Forms.Mensagens.MensagensEnviadas" %>
<asp:Panel
    ID="pnlMensagensEnviadas"
    runat="server">
    <asp:UpdatePanel
        ID="upMensagensEnviadas"
        runat="server"
        UpdateMode="Conditional">
        <ContentTemplate>
            <div class="titulo_filtro">
                <h2 class="titulo_msg_content">
                    <asp:Label
                        ID="lblTitulo"
                        Text="Mensagens Enviadas"
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
                        OnClick="btiPesquisar_Click"
                        Style="width: 16px; height: 16px" />
                </div>
            </div>
            <telerik:RadGrid
                ID="gvMensagensEnviadas"
                AlternatingItemStyle-CssClass="alt_row"
                runat="server"
                AllowPaging="True"
                AllowCustomPaging="true"
                CssClass="gridview_padrao mensagens"
                Skin="Office2007"
                GridLines="None"
                OnItemCommand="gvMensagensEnviadas_ItemCommand"
                OnPageIndexChanged="gvMensagensEnviadas_PageIndexChanged">
                <ClientSettings
                    EnablePostBackOnRowClick="False">
                </ClientSettings>
                <PagerStyle
                    Mode="NextPrevNumericAndAdvanced"
                    PagerTextFormat=" {4} Mensagens {2} a {3} de {5}"
                    Position="TopAndBottom" />
                <AlternatingItemStyle
                    CssClass="alt_row" />
                <MasterTableView
                    DataKeyNames="Idf_Mensagem_CS">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Mensagem" ItemStyle-CssClass="dadoscol1 esquerda" HeaderStyle-CssClass="rgHeader titulo esquerda">
                            <ItemTemplate>
                                <asp:Panel Style="float: left" class="rgIconeMsg" Visible='<%# !string.IsNullOrWhiteSpace(Eval("Des_Email_Assunto").ToString()) %>' runat="server">
                                    <i runat="server" class="fa fa-envelope" title="E-mail"></i>
                                </asp:Panel>
                                <asp:Panel Style="float: left" class="rgIconeMsg" Visible='<%# string.IsNullOrWhiteSpace(Eval("Des_Email_Assunto").ToString()) %>' runat="server">
                                    <i runat="server" class="fa fa-comments" title="SMS"></i>
                                </asp:Panel>
                              
                                <div style="float: left" class="rgLblTituloConteudo rgLblTituloConteudoEnviadas ">
                                     <div>
                                            <asp:Label ID="lblTituloDe" runat="server" Text="Para:" CssClass="rgLblTitulo">
                                            </asp:Label>
                                            <asp:Label ID="lblDestinatario" runat="server" Text='<%# (Eval("Destinatario").ToString().IndexOf(" ") > -1 ? Eval("Destinatario").ToString().Substring(0, Eval("Destinatario").ToString().IndexOf(" ")) : Eval("Destinatario").ToString()) + " - Código CV " + Eval("Idf_Curriculo")%>'></asp:Label>
                                        </div>
                                    <asp:LinkButton runat="server" CommandName="RowClick" Style="text-decoration:none; cursor:pointer;" >
                                       
                                        <div>
                                            <asp:Label ID="lblTituloAssunto" runat="server" Text="Assunto:" CssClass="rgLblTitulo" Visible='<%# !string.IsNullOrWhiteSpace(Eval("Des_Email_Assunto").ToString()) %>'>
                                            </asp:Label>
                                            <asp:Label ID="lblAssunto" runat="server" Text='<%# Eval("Des_Email_Assunto") %>' CssClass="rgLblTituloConteudoAssunto">
                                            </asp:Label>
                                        </div>
                                    </asp:LinkButton>
                                    <%--Só mostrar a mensagem se for SMS--%>
                                    <asp:Panel Visible='<%# string.IsNullOrWhiteSpace(Eval("Des_Email_Assunto").ToString()) %>' runat="server">

                                        <asp:Label ID="lblTituloMensagem" runat="server" Text="Mensagem:" CssClass="rgLblTitulo">
                                        </asp:Label>
                                        <asp:Label ID="lblMensagem" runat="server" Text='<%# Eval("Des_Mensagem") %>' CssClass="rgLblTituloConteudoAssunto">
                                        </asp:Label>

                                    </asp:Panel>
                                </div>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Data/Hora" ItemStyle-CssClass="dadoscol3 direita rgLblDataHora" HeaderStyle-CssClass="rgHeader  titulo direita">
                            <ItemTemplate>
                                <asp:Label ID="lblDataHora" runat="server" Text='<%# Eval("Dta_Cadastro","{0:dd/MM/yyyy HH:mm}")%>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NestedViewTemplate>
                        <asp:Panel ID="ContainerMensagem" runat="server" Visible="false">
                            <div class="msg">
                                <p>
                                    <asp:Label ID="lblCorpoMensagem" runat="server" Text='<%# Eval("Des_Mensagem")%>'></asp:Label>
                                </p>
                            </div>
                        </asp:Panel>
                    </NestedViewTemplate>
                    <NoRecordsTemplate>
                        <div class="mensagem_nodata">Nenhum item para mostrar.</div>
                    </NoRecordsTemplate>
                </MasterTableView>
            </telerik:RadGrid>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
