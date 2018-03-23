
$(document).ready(function () {
    inicializa_debito_HSBC();
});

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

//function destacaValor() {
//    $('.painel_valor').hide().fadeIn('slow');
//}

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

//function valida_cartao(val, args) {
//    var isValid = false;
//    var ccCheckRegExp = /[^0-9 ]/;
//    isValid = !ccCheckRegExp.test(args.Value);
//    if (isValid) {
//        var cardNumbersOnly = args.Value.replace(/ /g, "");
//        var cardNumberLength = cardNumbersOnly.length;
//        var lengthIsValid = (cardNumberLength == 16 || cardNumberLength == 13);
//        isValid = lengthIsValid;
//    }
//    if (isValid) {
//        var numberProduct;
//        var numberProductDigitIndex;
//        var checkSumTotal = 0;
//        for (digitCounter = cardNumberLength - 1; digitCounter >= 0; digitCounter--) {
//            checkSumTotal += parseInt(cardNumbersOnly.charAt(digitCounter));
//            digitCounter--;
//            numberProduct = String((cardNumbersOnly.charAt(digitCounter) * 2));
//            for (var productDigitCounter = 0; productDigitCounter < numberProduct.length; productDigitCounter++) {
//                checkSumTotal += parseInt(numberProduct.charAt(productDigitCounter));
//            }
//        }
//        isValid = (checkSumTotal % 10 == 0);
//    }

//    args.IsValid = isValid;
//}

//function valida_vencimento_anterior(val, args) {
//    var mes = parseInt($("#cphConteudo_ddlMesVencimento").val());
//    var ano = parseInt($("#cphConteudo_ddlAnoVencimento").val());

//    if ((val.id == "cphConteudo_CustomValidatorAnoVencimento" && $("#cphConteudo_CustomValidatorMesVencimento").is(":visible"))
//        || isNaN(mes) || isNaN(ano)) {
//        args.IsValid = true;
//        return;
//    }

//    var dataVencimento = new Date(2000 + ano, mes, 1, 0, 0, 0, 1);

//    if (dataVencimento < new Date()) {
//        args.IsValid = false;
//        $("#cphConteudo_CustomValidatorVencimento").hide();
//        return;
//    }

//    $("#cphConteudo_CustomValidatorAnoVencimento").hide();
//    $("#cphConteudo_CustomValidatorMesVencimento").hide();
//    args.IsValid = true;
//}

//function valida_vencimento(val, args) {
//    var mes = parseInt($("#cphConteudo_ddlMesVencimento").val());
//    var ano = parseInt($("#cphConteudo_ddlAnoVencimento").val());

//    if (!$("#cphConteudo_CustomValidatorMesVencimento").is(":visible") &&
//        !$("#cphConteudo_CustomValidatorAnoVencimento").is(":visible") &&
//        (isNaN(mes) || isNaN(ano))) {
//        args.IsValid = false;
//        return;
//    }

//    args.IsValid = true;
//}

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
    //if (parseInt($("#cphConteudo_txtAgenciaDebitoHSBC").val()) <= 0 ||
    //    parseInt($("#cphConteudo_txtContaCorrenteDebitoHSBC").val()) <= 0) {
    //    args.IsValid = false;
    //    return;
    //}
    //var dvOriginal = $("#cphConteudo_txtDigitoDebitoHSBC").val();
    //var numeroConta = $("#cphConteudo_txtAgenciaDebitoHSBC").val() + $("#cphConteudo_txtContaCorrenteDebitoHSBC").val() + dvOriginal[0];
    //dvOriginal = parseInt(dvOriginal[1]);

    //var peso = 8;
    //var somatoria = 0;

    //for (var i = 0; i < numeroConta.length; i++) {
    //    somatoria += peso * parseInt(numeroConta[i]);
    //    peso++;
    //    if (peso > 9) {
    //        peso = 2;
    //    }
    //}

    //var dvCalculado = somatoria % 11;
    //if (dvCalculado == 10 || dvCalculado == 0) {
    //    dvCalculado = 0;
    //}

    //if (dvCalculado == dvOriginal) {
    //    args.IsValid = true;
    //    return;
    //}

    //args.IsValid = false;
}