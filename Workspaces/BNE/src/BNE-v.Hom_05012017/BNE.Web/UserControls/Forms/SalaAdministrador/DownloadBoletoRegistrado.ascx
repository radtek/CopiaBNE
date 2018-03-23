<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DownloadBoletoRegistrado.ascx.cs" Inherits="BNE.Web.UserControls.Forms.SalaAdministrador.DownloadBoletoRegidtrado" %>
<h2>Download Boletos Registrados</h2>
<asp:Panel ID="idDownloadBoletoRegistrado" runat="server" CssClass="coluna_direita">
    <h3>Pesquisar Boletos</h3>
    <%--<p> Enviar o arquivo CNR com os boletos pagos. </p>--%>
    <asp:UpdatePanel ID="upDownloadBoletoRegistrado" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <telerik:RadComboBox ID="RadComboBoxPaymentMethod" runat="server" Width="256px" Style="float: left"
                Label="Banco:" AutoPostBack="true"
                EnableTextSelection="true" EnableLoadOnDemand="True" MarkFirstMatch="True" EmptyMessage="Selecione ..."
                AutoCompleteSeparator=";" Height="16px">
            </telerik:RadComboBox>
            <asp:Button ID="btnGerarArquivo" runat="server" OnClick="gerarArquivo_Click" Style="margin-left: 10px; float: left" Text="Gerar Arquivo(s)" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnGerarArquivo" />
        </Triggers>
    </asp:UpdatePanel>
    <br />
</asp:Panel>
