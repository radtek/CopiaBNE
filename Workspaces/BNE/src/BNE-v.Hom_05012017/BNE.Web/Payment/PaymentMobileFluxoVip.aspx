<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PaymentMobileFluxoVip.aspx.cs" Inherits="BNE.Web.Payment.PaymentMobileFluxoVip" %>

<%@ Register
    Src="~/Payment/component/pagamento/ucTelaPagamento.ascx"
    TagName="TelaPagamento"
    TagPrefix="uc1" %>

<%@ Register
    Src="~/Payment/component/ucLoginModal.ascx"
    TagName="ModalLogin"
    TagPrefix="uc1" %>

<!DOCTYPE html>
<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title runat="server">BNE - Mobyle Payment</title>
    <!-- Adjust Screen -->
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <!-- Roboto Font and Google icons 
    <link rel="stylesheet" href="https://fonts.googleapis.com/icon?family=Material+Icons">  -->
    <link href='https://fonts.googleapis.com/css?family=Roboto:400,300,500,700' rel='stylesheet' type='text/css' />

    <!-- Bootstrap  -->
    <link rel="stylesheet" href="css/bootstrap.min.css" />
    <!-- Main CSS (sass compiled)> -->
    <link rel="stylesheet" href="css/styles.css" />

    <!-- jQuery -->
    <script src="js/jquery.min.js"></script>

    <script type="text/javascript">
        (function (i, s, o, g, r, a, m) {
            i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
                (i[r].q = i[r].q || []).push(arguments)
            }, i[r].l = 1 * new Date(); a = s.createElement(o),
            m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
        })(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');

        var uri = document.baseURI || document.URL;
        var baseAddress = uri.replace("http://", "").replace("https://", "");
        if (baseAddress.indexOf("www.bne.com.br") == 0 || baseAddress.indexOf("bne.com.br") == 0)
            ga('create', 'UA-1937941-6', 'auto');
        else
            ga('create', 'UA-1937941-8', 'auto');

        ga('send', 'pageview');

        //Não exibir breadcrumb em mobile
        jQuery(document).ready(function ($) {
            if (navigator.userAgent.match(/Mobi/)) {
                $('#navBreadcrumbForms').css('visibility', 'hidden').css('display', 'none');
            }
        });
    </script>
</head>

<body class="mobile-payment" onload="window.scrollTo(0, 1);">
    <!-- VIP Bem-vindo -->

    <form runat="server" autocomplete="off">

        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

        <%--Integração Facebook--%>
        <div id="fb-root">
        </div>
        <script type="text/javascript">
            window.fbAsyncInit = function () {
                FB.init({
                    appId: 450210698402153,
                    status: true,
                    cookie: true,
                    xfbml: true
                });
            };

            (function (d, s, id) {
                var js, fjs = d.getElementsByTagName(s)[0];
                if (d.getElementById(id)) return;
                js = d.createElement(s); js.id = id;
                js.src = "//connect.facebook.net/pt_BR/all.js";
                fjs.parentNode.insertBefore(js, fjs);
            }(document, 'script', 'facebook-jssdk'));
        </script>
        <%--Fim Integração Facebook--%>

        <div class="fluxo-vip">
            <section id="welcome-stage" class="active">
                <div id="welcome-name" class="container-small">
                    <asp:UpdatePanel ID="updTextAcessoTopo" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                        <ContentTemplate>
                            <h4><strong>
                                <asp:Literal runat="server" ID="litNomeClienteTopo"></asp:Literal></strong></h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div id="welcome-message" class="container-small">
                    <h2>AQUI COMEÇA UMA<br />
                        <strong>NOVA FASE</strong> NA SUA<br />
                        <strong>CARREIRA</strong>!</h2>
                </div>
                <div class="stage-main-img">
                    <img class="person" src="img/welcome-stage.png">
                </div>
                <input type="image" src="img/down-arrow.png" class="down-arrow" id="pgdown-01" onclick="scroll_to_anchor('advantages01-stage'); return false;" />
            </section>

            <!-- VIP Vantagens 01 -->
            <section id="advantages01-stage">
                <div class="container-small advantage-txt">
                    <h2>QUEM É VIP TEM<br />
                        ACESSO <strong>ILIMITADO</strong> A<br />
                        <strong>TODAS AS VAGAS</strong>...</h2>
                </div>
                <input type="image" src="img/down-arrow.png" class="down-arrow" id="pgdown-02" onclick="scroll_to_anchor('advantages02-stage'); return false;" />
                <div class="stage-main-img advantage-img">
                    <img class="person" src="img/advantages-01.png">
                </div>
            </section>

            <!-- VIP Vantagens 02 -->
            <section id="advantages02-stage">
                <div class="container-small advantage-txt">
                    <h2>RECEBE UM AVISO<br />
                        A CADA <strong>VISUALIZAÇÃO</strong><br />
                        NO SEU CURRÍCULO...</h2>
                </div>
                <input type="image" src="img/down-arrow.png" class="down-arrow" id="pgdown-03" onclick="VerificaUsuarioLogado(); return false;" />
                <div class="stage-main-img advantage-img">
                    <img class="person" src="img/advantages-02.png">
                </div>
            </section>

            <!-- Sumário de compra -->
            <section id="summary-stage">
                <div class="container-small advantage-txt">
                    <h2>ESTÁ <strong>SEMPRE NO</strong><br />
                        <strong>TOPO</strong> DAS PESQUISAS<br />
                        E MUITO <strong>MAIS</strong>!</h2>
                </div>
                <div id="seleciona-plano" class="container-small origem-default">
                    <h3>Você está adquirindo um Plano VIP<br />
                        Selecione o plano ideal pra você:
                    </h3>

                    <!--RECORRENTE-->
                    <a id="PlanoRecorrente" onclick="EscolhaDePlano('Recorrente')">
                        <div class="plano assinatura">
                            <div class="nome-plano">
                                <h5><strong id="litNomePlanoRecorrente"></strong></h5>
                            </div>
                            <div class="desc-plano">
                                <h6>Apenas <span class="valor" id="litValorPlanoRecorrente"></span>por mês</h6>
                            </div>
                        </div>
                    </a>

                    <!--MENSAL-->
                    <a id="PlanoMensal" onclick="EscolhaDePlano('Mensal')">
                        <div class="plano">
                            <div class="nome-plano">
                                <h5><strong id="litNomePlanoMensal"></strong></h5>
                            </div>
                            <div class="desc-plano">
                                <h6>Apenas <span class="valor" id="litValorPlanoMensal"></span>por mês</h6>
                            </div>
                        </div>
                    </a>

                    <!--TRIMESTRAL-->
                    <a id="PlanoTrimestral" onclick="EscolhaDePlano('Trimestral')">
                        <div class="plano">
                            <div class="nome-plano">
                                <h5><strong id="litNomePlanoTrimestral"></strong></h5>
                            </div>
                            <div class="desc-plano">
                                <h6>Apenas <span class="valor" id="litValorPlanoTrimestral"></span>por mês</h6>
                            </div>
                        </div>
                    </a>

                    <a href="#" data-toggle="modal" data-target="#cupomModal">
                        <div id="cupom-desconto">
                            <div>
                                <h6><strong id="CupomDeDescontoAplicar">Eu tenho um cupom de desconto</strong></h6>
                            </div>
                        </div>
                    </a>
                </div>
            </section>

            <!-- Tela de Pagamento -->
            <uc1:TelaPagamento
                ID="ucTelaPagamento"
                runat="server" />
            <!-- Fim Tela de Pagamento -->
        </div>

        <!-- Modal Bradesco -->
        <div class="modal fade" id="redirectModalBradesco" tabindex="-1" role="dialog" aria-labelledby="redirectModalLabel">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h5 class="modal-title" id="redirectModalLabelBradesco">Finalizando a compra</h5>
                    </div>
                    <div class="modal-body">
                        Você será redirecionado para o Bradesco para finalizar a compra.   
                    </div>
                    <div class="modal-footer">
                        <button id="btBradesc" class="btn btn-success btn-block" data-loading-text="Carregando..." onclick="Pagamento_Bradesco();">OK, entendi!</button>

                    </div>
                </div>
            </div>
        </div>

        <!-- Modal BB -->
        <div class="modal fade" id="redirectModalBB" tabindex="-1" role="dialog" aria-labelledby="redirectModalLabel">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h5 class="modal-title" id="redirectModalLabelBB">Finalizando a compra</h5>
                    </div>
                    <div class="modal-body">
                        Você será redirecionado para o Banco do Brasil para finalizar a compra.   
                    </div>
                    <div class="modal-footer">
                        <button id="btBB" class="btn btn-success btn-block" data-loading-text="Carregando..." onclick="Pagamento_Banco_Do_Brasil();">OK, entendi!</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal PayPal -->
        <div class="modal fade" id="redirectModalPayPal" tabindex="-1" role="dialog" aria-labelledby="redirectModalLabel">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h5 class="modal-title" id="redirectModalLabelPayPal">Finalizando a compra</h5>
                    </div>
                    <div class="modal-body">
                        Você será redirecionado para o PayPal para finalizar a compra.   
                    </div>
                    <div class="modal-footer">
                        <button id="btPayPal" class="btn btn-success btn-block" data-loading-text="Carregando..." onclick="Pagamento_PayPal();">OK, entendi!</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal PagSeguro -->
        <div class="modal fade" id="redirectModalPagSeguro" tabindex="-1" role="dialog" aria-labelledby="redirectModalLabel">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h5 class="modal-title" id="redirectModalLabelPagSeguro">Finalizando a compra</h5>
                    </div>
                    <div class="modal-body">
                        Você será redirecionado para o Pagseguro para finalizar a compra.   
                    </div>
                    <div class="modal-footer">
                        <button id="btPagSeguro" class="btn btn-success btn-block" data-loading-text="Carregando..." onclick="Pagamento_PagSeguro();">OK, entendi"</button>
                    </div>
                </div>
            </div>
        </div>


        <!--Boleto-->
        <div class="modal fade" id="redirectModalBoleto" tabindex="-1" role="dialog" aria-labelledby="redirectModalLabel">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h5 class="modal-title" id="redirectModalLabelBoleto">Mensagem enviada com sucesso!</h5>
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal Cupom de Desconto-->
        <div class="modal fade" id="cupomModal" tabindex="-1" role="dialog" aria-labelledby="cupomModalLabel">
            <div class="vertical-alignment-helper">
                <div class="modal-dialog vertical-align-center" role="document">

                    <div class="modal-content">
                        <div class="modal-header">

                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h5 class="modal-title" id="redirectModalLabel">CUPOM DE DESCONTO</h5>
                        </div>
                        <div class="modal-body row ">
                            <div class="col-xs-12 form-group" id="erroCupom">
                                <input id="IdCupomDeDescontoText" type="text" class="form-control input-lg" placeholder="Digite seu código de desconto" />
                                <span class="help-block" id="msgErroCupom"></span>
                            </div>
                            <div class="col-xs-12 form-group">
                                <button id="idCupomDeDesconto" type="button" class="btn btn-lg btn-primary btn-block" onclick="calculaCupomDeDesconto();">APLICAR DESCONTO</button>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>

        <!-- Modal de Login -->
            <uc1:ModalLogin
                ID="ModalLogin"
                runat="server" />
            <!-- Modal de Login -->

    </form>

    <input id="txtPlanoAdquirido" type="hidden" runat="server" />
    <input id="txtPessoaFisica" type="hidden" runat="server" />
    <input id="txtCurriculo" type="hidden" runat="server" />
    <input id="txtUsuarioFilialPerfil" type="hidden" runat="server" />
    <input id="txtUniversitarioSTC" type="hidden" runat="server" />
    <input id="txtIdCodigoDeDesconto" type="hidden" runat="server" />

    <input id="txtIdPlanoRecorrente" type="hidden" runat="server" />
    <input id="txtIdPlanoMensal" type="hidden" runat="server" />
    <input id="txtIdPlanoTrimestral" type="hidden" runat="server" />



    <!-- Bootstrap JS -->
    <script src="js/bootstrap.min.js"></script>

    <!--Mascara-->
    <script src="js/jquery.maskedinput.min.js"></script>
    <script src="js/jquery.validate.min.js"></script>
    <script src="js/clipboard.min.js"></script>

    <!--Pagamento-->
    <script src="js/jquery.payment.js"></script>
    <script src="js/pagamento-validation.js"></script>
    <script src="js/pagamento-login.js"></script>
    <script src="js/pagamento-mobile.js"></script>
    <script src="js/pagamento-scroll-accordion.js"></script>
    <script src="js/pagamento-escolha-de-plano-vip.js"></script>
    <script src="js/pagamento-cupom-de-desconto.js"></script>
</body>
</html>
