var VariaveisGlobais = {
    MsgFaixaSalarial: '',
    idFuncao: null,
    siglaEstado: ''
}

function InicializarVariaveis(parametros) {
    VariaveisGlobais.MsgFaixaSalarial = parametros.msgFaixaSalarial;
}

function removerUltimaVirgula(str) {
    return str.replace(/,$/, "");
}

$(document).ready(function () {
    autocomplete.cidade("txtCidadeAnunciarVaga");
    RecuperarBairros();
    AjustarPretensaoSalarial();
});

function redirectToWebEstagios() {
    var href = 'webestagios.com.br';
    var link = $('<a href="http://' + href + '" />');
    link.attr('target', '_blank');
    window.open(link.attr('href'));
    return false;
}

function cvCidade_Validate(sender, args) {
    var res = AnunciarVaga.ValidarCidade(args.Value);
    args.IsValid = (res.error == null && res.value);
}

function RecuperarBairros() {
    var cidade = $('#cphConteudo_txtCidadeAnunciarVaga').val().split('/')[0];
    var estado = $('#cphConteudo_txtCidadeAnunciarVaga').val().split('/')[1];

    //$('#cphConteudo_txtBairroAnunciarVaga').val('');
    autocomplete.bairro("txtBairroAnunciarVaga", cidade, estado, null, 'true');
}

function Cidade_OnChange() {

    var value = employer.controles.recuperarValor('txtFuncaoAnunciarVaga');
    if (value == null) value = '';
    var secondValue = employer.controles.recuperarValor('txtCidadeAnunciarVaga');
    if (secondValue == null) secondValue = '';

    var mediaSalarial = AnunciarVaga.PesquisarMediaSalarial(value, secondValue);
    if (mediaSalarial.value != null && mediaSalarial.value != 0) {
        var txtFuncao = employer.controles.recuperarValor('txtFuncaoAnunciarVaga');

        var valorMinimo = mediaSalarial.value.split(';')[0];
        var valorMaximo = mediaSalarial.value.split(';')[1];
        var mensagem = VariaveisGlobais.MsgFaixaSalarial.replace('{0}', txtFuncao).replace('{1}', valorMinimo).replace('{2}', valorMaximo);
        employer.controles.setAttr('faixa_salarial', 'innerHTML', mensagem);
        employer.util.findControl('faixa_salarial').css('display', 'block');
        employer.util.findControl('hfFaixaSalarial')[0].value = mensagem;

        employer.controles.setValorControle('txtSalarioDe', valorMinimo);
        employer.controles.setValorControle('txtSalarioAte', valorMaximo);
    }
    else {
        employer.util.findControl('faixa_salarial').css('display', 'none');
        employer.util.findControl('hfFaixaSalarial')[0].value = '';
    }
}

function AjustarScroll() {
    $(window).scrollTop(0);
}

function Funcao_LostFocus(e) {


    if (e == null || typeof e == 'undefinied') {
        return;
    }

    ocultarImgAmigavel();

    if ($('#cphConteudo_ucContratoFuncao_rblContrato_1').attr('checked')) { //estagio
        employer.util.findControl('txtSugestaoTarefasAnunciarVaga').prop('text', '');
        return;
    }

    var res = AnunciarVaga.RecuperarJobFuncao(e.val());

    if (res.error != null)
        return;


    var textBox = $("textarea[name*='txtSugestaoTarefasAnunciarVaga']");
    if (textBox == null || typeof textBox == 'undefined') {
        textBox = $("input[name*='txtSugestaoTarefasAnunciarVaga']");
    }
    if (textBox == null || typeof textBox == 'undefined')
        return;

    textBox.text(res.value);
}

//Validar tipo de contrato com a escolaridade
function ValidarTipoContratoComEscolaridade() {

    if ($('#cphConteudo_ucContratoFuncao_rblContrato_1').attr('checked')) {
        if ($('.rcbList').find('.rcbHovered:first:contains("Incompleto")').length == 0) {
            alert('Escolaridade não condiz com o tipo de contrato Estágio!');
        }
    }
}

function exibirImgAmigavel() {
    employer.util.findControl('imgAmigavelAnuncioVaga').fadeIn("slow");
}

function ocultarImgAmigavel() {
    employer.util.findControl('imgAmigavelAnuncioVaga').css('display', 'none');
}

function ValidarMediaSalarial() {
    var salario_de = '';
    var salario_ate = '';

    ocultarImgAmigavel();


    //Validar Função e Cidade
    var value = employer.controles.recuperarValor('txtFuncaoAnunciarVaga');
    if (value == null) value = '';
    var secondValue = employer.controles.recuperarValor('txtCidadeAnunciarVaga');
    if (secondValue == null) secondValue = '';

    var retorno = AnunciarVaga.ValidarFuncaoCidade(value, secondValue);
    if (retorno == null || retorno.value == '')
        return; //informação do estado e da função, executa o processo caso eles tenham sido informados

    var idFuncao = retorno.value.split(';')[0];
    var siglaEstado = retorno.value.split(';')[1];

    VariaveisGlobais.idFuncao = idFuncao;
    VariaveisGlobais.siglaEstado = siglaEstado;

    if ($('#cphConteudo_txtSalarioDe').val() != '')
        salario_de = $('#cphConteudo_txtSalarioDe').val().replace('.', '');

    if ($('#cphConteudo_txtSalarioAte').val() != '')
        salario_ate = $('#cphConteudo_txtSalarioAte').val().replace('.', '');

    if (salario_de == '' && salario_ate == '')
        return;

    if (salario_de != '' && salario_ate != '') {
        if (parseFloat(salario_ate) < parseFloat(salario_de))
            return;
    }

    //Post API Salario BR para verificar a média
    $.ajax({
        type: "GET",
        url: urlApiSalarioBR + "Funcoes/ValidaFaixaSalarialFuncao?funcao=" + idFuncao + "&sig_estado=" + siglaEstado + "&funcoes_sbr=false&salario_de=" + salario_de + "&salario_ate=" + salario_ate,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            $('#txtNivelSalario').removeClass('textoMediaSalarial_vermelho');
            $('#txtNivelSalario').removeClass('textoMediaSalarial_amarelo');
            $('#txtNivelSalario').removeClass('textoMediaSalarial_verde');

            if (msg.retorno == "abaixo") {
                $('#txtNivelSalario').html('ABAIXO');
                $('#txtNivelSalario').addClass('textoMediaSalarial_vermelho');
                exibirImgAmigavel();
            } else if (msg.retorno == "media") {
                $('#txtNivelSalario').html('NA MÉDIA');
                $('#txtNivelSalario').addClass('textoMediaSalarial_amarelo');
                exibirImgAmigavel();
            } else if (msg.retorno == "acima") {
                $('#txtNivelSalario').addClass('textoMediaSalarial_verde');
                $('#txtNivelSalario').html('ACIMA');
                exibirImgAmigavel();
            }
        }, error: function () {
            console.log('Falha na busca dos dados Salario BR.');
        }
    });
}

function AjustarPretensaoSalarial() {
    $('#cphConteudo_avisoSalarioinvalido').hide();

    $('#cphConteudo_txtSalarioDe').on('focus', function () { $('#cphConteudo_avisoSalarioinvalido').hide(); });
    $('#cphConteudo_anunciarvaga_txtSalarioAte').on('focus', function () { $('#cphConteudo_avisoSalarioinvalido').hide(); });

    var options = {
        reverse: true
    }
    $('#cphConteudo_txtSalarioDe').mask('0.000.000,00', options);
    $('#cphConteudo_txtSalarioAte').mask('0.000.000,00', options);

    $('#cphConteudo_txtSalarioDe').on('blur', function () {
        validarSalarioMinimo($('#cphConteudo_txtSalarioDe').val(), $('#cphConteudo_txtSalarioAte').val());
    });

    $('#cphConteudo_txtSalarioAte').on('blur', function () {
        validarSalarioMinimo($('#cphConteudo_txtSalarioDe').val(), $('#cphConteudo_txtSalarioAte').val());
    });

    function validarSalarioMinimo(valorDe, valorAte) {
        var possuiErro = false;
        var ehEstagio = $('#cphConteudo_hfEstagio').val() === "1" ? true : false;
        var valordoMinimo = $('#cphConteudo_hdfValorSalarioMinimo').val().replace(',', '.');

        if (ehEstagio) {
            valordoMinimo = "1.00";
        }

        var possuiValorDe = typeof (valorDe) !== 'undefined' && valorDe !== '';
        var possuiValorAte = typeof (valorAte) !== 'undefined' && valorAte !== '';
        if (possuiValorDe) {
            valorDe = valorDe.replace('.', '').replace(',', '.');
            if (parseFloat(valorDe) < parseFloat(valordoMinimo)) {
                possuiErro = true;
                if (ehEstagio) {
                    $('#cphConteudo_avisoSalarioInvalido').text('O valor do salário deve ser maior que R$ ' + valordoMinimo.replace('.', ','));
                } else {
                    $('#cphConteudo_avisoSalarioInvalido').text('O valor do salário deve ser maior que o Salário Mínimo Nacional R$ ' + valordoMinimo.replace('.', ','));
                }
            }
        }
        if (possuiValorAte) {
            valorAte = valorAte.replace('.', '').replace(',', '.');
            if (parseFloat(valorAte) < parseFloat(valordoMinimo)) {
                possuiErro = true;
                if (ehEstagio) {
                    $('#cphConteudo_avisoSalarioInvalido').text('O valor do salário deve ser maior que R$ ' + valordoMinimo.replace('.', ','));
                } else {
                    $('#cphConteudo_avisoSalarioInvalido').text('O valor do salário deve ser maior que o Salário Mínimo Nacional R$ ' + valordoMinimo.replace('.', ','));
                }
            }
        }

        if (possuiValorDe && possuiValorAte && !possuiErro) {
            if (parseFloat(valorAte) < parseFloat(valorDe)) {
                possuiErro = true;
                $('#cphConteudo_avisoSalarioInvalido').text('Valor mínimo de R$ ' + valorDe.replace('.', ','));
            }
        }

        if (possuiErro) {
            $('#cphConteudo_avisoSalarioInvalido').show();
        } else {
            $('#cphConteudo_avisoSalarioInvalido').hide();
        }
    }

}

function LocalPageLoad() {
    AjustarPretensaoSalarial();
}