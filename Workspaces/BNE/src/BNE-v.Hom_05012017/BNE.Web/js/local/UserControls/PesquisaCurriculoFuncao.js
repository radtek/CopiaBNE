
function cvFuncaoAnunciarVaga_Validate(sender, args) {

    res = ContratoFuncao.ValidarFuncao(args.Value);
    args.IsValid = (res.error == null && res.value);

    if (!args.IsValid) {
        sender.innerHTML = "Função Inválida";
        return;
    }

    var res = ValidarFuncaoLimitacaoPorContrato(args.Value);
    args.IsValid = (res.error == null && res.value);

    if (!args.IsValid) {
        //sender.innerHTML = "Função indisponível, favor utilizar o tipo de contrato</br>&nbsp;&nbsp;ao especificar uma vaga para Aprendiz ou Estagiário";
        sender.innerHTML = "Função indisponível";
    }
}

function ValidarFuncaoLimitacaoPorContrato(value) {
    if (value == null)
        return new { IsValid: false };

    var limitacaoContrato = replaceDiacritics(value).toString().trim().toLowerCase();
    if (limitacaoContrato == "aprendiz" || limitacaoContrato == "estagiario" || limitacaoContrato == "estagiaria" || limitacaoContrato == "estagio")
        return new { IsValid: true };

    return new { IsValid: false };
}

//alterado por Charan em 26/08/2014 12:37
function cvFuncaoAnunciarVagaLimitacaoPorContrato_Validate(sender, args) {
    var res = AnunciarVaga.ValidarFuncaoLimitacaoPorContrato(args.Value);
    args.IsValid = (res.error == null && res.value);
}

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
    var textBox = $("input:text[name*='txtFuncaoPretendida']");
    if (textBox == null) return;

    var text = textBox.val();
    if (text == null) return;

    text = $.trim(text);
    if (text.length == 0) return;

    text = replaceDiacritics(text);
    text = text.toString().toLowerCase();
    if (text != "aprendiz" && text != "estagio" && text != "estagiario" && text != "estagiaria") {
        return;
    }

    var checkbox = getContratoCheckBox(text);

    if (checkbox == null || checkbox.length == 0 || checkbox.is(':checked')) return;

    checkbox.attr('checked', true);
    
    $('.adicionar_alert').trigger('click');

    $("[id='box_tipo_contrato']").show();
}

function bindEvents() {
    bindFuncaoEvents();
    bindCheckBoxListEvents();

    //var first = "aprendiz";
    //var checkbox = getContratoCheckBox(first);
    var checkbox = null;
    var second = "estágio";
    var secondCheckBox = getContratoCheckBox(second);
    calcularBoxTipoContrato(checkbox, secondCheckBox);
}

function bindCheckBoxListEvents() {
    //var first = "aprendiz";
    //var checkbox = getContratoCheckBox(first);
    var checkbox = null;
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
        $("[id='box_tipo_contrato']").show();
    }
    else {
        $("[id='box_tipo_contrato']").hide();
    }
}
function bindFuncaoEvents() {
    var textBox = $("input:text[name*='txtFuncaoPretendida']");
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
    var table = $('div[id*="container_checkbox"]');
    if (table == null) return null;

    //var label;
    //if (text == "aprendiz")
    //    label = table.find('label:contains("Aprendiz")');
    //else
    //    label = table.find('label:contains("Estágio")');

    var checkbox = table.find('input:checkbox:first');
    //var checkbox = $('[id="' + label.attr('for') + '"]');
    return checkbox;
}