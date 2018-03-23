<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="RelatorioSalarialMercado.aspx.cs" Inherits="BNE.Web.RelatorioSalarialMercado" %>

<%@ Register Src="UserControls/Modais/ModalVendaRSM.ascx" TagName="ModalVendaRSM"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upRSM">
        <ContentTemplate>
            <asp:Literal runat="server" ID="litConteudo"></asp:Literal>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
        <asp:Button ID="btnVoltar" runat="server" CssClass="botao_padrao" Text="Voltar" CausesValidation="false"
            PostBackUrl="/SalaVip.aspx" />
        <%--<asp:Button ID="btnGerarPdf" runat="server" OnClick="bntGerarPdf_Click" CssClass="botao_padrao"
            CausesValidation="false" Text="Gerar PDF" />--%>
        <asp:Button ID="btnImprimir" runat="server" CssClass="botao_padrao"
            CausesValidation="false" OnClientClick="window.print(); return false;" PostBackUrl="javascript:;"
            Text="Imprimir" />
    </asp:Panel>
    <uc1:ModalVendaRSM ID="ModalVendaRSM" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
