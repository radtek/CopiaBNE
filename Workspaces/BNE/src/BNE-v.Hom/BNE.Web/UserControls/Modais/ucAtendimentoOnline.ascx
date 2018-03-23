<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAtendimentoOnline.ascx.cs"
    Inherits="BNE.Web.UserControls.Modais.ucAtendimentoOnline" %>
<Employer:DynamicScript runat="server" Src="/js/local/UserControls/ucAtendimentoOnline.js" type="text/javascript" />
<asp:UpdatePanel ID="upSOS" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>
        
                
            <a id="aSOS" runat="server" href="#" onclick="return AbrirAtendimentoOnline();">
                <asp:Literal runat="server" ID="litImagem"></asp:Literal>
                <asp:Literal runat="server" ID="litScript"></asp:Literal>
            </a>
            <a id="aSOSRodape" runat="server" href="#" onclick="return AbrirAtendimentoOnline();">Atendimento Online
            </a>
        
    </ContentTemplate>
</asp:UpdatePanel>
