<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucModalLogin.ascx.cs" Inherits="BNE.Web.UserControls.Modais.ucModalLogin" %>
<%@ Register Src="~/UserControls/Login.ascx" TagName="Login" TagPrefix="uc1" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/ModalLogin.css" type="text/css" rel="stylesheet" />
<asp:Panel id="pnlLoginModal" runat="server" class="modal_conteudo pnlLoginModal candidato reduzida" style="display: none">

    <h2 class="titulo_modal">
        <span>Entrar</span>
    </h2>
    <asp:UpdatePanel ID="upBtiFechar" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:LinkButton runat="server" ID="btiFechar" CausesValidation="false" OnClick="btiFechar_Click"> <i class="fa fa-times-circle"></i> <span>Fechar</span> </asp:LinkButton>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--<asp:Panel CssClass="coluna_esquerda" ID="pnlColunaEsquerda" runat="server">
        <asp:Panel CssClass="painel_imagem" ID="pnlEsquerdaImagem" runat="server">
            <img alt="" src="/img/img_modal_login.png" />
        </asp:Panel>
    </asp:Panel>--%>
    <%--<asp:Panel CssClass="coluna_direita ajuste" ID="pnlColunaDireita" runat="server">--%>
        <uc1:Login ID="ucLogin" runat="server" />
    <%--</asp:Panel>--%>
</asp:Panel>
<asp:HiddenField ID="hfVariavel" runat="server" />
<AjaxToolkit:ModalPopupExtender  ID="mpeLogin" runat="server" PopupControlID="pnlLoginModal" TargetControlID="hfVariavel">
</AjaxToolkit:ModalPopupExtender>
<%--BehaviorID="mpeLoginBehavior"--%>
