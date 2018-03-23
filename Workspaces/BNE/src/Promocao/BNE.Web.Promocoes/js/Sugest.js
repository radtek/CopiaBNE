$(document).ready(function () {
    autoCompletecidade();
    autoCompletefuncao();
    validar();

    $("#celular").on('keypress', function () {
        somenteNumeros($('#celular'));
    });

    $("#celular").blur(function () {
        valCelular();
        mascaraCelular($('#celular'));
    });

    $("#pretensao").on('keypress',function () {
        somenteNumeros($('#pretensao'));
    });

    $('input[name=sexo]').change(function () {
        validarSexo();
    });

    $("#pretensao").on('focus', function () {
        $('#pretensao').val('');
    });

    $("#pretensao").blur(function () {
        valPretensaoMaiorqueSalarioMinimo();
        mascaraPretensao($('#pretensao'));
    });
    

    var originalPageTitle = document.title;

    window.addEventListener('blur', function () {
        document.title = 'Não pera, volta aqui =/';
    });

    window.addEventListener('focus', function () {
        document.title = originalPageTitle;
    });
});


function validar() {
    $("#funcao").blur(function () {
        validarFuncao();
    });

    $("#funcao").autocomplete({
        select: function (a, b) {
            setTimeout(function () {
                validarFuncao();
            }, 500);
        }
    })

    $("#cidade").blur(function () {
        validarCidade();
    });

    $("#cidade").autocomplete({
        select: function (a, b) {
            setTimeout(function () {
                validarCidade();
            }, 500);
        }
    })

}

function valNome() {
    if ($("#nome").val().length > 2) {
        $("#avisoNome").css('display', 'none');
    }
    else {
        $('#avisoNome').html('Campo Obrigatório.').addClass('aviso');
        $("#avisoNome").css('display', 'block');
    }
}

function ConfigurarCampos() {
    bne.components.web.CPF('txtCPF', true);
    bne.components.web.data('txtDataNascimento', true, bne.components.web.data.type.dataNascimento);
}

function valCPF() {
    var value = $("#txtCPF").val();

    var digitosIguais, cpf = value.replace(/\D+/g, '');

    digitosIguais = 1;

    if (cpf.length !== 11) {
        $('#avisoCPF').html('CPF Inválido.').addClass('aviso');
        $("#avisoCPF").css('display', 'block');
        return;
    }
    var i;
    for (i = 0; i < cpf.length - 1; i++) {
        if (cpf.charAt(i) !== cpf.charAt(i + 1)) {
            digitosIguais = 0;
            break;
        }
    }

    if (!digitosIguais) {
        var add = 0;
        for (i = 0; i < 9; i++)
            add += parseInt(cpf.charAt(i)) * (10 - i);

        var rev = 11 - (add % 11);
        if (rev === 10 || rev === 11)
            rev = 0;

        if (rev !== parseInt(cpf.charAt(9))) {
            $('#avisoCPF').html('CPF Inválido.').addClass('aviso');
            $("#avisoCPF").css('display', 'block');
            return;
        }

        add = 0;
        for (i = 0; i < 10; i++)
            add += parseInt(cpf.charAt(i)) * (11 - i);
        rev = 11 - (add % 11);
        if (rev === 10 || rev === 11)
            rev = 0;

        if (rev !== parseInt(cpf.charAt(10))) {
            $('#avisoCPF').html('CPF Inválido.').addClass('aviso');
            $("#avisoCPF").css('display', 'block');
            return;
        }
        $("#avisoCPF").css('display', 'none');
    }
    else {
        $('#avisoCPF').html('CPF Inválido.').addClass('aviso');
        $("#avisoCPF").css('display', 'block');
        return;
    }
}

function CPFMask(control) {
    var valor = $("#txtCPF").val();
    if (valor === '')
        return;

    if (valor.length > 14) {
        $("#txtCPF").val(valor.substring(0, 14));
        return;
    }

    if (valor.match("\\d{11}")) {
        valor = valor.replace(/(\d{3})(\d)/, "$1.$2");
        valor = valor.replace(/(\d{3})(\d)/, "$1.$2");
        valor = valor.replace(/(\d{3})(\d)/, "$1-$2");
    }
    $("#txtCPF").val(valor);
}

function valDataNascimento() {
    if (validarData() && validarIdadeMinima($("#txtDataNascimento").val())) {
        $("#avisoDataNascimento").css('display', 'none');
    } else {
        $('#avisoDataNascimento').html('Data de nascimento Inválido.').addClass('aviso');
        $("#avisoDataNascimento").css('display', 'block');
    }
}

function dataMask() {
    var valor = $('#txtDataNascimento').val()
    if (valor === '')
        return;

    if (valor.length > 10) {
        $("#txtDataNascimento").val(valor.substring(0, 10));
        return;
    }

    if (valor.match(/^(\d{6})$/)) {
        valor = valor.replace(/(\d{2})(\d{2})(\d{2})/, "$1/$2/" + retornarAno(valor.replace(/(\d{2})(\d{2})(\d{2})/, "$3")));
    }
    else if (valor.match(/^(\d{8})$/)) {
        valor = valor.replace(/(\d{2})(\d{2})(\d{4})/, "$1/$2/$3");
    }

    $('#txtDataNascimento').val(valor);
}

function valEmail() {

    var filtro = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i
    if (filtro.test($("#email").val()))
        $("#avisoEmail").css('display', 'none');
    else {
        $('#avisoEmail').html('Email Inválido.').addClass('aviso');
        $("#avisoEmail").css('display', 'block');
    }
}

function validarCidade() {
    $("#cidade").parent().find(".spinner").hide();
    $.ajax({
        type: "GET",
        url: "bh/Cidade/Val?desc=" + encodeURIComponent($("#cidade").val()),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            if (msg == "True") {
                $("#idCidade").val(msg);
                $("#avisoCidade").css('display', 'none');
            }
            else {
                $("#idCidade").val(msg);
                $('#avisoCidade').html('Cidade Inválida.').addClass('aviso');
                $('#avisoCidade').css('display', 'block');
            }
        },
        error: function (data) {
            console.log(data.responseText);

        }
    });
}

function validarFuncao() {
    $("#funcao").parent().find(".spinner").hide();
    $.ajax({
        type: "GET",
        url: "bh/funcao/Val?desc=" + encodeURIComponent($("#funcao").val()),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            if (msg == "True") {
                $("#idFuncao").val(msg);
                $('#avisoFuncao').css('display', 'none');
            }
            else {
                $("#idFuncao").val(msg);
                $('#avisoFuncao').html('Função Inválida.').addClass('aviso');
                $('#avisoFuncao').css('display', 'block');

            }
        },
        error: function (data) {
            console.log(data.responseText);

        }
    });
}

function autoCompletecidade() {

    var cachecidades = {};

    $("#cidade").autocomplete({
        minLength: 2,
        delay: 100,
        open: function (event, ui) {
            var ul = $(".ui-autocomplete");
            ul.outerWidth($("#cidade").outerWidth());
        },
        change: function (event, ui) {
            //if (ui.item == null) {
            //    $("#hfcidadeVaga").val(0);
            //} else {
            //    $("#hfcidadeVaga").val(ui.item.id);

            //}
        },

        source: function (request, response) {
            var term = request.term;
            if (term in cachecidades) {
                response(cachecidades[term]);
                return;
            }

            //  ajustarLoading($("#cidade"));

            $("#cidade").parent().find(".spinner").show();

            $.ajax({
                type: "GET",
                url: "bh/cidade/List?desc=" + encodeURIComponent($("#cidade").val()),
                contentType: "application/json; charset=utf-8",
                dataType: "json",

                success: function (data) {
                    // console.log(data);
                    cachecidades[term] = data;
                    for (var i in cachecidades[term]) {
                        cachecidades[term][i].value = cachecidades[term][i].descricao;
                    }
                    console.log(cachecidades[term]);
                    response(cachecidades[term]);
                    $("#cidade").parent().find(".spinner").hide();
                },
            });
        }
    }).autocomplete("instance")._renderItem = function (ul, item) {
        console.log(item); console.log(ul);
        return $("<li>")
              .text(item.value)
              .appendTo(ul);
    };
};

function autoCompletefuncao() {

    var cachefuncao = {};

    $("#funcao").autocomplete({
        minLength: 2,
        delay: 100,
        open: function (event, ui) {
            var ul = $(".ui-autocomplete");
            ul.outerWidth($("#funcao").outerWidth());
        },
        change: function (event, ui) {
            //if (ui.item == null) {
            //    $("#hfcidadeVaga").val(0);
            //} else {
            //    $("#hfcidadeVaga").val(ui.item.id);

            //}
        },

        source: function (request, response) {
            var term = request.term;
            if (term in cachefuncao) {
                response(cachefuncao[term]);
                return;
            }

            // ajustarLoading($("#funcao"));

            $("#funcao").parent().find(".spinner").show();
            console.log($("#funcao").val());
            $.ajax({
                type: "GET",
                url: "bh/funcao/List?desc=" + encodeURIComponent($("#funcao").val()),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    console.log(data);
                    cachefuncao[term] = data;
                    for (var i in cachefuncao[term]) {
                        cachefuncao[term][i].value = cachefuncao[term][i].descricao;
                    }
                    response(cachefuncao[term]);
                    $("#funcao").parent().find(".spinner").hide();
                },
            });
        }
    }).autocomplete("instance")._renderItem = function (ul, item) {

        return $("<li>")
              .text(item.descricao)
              .appendTo(ul);
    };
};

function ajustarLoading(elemento) {
    var top = +elemento.position().top + 10;
    elemento.parent().find(".spinner").css('top', top);
}

function modal() {
    swal({
        title: "Cadastro realizado com sucesso!",
        type: "success",
        confirmButtonClass: "plain button mint ",
        confirmButtonText: "Ir para o BNE"
    }, function () {
        location.href = "http://www.bne.com.br/vagas-de-emprego-em-belo-horizonte-mg/?utm_source=Hotsite-BH&utm_medium=hotsite&utm_campaign=IndicacaoBH&utm_term=btnSucesso"
    })
}

function modalErro() {
    swal({
        title: "Ocorreu um erro. Tente novamente mais tarde!",
        type: "error", confirmButtonClass: "plain button mint ",
        confirmButtonText: "OK"
    })
} setInterval(function () {  }, 1e3);

function validarCadastro() {
    var sexo = $("input:checked").val();
    validarCidade();
    validarFuncao();
    valEmail();
    valCPF();
    valDataNascimento();
    //valPretensaoMaiorqueSalarioMinimo();
    valCelular();
    validarSexo();
    valNome();
    if ($("#nome").val().length > 2
        && $("#email").val() != ""
        && $("#Cidade").val() != ""
        && $("#funcao").val() != ""
        && $("#pretensao").val() != ""
        && $("#celular").val() != ""
        && $("#idFuncao").val() != ""
        && $("#idCidade").val() != "") {
        document.getElementById("btnCadastrar").disabled = true;
        var cad = "{'nome':'" + $("#nome").val() +
                  "','email':'" + $("#email").val() +
                  "','cpf':'" + $("#txtCPF").val() +
                  "','dataNascimento':'" + $("#txtDataNascimento").val().split('/')[2] + "-" + $("#txtDataNascimento").val().split('/')[1] + "-" + $("#txtDataNascimento").val().split('/')[0] +
                  "','funcao':'" + encodeURIComponent($("#funcao").val()) +
                  "','pretensao':'" + encodeURIComponent($("#pretensao").val()) +
                  "','celular':'" + encodeURIComponent($("#celular").val()) +
                  "','sexo':'" + encodeURIComponent(sexo) +
                  "','cidade':'" + $("#cidade").val() + "'}";

        $.ajax({
            type: "POST",
            url: "bh/Home/cad?json=" + cad,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                if (msg == "True") {
                    modal();
                }
                else {
                    $("#idFuncao").val(msg);
                    document.getElementById("btnCadastrar").disabled = false;
                }
            },
            error: function (data) {
                modalErro();
                document.getElementById("btnCadastrar").disabled = false;

            }
        });

    }
    // else
    // modalErro();
}

function validarSexo() {
    var sexo = $("input:checked").val();
    
    if (sexo == null) {
        $('#avisoSexo').html('Selecione o sexo').addClass('aviso');
        $('#avisoSexo').css('display', 'block');
    } else {
        $('#avisoSexo').css('display', 'none');
    }
}

function validarIdadeMinima(idade) {
    value = idade.replace(/\//g, '');

    var dia = value.substring(0, 2);
    var mes = value.substring(2, 4);
    var ano = value.substring(4, 8);

    var dataNascimento = new Date(ano, mes - 1, dia);
    var hoje = new Date();
    var dataMinima = new Date(hoje.getFullYear() - 13, hoje.getMonth() - 1, hoje.getDay());

    if (dataNascimento > dataMinima)
        return false;
    else
        return true;
}

function validarData() {
    value = $('#txtDataNascimento').val().replace('.', '').replace(/\//g, '');

    var valido = true;

    var dia = value.substring(0, 2);
    var mes = value.substring(2, 4);
    var ano = value.substring(4, 8);

    if (value.match(/^(\d{6})$/)) {
        ano = retornarAno(value.replace(/(\d{2})(\d{2})(\d{2})/, "$3"));
    }

    var novaData = new Date(ano, (mes - 1), dia);

    var mesmoDia = parseInt(dia, 10) === parseInt(novaData.getDate());
    var mesmoMes = parseInt(mes, 10) === parseInt(novaData.getMonth()) + 1;
    var mesmoAno = parseInt(ano) === parseInt(novaData.getFullYear());

    if (!((mesmoDia) && (mesmoMes) && (mesmoAno)))
        valido = false;

    return valido;
}

function retornarAno(ano) {
    if (ano != null) {
        if (ano.toString().length < 4) {
            if (ano >= 30 && ano <= 99)
                ano = "1900".substring(0, 4 - ano.length) + ano;
            else
                ano = "2000".substring(0, 4 - ano.length) + ano;
        }
        else
            ano = ano.substring(0, 4);
    }
    return ano;
}

function valCelular() {

    var celular = $('#celular').val().replace('(','').replace(')','').replace('-','');
    var validado = true;
    var numeroCelular = celular.substring(2, celular.length);

    if (numeroCelular.length < 8) {
        validado = false;
        //return validado;
    }

    var totalCaracteres = numeroCelular.length;

    if (totalCaracteres > 9) { validado = false; }

    if (totalCaracteres <= 7) {
        validado = false;
    }
    if (totalCaracteres <= 6) {
        validado = false;
    }

    
        //numeroCelular = numeroCelular.substring(0, numeroCelular.length - 1);

        //if (totalCaracteres - 1 == 7) {
        //    validado = false;
        //}

        if (totalCaracteres == 8 && numeroCelular <= 4999999) {
            validado = false;
        }
        if (totalCaracteres == 9 && numeroCelular <= 49999999) {
            validado = false;
        }
        if (totalCaracteres == 10 && numeroCelular <= 499999999) {
            validado = false;
        }

    if (!validado) {
        $('#avisoCelular').html('Celular inválido.').addClass('aviso');
        $('#avisoCelular').css('display', 'block');
    } else {
        $('#avisoCelular').css('display', 'none');
    }

    return validado;
}

function valPretensaoMaiorqueSalarioMinimo() {
    var pretensao = $('#pretensao').val();
    var vlrMinimo = '880'
    var validado = false;
    var pretensaoSalarial = pretensao;

    var valordoMinimo = vlrMinimo + '.00';
    if (parseFloat(pretensaoSalarial) >= parseFloat(valordoMinimo)) {
        validado = true;
    }

    if (!validado) {
        $('#avisoPretensao').html('A sua pretensão deve ser igual ou maior que R$ '+ valordoMinimo).addClass('aviso');
        $('#avisoPretensao').css('display', 'block');
    } else {
        $('#avisoPretensao').css('display', 'none');
    }
    return validado;
}

function mascaraPretensao(control) {
    var valor = control.val().replace(/^\s+|\s+$/g, '');

    if (valor === '')
        return '';

    valor = valor.replace(/\ /g, '');

    somenteNumeros(control);

    valor = control.val();

    if (valor.match(/[^0-9]*/g)) {
        if (valor.length >= 8) {

            valor1 = valor.replace(/^\$?([0-9]{1,3}([0-9]{3})*[0-9]{3}|[0-9]+)([0-9][0-9])?$/, "$1");
            valor2 = valor.replace(/^\$?([0-9]{1,3}([0-9]{3})*[0-9]{3}|[0-9]+)([0-9][0-9])?$/, "$3")
            valor = valor1.replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.") + ',' + valor2;
        }
        else {
            valor = valor.replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
            //valor = valor + ',00';
        }
    }

    control.val(valor);
};

function mascaraCelular(control) {
    var valor = control.val().replace(/^\s+|\s+$/g, '');
    if (valor === '')
        return;

    valor = valor.replace(/\ /g, '');

    if (valor.match(/^(\d{10})$/)) {
        valor = valor.replace(/(\d{2})(\d{4})(\d{4})/, "($1) $2-$3");
    }
    else if (valor.match(/^(\d{11})$/)) {
        valor = valor.replace(/(\d{2})(\d{5})(\d{4})/, "($1) $2-$3");
    }
    else if (valor.match(/^(\d{11})$/) && valor.substring(0, 1) === "0") {
        valor = valor.replace(/(\d{4})(\d{3})(\d{4})/, "$1-$2-$3");
    }
    control.val(valor);
}

function somenteNumeros(control) {
    var expre = /[^0-9]/g;
    var ret;

    if (control.val().match(expre)) {
        ret = control.val().replace(expre, '')
    }

    if (ret == null)
        control.val(control.val())
    else
        control.val(ret);
}

function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}