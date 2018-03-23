$(document).ready(function () {
    $(".vaga").each(
        function () {
            var self = $(this);
            var link = self.find(".link");
            self.find(".rrssb-buttons").rrssb({ url: link.attr('href'), title: link.attr('title') });
        }
    );

    if ($("#hdfCidade")[0].value && !getCookie('ckfc')) {
        modal.abrirModal('modalAlertaVaga');
    }
});


function NaoMostarNovamente(name) {
    if (getCookie(name)) {
        deleteCookie(name);
    } else {
        gravarCookie(name, 4);
    }
}

function gravarCookie(name, value) {
    var cookie = name + "=" + escape(value) + ";expires=Thu, 13 Mai 2214 12:00:00 GMT";
    document.cookie = cookie;
    getCookie(name);
}

function deleteCookie(name) {
    if (getCookie(name)) {
        document.cookie = name + "=" +
              "; expires=Thu, 01-Jan-70 00:00:01 GMT";
    }
}

function getCookie(name) {
    var cookies = document.cookie;
    var prefix = name + "=";
    var begin = cookies.indexOf("; " + prefix);

    if (begin == -1) {

        begin = cookies.indexOf(prefix);

        if (begin != 0) {
            return false;
        }

    } else {
        begin += 2;
    }

    var end = cookies.indexOf(";", begin);

    if (end == -1) {
        end = cookies.length;
    }
    //console.log(unescape(cookies.substring(begin + prefix.length, end)));
    //return unescape(cookies.substring(begin + prefix.length, end));
    return true;
}

function SalvarAlerta() {
    var dados = "{'idCidade':'" + $("#hdfIdCidade")[0].value + "', 'idFuncao':'" + $("#hdfIdFuncao")[0].value +
        "', 'cidade':'" + $("#hdfCidade")[0].value + "', 'uf':'" + $("#hdfUf")[0].value + "', 'descfuncao':'" + $("#hdfFuncao")[0].value + "'}";
    $.ajax({
        type: "POST",
        url: "/ResultadoPesquisaVaga/SalvarAlerta",
        data: dados,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (retorno) {
            modal.abrirModal('modalAlertaVaga');
            modal.abrirModal('modalConfirmacaoAlerta');
        }
    });

}
