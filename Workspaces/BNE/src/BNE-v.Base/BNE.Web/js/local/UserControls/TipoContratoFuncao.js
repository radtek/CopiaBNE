
function cvFuncaoAnunciarVaga_Validate(sender, args) {

    res = ContratoFuncao.ValidarFuncao(args.Value);
    args.IsValid = (res.error == null && res.value);

    if (!args.IsValid) {
        sender.innerHTML = "Função Inválida";
        return;
    }

    var res = ValidarFuncaoLimitacaoPorContrato(args.Value);
    args.IsValid = (res.error == null && res.value);

    //if (!args.IsValid) {
    //    sender.innerHTML = "Função indisponível";
    //}
}

function ValidarFuncaoLimitacaoPorContrato(value) {
    if (value == null)
        return { 'IsValid': false, 'value': false, 'error': null };

    var limitacaoContrato = replaceDiacritics(value).toString().trim().toLowerCase();
    if (limitacaoContrato == "aprendiz"
        || limitacaoContrato == "estagiario"
        || limitacaoContrato == "estagiaria"
        || limitacaoContrato == "estagio")
        return { 'IsValid': false, 'value': false, 'error': null };

    return { 'IsValid': true, 'value': true, 'error': null };
}
//function cvFuncaoAnunciarVagaLimitacaoPorContrato_Validate(sender, args) {
//    var res = AnunciarVaga.ValidarFuncaoLimitacaoPorContrato(args.Value);
//    args.IsValid = (res.error == null && res.value);
//}

function replaceDiacritics(s) {
    var s;
    var diacritics = [
        /[\300-\306]/g, /[\340-\346]/g,  // A, a
        /[\310-\313]/g, /[\350-\353]/g,  // E, e
        /[\314-\317]/g, /[\354-\357]/g,  // I, i
        /[\322-\330]/g, /[\362-\370]/g,  // O, o
        /[\331-\334]/g, /[\371-\374]/g,  // U, u
    ];

    var chars = ['A', 'a', 'E', 'e', 'I', 'i', 'O', 'o', 'U', 'u'];

    for (var i = 0; i < diacritics.length; i++) {
        s = s.replace(diacritics[i], chars[i]);
    }

    return s;
}

function ComportamentoBaseadoEmEstagioAprendiz() {
    var textBox = $("input:text[name*='txtFuncaoAnunciarVaga']");
    if (textBox == null) return;

    var text = textBox.val();
    if (text == null) return;

    text = $.trim(text);
    if (text.length <= 2) return;

    text = replaceDiacritics(text);
    text = text.toString().toLowerCase();
    if (text != "aprendiz" && text != "estagio" && text != "estagiario" && text != "estagiaria") {
        return;
    }

    var checkbox = getContratoCheckBox(text);

    if (checkbox == null || checkbox.length == 0 || checkbox.is(':checked')) return;

    limparCheckBoxList();

    checkbox.attr('checked', true);
    if (text != "aprendiz") {
        $("[id='box_tipo_contrato']").show();
    }
}

function bindEvents() {
    bindFuncaoEvents();
    bindCheckBoxListEvents();

    var first = "aprendiz";
    var checkbox = getContratoCheckBox(first);
    var second = "estágio";
    var secondCheckBox = getContratoCheckBox(second);
    calcularBoxTipoContrato(checkbox, secondCheckBox);
}

function bindCheckBoxListEvents() {
    var first = "aprendiz";
    var checkbox = getContratoCheckBox(first);
    var second = "estágio";
    var secondCheckBox = getContratoCheckBox(second);

    if (checkbox != null && typeof checkbox != 'undefined') {
        checkbox.change(function () {
            calcularBoxTipoContrato(checkbox, secondCheckBox);
        });
    }

    if (secondCheckBox == null || typeof secondCheckBox == 'undefined')
        return;

    secondCheckBox.change(function () {
        calcularBoxTipoContrato(checkbox, secondCheckBox);
    });
}

function calcularBoxTipoContrato(aprendizCheckBox, estagioCheckBox) {
    if ((aprendizCheckBox != null && aprendizCheckBox.is(':checked')) || (estagioCheckBox != null && estagioCheckBox.is(':checked'))) {
       
        var pagina;
        // PEGANDO O NOME DO DOCUMENTO OU PÁGINA ATUAL
        pagina = window.location.href.substr(window.location.href.lastIndexOf("/") + 1);
        
        if (pagina == "anunciar-vaga-gratis") {
            $("[id='div_AnuncioVagasEstagio']").show();
            $("[id='box_tipo_contrato']").hide();
        }
     
        if (pagina == "pesquisa-de-vagas") {
           // alert('opa');
            $("[id='box_tipo_contrato']").show();
            $("[id='div_AnuncioVagasEstagio']").hide();
        }

    }
    else {
        $("[id='box_tipo_contrato']").hide();
        $("[id='div_AnuncioVagasEstagio']").hide();
    }
}
function bindFuncaoEvents() {
    var textBox = $("input:text[name*='txtFuncaoAnunciarVaga']");
    if (textBox == null || typeof textBox == 'undefined') return;
    var lastValue = '';
    textBox.on('change keyup paste mouseup focusout', function () {
       
        if ($(this).val() != lastValue) {
            lastValue = $(this).val();
            ComportamentoBaseadoEmEstagioAprendiz();
        }
    });
}

function getContratoCheckBox(text) {
    var table = $('table[id*="chblContrato"]');
    if (table == null) return null;

    var label;
    if (text == "aprendiz")
        label = table.find('label:contains("Aprendiz")');
    else
        label = table.find('label:contains("Estágio")');

    var checkbox = $('[id="' + label.attr('for') + '"]');
    return checkbox;
}

function limparCheckBoxList() {

    $('#cphConteudo_ucContratoFuncao_chblContrato_0').removeAttr('checked');
    $('#cphConteudo_ucContratoFuncao_chblContrato_1').removeAttr('checked');
    $('#cphConteudo_ucContratoFuncao_chblContrato_2').removeAttr('checked');
    $('#cphConteudo_ucContratoFuncao_chblContrato_3').removeAttr('checked');
    $('#cphConteudo_ucContratoFuncao_chblContrato_4').removeAttr('checked');
    $('#cphConteudo_ucContratoFuncao_chblContrato_5').removeAttr('checked');

}