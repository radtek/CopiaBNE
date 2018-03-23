<%@ Control Language="C#" AutoEventWireup="False" CodeBehind="ucAtendimentoOnline.ascx.cs"
    Inherits="BNE.Web.UserControls.Modais.ucAtendimentoOnline" %>
<%@ Register TagPrefix="uc" TagName="WebCallBack_Modais" Src="~/UserControls/Modais/ucWebCallBack_Modais.ascx" %>
<Employer:DynamicScript runat="server" Src="/js/local/UserControls/ucAtendimentoOnline.js" type="text/javascript" />
<asp:UpdatePanel ID="upSOS" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>
        <asp:Panel ID="pnlSOS" runat="server" Visible="False">
            <div id="div49939200">
                <a id="aSOS" runat="server" href="#" onclick="return AbrirAtendimentoOnline();">
                    <asp:Literal runat="server" ID="litImagem"></asp:Literal>
                    <asp:Literal runat="server" ID="litScript"></asp:Literal>
                </a>
                <a id="aSOSRodape" runat="server" href="#" onclick="return AbrirAtendimentoOnline();">Atendimento Online
                </a>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="upWebCallBack" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="pnlWebCallBack" runat="server" Visible="False">
            <a id="modalzinha" runat="server" class="btn-atendimentoOnline" data-toggle="modal" data-target="#nomeDaModal">
                <i class="fa fa-phone"></i>
                <small>Fale conosco</small>
            </a>
            <uc:WebCallBack_Modais ID="ucWebCallBack_Modais" runat="server" DeveGravarLogCRM="False" />
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
