<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true" CodeBehind="RelatorioSalarioBr.aspx.cs" Inherits="BNE.Web.RelatorioSalarioBr" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphHead" runat="server">
    <link href="css/local/RelatorioSalarioBr.css" rel="stylesheet" />
    <script src="js/highcharts/highcharts.js"></script>
    <script src="js/highcharts/exporting.js"></script>
    <script>
        function BuscarGraficos() {
            $('#svgValores').val(escape(grafico.getSVG()));
            $('#svgGenero').val(escape(graficoGenero.getSVG()));
            $('#svgFaixaEtaria').val(escape(graficoFaixaEtaria.getSVG()));
            $('#svgExperiencias').val(escape(graficoExperiencia.getSVG()));
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphConteudo" runat="server">
    
    <asp:HiddenField runat="server" ID="hdnIdFuncao" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="hdnSiglaEstado" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="svgValores" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="svgGenero" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="svgFaixaEtaria" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="svgExperiencias" ClientIDMode="Static" Value="" />
    
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upRSM">
        <ContentTemplate>
            <asp:Literal runat="server" ID="litConteudo"></asp:Literal>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
        <asp:Button ID="btnGerarPdf" Visible="false" runat="server" OnClick="bntGerarPdf_Click" OnClientClick="return BuscarGraficos();" CssClass="botao_padrao"
            CausesValidation="false" Text="Gerar PDF" />
    </asp:Panel>

    <div runat="server" id="DivBannerSalarioBR" visible="false" class="banner_salariobr">
    </div>

    <div runat="server" id="DivFaltaParametros" visible="false">
        Parâmetros incorretos. Verifique.
    </div>
    <div runat="server" id="DivSemPlano" visible="false">
        A geração do relatório não pode ser concluida. Nenhum plano ativo foi encontrado.
    </div>
    <script>
            $('#cphConteudo_DivBannerSalarioBR').on("click", function () {
                window.open('http://www.salariobr.com', '_blank');
            });
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
