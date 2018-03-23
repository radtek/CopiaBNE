<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="CIAProduto.aspx.cs" Inherits="BNE.Web.CIAProduto" %>

<%@ Register Src="UserControls/Modais/ucWebCallBack.ascx" TagName="WebCallBack" TagPrefix="uc" %>
<asp:Content ID="ContentExperimentos" ContentPlaceHolderID="cphExperimentos" runat="server">
    <!-- Google Analytics Content Experiment code -->
    <!-- 1. Load the Content Experiments JavaScript Client -->
    <script src="//www.google-analytics.com/cx/api.js?experiment=XvF-ZHQjSaegh3G5tvwhBQ"></script>
    <script>
        var css_variations = [
            'exibirAbaixo',
            'exibirTopo'
        ]

        // 2. Choose the Variation for the User
        var variation = cxApi.chooseVariation();

        window.onload = function () {
            // 3. Evaluate the result and update the image
            priceTable = document.getElementById('painelVenda');
            priceTable.classList.add(css_variations[variation]);
        }
    </script>
</asp:Content>    
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/CIAProdutoNovo.css" type="text/css" rel="stylesheet" />

    <script>
        $(document).ready(function () {
            $('.painel_filtro').hide(); //Busca de Vagas e CVs
            $('.painel_icones').hide(); //Teste das Cores, SistMars, Média Salarial, etc
            $('.barra_rodape').hide();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">


<!-- Tela CIA - CSS --> 
    <style>
        #cia-preplano {width:880px;margin:auto;font-family:Arial, Helvetica, sans-serif;height:350px;background-image:url('img/ciaProduto.jpg'); color:#333333; line-height:24px;}
        a.cia{cursor:hand; background-color: #8acb00; padding:15px 50px 15px 50px; border:none; color:#ffffff; text-transform:uppercase; border-radius:12px;font-size:18px; text-decoration:none;}
        a.cia:hover{background-color: #7e9a37;cursor:hand; }
        .cia-1-col {width:880px; text-align:center;clear:both;}
        .cia-3-col {width:293px; float:left;margin:auto;margin-top:285px; text-align:center;font-weight:200; padding-bottom:32px;font-size:16px}
        .cia-3-col strong{font-weight:600}
        .cia-icon {height:60px; text-align:center;}
        .conteudo .interna { margin-top: 0px !important; }
    </style>

    <div style="font-size:30px; text-align:center; padding:5px; margin-bottom:5px">
        Aqui seu recrutamento é completo!
    </div>
<div style="margin:0; padding:0;">


    <!-- START Tela CIA - HTML -->
    <div id="cia-preplano">
        <div class="cia-3-col">
            <div class="cia-icon"><img src="img/ciaProduto-icon-01.png" /></div>
            <div>Especializado em currículos <strong>Operacionais</strong> e de <strong>Apoio</strong></div>
        </div>
        <div class="cia-3-col">
            <div class="cia-icon"><img src="img/ciaProduto-icon-02.png" /></div>
            <div><strong>13 milhões</strong> de currículos,<br /> <strong>+15 mil</strong> novos e<br /> atualizados <strong>diariamente</strong></div>
        </div>
        <div class="cia-3-col">
            <div class="cia-icon"><img src="img/ciaProduto-icon-03.png" /></div>
            <div>Comunicação <strong>100% integrada</strong><br /> ao e-mail e celular dos candidatos</div>
        </div>
        <div class="cia-1-col"><a href="/Escolha-de-Plano-CIA" class="cia" onclick="trackEvent('Funil CIA', 'Continuar Tela de Vendas', 'label: Conheca os planos'); return true;">Continuar</a></div>
    </div>
<!-- END Tela CIA - HTML-->

    </div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
    <asp:UpdatePanel ID="upWebCallBack" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlWebCallBack" runat="server" Visible="true">
                <uc:WebCallBack ID="ucWebCallBack" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

