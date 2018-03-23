<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="LoginComercialEmpresa.aspx.cs" Inherits="BNE.Web.LoginComercialEmpresa" %>

<%@ Register Src="UserControls/Login.ascx" TagName="Login" TagPrefix="uc1" %>
<asp:Content ID="conHead" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/LoginComercialEmpresa.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicScript runat="server" Src="/js/local/Forms/LoginComercialEmpresa.js" type="text/javascript" />
</asp:Content>
<asp:Content ID="conConteudo" ContentPlaceHolderID="cphConteudo" runat="server">
    <h1>Vantagens e Serviços</h1>
    <div class="painel_servico_destaque">
        <div class="imagem_servico_destaque">
            <asp:Literal ID="litIconeGrandeServicoDestaque" runat="server"></asp:Literal>
        </div>
        <div class="nome_descricao_servico_destaque">
            <h2>
                <asp:Literal ID="litNomeServicoDestaque" runat="server"></asp:Literal></h2>
            <asp:Literal ID="litDescricaoServicoDestaque" runat="server"></asp:Literal>
        </div>
    </div>
    <div class="painel_uclogin">
        <uc1:Login ID="ucLogin" runat="server" LoginComFacebook="False" />
    </div>
    <h3>Outros serviços</h3>
    <asp:Panel CssClass="painel_servicos_baixo" ID="pnlServicosBaixo" runat="server">
        <%-- Serviço --%>
        <asp:Panel CssClass="painel_servico" ID="pnlServicoPesquisarCVs" runat="server">
            <div class="icone_servico">
                <asp:HyperLink ID="hlPesquisaCurriculo" runat="server">
                    <p class="fa-stack fa-2x">
  <i class="fa fa-file-o fa-stack-2x"></i>
  <i class="fa fa-search fa-stack-1x"></i>
</p>
                  
                </asp:HyperLink>
            </div>
            <div class="texto_nome_servico">
                Pesquisar CVs
            </div>
        </asp:Panel>
        <%-- FIM: Serviço --%>
        <%-- Serviço --%>
        <asp:Panel CssClass="painel_servico" ID="pnlServicoAnunciarVagas" runat="server">
            <div class="icone_servico">
                <asp:HyperLink ID="hlAnunciarVaga" runat="server">
                    <i class="fa fa-bullhorn fa-5x"></i>
                </asp:HyperLink>
            </div>
            <div class="texto_nome_servico">
                Anunciar Vagas
            </div>
        </asp:Panel>
        <%-- FIM: Serviço --%>
        <%-- Serviço --%>
        <asp:Panel CssClass="painel_servico" ID="pnlServicoSalaSelecionadora" runat="server">
            <div class="icone_servico">
                <asp:HyperLink ID="hlSalaSelecionadora" runat="server">
                    <i class="fa fa-key fa-5x"></i>
                </asp:HyperLink>
            </div>
            <div class="texto_nome_servico">
                Sala Selecionadora
            </div>
        </asp:Panel>
        <%-- FIM: Serviço --%>
        <%-- Serviço --%>
        <asp:Panel CssClass="painel_servico" ID="pnlServicoCompreCV" runat="server">
            <div class="icone_servico">
                <asp:HyperLink ID="hlCompreCV" runat="server">
                    <span class="fa-stack fa-2x">
  <i class="fa fa-file-o fa-stack-2x"></i>
  <i class="fa fa-usd fa-stack-1x"></i>
</span>
                    

                </asp:HyperLink>
            </div>
            <div class="texto_nome_servico">
                Compre CV
            </div>
        </asp:Panel>
        <%-- FIM: Serviço --%>
        <%-- Serviço --%>
        <asp:Panel CssClass="painel_servico" ID="pnlServicoSiteTrabalheConosco" runat="server">
            <div class="icone_servico">
                <asp:HyperLink ID="hlSTC" runat="server">
                    <i class="fa fa-database fa-5x"></i>
                </asp:HyperLink>
            </div>
            <div class="texto_nome_servico">
                Exclusivo Banco de Currículos
            </div>
        </asp:Panel>
        <%-- FIM: Serviço --%>
        <%-- Serviço --%>
        <asp:Panel CssClass="painel_servico" ID="pnlServicoAtualizarEmpresa" runat="server">
            <div class="icone_servico">
                <asp:HyperLink ID="hlAtualizarEmpresa" runat="server">
                    <i class="fa fa-building-o fa-4x"></i>
                </asp:HyperLink>
            </div>
            <div class="texto_nome_servico">
                Atualizar Empresa
            </div>
        </asp:Panel>
        <%-- FIM: Serviço --%>
        <%-- Serviço --%>
        <asp:Panel CssClass="painel_servico" ID="pnlServicoCVsRecebidos" runat="server">
            <div class="icone_servico">
                <asp:HyperLink ID="hlCurriculosRecebidos" runat="server">
                    <i class="fa fa-files-o fa-5x"></i>
                </asp:HyperLink>
            </div>
            <div class="texto_nome_servico">
                CVs Recebidos
            </div>
        </asp:Panel>
        <%-- FIM: Serviço --%>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
