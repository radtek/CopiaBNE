<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="SalaVip.aspx.cs" Inherits="BNE.Web.SalaVip" %>

<%@ Register Src="UserControls/Forms/SalaVip/Dados.ascx" TagName="Dados" TagPrefix="uc1" %>
<%@ Register Src="UserControls/Forms/SalaVip/MenuSalaVip.ascx" TagName="MenuSalaVip"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicScript runat="server" Src="/js/local/Forms/SalaVip.js" type="text/javascript" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/sala_vip.css" type="text/css" rel="stylesheet" />
        <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=0">
    <script type="text/javascript">

        $(document).ready(function () {
            if ($("#navBreadcrumbForms").css('display') == 'none') {
                $("#pnlMenuSecaoCandidato").css('position', 'relative').css('top', '90px');
            }
        });

    </script>

    <script src="http://m2d.m2.ai/m2d.bne.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <!-- Sala Vip BNE -->
    <div style="text-align: center; height:AUTO;">
        <%  if (!Request.Browser.IsMobileDevice) { %>
            <div id="BNE_CandidateRoom_728x90_Desktop"></div>
        <% } else { %>
            <div id="BNE_CandidateRoom_300x250_Mobile"></div>
        <% } %>
    </div>
    <div class="painel_padrao_sala_vip">
        <uc1:Dados ID="ucDados" runat="server" />
        <uc2:MenuSalaVip ID="ucTabs" runat="server" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
