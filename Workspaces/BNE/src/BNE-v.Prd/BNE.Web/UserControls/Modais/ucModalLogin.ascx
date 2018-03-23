<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucModalLogin.ascx.cs" Inherits="BNE.Web.UserControls.Modais.ucModalLogin" %>
<%@ Register Src="~/UserControls/Login.ascx" TagName="Login" TagPrefix="uc1" %>
<Employer:DynamicHtmlLink runat="server" Href="~/css/local/UserControls/Modais/ModalLogin.css" type="text/css" rel="stylesheet" />
<asp:Panel ID="pnlLoginModal" runat="server" class="modal_conteudo pnlLoginModal candidato" Style="display: none">
    <div class="titulo_modal">
        <div class="titulo_modal_logo"><img src="/img/logo-bne-white-transp.png"/></div>
        <asp:UpdatePanel ID="upBtiFechar" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:LinkButton runat="server" ID="btiFechar" CausesValidation="false" OnClick="btiFechar_Click" CssClass="fechar">
                    <span aria-hidden="true">×</span>
                </asp:LinkButton>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <uc1:Login ID="ucLogin" runat="server" />
</asp:Panel>
<asp:HiddenField ID="hfVariavel" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeLogin" runat="server" PopupControlID="pnlLoginModal" TargetControlID="hfVariavel">
</AjaxToolkit:ModalPopupExtender>
