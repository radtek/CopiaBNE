<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="SalaSelecionador.aspx.cs" Inherits="BNE.Web.SalaSelecionador" %>

<%@ Register Src="UserControls/Forms/SalaSelecionador/Dados.ascx" TagName="Dados"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    
    <style>
        .ModalBanner { width: 348px !important; height: 350px !important; left: 56% !important; background: transparent !important; border: 0px solid !important; }
        .close { color: #fff !important; text-shadow: none !important; opacity: 100 !important; filter: inherit; }
        .modal { margin-left: -356px !important; width:auto !important; box-shadow: none !important;}
    </style>

    <link href="css/local/Bootstrap/bootstrap.css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/Sala_Selecionador.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicScript runat="server" Src="/js/local/SalaSelecionador.js" type="text/javascript" />
    <script src="js/bootstrap/bootstrap.js"></script>

    <script>
        var abrirModal = '';
        $(document).ready(function () {
            if (abrirModal == true)
                $('#modalBanner').modal('show');

            $('#modalBanner').on('hide', function () {
                $("#video_player").attr("src", "");
            })

            
        });

        function AbrirBanner(value) {
            abrirModal = value;
        }



    </script>

     
    <link rel="Stylesheet" type="text/css" href="/css/local/Sala_Selecionador.css" />
    <script src="js/local/SalaSelecionador.js" type="text/javascript"></script>
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
                                           <i class="fa fa-bullhorn fa-5x pull-left"></i>
                                            <div class="titulo_texto_btn menor">
                                                <span class="tit_btn_ss">Anunciar Vagas</span> <span class="texto_btn_ss">
                                                    <ul class="item_btn">
                                                        <li id="liVagasAnunciadas" runat="server"><b>
                                                            <asp:Label ID="lblVagasAnunciadasQuantidade" runat="server"></asp:Label></b>
                                                            <asp:Label ID="lblVagasAnunciadasMensagem" runat="server"></asp:Label></li>
                                                        <li id="liCVsRecebidos" runat="server"><b>
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
                            <asp:LinkButton ID="BotaoMeuPlano" runat="server" OnClick="btlCompraPlanos_Click"
                                CausesValidation="false" ToolTip="Meu Plano">
                                <div class="btn_col bottom">
                                    <div class="fundo_btn_ss">
                                        <div class="sombra_btn_ss">
                                            <i class="fa fa-check fa-5x pull-left"></i>
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
                        </div>
                        <%-- Fim da Coluna 01 do Menu da Tela Selecionadora --%>
                        <%-- Coluna 02 do Menu da Tela Selecionadora --%>
                        <div class="col02">
                            <asp:LinkButton ID="btlConfiguracoes" runat="server" OnClick="btlConfiguracoes_Click"
                                CausesValidation="false" ToolTip="Configurações">
                                <div class="btn_col">
                                    <div class="fundo_btn_ss">
                                        <div class="sombra_btn_ss">
                                            <i class="fa fa-cogs fa-5x pull-left"></i>
                                            <div class="titulo_texto_btn menor">
                                                <span class="tit_btn_ss">
                                                    Configurações</span>
                                                <span class="texto_btn_ss">
                                                    <ul class="item_btn">
                                                        <li>
                                                            Personalize o retorno para os candidatos</li>
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

                            <asp:LinkButton ID="BotaoRecrutamento" runat="server" OnClick="btlRequisicaoR1_Click"
                                CausesValidation="false" ToolTip="Recrutamento R1">
                                <div class="btn_col">
                                    <div class="fundo_btn_ss fundo_btn_ss-adjust">
                                        <div class="sombra_btn_ss">
                                            <img src="../../../img/SalaSelecionadora/icn_R1.png" alt="Recrutamento R1" />
                                            <div class="titulo_texto_btn menor">
                                                <div class="titulo_texto_btn menor">
                                                <span class="tit_btn_ss">Recrutamento R1</span> <span class="texto_btn_ss">
                                                    <ul class="item_btn">
                                                        <li><b>
                                                            <asp:Label ID="lblQuantidadeSolicitacao" runat="server"></asp:Label></b>
                                                            <asp:Label ID="lblSolicitacaoMensagem" runat="server"></asp:Label>
                                                        </li>
                                                        <li><b>
                                                            <asp:Label ID="lblQuantidadeCvsRecebidos" runat="server"></asp:Label></b>
                                                            <asp:Label ID="lblCvsRecebidosMensagem" runat="server"></asp:Label>
                                                        </li>
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

    <div id="modalBanner" class="modal hide fade modal-size ModalBanner" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
        <iframe id="video_player" width="560" height="315" src="https://www.youtube.com/embed/bDc4uaQQHHY" frameborder="0" allowfullscreen></iframe>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
