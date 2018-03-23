<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="SalaSelecionador.aspx.cs" Inherits="BNE.Web.SalaSelecionador" %>

<%@ Register Src="UserControls/Forms/SalaSelecionador/Dados.ascx" TagName="Dados"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">

    <link href="css/local/Bootstrap/bootstrap.css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/Sala_Selecionador.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicScript runat="server" Src="/js/local/SalaSelecionador.js" type="text/javascript" />
    <script src="js/bootstrap/bootstrap.js"></script>

    <script>
        var abrirModal = '';
        $(document).ready(function () {
            if (abrirModal == true)
                AbrirModalVideoSMSSelecionadora();
        });

        function AbrirBanner(value) {
            value = false; //optado por esconder temporáriamente
            abrirModal = value;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <div class="painel_padrao_sala_selecionador">
        <asp:Panel ID="pnlDados" runat="server">
            <uc1:Dados ID="ucDados" runat="server" />
        </asp:Panel>
        <asp:UpdatePanel ID="upMenuNavegacao" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlNavegacao" CssClass="rtsUL" runat="server">
                    <%--Menu Tela Selecionadora --%>
                    <div class="menu_ss">
                        <%-- Coluna 01 do Menu da Tela Selecionadora --%>
                        <div class="col01">
                            <asp:LinkButton ID="BotaoVagas" runat="server" OnClick="btlMinhasVagas_Click" CausesValidation="false"
                                ToolTip="Vagas">
                                <div class="btn_col">
                                    <div class="fundo_btn_ss">
                                        <div class="sombra_btn_ss">
                                            <i class="fa fa-newspaper-o fa-5x pull-left"></i>
                                            <div class="titulo_texto_btn menor">
                                                <span class="tit_btn_ss">Minhas Vagas</span> <span class="texto_btn_ss">
                                                    <ul class="item_btn">
                                                        <li>Administre ou divulgue vagas aqui
                                                        </li>
                                                        <li id="liVagasAnunciadas" runat="server" style="display: none"><b>
                                                            <asp:Label ID="lblVagasAnunciadasQuantidade" runat="server"></asp:Label></b>
                                                            <asp:Label ID="lblVagasAnunciadasMensagem" runat="server"></asp:Label></li>
                                                        <li id="liCVsRecebidos" runat="server" style="display: none"><b>
                                                            <asp:Label ID="lblCurriculosRecebidosQuantidade" runat="server"></asp:Label></b>
                                                            <asp:Label ID="lblCurriculosRecebidosMensagem" runat="server"></asp:Label>
                                                        </li>
                                                    </ul>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btlRastreadorCurriculos" runat="server" OnClick="btlRastreadorCurriculos_Click"
                                CausesValidation="false" ToolTip="Alerta de Currículos">
                                <div class="btn_col">
                                    <div class="fundo_btn_ss">
                                        <div class="sombra_btn_ss">
                                            <p class="block-ico">
                            <span class="fa-stack fa-lg fa-2x">
                                <i class="fa fa-comment-o fa-flip-horizontal fa-stack-2x"></i>
                                <i class="fa fa-exclamation fa-stack-1x"></i>
                            </span>
                        </p>
                                            <div class="titulo_texto_btn menor">
                                                <span class="tit_btn_ss">
                                                    Alerta de Currículos</span>
                                                <span class="texto_btn_ss">
                                                    <ul class="item_btn">
                                                        <li>
                                                            Receba candidatos no perfil em seu e-mail</li>
                                                    </ul>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:LinkButton>
                        </div>
                        <%-- Fim da Coluna 01 do Menu da Tela Selecionadora --%>
                        <%-- Coluna 02 do Menu da Tela Selecionadora --%>
                        <div class="col02">
                            <asp:LinkButton ID="BotaoMeuPlano" runat="server" OnClick="btlCompraPlanos_Click"
                                CausesValidation="false" ToolTip="Meu Plano">
                                <div class="btn_col">
                                    <div class="fundo_btn_ss">
                                        <div class="sombra_btn_ss">
                                            <asp:Literal runat="server" ID="litSemPlano" Visible="False">
                                                <i class="fa fa-times fa-5x pull-left red"></i>
                                            </asp:Literal>
                                            <asp:Literal runat="server" ID="litComPlano" Visible="False">
                                                <i class="fa fa-check fa-5x pull-left"></i>
                                            </asp:Literal>
                                            <div class="titulo_texto_btn menor">
                                                <span class="tit_btn_ss">Meu Plano</span> <span class="texto_btn_ss">
                                                    <ul class="item_btn">
                                                        <li>
                                                            <asp:Label ID="lblPlanoAcessoValidade" runat="server"></asp:Label>
                                                        </li>
                                                        <li><b>
                                                            <asp:Label ID="lblQuantidadeSMSUtilizado" runat="server"></asp:Label></b>
                                                            <asp:Label ID="lblSMSUtilizadoMensagem" runat="server"></asp:Label></li>
                                                    </ul>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btlMensagens" runat="server" OnClick="btlMensagensEnviadas_Click"
                                CausesValidation="false" ToolTip="Mensagens">
                                <div class="btn_col bottom">
                                    <div class="fundo_btn_ss">
                                        <div class="sombra_btn_ss">
                                            <i class="fa fa-envelope-o fa-5x pull-left"></i>
                                            <div class="titulo_texto_btn">
                                                <span class="tit_btn_ss">Mensagens</span> <span class="texto_btn_ss">
                                                    <ul class="item_btn">
                                                        <%-- Esse <li> não pode aparecer sem ter dados, o IE7 esta interpretando como se ele tivesse conteúdo --%>
                                                        <li>
                                                            <asp:Label ID="lblMensagemPossuiMensagem" runat="server"></asp:Label></li>
                                                        <li><b>
                                                            <asp:Label ID="lblQuantidadeMensagens" runat="server"></asp:Label></b>
                                                            <asp:Label ID="lblTextoMensagem" runat="server"></asp:Label>
                                                        </li>
                                                    </ul>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:LinkButton>
                        </div>
                        <%-- Fim da Coluna 01 do Menu da Tela Selecionadora --%>
                        <%-- Coluna 03 do Menu da Tela Selecionadora --%>
                        <div class="col03">

                            <asp:LinkButton ID="BotaoRecrutamento" runat="server" OnClick="btlCampanhaRecrutamento_Click"
                                CausesValidation="false" ToolTip="Campanha">
                                <div class="btn_col">
                                    <div class="fundo_btn_ss fundo_btn_ss-adjust">
                                        <div class="sombra_btn_ss">
                                            <p style="width: 80px; padding: 10px 0 0 10px; float: left">
                                                <i class="fa fa-bullhorn fa-5x pull-left"></i>
                                            </p>
                                            <div class="titulo_texto_btn menor">
                                                <div class="titulo_texto_btn menor">
                                                    <span class="tit_btn_ss">Campanha de Recrutamento</span> <span class="texto_btn_ss">
                                                        <ul class="item_btn">
                                                            <li>
                                                                <asp:Literal ID="litSaldoCampanha" runat="server" Text="0"></asp:Literal></li>
                                                        </ul>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btlPesquisaAvancada" runat="server" CausesValidation="false"
                                OnClick="btlPesquisaAvancada_OnClick" ToolTip="Pesquisa Avançada">
                                <div class="btn_col bottom">
                                    <div class="fundo_btn_ss">
                                        <div class="sombra_btn_ss">
                                            <p style=" width:80px; padding:10px 0 0 10px; float:left">
                                                <span class="fa-stack fa-2x pull-left">
                                                    <i class="fa fa-file-o fa-stack-2x"></i>
                                                    <i class="fa fa-search fa-stack-1x"></i>
                                                </span>
                                            </p>
                                            <div class="titulo_texto_btn menor">
                                                <span class="tit_btn_ss">
                                                    Pesquisa de Currículos</span>
                                                <span class="texto_btn_ss">
                                                    <ul class="item_btn">
                                                        <li>
                                                            Filtro completo para uma busca assertiva</li>
                                                    </ul>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:LinkButton>
                        </div>
                        <%-- Fim da Coluna 03 do Menu da Tela Selecionadora --%>
                    </div>
                    <%-- Fim do Menu Tela Selecionadora --%>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnAviso" Visible="false">
                    <div class="alert alert-error">Sua empresa está bloqueada, para mais informações ligue 0800 41 2400</div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
