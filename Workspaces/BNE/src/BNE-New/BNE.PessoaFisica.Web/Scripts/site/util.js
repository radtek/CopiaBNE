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
        //$('.loading').child('.message').hide();
        $('.loading').show();
    }
}).ajaxComplete(function () {
    $('.loading').hide();
    //$('.loading').child('.message').hide();
});



var modal = {
    abrirModal: function (control) {

        var id = control + $.now();
        $('#' + control).attr('id', id);

        //$('.modal:not(#' + control.id + ')').bPopup().close();

        if (latestOpenedModal !== '' && latestOpenedModal !== id) {
            if ($('#' + latestOpenedModal).length > 0) //Verifica se o elemento existe
            {
                $('#' + latestOpenedModal).bPopup().close();
                $('#' + latestOpenedModal).remove();
            }
        }

        latestOpenedModal = id;

        $('#' + id).bPopup({
            modalClose: false, modalColor: '#cde1e8', zIndex: 2, easing: 'swing', onClose: function () {
                $(this).remove();
            }
        });
    }
};

var latestOpenedModal = '';