function cvEmailEnviarPara_Validate(sender, args) {

    var emails = employer.controles.recuperarValor('txtEnvioPara').trim();

    var last = emails[emails.length - 1];
    if (last == ";") {
        emails = emails.replaceAt(emails.length - 1, '');
    }
    var mensagem = EnvioCurriculo.ValidarDestinatarios(emails);

    if (mensagem.error == null) {
        if (!mensagem.value) {
            args.IsValid = true;
        }
        else {
            args.IsValid = false;
            employer.controles.setAttr(sender.id, 'innerText', mensagem.value);
        }
    }

}

function itemSelected() {

    var moreEmail = employer.controles.recuperarValor('txtEnvioPara');
    var email = document.getElementById('cphRodape_ucEnvioCurriculo_hdfEmailEnviar').value + ';';
    email = email.replace(',', ';').replace(':', ';');
    var emails = email.split(';');
    var tratado = '';
    for (i = 0; i <= emails.length; i++) {
        if (validateEmail(emails[i])) {
            tratado += emails[i] + ';';
        }

    }
    var pesquisa = tratado + moreEmail;
    document.getElementById('cphRodape_ucEnvioCurriculo_hdfEmailEnviar').value = pesquisa;
    employer.controles.setValor('txtEnvioPara', pesquisa);

}

function validateEmail(emailAddress) {
    var regularExpression = /^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))){2,6}$/i;
    return regularExpression.test(emailAddress);
}


var itemdel = 0;


function ajusteTxt(event) {


    var key = event.keyCode || event.charCode;
    if (key == 32) {//space
        var txt = employer.controles.recuperarValor('txtEnvioPara').trim();
        if (txt[txt.length - 1] == ';') {
            txt = txt.substring(0, txt.length - 1);

        }
        else {
            employer.controles.setValor('txtEnvioPara', employer.controles.recuperarValor('txtEnvioPara').trim() + "; ");
        }

        //if (employer.controles.recuperarValor('txtEnvioPara') == 'Digite o e-mail ou e-mails separados por ;') {
        //    document.getElementById('cphRodape_ucEnvioCurriculo_hdfEmailEnviar').value = '';
        //}
        //else {
        //    document.getElementById('cphRodape_ucEnvioCurriculo_hdfEmailEnviar').value = employer.controles.recuperarValor('txtEnvioPara');
    }

}



function EfetuarRequisicao(id) {
    itemdel = id;
    var dados = "{'id':'" + id + "', 'idufp':'" + document.getElementById('cphRodape_ucEnvioCurriculo_hdfIdUfp').value + "'}";
    $.ajax({
        type: "POST",
        url: "/ajax.aspx/RemoveEmailList",
        data: dados,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (retorno) {

        }
    });

}

$(document).ready(function () {
    String.prototype.replaceAt = function (index, char) {
        return this.substr(0, index) + char + this.substr(index + 1);
    }
    autocompleteEmail()
});

function split(val) {
    return val.split(/;\s*/);
}
function extractLast(term) {
    return split(term).pop();
}

function autocompleteEmail() {
    var email = $("#txtEnvioPara").val();
    var cacheEmail = {};
    var dados = "{'prefixText':'" + email + "', 'count':'10','contextKey':'" + +document.getElementById('cphRodape_ucEnvioCurriculo_hdfIdUfp').value + "' }";
    $("#txtEnvioPara").autocomplete({
        minLength: 1,
        delay: 100,
        source: function (request, response) {
            var term = request.term;

            $.ajax({
                type: "POST",
                url: "/ajax.aspx/ListarSugestEmailUsuarioFilial",
                data: dados,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    cacheEmail[term] = data;
                    for (var i in cacheEmail[term]) {
                        cacheEmail[term][i].value = cacheEmail[term][i];
                        response(cacheEmail[term][i].value);
                    }
                },
                error: function () {
                    console.log(Error);
                }

            });
        },
        select: function (event, ui) {
            if (itemdel > 0) {
                itemdel = 0;
                return false
            }
            var terms = split(this.value);
            terms.pop();
            terms.push(ui.item.value);
            terms.push("");
            this.value = terms.join("; ");
            return false;
        }
    }).autocomplete("instance")._renderItem = function (ul, item) {
        var obj = jQuery.parseJSON(item.value);
        item.value = obj.First
        var retorno = $("<div class=\"ajax_ace_delete\" >")
                .data("item.autocomplete", item.value)
                .append("<span OnClick=\"consoleteste();\"> " + item.value +
                "</span>")
                .appendTo(ul).append("<button class=\"btnExcluir\" onclick=\"EfetuarRequisicao(" + obj.Second + ");\"> X </button>");

        return retorno;
    };


}

