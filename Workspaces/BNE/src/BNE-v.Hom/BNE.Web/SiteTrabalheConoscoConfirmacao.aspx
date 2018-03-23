<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master"
    AutoEventWireup="true" CodeBehind="SiteTrabalheConoscoConfirmacao.aspx.cs"
    Inherits="BNE.Web.SiteTrabalheConoscoConfirmacao" %>

<asp:Content ID="conHead" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/SiteTrabalheConoscoPasso04.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicScript runat="server" Src="/js/local/SiteTrabalheConoscoPasso04.js" type="text/javascript" />
</asp:Content>
<asp:Content ID="conConteudo" ContentPlaceHolderID="cphConteudo" runat="server">
    <h1>
        Exclusivo Banco de Currículos</h1>
    <div class="interno_abas">
        <asp:Panel ID="pnlDadosPessoais" runat="server" CssClass="painel_padrao">
            <div class="painel_padrao_topo">
                &nbsp;</div>
            <div class="icone_confirmacao">
                <img alt="" src="/img/img_icone_check_128x128.png" />
            </div>
            <p class="texto_criado_sucesso">
                O seu Exclusivo Banco de Currículos foi criado com sucesso!</p>
            <p class="texto_pode_divulgar">
                Você já pode divulgar o endereço <span class="link_endereco_www">
                    <asp:HyperLink ID="hlkEnderecoWww" runat="server" NavigateUrl="http://www.bne.com.br/"
                        Text="www.bne.com.br/">
                    </asp:HyperLink></span>,
                <br />
                anunciar suas vagas e receber currículos dos interessados.
            </p>
        </asp:Panel>
    </div>
    <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
        <asp:Button CausesValidation="false" CssClass="botao_padrao" ID="btnComeceUtilizarJa"
            runat="server" Text="Começar a Utilizar meu Exclusivo Banco de Currículos"
            onclick="btnComeceUtilizarJa_Click" />
    </asp:Panel>
</asp:Content>
<asp:Content ID="conRodape" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
