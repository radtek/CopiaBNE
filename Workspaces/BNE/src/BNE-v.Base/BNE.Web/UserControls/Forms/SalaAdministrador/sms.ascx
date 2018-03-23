<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="sms.ascx.cs" Inherits="BNE.Web.UserControls.Forms.SalaAdministrador.sms" %>
<div class="painel_configuracao_conteudo">
    <asp:UpdatePanel ID="upPnlSMS" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="titulo_filtro">
                <h2 class="titulo_carta_content">
                    <asp:Label ID="lblTitulo" Text="SMS" runat="server"></asp:Label>
                </h2>
                <telerik:RadComboBox runat="server" ID="rcbSMS" EmptyMessage="Selecione o SMS desejado"
                    AllowCustomText="false" CssClass="checkbox_large" Width="200" AutoPostBack="True"
                    OnSelectedIndexChanged="rcbSMS_SelectedIndexChanged">
                </telerik:RadComboBox>
            </div>
            <asp:TextBox ID="txtSMS" CssClass="textbox_SMS" runat="server" ValidationGroup="SalvarSMS" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="contagem_caracteres">
        <asp:UpdatePanel ID="upContagemCaracter" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label ID="lblTotalCaracteres" CssClass="total_caractares" runat="server" Text="140 Caracteres"
                    AssociatedControlID="txtSMS"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <%-- Botão Salvar --%>
    <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
        <asp:Button ID="btnSalvar" runat="server" CssClass="botao_padrao painelinterno" Text="Salvar"
            CausesValidation="true" ValidationGroup="SalvarSMS" OnClick="btnSalvar_Click" />
    </asp:Panel>
</div>
