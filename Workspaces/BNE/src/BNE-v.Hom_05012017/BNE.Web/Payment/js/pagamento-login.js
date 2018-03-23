/*---------------------------PAGE LOAD-------------------------------*/
var urlPost = "Payment.aspx/";
$(function () {
    exibirLogin(true);
});

$("#ModalLoginCPF").on('keydown', function (event) {

    if (event.keyCode === 13)
        ValidarLogin();

});

$("#ModalLoginDataDeNascimento").on('keydown', function (event) {

    if (event.keyCode === 13)
        ValidarLogin();

});

$('#loginModal').on('shown.bs.modal', function () {
    $("body.mobile-payment").css("overflow-y", "hidden");
});

/*---------------------------EVENTOS--------------------------------*/
function cadastroNovo() {
    //Envia a requisição
    $.ajax({
        type: "POST",
        url: urlPost+"CadastroMini",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            window.location = response.d.Url;
        }
    });
}

function exibirLogin(valor) {
    if (valor && empty($("#txtCurriculo").val()) && empty($("#txtPessoaFisica").val()) && empty($("#txtUsuarioFilialPerfil").val()))
        $("#loginModal").modal('show');
    else
        $("#loginModal").modal('hide');
}

function ValidarLogin() {

    var login = $("#ModalLoginCPF").val();
    var dtaNascimento = $("#ModalLoginDataDeNascimento").val();

    if (valida_cpf_login(login) && valida_dta_nascimento_login(dtaNascimento))
        EfetuarLogin(login, dtaNascimento);

}

function EfetuarLogin(login, dtaNascimento) {
    //Recupera os Dados
    var Dados = {};
    Dados.login = login;
    Dados.dataNascimento = dtaNascimento;

    //Envia a requisição
    $.ajax({
        type: "POST",
        url: urlPost + "ValidarLogin",
        data: JSON.stringify(Dados),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {

            /*Valida se CSS já está aplicado*/


            var result = response.d;

            if (result.Falha) {
                if (!empty(result.Url))
                    window.location = result.Url;
                else {
                    valida_dados_login(true, result.Mensagem);
                }
            } else {
                valida_dados_login(false, "");
                exibirLogin(false);
                location.reload();
            }
        }
    });
}
/*----------------------Validações Login----------------------------*/

function valida_cpf_login(value) {

    /*Valida se CSS já está aplicado*/
    var classAnoAplicada = $("#erroCpf").hasClass("has-error");

    var valido = valida_cpf(value);

    if ((!valido && !classAnoAplicada)) {
        $("#erroCpf").toggleClass("has-error");
        $("#msgErroCpf").html("CPF Inválido!");
    } else if ((valido && classAnoAplicada)) {
        $("#erroCpf").toggleClass("has-error");
        $("#msgErroCpf").html("");
    }
    valida_dados_login(false, "");
    return valido;

}

function valida_dta_nascimento_login(value) {
    /*Regex para validar data*/
    var ccCheckRegExp = /^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)(?:0?2)\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/;
    var valido = ccCheckRegExp.test(value);

    var classoAplicada = $("#erroDataNascimento").hasClass("has-error");

    if ((!valido && !classoAplicada)) {
        $("#erroDataNascimento").toggleClass("has-error");
        $("#msgErroDtaNasc").html("Data de Nascimento Inválida!");
    } else if ((valido && classoAplicada)) {
        $("#erroDataNascimento").toggleClass("has-error");
        $("#msgErroDtaNasc").html("");
    }
    valida_dados_login(false, "");
    return valido;
}

function valida_dados_login(value, mensagem) {

    var classLoginAplicada = $("#erroLogin").hasClass("has-error");

    if (value) {
        $("#msgErroLogin").html(mensagem);
        if (!classLoginAplicada)
            $("#erroLogin").toggleClass("has-error");
    } else {
        $("#msgErroLogin").html("");
        if (classLoginAplicada)
            $("#erroLogin").toggleClass("has-error");
    }
}


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


/*--------------------------------------------------------------*/
/*----------------------------------------------------------------------------------------------------------------------------------------------*/
var BNEFacebook = {};

BNEFacebook.EfetuarLogin = function () {
    FB.login(
        BNEFacebook.LoginFacebookCallback,
        { scope: 'email,user_about_me,user_birthday,user_location,user_work_history,user_education_history,user_relationships,user_likes,read_friendlists' }
    );
};

BNEFacebook.LoginFacebookCallback = function (response) {
    if (response.authResponse) {
        var objectMe;
        var objectFriends;
        var objectPicture;

        //Recuperando os dados do usuário
        FB.api('/me?locale=pt_BR', function (responseMe) {
            objectMe = responseMe;

            //Recuperando a lista de amigos
            FB.api('/me/friends', function (responseFriends) {
                objectFriends = responseFriends;

                //Recuperando a foto
                FB.api('/me/picture?width=180&height=180', function (responsePicture) {
                    objectPicture = responsePicture;
                    var Dados = {};
                    Dados.idFacebook = objectMe.id;
                    //Depois de pegar todas as informações necessárias, valida se o usuário existe.
                    $.ajax({
                        type: "POST",
                        url: urlPost + "ValidarFacebook",
                        data: JSON.stringify(Dados),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            var result = response.d;
                            if (!result.Falha) {
                                valida_dados_login(false, "");
                                exibirLogin(false);
                                location.reload();
                            } else {

                                var retorno = {};
                                retorno.jsonFacebook = JSON.stringify(objectMe);
                                retorno.jsonFriendsFacebook = JSON.stringify(objectFriends);
                                retorno.jsonMePicture = JSON.stringify(objectPicture);

                                $.ajax({
                                    type: "POST",
                                    url: urlPost + "ArmazenarDadosFacebook",
                                    data: JSON.stringify(retorno),
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (response) {
                                        var result = response.d;
                                        window.location = result.Url;
                                    }
                                });
                            }
                        }
                    });
                });
            });
        });
    }
};