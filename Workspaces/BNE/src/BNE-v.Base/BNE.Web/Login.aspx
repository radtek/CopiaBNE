<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BNE.Web.Login" %>
<%@ Register Src="~/UserControls/Modais/ucModalLogin.ascx" TagPrefix="uc1" TagName="ucModalLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="//ajax.googleapis.com/ajax/libs/jquery/2.1.3/jquery.min.js" type="text/javascript"></script>
    <Employer:DynamicScript runat="server" Src="/js/local/Forms/Login.js" type="text/javascript" />
    <Employer:DynamicScript runat="server" Src="/js/global/geral.js" type="text/javascript" />

    <link href='//fonts.googleapis.com/css?family=Open+Sans:700' rel='stylesheet' type='text/css' />
    <link href="//maxcdn.bootstrapcdn.com/font-awesome/4.2.0/css/font-awesome.min.css" rel="stylesheet">
    <Employer:DynamicHtmlLink runat="server" Href="/css/global/geral.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/global/topo.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/global/conteudo.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/global/componentes.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/global/custom.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="smGlobal" runat="server" AsyncPostBackTimeout="900" EnablePartialRendering="True"
            EnableScriptLocalization="True" EnableScriptGlobalization="True" EnablePageMethods="True">
        </asp:ScriptManager>
        <%--Integração Facebook--%>
        <div id="fb-root">
        </div>
        <script type="text/javascript">
            window.fbAsyncInit = function () {
                FB.init({
                    appId: 450210698402153,
                    status: true,
                    cookie: true,
                    xfbml: true
                });
            };

            (function (d, s, id) {
                var js, fjs = d.getElementsByTagName(s)[0];
                if (d.getElementById(id)) return;
                js = d.createElement(s); js.id = id;
                js.src = "//connect.facebook.net/pt_BR/all.js";
                fjs.parentNode.insertBefore(js, fjs);
            }(document, 'script', 'facebook-jssdk'));
        </script>
        <%--Fim Integração Facebook--%>

        <div>
            <uc1:ucModalLogin runat="server" ID="ucModalLogin" />
        </div>
        <asp:Panel ID="pnlAviso" runat="server" class="painel_avisos" Style="display: none">
            <asp:UpdatePanel ID="updAviso" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div style="position: relative">
                        <span id="icoavisos_img_container" class="icoavisos_img_container">
                            <i class="fa fa-exclamation-triangle fa-3x"></i>
                        </span>
                        <asp:Label ID="lblAviso" runat="server" Text=""></asp:Label>
                        <div class="botao_fechar_aviso" onclick="OcultarAviso();"> 
                            <div style="margin-top:22px;">Fechar <i class="fa fa-times-circle"></i></div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </form>
</body>
</html>
