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
    $(".cpf_debito_hsbc").attr("placeholder", "CPF do Titular da Conta");

    $(".ag").mask("9999", { placeholder: "_" });
    $(".ag").focusout(function () { $(".ag").val(numericoParaTamanhoExato($(".ag").val(), 4)); });

    $(".conta_corrente").mask("99999", { placeholder: "_" });
    $(".conta_corrente").focusout(function () { $(".conta_corrente").val(numericoParaTamanhoExato($(".conta_corrente").val(), 5)); });

    $(".digito_conta").mask("99", { placeholder: "_" });
    $(".digito_conta").focusout(function () { $(".digito_conta").val(numericoParaTamanhoExato($(".digito_conta").val(), 2)); });
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
        var lengthIsValid = (cardNumberLength == 16 || cardNumberLength == 13);
        var prefixIsValid = false;
        var prefixRegExp = /(^5[1-5])|^4/;
        
        prefixIsValid = prefixRegExp.test(cardNumbersOnly);
        isValid = prefixIsValid && lengthIsValid;
    }
    if (isValid) {
        var numberProduct;
        var numberProductDigitIndex;
        var checkSumTotal = 0;
        for (digitCounter = cardNumberLength - 1; digitCounter >= 0; digitCounter--) {
            checkSumTotal += parseInt(cardNumbersOnly.charAt(digitCounter));
            digitCounter--;
            numberProduct = String((cardNumbersOnly.charAt(digitCounter) * 2));
            for (var productDigitCounter = 0; productDigitCounter < numberProduct.length; productDigitCounter++) {
                checkSumTotal += parseInt(numberProduct.charAt(productDigitCounter));
            }
        }
        isValid = (checkSumTotal % 10 == 0);
    }

    args.IsValid = isValid;
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
    if ($(".ag").val() == "" ||
        $(".conta_corrente").val() == "" ||
        $(".digito_conta").val() == "") {
        args.IsValid = false;
        return;
    }
    args.IsValid = true;
}

function valida_conta_corrente_hsbc(val, args) {
    if ($(".ag").val() == "" ||
        $(".conta_corrente").val() == "" ||
        $(".digito_conta").val() == "") {
        args.IsValid = true;
        return;
    }
    if (parseInt($(".ag").val()) <= 0||
        parseInt($(".conta_corrente").val()) <= 0) {
        args.IsValid = false;
        return;
    }
    var dvOriginal = $(".digito_conta").val();
    var numeroConta = $(".ag").val()+$(".conta_corrente").val() + dvOriginal[0];
    dvOriginal = parseInt(dvOriginal[1]);

    var peso = 8;
    var somatoria = 0;

    for (var i = 0; i < numeroConta.length; i++) {
        somatoria += peso * parseInt(numeroConta[i]);
        peso++;
        if (peso > 9) {
            peso = 2;
        }
    }

    var dvCalculado = somatoria % 11;
    if (dvCalculado == 10 || dvCalculado == 0) {
        dvCalculado = 0;
    }

    if (dvCalculado == dvOriginal) {
        args.IsValid = true;
        return;
    }

    args.IsValid = false;
}