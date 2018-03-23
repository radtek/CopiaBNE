var Parametros = {
    IdadeMin: 0,
    IdadeMax: 0
}

function InicializarParametro(parametros) {
    Parametros.IdadeMin = parametros.IdadeMin;
    Parametros.IdadeMax = parametros.IdadeMax;
}

function cvIdade_ClientValidationFunction(sender, args) {
    //Valor da mensagem de erro _304501
    var erro = '304501 - O valor {0} deve ser igual ou {1} do que o valor {2}.';
    var txtIdadeDe = (+employer.controles.recuperarValorControle('txtIdadeDe'));
    var txtIdadeAte = (+employer.controles.recuperarValorControle('txtIdadeAte'));
    args.IsValid = true;
    if (txtIdadeDe != '' && txtIdadeAte != '') {
        if (txtIdadeAte < txtIdadeDe) {
            erro = erro.replace('{0}', 'Máximo').replace('{1}', 'maior').replace('{2}', 'Mínimo');
            args.IsValid = false;
            employer.controles.setAttr(sender.id, 'innerText', erro);
        }
    }
}

function txtSalarioDe_ValorAlterado(args) {
    var valor = employer.controles.recuperarValor(args.NomeCampoValor);
    if (valor == '')
        return;
    employer.controles.setAttr('txtSalarioAte', 'ValorMinimo', valor);
}

function txtCidade_onChange(sender) {
    AjustarCidade(sender.value);
}

function AjustarCampos() {
    AjustarCidade(employer.controles.recuperarValor('txtCidadePesquisa'));
}

function AjustarCidade(valor) {
    employer.controles.setAttr('txtBairro', 'disabled', valor == '');
    employer.controles.setAttr('txtLogradouro', 'disabled', valor == '');
    employer.util.findControl('lblAvisoCidadeEndereco').css("display", valor != '' ? 'none' : '');
}

function cvCidade_Validate(sender, args) {
    var res = PesquisaCurriculoAvancada.ValidarCidade(args.Value);
    args.IsValid = (res.error == null && res.value);
}

function cvFuncao_Validate(sender, args) {
    var res = PesquisaCurriculoAvancada.ValidarFuncao(args.Value);
    args.IsValid = (res.error == null && res.value);
}

function cvFuncaoFaixaCepAte_Validate(sender, args) {
    //    var erro = '304501 - O valor {0} deve ser igual ou {1} do que o valor {2}.';
    var txtFaixaCepDe = (+(employer.controles.recuperarValorControle('txtFaixaCep').replace('-', '')));
    var txtFaixaCepAte = (+(employer.controles.recuperarValorControle('txtFaixaCepAte').replace('-', '')));
    args.IsValid = true;
    if (typeof (txtFaixaCepDe) != 'undefined' && txtFaixaCepDe != NaN && typeof (txtFaixaCepAte) != 'undefined' && txtFaixaCepAte != NaN) {
        if (txtFaixaCepAte < txtFaixaCepDe) {
            //erro = erro.replace('{0}', 'Máximo').replace('{1}', 'maior').replace('{2}', 'Mínimo');
            args.IsValid = false;
            //employer.controles.setAttr(sender.id, 'innerText', erro);
        }
    }
}

//Completa uma string com zeros à esquerda de acordo com o comprimento informado
function StringPad(number, length) {
    var str = '' + number;

    while (str.length < length) {
        str = '0' + str;
    }
    
    return str;
}

function txtFaixaCep_ValorAlteradoClient(sender) {
    var txtCepDe = employer.controles.recuperarValorControle('txtFaixaCep').replace('-', '');
    var txtCepAte = employer.controles.recuperarValorControle('txtFaixaCepAte').replace('-', '');

    if (txtCepDe.length > 0) {
        if (txtCepAte.length == 0) {
            employer.controles.setValorControle('txtFaixaCepAte', StringPad(Number(txtCepDe) + 500, 8));
        }
    }

    
}

function txtFaixaCepAte_ValorAlteradoClient(sender) {
    var txtCepDe = employer.controles.recuperarValorControle('txtFaixaCep').replace('-', '');
    var txtCepAte = employer.controles.recuperarValorControle('txtFaixaCepAte').replace('-', '');

    if (txtCepAte.length > 0) {
        if (txtCepDe.length == 0) {
            employer.controles.setValorControle('txtFaixaCep', StringPad(Number(txtCepAte) - 500, 8));
        }
    }
}

function onCheckBoxIdiomaClick(chk) {
    var combo = $find(employer.util.findControl('rcbIdioma')[0].id);
    //holds the text of all checked items
    var text = '';
    //holds the values of all checked items
    var values = "";
    //get the collection of all items
    var items = combo.get_items();
    //enumerate all items
    for (var i = 0; i < items.get_count(); i++) {
        var item = items.getItem(i);
        //get the checkbox element of the current item
        var chk = $get(combo.get_id() + "_i" + i + "_chkIdioma");
        if (chk.checked) {
            text += item.get_text() + ",";
            values += item.get_value() + ",";
        }
    }
    //remove the last comma from the string
    text = removerUltimaVirgula(text);
    values = removerUltimaVirgula(values);

    if (text.length > 0) {
        //set the text of the combobox
        combo.set_text(text);
    }
    else {
        //all checkboxes are unchecked 
        //so reset the controls
        combo.set_text("");
    }
}

function onCheckBoxDisponibilidadeClick(chk) {
    var combo = $find(employer.util.findControl('rcbDisponibilidade')[0].id);
    //holds the text of all checked items
    var text = '';
    //holds the values of all checked items
    var values = "";
    //get the collection of all items
    var items = combo.get_items();
    //enumerate all items
    for (var i = 0; i < items.get_count(); i++) {
        var item = items.getItem(i);
        //get the checkbox element of the current item
        var chk = $get(combo.get_id() + "_i" + i + "_chkDisponibilidade");
        if (chk.checked) {
            text += item.get_text() + ",";
            values += item.get_value() + ",";
        }
    }
    //remove the last comma from the string
    text = removerUltimaVirgula(text);
    values = removerUltimaVirgula(values);

    if (text.length > 0) {
        //set the text of the combobox
        combo.set_text(text);
    }
    else {
        //all checkboxes are unchecked 
        //so reset the controls
        combo.set_text("");
    }
}

function removerUltimaVirgula(str) {
    return str.replace(/,$/, "");
}

function InstituicaoGraduacao(sender, args) {
    var hfInstituicaoGraduacao = employer.util.findControl('hfInstituicaoGraduacao');
    hfInstituicaoGraduacao[0].value = args.get_value();

    employer.form.util.autoCompleteClientSelected(sender, args);
}

function InstituicaoOutrosCursos(sender, args) {
    var hfInstituicaoOutrosCursos = employer.util.findControl('hfInstituicaoOutrosCursos');
    hfInstituicaoOutrosCursos[0].value = args.get_value();

    employer.form.util.autoCompleteClientSelected(sender, args);
}

$(document).ready(function () {
    //Ajustando o scroll
    $(window).scroll(function () {
        //$("#divSlowDown").stop().animate({ "top": $(window).scrollTop() + ($(screen)[0].height / 4) + "px" }, { duration: 700, queue: false });
        var vTop = ($(window).height() / 2) - $("#divSlowDown").height() / 2 + $(top.window).scrollTop();

        //Ajustando a altura para não estourar o layout
        if (vTop < 250)
            vTop = 250;
        else if (vTop > 830)
            vTop = 825;

        $("#divSlowDown").stop().animate({ "top": vTop + "px" }, { duration: 700, queue: false });
    });
    
    //Ajustando o background
    $('body').addClass('bg_fundo_empresa');
});

function PesquisaCurriculoAvancadaCidadeOnBlur(sender) {
    var res = PesquisaCurriculoAvancada.RecuperarCidade(sender.value);

    if (res.error == null && res.value)
        employer.controles.setValor(sender.id, res.value);
}