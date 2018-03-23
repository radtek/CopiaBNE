<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="ProdutoVIP.aspx.cs" Inherits="BNE.Web.ProdutoVIP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/ProdutoVIP.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicScript runat="server" Src="/js/local/ProdutoVIP.js" type="text/javascript" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <div class="painel_padrao">
        <div class="painel_padrao_topo">
            &nbsp;</div>
        <%--INICIO DOS ORANGE BOXES--%>
        <div id="container_vantagem" class="container_vantagem" runat="server" />
        <div class="seu_vip_inclui">
            <h2>
                <span>O serviço VIP também inclui:</span>
            </h2>
            <br />
            <div id="container_lista">
                <div class="seta_laranja">
                    &nbsp;
                </div>
                <ul id="listaVantagens">
                    <li>
                        <asp:HyperLink runat="server" ID="hlVagas">Vagas</asp:HyperLink></li>
                    <li>
                        <asp:HyperLink runat="server" ID="hlQuemMeViu">Quem Me Viu</asp:HyperLink></li>
                    <li>
                        <asp:HyperLink runat="server" ID="hlEscolherEmpresas">Escolher Empresas</asp:HyperLink></li>
                    <li>
                        <asp:HyperLink runat="server" ID="hlTopoLista">Topo na Lista de Pesquisas</asp:HyperLink></li>
                    <li>
                        <asp:HyperLink runat="server" ID="hlDadosEmpresa">Acesso aos Dados das Empresas</asp:HyperLink></li>
                </ul>
            </div>
        </div>
        <div class="btn_continuar_cinza">
            <asp:ImageButton runat="server" ImageUrl="/img/pacotes_bne/btn_continuar_cinza.png"
                OnClick="btnContinuar_Click" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
