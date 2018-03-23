var VariaveisGlobais = {
    MsgFaixaSalarial: ''
}

function InicializarVariaveis(parametros) {
    VariaveisGlobais.MsgFaixaSalarial = parametros.msgFaixaSalarial;
}

function removerUltimaVirgula(str) {
    return str.replace(/,$/, "");
}

$(document).ready(function () {
    employer.util.findControl('imgAmigavelAnuncioVaga').fadeIn("slow");
});

function ocultarImgAmigavel() {
    employer.util.findControl('imgAmigavelAnuncioVaga').css('display', 'none');
}

function redirectToWebEstagios() {
    var href = 'webestagios.com.br';
    var link = $('<a href="http://' + href + '" />');
    link.attr('target', '_blank');
    window.open(link.attr('href'));
    return false;
}
/*
function SetarAtribuicaoFuncao()
{
    var funcao = employer.controles.recuperarValor('txtFuncaoAnunciarVaga');
    var atribuicao = AnunciarVaga.FuncaoAtribuicaoJob(funcao);
    if(atribuicao.value != null)
        employer.controles.setValor('txtSugestaoAtribuicoes', atribuicao.value);
    else
        employer.controles.setValor('txtSugestaoAtribuicoes', " ");
}*/

function cvCidade_Validate(sender, args) {
    var res = AnunciarVaga.ValidarCidade(args.Value);
    args.IsValid = (res.error == null && res.value);
}

function Cidade_OnChange(args) {
    var value = employer.controles.recuperarValor('txtFuncaoAnunciarVaga');
    if (value == null) value = '';
    var secondValue = employer.controles.recuperarValor('txtCidadeAnunciarVaga');
    if (secondValue == null) secondValue = '';

    var mediaSalarial = AnunciarVaga.PesquisarMediaSalarial(value, secondValue);
    if (mediaSalarial.value != null && mediaSalarial.value != 0) {
        var txtFuncao = employer.controles.recuperarValor('txtFuncaoAnunciarVaga');

        var valorMinimo = mediaSalarial.value.split(';')[0];//parseFloat(mediaSalarial.value.split(';')[0]).toFixed(2).replace('.', ',');
        var valorMaximo = mediaSalarial.value.split(';')[1];//parseFloat(mediaSalarial.value.split(';')[1]).toFixed(2).replace('.', ',');
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
   
   
    if ($('#cphConteudo_ucContratoFuncao_chblContrato_1').attr('checked')) { //estagio
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

    if ($('#cphConteudo_ucContratoFuncao_chblContrato_1').attr('checked')) {
        if ($('.rcbList').find('.rcbHovered:first:contains("Incompleto")').length == 0) {
            alert('Escolaridade não condiz com o tipo de contrato Estágio!');
        }
    }
}


