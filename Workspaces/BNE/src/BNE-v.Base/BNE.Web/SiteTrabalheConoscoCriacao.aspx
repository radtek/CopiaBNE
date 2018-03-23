<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="SiteTrabalheConoscoCriacao.aspx.cs" Inherits="BNE.Web.SiteTrabalheConoscoCriacao" %>

<asp:Content ID="conHead" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/SiteTrabalheConoscoPasso01.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicScript runat="server" Src="/js/local/SiteTrabalheConoscoPasso01.js" type="text/javascript" />
</asp:Content>
<asp:Content ID="conConteudo" ContentPlaceHolderID="cphConteudo" runat="server">
    <h1>Exclusivo Banco de Currículos</h1>
    <div>
        <br />
        <asp:Label ID="lblDescricaoPasso1" runat="server" Text="Ao criar um Exclusivo Banco de Currículos você adquire um endereço exclusivo e com a cara da sua empresa para que os candidatos interessados em suas vagas se cadastrem. A ferramenta oferece também pesquisa avançada e rastreador de currículos, anúncio de vagas e para seus candidatos cadastro de currículo e pesquisa de vagas.">
        </asp:Label>
    </div>
    <div>
        <br />
    </div>
    <div class="interno_abas">
        <asp:Panel ID="pnlDadosPessoais" runat="server" CssClass="painel_padrao">
            <div class="painel_padrao_topo">
                &nbsp;
            </div>
            <p class="texto_escolha_nesta_tela">
                Escolha nesta tela as principais características do seu site de currículos como:
                endereço na Internet, tema e logo.
            </p>
            <div class="coluna_configuracoes">
                <h2>
                    <img src="img/icn_configurar.png" /><span>Configurações</span></h2>
                <div class="endereco_rhoffice">
                    <div style="float: left; margin: 5px 3px 0 0;">
                        <asp:Label runat="server" Text="Escolha o Endereço:" CssClass="texto_inline" />
                        <span class="texto_www_bne texto_inline">www.bne.com.br/</span>
                    </div>
                    <div style="float: left;">
                        <componente:AlfaNumerico Columns="25" ID="txtEnderecoSite" runat="server" ValidationGroup="SiteTrabalheConosco"
                            Tipo="Letras" CssClass="texto_inline" ClientValidationFunction="ValidarEnderecoSTC"></componente:AlfaNumerico>
                        <div style="margin: 3px  0 0 4px;">
                            <Componentes:BalaoSaibaMais ID="bsmIdade" runat="server" ToolTipText="Utilize a sugestão do sistema ou configure o seu endereço no BNE conforme sua escolha."
                                Text="Saiba mais" ToolTipTitle="Escolha o Endereço:" CssClassLabel="balao_saiba_mais" ShowOnMouseover="True" />
                        </div>
                    </div>
                </div>
                <div class="texto_botoes_tema">
                    <div class="texto_escolha_tema">
                        Escolha o Tema:
                    </div>
                    <div class="botoes_escolha_tema">
                        <asp:Button CssClass="botao_tema_amarelo" ID="btnEscolhaTemaAmarelo" UseSubmitBehavior="false"
                            runat="server" />
                        <asp:Button CssClass="botao_tema_azul" ID="btnEscolhaTemaAzul" UseSubmitBehavior="false"
                            runat="server" />
                        <asp:Button CssClass="botao_tema_cinza" ID="btnEscolhaTemaCinza" UseSubmitBehavior="false"
                            runat="server" />
                        <asp:Button CssClass="botao_tema_laranja" ID="btnEscolhaTemaLaranja" UseSubmitBehavior="false"
                            runat="server" />
                        <asp:Button CssClass="botao_tema_verde" ID="btnEscolhaTemaVerde" UseSubmitBehavior="false"
                            runat="server" />
                        <asp:Button CssClass="botao_tema_vermelho" ID="btnEscolhaTemaVermelho" UseSubmitBehavior="false"
                            runat="server" />
                    </div>
                </div>
                <div class="painel_insira_logo">
                    <asp:Label CssClass="label_coloque_sua_logo" runat="server" Text="Coloque sua Logo:" />
                    <div class="inserir_logo_empresa">
                        <asp:UpdatePanel ID="upFoto" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <Componentes:SelecionarFoto ID="ucFoto" runat="server" InitialSelection="5;5;20;20"
                                    PainelFileUploadCssClass="painel_upload_foto" PainelImagemFotoCssClass="painel_imagem_foto_logo"
                                    SemFotoImagemUrl="/img/btn_coloque_sua_logo.png" ThumbDir="~/ArquivosTemporarios/" BotaoFecharImageUrl="/img/botao_padrao_fechar.gif"
                                    MinAcceptedHeight="100" MinAcceptedWidth="100" ResizeImageHeight="203" ResizeImageWidth="1400"
                                    MaxHeight="1024" MaxWidth="1280" OnError="ucFoto_Error" />
                                <asp:LinkButton runat="server" ID="btlExisteLogoWS" OnClick="btlExisteLogoWS_Click"
                                    Visible="False" Text="Já existe uma logo. Clique para recuperar." ToolTip="Já existe uma foto. Clique para recuperar."
                                    CssClass="link_alterar_foto" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div class="coluna_resultado">
                <h2>Resultado</h2>
                <div class="container_imagem_resultado">
                    <asp:Image ID="imgMiniaturaResultado" runat="server" />
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
            <asp:UpdatePanel ID="upBotoes" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <asp:Button ID="btnSalvarAvancar" runat="server" CssClass="botao_padrao" Text="Salvar e Avançar"
                        ValidationGroup="SiteTrabalheConosco" OnClick="btnSalvarAvancar_Click" />
                    <asp:Button ID="btnVoltar" runat="server" CssClass="botao_padrao" Text="Voltar" CausesValidation="false"
                        OnClick="btnVoltar_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
    <asp:HiddenField ID="hfTemplate" runat="server" Value="0" />
</asp:Content>
<asp:Content ID="conRodape" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
