<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="RelatorioSalarialMercadoTelaMagica.aspx.cs" Inherits="BNE.Web.RelatorioSalarialMercadoTelaMagica" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <link href='http://fonts.googleapis.com/css?family=Open+Sans+Condensed:300' rel='stylesheet' type='text/css'>
    <Employer:DynamicHtmlLink runat="server" Href="css/local/RelatorioSalarialMercadoTelaMagica.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicScript runat="server" Src="/js/global/geral.js" type="text/javascript" />
    <script type="text/javascript">
        function btnComprarVip_Click() {
            trackEvent('Anuncios', 'Click', 'Banner_Relatorio_Salarial_Mercado');
            return true;
        }
    </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <div class="painel_padrao ajusta">
        <div class="containerImgRelatorioSalarialTelaMagica">
            <div class="containerTextos">
                <div class="containerTextoQuemMeViu">Relatório Salarial</div>
                <div class="containerTextoSejaCliente">
                    <span class="enfatizaTexto">Seja um cliente VIP</span> e saiba se a sua
                    <br />
                    pretensão salarial está de acordo
                    <br />
                    com a praticada pelo mercado.
                </div>
                <div class="containerBotao">
                    <asp:ImageButton runat="server" ID="btnComprarVip_ClickRelSalarial" AlternateText="Comprar" ImageUrl="/img/RelatorioSalarialMercadoTelaMagica/btnVerdeComprar.png"
                        OnClick="btnComprar_Click" OnClientClick="return btnComprarVip_Click()" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
