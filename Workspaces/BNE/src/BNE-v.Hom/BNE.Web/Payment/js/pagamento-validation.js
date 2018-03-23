
/*Validations CARTAO DE CREDITO*/
/*-------------------------------------------------------------------------------*/
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

        var classcartaoAplicada = $("#campo_cartao_credito").hasClass("has-error");

        if (!isValid && !classcartaoAplicada) $("#campo_cartao_credito").toggleClass("has-error");
    }

    /*Valida a Bandeira do Cartão*/
    var bandeira = cardNumbersOnly.substring(0, 1);

    if (bandeira == 5) {
        $("#visa").hide();
        $("#mastercard").show();
    }

    else if (bandeira == 4) {
        $("#mastercard").hide();
        $("#visa").show();
    }
    else {
        $("#visa").show();
        $("#mastercard").show();
    }

    args.IsValid = isValid;
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

/*Fim Validations CARTAO DE CREDITO*/
/*-------------------------------------------------------------------------------*/


/*Validations DEBITO HSBC*/
/*-------------------------------------------------------------------------------*/

function verifica_cpf_cnpj(valor) {

    // Garante que o valor é uma string
    valor = valor.toString();

    // Remove caracteres inválidos do valor
    valor = valor.replace(/[^0-9]/g, '');

    // Verifica CPF
    if (valor.length === 11) {
        return 'CPF';
    }

        // Verifica CNPJ
    else if (valor.length === 14) {
        return 'CNPJ';
    }

        // Não retorna nada
    else {
        return false;
    }

} // verifica_cpf_cnpj

function validar_cnpj_ou_cpf() {

    /*Valida se CSS já está aplicado*/
    var classAnoAplicada = $("#campo_cpf_cnpj").hasClass("has-error");
    var cpf_cnpj = $('#txtCPFouCNPJHSBC').val();

    var valido = valida_cpf_cnpj(cpf_cnpj);
    if (!valido &&!classAnoAplicada) 
            $("#campo_cpf_cnpj").toggleClass("has-error");
    else {
        if (valido && classAnoAplicada)
            $("#campo_cpf_cnpj").toggleClass("has-error");
    }
};

/*
 calc_digitos_posicoes
 
 Multiplica dígitos vezes posições
 
 @param string digitos Os digitos desejados
 @param string posicoes A posição que vai iniciar a regressão
 @param string soma_digitos A soma das multiplicações entre posições e dígitos
 @return string Os dígitos enviados concatenados com o último dígito
*/
function calc_digitos_posicoes(digitos, posicoes, soma_digitos) {

    // Garante que o valor é uma string
    digitos = digitos.toString();

    // Faz a soma dos dígitos com a posição
    // Ex. para 10 posições:
    //   0    2    5    4    6    2    8    8   4
    // x10   x9   x8   x7   x6   x5   x4   x3  x2
    //   0 + 18 + 40 + 28 + 36 + 10 + 32 + 24 + 8 = 196
    for (var i = 0; i < digitos.length; i++) {
        // Preenche a soma com o dígito vezes a posição
        soma_digitos = soma_digitos + (digitos[i] * posicoes);

        // Subtrai 1 da posição
        posicoes--;

        // Parte específica para CNPJ
        // Ex.: 5-4-3-2-9-8-7-6-5-4-3-2
        if (posicoes < 2) {
            // Retorno a posição para 9
            posicoes = 9;
        }
    }

    // Captura o resto da divisão entre soma_digitos dividido por 11
    // Ex.: 196 % 11 = 9
    soma_digitos = soma_digitos % 11;

    // Verifica se soma_digitos é menor que 2
    if (soma_digitos < 2) {
        // soma_digitos agora será zero
        soma_digitos = 0;
    } else {
        // Se for maior que 2, o resultado é 11 menos soma_digitos
        // Ex.: 11 - 9 = 2
        // Nosso dígito procurado é 2
        soma_digitos = 11 - soma_digitos;
    }

    // Concatena mais um dígito aos primeiro nove dígitos
    // Ex.: 025462884 + 2 = 0254628842
    var cpf = digitos + soma_digitos;

    // Retorna
    return cpf;

} // calc_digitos_posicoes

/*
 Valida CPF
 
 Valida se for CPF
 
 @param  string cpf O CPF com ou sem pontos e traço
 @return bool True para CPF correto - False para CPF incorreto
*/
function valida_cpf(valor) {

    // Garante que o valor é uma string
    valor = valor.toString();

    // Remove caracteres inválidos do valor
    valor = valor.replace(/[^0-9]/g, '');

    // Captura os 9 primeiros dígitos do CPF
    // Ex.: 02546288423 = 025462884
    var digitos = valor.substr(0, 9);

    // Faz o cálculo dos 9 primeiros dígitos do CPF para obter o primeiro dígito
    var novo_cpf = calc_digitos_posicoes(digitos, 10, 0);

    // Faz o cálculo dos 10 dígitos do CPF para obter o último dígito
    var novo_cpf = calc_digitos_posicoes(novo_cpf, 11, 0);

    // Verifica se o novo CPF gerado é idêntico ao CPF enviado
    if (novo_cpf === valor) {
        // CPF válido
        return true;
    } else {
        // CPF inválido
        return false;
    }

} // valida_cpf

/*
 valida_cnpj
 
 Valida se for um CNPJ
 
 @param string cnpj
 @return bool true para CNPJ correto
*/
function valida_cnpj(valor) {

    // Garante que o valor é uma string
    valor = valor.toString();

    // Remove caracteres inválidos do valor
    valor = valor.replace(/[^0-9]/g, '');


    // O valor original
    var cnpj_original = valor;

    // Captura os primeiros 12 números do CNPJ
    var primeiros_numeros_cnpj = valor.substr(0, 12);

    // Faz o primeiro cálculo
    var primeiro_calculo = calc_digitos_posicoes(primeiros_numeros_cnpj, 5, 0);

    // O segundo cálculo é a mesma coisa do primeiro, porém, começa na posição 6
    var segundo_calculo = calc_digitos_posicoes(primeiro_calculo, 6, 0);

    // Concatena o segundo dígito ao CNPJ
    var cnpj = segundo_calculo;

    // Verifica se o CNPJ gerado é idêntico ao enviado
    if (cnpj === cnpj_original) {
        return true;
    }

    // Retorna falso por padrão
    return false;

} // valida_cnpj

/*
 valida_cpf_cnpj
 
 Valida o CPF ou CNPJ
 
 @access public
 @return bool true para válido, false para inválido
*/
function valida_cpf_cnpj(valor) {

    // Verifica se é CPF ou CNPJ
    var valida = verifica_cpf_cnpj(valor);

    // Garante que o valor é uma string
    valor = valor.toString();

    // Remove caracteres inválidos do valor
    valor = valor.replace(/[^0-9]/g, '');


    // Valida CPF
    if (valida === 'CPF') {
        // Retorna true para cpf válido
        return valida_cpf(valor);
    }

        // Valida CNPJ
    else if (valida === 'CNPJ') {
        // Retorna true para CNPJ válido
        return valida_cnpj(valor);
    }

        // Não retorna nada
    else {
        return false;
    }

} // valida_cpf_cnpj

/*Fim Validations DEBITO HSBC*/
/*-------------------------------------------------------------------------------*/


