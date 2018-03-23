var to;
var util = {
    mostrarerro: function (mensagem) {
        util.mostrarmensagem(mensagem, 'texto_avisos_padrao');
    },
    mostraraviso: function (mensagem) {
        util.mostrarmensagem(mensagem, 'texto_avisos_erro');
    },
    mostrarmensagem: function (mensagem, csslabel) {
        clearTimeout(to);

        $("#lblAviso").removeClass().addClass(csslabel);
        $("#lblAviso").text(mensagem);

        $(".botao_fechar_aviso").on('click', util.ocultaraviso);
        $(".painel_avisos").css("display", "block");
        $(".painel_avisos").css("opacity", "0.9");

        to = setTimeout("util.ocultaraviso()", 20000);
    },
    ocultaraviso: function () {
        $(".painel_avisos").css("display", "none");
        clearTimeout(to);
    }
};
util.control = {
    findControl: function (controlName, controlType, controlElement) {
        if (controlType == null) {
            return $('' + controlElement + '[id*=' + controlName + ']');
        }
        return $('' + controlElement + ':' + controlType + '[id*=' + controlName + ']');
    },
    findInputText: function (controlName) {
        return util.control.findControl(controlName, 'text', 'input');
    },
    findButton: function (controlName) {
        return util.control.findControl(controlName, null, 'button');
    }
}

$(document).ajaxSend(function (event, jqxhr, settings) {
    if (settings.type.toLowerCase() === 'post') {
        $('.loading').find('.message').css("display", "block").hide();
        $('.loading').show();
        setTimeout(function () {
            $('.loading').find('.message').fadeIn();
        }, 2000);
    }
}).ajaxComplete(function () {
    $('.loading').find('.message').hide();
    $('.loading').hide();
});

$.validator.setDefaults({
    errorPlacement: function (label, element) {
        if (element.is(':radio')) {
            label.insertAfter(element.next('label'));
        } else {
            label.insertAfter(element);
        }
    }
});
