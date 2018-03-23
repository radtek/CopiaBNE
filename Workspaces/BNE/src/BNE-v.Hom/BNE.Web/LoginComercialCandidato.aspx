<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="LoginComercialCandidato.aspx.cs" Inherits="BNE.Web.LoginComercialCandidato" %>

<%@ Register Src="UserControls/Login.ascx" TagName="Login" TagPrefix="uc1" %>

<asp:Content ID="conHead" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/LoginComercialCandidato.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicScript runat="server" Src="/js/local/Forms/LoginComercialCandidato.js" type="text/javascript" />
</asp:Content>

<asp:Content ID="conConteudo" ContentPlaceHolderID="cphConteudo" runat="server">
    <h1>
        Vantagens e Serviços para Você</h1>
    <div class="painel_servico_destaque">
        <div class="imagem_servico_destaque">
            <asp:Literal runat="server" ID="litIconeGrandeServicoDestaque"></asp:Literal>
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
    <h3>
        Outros serviços</h3>
    <asp:Panel CssClass="painel_servicos_baixo" ID="pnlServicosBaixo" runat="server">
        <%-- Serviço --%>
        <asp:Panel CssClass="painel_servico" ID="pnlServicoSalaVIP" runat="server">
            <div class="icone_servico">
                <a href="javascript:;">
                    <i class="fa fa-key fa-5x"></i>
                </a>
            </div>
            <div class="texto_nome_servico">
                Sala VIP</div>
        </asp:Panel>
        <%-- FIM: Serviço --%>
        <%-- Serviço --%>
        <asp:Panel CssClass="painel_servico" ID="pnlServicoQuemMeViu" runat="server">
            <div class="icone_servico">
                <a href="javascript:;">
  <i class="fa fa-eye fa-5x"></i>
                                  </a>
            </div>
            <div class="texto_nome_servico">
                Quem me Viu?</div>
        </asp:Panel>
        <%-- FIM: Serviço --%>
        <%-- Serviço --%>
        <asp:Panel CssClass="painel_servico" ID="pnlServicoVIP" runat="server">
            <div class="icone_servico">
                <a href="javascript:;">
                    <strong  class="fa-4x">VIP</strong>

                </a>
            </div>
            <div class="texto_nome_servico">
                Atendimento Diferenciado</div>
        </asp:Panel>
        <%-- FIM: Serviço --%>
        <%-- Serviço --%>
        <asp:Panel CssClass="painel_servico" ID="pnlServicoAtualizarCurriculo" runat="server">
            <div class="icone_servico">
                <a href="javascript:;">
                    <span class="fa-stack fa-lg fa-2x">
  <i class="fa fa-file-o fa-stack-2x"></i>
  <i class="fa fa-refresh fa-stack-1x"></i>
</span>
                </a>
            </div>
            <div class="texto_nome_servico">
                Atualizar Currículo</div>
        </asp:Panel>
        <%-- FIM: Serviço --%>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
