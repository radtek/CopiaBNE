$(document).ready(function () {
    //Ajustando o scroll
    $(window).scroll(function () {
        var vTop = ($(window).height() / 2) - $("#divSlowDown").height() / 2 + $(top.window).scrollTop();

        //Ajustando a altura para não estourar o layout
        if (vTop < 650)
            vTop = 650;
        else if (vTop > 3230)
            vTop = 3225;

        //if(vTop >850
        $("#divSlowDown").stop().animate({ "top": vTop + "px" }, { duration: 700, queue: false });
    });

    //Ajustando o background
    $('body').addClass('bg_fundo_empresa');

    //ocultando a div de aviso sobre a data de demissão
    $('.tooltips_aviso').hide();

    $("#cphConteudo_ucDadosPessoais_lnkAddExperiencia").click(function () {
        $('html, body').animate({
            scrollTop: $("#cphConteudo_ucDadosPessoais_btnSalvar").offset().top
        }, 2000);
    });

});


function txtAtividadeExercida1_OnBlur() {
    employer.controles.setFocusControle('txtUltimoSalario');
}
function txtAtividadeExercida2_OnBlur() {
    employer.controles.setFocus('txtUltimoSalario2');
}
function txtAtividadeExercida3_OnBlur() {
    employer.controles.setFocus('txtUltimoSalario3');
}
function txtAtividadeExercida4_OnBlur() {
    employer.controles.setFocus('txtUltimoSalario4');
}
function txtAtividadeExercida5_OnBlur() {
    employer.controles.setFocus('txtUltimoSalario5');
}
function txtAtividadeExercida6_OnBlur() {
    employer.controles.setFocus('txtUltimoSalario6');
}
function txtAtividadeExercida7_OnBlur() {
    employer.controles.setFocus('txtUltimoSalario7');
}
function txtAtividadeExercida8_OnBlur() {
    employer.controles.setFocus('txtUltimoSalario8');
}
function txtAtividadeExercida9_OnBlur() {
    employer.controles.setFocus('txtUltimoSalario9');
}
function txtAtividadeExercida10_OnBlur() {
    employer.controles.setFocus('txtUltimoSalario10');
}

function cvCidadeDadosPessoais_Validate(sender, args) {
    var res = DadosPessoais.ValidarCidade(args.Value);
    args.IsValid = (res.error == null && res.value);
}

$(document).ready(function () {
    window.onload = null;
    window.onunload = null;
    window.onbeforeunload = null;
});

function AjustarCamposExp1(value) {
    if (value != '') {
        employer.controles.setAttr('txtDataAdmissao1', 'Obrigatorio', true);
        employer.controles.enableValidatorVal('rfvFuncaoExercida1', true, true);
        employer.controles.enableValidatorVal('cvFuncaoExercida1', true, true);
        employer.controles.enableValidatorControleVal('txtAtividadeExercida1', 'rfValor', true, true);
        employer.controles.setAttr('txtAtividadeExercida1', 'Obrigatorio', true);
        employer.controles.enableValidatorVal('cvAtividadeEmpresa1', true, true);
        employer.controles.enableValidatorVal('rfvUltimoSalario', true, true);
    }
    else {
        employer.controles.setAttr('txtDataAdmissao1', 'Obrigatorio', false);
        employer.controles.enableValidatorVal('rfvFuncaoExercida1', false, true);
        employer.controles.enableValidatorVal('cvFuncaoExercida1', false, true);
        employer.controles.enableValidatorControleVal('txtAtividadeExercida1', 'rfValor', false, true);
        employer.controles.setAttr('txtAtividadeExercida1', 'Obrigatorio', false);
        employer.controles.enableValidatorVal('cvAtividadeEmpresa1', false, true);
        employer.controles.enableValidatorVal('rfvUltimoSalario', false, true);
    }
}
function AjustarCamposExp2(value) {
    if (value != '') {
        employer.controles.setAttr('txtDataAdmissao2', 'Obrigatorio', true);
        employer.controles.enableValidatorVal('rfvFuncaoExercida2', true, true);
        employer.controles.enableValidatorVal('cvFuncaoExercida2', true, true);
        employer.controles.setAttr('txtAtividadeExercida2', 'Obrigatorio', true);
        employer.controles.enableValidatorControleVal('txtAtividadeExercida2', 'rfValor', true, true);
        employer.controles.enableValidatorVal('cvAtividadeEmpresa2', true, true);
    }
    else {
        employer.controles.setAttr('txtDataAdmissao2', 'Obrigatorio', false);
        employer.controles.enableValidatorVal('rfvFuncaoExercida2', false, true);
        employer.controles.enableValidatorVal('cvFuncaoExercida2', false, true);
        employer.controles.setAttr('txtAtividadeExercida2', 'Obrigatorio', false);
        employer.controles.enableValidatorControleVal('txtAtividadeExercida2', 'rfValor', false, true);
        employer.controles.enableValidatorVal('cvAtividadeEmpresa2', false, true);
    }
}
function AjustarCamposExp(value) {
    if (value != '') {
        employer.controles.setAttr('txtDataAdmissao3', 'Obrigatorio', true);
        employer.controles.enableValidatorVal('rfvFuncaoExercida3', true, true);
        employer.controles.enableValidatorVal('cvFuncaoExercida3', true, true);
        employer.controles.setAttr('txtAtividadeExercida3', 'Obrigatorio', true);
        employer.controles.enableValidatorControleVal('txtAtividadeExercida3', 'rfValor', true, true);
        employer.controles.enableValidatorVal('cvAtividadeEmpresa3', true, true);
    }
    else {
        employer.controles.setAttr('txtDataAdmissao3', 'Obrigatorio', false);
        employer.controles.enableValidatorVal('rfvFuncaoExercida3', false, true);
        employer.controles.enableValidatorVal('cvFuncaoExercida3', false, true);
        employer.controles.setAttr('txtAtividadeExercida3', 'Obrigatorio', false);
        employer.controles.enableValidatorControleVal('txtAtividadeExercida3', 'rfValor', false, true);
        employer.controles.enableValidatorVal('cvAtividadeEmpresa3', false, true);
    }
}
function AjustarCamposExp4(value) {
    if (value != '') {
        employer.controles.setAttr('txtDataAdmissao4', 'Obrigatorio', true);
        employer.controles.enableValidatorVal('rfvFuncaoExercida4', true, true);
        employer.controles.enableValidatorVal('cvFuncaoExercida4', true, true);
        employer.controles.setAttr('txtAtividadeExercida4', 'Obrigatorio', true);
        employer.controles.enableValidatorControleVal('txtAtividadeExercida4', 'rfValor', true, true);
        employer.controles.enableValidatorVal('cvAtividadeEmpresa4', true, true);
    }
    else {
        employer.controles.setAttr('txtDataAdmissao4', 'Obrigatorio', false);
        employer.controles.enableValidatorVal('rfvFuncaoExercida4', false, true);
        employer.controles.enableValidatorVal('cvFuncaoExercida4', false, true);
        employer.controles.setAttr('txtAtividadeExercida4', 'Obrigatorio', false);
        employer.controles.enableValidatorControleVal('txtAtividadeExercida4', 'rfValor', false, true);
        employer.controles.enableValidatorVal('cvAtividadeEmpresa4', false, true);
    }
}
function AjustarCamposExp5(value) {
    if (value != '') {
        employer.controles.setAttr('txtDataAdmissao5', 'Obrigatorio', true);
        employer.controles.enableValidatorVal('rfvFuncaoExercida5', true, true);
        employer.controles.enableValidatorVal('cvFuncaoExercida5', true, true);
        employer.controles.setAttr('txtAtividadeExercida5', 'Obrigatorio', true);
        employer.controles.enableValidatorControleVal('txtAtividadeExercida5', 'rfValor', true, true);
        employer.controles.enableValidatorVal('cvAtividadeEmpresa5', true, true);
    }
    else {
        employer.controles.setAttr('txtDataAdmissao5', 'Obrigatorio', false);
        employer.controles.enableValidatorVal('rfvFuncaoExercida5', false, true);
        employer.controles.enableValidatorVal('cvFuncaoExercida5', false, true);
        employer.controles.setAttr('txtAtividadeExercida5', 'Obrigatorio', false);
        employer.controles.enableValidatorControleVal('txtAtividadeExercida5', 'rfValor', false, true);
        employer.controles.enableValidatorVal('cvAtividadeEmpresa5', false, true);
    }
}
function AjustarCamposExp6(value) {
    if (value != '') {
        employer.controles.setAttr('txtDataAdmissao6', 'Obrigatorio', true);
        employer.controles.enableValidatorVal('rfvFuncaoExercida6', true, true);
        employer.controles.enableValidatorVal('cvFuncaoExercida6', true, true);
        employer.controles.setAttr('txtAtividadeExercida6', 'Obrigatorio', true);
        employer.controles.enableValidatorControleVal('txtAtividadeExercida6', 'rfValor', true, true);
        employer.controles.enableValidatorVal('cvAtividadeEmpresa6', true, true);
    }
    else {
        employer.controles.setAttr('txtDataAdmissao6', 'Obrigatorio', false);
        employer.controles.enableValidatorVal('rfvFuncaoExercida6', false, true);
        employer.controles.enableValidatorVal('cvFuncaoExercida6', false, true);
        employer.controles.setAttr('txtAtividadeExercida6', 'Obrigatorio', false);
        employer.controles.enableValidatorControleVal('txtAtividadeExercida6', 'rfValor', false, true);
        employer.controles.enableValidatorVal('cvAtividadeEmpresa6', false, true);
    }
}
function AjustarCamposExp7(value) {
    if (value != '') {
        employer.controles.setAttr('txtDataAdmissao7', 'Obrigatorio', true);
        employer.controles.enableValidatorVal('rfvFuncaoExercida7', true, true);
        employer.controles.enableValidatorVal('cvFuncaoExercida7', true, true);
        employer.controles.setAttr('txtAtividadeExercida7', 'Obrigatorio', true);
        employer.controles.enableValidatorControleVal('txtAtividadeExercida7', 'rfValor', true, true);
        employer.controles.enableValidatorVal('cvAtividadeEmpresa7', true, true);
    }
    else {
        employer.controles.setAttr('txtDataAdmissao7', 'Obrigatorio', false);
        employer.controles.enableValidatorVal('rfvFuncaoExercida7', false, true);
        employer.controles.enableValidatorVal('cvFuncaoExercida7', false, true);
        employer.controles.setAttr('txtAtividadeExercida7', 'Obrigatorio', false);
        employer.controles.enableValidatorControleVal('txtAtividadeExercida7', 'rfValor', false, true);
        employer.controles.enableValidatorVal('cvAtividadeEmpresa7', false, true);
    }
}
function AjustarCamposExp8(value) {
    if (value != '') {
        employer.controles.setAttr('txtDataAdmissao8', 'Obrigatorio', true);
        employer.controles.enableValidatorVal('rfvFuncaoExercida8', true, true);
        employer.controles.enableValidatorVal('cvFuncaoExercida8', true, true);
        employer.controles.setAttr('txtAtividadeExercida8', 'Obrigatorio', true);
        employer.controles.enableValidatorControleVal('txtAtividadeExercida8', 'rfValor', true, true);
        employer.controles.enableValidatorVal('cvAtividadeEmpresa8', true, true);
    }
    else {
        employer.controles.setAttr('txtDataAdmissao8', 'Obrigatorio', false);
        employer.controles.enableValidatorVal('rfvFuncaoExercida8', false, true);
        employer.controles.enableValidatorVal('cvFuncaoExercida8', false, true);
        employer.controles.setAttr('txtAtividadeExercida8', 'Obrigatorio', false);
        employer.controles.enableValidatorControleVal('txtAtividadeExercida8', 'rfValor', false, true);
        employer.controles.enableValidatorVal('cvAtividadeEmpresa8', false, true);
    }
}
function AjustarCamposExp9(value) {
    if (value != '') {
        employer.controles.setAttr('txtDataAdmissao9', 'Obrigatorio', true);
        employer.controles.enableValidatorVal('rfvFuncaoExercida9', true, true);
        employer.controles.enableValidatorVal('cvFuncaoExercida9', true, true);
        employer.controles.setAttr('txtAtividadeExercida9', 'Obrigatorio', true);
        employer.controles.enableValidatorControleVal('txtAtividadeExercida9', 'rfValor', true, true);
        employer.controles.enableValidatorVal('cvAtividadeEmpresa9', true, true);
    }
    else {
        employer.controles.setAttr('txtDataAdmissao9', 'Obrigatorio', false);
        employer.controles.enableValidatorVal('rfvFuncaoExercida9', false, true);
        employer.controles.enableValidatorVal('cvFuncaoExercida9', false, true);
        employer.controles.setAttr('txtAtividadeExercida9', 'Obrigatorio', false);
        employer.controles.enableValidatorControleVal('txtAtividadeExercida9', 'rfValor', false, true);
        employer.controles.enableValidatorVal('cvAtividadeEmpresa9', false, true);
    }
}
function AjustarCamposExp10(value) {
    if (value != '') {
        employer.controles.setAttr('txtDataAdmissao10', 'Obrigatorio', true);
        employer.controles.enableValidatorVal('rfvFuncaoExercida10', true, true);
        employer.controles.enableValidatorVal('cvFuncaoExercida10', true, true);
        employer.controles.setAttr('txtAtividadeExercida10', 'Obrigatorio', true);
        employer.controles.enableValidatorControleVal('txtAtividadeExercida10', 'rfValor', true, true);
        employer.controles.enableValidatorVal('cvAtividadeEmpresa10', true, true);
    }
    else {
        employer.controles.setAttr('txtDataAdmissao10', 'Obrigatorio', false);
        employer.controles.enableValidatorVal('rfvFuncaoExercida10', false, true);
        employer.controles.enableValidatorVal('cvFuncaoExercida10', false, true);
        employer.controles.setAttr('txtAtividadeExercida10', 'Obrigatorio', false);
        employer.controles.enableValidatorControleVal('txtAtividadeExercida10', 'rfValor', false, true);
        employer.controles.enableValidatorVal('cvAtividadeEmpresa10', false, true);
    }
}

function cvEstadoCivil_Validate(sender, args) {
    var res = DadosPessoais.ValidarEstadoCivil(args.Value);
    args.IsValid = (res.error == null && res.value != '');
}

function cvAtividadeExercida1_Validate(sender, args) {
    args.IsValid = args.Value > 0 ? true : false;
}
function cvAtividadeExercida2_Validate(sender, args) {
    args.IsValid = args.Value > 0 ? true : false;
}
function cvAtividadeExercida3_Validate(sender, args) {
    args.IsValid = args.Value > 0 ? true : false;
}
function cvAtividadeExercida4_Validate(sender, args) {
    args.IsValid = args.Value > 0 ? true : false;
}
function cvAtividadeExercida5_Validate(sender, args) {
    args.IsValid = args.Value > 0 ? true : false;
}
function cvAtividadeExercida6_Validate(sender, args) {
    args.IsValid = args.Value > 0 ? true : false;
}
function cvAtividadeExercida7_Validate(sender, args) {
    args.IsValid = args.Value > 0 ? true : false;
}
function cvAtividadeExercida8_Validate(sender, args) {
    args.IsValid = args.Value > 0 ? true : false;
}
function cvAtividadeExercida9_Validate(sender, args) {
    args.IsValid = args.Value > 0 ? true : false;
}
function cvAtividadeExercida10_Validate(sender, args) {
    args.IsValid = args.Value > 0 ? true : false;
}

function txtFuncaoExercida1_TextChanged(args) {
    var res = DadosPessoais.RecuperarJobFuncao(args.value);
    if (res.error == null) { }
}
function txtFuncaoExercida2_TextChanged(args) {
    var res = DadosPessoais.RecuperarJobFuncao(args.value);
    if (res.error == null) { }
}
function txtFuncaoExercida3_TextChanged(args) {
    var res = DadosPessoais.RecuperarJobFuncao(args.value);
    if (res.error == null) { }
}
function txtFuncaoExercida4_TextChanged(args) {
    var res = DadosPessoais.RecuperarJobFuncao(args.value);
    if (res.error == null) { }
}
function txtFuncaoExercida5_TextChanged(args) {
    var res = DadosPessoais.RecuperarJobFuncao(args.value);
    if (res.error == null) { }
}
function txtFuncaoExercida6_TextChanged(args) {
    var res = DadosPessoais.RecuperarJobFuncao(args.value);
    if (res.error == null) { }
}
function txtFuncaoExercida7_TextChanged(args) {
    var res = DadosPessoais.RecuperarJobFuncao(args.value);
    if (res.error == null) { }
}
function txtFuncaoExercida8_TextChanged(args) {
    var res = DadosPessoais.RecuperarJobFuncao(args.value);
    if (res.error == null) { }
}
function txtFuncaoExercida9_TextChanged(args) {
    var res = DadosPessoais.RecuperarJobFuncao(args.value);
    if (res.error == null) { }
}
function txtFuncaoExercida10_TextChanged(args) {
    var res = DadosPessoais.RecuperarJobFuncao(args.value);
    if (res.error == null) { }
}

function txtAtividadeExercida1_KeyUp() {
    contarCaracteres(document.getElementById('cphConteudo_ucDadosPessoais_txtAtividadeExercida1_txtValor').value, 'GraficoQualidade1');
}
function txtAtividadeExercida2_KeyUp() {
    contarCaracteres(document.getElementById('cphConteudo_ucDadosPessoais_txtAtividadeExercida2_txtValor').value, 'GraficoQualidade2');
}
function txtAtividadeExercida3_KeyUp() {
    contarCaracteres(document.getElementById('cphConteudo_ucDadosPessoais_txtAtividadeExercida3_txtValor').value, 'GraficoQualidade3');
}
function txtAtividadeExercida4_KeyUp() {
    if (document.getElementById('cphConteudo_ucDadosPessoais_txtAtividadeExercida4_txtValor') != null) {
        contarCaracteres(document.getElementById('cphConteudo_ucDadosPessoais_txtAtividadeExercida4_txtValor').value, 'GraficoQualidade4');
    }
}
function txtAtividadeExercida5_KeyUp() {
    if (document.getElementById('cphConteudo_ucDadosPessoais_txtAtividadeExercida5_txtValor') != null) {
        contarCaracteres(document.getElementById('cphConteudo_ucDadosPessoais_txtAtividadeExercida5_txtValor').value, 'GraficoQualidade5');
    }
}
function txtAtividadeExercida6_KeyUp() {
    if (document.getElementById('cphConteudo_ucDadosPessoais_txtAtividadeExercida6_txtValor') != null) {
        contarCaracteres(document.getElementById('cphConteudo_ucDadosPessoais_txtAtividadeExercida6_txtValor').value, 'GraficoQualidade6');
    }
}
function txtAtividadeExercida7_KeyUp() {
    if (document.getElementById('cphConteudo_ucDadosPessoais_txtAtividadeExercida7_txtValor') != null) {
        contarCaracteres(document.getElementById('cphConteudo_ucDadosPessoais_txtAtividadeExercida7_txtValor').value, 'GraficoQualidade7');
    }
}
function txtAtividadeExercida8_KeyUp() {
    if (document.getElementById('cphConteudo_ucDadosPessoais_txtAtividadeExercida8_txtValor') != null) {
        contarCaracteres(document.getElementById('cphConteudo_ucDadosPessoais_txtAtividadeExercida8_txtValor').value, 'GraficoQualidade8');
    }
}
function txtAtividadeExercida9_KeyUp() {
    if (document.getElementById('cphConteudo_ucDadosPessoais_txtAtividadeExercida9_txtValor') != null) {
        contarCaracteres(document.getElementById('cphConteudo_ucDadosPessoais_txtAtividadeExercida9_txtValor').value, 'GraficoQualidade9');
    }
}
function txtAtividadeExercida10_KeyUp() {
    if (document.getElementById('cphConteudo_ucDadosPessoais_txtAtividadeExercida10_txtValor') != null) {
        contarCaracteres(document.getElementById('cphConteudo_ucDadosPessoais_txtAtividadeExercida10_txtValor').value, 'GraficoQualidade10');
    }
}

function ChecarGraficoQualidadeDasAtividadesExercidas() {

    txtAtividadeExercida1_KeyUp();
    txtAtividadeExercida2_KeyUp();
    txtAtividadeExercida3_KeyUp();
    txtAtividadeExercida4_KeyUp();
    txtAtividadeExercida5_KeyUp();
    txtAtividadeExercida6_KeyUp();
    txtAtividadeExercida7_KeyUp();
    txtAtividadeExercida8_KeyUp();
    txtAtividadeExercida9_KeyUp();
    txtAtividadeExercida10_KeyUp();
}

function contarCaracteres(box, campoRetorno) {
    var conta = box.length;

    if (conta > 0) {
        if (conta <= 70) {
            document.getElementById(campoRetorno).innerHTML = "<div class='icon icon-fraco icon-size'></br><span class='labelIcon'>FRACO</span></div>";
            //document.getElementById('contadorcatacter1').innerHTML = conta + ' caracteres';
        }
        else if (conta > 70 && conta <= 140) {
            document.getElementById(campoRetorno).innerHTML = "<div class='icon icon-regular icon-size'></br><span class='labelIcon'>REGULAR</span></div>";
            //document.getElementById('contadorcatacter1').innerHTML = conta + ' caracteres';
        }
        else {
            document.getElementById(campoRetorno).innerHTML = "<div class='icon icon-otimo icon-size'></br><span class='labelIcon'>ÓTIMO</span></div>";
            //document.getElementById('contadorcatacter1').innerHTML = conta + ' caracteres';
        }
    }
}

//Validar data de demissão para que não seja menor que a data de Admissão
function ValidarDataDemissao(campoDtaAdmissao, campoDtaDemissao, divAviso) {

    var txtDataAdmissao = document.getElementById(campoDtaAdmissao).value;
    var txtDataDemissao = document.getElementById(campoDtaDemissao).value;

    var dataAdmissao = parseInt(txtDataAdmissao.split("/")[2].toString() + txtDataAdmissao.split("/")[1].toString() + txtDataAdmissao.split("/")[0].toString());
    var dataDemissao = parseInt(txtDataDemissao.split("/")[2].toString() + txtDataDemissao.split("/")[1].toString() + txtDataDemissao.split("/")[0].toString());

    if (dataAdmissao > dataDemissao) {

        $('#' + campoDtaDemissao).addClass('inputError');
        $('#cphConteudo_ucDadosPessoais_' + divAviso).show();
        $('#cphConteudo_ucDadosPessoais_btnSalvar').attr('disabled', 'disabled');
    }
    else {
        $('#' + campoDtaDemissao).removeClass('inputError');
        $('#cphConteudo_ucDadosPessoais_' + divAviso).hide();
        $('#cphConteudo_ucDadosPessoais_btnSalvar').removeAttr('disabled');
        
    }
}