<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Dados.ascx.cs" Inherits="BNE.Web.UserControls.Forms.SalaSelecionador.Dados" %>
<%@ Register Src="../../UCLogoFilial.ascx" TagName="UCLogoFilial" TagPrefix="uc1" %>
<asp:Panel runat="server" ID="pnlDados">
    <h2 class="nome_destaque">
        <span class="nome_usuario_ss">Olá
            <asp:Label ID="lblNomeEmpresaValor" runat="server" /></span> <span class="logo_empresa">
                <uc1:UCLogoFilial ID="UCLogoFilial1" runat="server" />
            </span>
    </h2>
</asp:Panel>
