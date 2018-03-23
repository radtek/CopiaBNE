<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="CIAProdutoNovo.aspx.cs" Inherits="BNE.Web.CIAProduto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <link rel="Stylesheet" type="text/css" href="/css/local/CIAProdutoNovo.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <div class="painel_padrao">
        <div class="containerLabelSlogan">
           ACESSO A MAIS DE <span class="enfatizaTexto">5.6 MILHÕES</span> DE CURRÍCULOS
        </div>
        
        <div class="imgIpad"></div>
       
        <div class="containerMetricas">
                <span class="labelServicosEmpresa">
                Serviços para empresas:
                </span>
                <br /><br />
                <span class="textoLabelLaranja">
                <%--<div class="ImgChecked_1"></div>--%>
                PUBLICAÇÃO DE VAGAS ILIMITADAS
                </span>
                
                <div class="texto1Topico">
                Para todos os candidatos com perfil, conectados 
                em nossas redes sociais e parceiros.
                </div>
                <br />
                <div class="texto1Topico"><span class="textoLabelLaranja">
                <%--<div class="ImgChecked_2"></div>--%>
                BUSCA DE CURRÍCULOS<br />
                </span>
                Pesquisa simples ou mais de 30 filtros 
                para encontrar o seu candidato ideal.
                </div>
                <br />
                <div class="texto1Topico"><span class="textoLabelLaranja">
                <%--<div class="ImgChecked_3"></div>--%>
                COMUNICAÇÃO INTEGRADA<br />
                </span>
                Ferramentas especializadas para agilizar o seu 
                processo de regulamento por SMS, e-mail e redes sociais.
                </div>
                <br />                
                <div class="texto1Topico ajusteDiagramacao"><span class="textoLabelLaranja">
                <%--<div class="ImgChecked_3"></div>--%>
                CURRÍCULOS NO SEU PERFIL<br />
                </span>
                Lista de candidatos interessados nas vagas anúnciadas.
                Sistema inteligente de comparação de candidatos.</div>
                </div>
       
        <div class="btn_verde">
            <asp:HyperLink ID="hlContinuar" runat="server" Text="Conheça os planos" CssClass="label_verde_continuar"
                NavigateUrl="/CIAPlanosNovo.aspx"></asp:HyperLink>
        </div>
        <%--FIM Button Cinza: Continuar--%>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
