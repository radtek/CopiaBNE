<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PaymentMobileErro.aspx.cs" Inherits="BNE.Web.Payment.PaymentMobileErro" %>

<!DOCTYPE html>
<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title runat="server">BNE - Mobyle Payment</title>
    <!-- Adjust Screen -->
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <!-- Roboto Font and Google icons 
    <link rel="stylesheet" href="https://fonts.googleapis.com/icon?family=Material+Icons">  -->
    <link href='https://fonts.googleapis.com/css?family=Roboto:400,300,500,700' rel='stylesheet' type='text/css'>

    <!-- Bootstrap  -->
    <link rel="stylesheet" href="css/bootstrap.min.css">
    <!-- Main CSS (sass compiled)> -->
    <link rel="stylesheet" href="css/styles.css">
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
<body class="mobile-payment return" style="background-color: #FDC215 !important;">
    <form id="form1" runat="server" autocomplete="off">
        <!-- Header -->
        <section id="mobile-payment-header">
            <div id="mobile-payment-header-content" class="container">
                <div id="logo">
                    <img src="img/logo-bne-pf.png">
                </div>
                <div id="secure">Você está em um ambiente seguro!<img src="img/lock.png" width="9" height="12"></div>
            </div>
        </section>
        <!-- Retorno -->
        <section id="mobile-payment-return " class="container-small">

            <div id="payment-return">
                <!-- Liberação Imediata -->
                <div class="col-xs-12 payment-return-icon">
                    <img src="img/payment-error.png">
                </div>
                <div class="col-xs-12">
                    <h2>Ops, algo deu errado!</h2>
                </div>
                <div class="col-xs-12">
                    <h4>Ocorreu um erro no pagamento. Entre em contato com seu banco.</br></br>
                    Você pode tentar outra forma de pagamento ou falar conosco pelo <a href="tel:0800412400"><strong style="white-space: nowrap">0800 41 2400</strong></a>.</h4>
                </div>
                <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 payment-return-actions">
                    <asp:Button ID="btnPagamento" runat="server" class="btn btn-lg btn-primary btn-block" Text="Formas de Pagamento" OnClick="btnPagamento_Click" />
                </div>
                <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 payment-return-actions">
                    <asp:Button ID="btnHome" runat="server" class="btn btn-lg btn-block" Text="Página inicial" OnClick="btnHome_Click" />
                </div>
            </div>
        </section>
        <div class="clear"></div>
        <!-- Footer -->
        <section id="mobile-payment-footer" class="container-small">
            <p>Banco Nacional de Empregos © 2016</p>
        </section>
        <input id="txtPlano" type="hidden" runat="server" />
        <input id="txtPessoaFisica" type="hidden" runat="server" />
    </form>
    <!-- jQuery -->
    <script src="js/jquery.min.js"></script>
    <!-- Bootstrap JS -->
    <script src="js/bootstrap.min.js"></script>
    <script>

        /*-------------------*/
        var hasClicked = false;
        $('.payment-method-type').click(function (e) {
            if (!hasClicked) {
                $('html, body').animate({
                    scrollTop: $(this).offset().top - 8
                }, 'slow');
                hasClicked = !hasClicked;
            }
        });

        $('#accordion').on('hidden.bs.collapse', function () {
            $('html, body').animate({
                scrollTop: $("div[aria-expanded='true']").offset().top - 8
            }, 'slow');
        });
        /*-------------------*/

    </script>
</body>
</html>
