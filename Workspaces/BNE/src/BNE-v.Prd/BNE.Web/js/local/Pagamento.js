var valorDigitado2;

$(document).ready(function () {
    inicializa_cartao_credito();
    inicializa_debito_HSBC();
 });

function inicializa_cartao_credito() {
    $("#cphConteudo_txtNumeroCartao_txtValor").mask("9999 9999 9999 9999", { placeholder: "_" });
    $("#cphConteudo_txtNumeroCartao_txtValor").attr("placeholder", "Número do Cartão");
    $("#cphConteudo_txtCodigoVerificadorCartao_txtValor").mask("999", { placeholder: "_" });
    $("#cphConteudo_txtCodigoVerificadorCartao_txtValor").attr("placeholder", "CV");
}

function inicializa_debito_HSBC() {
    $("#cphConteudo_txtCPFDebitoHSBC_txtValor").attr("placeholder", "CPF do Titular da Conta");
    $("#cphConteudo_txtCNPJDebitoHSBC_txtValor").attr("placeholder", "CNPJ do Titular da Conta");

    $("#cphConteudo_txtAgenciaDebitoHSBC").mask("9999", { placeholder: "_" });
    $("#cphConteudo_txtAgenciaDebitoHSBC").focusout(function () { $("#cphConteudo_txtAgenciaDebitoHSBC").val(numericoParaTamanhoExato($("#cphConteudo_txtAgenciaDebitoHSBC").val(), 4)); });

    $("#cphConteudo_txtContaCorrenteDebitoHSBC").mask("99999", { placeholder: "_" });
    $("#cphConteudo_txtContaCorrenteDebitoHSBC").focusout(function () { $("#cphConteudo_txtContaCorrenteDebitoHSBC").val(numericoParaTamanhoExato($("#cphConteudo_txtContaCorrenteDebitoHSBC").val(), 5)); });

    $("#cphConteudo_txtDigitoDebitoHSBC").mask("99", { placeholder: "_" });
    $("#cphConteudo_txtDigitoDebitoHSBC").focusout(function () { $("#cphConteudo_txtDigitoDebitoHSBC").val(numericoParaTamanhoExato($("#cphConteudo_txtDigitoDebitoHSBC").val(), 2)); });
}

function destacaValor() {
    $('.painel_valor').hide().fadeIn('slow');
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


function valida_cartao(val, args) {

    var isValid = false;
    var ccCheckRegExp = /[^0-9 ]/;
    isValid = !ccCheckRegExp.test(args.Value);

  
    if (isValid) {
        var cardNumbersOnly = args.Value.replace(/ /g, "");
        var cardNumberLength = cardNumbersOnly.length;
        var lengthIsValid = (cardNumberLength == 16 || cardNumberLength == 13 || cardNumberLength == 15 || cardNumberLength == 14);
        var prefixIsValid = false;
        var prefixRegExp = /^6504|^6550|^5([1-5])|^3[47]|^4|^38[0-9]|^60[0-9]|^30[0-5]|^(30[6-9]|36|38)|^(63|438935|504175|451416|5067|4576|4011|506699)|^(4026|417500|4508|4844|491(3|7))|^(50)|^(6011|622(12[6-9]|1[3-9][0-9]|[2-8][0-9]{2}|9[0-1][0-9]|92[0-5]|64[4-9])|65)) /;

        prefixIsValid = prefixRegExp.test(cardNumbersOnly);
        isValid = prefixIsValid && lengthIsValid;
    }
    //if (isValid) {
    //    var numberProduct;
    //    var numberProductDigitIndex;
    //    var checkSumTotal = 0;
    //    for (digitCounter = cardNumberLength - 1; digitCounter >= 0; digitCounter--) {
    //        checkSumTotal += parseInt(cardNumbersOnly.charAt(digitCounter));
    //        digitCounter--;
    //        numberProduct = String((cardNumbersOnly.charAt(digitCounter) * 2));
    //        for (var productDigitCounter = 0; productDigitCounter < numberProduct.length; productDigitCounter++) {
    //            checkSumTotal += parseInt(numberProduct.charAt(productDigitCounter));
    //        }
    //    }
    //    isValid = (checkSumTotal % 10);
    //}
    //console.log(checkSumTotal % 10 == 0);
  
    args.IsValid = isValid;
}

function valida_bandeira(val, args) {
    if ($("#selBandeira").val() == "undefined") {
        args.IsValid = true;
        return;
    }
    return args.IsValid = false;
   
}


function valida_vencimento_anterior(val, args) {
    var mes = parseInt($("#cphConteudo_ddlMesVencimento").val());
    var ano = parseInt($("#cphConteudo_ddlAnoVencimento").val());

    if ((val.id == "cphConteudo_CustomValidatorAnoVencimento" && $("#cphConteudo_CustomValidatorMesVencimento").is(":visible"))
        || isNaN(mes) || isNaN(ano)) {
        args.IsValid = true;
        return;
    }

    var dataVencimento = new Date(2000 + ano, mes, 1, 0, 0, 0, 1);

    if (dataVencimento < new Date()) {
        args.IsValid = false;
        $("#cphConteudo_CustomValidatorVencimento").hide();
        return;
    }

    $("#cphConteudo_CustomValidatorAnoVencimento").hide();
    $("#cphConteudo_CustomValidatorMesVencimento").hide();
    args.IsValid = true;
}

function valida_vencimento(val, args) {
    var mes = parseInt($("#cphConteudo_ddlMesVencimento").val());
    var ano = parseInt($("#cphConteudo_ddlAnoVencimento").val());

    if (!$("#cphConteudo_CustomValidatorMesVencimento").is(":visible") &&
        !$("#cphConteudo_CustomValidatorAnoVencimento").is(":visible") &&
        (isNaN(mes) || isNaN(ano))) {
        args.IsValid = false;
        return;
    }

    args.IsValid = true;
}

function valida_conta_corrente_hsbc_obrigatoria(val, args) {
    if ($("#cphConteudo_txtAgenciaDebitoHSBC").val() == "" ||
        $("#cphConteudo_txtContaCorrenteDebitoHSBC").val() == "" ||
        $("#cphConteudo_txtDigitoDebitoHSBC").val() == "") {
        args.IsValid = false;
        return;
    }
    args.IsValid = true;
}

function valida_agencia_hsbc_obrigatoria(val, args) {
    if ($("#cphConteudo_txtAgenciaDebitoHSBC").val() == "") {
        args.IsValid = false;
        return;
    }
    args.IsValid = true;
}

function valida_conta_corrente_hsbc(val, args) {
    if ($("#cphConteudo_txtAgenciaDebitoHSBC").val() == "" ||
        $("#cphConteudo_txtContaCorrenteDebitoHSBC").val() == "" ||
        $("#cphConteudo_txtDigitoDebitoHSBC").val() == "") {
        args.IsValid = false;
        return;
    }
}

function ValidaBandeira() {
    
    var number = $("#cphConteudo_txtNumeroCartao_txtValor").val().replace(/[^0-9]+/g, '');

    if (number.length == 0) {
        //$("#imageCartao").attr("src", "Payment/img/method-vmeha.png");
        return;
    }

    if (number.length > 4)
        return;


    if (re = new RegExp('^38[0-9]'), null != number.match(re)) {
        $("#imageCartao").attr("src", "Payment/img/method-hipercard.png")
        return;
    }

    if (re = new RegExp('^60[0-9]'), null != number.match(re)) {
        $("#imageCartao").attr("src", "Payment/img/method-hipercard.png")
        return;
    }

    if (re = new RegExp('^30[0-5]'), null != number.match(re)) {
        $("#imageCartao").attr("src", "Payment/img/method-dinners.png");
        return;
    }

    if (re = new RegExp('^(30[6-9]|36|38)'), null != number.match(re)) {
        $("#imageCartao").attr("src", "Payment/img/method-dinners.png");
        return;
    }

    if (re = new RegExp("^3[47]"), null != number.match(re)) {
        var valorDigitado = $("#cphConteudo_txtNumeroCartao_txtValor").val().replace(/[^\d]+/g, '');
        
        if (valorDigitado.length > 2)
        {
            return;
        }

        $("#imageCartao").attr("src", "Payment/img/method-amex.png");
        
        return;
    }

    if (re = new RegExp("^(63|438935|504175|451416|5067|4576|4011|506699|6504|6550)"), null != number.match(re)) {
        $("#imageCartao").attr("src", "Payment/img/method-elo.png");
        return;
    }

    if (re = new RegExp("^(4026|417500|4508|4844|491(3|7))"), null != number.match(re)) {
        $("#imageCartao").attr("src", "Payment/img/method-visa.png");
        return;
    }


    if (re = new RegExp("^(50)"), null != number.match(re)) {
        $("#imageCartao").attr("src", "Payment/img/method-aura.png");
        return;
    }

    var re = new RegExp("^4");

    if (null != number.match(re))
    {
        $("#imageCartao").attr("src", "Payment/img/method-visa.png");
        return;
    }

    if (re = new RegExp("^5[1-5]"), null != number.match(re))
    {
        $("#imageCartao").attr("src", "Payment/img/method-mastercard.png");
        return;
    }

    if (re = new RegExp("^(6011|622(12[6-9]|1[3-9][0-9]|[2-8][0-9]{2}|9[0-1][0-9]|92[0-5]|64[4-9])|65)"), null != number.match(re)) {
        $("#imageCartao").attr("src", "Payment/img/method - discover.png");
        return;
    }

    $("#imageCartao").attr("src", "Payment/img/method-all.png");
}

function AlteraBandeira()
{
    var bandeira = $("#selBandeira").val();
    if (bandeira == "amex")
    {
        $("#cphConteudo_txtNumeroCartao_txtValor").mask("9999 999999 99999", { placeholder: "_" });
        $("#cphConteudo_txtCodigoVerificadorCartao_txtValor").mask("9999", { placeholder: "_" });
        $("#imageCartao").attr("src", "Payment/img/method-amex.png");
        return;
    }

    if (bandeira == "dinners") {
        $("#cphConteudo_txtNumeroCartao_txtValor").mask("9999 9999 9999 99  ", { placeholder: "_" });
        $("#cphConteudo_txtCodigoVerificadorCartao_txtValor").mask("999", { placeholder: "_" });
        $("#imageCartao").attr("src", "Payment/img/method-dinners.png");
        return;
    }

    if (bandeira == "elo")
    {
        $("#imageCartao").attr("src", "Payment/img/method-elo.png");
    }
    if (bandeira == "visa")
    {
        $("#imageCartao").attr("src", "Payment/img/method-visa.png");
    }
    if (bandeira == "master")
    {
        $("#imageCartao").attr("src", "Payment/img/method-mastercard.png");
    }
    if (bandeira == "hipercard")
    {
        $("#imageCartao").attr("src", "Payment/img/method-hipercard.png");
    }
    if (bandeira == "aura")
    {
        $("#imageCartao").attr("src", "Payment/img/method-aura.png");
    }
    if (bandeira == "discover")
    {
        $("#imageCartao").attr("src", "Payment/img/method-discover.png");
    }

    inicializa_cartao_credito();
   
}





