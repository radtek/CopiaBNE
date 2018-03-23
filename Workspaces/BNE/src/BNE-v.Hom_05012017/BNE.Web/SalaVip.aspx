<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="SalaVip.aspx.cs" Inherits="BNE.Web.SalaVip" %>

<%@ Register Src="UserControls/Forms/SalaVip/Dados.ascx" TagName="Dados" TagPrefix="uc1" %>
<%@ Register Src="UserControls/Forms/SalaVip/MenuSalaVip.ascx" TagName="MenuSalaVip"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicScript runat="server" Src="/js/local/Forms/SalaVip.js" type="text/javascript" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/sala_vip.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript">

        $(document).ready(function () {
            if ($("#navBreadcrumbForms").css('display') == 'none') {
                $("#pnlMenuSecaoCandidato").css('position', 'relative').css('top', '90px');
            }
        });

    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <!-- Sala Vip BNE -->
    <div style="text-align: center;">
        <script type='text/javascript'>
            <!--//<![CDATA[
            document.MAX_ct0 = '';
            var m3_u = (location.protocol == 'https:' ? 'https://cas.criteo.com/delivery/ajs.php?' : 'http://cas.criteo.com/delivery/ajs.php?');
            var m3_r = Math.floor(Math.random() * 99999999999);
            document.write("<scr" + "ipt type='text/javascript' src='" + m3_u);
            document.write("zoneid=300530"); document.write("&amp;nodis=1");
            document.write('&amp;cb=' + m3_r);
            if (document.MAX_used != ',') document.write("&amp;exclude=" + document.MAX_used);
            document.write(document.charset ? '&amp;charset=' + document.charset : (document.characterSet ? '&amp;charset=' + document.characterSet : ''));
            document.write("&amp;loc=" + escape(window.location).substring(0, 1600));
            if (document.context) document.write("&context=" + escape(document.context));
            if ((typeof (document.MAX_ct0) != 'undefined') && (document.MAX_ct0.substring(0, 4) == 'http')) {
                document.write("&amp;ct0=" + escape(document.MAX_ct0));
            }
            if (document.mmm_fo) document.write("&amp;mmm_fo=1");

            document.write("'></scr" + "ipt>");
            //]]>-->
        </script>
    </div>
    <div class="painel_padrao_sala_vip">
        <uc1:Dados ID="ucDados" runat="server" />
        <uc2:MenuSalaVip ID="ucTabs" runat="server" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
