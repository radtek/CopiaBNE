<%@ Control
    Language="C#"
    AutoEventWireup="false"
    CodeBehind="CorpoMensagem.ascx.cs"
    Inherits="BNE.Web.UserControls.Forms.Mensagens.CorpoMensagem" %>
<asp:Panel
    ID="pnlCorpoMensagem"
    runat="server">
    <div class="titulo_filtro">
        <h2 class="titulo_msg_content">
            <asp:Label
                ID="lblTitulo"
                runat="server"></asp:Label>
        </h2>
    </div>
    <asp:Panel
        ID="pnlMensagem"
        CssClass="mensagem"
        runat="server">
        <asp:UpdatePanel
            ID="upMensagem"
            runat="server"
            UpdateMode="Conditional">
            <ContentTemplate>
                <div class="navegacao">
                    <asp:ImageButton
                        ID="btiVoltarMensagem"
                        runat="server"
                        ImageUrl="~/img/icn_retroceder.png"
                        CssClass="btn_retroceder"
                        ToolTip="Voltar uma Mensagem"
                        OnClick="btnVoltarMensagem_Click" />
                    <asp:ImageButton
                        ID="btiAvancarMensagem"
                        runat="server"
                        ImageUrl="~/img/icn_avancar.png"
                        CssClass="btn_avancar"
                        ToolTip="Avançar uma Mensagem"
                        OnClick="btnAvancarMensagem_Click" />
                </div>
                <div class="titulo_msg">
                    <asp:Label
                        ID="lblAssunto"
                        runat="server"></asp:Label>
                </div>
                <div class="info_adicionais">
                    <ul>
                        <li class="remetente">
                            <asp:Image
                                ID="imgRemetente"
                                runat="server" />
                        </li>
                        <asp:Label
                            ID="lblRemetente"
                            runat="server"></asp:Label>
                        <li class="data_hora">
                            <b>
                                <asp:Label
                                    ID="lblDataHorasEnviada"
                                    runat="server"
                                    Text="Enviada em:"></asp:Label>
                            </b>
                            <asp:Label
                                ID="lblDataHorasEnviadaValor"
                                runat="server"></asp:Label></li>
                        <li class="destinatario">
                            <b>
                                <asp:Label
                                    ID="lblPara"
                                    runat="server"
                                    Text="Para:"></asp:Label></b>
                            <asp:Label
                                ID="lblParaValor"
                                runat="server"></asp:Label>
                        </li>
                        <li class="anexos">
                            <b>
                                <asp:Label
                                    ID="lblAnexo"
                                    runat="server"
                                    Text="Anexo:"></asp:Label>
                            </b><span>
                                <asp:Image
                                    ID="imgAnexo"
                                    runat="server"
                                    AlternateText="Arquivo Anexado"
                                    ImageUrl="~/img/icn_curriculo.png" />
                                <asp:LinkButton
                                    ID="lbAnexo"
                                    runat="server"
                                    OnClick="lbAnexo_Click"></asp:LinkButton>
                            </span>
                        </li>
                    </ul>
                </div>
                <div class="msg">
                    <p>
                        <asp:Label
                            ID="lblCorpoMensagem"
                            runat="server"></asp:Label>
                    </p>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Panel>
