/*PAGE LOAD*/
/*------------------- Scroll Collapse Formas de Pagamento -------------------*/
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

function checkOrientation() {
    var tela = screen;

    if (tela.orientation == null)
        tela = window;

    if (typeof (tela.orientation) == "number") {
        if (tela.orientation != 0) {
            $("img.person").hide();
        }
        else {
            $("img.person").show();
        }
    }
    else if (typeof (tela.orientation) == "object") {
        if (tela.orientation.angle > 0) {
            $("img.person").hide();
        }
        else {
            $("img.person").show();
        }
    }

    $('#log').text($('section.active').offset().top);

    $('html, body').animate({ scrollTop: $('section.active').offset().top }, 'fast');

}

$(document).ready(function () {
    window.addEventListener("orientationchange", checkOrientation);
    checkOrientation();
    $('html, body').animate({ scrollTop: 0 }, 'fast');
});


$(document).ready(function () {
    inicializa_cartao_credito();
    inicializa_debito_HSBC();
    inicializa_login_BNE();

    var clipboard = new Clipboard('#btnCopiar');

    clipboard.on('success', function (e) {
        $('#btnCopiar').val("Copiado!");
    });

});

function inicializa_cartao_credito() {
    $("#txtMesValidade").mask("99", { placeholder: "_" });
    $("#txtAnoValidade").mask("99", { placeholder: "_" });
}

function inicializa_debito_HSBC() {

    On_Change_Por_Radio_CNPJ_CPF();

    $("#txtAgenciaDebitoHSBC").mask("9999", { placeholder: "_" });
    $("#txtAgenciaDebitoHSBC").focusout(function () { $("#txtAgenciaDebitoHSBC").val(numericoParaTamanhoExato($("#txtAgenciaDebitoHSBC").val(), 4)); });

    $("#txtContaCorrenteDebitoHSBC").mask("99999", { placeholder: "_" });
    $("#txtContaCorrenteDebitoHSBC").focusout(function () { $("#txtContaCorrenteDebitoHSBC").val(numericoParaTamanhoExato($("#txtContaCorrenteDebitoHSBC").val(), 5)); });

    $("#txtDigitoDebitoHSBC").mask("99", { placeholder: "_" });
    $("#txtDigitoDebitoHSBC").focusout(function () { $("#txtDigitoDebitoHSBC").val(numericoParaTamanhoExato($("#txtDigitoDebitoHSBC").val(), 2)); });
}

function inicializa_login_BNE() {
    $("#ModalLoginCPF").mask("999.999.999-99");
    $('#ModalLoginDataDeNascimento').mask("99/99/9999");
}

function numericoParaTamanhoExato(val, len) {
    val = val.replace(/[^0-9]/g, "");
    if (val.length <= 0) {
        return "";
    }
    var zeros = len - val.length;
    for (var i = zeros; i > 0; i--) {
        val = 0 + val;
    }
    return val;
}

/*FIM PAGE LOAD*/
/*-------------------------------------------------------------------------------*/

/******************** EVENTOS *******************************/

/*------------CHANGE------------*/
function On_Change_Por_Radio_CNPJ_CPF() {
    var radioButtons = document.getElementsByName("tpdebt");
    for (var x = 0; x < radioButtons.length; x++) {
        if (radioButtons[x].checked) {
            if (radioButtons[x].value == "CNPJ") {
                $("#txtCPFouCNPJHSBC").attr("placeholder", "CNPJ do Titular da Conta");
                $("#txtCPFouCNPJHSBC").mask("99.999.999/9999-99");
            } else {
                $("#txtCPFouCNPJHSBC").attr("placeholder", "CPF do Titular da Conta");
                $("#txtCPFouCNPJHSBC").mask("999.999.999-99");
            }
        }
    }
}

/*---------------FIM CHANGE-----------------*/

/*-------------CLICK ----------------------- */

function bloquearBotoes(valor, nameButton) {

    if (nameButton == "#btnCopiar") {
        $("#btnCopiar").data('loading-text', 'Copiando...');
        $("#btnEnviarPorEmail").data('loading-text', 'ENVIAR POR E-MAIL');
    } else if (nameButton == "#btnEnviarPorEmail") {
        $("#btnEnviarPorEmail").data('loading-text', 'Enviando...');
        $("#btnCopiar").data('loading-text', 'COPIAR CÓDIGO DE BARRAS');
    }

    if (valor) {
        $("#btPayPal").button("loading");
        $("#btnCopiar").button("loading");
        $("#btPagSeguro").button("loading");
        $("#btPagamentoBB").button("loading");
        $("#btnEnviarPorEmail").button("loading");
        $("#btPagamentoBradesco").button("loading");
        $("#btnFinalizarDebitoHSBC").button("loading");
        $("#btFinalizarCartaoDeCredito").button("loading");
    } else {
        $("#btPayPal").button("reset");
        $("#btnCopiar").button("reset");
        $("#btPagSeguro").button("reset");
        $("#btPagamentoBB").button("reset");
        $("#btnEnviarPorEmail").button("reset");
        $("#btPagamentoBradesco").button("reset");
        $("#btnFinalizarDebitoHSBC").button("reset");
        $("#btFinalizarCartaoDeCredito").button("reset");

    }
}

//PAYPAL
function Pagamento_PayPal() {
    //Recupera os Dados
    var Dados = {};
    Dados.idPlanoAdquirido = $("#txtPlanoAdquirido").val();
    Dados.idPessoaFisica = $("#txtPessoaFisica").val();

    //bloqueio de botões
    bloquearBotoes(true, "");

    //Envia a requisição
    $.ajax({
        type: "POST",
        url: "Payment.aspx/PagamentoPayPal",
        data: JSON.stringify(Dados),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            window.location = response.d.Url;
            bloquearBotoes(false, "");
        },
        error: function (response) {
            redirectPageError(Dados.idPessoaFisica, Dados.idPlanoAdquirido);
        }
    });
}

//CARTAO DE CREDITO
function Pagamento_Cartao_De_Credito() {

    if (!valida_mes_e_ano())
        return;
    //Validações
    if ($("#campo_cartao_credito").hasClass("has-error") || $("#campo_mes_validade").hasClass("has-error") || $("#campo_ano_validade").hasClass("has-error")
        || $("#campo_cod_seg").hasClass("has-error"))
        return;

    if ($("#txtNumeroCartao").val() == "" || $("#txtMesValidade").val() == "" || $("#txtAnoValidade").val() == "" ||
    $("#txtCodigoSeguranca").val() == "" || $("#txtPlanoAdquirido").val() == "")
        return;

    //Coloca o Botão em Loading
    bloquearBotoes(true, "");

    //Recupera os Dados
    var Dados = {};
    Dados.numeroCartao = $("#txtNumeroCartao").val();
    Dados.mesValidade = $("#txtMesValidade").val();
    Dados.anoValidade = $("#txtAnoValidade").val();
    Dados.codigoSeguranca = $("#txtCodigoSeguranca").val();
    Dados.idPlanoAdquirido = $("#txtPlanoAdquirido").val();
    Dados.idPessoaFisica = $("#txtPessoaFisica").val();

    //Envia a requisição
    $.ajax({
        type: "POST",
        url: "Payment.aspx/PagamentoCartaoDeCredito",
        data: JSON.stringify(Dados),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            window.location = response.d.Url;
            bloquearBotoes(false, "");
        },
        error: function (response) {
            redirectPageError(Dados.idPessoaFisica, Dados.idPlanoAdquirido);
        }
    });
}

//BOLETO - DOWNLOAD
function Pagamento_Boleto_Copiar() {

    if ($('#boletoCollapseType').attr('aria-expanded') == 'true')
        return;

    //bloquear buttons
    bloquearBotoes(true, "");

    //Recupera os Dados
    var Dados = {};
    Dados.idPessoaFisica = $("#txtPessoaFisica").val();
    Dados.idPlanoAdquirido = $("#txtPlanoAdquirido").val();
    Dados.isEmail = 0;

    //Envia a requisição
    $.ajax({
        type: "POST",
        url: "Payment.aspx/PagamentoBoleto",
        data: JSON.stringify(Dados),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {

            var result = response.d;

            if (result.Falha)
                window.location = result.Url;
            else {
                var boleto = result.Dados;
                boleto = boleto.substring(0, 5) + "." + boleto.substring(5, 10) + " " +
                    boleto.substring(10, 15) + "." + boleto.substring(15, 20) + " " +
                    boleto.substring(20, 25) + "." + boleto.substring(25, 30) + " " +
                    boleto.substring(30, 47);

                $("#copyTxtArea").val(boleto);
            }
            bloquearBotoes(false, "");
        },
        error: function (response) {
            redirectPageError(Dados.idPessoaFisica, Dados.idPlanoAdquirido);
        }
    });
}

//BOLETO - COPIAR 
function Pagamento_Boleto_EnviarEmail() {

    //bloquear botoes

    bloquearBotoes(true, "#btnEnviarPorEmail");

    //Recupera os Dados
    var Dados = {};
    Dados.idPessoaFisica = $("#txtPessoaFisica").val();
    Dados.idPlanoAdquirido = $("#txtPlanoAdquirido").val();
    Dados.isEmail = 1;

    //Envia a requisição
    $.ajax({
        type: "POST",
        url: "Payment.aspx/PagamentoBoleto",
        data: JSON.stringify(Dados),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $("#redirectModalBoleto").modal('show');
            bloquearBotoes(false, "");
        },
        error: function (response) {
            redirectPageError(Dados.idPessoaFisica, Dados.idPlanoAdquirido);
        }
    });
}

//PAGSEGURO
function Pagamento_PagSeguro() {

    //Recupera os Dados
    bloquearBotoes(true, "");

    var Dados = {};
    Dados.idPlanoAdquirido = $("#txtPlanoAdquirido").val();
    Dados.idPessoaFisica = $("#txtPessoaFisica").val();

    //Envia a requisição
    $.ajax({
        type: "POST",
        url: "Payment.aspx/PagamentoPagSeguro",
        data: JSON.stringify(Dados),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            window.location = response.d.Url;
            bloquearBotoes(false, "");
        },
        error: function (response) {
            redirectPageError(Dados.idPessoaFisica, Dados.idPlanoAdquirido);
        }
    });

}

//HSBC
function Pagamento_HSBC(evt) {

    //Validações
    if ($("#campo_cpf_cnpj").hasClass("has-error") || $("#campo_agencia").hasClass("has-error") || $("#campo_conta_corrente").hasClass("has-error")
        || $("#campo_codigo_seguranca").hasClass("has-error")) {
        //evt.preventDefault();
        return;

    }


    if ($("#txtCPFouCNPJHSBC").val() == "" || $("#txtAgenciaDebitoHSBC").val() == "" || $("#txtContaCorrenteDebitoHSBC").val() == "" ||
            $("#txtDigitoDebitoHSBC").val() == "" || $("#txtPlanoAdquirido").val() == "") {
        //evt.preventDefault();
        return;

    }

    //Coloca o Botão em Loading
    bloquearBotoes(true, "");
    //Recupera os Dados

    var Dados = {};
    Dados.cpfOuCnpj = $("#txtCPFouCNPJHSBC").val();
    Dados.agencia = $("#txtAgenciaDebitoHSBC").val();
    Dados.conta = $("#txtContaCorrenteDebitoHSBC").val();
    Dados.digito = $("#txtDigitoDebitoHSBC").val();
    Dados.idPlanoAdquirido = $("#txtPlanoAdquirido").val();
    Dados.idPessoaFisica = $("#txtPessoaFisica").val();

    //Envia a requisição
    $.ajax({
        type: "POST",
        url: "Payment.aspx/PagamentoDebitoRecorrenteHSBC",
        data: JSON.stringify(Dados),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            window.location = response.d.Url;
            bloquearBotoes(false, "");
        },
        error: function (response) {
            redirectPageError(Dados.idPessoaFisica, Dados.idPlanoAdquirido);
        }
    });
}

//BRADESCO
function Pagamento_Bradesco() {

    //Coloca o Botão em Loading
    bloquearBotoes(true, "");

    //Recupera os Dados
    var Dados = {};
    Dados.idPlanoAdquirido = $("#txtPlanoAdquirido").val();
    Dados.idPessoaFisica = $("#txtPessoaFisica").val();

    //Envia a requisição
    $.ajax({
        type: "POST",
        url: "Payment.aspx/PagamentoBradescoOnline",
        data: JSON.stringify(Dados),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            window.location = response.d.Url;
            bloquearBotoes(false, "");
        },
        error: function (response) {
            redirectPageError(Dados.idPessoaFisica, Dados.idPlanoAdquirido);
        }
    });
}

//BANCO DO BRASIL
function Pagamento_Banco_Do_Brasil() {

    //Coloca o Botão em Loading

    bloquearBotoes(true, "");
    //Recupera os Dados
    //Recupera os Dados
    var Dados = {};
    Dados.idPlanoAdquirido = $("#txtPlanoAdquirido").val();
    Dados.idPessoaFisica = $("#txtPessoaFisica").val();

    //Envia a requisição
    $.ajax({
        type: "POST",
        url: "Payment.aspx/PagamentoBancoDoBrasilOnline",
        data: JSON.stringify(Dados),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            window.location = response.d.Url;
            bloquearBotoes(false, "");
        },
        error: function (response) {
            redirectPageError(Dados.idPessoaFisica, Dados.idPlanoAdquirido);
        }
    });
}

//Redirecionamento de Erro
function redirectPageError(idPessoaFisica, idPlanoAdquirido) {
    var Dados = {};
    Dados.idPessoaFisica = idPessoaFisica;
    Dados.idPlanoAdquirido = idPlanoAdquirido;
    $.ajax({
        type: "POST",
        url: "Payment.aspx/RedirectTelaPaymentMobileErro",
        data: JSON.stringify(Dados),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            window.location = response.d.Url;
            bloquearBotoes(false);
        },
        error: function (response) {
            bloquearBotoes(false, "");
        }
    });
}

/*---------------------FIM CLICK ----------------------*/

/******************** FIM EVENTOS **************************/

