<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Dados.ascx.cs" Inherits="BNE.Web.UserControls.Forms.SalaSelecionador.Dados" %>
<%@ Register Src="../../UCLogoFilial.ascx" TagName="UCLogoFilial" TagPrefix="uc1" %>
<asp:Panel runat="server" ID="pnlDados">
    <div class="nome_destaque">
        <div>
            <div class="nome_usuario_ss"><asp:Label ID="lblNomeEmpresaValor" runat="server" /></div>
            <asp:HyperLink runat="server" CssClass="atualiza_usuario_ss" ID="hlAtualizarEmpresa"><i class="fa fa-refresh"></i> Atualizar Empresa</asp:HyperLink>
        </div> 
        <div class="logo_empresa"><uc1:UCLogoFilial ID="UCLogoFilial1" runat="server" /></div>
    </div>
    
</asp:Panel>
