<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master"
    AutoEventWireup="true" CodeBehind="SiteTrabalheConoscoPasso01.aspx.cs"
    Inherits="BNE.Web.SiteTrabalheConoscoPasso01" %>

<%@ Register Src="UserControls/ucFotos.ascx" TagName="ucFotos" TagPrefix="uc1" %>
<asp:Content ID="conHead" ContentPlaceHolderID="cphHead" runat="server">
    <link href="/css/local/SiteTrabalheConoscoPasso01.css?1" rel="Stylesheet"
        type="text/css" />
    <script src="/js/local/SiteTrabalheConoscoPasso01.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="conConteudo" ContentPlaceHolderID="cphConteudo" runat="server">
    <h1>
        Site Trabalhe Conosco</h1>
    <div>
        <br />
        <asp:Label ID="lblDescricaoPasso1" runat="server" Text="Ao criar um Site Trabalhe Conosco você adquiri um endereço exclusivo e com a cara da sua empresa para que os candidatos interessados em suas vagas se cadastrem. A ferramenta oferece também pesquisa avançada e rastreador de currículos, anúncio de vagas e para seus candidatos cadastro de currículo e pesquisa de vagas."></asp:Label>
    </div>
    <div>
        <br />
    </div>
    <%-- Tabs --%>
    <asp:UpdatePanel ID="upnAbas" runat="server">
        <ContentTemplate>
            <div class="linha_abas">
                <div class="abas_nova">
                    <span class="aba_esquerda_selecionada">
                        <asp:Label ID="lblPasso1" runat="server" Text="Passo 1" CssClass="texto_abas_selecionada"></asp:Label>
                    </span><span class="aba_direita_selecionada"></span>
                </div>
                <div class="abas_nova">
                    <span class="aba_esquerda_nao_selecionada">
                        <asp:LinkButton ID="lbtPasso2" runat="server" ValidationGroup="SiteTrabalheConosco"
                            Text="Passo 2" CssClass="texto_abas" OnClick="lbtPasso2_Click"></asp:LinkButton>
                    </span><span class="aba_direita_nao_selecionada"></span>
                </div>
                <div class="abas_nova">
                    <span class="aba_esquerda_nao_selecionada">
                        <asp:LinkButton ID="lbtPasso3" runat="server" ValidationGroup="SiteTrabalheConosco"
                            Text="Passo 3" CssClass="texto_abas" OnClick="lbtPasso3_Click"></asp:LinkButton>
                    </span><span class="aba_direita_nao_selecionada"></span>
                </div>
                <%--<div class="abas_nova">
                    <span class="aba_esquerda_nao_selecionada">
                        <asp:LinkButton
                            ID="lbtPasso4"
                            runat="server"
                            ValidationGroup="SiteTrabalheConosco"
                            Text="Passo 4"
                            CssClass="texto_abas"
                            OnClick="lbtPasso4_Click"></asp:LinkButton>
                    </span><span
                        class="aba_direita_nao_selecionada">
                    </span>
                </div>--%>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%-- FIM: Tabs --%>
    <div class="interno_abas">
        <asp:Panel ID="pnlDadosPessoais" runat="server" CssClass="painel_padrao">
            <div class="painel_padrao_topo">
                &nbsp;</div>
            <p class="texto_escolha_nesta_tela">
                Escolha nesta tela as principais características do seu site de currículos
                como: endereço na Internet, tema e logo.</p>
            <div class="coluna_configuracoes">
                <h2>
                    <img src="img/icn_configurar.png" /><span>Configurações</span></h2>
                <div class="endereco_rhoffice">
                    <div style="float: left; margin: 5px 3px 0 0;">
                        <asp:Label runat="server" Text="Escolha o Endereço:" CssClass="texto_inline" />
                        <span class="texto_www_bne texto_inline">www.bne.com.br/</span></div>
                    <div style="float: left;">
                        <componente:AlfaNumerico Columns="25" ID="txtEnderecoSite" runat="server"
                            ValidationGroup="SiteTrabalheConosco" Tipo="Letras" CssClass="texto_inline"
                            ClientValidationFunction="ValidarEnderecoSTC"></componente:AlfaNumerico>
                        <div style="margin: 3px  0 0 4px;">
                            <Employer:BalaoSaibaMais ID="bsmIdade" runat="server" ToolTipText="Utilize a sugestão do sistema ou configure o seu endereço no BNE conforme sua escolha."
                                Text="Saiba mais" ToolTipTitle="Escolha o Endereço:" CssClassLabel="balao_saiba_mais" />
                        </div>
                    </div>
                </div>
                <div class="texto_botoes_tema">
                    <div class="texto_escolha_tema">
                        Escolha o Tema:</div>
                    <div class="botoes_escolha_tema">
                        <asp:Button CssClass="botao_tema_amarelo" ID="btnEscolhaTemaAmarelo"
                            UseSubmitBehavior="false" runat="server" />
                        <asp:Button CssClass="botao_tema_azul" ID="btnEscolhaTemaAzul" UseSubmitBehavior="false"
                            runat="server" />
                        <asp:Button CssClass="botao_tema_cinza" ID="btnEscolhaTemaCinza" UseSubmitBehavior="false"
                            runat="server" />
                        <asp:Button CssClass="botao_tema_laranja" ID="btnEscolhaTemaLaranja"
                            UseSubmitBehavior="false" runat="server" />
                        <asp:Button CssClass="botao_tema_verde" ID="btnEscolhaTemaVerde" UseSubmitBehavior="false"
                            runat="server" />
                        <asp:Button CssClass="botao_tema_vermelho" ID="btnEscolhaTemaVermelho"
                            UseSubmitBehavior="false" runat="server" />
                    </div>
                </div>
                <div class="painel_insira_logo">
                    <asp:Label CssClass="label_coloque_sua_logo" runat="server" Text="Coloque sua Logo:" />
                    <div class="inserir_logo_empresa">
                        <uc1:ucFotos ID="ucFoto" runat="server" MinAcceptedHeight="20" MinAcceptedWidth="20"
                            Height="133" Width="178" MaxWidth="1024" MaxHeight="1280" InitialSelection="5;5;40;40"
                            AspectRatio="" ImageUrl="/img/btn_coloque_sua_logo.png" DefaultImageUrl="/img/btn_coloque_sua_logo.png"
                            TipoImagem="LogoEmpresas" />
                    </div>
                </div>
            </div>
            <div class="coluna_resultado">
                <h2>
                    Resultado</h2>
                <div class="container_imagem_resultado">
                    <asp:Image ID="imgMiniaturaResultado" runat="server" />
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
            <asp:UpdatePanel ID="upBotoes" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <asp:Button ID="btnSalvarAvancar" runat="server" CssClass="botao_padrao"
                        Text="Salvar e Avançar" ValidationGroup="SiteTrabalheConosco"
                        OnClick="btnSalvarAvancar_Click" />
                    <asp:Button ID="btnVoltar" runat="server" CssClass="botao_padrao"
                        Text="Voltar" CausesValidation="false" OnClick="btnVoltar_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
    <asp:HiddenField ID="hfTemplate" runat="server" Value="0" />
</asp:Content>
<asp:Content ID="conRodape" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
