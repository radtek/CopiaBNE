<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true" CodeBehind="QuemMeViuTelaMagica.aspx.cs" Inherits="BNE.Web.QuemMeViuTelaMagica" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <link href='http://fonts.googleapis.com/css?family=Open+Sans+Condensed:300' rel='stylesheet' type='text/css'>
    <Employer:DynamicHtmlLink runat="server" Href="css/local/QuemMeViuTelaMagica.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
<div class="painel_padrao ajusta">
    <div class="containerImgQuemMeViuTelaMagica">
        <div class="containerTextos">
            <div class="containerTextoQuemMeViu">QUEM ME VIU?</div>
            <div ID="divQuantidade" runat="server" class="containerTexto3empresas">
        
            <span class="enfatizaTexto"><asp:Literal ID="litQtdeEmpresas" runat="server"></asp:Literal></span>
            <asp:Literal ID="litVisualizaram" runat="server"></asp:Literal> seu currículo!

            </div>
            <div class="containerTextoSejaCliente">
             Seja um Cliente <span class="enfatizaTexto">VIP</span>! 
             Você terá acesso ao relatório
             diário informando o horário e
             o nome das <span class="enfatizaTexto">empresas</span> que
             <span class="enfatizaTexto">visualizaram</span> seu currículo.
            </div>

            <div class="containerBotao">
                <asp:ImageButton runat="server" ID="btnContinuar" AlternateText="Continuar" ImageUrl="/img/QuemMeViuTelaMagica/btnVerdeComprar.png" OnClick="btnContinuar_Click"/>
            </div>
        </div>
    </div>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
