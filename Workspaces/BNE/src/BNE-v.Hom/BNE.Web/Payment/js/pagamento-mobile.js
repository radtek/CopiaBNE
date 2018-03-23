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

/*------------------- Botão Rolar pra baixo —-----------------*/
function scroll_to_anchor(anchor_id) {
    var section = $("#" + anchor_id + "");
    section.addClass("active");
    $("body.mobile-payment").css("background-color", section.attr("next-color"));

    setTimeout(function () {
        $('html, body').animate({ scrollTop: section.offset().top }, 1000, function () {
            section.prev().removeClass("active");
        });

    }, 50);

    if ("payment-stage" == anchor_id) {
        $("body.mobile-payment").css("overflow", "auto");

    }
    return false;
}

function checkOrientation() {
    $('#log').text('foi...');
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

    var clipboard = new Clipboard('#btnCopiar');

    clipboard.on('success', function (e) {
        $('#btnCopiar').val("Copiado!");
    });

});

function inicializa_cartao_credito() {
    removeErros();
    $("#txtNumeroCartao").mask("9999 9999 9999 9999", { placeholder: "_" });
    $("#txtMesValidade").mask("99", { placeholder: "_" });
    $("#txtAnoValidade").mask("99", { placeholder: "_" });
    $("#txtCodigoSeguranca").mask("999", { placeholder: "_" });
    
}

function removeErros()
{
    $("#txtNumeroCartao").val("");
    $("#txtMesValidade").val("");
    $("#txtAnoValidade").val("");
    $("#txtCodigoSeguranca").val("");

    $("#campo_mes_validade").removeClass("has-error");
    $("#campo_ano_validade").removeClass("has-error");
    $("#txtMesValidade_custom").hide();
    $("#txtAnoValidade_custom").hide();
}

function inicializa_debito_HSBC() {

    $("#updDebitoEmConta").remove();
    On_Change_Por_Radio_CNPJ_CPF();

    $("#txtAgenciaDebitoHSBC").mask("9999", { placeholder: "_" });
    $("#txtAgenciaDebitoHSBC").focusout(function () { $("#txtAgenciaDebitoHSBC").val(numericoParaTamanhoExato($("#txtAgenciaDebitoHSBC").val(), 4)); });

    $("#txtContaCorrenteDebitoHSBC").mask("99999", { placeholder: "_" });
    $("#txtContaCorrenteDebitoHSBC").focusout(function () { $("#txtContaCorrenteDebitoHSBC").val(numericoParaTamanhoExato($("#txtContaCorrenteDebitoHSBC").val(), 5)); });

    $("#txtDigitoDebitoHSBC").mask("99", { placeholder: "_" });
    $("#txtDigitoDebitoHSBC").focusout(function () { $("#txtDigitoDebitoHSBC").val(numericoParaTamanhoExato($("#txtDigitoDebitoHSBC").val(), 2)); });
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
            console.log(radioButtons[x].value);
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
        url: "PaymentMobile.aspx/PagamentoPayPal",
        data: JSON.stringify(Dados),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var result = response.d;

            if (result.status == "OK")
                window.location = result.url;
            else
                window.location = result.url;

            bloquearBotoes(false, "");
        },
        error: function (response) {
            redirectPageError(Dados.idPessoaFisica, Dados.idPlanoAdquirido);
        }
    });
}

//CARTAO DE CREDITO
$("#txtNomeCartao").blur(function(val, args) {
    var valor = $("#txtNomeCartao").val().toUpperCase();
    $("#txtNomeCartao").val(valor);
});


$("#txtNomeCartao").keyup(function () {
    setTimeout(function() {
        var valor = $("#txtNomeCartao").val().replace(/[^a-zA-Z" "]+/g, '');
        $("#txtNomeCartao").val(valor);
    }, 1);
});

function Pagamento_Cartao_De_Credito() {

    //Validações
    if ($("#campo_cartao_credito").hasClass("has-error") || $("#campo_mes_validade").hasClass("has-error") || $("#campo_ano_validade").hasClass("has-error")
        || $("#campo_cod_seg").hasClass("has-error") || $("#campo_cartao_nome").hasClass("has-error"))
        return;

    if ($("#txtNumeroCartao").val() == "" || $("#txtMesValidade").val() == "" || $("#txtAnoValidade").val() == "" ||
    $("#txtCodigoSeguranca").val() == "" || $("#txtPlanoAdquirido").val() == "" || $("#txtNomeCartao").val() == "")
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
    Dados.nomeCartaoCredito = $("#txtNomeCartao").val();

    //Envia a requisição
    $.ajax({
        type: "POST",
        url: "PaymentMobile.aspx/PagamentoCartaoDeCredito",
        data: JSON.stringify(Dados),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            window.location = response.d.url;
            bloquearBotoes(false, "");
        },
        error: function (response) {
            redirectPageError(Dados.idPessoaFisica, Dados.idPlanoAdquirido);
        }
    });
}


function Pagamento_Cartao_De_Credito_Gratis() {

    //Validações
    if ($("#campo_cartao_credito").hasClass("has-error") || $("#campo_mes_validade").hasClass("has-error") || $("#campo_ano_validade").hasClass("has-error")
        || $("#campo_cod_seg").hasClass("has-error") || $("#campo_cartao_nome").hasClass("has-error"))
        return;

    if ($("#txtNumeroCartao").val() == "" || $("#txtMesValidade").val() == "" || $("#txtAnoValidade").val() == "" ||
        $("#txtCodigoSeguranca").val() == "" || $("#txtPlanoAdquirido").val() == "" || $("#txtNomeCartao").val() == "")
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
    Dados.nomeCartaoCredito = $("#txtNomeCartao").val();

    //Envia a requisição
    $.ajax({
        type: "POST",
        url: "PaymentMobile.aspx/PagamentoCartaoDeCreditoGratis",
        data: JSON.stringify(Dados),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            window.location = response.d.url;
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
        url: "PaymentMobile.aspx/PagamentoBoleto",
        data: JSON.stringify(Dados),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response.d.status == "OK") {
                var boleto = response.d.text;
                boleto = boleto.substring(0, 5) + "." + boleto.substring(5, 10) + " " +
                    boleto.substring(10, 15) + "." + boleto.substring(15, 20) + " " +
                    boleto.substring(20, 25) + "." + boleto.substring(25, 30) + " " +
                    boleto.substring(30, 47);

                $("#copyTxtArea").val(boleto);
            } else {
                window.location = response.d.text;
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
        url: "PaymentMobile.aspx/PagamentoBoleto",
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
        url: "PaymentMobile.aspx/PagamentoPagSeguro",
        data: JSON.stringify(Dados),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            window.location = response.d.url;
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
        url: "PaymentMobile.aspx/PagamentoDebitoRecorrenteHSBC",
        data: JSON.stringify(Dados),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var result = response.d;
            //if (result.status == "OK")
                window.location = result.url;
            //else {
            //    $("#textError").val(result.msg);
            //}
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
        url: "PaymentMobile.aspx/PagamentoBradescoOnline",
        data: JSON.stringify(Dados),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            window.location = response.d.url;
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
        url: "PaymentMobile.aspx/PagamentoBancoDoBrasilOnline",
        data: JSON.stringify(Dados),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            window.location = response.d.url;
            bloquearBotoes(false, "");
        },
        error: function (response) {
            redirectPageError(Dados.idPessoaFisica, Dados.idPlanoAdquirido);
        }
    });
}

function redirectPageError(idPessoaFisica, idPlanoAdquirido) {
    var Dados = {};
    Dados.idPessoaFisica = idPessoaFisica;
    Dados.idPlanoAdquirido = idPlanoAdquirido;
    $.ajax({
        type: "POST",
        url: "PaymentMobile.aspx/RedirectTelaPaymentMobileErro",
        data: JSON.stringify(Dados),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            window.location = response.d;
            ibloquearBotoes(false);
        },
        error: function (response) {
            bloquearBotoes(false, "");
        }
    });
}

/*---------------------FIM CLICK ----------------------*/

/******************** FIM EVENTOS **************************/


/*Validations*/
/*-------------------------------------------------------------------------------*/

function valida_nome_cartao(val, args) {
    var checaRegularExpress = /[^A-Z" "]/;

    var isValid = !checaRegularExpress.test(args);
    if (isValid) {
        $("#campo_cartao_nome").toggleClass("has-error");
    }

}



function valida_cartao(val, args) {
    var isValid = false;
    var ccCheckRegExp = /[^0-9 ]/;
    isValid = !ccCheckRegExp.test(args.Value);
    if (isValid) {
        var cardNumbersOnly = args.Value.replace(/ /g, "");
        var cardNumberLength = cardNumbersOnly.length;
        var lengthIsValid = (cardNumberLength == 16 || cardNumberLength == 13 || cardNumberLength == 15 || cardNumberLength == 14);
        var prefixIsValid = false;
        var prefixRegExp = /^6504|^6550|^5([1-5])|^3[47]|^4|^38[0-9]|^60[0-9]|^30[0-5]|^(30[6-9]|36|38)|^(63|438935|504175|451416|5067|4576|4011|506699)|^(4026|417500|4508|4844|491(3|7))|^(50)|^(6011|622(12[6-9]|1[3-9][0-9]|[2-8][0-9]{2}|9[0-1][0-9]|92[0-5]|64[4-9])|65) /;

        prefixIsValid = prefixRegExp.test(cardNumbersOnly);
        isValid = prefixIsValid && lengthIsValid;
    }

    var classcartaoAplicada = $("#campo_cartao_credito").hasClass("has-error");

    if (!isValid && !classcartaoAplicada) $("#campo_cartao_credito").toggleClass("has-error");
    else if (isValid && classcartaoAplicada) $("#campo_cartao_credito").toggleClass("has-error");

    console.log(isValid);
    args.IsValid = isValid;
}


function AlteraBandeira() {
    $("#imageCartaoTodos").hide();
    $("#imageCartao").show();

    var bandeira = $("#selBandeira").val();
    console.log(bandeira);
    if (bandeira == "amex") {
        removeErros();
        $("#txtNumeroCartao").mask("9999 999999 99999", { placeholder: "_" });
        $("#txtCodigoSeguranca").mask("9999", { placeholder: "_" });
        $("#imageCartao").attr("src", "../Payment/img/method-amex.png");
        return;
    }

    if (bandeira == "dinners") {
        removeErros();
        $("#txtNumeroCartao").mask("9999 9999 9999 99  ", { placeholder: "_" });
        $("#txtCodigoSeguranca").mask("999", { placeholder: "_" });
        $("#imageCartao").attr("src", "../Payment/img/method-dinners.png");
        return;
    }

    if (bandeira == "elo") {
        
        $("#imageCartao").attr("src", "../Payment/img/method-elo.png");

    }
    if (bandeira == "visa") {
        $("#imageCartao").attr("src", "../Payment/img/method-visa.png");
        
    }
    if (bandeira == "master") {
        $("#imageCartao").attr("src", "../Payment/img/method-mastercard.png");
        
    }
    if (bandeira == "hipercard") {
        $("#imageCartao").attr("src", "../Payment/img/method-hipercard.png");
        
    }
    if (bandeira == "aura") {
        $("#imageCartao").attr("src", "../Payment/img/method-aura.png");
        
    }
    if (bandeira == "discover") {
        $("#imageCartao").attr("src", "../Payment/img/method-discover.png");
        
    }

    inicializa_cartao_credito();

}

function ValidaBandeira() {
    console.log('valida bandeira');
    var number = $("#txtNumeroCartao").val().replace(/[^0-9]+/g, '');

    if (number.length == 0) {
        //$("#imageCartao").attr("src", "Payment/img/method-vmeha.png");
        return;
    }

    if (number.length > 4)
        return;


    if (re = new RegExp('^38[0-9]'), null != number.match(re)) {
        $("#imageCartao").attr("src", "../Payment/img/method-hipercard.png")
        return;
    }

    if (re = new RegExp('^60[0-9]'), null != number.match(re)) {
        $("#imageCartao").attr("src", "../Payment/img/method-hipercard.png")
        return;
    }

    if (re = new RegExp('^30[0-5]'), null != number.match(re)) {
        $("#imageCartao").attr("src", "../Payment/img/method-dinners.png");
        return;
    }

    if (re = new RegExp('^(30[6-9]|36|38)'), null != number.match(re)) {
        $("#imageCartao").attr("src", "../Payment/img/method-dinners.png");
        return;
    }

    if (re = new RegExp("^3[47]"), null != number.match(re)) {
        var valorDigitado = $("#txtNumeroCartao").val().replace(/[^\d]+/g, '');

        if (valorDigitado.length > 2) {
            return;
        }

        $("#imageCartao").attr("src", "../Payment/img/method-amex.png");

        return;
    }

    if (re = new RegExp("^(63|438935|504175|451416|5067|4576|4011|506699|6504|6550)"), null != number.match(re)) {
        $("#imageCartao").attr("src", "../Payment/img/method-elo.png");
        return;
    }

    if (re = new RegExp("^(4026|417500|4508|4844|491(3|7))"), null != number.match(re)) {
        $("#imageCartao").attr("src", "../Payment/img/method-visa.png");
        return;
    }


    if (re = new RegExp("^(50)"), null != number.match(re)) {
        $("#imageCartao").attr("src", "../Payment/img/method-aura.png");
        return;
    }

    var re = new RegExp("^4");

    if (null != number.match(re)) {
        $("#imageCartao").attr("src", "../Payment/img/method-visa.png");
        return;
    }

    if (re = new RegExp("^5[1-5]"), null != number.match(re)) {
        $("#imageCartao").attr("src", "../Payment/img/method-mastercard.png");
        return;
    }

    if (re = new RegExp("^(6011|622(12[6-9]|1[3-9][0-9]|[2-8][0-9]{2}|9[0-1][0-9]|92[0-5]|64[4-9])|65)"), null != number.match(re)) {
        $("#imageCartao").attr("src", "../Payment/img/method - discover.png");
        return;
    }

    $("#imageCartao").attr("src", "../Payment/img/method-all.png");
}

function valida_range_mes(val, args) {

    var mes = $("#txtMesValidade").val().toString();

    if (isNaN(mes)) {
        args.IsValid = true;
        return;
    }

    var isMesValid = false;
    var regExpMes = /(0[1-9]|1[0-2])/;

    /*Valida o Regex*/
    isMesValid = regExpMes.test(mes);

    /*Valida se CSS já está aplicado*/
    var classMesAplicada = $("#campo_mes_validade").hasClass("has-error");

    /*Validando campo Mes*/
    if (!isMesValid && !classMesAplicada) {
        $("#campo_mes_validade").toggleClass("has-error");
        args.IsValid = false;
        return;
    }

    /*Caso exista algum falho exiba a mensagem de erro*/
    if (!isMesValid) {
        args.IsValid = false;
        return;
    }

    /*Validação caso a data seja menor que a atual*/
    var ano = parseInt($("#txtAnoValidade").val());

    var dataVencimento = new Date(2000 + ano, mes, 1, 0, 0, 0, 1);

    if (dataVencimento < new Date()) {
        args.IsValid = false;

        if (dataVencimento.getMonth() < new Date().getMonth() && !classMesAplicada)
            $("#campo_mes_validade").toggleClass("has-error");

        return;
    }

    /*Validações Ok*/
    if (classMesAplicada)
        $("#campo_mes_validade").toggleClass("has-error");
    args.IsValid = true;


}

function valida_range_ano(val, args) {

    var ano = parseInt($("#txtAnoValidade").val());

    if (isNaN(ano)) {
        args.IsValid = true;
        return;
    }

    var isAnoValid = false;
    var regExpAno = /(1[6-9]|2[0-9])/;

    /*Valida o Regex*/
    isAnoValid = regExpAno.test(ano);

    /*Valida se CSS já está aplicado*/
    var classAnoAplicada = $("#campo_ano_validade").hasClass("has-error");

    /*Validando campo Ano*/
    if (!isAnoValid && !classAnoAplicada) {
        $("#campo_ano_validade").toggleClass("has-error");
    }

    /*Caso exista algum falho exiba a mensagem de erro*/
    if (!isAnoValid) {
        args.IsValid = false;
        return;
    }

    if (classAnoAplicada)
        $("#campo_ano_validade").toggleClass("has-error");
    args.IsValid = true;
}

/*Fim Validations*/
/*-------------------------------------------------------------------------------*/

// Initialize the tooltip.
// When the copy button is clicked, select the value of the text box, attempt
// to execute the copy command, and trigger event to update tooltip message
// to indicate whether the text was successfully copied.

//$('#btnCopiar').bind('click', function () {
//    var input = document.querySelector('#copyTxtArea');
//    input.select();
//    try {
//        var success = document.execCommand('copy');
//        if (success) {
//            $('#btnCopiar').attr('value', 'Copiado!');
//        }
//    } catch (err) {
//        window.location = "http://www.bne.com.br/PaymentMobileErro.aspx";
//    }
//});
