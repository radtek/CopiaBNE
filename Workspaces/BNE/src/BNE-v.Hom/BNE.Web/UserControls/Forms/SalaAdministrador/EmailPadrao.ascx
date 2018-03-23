<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmailPadrao.ascx.cs"
    Inherits="BNE.Web.UserControls.Forms.SalaAdministrador.EmailPadrao" %>
<div class="painel_configuracao_conteudo">
    <div class="titulo_filtro">
        <h2 class="titulo_carta_content">
            <asp:Label ID="lblTitulo" Text="Mensagens Enviadas" runat="server"></asp:Label>
        </h2>
        <telerik:RadComboBox runat="server" ID="rcbEmail" EmptyMessage="Selecione o email desejado"
            AllowCustomText="false" CssClass="checkbox_large" Width="200" OnSelectedIndexChanged="rcbEmail_SelectedIndexChanged"
            AutoPostBack="True" />
    </div>
    <asp:UpdatePanel ID="upPnlEmail" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <telerik:RadEditor CssClass="radeditor_boas_vindas_candidato" Height="270px" ID="reEmail"
                runat="server" SkinID="RadEditorControlesBasicos" Width="618px">
            </telerik:RadEditor>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%-- Botão Salvar --%>
    <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
        <asp:Button ID="btnSalvar" runat="server" CssClass="botao_padrao painelinterno" Text="Salvar"
            CausesValidation="false" OnClick="btnSalvar_Click" />
    </asp:Panel>
</div>
